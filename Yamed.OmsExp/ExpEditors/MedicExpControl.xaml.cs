using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Yamed.Control;
using Yamed.Core;
using Yamed.Entity;
using Yamed.Server;

namespace Yamed.OmsExp.ExpEditors
{
    public class ExpClass
    {
        public object Row { get; set; }
        public D3_SANK_OMS Sank { get; set; }
        public D3_SANK_OMS ReSank { get; set; }
    }


    /// <summary>
    /// Логика взаимодействия для MeeWindow.xaml
    /// </summary>
    public partial class MedicExpControl : UserControl
    {
        private UserControl _panel;
        private bool _isNew;
        //D3_AKT_MEE_TBL _meeSank;
        private decimal? _sump;
        private ObservableCollection<DynamicBaseClass> _sankAutos;
        private List<ExpClass> _slpsList;

        private ElmedDataClassesDataContext _dc1;

        private int? _stype;

        private int? _sid;
        private object _row;
        private int _re;

        public MedicExpControl(int? stype, int? sid = null, object row = null, int re = 0)
        {
            InitializeComponent();
            _stype = stype;
            _sid = sid;
            _row = row;
            _re = re;

            _isNew = sid == null;

            ExpertColumnEdit.DataContext = SprClass.ExpertDbs;


            var videxp = ((IEnumerable<dynamic>)SprClass.TypeExp2).Where(x => ObjHelper.GetAnonymousValue(x, "EXP_TYPE") == _stype && ObjHelper.GetAnonymousValue(x, "EXP_RE") == _re).ToList();
            VidExpEdit.DataContext = videxp;
        }


        public static string Obrezka(string str, int count)
        {
            if (str != null && str.Length > count)
                return str.Substring(0, count);
            return str;
        }



        private void MeeWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

            _sankAutos = SqlReader.Select("Select * from Yamed_ExpSpr_Sank order by Name", SprClass.LocalConnectionString);

            ShablonEdit.DataContext = _sankAutos;

            _slpsList = new List<ExpClass>();

            if (_isNew)
            {
                foreach (var row in DxHelper.LoadedRows)
                {
                    ExpClass expList = new ExpClass();
                    expList.Row = row;
                    expList.Sank = new D3_SANK_OMS()
                    {
                        D3_ZSLID = (int) ObjHelper.GetAnonymousValue(row, "ID"),
                        D3_SCID = (int) ObjHelper.GetAnonymousValue(row, "D3_SCID"),
                        S_TIP = _re == 0 ? (int?) _stype : null,
                        S_CODE = Guid.NewGuid().ToString()
                    };
                    _slpsList.Add(expList);

                    if (_re == 1)
                    {
                        var reSank = 
                            Reader2List.CustomSelect<D3_SANK_OMS>($@"Select * From D3_SANK_OMS where D3_ZSLID={(int)ObjHelper.GetAnonymousValue(row, "ID")} and S_TIP = {_stype}",
                                SprClass.LocalConnectionString);
                        if (reSank.Count > 0)
                            expList.ReSank = reSank[0];
                    }
                }
            }
            else
            {
                ExpClass slupacsank = new ExpClass();

                List<D3_SANK_OMS> sank;
                using (SqlConnection con1 = new SqlConnection(SprClass.LocalConnectionString))
                {
                    SqlCommand cmd1 = new SqlCommand(string.Format(@"Select * From D3_SANK_OMS where ID={0}", _sid), con1);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    con1.Open();

                    cmd1.CommandTimeout = 0;
                    SqlDataReader dr1 = cmd1.ExecuteReader();

                    sank = Reader2List.DataReaderMapToList<D3_SANK_OMS>(dr1);


                    dr1.Close();
                    con1.Close();
                }



                slupacsank.Row = _row;
                slupacsank.Sank = sank.First();
                _slpsList.Add(slupacsank);

                ExpertGridControl.DataContext = _expertList =
                    Reader2List.CustomSelect<D3_SANK_EXPERT_OMS>($@"Select * From D3_SANK_EXPERT_OMS where D3_SANKID={slupacsank.Sank.ID}",
                        SprClass.LocalConnectionString);

            }


            sluchGridControl.DataContext = _slpsList;

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render,
                new Action(delegate ()
                {
                    sluchGridControl.SelectRange(0,0);
                }));

        }


        private List<D3_SANK_EXPERT_OMS> _expertList;
        private List<D3_SANK_EXPERT_OMS> _expert_delList;

        private void ExpertAddItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_expertList == null)
            {
                ExpertGridControl.DataContext = _expertList = new List<D3_SANK_EXPERT_OMS>();
            }
            
            var add = new D3_SANK_EXPERT_OMS() { D3_SANKGID =  ((D3_SANK_OMS)ExpLayGr.DataContext).S_CODE};
            _expertList.Add(add);

            ExpertGridControl.RefreshData();

        }

        private void ExpertDelItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var del = (D3_SANK_EXPERT_OMS)ExpertGridControl.SelectedItem;

            if (del.ID == 0)
            {
                _expertList.Remove(del);
            }
            else
            {
                if (_expert_delList == null) _expert_delList = new List<D3_SANK_EXPERT_OMS>();

                _expert_delList.Add(del);
                _expertList.Remove(del);
            }

            ExpertGridControl.RefreshData();
        }

        private void SluchGridControl_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.NewItem == null) return;
            var sa = ((ExpClass)e.NewItem).Sank;
            ExpLayGr.DataContext = sa;

            ExpertGridControl.FilterString = $"([D3_SANKGID] = '{sa.S_CODE}')";

        }

        private void ShablonEdit_OnPopupOpening(object sender, OpenPopupEventArgs e)
        {
            var sa = ((ExpClass)sluchGridControl.SelectedItem).Sank;
            if (sa.S_TIP2 == null || sa.DATE_ACT == null)
            {
                DXMessageBox.Show("Заполнены не все обязательные поля (дата акта, вид экспертизы)");
                e.Cancel = true;
            }

        }

        private void ShablonEdit_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {


        }

        private void Save_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //foreach (var ex in _expertList)
            //{
            //    _slpsList.Sank.CODE_EXP = ObjHelper.GetAnonymousValue(ExpertBoxEdit.SelectedItem, "KOD").ToString();

            //}

            foreach (var obj in _slpsList.Select(x=>x.Sank).Where(x=>x.MODEL_ID != null))
            {
                if (obj.ID == 0)
                {
                    var id = Reader2List.ObjectInsertCommand("D3_SANK_OMS", obj, "ID", SprClass.LocalConnectionString);
                    obj.ID = (int)id;
                }
                else
                {
                    var upd = Reader2List.CustomUpdateCommand("D3_SANK_OMS", obj, "ID");
                    Reader2List.CustomExecuteQuery(upd, SprClass.LocalConnectionString);
                }
            }

            if (_expertList != null)
                foreach (var obj in _expertList)
                {
                    if (obj.ID == 0)
                    {
                        obj.D3_SANKID = _slpsList.Single(x => x.Sank.S_CODE == obj.D3_SANKGID).Sank.ID;
                        var id = Reader2List.ObjectInsertCommand("D3_SANK_EXPERT_OMS", obj, "ID",
                            SprClass.LocalConnectionString);
                        obj.ID = (int) id;
                    }
                    else
                    {
                        var upd = Reader2List.CustomUpdateCommand("D3_SANK_EXPERT_OMS", obj, "ID");
                        Reader2List.CustomExecuteQuery(upd, SprClass.LocalConnectionString);
                    }
                }

            var scs = _slpsList.Select(x => ObjHelper.GetAnonymousValue(x.Row, "D3_SCID")).Distinct();
            if (_re == 0)
            {
                foreach (var sc in scs)
                {
                    Reader2List.CustomExecuteQuery($@"EXEC p_oms_calc_sank {sc}; EXEC p_oms_calc_schet {sc};", SprClass.LocalConnectionString);
                }
            }
            else
            {
                foreach (var sc in scs)
                {
                    Reader2List.CustomExecuteQuery($@"EXEC p_oms_calc_sank_ {sc}; EXEC p_oms_calc_schet {sc};", SprClass.LocalConnectionString);
                }
            }

            ((DXWindow)this.Parent).Close();


        }

        private void GridViewBase_OnCellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            (sender as TableView).PostEditor();
        }



        void CalcSank(ExpClass ex)
        {
            var sh = (DynamicBaseClass) ShablonEdit.SelectedItem;
            if (sh == null) return;

            var pe1 = (int) sh.GetValue("Penalty_1");
            var osn = (string) sh.GetValue("Osn");
            ex.Sank.S_OSN = osn;

            var pe2 = (decimal?) SqlReader.Select(
                $@"EXEC	[dbo].[p_oms_calc_medexp]
            		@zslid = {ex.Sank.D3_ZSLID},
                    @model = {ex.Sank.MODEL_ID},
            		@date = '{ex.Sank.DATE_ACT.Value.ToString("yyyyMMdd")}'",
                SprClass.LocalConnectionString).FirstOrDefault()?.GetValue("s_sum2");

            decimal? sump, sum_np;

            if (_re == 0)
            {
                if (ObjHelper.GetAnonymousValue(ex.Row, "SUMP") == null ||
                    (decimal) ObjHelper.GetAnonymousValue(ex.Row, "SUMP") == 0 || !_isNew)
                    sump = (decimal) ObjHelper.GetAnonymousValue(ex.Row, "SUMV");
                else
                    sump = (decimal) ObjHelper.GetAnonymousValue(ex.Row, "SUMP");

                sum_np = Math.Round((decimal) sump * pe1 / 100, 2,
                    MidpointRounding.AwayFromZero);

            }
            else if (_re == 1 && pe1 == -100)
            {
                sum_np = 0;
                pe2 = 0;
                ex.Sank.S_OSN = ex.ReSank.S_OSN;
            }
            else
            {
                sump = (decimal) ObjHelper.GetAnonymousValue(ex.Row, "SUMV");
                var rsum = sump * pe1 / 100;
                if (ex.ReSank.S_SUM > rsum)
                    sum_np = -(ex.ReSank.S_SUM - rsum);
                else sum_np = rsum - ex.ReSank.S_SUM;
            }

            sluchGridControl.DataController.RefreshData();

            ex.Sank.S_SUM = sum_np;
            ex.Sank.S_SUM2 = pe2;
        }

        private void CalcButtonInfo_OnClick(object sender, RoutedEventArgs e)
        {

            CalcSank((ExpClass)sluchGridControl.SelectedItem);

        }

        private void PacketItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var sa = ((ExpClass)sluchGridControl.SelectedItem).Sank;

            foreach (var ex in sluchGridControl.SelectedItems.OfType<ExpClass>())
            {
                ex.Sank.S_TIP2 = sa.S_TIP2;
                ex.Sank.S_DATE = sa.S_DATE;
                ex.Sank.ExpOrder = sa.ExpOrder;
                ex.Sank.DATE_ACT = sa.DATE_ACT;
                ex.Sank.NUM_ACT = sa.NUM_ACT;
                ex.Sank.MODEL_ID = sa.MODEL_ID;

                CalcSank(ex);

                ex.Sank.Z_INFO = sa.Z_INFO;
                ex.Sank.Z_DS1 = sa.Z_DS1;
                ex.Sank.Z_DS2 = sa.Z_DS2;
                ex.Sank.Z_DS3 = sa.Z_DS3;
                ex.Sank.Z_OKAZ_MP = sa.Z_OKAZ_MP;
                ex.Sank.Z_NEG_POS = sa.Z_NEG_POS;
                ex.Sank.Z_PREEM = sa.Z_PREEM;
                ex.Sank.Z_OBOSN_OB = sa.Z_OBOSN_OB;
                ex.Sank.Z_PROF_GOSP = sa.Z_PROF_GOSP;
                ex.Sank.S_ZAKL = sa.S_ZAKL;

                if (_expertList != null)
                    foreach (var expert in _expertList.Where(x => x.D3_SANKGID == sa.S_CODE).ToList())
                    {
                        D3_SANK_EXPERT_OMS nexpert = new D3_SANK_EXPERT_OMS()
                        {
                            D3_SANKGID = ex.Sank.S_CODE,
                            ExpertCode = expert.ExpertCode
                        };
                        if (!_expertList.Any(x =>
                            x.D3_SANKGID == nexpert.D3_SANKGID && x.ExpertCode == nexpert.ExpertCode))
                            _expertList.Add(nexpert);
                    }

            }

        }
    }
}
