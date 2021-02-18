using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using DevExpress.Xpf.Core;
using Ionic.Zip;
using Microsoft.Win32;
using Yamed.Control;
using Yamed.Core;
using Yamed.Entity;
using Yamed.Hospital;
using Yamed.Reports;
using Yamed.Server;

namespace Yamed.Oms
{
    /// <summary>
    /// Логика взаимодействия для EconomyControl.xaml
    /// </summary>
    public partial class EconomyControl : UserControl
    {
        public class D3_PACIENT_OMS_LIS
        {

            public int ID { get; set; }

            public int D3_SCID { get; set; }

            public string ID_PAC { get; set; }

            public string FAM { get; set; }

            public string IM { get; set; }

            public string OT { get; set; }

            public int? W { get; set; }

            public DateTime? DR { get; set; }

            public string TEL { get; set; }

            public string FAM_P { get; set; }

            public string IM_P { get; set; }

            public string OT_P { get; set; }

            public int? W_P { get; set; }

            public DateTime? DR_P { get; set; }

            public string MR { get; set; }

            public int? DOCTYPE { get; set; }

            public string DOCSER { get; set; }

            public string DOCNUM { get; set; }

            public string SNILS { get; set; }

            public string OKATOG { get; set; }

            public string OKATOP { get; set; }

            public string COMENTP { get; set; }

            public int? VPOLIS { get; set; }

            public string SPOLIS { get; set; }

            public string NPOLIS { get; set; }

            public string ST_OKATO { get; set; }

            public string SMO { get; set; }

            public string SMO_OGRN { get; set; }

            public string SMO_OK { get; set; }

            public string SMO_NAM { get; set; }

            public int? INV { get; set; }

            public int? MSE { get; set; }

            public string NOVOR { get; set; }

            public int? VNOV_D { get; set; }

            public int? N_ZAP { get; set; }

            public decimal? PR_NOV { get; set; }

            public int? VETERAN { get; set; }

            public int? WORK_STAT { get; set; }

        }
        public class D3_ZSL_OMS_LIS
        {

            public int ID { get; set; }

            public string ZSL_ID { get; set; }

            public int D3_PID { get; set; }

            public string D3_PGID { get; set; }

            public int D3_SCID { get; set; }

            public int? IDCASE { get; set; }

            public int? VIDPOM { get; set; }

            public int? FOR_POM { get; set; }

            public string NPR_MO { get; set; }

            public string LPU { get; set; }

            public int? VBR { get; set; }

            public DateTime? DATE_Z_1 { get; set; }

            public DateTime? DATE_Z_2 { get; set; }

            public int? P_OTK { get; set; }

            public int? RSLT_D { get; set; }

            public int? KD_Z { get; set; }

            public int? VNOV_M { get; set; }

            public int? RSLT { get; set; }

            public int? ISHOD { get; set; }

            public int? OS_SLUCH { get; set; }

            public int? OS_SLUCH_REGION { get; set; }

            public string VOZR { get; set; }

            public int? VB_P { get; set; }

            public int? IDSP { get; set; }

            public decimal? SUMV { get; set; }

            public int? OPLATA { get; set; }

            public decimal? SUMP { get; set; }

            public decimal? SANK_IT { get; set; }

            public string MEK_COMENT { get; set; }

            public string OSP_COMENT { get; set; }

            public int? MEK_COUNT { get; set; }

            public int? MEE_COUNT { get; set; }

            public int? EKMP_COUNT { get; set; }

            public bool MTR { get; set; }

            public int? USL_OK { get; set; }

            public string P_CEL { get; set; }

            public string EXP_COMENT { get; set; }

            public int? EXP_TYPE { get; set; }

            public DateTime? EXP_DATE { get; set; }

            public int? USERID { get; set; }

            public int? ReqID { get; set; }

            public string USER_COMENT { get; set; }

            public DateTime? NPR_DATE { get; set; }

            public int? DTP { get; set; }

            public int? T_ARRIVAL { get; set; }

            public int? N_ZAP { get; set; }

            public int? PR_NOV { get; set; }

            public int? St_IDCASE { get; set; }

        }

        public class D3_SL_OMS_LIS
        {
            public int ID { get; set; }

            public int D3_ZSLID { get; set; }

            public string D3_ZSLGID { get; set; }

            public string SL_ID { get; set; }

            public int? USL_OK { get; set; }

            public string VID_HMP { get; set; }

            public int? METOD_HMP { get; set; }

            public string LPU_1 { get; set; }

            public string PODR { get; set; }

            public int? PROFIL { get; set; }

            public int? DET { get; set; }

            public string P_CEL { get; set; }

            public string TAL_NUM { get; set; }

            public DateTime? TAL_D { get; set; }

            public DateTime? TAL_P { get; set; }

            public string NHISTORY { get; set; }

            public int? P_PER { get; set; }

            public DateTime? DATE_1 { get; set; }

            public DateTime? DATE_2 { get; set; }

            public int? KD { get; set; }

            public string DS0 { get; set; }

            public string DS1 { get; set; }

            public int? DS1_PR { get; set; }

            public int? DN { get; set; }

            public string CODE_MES1 { get; set; }

            public string CODE_MES2 { get; set; }

            public string KSG_DKK { get; set; }

            public string N_KSG { get; set; }

            public int? KSG_PG { get; set; }

            public int? SL_K { get; set; }

            public decimal? IT_SL { get; set; }

            public int? REAB { get; set; }

            public int? PRVS { get; set; }

            public string VERS_SPEC { get; set; }

            public string IDDOKT { get; set; }

            public decimal? ED_COL { get; set; }

            public decimal? TARIF { get; set; }

            public decimal? SUM_M { get; set; }

            public string COMENTSL { get; set; }

            public int? PROFIL_K { get; set; }

            public int? PRVS15 { get; set; }

            public int? PRVS21 { get; set; }

            public string P_CEL25 { get; set; }

            public string PRVS_VERS { get; set; }

            public int? PRVD { get; set; }

            public int? C_ZAB { get; set; }

            public int? DS_ONK { get; set; }

            public int? St_IDCASE { get; set; }

            public decimal? KSKP { get; set; }

            public int? POVOD { get; set; }

            public string PROFIL_REG { get; set; }

            public int? GRAF_DN { get; set; }

            public int? VID_VIZ { get; set; }

            public int? VID_BRIG { get; set; }

            public int? OMI_INVOICES_EV_ID { get; set; }
        }
        public partial class D3_USL_OMS_LIS
        {

            public int ID { get; set; }

            public int D3_SLID { get; set; }

            public int D3_ZSLID { get; set; }

            public string D3_SLGID { get; set; }

            public string IDSERV { get; set; }

            public string LPU { get; set; }

            public string LPU_1 { get; set; }

            public string PODR { get; set; }

            public int? PROFIL { get; set; }

            public string VID_VME { get; set; }

            public int? DET { get; set; }

            public DateTime? DATE_IN { get; set; }

            public DateTime? DATE_OUT { get; set; }

            public int? P_OTK { get; set; }

            public string DS { get; set; }

            public string CODE_USL { get; set; }

            public decimal? KOL_USL { get; set; }

            public decimal? TARIF { get; set; }

            public decimal? SUMV_USL { get; set; }

            public int? PRVS { get; set; }

            public string CODE_MD { get; set; }

            public int? NPL { get; set; }

            public string COMENTU { get; set; }

            public int? PRVS15 { get; set; }

            public int? PRVS21 { get; set; }

            public string VERS_SPEC { get; set; }

            public string PRVS_VERS { get; set; }

            public int? PRVD { get; set; }

            public int? DOP { get; set; }

            public int? PP { get; set; }

            public int? St_IDSERV { get; set; }

            public int? KOD_SP { get; set; }

            public string FORMUL { get; set; }


        }
        public EconomyControl()
        {
            InitializeComponent();
            if (SprClass.Region == "37")
            {
                lmis.IsVisible = false;
                ldbf.IsVisible = true;
            }
            else if (SprClass.Region !="37")
            {
                lmis.IsVisible = true;
                ldbf.IsVisible = false;
            }
        }

        //private void Hosp_OnClick(object sender, RoutedEventArgs e)
        //{
        //    //Menu.Hide();
        //    //((Button)sender).IsEnabled = false;
        //    //Decorator.IsSplashScreenShown = true;
        //    var sc = ObjHelper.ClassConverter<D3_SCHET_OMS>(DxHelper.GetSelectedGridRow(EconomyWindow11.gridControl));

        //    СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
        //    {
        //        Header = "Реестр Стационар",
        //        MyControl = new ReestrControl1(sc),
        //        IsCloseable = "True",
        //        //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
        //    });

        //    //((Button)sender).IsEnabled = true;
        //}

        private void Amb_OnClick(object sender, RoutedEventArgs e)
        {
            var sc = ObjHelper.ClassConverter<D3_SCHET_OMS>(DxHelper.GetSelectedGridRow(EconomyWindow11.gridControl));
            var rc = new SchetRegisterControl(sc);
            rc.SchetRegisterGrid1.Scids = new List<int> { sc.ID };
            rc.SchetRegisterGrid1.BindDataZsl();
            string M = "";
            if (sc.MONTH.ToString().Length < 2)
            {
                M = "0" + sc.MONTH.ToString();
            }
            else
            {
                M = sc.MONTH.ToString();
            }
            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Реестр счета "+ sc.SchetType+ " от "+ M+"-"+sc.YEAR,
                MyControl = rc,
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }

        private void LoadOldPoliclinic_OnClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "OMS File |*.oms;*.zip";


            var result = openFileDialog.ShowDialog();

            var rfile = openFileDialog.FileName;
            var omszfn = openFileDialog.SafeFileName;

            string zapfn = "";
            string persfn = "";
            var zapms = new MemoryStream();
            var persms = new MemoryStream();

            if (result != true) return;

            var sc = ObjHelper.ClassConverter<D3_SCHET_OMS>(DxHelper.GetSelectedGridRow(EconomyWindow11.gridControl));
            ((DevExpress.Xpf.Bars.BarButtonItem)sender).IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (ZipFile zip = ZipFile.Read(rfile))
                    {
                        foreach (ZipEntry zipEntry in zip)
                        {
                            if (zipEntry.FileName.StartsWith("HM") || zipEntry.FileName.StartsWith("D") ||
                                zipEntry.FileName.StartsWith("T"))
                            {
                                zapfn = zipEntry.FileName;
                                zipEntry.Extract(zapms);
                                zapms.Position = 0;
                            }

                            if (zipEntry.FileName.StartsWith("L"))
                            {
                                persfn = zipEntry.FileName;
                                zipEntry.Extract(persms);
                                persms.Position = 0;
                            }
                        }
                    }
                    //"0x" + BitConverter.ToString(arraytoinsert).Replace("-", "")
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        DXMessageBox.Show(ex.Message + Environment.NewLine + ex.InnerException?.Message);
                    });
                    return;
                }


                var zapsr = new StreamReader(zapms, Encoding.Default);
                string zapxml = zapsr.ReadToEnd();
                zapms.Dispose();
                zapms.Close();

                var perssr = new StreamReader(persms, Encoding.Default);
                string persxml = perssr.ReadToEnd();
                persms.Dispose();
                persms.Close();

                if (string.IsNullOrEmpty(zapxml) || string.IsNullOrEmpty(persxml))
                {
                    //Reader2List.CustomExecuteQuery($@"Update DOX_SCHET SET DOX_STATUS=12 ", SprClass.LocalConnectionString);
                }
                else
                {
                    try
                    {
                        var q = $@"
                            EXEC [dbo].[LoadFromMedialog] '{zapxml.Replace("'", "")}', '{persxml.Replace("'", "")}', '{zapfn}', '{persfn}'
                        ";

                        zapxml = null;
                        persxml = null;
                        Reader2List.CustomExecuteQuery(q, SprClass.LocalConnectionString);
                        q = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            DXMessageBox.Show(ex.Message + Environment.NewLine + ex.InnerException?.Message);
                        });
                    }
                }
                zapsr.Dispose();
                zapsr.Close();
                perssr.Dispose();
                perssr.Close();


                //Console.WriteLine("Распакован " + id);

                GC.WaitForPendingFinalizers();
                GC.Collect();

            }).ContinueWith(x =>
            {
                EconomyWindow11.linqInstantFeedbackDataSource.Refresh();

                DXMessageBox.Show("Загрузка успешно завершена");

                ((DevExpress.Xpf.Bars.BarButtonItem)sender).IsEnabled = true;


            }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void Calculate_OnClick(object sender, RoutedEventArgs e)
        {
            var region = SprClass.Region;
            var sc = ObjHelper.ClassConverter<D3_SCHET_OMS>(DxHelper.GetSelectedGridRow(EconomyWindow11.gridControl));
            ((DevExpress.Xpf.Bars.BarButtonItem)sender).IsEnabled = false;

            var calcs = SqlReader.Select(
                $@"select [name] from sys.procedures where [name] like '%p_autocalc_0{region}%' order by [name]",
                SprClass.LocalConnectionString);

            Reader2List.CustomExecuteQuery($@"
                    exec [dbo].[p_fix] {sc.ID}", SprClass.LocalConnectionString);
            foreach (var calc in calcs)
            {
                Reader2List.CustomExecuteQuery($@"
                    exec [dbo].[{calc.GetValue("name")}] {sc.ID}", SprClass.LocalConnectionString);
            }


            if (region == "46" || region == "046")
            {
                Reader2List.CustomExecuteQuery($@"
                    exec[dbo].[p_oms_calc_kslp_sum] {0}, {sc.ID}, 'scid'", SprClass.LocalConnectionString);

                Reader2List.CustomExecuteQuery($@"
                    exec[dbo].[p_oms_calc_ksg] {0}, {sc.ID}, 'scid'", SprClass.LocalConnectionString);
            }
            if (region == "37" || region == "037")
            {
                Reader2List.CustomExecuteQuery($@"
                    exec[dbo].[p_oms_calc_kslp_sum] {0}, {sc.ID}, 'scid'", SprClass.LocalConnectionString);
                var ksgs = SqlReader.Select(
                $@"select auto_ksg from D3_KSG_KPG_OMS ksg join D3_SL_OMS sl on sl.ID=ksg.D3_SLID join D3_ZSL_OMS zsl on zsl.ID=sl.D3_ZSLID where zsl.D3_SCID={sc.ID}",
                SprClass.LocalConnectionString);
                foreach (var ksg in ksgs)
                {
                    if ((bool?)ksg.GetValue("auto_ksg")==true)
                    {
                        Reader2List.CustomExecuteQuery($@"
                    exec[dbo].[p_oms_calc_ksg] {0}, {sc.ID}, 'scid'", SprClass.LocalConnectionString);
                    }
                    else if ((bool?)ksg.GetValue("auto_ksg") == false)
                    {
                        Reader2List.CustomExecuteQuery($@"
                    exec[dbo].[p_oms_calc_ksg_m] {0}, {sc.ID}, 'scid'", SprClass.LocalConnectionString);
                    }
                }
            }
            DXMessageBox.Show("Расчет завершен.");

            ((DevExpress.Xpf.Bars.BarButtonItem)sender).IsEnabled = true;

        }





        private void ExportOms_OnClick(object sender, RoutedEventArgs e)
        {
            var sc = ObjHelper.ClassConverter<D3_SCHET_OMS>(DxHelper.GetSelectedGridRow(EconomyWindow11.gridControl));

            ((DevExpress.Xpf.Bars.BarButtonItem)sender).IsEnabled = false;

            //Task.Factory.StartNew(() =>
            //{

            //XmlStreem2(sc, true);
            ExportOms30K(sc);
            //}).ContinueWith(x =>
            //{
            ((DevExpress.Xpf.Bars.BarButtonItem)sender).IsEnabled = true;

            //DXMessageBox.Show("Невозможно выполнить экспорт, не все случаи просчитаны, проверте случаи с пустыми суммами и выполните расчет.");
            //}, TaskScheduler.FromCurrentSynchronizationContext());
        }





        void ExportOms30K(object schet)
        {
      //      Reader2List.CustomExecuteQuery($@"
      //                  UPDATE D3_ZSL_OMS SET ZSL_ID = newid() WHERE ZSL_ID IS NULL
      //                  UPDATE D3_SL_OMS SET SL_ID = newid() WHERE SL_ID IS NULL
      //                  UPDATE D3_PACIENT_OMS SET ID_PAC = newid() WHERE ID_PAC IS NULL
      //                  UPDATE PACIENT SET ID_PAC = newid() WHERE ID_PAC IS NULL
      //                  UPDATE SLUCH SET IDSLG = newid() WHERE IDSLG IS NULL

						//UPDATE D3_SL_OMS SET SL_ID = newid() where SL_ID in (
						//SELECT SL_ID FROM D3_SL_OMS GROUP by SL_ID HAVING count(*) > 1)

						//UPDATE D3_ZSL_OMS SET ZSL_ID = newid() where ZSL_ID in (
						//SELECT ZSL_ID FROM D3_ZSL_OMS GROUP by ZSL_ID HAVING count(*) > 1)

						//UPDATE D3_PACIENT_OMS SET ID_PAC = newid() where ID_PAC in (
						//SELECT ID_PAC FROM D3_PACIENT_OMS GROUP by ID_PAC HAVING count(*) > 1)

						//UPDATE SLUCH SET IDSLG = newid() where IDSLG in (
						//SELECT IDSLG FROM SLUCH GROUP by IDSLG HAVING count(*) > 1)

						//UPDATE D3_USL_OMS SET D3_SLGID = sl.SL_ID
						//FROM D3_SL_OMS sl
      //                  join D3_ZSL_OMS zsl on sl.D3_ZSLID = zsl.ID and D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")}
						//join D3_USL_OMS u on sl.ID = u.D3_SLID
						//where SL_ID <> D3_SLGID


      //                ", SprClass.LocalConnectionString);

            //Reader2List.CustomExecuteQuery($@"
            //            UPDATE D3_SL_OMS SET USL_OK = 3 WHERE USL_OK is null
            //            UPDATE D3_SL_OMS SET P_CEL = '1.1' WHERE P_CEL is null and USL_OK = 3
            //            UPDATE D3_SL_OMS SET P_CEL = NULL WHERE USL_OK = 4
            //", SprClass.LocalConnectionString);

//            Reader2List.CustomExecuteQuery($@"
//            UPDATE SLUCH SET IDDOKT = d.SNILS 
//            FROM SLUCH sl
//            Join DoctorBd d on sl.IDDOKTO = d.id
//            where schet_id = {ObjHelper.GetAnonymousValue(schet, "ID")}
//            ", SprClass.LocalConnectionString);

//            Reader2List.CustomExecuteQuery($@"
//            UPDATE USL SET CODE_MD = d.SNILS 
//            FROM USL u
//            Join DoctorBd d on u.CODE_MDLPU = d.id
//            where schet_id = {ObjHelper.GetAnonymousValue(schet, "ID")}
//            ", SprClass.LocalConnectionString);

//            Reader2List.CustomExecuteQuery($@"
// UPDATE D3_USL_OMS
//            SET	LPU = z.LPU   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.LPU is NULL
// UPDATE D3_USL_OMS
//            SET	PROFIL = s.PROFIL   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.PROFIL is NULL
//UPDATE D3_USL_OMS
//            SET	DET = s.DET   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.DET is NULL
//UPDATE D3_USL_OMS
//            SET	DATE_IN = s.DATE_1  
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.DATE_IN is NULL
//UPDATE D3_USL_OMS
//            SET	DATE_OUT = s.DATE_2   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.DATE_OUT is NULL
//UPDATE D3_USL_OMS
//            SET	DS = s.DS1   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.DS is NULL
//UPDATE D3_USL_OMS
//            SET	CODE_USL = u.VID_VME   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.CODE_USL is NULL
//UPDATE D3_USL_OMS
//            SET	PRVS = s.PRVS   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.PRVS is NULL
//UPDATE D3_USL_OMS
//            SET	CODE_MD = s.IDDOKT   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.CODE_MD is NULL
//UPDATE D3_USL_OMS
//            SET	KOL_USL = 1  
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.KOL_USL is NULL
//UPDATE D3_USL_OMS
//            SET	TARIF = 0.00   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.TARIF is NULL
//UPDATE D3_USL_OMS
//            SET	SUMV_USL = 0.00   
            
//            FROM D3_SL_OMS AS s 
//            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
//			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
//            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.SUMV_USL is NULL
//            ", SprClass.LocalConnectionString);


            var qxml = SqlReader.Select($@"
            exec p_oms_export_30K {ObjHelper.GetAnonymousValue(schet, "ID")}"
, SprClass.LocalConnectionString);



            string result1 = "<?xml version=\"1.0\" encoding=\"windows-1251\"?>" + (string)qxml[0].GetValue("HM");
            string result2 = "<?xml version=\"1.0\" encoding=\"windows-1251\"?>" + (string)qxml[0].GetValue("LM");
            using (ZipFile zip = new ZipFile(Encoding.GetEncoding("windows-1251")))
            {
                zip.AddEntry((string)qxml[0].GetValue("hf_name") + ".xml", result1);
                zip.AddEntry((string)qxml[0].GetValue("lf_name") + ".xml", result2);

                //if (isLoop)
                //{
                //    if (!Directory.Exists(Environment.CurrentDirectory + @"\Out"))
                //        Directory.CreateDirectory(Environment.CurrentDirectory + @"\Out");

                //    var zipFile = Environment.CurrentDirectory + @"\Out\" + (string)qxml[0].GetValue("hf_name") + ".oms";
                //    zip.Save(zipFile);
                //}
                //else
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();

                        saveFileDialog.Filter = "OMS File (*.oms)|*.oms";
                        saveFileDialog.FileName = (string)qxml[0].GetValue("hf_name") + ".oms";

                        bool? result = saveFileDialog.ShowDialog();
                        if (result == true)
                            zip.Save(saveFileDialog.FileName);
                    });
                }

            }

        }

        public void XmlStreem2(object schet, bool isTfomsSchet, bool isLoop = false, int loopCount = 1)
        {

            //LoadingDecorator1.IsSplashScreenShown = true;

            int i = 0;
            var schetp = schet;
            string plat = string.IsNullOrEmpty((string)ObjHelper.GetAnonymousValue(schetp, "PLAT")) ? "46" : (string)ObjHelper.GetAnonymousValue(schetp, "PLAT");
            string m = Convert.ToString(ObjHelper.GetAnonymousValue(schetp, "MONTH"));
            string mm = (int)ObjHelper.GetAnonymousValue(schetp, "MONTH") < 10 ? "0" + m : m;
            string sf = plat.Length > 2 ? "S" : "T";

            string filename1 = "";
            string filename2 = "";

            if (SprClass.ProdSett.OrgTypeStatus == OrgType.Lpu)
            {
                filename1 = "HM" + ObjHelper.GetAnonymousValue(schetp, "CODE_MO") + sf + plat + "_" +
                            ObjHelper.GetAnonymousValue(schetp, "YEAR").ToString().Substring(2) +
                            mm + "1";
                filename2 = "LM" + ObjHelper.GetAnonymousValue(schetp, "CODE_MO") + sf + plat + "_" +
                            ObjHelper.GetAnonymousValue(schetp, "YEAR").ToString().Substring(2) + mm + "1";
            }
            else if (ObjHelper.GetAnonymousValue(schetp, "PLAT") != null && ((string)ObjHelper.GetAnonymousValue(schetp, "PLAT")).StartsWith("04"))
            {

                var com1 = (string)ObjHelper.GetAnonymousValue(schetp, "COMENTS");
                var com2 = com1.Substring(com1.IndexOf("M") + 1);
                var codemoreg = com2.Remove(com2.IndexOf("S"));

                string codef = "";
                if (((string)ObjHelper.GetAnonymousValue(schetp, "COMENTS")).StartsWith("D"))
                {
                    codef = ((string)ObjHelper.GetAnonymousValue(schetp, "COMENTS")).Substring(0, 2);

                }
                else
                    codef = ((string)ObjHelper.GetAnonymousValue(schetp, "COMENTS")).Substring(0, 1);

                filename1 = codef + "S" + ((string)ObjHelper.GetAnonymousValue(schetp, "PLAT")).Substring(3) + "T" +
                            ((string)ObjHelper.GetAnonymousValue(schetp, "PLAT")).Substring(0, 2) + "_" +
                            ObjHelper.GetAnonymousValue(schetp, "YEAR").ToString().Substring(2) +
                            mm + loopCount + "_" + codemoreg;
                filename2 = "L" + codef.Remove(0, 1) + "S" + "" + ((string)ObjHelper.GetAnonymousValue(schetp, "PLAT")).Substring(3) + "T" +
                            ((string)ObjHelper.GetAnonymousValue(schetp, "PLAT")).Substring(0, 2) + "_" +
                            ObjHelper.GetAnonymousValue(schetp, "YEAR").ToString().Substring(2) +
                            mm + loopCount + "_" + codemoreg;
            }
            else
            {
                filename1 = (string)ObjHelper.GetAnonymousValue(schetp, "ZAPFILENAME");
                filename2 = (string)ObjHelper.GetAnonymousValue(schetp, "PERSFILENAME");
            }
            string filename3 = "XM" + ObjHelper.GetAnonymousValue(schetp, "CODE_MO") + sf + plat + "_" + ObjHelper.GetAnonymousValue(schetp, "YEAR").ToString().Substring(2) +
                    mm + "1";


            Reader2List.CustomExecuteQuery($@"
                        UPDATE D3_ZSL_OMS SET ZSL_ID = newid() WHERE ZSL_ID IS NULL
                        UPDATE D3_SL_OMS SET SL_ID = newid() WHERE SL_ID IS NULL
                        UPDATE D3_PACIENT_OMS SET ID_PAC = newid() WHERE ID_PAC IS NULL
                        UPDATE SLUCH SET IDSLG = newid() WHERE IDSLG IS NULL

						UPDATE D3_SL_OMS SET SL_ID = newid() where SL_ID in (
						SELECT SL_ID FROM D3_SL_OMS GROUP by SL_ID HAVING count(*) > 1)

						UPDATE D3_ZSL_OMS SET ZSL_ID = newid() where ZSL_ID in (
						SELECT ZSL_ID FROM D3_ZSL_OMS GROUP by ZSL_ID HAVING count(*) > 1)

						UPDATE D3_PACIENT_OMS SET ID_PAC = newid() where ID_PAC in (
						SELECT ID_PAC FROM D3_PACIENT_OMS GROUP by ID_PAC HAVING count(*) > 1)

						UPDATE SLUCH SET IDSLG = newid() where IDSLG in (
						SELECT IDSLG FROM SLUCH GROUP by IDSLG HAVING count(*) > 1)

						UPDATE D3_USL_OMS SET D3_SLGID = sl.SL_ID
						FROM D3_SL_OMS sl
                        join D3_ZSL_OMS zsl on sl.D3_ZSLID = zsl.ID and D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")}
						join D3_USL_OMS u on sl.ID = u.D3_SLID
						where SL_ID <> D3_SLGID


                      ", SprClass.LocalConnectionString);

            Reader2List.CustomExecuteQuery($@"
                        UPDATE D3_SL_OMS SET PROFIL = 136 WHERE PROFIL = 2-- or PROFIL = 184
                        UPDATE SLUCH SET PROFIL = 136 WHERE PROFIL = 2-- or PROFIL = 184
                        UPDATE USL SET PROFIL = 136 WHERE PROFIL = 2-- or PROFIL = 184
                        UPDATE D3_USL_OMS SET PROFIL = 136 WHERE PROFIL = 2-- or PROFIL = 184
            ", SprClass.LocalConnectionString);

            Reader2List.CustomExecuteQuery($@"
                        UPDATE D3_SL_OMS SET PROFIL = 162 WHERE PROFIL = 64
                        UPDATE SLUCH SET PROFIL = 162 WHERE PROFIL = 64
                        UPDATE USL SET PROFIL = 162 WHERE PROFIL = 64
                        UPDATE D3_USL_OMS SET PROFIL = 162 WHERE PROFIL = 64
            ", SprClass.LocalConnectionString);

            Reader2List.CustomExecuteQuery($@"
                        UPDATE D3_SL_OMS SET USL_OK = 3 WHERE USL_OK is null
                        UPDATE D3_SL_OMS SET P_CEL = '1.1' WHERE P_CEL is null and USL_OK = 3
                        UPDATE D3_SL_OMS SET P_CEL = NULL WHERE USL_OK = 4
            ", SprClass.LocalConnectionString);

            Reader2List.CustomExecuteQuery($@"
            UPDATE SLUCH SET IDDOKT = d.SNILS 
            FROM SLUCH sl
            Join DoctorBd d on sl.IDDOKTO = d.id
            where schet_id = {ObjHelper.GetAnonymousValue(schet, "ID")}
            ", SprClass.LocalConnectionString);

            Reader2List.CustomExecuteQuery($@"
            UPDATE USL SET CODE_MD = d.SNILS 
            FROM USL u
            Join DoctorBd d on u.CODE_MDLPU = d.id
            where schet_id = {ObjHelper.GetAnonymousValue(schet, "ID")}
            ", SprClass.LocalConnectionString);

            Reader2List.CustomExecuteQuery($@"
 UPDATE D3_USL_OMS
            SET	LPU = z.LPU   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.LPU is NULL
 UPDATE D3_USL_OMS
            SET	PROFIL = s.PROFIL   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.PROFIL is NULL
UPDATE D3_USL_OMS
            SET	DET = s.DET   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.DET is NULL
UPDATE D3_USL_OMS
            SET	DATE_IN = s.DATE_1  
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.DATE_IN is NULL
UPDATE D3_USL_OMS
            SET	DATE_OUT = s.DATE_2   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.DATE_OUT is NULL
UPDATE D3_USL_OMS
            SET	DS = s.DS1   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.DS is NULL
UPDATE D3_USL_OMS
            SET	CODE_USL = u.VID_VME   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.CODE_USL is NULL
UPDATE D3_USL_OMS
            SET	PRVS = s.PRVS   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.PRVS is NULL
UPDATE D3_USL_OMS
            SET	CODE_MD = s.IDDOKT   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.CODE_MD is NULL
UPDATE D3_USL_OMS
            SET	KOL_USL = 1  
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.KOL_USL is NULL
UPDATE D3_USL_OMS
            SET	TARIF = 0.00   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.TARIF is NULL
UPDATE D3_USL_OMS
            SET	SUMV_USL = 0.00   
            
            FROM D3_SL_OMS AS s 
            JOIN D3_USL_OMS AS u ON u.D3_SLID = s.ID
			JOIN D3_ZSL_OMS AS z ON s.D3_ZSLID = z.ID
            WHERE z.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} and u.SUMV_USL is NULL
            ", SprClass.LocalConnectionString);





            var pacientList = Reader2List.CustomSelect<D3_PACIENT_OMS>($@"
			Select distinct t.* FROM (
			Select pa.* From D3_ZSL_OMS zsl
            Join D3_PACIENT_OMS pa on zsl.D3_PID = pa.id
            where zsl.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")}
            --order by pa.FAM, pa.IM, pa.OT, pa.DR
			UNION
			Select pa.* From SLUCH sl
            Join D3_PACIENT_OMS pa on sl.PID = pa.id
            where sl.SCHET_ID = {ObjHelper.GetAnonymousValue(schet, "ID")}
			and sl.USL_OK in (1,2)
			) as t
            order by FAM, IM, OT, DR
            ", SprClass.LocalConnectionString);

            var zsluchList = Reader2List.CustomSelect<D3_ZSL_OMS>($@"
            Select ID, D3_PID, D3_SCID,  [VIDPOM]      ,[FOR_POM]      ,[NPR_MO]      ,[LPU]      ,[VBR]      ,[DATE_Z_1]      ,[DATE_Z_2]      ,[P_OTK]      ,[RSLT_D]      ,[KD_Z]      ,[VNOV_M]      ,[RSLT]      ,[ISHOD]
					,[OS_SLUCH]      ,[OS_SLUCH_REGION]      ,[VOZR]      ,[VB_P]      ,[IDSP]      ,[SUMV]      ,NULL [OPLATA]      ,[SUMP]      ,[SANK_IT]      ,[IDCASE]      ,ISNULL ( [ZSL_ID], NEWID()) [ZSL_ID],
                      NULL D3_PGID, NULL FLK_COMENT, NULL OSP_COMENT, NULL USERID, VETERAN
			From D3_ZSL_OMS zsl
            where zsl.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} 
			UNION ALL
            Select ID, PID D3_PID, SCHET_ID D3_SCID,  [VIDPOM]      ,
            case 
                when extr = 2 and USL_OK in (1,2) then 1
                when (extr = 1 OR EXTR IS NULL) and USL_OK in (1,2) then 3
                when SLUCH_TYPE = 1 and USL_OK = 4 then 1
                when SLUCH_TYPE = 3 and USL_OK = 4 then 2
                else 3 
            end [FOR_POM]      
            ,[NPR_MO]      ,[LPU]      ,NULL [VBR]      ,DATE_1 [DATE_Z_1]      ,DATE_2 [DATE_Z_2]      ,NULL [P_OTK]      ,[RSLT_D]      ,							
			(CASE 
				WHEN sl.USL_OK = 1 THEN (CASE WHEN sl.DATE_1 = sl.DATE_2 THEN 1 ELSE DATEDIFF(DAY, sl.DATE_1, sl.DATE_2) END)
				WHEN sl.USL_OK = 2 THEN DATEDIFF(DAY, sl.DATE_1, sl.DATE_2) + 1 -
				(SELECT COUNT(*) FROM dbo.WORK_DAY wd WHERE wd.LPU = sl.LPU and wd.PODR_ID = sl.LPU_1 and H_DATE BETWEEN sl.DATE_1 AND sl.DATE_2)
			END) [KD_Z]      ,NULL [VNOV_M]      ,[RSLT]      ,[ISHOD]
					,[OS_SLUCH]      ,[OS_SLUCH_REGION]      ,[VOZR]      ,NULL [VB_P]      ,[IDSP]      ,[SUMV]      ,NULL [OPLATA]      ,[SUMP]      ,[SANK_IT]      ,[IDCASE]      ,IDSLG [ZSL_ID],
                      NULL D3_PGID, NULL FLK_COMENT, NULL OSP_COMENT, NULL USERID, NULL VETERAN
			From SLUCH sl
            where sl.SCHET_ID = {ObjHelper.GetAnonymousValue(schet, "ID")}
			and USL_OK in (1,2) 
            ", SprClass.LocalConnectionString).OrderBy(x => x.D3_PID);


            var sluchList = Reader2List.CustomSelect<D3_SL_OMS>($@"
            Select sl.ID  ,[D3_ZSLID]      ,ISNULL ( [SL_ID], NEWID()) [SL_ID]      ,[USL_OK]      ,[VID_HMP]      ,[METOD_HMP]      ,[LPU_1]      ,[PODR]      ,[PROFIL]      ,[DET]      ,[P_CEL]      ,[TAL_N]      ,[TAL_D]      ,[TAL_P]
				,[NHISTORY]      ,[P_PER]      ,[DATE_1]      ,[DATE_2]      ,[KD]      ,[DS0]      ,[DS1]      ,[DS1_PR]      ,[DN]      ,[CODE_MES1]      ,[CODE_MES2]      ,[N_KSG]      ,[KSG_PG]
					,[SL_K]      ,[IT_SL]      ,[REAB]      ,[PRVS]      ,'V015' [VERS_SPEC]      ,[IDDOKT]      ,[ED_COL]      ,[TARIF]      ,[SUM_M]      ,[COMENTSL]      ,[KSG_DKK]
			From D3_SL_OMS sl
			JOIN D3_ZSL_OMS zsl on sl.D3_ZSLID = zsl.ID
            where zsl.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} 
			UNION ALL
            Select u.slid ID , SLID [D3_ZSLID]      ,/*ISNULL (IDSLG, NEWID()) [SL_ID]*/ NEWID() [SL_ID]  ,[USL_OK]      ,[VID_HMP]      ,[METOD_HMP]      ,u.[LPU_1]      ,u.[PODR]      ,u.[PROFIL]      ,u.[DET]      ,NULL [P_CEL]      ,NNAPR [TAL_N]      ,DNAPR [TAL_D]      ,NULL [TAL_P]
				,[NHISTORY]      ,NULL [P_PER]      ,[DATE_IN]      ,[DATE_OUT]      ,
				(CASE 
					WHEN sl.USL_OK = 1 THEN (CASE WHEN u.DATE_IN = u.DATE_OUT THEN 1 ELSE DATEDIFF(DAY, u.DATE_IN, u.DATE_OUT) END)
					WHEN sl.USL_OK = 2 THEN DATEDIFF(DAY, u.DATE_IN, u.DATE_OUT) + 1 -
					(SELECT COUNT(*) FROM dbo.WORK_DAY wd WHERE wd.LPU = u.LPU and wd.PODR_ID = u.LPU_1 and H_DATE BETWEEN u.DATE_IN AND u.DATE_OUT)
                END) [KD]      ,[DS0]      ,[DS]      , null [DS1_PR]      ,null [DN]      ,[CODE_MES1]      ,[CODE_MES2]      ,(select KSGNUM from SprKsg where id = u.IDKSG) [N_KSG]
				      ,null [KSG_PG]
					,case when ISNULL(u.DIFF_K, 0.00) <> 0.00 then 1 else null end [SL_K]      ,case when ISNULL(u.DIFF_K, 0.00) <> 0.00 then u.DIFF_K else null end  [IT_SL]      ,null [REAB]      ,u.MSPUID [PRVS]      ,'V015' [VERS_SPEC]      ,CODE_MD [IDDOKT]      ,u.KOL_USL      ,u.[TARIF]      ,u.SUMV_USL [SUM_M]      ,[COMENTSL]      ,[KSG_DKK]
			From USL u
			join SLUCH sl on u.SLID = sl.ID
            where sl.SCHET_ID = {ObjHelper.GetAnonymousValue(schet, "ID")}
			and USL_OK in (1,2) and u.CODE_USL like 'TF%'
			--UNION ALL
			--SELECT [ID]       , ID [D3_ZSLID]      , ISNULL (IDSLG, NEWID()) [SL_ID]      ,[USL_OK]      ,[VID_HMP]      ,[METOD_HMP]      ,[LPU_1]      ,[PODR]      ,[PROFIL]      ,[DET]
			--,CASE
			--	WHEN SLUCH_TYPE = 1 and USL_OK = 3 THEN  '1.3'
			--	WHEN SLUCH_TYPE = 2 and USL_OK = 3 THEN  '1.1'
			--	WHEN SLUCH_TYPE = 3 and USL_OK = 3 THEN  '2.0'
			--	END [P_CEL]      
			--	, NULL [TAL_N]      ,NULL [TAL_D]      ,NULL [TAL_P]      ,[NHISTORY]      ,NULL [P_PER]      ,[DATE_1]      ,[DATE_2]
			--		 ,NULL [KD]      ,[DS0]      ,[DS1]      ,NULL [DS1_PR]      ,NULL [DN]      ,[CODE_MES1]      ,[CODE_MES2]      ,NULL [N_KSG]      ,NULL [KSG_PG]      ,NULL [SL_K]      ,NULL [IT_SL]      ,NULL [REAB]      , MSPID [PRVS]      ,'V015' [VERS_SPEC]      ,[IDDOKT]      ,[ED_COL]      ,[TARIF]      ,SUMV [SUM_M]      ,[COMENTSL]      ,NULL [KSG_DKK]
			--FROM [dbo].SLUCH sl
			--where sl.SCHET_ID = {ObjHelper.GetAnonymousValue(schet, "ID")} and 
			--USL_OK in (3,4)
            ", SprClass.LocalConnectionString).OrderBy(x => x.D3_ZSLID);

            var ds2List = Reader2List.CustomSelect<SLUCH_DS2>($@"
            Select distinct ds2.* From SLUCH sl
            Join SLUCH_DS2 ds2 on sl.ID = ds2.SLID
            where sl.schet_id = {ObjHelper.GetAnonymousValue(schet, "ID")}
            ", SprClass.LocalConnectionString).OrderBy(x => x.ID);

            var ds3List = Reader2List.CustomSelect<SLUCH_DS2>($@"
            Select distinct ds3.* From SLUCH sl
            Join SLUCH_DS3 ds3 on sl.ID = ds3.SLID
            where sl.schet_id = {ObjHelper.GetAnonymousValue(schet, "ID")}
            ", SprClass.LocalConnectionString).OrderBy(x => x.ID);

            var kslpList = Reader2List.CustomSelect<USL_KSLP>($@"
			SELECT distinct 	ks.[ID], ks.[IDSL], ks.[Z_SL], sl.[ID] SLID
			From SLUCH sl
            Join USL u on sl.id = u.SLID
            JOIN USL_KSLP ks on ks.SLID = u.ID
            where sl.SCHET_ID = {ObjHelper.GetAnonymousValue(schet, "ID")}
			and USL_OK in (1,2)
            ", SprClass.LocalConnectionString).OrderBy(x => x.ID);

            var uslList = Reader2List.CustomSelect<D3_USL_OMS>($@"
			SELECT  u.ID      ,[D3_SLID]      ,u.[D3_ZSLID]      ,[D3_SLGID]      ,[IDSERV]      ,u.[LPU]      ,u.[LPU_1]      ,u.[PODR]      ,u.[PROFIL]
					,[VID_VME]      ,u.[DET]      ,[DATE_IN]      ,[DATE_OUT]      ,u.[P_OTK]      ,[DS]      ,[CODE_USL]      ,[KOL_USL]      ,u.[TARIF]
						,[SUMV_USL]      ,u.[PRVS]      ,[CODE_MD]      ,[NPL]      ,[COMENTU]
			From D3_SL_OMS sl
            Join D3_USL_OMS u on sl.id = u.D3_SLID
			join D3_ZSL_OMS zsl on sl.D3_ZSLID = zsl.ID
            where zsl.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")}
			UNION
						
			SELECT  u.ID  , u.slid [D3_SLID]      ,null [D3_ZSLID]      ,null [D3_SLGID]      ,[IDSERV]      ,u.[LPU]      ,u.[LPU_1]      ,u.[PODR]      ,u.[PROFIL]
					,[VID_VME]      ,u.[DET]      ,[DATE_IN]      ,[DATE_OUT]      ,null [P_OTK]      ,[DS]      ,[CODE_USL]      ,[KOL_USL]      ,u.[TARIF]
						,[SUMV_USL]      ,u.MSPUID [PRVS]      ,[CODE_MD]      ,null [NPL]      ,[COMENTU]
			From SLUCH sl
            Join USL u on sl.id = u.SLID
            where sl.SCHET_ID = {ObjHelper.GetAnonymousValue(schet, "ID")}
			and (USL_OK in (1,2) and u.CODE_USL like 'VM%')-- or USL_OK in (3,4)
            ", SprClass.LocalConnectionString);

            //var sankList = Reader2List.CustomSelect<SANK>(
            //(SprClass.ProdSett.OrgTypeStatus == OrgType.Smo || SprClass.ProdSett.OrgTypeStatus == OrgType.Lpu) && isTfomsSchet ? $@"
            //Select distinct sa.* From SLUCH sl
            //Join sank sa on sl.id = sa.slid
            //where sl.schet_id = {ObjHelper.GetAnonymousValue(schet, "ID")} and (sl.vopl is null or sl.vopl = 1)" :
            //SprClass.ProdSett.OrgTypeStatus == OrgType.Tfoms && isTfomsSchet ? $@"
            //Select distinct sa.* From SLUCH sl
            //Join D3_PACIENT_OMS pa on sl.pid = pa.id and (pa.SMO is null or pa.SMO not like '46%')
            //Join sank sa on sl.id = sa.slid
            //where sl.schet_id = {ObjHelper.GetAnonymousValue(schet, "ID")}" :
            //$@"
            //Select distinct sa.* From SLUCH sl
            //Join sank sa on sl.id = sa.slid
            //where sl.SCHET_OMS_ID = {ObjHelper.GetAnonymousValue(schet, "CODE")} and (sl.vopl is null or sl.vopl = 1)", SprClass.LocalConnectionString);

            decimal? sumInTer = null;
            //if (SprClass.ProdSett.OrgTypeStatus == OrgType.Tfoms)
            {
                var sumInTerQuery = Reader2List.CustomAnonymousSelect(
                        $@"
                Select sum(tt.SUMV) as SUMV from 
				(Select distinct sl.SUMV From SLUCH sl
            Join D3_PACIENT_OMS pa on sl.pid = pa.id and(pa.SMO is null or pa.SMO not like '46%')
            where sl.schet_id = {ObjHelper.GetAnonymousValue(schet, "ID")} 
			union
			Select distinct zsl.SUMV From D3_ZSL_OMS zsl
            Join D3_PACIENT_OMS pa on zsl.D3_PID = pa.id and(pa.SMO is null or pa.SMO not like '46%')
            where zsl.D3_SCID = {ObjHelper.GetAnonymousValue(schet, "ID")} 
			) as tt
            ", SprClass.LocalConnectionString);
                sumInTer = (decimal?)ObjHelper.GetAnonymousValue(((IList)sumInTerQuery)[0], "SUMV");
            }


            var isWriter3 = false;

            int nzap1 = 1;
            int nzap3 = 1;
            int idcase1 = 1;
            int idcase3 = 1;
            XmlWriterSettings writerSettings
                = new XmlWriterSettings()
                {
                    Encoding = Encoding.GetEncoding("windows-1251"),
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = Environment.NewLine,
                    ConformanceLevel = ConformanceLevel.Document
                };
            using (ZipFile zip = new ZipFile())
            {
                var stream1 = new MemoryStream();
                var stream2 = new MemoryStream();
                var stream3 = new MemoryStream();

                using (XmlWriter writer1 = XmlWriter.Create(stream1, writerSettings))
                {
                    using (XmlWriter writer2 = XmlWriter.Create(stream2, writerSettings))
                    {
                        using (XmlWriter writer3 = XmlWriter.Create(stream3, writerSettings))
                        {
                            // writer1
                            writer1.WriteStartElement("ZL_LIST");
                            writer1.WriteStartElement("ZGLV");
                            writer1.WriteElementString("VERSION", "3.0");
                            writer1.WriteElementString("DATA", DateTime.Now.Date.ToString("yyyy-MM-dd"));
                            writer1.WriteElementString("FILENAME", filename1);
                            writer1.WriteElementString("SD_Z", zsluchList.Count().ToString());
                            writer1.WriteEndElement();
                            writer1.WriteStartElement("D3_SCHET_OMS");
                            if (ObjHelper.GetAnonymousValue(schet, "CODE") != null) writer1.WriteElementString("CODE", Convert.ToString(ObjHelper.GetAnonymousValue(schet, "CODE")));
                            else writer1.WriteElementString("CODE", ObjHelper.GetAnonymousValue(schet, "ID").ToString());
                            if (ObjHelper.GetAnonymousValue(schet, "CODE_MO") != null) writer1.WriteElementString("CODE_MO", (string)ObjHelper.GetAnonymousValue(schet, "CODE_MO"));
                            if (ObjHelper.GetAnonymousValue(schet, "YEAR") != null) writer1.WriteElementString("YEAR", Convert.ToString(ObjHelper.GetAnonymousValue(schet, "YEAR")));
                            if (ObjHelper.GetAnonymousValue(schet, "MONTH") != null) writer1.WriteElementString("MONTH", Convert.ToString(ObjHelper.GetAnonymousValue(schet, "MONTH")));
                            if (ObjHelper.GetAnonymousValue(schet, "NSCHET") != null) writer1.WriteElementString("NSCHET", (string)ObjHelper.GetAnonymousValue(schet, "NSCHET"));
                            if (ObjHelper.GetAnonymousValue(schet, "DSCHET") != null) writer1.WriteElementString("DSCHET", ((DateTime)ObjHelper.GetAnonymousValue(schet, "DSCHET")).Date.ToString("yyyy-MM-dd"));
                            if (ObjHelper.GetAnonymousValue(schet, "PLAT") != null) writer1.WriteElementString("PLAT", (string)ObjHelper.GetAnonymousValue(schet, "PLAT"));
                            writer1.WriteElementString("SUMMAV", Convert.ToString(sumInTer).Replace(",", "."));
                            if (ObjHelper.GetAnonymousValue(schet, "COMENTS") != null) writer1.WriteElementString("COMENTS", (string)ObjHelper.GetAnonymousValue(schet, "COMENTS"));
                            if (ObjHelper.GetAnonymousValue(schet, "SUMMAP") != null && (SprClass.ProdSett.OrgTypeStatus == OrgType.Smo || SprClass.ProdSett.OrgTypeStatus == OrgType.Tfoms)) writer1.WriteElementString("SUMMAP", Convert.ToString(ObjHelper.GetAnonymousValue(schet, "SUMMAP")).Replace(",", "."));
                            if (ObjHelper.GetAnonymousValue(schet, "SANK_MEK") != null && (SprClass.ProdSett.OrgTypeStatus == OrgType.Smo || SprClass.ProdSett.OrgTypeStatus == OrgType.Tfoms)) writer1.WriteElementString("SANK_MEK", Convert.ToString(ObjHelper.GetAnonymousValue(schet, "SANK_MEK")).Replace(",", "."));
                            if (ObjHelper.GetAnonymousValue(schet, "SANK_MEE") != null && (SprClass.ProdSett.OrgTypeStatus == OrgType.Smo || SprClass.ProdSett.OrgTypeStatus == OrgType.Tfoms)) writer1.WriteElementString("SANK_MEE", Convert.ToString(ObjHelper.GetAnonymousValue(schet, "SANK_MEE")).Replace(",", "."));
                            if (ObjHelper.GetAnonymousValue(schet, "SANK_EKMP") != null && (SprClass.ProdSett.OrgTypeStatus == OrgType.Smo || SprClass.ProdSett.OrgTypeStatus == OrgType.Tfoms)) writer1.WriteElementString("SANK_EKMP", Convert.ToString(ObjHelper.GetAnonymousValue(schet, "SANK_EKMP")).Replace(",", "."));
                            if ((string)ObjHelper.GetAnonymousValue(schet, "COMENTS") != null)
                            {
                                var com = (string)ObjHelper.GetAnonymousValue(schet, "COMENTS");
                                if (com.StartsWith("DP"))
                                    writer1.WriteElementString("DISP", "ДВ1");
                                if (com.StartsWith("DV"))
                                    writer1.WriteElementString("DISP", "ДВ2");
                                if (com.StartsWith("DO"))
                                    writer1.WriteElementString("DISP", "ОПВ");
                                if (com.StartsWith("DS"))
                                    writer1.WriteElementString("DISP", "ДС1");
                                if (com.StartsWith("DU"))
                                    writer1.WriteElementString("DISP", "ДС2");
                                if (com.StartsWith("DF"))
                                    writer1.WriteElementString("DISP", "ОН1");
                                if (com.StartsWith("DD"))
                                    writer1.WriteElementString("DISP", "ОН2");
                                if (com.StartsWith("DR"))
                                    writer1.WriteElementString("DISP", "ОН3");
                            }

                            writer1.WriteEndElement();
                            //
                            // writer2
                            writer2.WriteStartElement("PERS_LIST");
                            writer2.WriteStartElement("ZGLV");
                            writer2.WriteElementString("VERSION", "3.0");
                            writer2.WriteElementString("DATA", DateTime.Now.Date.ToString("yyyy-MM-dd"));
                            writer2.WriteElementString("FILENAME", filename2);
                            writer2.WriteElementString("FILENAME1", filename1);
                            writer2.WriteEndElement();
                            //

                            foreach (var zap in pacientList)
                            {
                                var pacWriter3 = false;
                                var pacWriter1 = false;

                                //writer2
                                writer2.WriteStartElement("PERS");
                                if (zap.ID_PAC != null) writer2.WriteElementString("ID_PAC", zap.ID_PAC);
                                if (zap.FAM != null) writer2.WriteElementString("FAM", (zap.FAM));
                                if (zap.IM != null) writer2.WriteElementString("IM", zap.IM);
                                if (zap.OT != null) writer2.WriteElementString("OT", zap.OT);
                                if (zap.W != null) writer2.WriteElementString("W", Convert.ToString(zap.W));
                                if (zap.DR != null) writer2.WriteElementString("DR", zap.DR.Value.ToString("yyyy-MM-dd"));
                                //if (zap.DOST != null) writer2.WriteElementString("DOST", zap.DOST.ToString());
                                if (zap.FAM_P != null) writer2.WriteElementString("FAM_P", zap.FAM_P);
                                if (zap.IM_P != null) writer2.WriteElementString("IM_P", zap.IM_P);
                                if (zap.OT_P != null) writer2.WriteElementString("OT_P", zap.OT_P);
                                if (zap.W_P != null) writer2.WriteElementString("W_P", zap.W_P.ToString());
                                if (zap.DR_P != null) writer2.WriteElementString("DR_P", zap.DR_P.Value.ToString("yyyy-MM-dd"));
                                if (zap.MR != null) writer2.WriteElementString("MR", zap.MR);
                                if (zap.DOCTYPE != null) writer2.WriteElementString("DOCTYPE", zap.DOCTYPE.ToString());
                                if (zap.DOCSER != null) writer2.WriteElementString("DOCSER", (zap.DOCSER));
                                if (zap.DOCNUM != null) writer2.WriteElementString("DOCNUM", (zap.DOCNUM));
                                if (zap.SNILS != null) writer2.WriteElementString("SNILS", zap.SNILS);
                                if (zap.OKATOG != null) writer2.WriteElementString("OKATOG", zap.OKATOG);
                                if (zap.OKATOP != null) writer2.WriteElementString("OKATOP", zap.OKATOP);
                                if (zap.COMENTP != null) writer2.WriteElementString("COMENTP", zap.COMENTP);
                                writer2.WriteEndElement();
                                //
                                foreach (var zslu in zsluchList.Where(x => x.D3_PID == zap.ID))
                                {
                                    if (pacWriter1 == false)
                                    {
                                        // writer1
                                        writer1.WriteStartElement("ZAP");
                                        writer1.WriteElementString("N_ZAP", Convert.ToString(nzap1++));
                                        writer1.WriteElementString("PR_NOV", "0");
                                        writer1.WriteStartElement("D3_PACIENT_OMS");
                                        if (zap.ID_PAC != null) writer1.WriteElementString("ID_PAC", zap.ID_PAC);
                                        if (zap.VPOLIS != null) writer1.WriteElementString("VPOLIS", Convert.ToString(zap.VPOLIS));
                                        if (zap.SPOLIS != null) writer1.WriteElementString("SPOLIS", zap.SPOLIS);
                                        if (zap.NPOLIS != null) writer1.WriteElementString("NPOLIS", zap.NPOLIS);
                                        if (zap.SMO != null) writer1.WriteElementString("SMO", zap.SMO);
                                        if (zap.SMO_OGRN != null && zap.SMO == null) writer1.WriteElementString("SMO_OGRN", zap.SMO_OGRN);
                                        if (zap.SMO_OK != null && zap.SMO == null) writer1.WriteElementString("SMO_OK", zap.SMO_OK);
                                        if (zap.SMO_NAM != null && zap.SMO == null) writer1.WriteElementString("SMO_NAM", zap.SMO_NAM);
                                        if (zap.NOVOR != null) writer1.WriteElementString("NOVOR", zap.NOVOR);
                                        else writer1.WriteElementString("NOVOR", "0");
                                        writer1.WriteEndElement();
                                        pacWriter1 = true;
                                    }

                                    writer1.WriteStartElement("Z_SL");
                                    writer1.WriteElementString("IDCASE", idcase1++.ToString());
                                    writer1.WriteElementString("ZSL_ID", zslu.ZSL_ID.ToString());
                                    if (zslu.VIDPOM != null) writer1.WriteElementString("VIDPOM", zslu.VIDPOM.ToString());
                                    if (zslu.FOR_POM != null) writer1.WriteElementString("FOR_POM", zslu.FOR_POM.ToString());
                                    if (zslu.NPR_MO != null) writer1.WriteElementString("NPR_MO", zslu.NPR_MO);
                                    if (zslu.LPU != null) writer1.WriteElementString("LPU", zslu.LPU);
                                    else
                                        writer1.WriteElementString("LPU",
                                            (ObjHelper.GetAnonymousValue(schet, "CODE_MO").ToString()));
                                    if (zslu.VBR != null) writer1.WriteElementString("VBR", zslu.VBR.ToString());
                                    //if (!string.IsNullOrWhiteSpace(slu.NHISTORY)) writer1.WriteElementString("NHISTORY", slu.NHISTORY);
                                    //else writer1.WriteElementString("NHISTORY", slu.ID.ToString());
                                    if (zslu.DATE_Z_1 != null) writer1.WriteElementString("DATE_Z_1", zslu.DATE_Z_1.Value.ToString("yyyy-MM-dd"));
                                    if (zslu.DATE_Z_2 != null) writer1.WriteElementString("DATE_Z_2", zslu.DATE_Z_2.Value.ToString("yyyy-MM-dd"));
                                    if (zslu.RSLT_D != null) writer1.WriteElementString("RSLT_D", zslu.RSLT_D.ToString());
                                    if (zslu.KD_Z != null) writer1.WriteElementString("KD_Z", zslu.KD_Z.ToString());

                                    //foreach (var ds2 in ds2List.Where(x => x.SLID == slu.ID))
                                    //{
                                    //    if (ds2.DS != null) writer1.WriteElementString("DS2", ds2.DS);
                                    //}
                                    //foreach (var ds3 in ds3List.Where(x => x.SLID == slu.ID))
                                    //{
                                    //    if (ds3.DS != null) writer1.WriteElementString("DS3", ds3.DS);
                                    //}
                                    if (zslu.VNOV_M != null) writer1.WriteElementString("VNOV_M", zslu.VNOV_M.ToString());
                                    if (zslu.RSLT != null) writer1.WriteElementString("RSLT", zslu.RSLT.ToString());
                                    if (zslu.ISHOD != null) writer1.WriteElementString("ISHOD", zslu.ISHOD.ToString());
                                    if (zslu.OS_SLUCH           != null) writer1.WriteElementString("OS_SLUCH", zslu.OS_SLUCH.ToString());
                                    if (zslu.OS_SLUCH_REGION    != null) writer1.WriteElementString("OS_SLUCH_REGION", zslu.OS_SLUCH_REGION.ToString());
                                    if (zslu.VOZR               != null) writer1.WriteElementString("VOZR", zslu.VOZR);
                                    //if (zslu.VETERAN != null) writer1.WriteElementString("VETERAN", zslu.VETERAN.ToString());

                                    //if (zslu.VETERAN          !  = null && plat.StartsWith("57")) writer1.WriteElementString("CODE_KSG", slu.IDKSG.ToString());
                                    //if (zslu.WORK_STAT        !  = null && plat.StartsWith("57")) writer1.WriteElementString("CODE_KSG", slu.IDKSG.ToString());
                                    if (zslu.VB_P               != null) writer1.WriteElementString("VB_P", zslu.VB_P.ToString());

                                    if (zslu.IDSP == 33)
                                    {
                                        var xx = sluchList.Where(x => x.D3_ZSLID == zslu.ID).ToList();
                                    }
                                    foreach (var sl in sluchList.Where(x => x.D3_ZSLID == zslu.ID))
                                    {
                                        writer1.WriteStartElement("SL");
                                        writer1.WriteElementString("SL_ID", sl.SL_ID.ToString());
                                        
                                        if (sl.VID_HMP      != null) writer1.WriteElementString("VID_HMP", sl.VID_HMP.ToString());
                                        if (sl.METOD_HMP    != null) writer1.WriteElementString("METOD_HMP", sl.METOD_HMP.ToString());
                                        if (sl.USL_OK       != null) writer1.WriteElementString("USL_OK", sl.USL_OK.ToString());
                                        if (sl.LPU_1        != null) writer1.WriteElementString("LPU_1", sl.LPU_1.ToString());
                                        if (sl.PODR         != null) writer1.WriteElementString("PODR", sl.PODR.ToString());
                                        if (sl.PROFIL       != null) writer1.WriteElementString("PROFIL", sl.PROFIL.ToString());
                                        if (sl.DET          != null) writer1.WriteElementString("DET", sl.DET.ToString());
                                        if (sl.P_CEL        != null) writer1.WriteElementString("P_CEL", sl.P_CEL.ToString());
                                        if (sl.TAL_NUM != null) writer1.WriteElementString("TAL_NUM", sl.TAL_NUM.ToString());
                                        if (sl.TAL_D        != null) writer1.WriteElementString("TAL_D", sl.TAL_D.ToString());
                                        if (sl.TAL_P        != null) writer1.WriteElementString("TAL_P", sl.TAL_P.ToString());
                                                                                if (sl.NHISTORY != null) writer1.WriteElementString("NHISTORY", sl.NHISTORY);
                                        else writer1.WriteElementString("NHISTORY", sl.ID.ToString());
                                                                                if (sl.P_PER        != null) writer1.WriteElementString("P_PER", sl.P_PER.ToString());
                                        if (sl.DATE_1       != null) writer1.WriteElementString("DATE_1", sl.DATE_1.Value.ToString("yyyy-MM-dd"));
                                        if (sl.DATE_2       != null) writer1.WriteElementString("DATE_2", sl.DATE_2.Value.ToString("yyyy-MM-dd"));
                                        //if (sl.P_OTK        != null) writer1.WriteElementString("P_OTK", sl.P_OTK.ToString());
                                        if (sl.KD           != null) writer1.WriteElementString("KD", sl.KD.ToString());
                                        if (sl.DS0          != null) writer1.WriteElementString("DS0", sl.DS0.ToString());
                                        if (sl.DS1          != null) writer1.WriteElementString("DS1", sl.DS1.ToString());
                                        if (sl.DS1_PR       != null) writer1.WriteElementString("DS1_PR", sl.DS1_PR.ToString());
                                        if (sl.DN != null) writer1.WriteElementString("DN", sl.DN.ToString());

                                        foreach (var ds2 in ds2List.Where(x => x.SLID == sl.ID))
                                        {
                                            if (ds2.DS != null)
                                            {
                                                writer1.WriteStartElement("DS2_N");
                                                writer1.WriteElementString("DS2", ds2.DS);
                                                //DS2_PR
                                                writer1.WriteEndElement();
                                            }
                                        }
                                        //if (sl.NAZ          != null) writer1.WriteElementString("S_TIP", sl.sank.S_TIP.ToString());

                                        foreach (var ds3 in ds3List.Where(x => x.SLID == sl.ID))
                                        {
                                            if (ds3.DS != null) writer1.WriteElementString("DS3", ds3.DS);
                                        }
                                        //if (sl.DS2_N        != null) writer1.WriteElementString("S_TIP", sl.sank.S_TIP.ToString());
                                        if (sl.CODE_MES1    != null) writer1.WriteElementString("CODE_MES1", sl.CODE_MES1.ToString());
                                        if (sl.CODE_MES2    != null) writer1.WriteElementString("CODE_MES2", sl.CODE_MES2.ToString());
                                        if (sl.KSG_DKK      != null) writer1.WriteElementString("KSG_DKK", sl.KSG_DKK.ToString());
                                        if (sl.N_KSG != null)
                                        {
                                            writer1.WriteStartElement("KSG");
                                            if (sl.N_KSG != null) writer1.WriteElementString("N_KSG", sl.N_KSG.ToString());
                                            if (sl.KSG_PG != null) writer1.WriteElementString("KSG_PG", sl.KSG_PG.ToString());
                                            if (sl.SL_K != null) writer1.WriteElementString("SL_K", sl.SL_K.ToString());
                                            if (sl.IT_SL != null) writer1.WriteElementString("IT_SL", sl.IT_SL.ToString().Replace(",", "."));
                                                foreach (var kslp in kslpList.Where(x => x.SLID == sl.ID))
                                                {
                                                    writer1.WriteStartElement("SL_KOEF");
                                                    writer1.WriteElementString("IDSL", kslp.IDSL.ToString());
                                                    writer1.WriteElementString("Z_SL", kslp.Z_SL.ToString().Replace(",", "."));

                                                    writer1.WriteEndElement();
                                                }

                                            writer1.WriteEndElement();

    //                                        SL_KOEF IDSL    O N(4)    Номер коэффициента сложности лечения пациента   В соответствии с региональным справочником.
    //Z_SL    O   N(1.5)  Значение коэффициента сложности лечения пациента

                                        }
                                        if (sl.REAB         != null) writer1.WriteElementString("REAB", sl.REAB.ToString());
                                        if (sl.PRVS         != null) writer1.WriteElementString("PRVS", sl.PRVS.ToString());
                                        if (sl.VERS_SPEC    != null) writer1.WriteElementString("VERS_SPEC", sl.VERS_SPEC.ToString());
                                        if (sl.IDDOKT       != null) writer1.WriteElementString("IDDOKT", sl.IDDOKT.ToString());
                                        if (sl.ED_COL       != null) writer1.WriteElementString("ED_COL", sl.ED_COL.ToString().Replace(",", "."));
                                        if (sl.TARIF        != null) writer1.WriteElementString("TARIF", sl.TARIF.ToString().Replace(",", "."));
                                        if (sl.SUM_M        != null) writer1.WriteElementString("SUM_M", sl.SUM_M.ToString().Replace(",", "."));
                                        
                                        int idserv = 1;
                                        foreach (var usl in uslList.Where(x => x.D3_SLID == sl.ID))
                                        {
                                            writer1.WriteStartElement("USL");
                                            writer1.WriteElementString("IDSERV", idserv++.ToString());
                                            if (usl.LPU != null) writer1.WriteElementString("LPU", usl.LPU);
                                            if (usl.LPU_1       != null) writer1.WriteElementString("LPU_1", usl.LPU_1);
                                            if (usl.PODR        != null) writer1.WriteElementString("PODR", usl.PODR);
                                            if (usl.PROFIL      != null) writer1.WriteElementString("PROFIL", usl.PROFIL.ToString());
                                            if (usl.VID_VME     != null) writer1.WriteElementString("VID_VME", usl.VID_VME);
                                            if (usl.DET         != null) writer1.WriteElementString("DET", usl.DET.ToString());
                                            if (usl.DATE_IN     != null) writer1.WriteElementString("DATE_IN", usl.DATE_IN.Value.ToString("yyyy-MM-dd"));
                                            if (usl.DATE_OUT    != null) writer1.WriteElementString("DATE_OUT", usl.DATE_OUT.Value.ToString("yyyy-MM-dd"));
                                            if (usl.DS          != null) writer1.WriteElementString("DS", usl.DS);
                                            if (usl.CODE_USL    != null) writer1.WriteElementString("CODE_USL", usl.CODE_USL);
                                            if (usl.KOL_USL     != null) writer1.WriteElementString("KOL_USL", usl.KOL_USL.ToString().Replace(",", "."));
                                            if (usl.TARIF       != null) writer1.WriteElementString("TARIF", usl.TARIF.ToString().Replace(",", "."));
                                            if (usl.SUMV_USL    != null) writer1.WriteElementString("SUMV_USL", usl.SUMV_USL.ToString().Replace(",", "."));
                                            if (usl.PRVS        != null) writer1.WriteElementString("PRVS", usl.PRVS.ToString());
                                            if (usl.CODE_MD     != null) writer1.WriteElementString("CODE_MD", usl.CODE_MD);
                                            if (usl.P_OTK       != null) writer1.WriteElementString("P_OTK", usl.P_OTK.ToString());
                                            if (usl.NPL         != null) writer1.WriteElementString("NPL", usl.NPL.ToString());
                                            if (usl.COMENTU != null) writer1.WriteElementString("COMENTU", usl.COMENTU);

                                            writer1.WriteEndElement();
                                        }

                                        if (sl.COMENTSL != null) writer1.WriteElementString("COMENTSL", sl.COMENTSL.ToString());


                                        writer1.WriteEndElement();
                                    }

                                    if (zslu.IDSP               != null) writer1.WriteElementString("IDSP", zslu.IDSP.ToString());
                                    if (zslu.SUMV               != null) writer1.WriteElementString("SUMV", zslu.SUMV.ToString().Replace(",", "."));
                                    if (zslu.OPLATA             != null) writer1.WriteElementString("OPLATA", zslu.OPLATA.ToString().Replace(",", "."));
                                    if (zslu.SUMP               != null) writer1.WriteElementString("SUMP", zslu.SUMP.ToString().Replace(",", "."));
                                    if (zslu.SANK_IT            != null) writer1.WriteElementString("SANK_IT", zslu.SANK_IT.ToString().Replace(",", "."));

                                    //foreach (var sank in sankList.Where(x => x.SLID == slu.ID))
                                    //{
                                    //    writer1.WriteStartElement("SANK");
                                    //    writer1.WriteElementString("S_CODE", Guid.NewGuid().ToString());
                                    //    if (sank.S_SUM != null) writer1.WriteElementString("S_SUM", sank.S_SUM.ToString().Replace(",", "."));
                                    //    if (sank.S_TIP != null) writer1.WriteElementString("S_TIP", sank.S_TIP.ToString());
                                    //    if (sank.S_OSN != null) writer1.WriteElementString("S_OSN", sank.S_OSN);
                                    //    if (sank.S_DATE != null) writer1.WriteElementString("S_DATE", sank.S_DATE.Value.ToString("yyyy-MM-dd"));
                                    //    if (sank.S_COM != null) writer1.WriteElementString("S_COM", sank.S_COM);
                                    //    if (sank.S_IST != null) writer1.WriteElementString("S_IST", sank.S_IST.ToString());
                                    //    writer1.WriteEndElement();
                                    //}

                                    writer1.WriteEndElement();
                                    if (isWriter3)
                                    {
                                        writer3.WriteEndElement();
                                    }
                                }
                                if (pacWriter1 == true)
                                    writer1.WriteEndElement();
                                if (isWriter3)
                                {
                                    writer3.WriteEndElement();
                                }

                            }
                            if (isWriter3)
                            {
                                writer3.WriteEndElement();
                                writer3.Flush();
                            }
                            writer3.Close();
                        }
                        writer2.WriteEndElement();
                        writer2.Flush();
                        writer2.Close();
                    }
                    writer1.WriteEndElement();
                    writer1.Flush();
                    writer1.Close();
                }

                string result1 = Encoding.Default.GetString(stream1.ToArray());
                string result2 = Encoding.Default.GetString(stream2.ToArray());
                string result3 = Encoding.Default.GetString(stream3.ToArray());

                zip.AddEntry(filename1 + ".xml", result1);
                zip.AddEntry(filename2 + ".xml", result2);
                if (isWriter3)
                    zip.AddEntry(filename3 + ".xml", result3);
                if (isLoop)
                {
                    if (!Directory.Exists(Environment.CurrentDirectory + @"\Out"))
                        Directory.CreateDirectory(Environment.CurrentDirectory + @"\Out");

                    var zipFile = Environment.CurrentDirectory + @"\Out\" + filename1 + ".oms";
                    zip.Save(zipFile);
                }
                else
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        if (plat.StartsWith("57"))
                        {
                            var num = isWriter3 ? "3" : "2";
                            saveFileDialog.Filter = "ZIP File (*.zip)|*.zip";
                            saveFileDialog.FileName = filename1.Insert(1, num) + ".zip";
                        }
                        else
                        {
                            saveFileDialog.Filter = "OMS File (*.oms)|*.oms";
                            saveFileDialog.FileName = filename1 + ".oms";
                        }

                        bool? result = saveFileDialog.ShowDialog();
                        if (result == true)
                            zip.Save(saveFileDialog.FileName);
                    });
                }
            }
            //LoadingDecorator1.IsSplashScreenShown = false;
        }

        public static MemoryStream ExportOspToXml(object sankExpList, string ifn)
        {
            XmlWriterSettings writerSettings
                = new XmlWriterSettings()
                {
                    Encoding = Encoding.GetEncoding("windows-1251"),
                    Indent = true,
                    IndentChars = "     ",
                    NewLineChars = Environment.NewLine,
                    ConformanceLevel = ConformanceLevel.Document
                };

            MemoryStream ms = new MemoryStream();

            using (ZipFile zip = new ZipFile())
            {
                var stream1 = new MemoryStream();

                using (XmlWriter writer1 = XmlWriter.Create(stream1, writerSettings))
                {
                    // writer1
                    writer1.WriteStartElement("ELMEDICINE");
                    //writer1.WriteElementString("FNAME", "O" + ifn.Remove(0, 1));
                    //writer1.WriteElementString("FNAME_I", ifn);

                    List2Xml("SLUCH", (IEnumerable<dynamic>)sankExpList, writer1);

                    writer1.WriteEndElement();
                    writer1.Flush();
                    writer1.Close();
                }

                string result1 = Encoding.Default.GetString(stream1.ToArray());
                zip.AddEntry("O" + ifn.Remove(0, 1) + ".xml", result1);

                zip.Save(ms);
            }
            return ms;
        }

        private static void List2Xml(string startElement, IEnumerable<dynamic> objects, XmlWriter writer)
        {

            foreach (var exp in objects)
            {
                writer.WriteStartElement(startElement);
                var obj = exp.GetType().GetProperties();
                foreach (var property in obj)
                {
                    var name = property.Name;
                    string value = null;
                    var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    if (type == Type.GetType("System.String")) value = (string)property.GetValue(exp, null);
                    else if (type == Type.GetType("System.DateTime"))
                    {
                        var dv = property.GetValue(exp, null);
                        if (dv != null) value = ((DateTime)dv).ToString("yyyy-MM-dd");
                    }
                    else value = Convert.ToString(property.GetValue(exp, null), CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(value)) writer.WriteElementString(name, value);
                }
                writer.WriteEndElement();
            }
        }

        private void ImportOms_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "OMS File |*.oms;*.zip";


            var result = openFileDialog.ShowDialog();

            var rfile = openFileDialog.FileName;
            var omszfn = openFileDialog.SafeFileName;

            string zapfn = "";
            string persfn = "";
            var zapms = new MemoryStream();
            var persms = new MemoryStream();

            if (result != true) return;

            var sc = ObjHelper.ClassConverter<D3_SCHET_OMS>(DxHelper.GetSelectedGridRow(EconomyWindow11.gridControl));
            ((DevExpress.Xpf.Bars.BarButtonItem)sender).IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (ZipFile zip = ZipFile.Read(rfile))
                    {
                        foreach (ZipEntry zipEntry in zip)
                        {
                            if (zipEntry.FileName.StartsWith("HM") || zipEntry.FileName.StartsWith("D") ||
                                zipEntry.FileName.StartsWith("T"))
                            {
                                zapfn = zipEntry.FileName;
                                zipEntry.Extract(zapms);
                                zapms.Position = 0;
                            }

                            if (zipEntry.FileName.StartsWith("L"))
                            {
                                persfn = zipEntry.FileName;
                                zipEntry.Extract(persms);
                                persms.Position = 0;
                            }
                        }
                    }
                    //"0x" + BitConverter.ToString(arraytoinsert).Replace("-", "")
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        DXMessageBox.Show(ex.Message + Environment.NewLine + ex.InnerException?.Message);
                    });
                }


                var zapsr = new StreamReader(zapms, Encoding.Default);
                string zapxml = zapsr.ReadToEnd();
                zapms.Dispose();
                zapms.Close();

                var perssr = new StreamReader(persms, Encoding.Default);
                string persxml = perssr.ReadToEnd();
                persms.Dispose();
                persms.Close();

                if (string.IsNullOrEmpty(zapxml) || string.IsNullOrEmpty(persxml))
                {
                    //Reader2List.CustomExecuteQuery($@"Update DOX_SCHET SET DOX_STATUS=12 ", SprClass.LocalConnectionString);
                }
                else
                {
                    try
                    {
                        //                        var q = $@"

                        //BEGIN TRANSACTION;  
                        //BEGIN TRY

                        //    Declare @sc int
                        //    INSERT INTO D3_SCHET_OMS (ZAPXMLFILE, PERSXMLFILE, ZAPFILENAME, PERSFILENAME) VALUES (CAST('{zapxml.Replace("'", "")}' AS XML), CAST('{persxml.Replace("'", "")}' AS XML), '{zapfn}', '{persfn}')
                        //    Select @sc = SCOPE_IDENTITY()

                        //    EXEC p_oms_load_schet @sc
                        //    EXEC p_oms_load_pacient @sc
                        //    EXEC p_oms_load_zsl @sc
                        //    EXEC p_oms_load_sl @sc
                        //    EXEC p_oms_load_usl @sc

                        //END TRY
                        //BEGIN CATCH
                        //    IF @@TRANCOUNT > 0  
                        //        ROLLBACK TRANSACTION;  
                        //    --PRINT 'In catch block.';
                        //DECLARE
                        //   @ErMessage NVARCHAR(2048),
                        //   @ErSeverity INT,
                        //   @ErState INT

                        // SELECT
                        //   @ErMessage = ERROR_MESSAGE(),
                        //   @ErSeverity = ERROR_SEVERITY(),
                        //   @ErState = ERROR_STATE()

                        // RAISERROR (@ErMessage,
                        //             @ErSeverity,
                        //             @ErState )
                        //END CATCH;

                        //IF @@TRANCOUNT > 0  
                        //    COMMIT TRANSACTION;  
                        //";

                        var q = $@"
                            EXEC [dbo].[p_oms_load_all_newformat] '{zapxml.Replace("'", "")}', '{persxml.Replace("'", "")}', '{zapfn}', '{persfn}'
                        ";

                        //                        var q = $@"

                        //BEGIN TRANSACTION;  
                        //BEGIN TRY

                        //    Declare @sc int
                        //    Update D3_SCHET_OMS SET ZAPXMLFILE=CAST('{zapxml.Replace("'", "")}' AS XML), PERSXMLFILE=CAST('{persxml.Replace("'", "")}' AS XML), ZAPFILENAME='{zapfn}', PERSFILENAME='{persfn}' WHERE ID = {sc.ID}
                        //    Select @sc = {sc.ID}

                        //    --EXEC p_oms_load_schet @sc
                        //    EXEC p_oms_load_pacient @sc
                        //    EXEC p_oms_load_zsl @sc
                        //    EXEC p_oms_load_sl @sc
                        //    EXEC p_oms_load_usl @sc

                        //END TRY
                        //BEGIN CATCH
                        //    IF @@TRANCOUNT > 0  
                        //        ROLLBACK TRANSACTION;  
                        //    --PRINT 'In catch block.';
                        //DECLARE
                        //   @ErMessage NVARCHAR(2048),
                        //   @ErSeverity INT,
                        //   @ErState INT

                        // SELECT
                        //   @ErMessage = ERROR_MESSAGE(),
                        //   @ErSeverity = ERROR_SEVERITY(),
                        //   @ErState = ERROR_STATE()

                        // RAISERROR (@ErMessage,
                        //             @ErSeverity,
                        //             @ErState )
                        //END CATCH;

                        //IF @@TRANCOUNT > 0  
                        //    COMMIT TRANSACTION;  
                        //";
                        zapxml = null;
                        persxml = null;
                        Reader2List.CustomExecuteQuery(q, SprClass.LocalConnectionString);
                        q = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            DXMessageBox.Show(ex.Message + Environment.NewLine + ex.InnerException?.Message);
                        });
                        //Reader2List.CustomExecuteQuery($@"Update DOX_SCHET SET DOX_STATUS=13 WHERE ID = {id}", _connectionString);
                        //return;
                    }
                }
                zapsr.Dispose();
                zapsr.Close();
                perssr.Dispose();
                perssr.Close();


                //Console.WriteLine("Распакован " + id);

                GC.WaitForPendingFinalizers();
                GC.Collect();

            }).ContinueWith(x =>
            {
                EconomyWindow11.linqInstantFeedbackDataSource.Refresh();

                DXMessageBox.Show("Загрузка успешно завершена");

                ((DevExpress.Xpf.Bars.BarButtonItem)sender).IsEnabled = true;


            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var schets = DxHelper.GetSelectedGridRows(EconomyWindow11.gridControl)?.Select(x => ObjHelper.GetAnonymousValue(x, "ID")).ToArray();

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Печатные формы",
                MyControl = new StatisticReports(schets),
                IsCloseable = "True",
            });

        }

        private void LoadMis_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var sc = ObjHelper.ClassConverter<D3_SCHET_OMS>(DxHelper.GetSelectedGridRow(EconomyWindow11.gridControl));
                var idSchet = (int)ObjHelper.GetAnonymousValue(sc, "ID");
                var month = (int)ObjHelper.GetAnonymousValue(sc, "MONTH");
                var year = (int)ObjHelper.GetAnonymousValue(sc, "YEAR");
                var lpu = (string)ObjHelper.GetAnonymousValue(sc, "CODE_MO");
                StreamWriter file = new StreamWriter(Environment.CurrentDirectory + "\\run.cmd");
                file.Write("RegistryUpload.SlashScreenYamed.exe" + " -reestruploadYamed" + " -lpu=" + lpu + " -year=" + year + " -month=" + month + " -idSchet=" + idSchet);
                file.Close();
                ProcessStartInfo startInfo = new ProcessStartInfo(Environment.CurrentDirectory + "\\run.cmd");
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(startInfo).WaitForExit();
                if (Process.GetProcessesByName("RegistryUpload.SlashScreenYamed").Length == 0)
                {
                    DXMessageBox.Show("Данные загружены!");
                }
            }
            catch
            {
                DXMessageBox.Show("Не выбран счет для загрузки МИС");
            }
        }

        private void Ldbf_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            
            OpenFileDialog OF = new OpenFileDialog();
            //OF.Filter= "Файлы DBF (.dbf)|*.dbf";
            OF.Title = "Выберите любой файл в папке с DBF файлами";
            bool res = OF.ShowDialog().Value;
            if (res == true)
            {
                DirectoryInfo dir = new DirectoryInfo(OF.FileName.Replace($"{OF.SafeFileName}", ""));
                var spr = dir.GetFiles("*.dbf");

                foreach (var f in spr)
                {
                    if (f.Name.StartsWith("V"))
                    { }
                    else
                    {
                        //string ConnectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        DataTable dt = new DataTable();
                        using (Stream fos = File.Open(f.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            var dbf = new DotNetDBF.DBFReader(fos);
                            dbf.CharEncoding = Encoding.GetEncoding(866);
                            var cnt = dbf.RecordCount;
                            var fields = dbf.Fields;
                            for (int ii = 0; ii < fields.Count(); ii++)
                            {
                                DataColumn workCol = dt.Columns.Add(fields[ii].Name, fields[ii].Type);
                                if (workCol.DataType == typeof(string)) workCol.MaxLength = fields[ii].FieldLength;
                                workCol.AllowDBNull = true;
                                workCol.DefaultValue = DBNull.Value;
                            }

                            for (int ii = 0; ii < dbf.RecordCount; ii++)
                            {
                                var rtt = dbf.NextRecord();

                                if (rtt != null)
                                {
                                    for (int i = 0; i < rtt.Count(); i++)
                                    {
                                        if (rtt[i] == null)
                                        {
                                            rtt[i] = null;
                                        }
                                        else
                                        if (rtt[i].ToString() == "")
                                        {
                                            rtt[i] = null;
                                        }
                                    }
                                    dt.LoadDataRow(rtt, true);

                                }

                            }
                        }
                        string sqltable;
                        string sqltable1;
                        if (f.Name.Contains('_'))
                        {
                            sqltable = f.Name.Replace(".dbf", "").Replace(".DBF", "").Replace(".Dbf", "");
                            sqltable1 = f.Name.Replace(f.Name, "DBF37_" + f.Name.Substring(0, 2) + f.Name.Substring(f.Name.IndexOf('_'), f.Name.Length - f.Name.IndexOf('_') - 4));
                        }
                        else
                        {
                            sqltable = f.Name.Replace(".dbf", "").Replace(".DBF", "").Replace(".Dbf", "");
                            sqltable1 = f.Name.Replace(f.Name, "DBF37_" + f.Name.Substring(0, 2));
                        }
                        Reader2List.LoadFromTable<DataTable>(SprClass.LocalConnectionString, dt, sqltable1);
                    }
                }
                MessageBox.Show("Файлы успешно загружены в таблицы базы данных");
            }
            // здесь будет процедура
            Reader2List.CustomExecuteQuery($@"EXEC p_load_DBF37", SprClass.LocalConnectionString);
            //удаляем таблицы
            string ConnectionString1 = SprClass.LocalConnectionString;
            SqlConnection con = new SqlConnection(ConnectionString1);
            var database = con.Database;
            List<string> spr1 = new List<string>();
            SqlCommand com0 = new SqlCommand($@"select name from sys.tables where name like 'DBF37_%'", con);
            con.Open();
            SqlDataReader reader = com0.ExecuteReader();

            if (reader.HasRows == true)
            {
                while (reader.Read())
                {
                    spr1.Add(reader["name"].ToString());
                }
                con.Close();
                foreach (var table in spr1)
                {

                    SqlCommand com = new SqlCommand($@"DROP table {table}  ", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
                var command = new SqlCommand($@"declare @n nvarchar(50)
                set @n=(SELECT name FROM sysfiles WHERE filename LIKE '%LDF%')
                ALTER DATABASE [{database}] SET RECOVERY SIMPLE WITH NO_WAIT
                DBCC SHRINKFILE (@n , 0)
                ALTER DATABASE [{database}] SET RECOVERY FULL", con);
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Таблицы успешно удалены из базы данных, данные загружены");
            }
            else
            {
                MessageBox.Show("Таблиц DBF в базе не найдено");
            }
        }

        private void Lissl_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                DateTime WD = SprClass.WorkDate;
                DateTime now = SprClass.WorkDate;
                DateTime last;
                if (WD.Month == 12)
                {
                    last = new DateTime(WD.Year + 1, 1, 1).AddDays(-1);
                }
                else
                {
                    last = new DateTime(WD.Year, WD.Month + 1, 1).AddDays(-1);
                }


                D3_SCHET_OMS sch = new D3_SCHET_OMS();
                sch.CODE_MO = SprClass.ProdSett.OrgCode;
                sch.YEAR = SprClass.WorkDate.Year;
                sch.MONTH = SprClass.WorkDate.Month;
                sch.DSCHET = last;
                sch.SchetType = "H";

                sch.ID = Reader2List.ObjectInsertCommand("D3_SCHET_OMS", sch, "ID", SprClass.LocalConnectionString);


                string cstr = "Data Source=109.194.54.128;Initial Catalog=Yamed_LIS;User ID=sa;Password=Gbljh:100";
                var lis_pac = Reader2List.CustomSelect<D3_PACIENT_OMS_LIS>($@"select p.* from d3_pacient_oms p
join d3_zsl_oms z on p.id=z.d3_pid 
where z.lpu='{sch.CODE_MO}' and year(z.date_z_2)={sch.YEAR} and month(z.date_z_2)={sch.MONTH}", cstr);
                var lis_zsl = Reader2List.CustomSelect<D3_ZSL_OMS_LIS>($@"select z.* from d3_pacient_oms p
join d3_zsl_oms z on p.id=z.d3_pid 
where z.lpu='{sch.CODE_MO}' and year(z.date_z_2)={sch.YEAR} and month(z.date_z_2)={sch.MONTH}", cstr);
                var lis_sl = Reader2List.CustomSelect<D3_SL_OMS_LIS>($@"select s.* from d3_pacient_oms p
join d3_zsl_oms z on p.id=z.d3_pid 
join D3_SL_OMS s on z.id=d3_zslid
where z.lpu='{sch.CODE_MO}' and year(z.date_z_2)={sch.YEAR} and month(z.date_z_2)={sch.MONTH}", cstr);
                var lis_usl = Reader2List.CustomSelect<D3_USL_OMS_LIS>($@"select u.* from d3_pacient_oms p
join d3_zsl_oms z on p.id=z.d3_pid 
join D3_SL_OMS s on z.id=d3_zslid
join D3_USL_OMS u on s.id=u.D3_SLID
where z.lpu='{sch.CODE_MO}' and year(z.date_z_2)={sch.YEAR} and month(z.date_z_2)={sch.MONTH}", cstr);



                Reader2List.InsertFromTable<DataTable>($@"insert into d3_pacient_oms ([D3_SCID] ,[ID_PAC] ,[FAM] ,[IM] ,[OT] ,[W] ,[DR] ,[TEL] ,[FAM_P] ,[IM_P] ,[OT_P] ,[W_P] ,[DR_P] ,[MR] ,[DOCTYPE] ,[DOCSER]
      ,[DOCNUM] ,[SNILS] ,[OKATOG] ,[OKATOP] ,[COMENTP] ,[VPOLIS] ,[SPOLIS] ,[NPOLIS] ,[ST_OKATO] ,[SMO] ,[SMO_OGRN] ,[SMO_OK]
      ,[SMO_NAM] ,[INV] ,[MSE] ,[NOVOR]  ,[VNOV_D] ,[N_ZAP] ,[PR_NOV] ,[VETERAN] ,[WORK_STAT])
select 
{sch.ID},[ID_PAC] ,[FAM] ,[IM] ,[OT] ,[W] ,[DR] ,[TEL] ,[FAM_P] ,[IM_P] ,[OT_P] ,[W_P] ,[DR_P] ,[MR] ,[DOCTYPE] ,[DOCSER]
      ,[DOCNUM] ,[SNILS] ,[OKATOG] ,[OKATOP] ,[COMENTP] ,[VPOLIS] ,[SPOLIS] ,[NPOLIS] ,[ST_OKATO] ,[SMO] ,[SMO_OGRN] ,[SMO_OK]
      ,[SMO_NAM] ,[INV] ,[MSE] ,[NOVOR]  ,[VNOV_D] ,[N_ZAP] ,[PR_NOV] ,[VETERAN] ,[WORK_STAT]
from @dt", SprClass.LocalConnectionString, Reader2List.ToDataTable(lis_pac), true);

                Reader2List.InsertFromTable<DataTable>($@"insert into d3_zsl_oms ([ZSL_ID] ,[D3_PID] ,[D3_PGID] ,[D3_SCID]  ,[IDCASE] ,[VIDPOM] ,[FOR_POM] ,[NPR_MO] ,[LPU] ,[VBR] ,[DATE_Z_1] ,[DATE_Z_2] ,[P_OTK] ,[RSLT_D] ,[KD_Z] ,[VNOV_M] ,[RSLT]
      ,[ISHOD] ,[OS_SLUCH] ,[OS_SLUCH_REGION] ,[VOZR] ,[VB_P] ,[IDSP] ,[SUMV] ,[OPLATA] ,[SUMP] ,[SANK_IT] ,[MEK_COMENT] ,[OSP_COMENT] ,[MEK_COUNT] ,[MEE_COUNT]  ,[EKMP_COUNT]
      ,[MTR] ,[USL_OK] ,[P_CEL] ,[EXP_COMENT] ,[EXP_TYPE] ,[EXP_DATE] ,[USERID] ,[ReqID] ,[USER_COMENT] ,[NPR_DATE] ,[DTP] ,[T_ARRIVAL] ,[N_ZAP] ,[PR_NOV] ,[St_IDCASE])
select 
[ZSL_ID] ,[D3_PID] ,[D3_PGID] , {sch.ID} ,[IDCASE] ,[VIDPOM] ,[FOR_POM] ,[NPR_MO] ,[LPU] ,[VBR] ,[DATE_Z_1] ,[DATE_Z_2] ,[P_OTK] ,[RSLT_D] ,[KD_Z] ,[VNOV_M] ,[RSLT]
      ,[ISHOD] ,[OS_SLUCH] ,[OS_SLUCH_REGION] ,[VOZR] ,[VB_P] ,[IDSP] ,[SUMV] ,[OPLATA] ,[SUMP] ,[SANK_IT] ,[MEK_COMENT] ,[OSP_COMENT] ,[MEK_COUNT] ,[MEE_COUNT]  ,[EKMP_COUNT]
      ,[MTR] ,[USL_OK] ,[P_CEL] ,[EXP_COMENT] ,[EXP_TYPE] ,[EXP_DATE] ,[USERID] ,[ReqID] ,[USER_COMENT] ,[NPR_DATE] ,[DTP] ,[T_ARRIVAL] ,[N_ZAP] ,[PR_NOV] ,[St_IDCASE]
from @dt", SprClass.LocalConnectionString, Reader2List.ToDataTable(lis_zsl), true);

                Reader2List.InsertFromTable<DataTable>($@"insert into d3_sl_oms ([D3_ZSLID] ,[D3_ZSLGID] ,[SL_ID] ,[USL_OK] ,[VID_HMP] ,[METOD_HMP] ,[LPU_1] ,[PODR] ,[PROFIL] ,[DET] ,[P_CEL] ,[TAL_NUM] ,[TAL_D] ,[TAL_P] ,[NHISTORY] ,[P_PER] ,[DATE_1] ,[DATE_2]
,[KD] ,[DS0] ,[DS1] ,[DS1_PR] ,[DN] ,[CODE_MES1] ,[CODE_MES2] ,[KSG_DKK] ,[N_KSG] ,[KSG_PG] ,[SL_K] ,[IT_SL] ,[REAB] ,[PRVS] ,[VERS_SPEC] ,[IDDOKT] ,[ED_COL] ,[TARIF] ,[SUM_M]
,[COMENTSL] ,[PROFIL_K] ,[PRVS21] ,[P_CEL25] ,[PRVS15] ,[PRVD] ,[C_ZAB] ,[DS_ONK] ,[St_IDCASE] ,[POVOD] ,[PROFIL_REG] ,[GRAF_DN] ,[KSKP] ,[VID_VIZ]
,[VID_BRIG]  ,[OMI_INVOICES_EV_ID])
select 
[D3_ZSLID] ,[D3_ZSLGID] ,[SL_ID] ,[USL_OK] ,[VID_HMP] ,[METOD_HMP] ,[LPU_1] ,[PODR] ,[PROFIL] ,[DET] ,[P_CEL] ,[TAL_NUM] ,[TAL_D] ,[TAL_P] ,[NHISTORY] ,[P_PER] ,[DATE_1] ,[DATE_2]
,[KD] ,[DS0] ,[DS1] ,[DS1_PR] ,[DN] ,[CODE_MES1] ,[CODE_MES2] ,[KSG_DKK] ,[N_KSG] ,[KSG_PG] ,[SL_K] ,[IT_SL] ,[REAB] ,[PRVS] ,[VERS_SPEC] ,[IDDOKT] ,[ED_COL] ,[TARIF] ,[SUM_M]
,[COMENTSL] ,[PROFIL_K] ,[PRVS21] ,[P_CEL25] ,[PRVS15] ,[PRVD] ,[C_ZAB] ,[DS_ONK] ,[St_IDCASE] ,[POVOD] ,[PROFIL_REG] ,[GRAF_DN] ,[KSKP] ,[VID_VIZ]
,[VID_BRIG]  ,[OMI_INVOICES_EV_ID]
from @dt", SprClass.LocalConnectionString, Reader2List.ToDataTable(lis_sl), true);

                Reader2List.InsertFromTable<DataTable>($@"insert into d3_usl_oms ([D3_SLID] ,[D3_ZSLID] ,[D3_SLGID] ,[IDSERV] ,[LPU] ,[LPU_1] ,[PODR] ,[PROFIL] ,[VID_VME] ,[DET] ,[DATE_IN] ,[DATE_OUT] ,[P_OTK] ,[DS] ,[CODE_USL] ,[KOL_USL] ,
	  [TARIF] ,[SUMV_USL]  ,[PRVS] ,[CODE_MD] ,[NPL] ,[COMENTU] ,[PRVS15] ,[PRVS21] ,[VERS_SPEC] ,[PRVD] ,[DOP] ,[PP] ,[St_IDSERV] ,[KOD_SP] ,[FORMUL])
select 
[D3_SLID] ,[D3_ZSLID] ,[D3_SLGID] ,[IDSERV] ,[LPU] ,[LPU_1] ,[PODR] ,[PROFIL] ,[VID_VME] ,[DET] ,[DATE_IN] ,[DATE_OUT] ,[P_OTK] ,[DS] ,[CODE_USL] ,[KOL_USL] ,
	  [TARIF] ,[SUMV_USL]  ,[PRVS] ,[CODE_MD] ,[NPL] ,[COMENTU] ,[PRVS15] ,[PRVS21] ,[VERS_SPEC] ,[PRVD] ,[DOP] ,[PP] ,[St_IDSERV] ,[KOD_SP] ,[FORMUL]
from @dt", SprClass.LocalConnectionString, Reader2List.ToDataTable(lis_usl), true);

                Reader2List.CustomExecuteQuery($@"update z set d3_pid=p.id 
from d3_pacient_oms p
join d3_zsl_oms z on p.id_pac=z.d3_pgid 
where p.d3_scid={sch.ID} 

update s set d3_zslid=z.id
from d3_pacient_oms p
join d3_zsl_oms z on p.id_pac=z.d3_pgid 
join D3_SL_OMS s on z.zsl_id=d3_zslgid
where p.d3_scid={sch.ID} 

update u set d3_zslid=z.id, d3_slid=s.id
from d3_pacient_oms p
join d3_zsl_oms z on p.id_pac=z.d3_pgid 
join D3_SL_OMS s on z.zsl_id=d3_zslgid
join D3_USL_OMS u on s.sl_id=u.D3_SLGID
where p.d3_scid={sch.ID} ", SprClass.LocalConnectionString);

                DXMessageBox.Show("Данные загружены!");
            }
            catch
            {
                DXMessageBox.Show("Не удалось загрузить данные из ЛИС!!!");
            }
        }
        public static void InsertFromTable<T>(string com, string connectionString, DataTable dt, bool deltype)
        {
            string sqltype = "";


            foreach (DataColumn dc in dt.Columns)
            {
                //dt.Columns.Add(d.NAME,d.TYPE);
                string s;
                switch (dc.DataType.Name.ToString())
                {
                    case "Int32":
                        s = "int";
                        break;
                    case "String":
                        s = "nvarchar(500)";
                        break;
                    case "Guid":
                        s = "uniqueidentifier";
                        break;
                    case "Boolean":
                        s = "bit";
                        break;
                    case "Binary":
                        s = "varbinary(20)";
                        break;
                    case "DateTime":
                        s = "DateTime2";
                        break;
                    case "Decimal":
                        s = "numeric(10,2)";
                        break;
                    default:
                        s = dc.DataType.Name.ToString();
                        break;
                }

                sqltype = sqltype + dc.ColumnName + " " + s + ",";

            }
            sqltype = sqltype.Substring(0, sqltype.Length - 1);
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd0 = new SqlCommand($@" IF exists (select * from sys.table_types where name='ForUpdate_LIS')  
                                                 DROP TYPE dbo.ForUpdate_LIS   
                                                 CREATE TYPE ForUpdate_LIS AS TABLE ({sqltype})", con);

            SqlCommand cmd = new SqlCommand(com, con);

            var t = new SqlParameter("@dt", SqlDbType.Structured);
            t.TypeName = "dbo.ForUpdate_LIS";
            t.Value = dt;
            cmd.Parameters.Add(t);
            cmd.CommandTimeout = 0;
            con.Open();
            cmd0.ExecuteNonQuery();
            int str = cmd.ExecuteNonQuery();
            int isrt = str;
            con.Close();
        }
    }
}
