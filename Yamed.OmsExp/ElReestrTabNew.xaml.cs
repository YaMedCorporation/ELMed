using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting.Native;
using Yamed.Control;
using Yamed.Core;
using Yamed.Entity;
using Yamed.OmsExp.ExpEditors;
using Yamed.Server;

namespace Yamed.OmsExp
{
    /// <summary>
    /// Логика взаимодействия для ElReestr.xaml
    /// </summary>
    public partial class ElReestrTabNew : UserControl
    {
        public readonly LinqInstantFeedbackDataSource _linqInstantFeedbackDataSource;
        private readonly YamedDataClassesDataContext _ElmedDataClassesDataContext;
        //public List<SQLTables.SluPacClass> _zsls;
        public List<int> Scids;

        public ElReestrTabNew()
        {
            InitializeComponent();

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            using (var dc = new YamedDataClassesDataContext(SprClass.LocalConnectionString))
            {
                Yamed_Users first = dc.Yamed_Users.Single(x => x.ID == SprClass.userId);
                writer.Write(first.LayRTable);
            }
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);


            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    if (stream.Length > 0)
                        gridControl1.RestoreLayoutFromStream(stream);

                    stream.Close();
                    writer.Close();

                    gridControl1.FilterString = "";
                    gridControl1.ClearSorting();
                }));



            ZUslOkEdit.DataContext = SprClass.conditionHelp;
            SmoEdit.DataContext = SprClass.smo;
            VidPomEdit.DataContext = SprClass.typeHelp;
            ForPomEdit.DataContext = SprClass.ForPomList;
            MoEdit.DataContext = SprClass.LpuList;
            NprMoEdit.DataContext = SprClass.LpuList;
            RsltEdit.DataContext = SprClass.helpResult;
            IshodEdit.DataContext = SprClass.helpExit;
            OsSlRegEdit.DataContext = SprClass.OsobSluchDbs;
            IdspEdit.DataContext = SprClass.tarifUsl;
            VozrEdit.DataContext = SprClass.VozrList;

            UslOkEdit.DataContext = SprClass.conditionHelp;
            ProfilEdit.DataContext = SprClass.profile;
            VidHmpEdit.DataContext = SprClass.VidVmpList;
            MetodHmpEdit.DataContext = SprClass.MetodVmpList;
            //KsgEdit.DataContext = SprClass.KsgGroups;
            Ds1Edit.DataContext = SprClass.mkbSearching;
            PrvsEdit.DataContext = SprClass.SpecAllList;
            OplataEdit.DataContext = SprClass.Spr79_F005;
            ExpTypeEdit.DataContext = SprClass.MeeTypeDbs;
            UserEdit.DataContext = SprClass.YamedUsers;
            PCelEdit.DataContext = SprClass.SprPCelList;
            DoctEdit.DataContext = SprClass.MedicalEmployeeList;

            Lpu1Edit.DataContext = SprClass.Podr;
            PodrEdit.DataContext = SprClass.OtdelDbs;

            _ElmedDataClassesDataContext = new YamedDataClassesDataContext()
            {
                ObjectTrackingEnabled = false,
                CommandTimeout = 0,
                Connection = { ConnectionString = SprClass.LocalConnectionString }
            };

            _linqInstantFeedbackDataSource = new LinqInstantFeedbackDataSource()
            {
                AreSourceRowsThreadSafe = false, KeyExpression = "KeyID"
            };
            gridControl1.DataContext = _linqInstantFeedbackDataSource;

        }

        private IQueryable _pQueryable;
        public void BindDataZsl(List<int> scids)
        {
            HideSlColumn();

            _pQueryable = from zsl in _ElmedDataClassesDataContext.D3_ZSL_OMS
                join pa in _ElmedDataClassesDataContext.D3_PACIENT_OMS on zsl.D3_PID equals pa.ID 
                join sc in _ElmedDataClassesDataContext.D3_SCHET_OMS on zsl.D3_SCID equals sc.ID
                //join sl in _ElmedDataClassesDataContext.D3_SL_OMS on zsl.ID equals sl.D3_ZSLID
                where (scids.Contains(zsl.D3_SCID) || !scids.Any())
                select new
                {
                    sc.YEAR,
                    sc.MONTH,

                    KeyID = zsl.ID,
                    zsl.D3_SCID,
                    zsl.ID,
                    zsl.ZSL_ID,
                    zsl.VIDPOM,
                    zsl.NPR_MO,
                    zsl.LPU,
                    zsl.FOR_POM,
                    zsl.DATE_Z_1,
                    zsl.DATE_Z_2,
                    zsl.RSLT,
                    zsl.ISHOD,
                    zsl.OS_SLUCH,
                    zsl.OS_SLUCH_REGION,
                    zsl.IDSP,
                    zsl.SUMV,
                    zsl.OPLATA,
                    zsl.SUMP,
                    zsl.SANK_IT,
                    zsl.MEK_COMENT,
                    zsl.OSP_COMENT,
                    Z_USL_OK = zsl.USL_OK,
                    Z_P_CEL = zsl.P_CEL,
                    zsl.MEK_COUNT,
                    zsl.MEE_COUNT,
                    zsl.EKMP_COUNT,
                    zsl.EXP_COMENT,
                    zsl.EXP_TYPE,
                    zsl.EXP_DATE,
                    zsl.ReqID,
                    zsl.USER_COMENT,
                    zsl.USERID,

                    pa.FAM,
                    pa.IM,
                    pa.OT,
                    pa.W,
                    pa.DR,
                    pa.FAM_P,
                    pa.IM_P,
                    pa.OT_P,
                    pa.W_P,
                    pa.DR_P,
                    pa.MR,
                    pa.DOCTYPE,
                    pa.DOCSER,
                    pa.DOCNUM,
                    pa.SNILS,
                    pa.OKATOG,
                    pa.OKATOP,
                    pa.COMENTP,
                    pa.VPOLIS,
                    pa.SPOLIS,
                    pa.NPOLIS,
                    pa.SMO,
                    pa.SMO_OGRN,
                    pa.SMO_OK,
                    pa.SMO_NAM,
                    pa.NOVOR,
                    zsl.VOZR,
                };

            _linqInstantFeedbackDataSource.QueryableSource = _pQueryable;
        }

        void ShowSlColumn()
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate()
                {
                    gridControl1.Columns.Where(x => x.Name.StartsWith("Column__SL__")).ForEach(x =>
                    {
                        x.Width = (GridColumnWidth) x.Tag;
                    });
                }));
        }

        void HideSlColumn()
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    gridControl1.Columns.Where(x => x.Name.StartsWith("Column__SL__")).ForEach(x =>
                    {
                        x.Tag = x.Width;
                        x.Width = 0;
                    });
                }));
        }


        void BindDataSl(List<int> scids)
        {
            ShowSlColumn();

            _pQueryable = from zsl in _ElmedDataClassesDataContext.D3_ZSL_OMS
                          join pa in _ElmedDataClassesDataContext.D3_PACIENT_OMS on zsl.D3_PID equals pa.ID
                          join sc in _ElmedDataClassesDataContext.D3_SCHET_OMS on zsl.D3_SCID equals sc.ID
                          join sl in _ElmedDataClassesDataContext.D3_SL_OMS on zsl.ID equals sl.D3_ZSLID
                          where (scids.Contains(zsl.D3_SCID) || !scids.Any())
                          select new
                          {
                              sc.YEAR,
                              sc.MONTH,

                              KeyID = sl.ID,
                              zsl.D3_SCID,
                              zsl.ID,
                              zsl.ZSL_ID,
                              zsl.VIDPOM,
                              zsl.NPR_MO,
                              zsl.LPU,
                              zsl.FOR_POM,
                              zsl.DATE_Z_1,
                              zsl.DATE_Z_2,
                              zsl.RSLT,
                              zsl.ISHOD,
                              zsl.OS_SLUCH,
                              zsl.OS_SLUCH_REGION,
                              zsl.IDSP,
                              zsl.SUMV,
                              zsl.OPLATA,
                              zsl.SUMP,
                              zsl.SANK_IT,
                              zsl.MEK_COMENT,
                              zsl.OSP_COMENT,
                              Z_USL_OK = zsl.USL_OK,
                              zsl.MEK_COUNT,
                              zsl.MEE_COUNT,
                              zsl.EKMP_COUNT,
                              zsl.EXP_COMENT,
                              zsl.EXP_TYPE,
                              zsl.EXP_DATE,
                              zsl.ReqID,
                              zsl.USER_COMENT,
                              zsl.USERID,


                              ///////////////////////////////////
                              sl.USL_OK,
                              sl.VID_HMP,
                              sl.METOD_HMP,
                              sl.LPU_1,
                              sl.PODR,
                              sl.PROFIL,
                              sl.DET,
                              sl.P_CEL,
                              sl.TAL_NUM,
                              sl.TAL_D,
                              sl.TAL_P,
                              sl.NHISTORY,
                              sl.P_PER,
                              sl.DATE_1,
                              sl.DATE_2,
                              sl.KD,
                              sl.DS0,
                              sl.DS1,
                              sl.DS1_PR,
                              sl.DN,
                              sl.CODE_MES1,
                              sl.CODE_MES2,
                              sl.KSG_DKK,
                              sl.N_KSG,
                              sl.KSG_PG,
                              sl.SL_K,
                              sl.IT_SL,
                              sl.REAB,
                              sl.PRVS,
                              sl.VERS_SPEC,
                              sl.PRVS_VERS,
                              sl.IDDOKT,
                              sl.ED_COL,
                              sl.TARIF,
                              sl.SUM_M,
                              sl.COMENTSL,
                              ////////////////////////////////

                              pa.FAM,
                              pa.IM,
                              pa.OT,
                              pa.W,
                              pa.DR,
                              pa.FAM_P,
                              pa.IM_P,
                              pa.OT_P,
                              pa.W_P,
                              pa.DR_P,
                              pa.MR,
                              pa.DOCTYPE,
                              pa.DOCSER,
                              pa.DOCNUM,
                              pa.SNILS,
                              pa.OKATOG,
                              pa.OKATOP,
                              pa.COMENTP,
                              pa.VPOLIS,
                              pa.SPOLIS,
                              pa.NPOLIS,
                              pa.SMO,
                              pa.SMO_OGRN,
                              pa.SMO_OK,
                              pa.SMO_NAM,
                              pa.NOVOR,
                              zsl.VOZR,
                          };
            _linqInstantFeedbackDataSource.QueryableSource = _pQueryable;
        }

        public void BindDataPacient(string fam, string im, string ot, DateTime? dr, string npol = null)
        {
            //ShowSlColumn();
            SlCheckEdit.IsEnabled = false;

            _pQueryable = from zsl in _ElmedDataClassesDataContext.D3_ZSL_OMS
                    join pa in _ElmedDataClassesDataContext.D3_PACIENT_OMS on zsl.D3_PID equals pa.ID
                    join sc in _ElmedDataClassesDataContext.D3_SCHET_OMS on zsl.D3_SCID equals sc.ID
                    join sl in _ElmedDataClassesDataContext.D3_SL_OMS on zsl.ID equals sl.D3_ZSLID
                    where (pa.FAM.StartsWith(fam) || fam == null) && (pa.IM.StartsWith(im) || im == null) && (pa.OT.StartsWith(ot) || ot == null) && (pa.DR == dr || dr ==null) && (pa.NPOLIS == npol || npol == null)
                          select new
                    {
                        sc.YEAR,
                        sc.MONTH,

                        KeyID = sl.ID,

                        zsl.D3_SCID,
                        zsl.ID,
                        zsl.ZSL_ID,
                        zsl.VIDPOM,
                        zsl.NPR_MO,
                        zsl.LPU,
                        zsl.FOR_POM,
                        zsl.DATE_Z_1,
                        zsl.DATE_Z_2,
                        zsl.RSLT,
                        zsl.ISHOD,
                        zsl.OS_SLUCH,
                        zsl.OS_SLUCH_REGION,
                        zsl.IDSP,
                        zsl.SUMV,
                        zsl.OPLATA,
                        zsl.SUMP,
                        zsl.SANK_IT,
                        zsl.MEK_COMENT,
                        zsl.OSP_COMENT,
                        //zsl.USL_OK,
                        //zsl.P_CEL,
                        zsl.MEK_COUNT,
                        zsl.MEE_COUNT,
                        zsl.EKMP_COUNT,
                        zsl.EXP_COMENT,
                        zsl.EXP_TYPE,
                        zsl.EXP_DATE,
                        zsl.ReqID,
                        zsl.USER_COMENT,
                        zsl.USERID,

                        sl.USL_OK,
                        sl.VID_HMP,
                        sl.METOD_HMP,
                        sl.LPU_1,
                        sl.PODR,
                        sl.PROFIL,
                        sl.DET,
                        sl.P_CEL,
                        sl.TAL_NUM,
                        sl.TAL_D,
                        sl.TAL_P,
                        sl.NHISTORY,
                        sl.P_PER,
                        sl.DATE_1,
                        sl.DATE_2,
                        sl.KD,
                        sl.DS0,
                        sl.DS1,
                        sl.DS1_PR,
                        sl.DN,
                        sl.CODE_MES1,
                        sl.CODE_MES2,
                        sl.KSG_DKK,
                        sl.N_KSG,
                        sl.KSG_PG,
                        sl.SL_K,
                        sl.IT_SL,
                        sl.REAB,
                        sl.PRVS,
                        sl.VERS_SPEC,
                        sl.PRVS_VERS,
                        sl.IDDOKT,
                        sl.ED_COL,
                        sl.TARIF,
                        sl.SUM_M,
                        sl.COMENTSL,


                        ////////////////////////////////
                        pa.FAM,
                        pa.IM,
                        pa.OT,
                        pa.W,
                        pa.DR,
                        pa.FAM_P,
                        pa.IM_P,
                        pa.OT_P,
                        pa.W_P,
                        pa.DR_P,
                        pa.MR,
                        pa.DOCTYPE,
                        pa.DOCSER,
                        pa.DOCNUM,
                        pa.SNILS,
                        pa.OKATOG,
                        pa.OKATOP,
                        pa.COMENTP,
                        pa.VPOLIS,
                        pa.SPOLIS,
                        pa.NPOLIS,
                        pa.SMO,
                        pa.SMO_OGRN,
                        pa.SMO_OK,
                        pa.SMO_NAM,
                        pa.NOVOR,
                        zsl.VOZR,
                    };
            _linqInstantFeedbackDataSource.QueryableSource = _pQueryable;

        }

        public void BindDataExpResult(List<int> ids)
        {
            //ShowSlColumn();
            SlCheckEdit.IsEnabled = false;

            _pQueryable = from zsl in _ElmedDataClassesDataContext.D3_ZSL_OMS
                          join pa in _ElmedDataClassesDataContext.D3_PACIENT_OMS on zsl.D3_PID equals pa.ID
                          join sc in _ElmedDataClassesDataContext.D3_SCHET_OMS on zsl.D3_SCID equals sc.ID
                          join sl in _ElmedDataClassesDataContext.D3_SL_OMS on zsl.ID equals sl.D3_ZSLID
                          where ids.Contains(zsl.ID)
                          select new
                          {
                              sc.YEAR,
                              sc.MONTH,

                              KeyID = sl.ID,

                              zsl.D3_SCID,
                              zsl.ID,
                              zsl.ZSL_ID,
                              zsl.VIDPOM,
                              zsl.NPR_MO,
                              zsl.LPU,
                              zsl.FOR_POM,
                              zsl.DATE_Z_1,
                              zsl.DATE_Z_2,
                              zsl.RSLT,
                              zsl.ISHOD,
                              zsl.OS_SLUCH,
                              zsl.OS_SLUCH_REGION,
                              zsl.IDSP,
                              zsl.SUMV,
                              zsl.OPLATA,
                              zsl.SUMP,
                              zsl.SANK_IT,
                              zsl.MEK_COMENT,
                              zsl.OSP_COMENT,
                              //zsl.USL_OK,
                              //zsl.P_CEL,
                              zsl.MEK_COUNT,
                              zsl.MEE_COUNT,
                              zsl.EKMP_COUNT,
                              zsl.EXP_COMENT,
                              zsl.EXP_TYPE,
                              zsl.EXP_DATE,
                              zsl.ReqID,
                              zsl.USER_COMENT,
                              zsl.USERID,

                              sl.USL_OK,
                              sl.VID_HMP,
                              sl.METOD_HMP,
                              sl.LPU_1,
                              sl.PODR,
                              sl.PROFIL,
                              sl.DET,
                              sl.P_CEL,
                              sl.TAL_NUM,
                              sl.TAL_D,
                              sl.TAL_P,
                              sl.NHISTORY,
                              sl.P_PER,
                              sl.DATE_1,
                              sl.DATE_2,
                              sl.KD,
                              sl.DS0,
                              sl.DS1,
                              sl.DS1_PR,
                              sl.DN,
                              sl.CODE_MES1,
                              sl.CODE_MES2,
                              sl.KSG_DKK,
                              sl.N_KSG,
                              sl.KSG_PG,
                              sl.SL_K,
                              sl.IT_SL,
                              sl.REAB,
                              sl.PRVS,
                              sl.VERS_SPEC,
                              sl.PRVS_VERS,
                              sl.IDDOKT,
                              sl.ED_COL,
                              sl.TARIF,
                              sl.SUM_M,
                              sl.COMENTSL,


                              ////////////////////////////////
                              pa.FAM,
                              pa.IM,
                              pa.OT,
                              pa.W,
                              pa.DR,
                              pa.FAM_P,
                              pa.IM_P,
                              pa.OT_P,
                              pa.W_P,
                              pa.DR_P,
                              pa.MR,
                              pa.DOCTYPE,
                              pa.DOCSER,
                              pa.DOCNUM,
                              pa.SNILS,
                              pa.OKATOG,
                              pa.OKATOP,
                              pa.COMENTP,
                              pa.VPOLIS,
                              pa.SPOLIS,
                              pa.NPOLIS,
                              pa.SMO,
                              pa.SMO_OGRN,
                              pa.SMO_OK,
                              pa.SMO_NAM,
                              pa.NOVOR,
                              zsl.VOZR,
                          };
            _linqInstantFeedbackDataSource.QueryableSource = _pQueryable;

        }


        public void BindDataSearch(string lpu, int? m1, int? m2, int? y1, int? y2,
            int? profil, string ds, string pcel, int? uslOk, int? osSl)
        {
            //ShowSlColumn();
            SlCheckEdit.IsEnabled = false;

            _pQueryable = from zsl in _ElmedDataClassesDataContext.D3_ZSL_OMS
                          join pa in _ElmedDataClassesDataContext.D3_PACIENT_OMS on zsl.D3_PID equals pa.ID
                          join sc in _ElmedDataClassesDataContext.D3_SCHET_OMS on zsl.D3_SCID equals sc.ID
                          join sl in _ElmedDataClassesDataContext.D3_SL_OMS on zsl.ID equals sl.D3_ZSLID
                          where ((sc.MONTH >= m1 && sc.MONTH <= m2) || m1 == null || m2 == null) && ((sc.YEAR >= y1 && sc.YEAR <= y2) || y1 == null || y2 == null)
                          && (zsl.LPU == lpu || lpu == null) && (sl.PROFIL == profil || profil == null) && (sl.DS1.StartsWith(ds) || ds == null) && (sl.P_CEL == pcel || pcel == null)
                          && (sl.USL_OK == uslOk || uslOk == null) && (zsl.OS_SLUCH_REGION == osSl || osSl== null)
                          select new
                          {
                              sc.YEAR,
                              sc.MONTH,

                              KeyID = sl.ID,

                              zsl.D3_SCID,
                              zsl.ID,
                              zsl.ZSL_ID,
                              zsl.VIDPOM,
                              zsl.NPR_MO,
                              zsl.LPU,
                              zsl.FOR_POM,
                              zsl.DATE_Z_1,
                              zsl.DATE_Z_2,
                              zsl.RSLT,
                              zsl.ISHOD,
                              zsl.OS_SLUCH,
                              zsl.OS_SLUCH_REGION,
                              zsl.IDSP,
                              zsl.SUMV,
                              zsl.OPLATA,
                              zsl.SUMP,
                              zsl.SANK_IT,
                              zsl.MEK_COMENT,
                              zsl.OSP_COMENT,
                              //zsl.USL_OK,
                              //zsl.P_CEL,
                              zsl.MEK_COUNT,
                              zsl.MEE_COUNT,
                              zsl.EKMP_COUNT,
                              zsl.EXP_COMENT,
                              zsl.EXP_TYPE,
                              zsl.EXP_DATE,
                              zsl.ReqID,
                              zsl.USER_COMENT,
                              zsl.USERID,

                              sl.USL_OK,
                              sl.VID_HMP,
                              sl.METOD_HMP,
                              sl.LPU_1,
                              sl.PODR,
                              sl.PROFIL,
                              sl.DET,
                              sl.P_CEL,
                              sl.TAL_NUM,
                              sl.TAL_D,
                              sl.TAL_P,
                              sl.NHISTORY,
                              sl.P_PER,
                              sl.DATE_1,
                              sl.DATE_2,
                              sl.KD,
                              sl.DS0,
                              sl.DS1,
                              sl.DS1_PR,
                              sl.DN,
                              sl.CODE_MES1,
                              sl.CODE_MES2,
                              sl.KSG_DKK,
                              sl.N_KSG,
                              sl.KSG_PG,
                              sl.SL_K,
                              sl.IT_SL,
                              sl.REAB,
                              sl.PRVS,
                              sl.VERS_SPEC,
                              sl.PRVS_VERS,
                              sl.IDDOKT,
                              sl.ED_COL,
                              sl.TARIF,
                              sl.SUM_M,
                              sl.COMENTSL,


                              ////////////////////////////////
                              pa.FAM,
                              pa.IM,
                              pa.OT,
                              pa.W,
                              pa.DR,
                              pa.FAM_P,
                              pa.IM_P,
                              pa.OT_P,
                              pa.W_P,
                              pa.DR_P,
                              pa.MR,
                              pa.DOCTYPE,
                              pa.DOCSER,
                              pa.DOCNUM,
                              pa.SNILS,
                              pa.OKATOG,
                              pa.OKATOP,
                              pa.COMENTP,
                              pa.VPOLIS,
                              pa.SPOLIS,
                              pa.NPOLIS,
                              pa.SMO,
                              pa.SMO_OGRN,
                              pa.SMO_OK,
                              pa.SMO_NAM,
                              pa.NOVOR,
                              zsl.VOZR,
                          };
            _linqInstantFeedbackDataSource.QueryableSource = _pQueryable;

        }


        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(delegate()
                    {
                        if (!(bool) SlCheckEdit.EditValue)
                        {
                            gridControl1.Columns.Where(x => x.Name.StartsWith("Column__SL__")).ForEach(x =>
                            {
                                if (x.Tag != null)
                                    x.Width = (GridColumnWidth) x.Tag;
                            });
                        }

                        Stream mStream = new MemoryStream();
                        gridControl1.SaveLayoutToStream(mStream);
                        mStream.Seek(0, SeekOrigin.Begin);
                        StreamReader reader = new StreamReader(mStream);

                        using (var dc = new YamedDataClassesDataContext(SprClass.LocalConnectionString))
                        {
                            Yamed_Users first = dc.Yamed_Users.Single(x => x.ID == SprClass.userId);
                            first.LayRTable = reader.ReadToEnd();
                            dc.SubmitChanges();
                        }

                        mStream.Close();
                        reader.Close();
                    }));




            //_zsls = null;

            _ElmedDataClassesDataContext.Dispose();
            _linqInstantFeedbackDataSource.Dispose();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {



        }


        private int _rowHandle;
        private void view_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed) return;
            GridColumn col = ((TableView)sender).GetColumnByMouseEventArgs(e) as GridColumn;
            if (col == null) return;
            int rowHandle = ((TableView)sender).GetRowHandleByMouseEventArgs(e);
            if (rowHandle == _rowHandle) return;

            if (rowHandle >= 0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    gridControl1.SelectItem(rowHandle);
                }));
            _rowHandle = rowHandle;
            }
        }

        private void View_OnCellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            (sender as TableView).PostEditor();
        }

        private void MtrCheck_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if ((bool?) e.NewValue == true)
            {
                if (string.IsNullOrEmpty(gridControl1.FilterString))
                {
                    gridControl1.FilterString = $"([SMO] Is Null Or [SMO] Not Like '46%')";

                }
                else
                {
                    gridControl1.FilterString += $"And ([SMO] Is Null Or [SMO] Not Like '46%')";

                }
            }
            else
            {
                gridControl1.FilterString =
                    gridControl1.FilterString.
                        Replace($" And ([SMO] Is Null Or [SMO] Not Like '46%')", "").
                        Replace($"([SMO] Is Null Or [SMO] Not Like '46%') And ", "").

                        Replace($"([SMO] Is Null Or [SMO] Not Like '46%')", "").
                        Replace($"[SMO] Is Null Or [SMO] Not Like '46%'", "");

            }
        }

        private void BaseEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if ((bool) e.NewValue)
            {
                BindDataSl(Scids);
            }
            else
            {
                BindDataZsl(Scids);
            }

        }

        private void PacientRowItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var row = DxHelper.GetSelectedGridRow(gridControl1);
            var fam = (string)ObjHelper.GetAnonymousValue(row, "FAM");
            var im = (string)ObjHelper.GetAnonymousValue(row, "IM");
            var ot = (string)ObjHelper.GetAnonymousValue(row, "OT");
            var dr = (DateTime)ObjHelper.GetAnonymousValue(row, "DR");

            //string cmd = SprClass.schetQuery +
            //             $" where FAM = '{row.FAM}' and IM = '{row.IM}' and OT= '{row.OT}' and DR = '{row.DR?.Date.ToString("yyyyMMdd")}'";

            var rc = new ReestrControl();
            rc.ElReestrTabNew11.BindDataPacient(fam, im, ot, dr);
            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Реестр счета",
                MyControl = rc,
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }

        public int GetSelectedRowId()
        {
            return (int)ObjHelper.GetAnonymousValue(DxHelper.GetSelectedGridRow(gridControl1), "ID");
        }

        private void UserComentItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref gridControl1);
            bool isLoaded = false;
            gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (gridControl1.IsAsyncOperationInProgress == false)
                        {
                            isLoaded = true;
                        }
                    });
                    if (isLoaded) break;
                    Thread.Sleep(200);
                }

            }).ContinueWith(lr =>
            {
                if (DxHelper.LoadedRows.Count > 0)
                {
                    var window = new DXWindow
                    {
                        ShowIcon = false,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        Content = new ReqControl(1),
                        Title = "Коментарий пользователя",
                        SizeToContent = SizeToContent.Height,
                        Width = 300
                    };
                    window.ShowDialog();
                }
                gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void FlkGroup()
        {
            gridControl1.ClearGrouping();
            gridControl1.GroupBy("EXP_COMENT");

        }
    }
}
