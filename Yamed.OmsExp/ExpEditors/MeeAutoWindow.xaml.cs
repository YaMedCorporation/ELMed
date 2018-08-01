using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Bars;
using Yamed.Control;
using Yamed.Core;
using Yamed.Entity;
using Yamed.OmsExp.MekEditor;
using Yamed.Server;

namespace Yamed.OmsExp.ExpEditors
{
    /// <summary>
    /// Логика взаимодействия для ReportsWindow.xaml
    /// </summary>
    public partial class MeeAutoWindow : UserControl
    {
        private object[] _rows;
        public MeeAutoWindow(object[] rows)
        {
            InitializeComponent();
            _rows = rows;

            //startBox1.Items.AddRange(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            //startBox2.Items.AddRange(new object[] { 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020 });
            //endBox1.Items.AddRange(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            //endBox2.Items.AddRange(new object[] { 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020 });

            MeeList.DataContext = SqlReader.Select("Select * From Yamed_ExpSpr_ExpAlg where ExpType = 2", SprClass.LocalConnectionString);

        }


        private void BarButtonItemMO_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ////var ef = new[] {"Е", "Ё"};

            //sb = new StringBuilder();
            //sb2 = new StringBuilder();
            ////int schet_id = ((D3_SCHET_OMS)_elReestrWindow.gridControl.SelectedItem).ID;

            //TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext(); //get UI thread context 

            //LogBox.Clear();
            //LogBox.Text += "Получение списка реестров..." + Environment.NewLine;

            //List<D3_SCHET_OMS> rl = new List<D3_SCHET_OMS>();
            //using (var dc = new ElmedDataClassesDataContext(SprClass.LocalConnectionString))
            //{
            //    DataLoadOptions options = new DataLoadOptions();
            //    options.LoadWith<D3_SCHET_OMS>(x => x.F003);
            //    dc.LoadOptions = options;
            //    rl =
            //        dc.D3_SCHET_OMS.Where(
            //            x =>
            //                x.YEAR >= (int)editItemDateEdit12.EditValue &&
            //                x.YEAR <= (int)editItemDateEdit21.EditValue &&
            //                x.MONTH >= (int)editItemDateEdit1.EditValue &&
            //                x.MONTH <= (int)editItemDateEdit2.EditValue).Select(x => x).ToList();
            //}

            //var mekTask = Task.Factory.StartNew(() =>
            //{

            //    foreach (D3_SCHET_OMS schet in rl)
            //    {
            //        D3_SCHET_OMS rSchet = schet;
            //        Dispatcher.BeginInvoke((Action)delegate()
            //        {
            //            LogBox.Text += "Загрузка реестра " + rSchet.F003.NameWithID + Environment.NewLine;
            //        });

            //        try
            //        {
            //            List<PolCheckLPU> lpList = new List<PolCheckLPU>();
            //            using (SqlConnection con1 = new SqlConnection(Properties.Settings.Default.ElmedConnectionString))
            //            {
            //                SqlCommand cmd1 = new SqlCommand(string.Format(@"
            //            select pa.fam, pa.im, pa.ot, pa.dr, pa.npolis, pa.SMO, sl.DATE_1, sl.DATE_2, sl.id, sl.sumv, sl.USL_OK from sluch sl
            //            join pacient pa on sl.PID = pa.ID
            //            where sl.SCHET_ID = {0}", rSchet.ID), con1);
            //                //cmd.CommandType = CommandType.StoredProcedure;
            //                con1.Open();

            //                cmd1.CommandTimeout = 0;
            //                SqlDataReader dr1 = cmd1.ExecuteReader();
            //                //Dispatcher.BeginInvoke((Action)delegate()
            //                //{

            //                //});

            //                while (dr1.Read())
            //                {
            //                    PolCheckLPU polLpu = new PolCheckLPU();
            //                    polLpu.Fam = !(dr1.GetValue(0) is DBNull) ? ((String)dr1.GetValue(0)).Trim().ToUpper() : "";
            //                    polLpu.Im = !(dr1.GetValue(1) is DBNull) ? ((String)dr1.GetValue(1)).Trim().ToUpper() : "";
            //                    polLpu.Ot = !(dr1.GetValue(2) is DBNull) ? ((String)dr1.GetValue(2)).Trim().ToUpper() : "";
            //                    polLpu.Dr = (dr1.GetValue(3) is DateTime) ? (DateTime)dr1.GetValue(3) : polLpu.Dr;
            //                    polLpu.Polis = !(dr1.GetValue(4) is DBNull) ? (String)dr1.GetValue(4) : "";
            //                    polLpu.Q = !(dr1.GetValue(5) is DBNull) ? (String)dr1.GetValue(5) : polLpu.Q;
            //                    polLpu.Date1 = (dr1.GetValue(6) is DateTime) ? (DateTime)dr1.GetValue(6) : polLpu.Date1;
            //                    polLpu.Date2 = (dr1.GetValue(7) is DateTime) ? (DateTime)dr1.GetValue(7) : polLpu.Date2;
            //                    polLpu.Id = (int)dr1.GetValue(8);
            //                    polLpu.Sumv = (dr1.GetValue(9) is Decimal) ? (Decimal)dr1.GetValue(9) : polLpu.Sumv;
            //                    polLpu.UslOk = (dr1.GetValue(10) is int) ? (int)dr1.GetValue(10) : polLpu.UslOk;

            //                    lpList.Add(polLpu);

            //                }
            //                dr1.Close();
            //                con1.Close();
            //            }

            //            List<SANK> mList = new List<SANK>();

            //            Dispatcher.BeginInvoke((Action)delegate()
            //            {
            //                LogBox.Text += "Проверка:" + Environment.NewLine;
            //            });

            //            const int count = 100;
            //            int i = 0;
            //            int i1 = 0;
            //            int c = 0;
            //            int max = lpList.Count();
            //            int progressInt = max / count;
            //            int progressIntLock = progressInt;
            //            int[] progressInts = new int[count];
            //            for (int j = 0; j < count; j++)
            //            {
            //                progressInts[j] = progressInt;
            //                progressInt += progressIntLock;
            //            }
            //            progressInts[count - 1] = max;
            //            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            //                                   new Action(delegate()
            //                                   {
            //                                       progressBar1.EditValue = 0;
            //                                       progressBar1.IsVisible = true;
            //                                   }));
            //            int ii = 0;




            //            foreach (var p in lpList) //, p =>
            //            {
            //                bool isValid = true;
            //                bool isPolisCheck = false;
            //                string coment = "";
            //                string coment2 = "";
            //                string coment3 = "";

            //                if (i == progressInts[c])
            //                {
            //                    if (c < count - 1) c += 1;
            //                    Dispatcher.BeginInvoke(
            //                        System.Windows.Threading.DispatcherPriority.Normal,
            //                        new Action(delegate()
            //                        {
            //                            ii++;
            //                            progressBar1.EditValue = ii;
            //                        }));
            //                }
            //                i += 1;

            //                //if (p.Fam == "МАХАНЬКОВ")
            //                //    p.Fam = "МАХАНЬКОВ";
            //                List<PolisCheck> polisChecks = new List<PolisCheck>();
            //                using (SqlConnection con1 = new SqlConnection(Properties.Settings.Default.SRZ2))
            //                {
            //                    var cmdStr = @"select p.FAM, p.IM, p.OT, p.DR, NPOL, p.DBEG, p.DEND, p.DSTOP, p.Q, p.OPDOC, p.ENP, p.ID, p.DS
            //                from PEOPLE p where ID = (SELECT TOP 1 PID 
            //                from HISTFDR where";
            //                    if (p.Fam.Contains("Е") || p.Fam.Contains("Ё"))
            //                        cmdStr = cmdStr + string.Format(@" FAM LIKE '{0}'", EFix(p.Fam));
            //                    else
            //                        cmdStr = cmdStr + string.Format(@" FAM = '{0}'", p.Fam);
            //                    if (p.Im.Contains("Е") || p.Im.Contains("Ё"))
            //                        cmdStr = cmdStr + string.Format(@" and IM LIKE '{0}'", EFix(p.Im));
            //                    else
            //                        cmdStr = cmdStr + string.Format(@" and IM = '{0}'", p.Im);
            //                    if (p.Ot.Contains("Е") || p.Ot.Contains("Ё"))
            //                        cmdStr = cmdStr + string.Format(@" and OT LIKE '{0}'", EFix(p.Ot));
            //                    else
            //                        cmdStr = cmdStr + string.Format(@" and OT = '{0}'", p.Ot);
            //                    cmdStr = cmdStr + string.Format(@" and DR = '{0}') and p.DS is not NULL", p.Dr.Value.ToString("yyyyMMdd"));

            //                    using (SqlCommand cmd1 = new SqlCommand(cmdStr, con1))
            //                    {
            //                        con1.Open();
            //                        cmd1.CommandTimeout = 0;
            //                        using (SqlDataReader dr1 = cmd1.ExecuteReader())
            //                        {
            //                            int ip = 0;
            //                            while (dr1.Read())
            //                            {
            //                                PolisCheck polCh = new PolisCheck();
            //                                polCh.Fam = !(dr1.GetValue(0) is DBNull) ? (String)dr1.GetValue(0) : "";
            //                                polCh.Im = !(dr1.GetValue(1) is DBNull) ? (String)dr1.GetValue(1) : "";
            //                                polCh.Ot = !(dr1.GetValue(2) is DBNull) ? (String)dr1.GetValue(2) : "";
            //                                polCh.Dr = (dr1.GetValue(3) is DateTime) ? (DateTime)dr1.GetValue(3) : polCh.Dr;
            //                                polCh.Polis = !(dr1.GetValue(4) is DBNull) ? (String)dr1.GetValue(4) : null;
            //                                polCh.Dbeg = (dr1.GetValue(5) is DateTime) ? (DateTime)dr1.GetValue(5) : polCh.Dbeg;
            //                                polCh.Dend = (dr1.GetValue(6) is DateTime) ? (DateTime)dr1.GetValue(6) : polCh.Dend;
            //                                polCh.Dstop = (dr1.GetValue(7) is DateTime) ? (DateTime)dr1.GetValue(7) : polCh.Dstop;
            //                                polCh.Q = !(dr1.GetValue(8) is DBNull) ? (String)dr1.GetValue(8) : null;
            //                                polCh.Opdoc = !(dr1.GetValue(9) is DBNull) ? (int)dr1.GetValue(9) : polCh.Opdoc;
            //                                polCh.ENP = !(dr1.GetValue(10) is DBNull) ? (String)dr1.GetValue(10) : null;
            //                                polCh.Id = (int)dr1.GetValue(11);
            //                                polCh.Ds = (dr1.GetValue(12) is DateTime) ? (DateTime)dr1.GetValue(12) : polCh.Ds;
            //                                polisChecks.Add(polCh);

            //                                //Dispatcher.BeginInvoke((Action)delegate()
            //                                //{
            //                                //    barStaticItem1.Content = ip++.ToString();
            //                                //});
            //                            }
            //                            dr1.Close();
            //                        }
            //                        con1.Close();
            //                    }
            //                }

            //                if (polisChecks.Any())
            //                {
            //                    var fq = polisChecks.First();
            //                    if (p.Date2 >= fq.Ds && new[] { 1, 2 }.Contains(p.UslOk))
            //                        isValid = false;
            //                    if (p.Date2 > fq.Ds && new[] { 3, 4 }.Contains(p.UslOk))
            //                        isValid = false;

            //                    if (!isValid)
            //                    {
            //                        i1++;
            //                        SluchUpdateContructor("2", "Дата смерти - " + fq.Ds.Value.ToShortDateString(), p.Id);
            //                    }
            //                }


            //            }

            //            if (sb.Length > 0) SluchUpdate(sb.ToString());

            //            Dispatcher.BeginInvoke((Action)delegate()
            //            {
            //                LogBox.Text += "Найдено в результате МЭЭ - " + i1 + Environment.NewLine + Environment.NewLine;
            //            });
            //        }
            //        catch (Exception ex)
            //        {
            //            Dispatcher.BeginInvoke((Action)delegate()
            //            {
            //                ErrorGlobalWindow.ShowError(ex.Message);
            //            });
            //        }
            //    }
    
            //});
            //mekTask.ContinueWith(x =>
            //{
            //    MessageBox.Show("Проверка закончена ");
            //    progressBar1.IsVisible = false;
            //}, uiScheduler);
        }

        
        private StringBuilder sb;
        private StringBuilder sb2;
        private void SluchUpdateContructor(string param1, string param2, int id)
        {
            sb.AppendLine(
                $@"UPDATE SLUCH SET MEE_TYPE = {param1}, MEE_COMENT = '{param2}', REQUEST_DATE = '{SprClass.WorkDate:yyyyMMdd}', USERID = {SprClass
                    .userId} WHERE ID = {id} and MEE_TYPE is NULL");

        }

        public static string EFix(string eFix)
        {
            var ef = eFix.Replace("Е", "@").Replace("Ё", "@");
            return ef.Replace("@", "[ЕЁ]");
        }

        private void LogBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LogBox.Focus();
            Dispatcher.BeginInvoke(new Action(() => LogBox.SelectionStart = LogBox.Text.Length));
        }

        string GetStringOfDates(object[] collection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int item in collection.Select(x => ObjHelper.GetAnonymousValue(x, "ID")))
            {
                //sb.Append("'");
                sb.Append(item.ToString());
                //sb.Append("'");
                sb.Append(",");
            }

            var ids = sb.ToString();
            ids = ids.Remove(ids.Length - 1);
            return ids;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();

            foreach (DynamicBaseClass mek in MeeList.SelectedItems)
            {
                var result = SqlReader.Select(((string)mek.GetValue("ExpSQL")).Replace("@pp1", GetStringOfDates(_rows)), SprClass.LocalConnectionString);
                ids.AddRange(result.Select(x => (int)x.GetValue("ID")));
            }



            var rc = new ReestrControl();
            rc.ElReestrTabNew11.BindDataExpResult(ids);

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Результат выборки",
                MyControl = rc,
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });

        }
    }
}
