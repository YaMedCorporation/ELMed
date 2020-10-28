﻿using System;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting.Native;
using Ionic.Zip;
using Microsoft.Win32;
using Yamed.Control;
using Yamed.Control.Editors;
using Yamed.Core;
using Yamed.Entity;
using Yamed.Reports;
using Yamed.Server;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;

namespace Yamed.Oms
{
    /// <summary>
    /// Логика взаимодействия для EconomyWindow.xaml
    /// </summary>
    public partial class AktRegisterGrid : UserControl
    {
        public readonly LinqInstantFeedbackDataSource _linqInstantFeedbackDataSource;
        private readonly YamedDataClassesDataContext _edc;
        public AktRegisterGrid()
        {
            InitializeComponent();

            _edc = new YamedDataClassesDataContext()
            {
                ObjectTrackingEnabled = false,
                CommandTimeout = 0,
                Connection = {ConnectionString = SprClass.LocalConnectionString}
            };

            _linqInstantFeedbackDataSource = new LinqInstantFeedbackDataSource()
            {
                AreSourceRowsThreadSafe = false,
                KeyExpression = "ID"
            };

            gridControl1.DataContext = _linqInstantFeedbackDataSource;

            
            LpuEdit.DataContext = SprClass.medOrg;
            TypeMpEdit.DataContext = SprClass.SprTypeMp;
            SType2Edit.DataContext = SprClass.TypeExp2;
            UserEdit.DataContext = SprClass.YamedUsers;
            kol();
            if (SprClass.Region != "37" && SprClass.ProdSett.OrgTypeStatus == OrgType.Smo)
            {
                LoadXmlItem.IsVisible = false;
                UnloadXmlItem.IsVisible = false;
            }
            if (SprClass.Region != "39" && SprClass.ProdSett.OrgTypeStatus == OrgType.Smo)
            {
                UnloadXmlK.IsVisible = false;
            }
        }


        private void Control_OnUnloaded(object sender, RoutedEventArgs e)
        {
            _linqInstantFeedbackDataSource.Dispose();
        }

        private void Control_OnLoaded(object sender, RoutedEventArgs e)
        {

            _linqInstantFeedbackDataSource.QueryableSource = _edc.D3_AKT_REGISTR_OMS;

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate()
                {
                    gridControl1.ExpandGroupRow(-1);
                }));
        }



        private void GridControl_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {


        }


        private void RefreshItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            _linqInstantFeedbackDataSource.Refresh();
            kol();
        }

        private void AddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = new D3_AKT_REGISTR_OMS();
            item.USERID_NOTEDIT = SprClass.userId;

            var sprEditWindow = new UniSprEditControl("D3_AKT_REGISTR_OMS", item, false, SprClass.LocalConnectionString);
            var window = new DXWindow
            {
                ShowIcon = false,
                WindowStartupLocation = WindowStartupLocation.Manual,
                SizeToContent = SizeToContent.Height,
                Width = 600,
                Content = sprEditWindow,
                Title = "Новая запись"
            };

            window.ShowDialog();
            _linqInstantFeedbackDataSource.Refresh();
        }

        private void EditItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = (EconomyWindow)((TabElement)СommonСomponents.DxTabObject).MyControl;
            var row = DxHelper.GetSelectedGridRow(gridControl1);
            if (row == null) return;

            var sc = ObjHelper.ClassConverter<D3_AKT_REGISTR_OMS>(row);

            var sprEditWindow = new UniSprEditControl("D3_AKT_REGISTR_OMS", sc, true, SprClass.LocalConnectionString);
            var window = new DXWindow
            {
                ShowIcon = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.Height,
                Width = 600,
                Content = sprEditWindow,
                Title = "Редактирование"
            };

            window.ShowDialog();
            _linqInstantFeedbackDataSource.Refresh();
        }

        private void DelItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = (EconomyWindow)((TabElement)СommonСomponents.DxTabObject).MyControl;
            var row = ObjHelper.ClassConverter<D3_AKT_REGISTR_OMS>(DxHelper.GetSelectedGridRow(gridControl1));
            if (row == null) return;

            MessageBoxResult result = MessageBox.Show("Удалить акт за период " + row.PERIOD_EXP_NOTEDIT  + "\n" + SprClass.LpuList.Single(x => x.mcod == row.LPU).NameWithID + "?", "Удаление",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();


                bool isDel = false;
                LoadingDecorator1.IsSplashScreenShown = true;
                var delSchet = Task.Factory.StartNew(() =>
                {
                    {
                        try
                        {
                            isDel = true;
                            Reader2List.CustomExecuteQuery($@"DELETE FROM D3_SANK_OMS where D3_ARID = {row.ID} ",
                                SprClass.LocalConnectionString);
                            Reader2List.CustomExecuteQuery($@"DELETE FROM D3_AKT_REGISTR_OMS where ID = {row.ID} ",
                                SprClass.LocalConnectionString);
                            Reader2List.CustomExecuteQuery($@"DELETE FROM D3_REQ_OMS where D3_ARID = {row.ID} ",
                                SprClass.LocalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            isDel = false;
                            Dispatcher.BeginInvoke((Action) delegate() { ErrorGlobalWindow.ShowError(ex.Message); });
                        }
                    }
                });
                delSchet.ContinueWith(x =>
                {

                    if (isDel)
                    {
                        LoadingDecorator1.IsSplashScreenShown = false;

                        _linqInstantFeedbackDataSource.Refresh();
                        DXMessageBox.Show("Акт удален");
                    }
                }, uiScheduler);

            }
        }



        private int _gr = 1;
        private void GrItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (_gr == 1)
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(delegate()
                    {
                        gridControl1.ClearGrouping();

                        gridControl1.GroupBy("LPU");
                    }));
                _gr = 2;
            } else if (_gr == 2)
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(delegate()
                    {
                        gridControl1.ClearGrouping();

                        gridControl1.GroupBy("PERIOD_EXP_NOTEDIT");
                    }));
                _gr = 1;
            }
        }


        private void ScRegisterItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var row = ObjHelper.ClassConverter<D3_AKT_REGISTR_OMS>(DxHelper.GetSelectedGridRow(gridControl1));


            var rc = new SchetRegisterControl();
            rc.scVid.IsVisible = false;
            //rc.aktExp.IsVisible = false;
            rc.zapPD.IsVisible = false;
            //rc.reexpertise.IsVisible = true;
            rc.add_mek.IsVisible = false;
            rc.re_mek.IsVisible = false;
            rc.reexpertise.IsVisible = false;

            rc.add_mee.IsVisible = true;
            rc.add_ekmp.IsVisible = true;
            rc.mek.Content = "Экспертизы";
            rc._arid = row.ID;
            rc.SchetRegisterGrid1.BindAktExp(row.ID);
            rc.AddSlAkt.IsVisible = false;
            rc.RazdelAkt.IsVisible = true;
            rc.SchetRegisterGrid1.arid = new List<int> { row.ID };
            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Акт экспертиз " + row.PERIOD_EXP_NOTEDIT + " номер " + row.NUM_ACT + " от " + row.DATE_ACT?.ToShortDateString(), 
                MyControl = rc,
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }

      
       
        private void ExcelExportItem_OnItemClick(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            var c = new GridControl();
            c.EnableSmartColumnsGeneration = true;
            c.AutoGenerateColumns = AutoGenerateColumnsMode.AddNew;
            c.ItemsSource = Reader2List.CustomAnonymousSelect($@"
select distinct 
akt.ID as [ИД акта],
akt.PERIOD_EXP_NOTEDIT as [Период],
case when left(zs.lpu,2)= '37' then sc.COMENTS else f3.NameWithID end as [МО],
zs.ID as [ИД зак. случая],
pa.FAM as [Фамилия], 
pa.IM as [Имя], 
pa.OT as [Отчество], 
DR as [Дата рождения], 
NPOLIS as [Номер полиса], 
case when left(zs.lpu,2)= '37' then sc.COMENTS else f3.NameWithID end as [Медиц. орг.],
m1.NameWithID as [Диагноз осн.], 
v9.NameWithID as [Результат],
f6.NameWithID as [Тип экспертизы],
v6.NameWithID as [Условия оказания МП], 
NHISTORY as [Номер истории],
zs.MEE_COUNT as [МЭЭ кол-во], 
zs.EKMP_COUNT as [ЭКМП кол-во], 
SUMV as [Сумма выставленная], 
f5.NameWithId as [Оплата], 
SUMP as [Сумма принятая], 
S_SUM as [Сумма удержанная], 
S_SUM2 as [Сумма штрафа], 
sank.Name as [Пункт удержания], 
S_COM as [Комментарий экспертизы], 
S_DATE as [Дата экспертизы],
typ.NameWithID as [Тип помощи],
sa.kol_exp as [Кол-во экспертиз],
req.kol_zap as [Кол-во записей],
akt.COMMENT as [Комментарий к акту],
akt.COMMENT_EKON as [Комментарий экономиста],
akt.DATE_ACT as [Дата создания акта],
akt.NUM_ACT as [Номер акта],
akt.DBEG as [Дата запроса],
akt.DEND as [Дата закрытия акта],
akt.DATE_PMD as [Дата предоставления документа],
akt.DATE_PODPIS_SMO as [Дата подписания СМО],
akt.DATE_TO_MO as [Дата отправки],
akt.DATE_PODPIS_MO as [Дата подписания МО],
akt.DATE_MO_TO_SMO as [Дата возвр. подписанных актов из МО в СМО],
akt.DATE_OPL_SHTRAF as [Дата оплаты штрафа],
users.UserName as [Пользователь]
            from D3_ZSL_OMS zs	 
            join (select count(D3_ARID) as kol_exp,D3_ARID,D3_ZSLID,S_SUM,S_SUM2,S_DATE,MODEL_ID,S_TIP2,S_COM from D3_SANK_OMS group by D3_ARID,D3_ZSLID,S_SUM,S_SUM2,S_DATE,MODEL_ID,S_TIP2,S_COM) sa on sa.D3_ZSLID=zs.id
            join D3_SL_OMS sl on sl.d3_zslid=zs.id
            join D3_PACIENT_OMS pa on pa.ID = zs.D3_PID
            join D3_AKT_REGISTR_OMS akt on akt.id=sa.d3_arid
			join D3_SCHET_OMS sc on sc.ID=zs.D3_SCID
			join (select count(d3_arid) as kol_zap, D3_ARID,D3_ZSLID from D3_REQ_OMS group by D3_ARID,D3_ZSLID) req on req.D3_ARID=akt.ID
			left join Yamed_ExpSpr_Sank sank on sank.ID=sa.MODEL_ID and sank.DEND is null
			left join Yamed_Spr_TypeMP typ on typ.ID=akt.TYPE_MP
			left join Yamed_Users users on users.ID=akt.USERID_NOTEDIT
            left join f003 f3 on f3.mcod=zs.LPU
			left join V006 v6 on v6.IDUMP=zs.USL_OK
			left join v009 v9 on v9.IDRMP=zs.RSLT
			left join F005 f5 on f5.Id=zs.OPLATA
            left join m001_ksg m1 on m1.idds=sl.ds1 and ISDELETE<>1
			left join F006_NEW f6 on f6.IDVID=sa.S_TIP2 and f6.DATEEND is null
            where sa.D3_ARID in ({MyIds(gridControl1.GetSelectedRowHandles(), gridControl1)})", SprClass.LocalConnectionString);
            foreach (var col in c.Columns)
            {
                c.GroupBy(col.HeaderCaption.ToString());
                if (col.HeaderCaption.ToString() == "МО")
                {
                    break;
                }
            }
            if (saveFileDialog.ShowDialog() == true)
            c.View.ExportToXlsx(saveFileDialog.FileName);
        }

        private void kol()
        {
            var kol = Reader2List.CustomAnonymousSelect($@"Select D3_ARID,count(D3_ARID) as kol_zap from D3_REQ_OMS group by d3_arid", SprClass.LocalConnectionString);
            kolzap.DataContext = kol;
            var exp = Reader2List.CustomAnonymousSelect($@"Select D3_ARID,count(D3_ARID) as kol_exp from D3_SANK_OMS group by d3_arid", SprClass.LocalConnectionString);
            kolexp.DataContext = exp;
        }
        public static string MyIds(int[] ids, DevExpress.Xpf.Grid.GridControl grid)
        {

            string sg_rows = "";
            int[] rt = ids;
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = grid.GetCellValue(rt[i], "ID");
                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                sg_rows = sgr;
            }

            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            return sg_rows;


        }

        private void GridControl1_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            
            //var row = DxHelper.GetSelectedGridRow(gridControl1);
            if (gridControl1.GetSelectedRowHandles().Count() == 0)
            {
                sankGridControl.DataContext = null;
            }
            else
            {
                var _sankList =
    Reader2List.CustomAnonymousSelect($@"
             select distinct sa.ID,FAM, IM, OT, DR, NPOLIS, 
m1.NameWithID as DS1, 
f6.NameWithID as TypeExp,
case when left(zs.lpu,2)= '37' then sc.COMENTS else f3.NameWithID end as LPU, 
v6.NameWithID as USL_OK, 
v9.NameWithID as RSLT,
NHISTORY, 
akt.NUM_ACT, 
zs.MEE_COUNT, 
zs.EKMP_COUNT, 
SUMV, 
f5.NameWithId as OPLATA,
SUMP, S_SUM, S_SUM2, sank.name as S_OSN, S_COM, S_DATE
            from D3_SANK_OMS sa
            join D3_ZSL_OMS zs on sa.D3_ZSLID = zs.ID
            join D3_SL_OMS sl on sl.d3_zslid=zs.id
            join D3_PACIENT_OMS pa on pa.ID = zs.D3_PID
            join D3_AKT_REGISTR_OMS akt on akt.id=sa.d3_arid
            join D3_SCHET_OMS sc on sc.id=zs.d3_scid
            left join Yamed_ExpSpr_Sank sank on sank.ID=sa.MODEL_ID and sank.DEND is null
            left join v009 v9 on v9.IDRMP=zs.RSLT
            left join f003 f3 on f3.mcod=zs.LPU
			left join V006 v6 on v6.IDUMP=zs.USL_OK
			left join F005 f5 on f5.Id=zs.OPLATA
            left join m001_ksg m1 on m1.idds=sl.ds1 and ISDELETE<>1
			left join F006_NEW f6 on f6.IDVID=sa.S_TIP2 and f6.DATEEND is null
            where sa.D3_ARID in ({MyIds(gridControl1.GetSelectedRowHandles(), gridControl1)})", SprClass.LocalConnectionString);
                sankGridControl.DataContext = _sankList;
            }
            //var id = ObjHelper.GetAnonymousValue(row, "ID");
            //var sankList =
            //    Reader2List.CustomAnonymousSelect($@"
            //select sa.ID, FAM, IM, OT, DR, NPOLIS, NHISTORY, SUMV, OPLATA, SUMP, S_SUM, S_SUM2, S_OSN, S_COM, S_DATE
            //from D3_SANK_OMS sa
            //join D3_ZSL_OMS zs on sa.D3_ZSLID = zs.ID
            //join D3_SL_OMS sl on sl.d3_zslid=zs.id
            //join D3_PACIENT_OMS pa on pa.ID = zs.D3_PID
            //where sa.D3_ARID = {id}", SprClass.LocalConnectionString);

            //sankGridControl.DataContext = sankList;
        }

        private void ExExcelItem_ItemClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == true)
                sankGridControl.View.ExportToXlsx(saveFileDialog.FileName);
        }

        private void ReportItem_OnItemClick(object sender, ItemClickEventArgs e)
        {

            DxHelper.GetSelectedGridRowsAsync(ref gridControl1);
            bool isLoaded = false;
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
                //List<int> sc = new List<int>();
                var rows = DxHelper.GetSelectedGridRows(gridControl1);
                var sc = rows.Select(x => ObjHelper.GetAnonymousValue(x, "ID")).ToArray();
                //if (rows != null)
                //{
                //    foreach (var row in rows)
                //    {
                //        sc.Add((int)ObjHelper.GetAnonymousValue(row, "ID"));
                //    }
                //}

                СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
                {
                    Header = "Печатные формы",
                    MyControl = new StatisticReports(sc, 2000),
                    IsCloseable = "True",
                });


                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void SlAddItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var row = DxHelper.GetSelectedGridRow(gridControl1);
            if (row == null) return;

            var sc = ObjHelper.ClassConverter<D3_AKT_REGISTR_OMS>(row);
            var exp_date = ObjHelper.GetAnonymousValue(row,"DATE_ACT");
            var coment = ObjHelper.GetAnonymousValue(row, "COMMENT");
            object lpu_status = DxHelper.LoadedRows.GroupBy(x=>ObjHelper.GetAnonymousValue(x,"LPU")).Select(gr=>gr.Key).Contains(sc.LPU);
            if ((bool?)lpu_status == false)
            {
                DXMessageBox.Show("Медицинская организация выбранного акта не соответствует медицинской организации выбранных случаев");
            }
            else
            {
                List<D3_REQ_OMS> rlist = new List<D3_REQ_OMS>();
                var sluids = new List<int>();
                foreach (var rows in DxHelper.LoadedRows)
                {
                    sluids.Add((int)ObjHelper.GetAnonymousValue(rows, "ID"));
                }
                foreach (int slid in sluids.ToArray().Distinct())
                {
                    if (SprClass.Region != "25")
                    {
                        Reader2List.CustomExecuteQuery($@"update d3_zsl_oms set exp_date='{exp_date}' where id={slid}", SprClass.LocalConnectionString);
                        Reader2List.CustomExecuteQuery($@"update d3_zsl_oms set exp_coment='{coment}' where id={slid}", SprClass.LocalConnectionString);
                    }
                    var rq = new D3_REQ_OMS()
                    {
                        D3_ARID = sc.ID,
                        D3_ZSLID = slid
                    };
                    rlist.Add(rq);
                }
                Reader2List.AnonymousInsertCommand("D3_REQ_OMS", rlist, "ID", SprClass.LocalConnectionString);
                DXMessageBox.Show("Добавлено случаев в акт - " + rlist.Count);
                
            }
            ((DXWindow)this.Parent).Close();
        }

        private void UnloadXmlItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var rows = gridControl1.GetSelectedRowHandles();
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "OMS File (*.oms)|*.oms";
            saveFileDialog.FileName = "Acts.oms";

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                int cntacts = 0;
                foreach (var r in rows)
                {
                    if (r >= 0)
                    {
                        var sc = (int)gridControl1.GetCellValue(r, "ID");

                        var qxml = SqlReader.Select($@"
                    exec Export_to_mobile {sc}"
                        , SprClass.LocalConnectionString);
                        string result1 = "<?xml version=\"1.0\" encoding=\"windows-1251\"?>" + (string)qxml[0].GetValue("HM");
                        string result2 = "<?xml version=\"1.0\" encoding=\"windows-1251\"?>" + (string)qxml[0].GetValue("LM");
                        using (ZipFile zip = new ZipFile(Encoding.GetEncoding("windows-1251")))
                        {
                            zip.AddEntry((string)qxml[0].GetValue("hf_name") + sc.ToString() + ".xml", result1);
                            zip.AddEntry((string)qxml[0].GetValue("lf_name") + sc.ToString() + ".xml", result2);
                            string fnm = saveFileDialog.FileName.Replace("Acts.oms", (string)qxml[0].GetValue("hf_name") + sc.ToString() + ".oms");
                            zip.Save(fnm);
                            cntacts = cntacts + 1;
                        }
                    }
                }
                DXMessageBox.Show("Успешно выгружено актов: " + cntacts + "\n" +
                    " Папка выгрузки: " + saveFileDialog.FileName.Replace("\\Acts.oms", ""));
            }
        }

        private void LoadXmlItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();
            OF.Multiselect = true;
            bool? result = OF.ShowDialog();
            string[] zipfiles = OF.FileNames;
            if (result == true)
            {
                foreach (var f in zipfiles)
                {
                    using (var zf = new ZipFile(f))
                    {
                        zf.ExtractAll(f.Replace(".oms", ""));
                    }
                    var files_xml = Directory.GetFiles(f.Replace(".oms", ""));
                    XmlDocument s_xml = new XmlDocument();
                    s_xml.Load(files_xml[0]);
                    SqlReader.Select($@"exec Import_mobile_acts '{s_xml.InnerXml}'", SprClass.LocalConnectionString);

                    foreach (var fx in files_xml)
                    {
                        File.Delete(fx);
                    }
                    Directory.Delete(f.Replace(".oms", ""));
                }
                DXMessageBox.Show("Экспертизы успешно загружены.");
            }
        }

        private void UnloadXmlK_ItemClick(object sender, ItemClickEventArgs e)
        {
            var g = ObjHelper.GetAnonymousValue(DxHelper.GetSelectedGridRow(gridControl1), "PERIOD_EXP_NOTEDIT").ToString().Split('-');           
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ZIP File (*.zip)|*.zip";
            saveFileDialog.FileName = $@"RS39001T39_{g[0].Substring(2,2)+g[1]}1.zip";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                        var sc = (int)ObjHelper.GetAnonymousValue(DxHelper.GetSelectedGridRow(gridControl1), "ID");
                        var qxml = SqlReader.Select($@"
                    exec Export_sank_Kaliningrad {sc},'{saveFileDialog.SafeFileName}'"
                        , SprClass.LocalConnectionString);
                        string result1 = "<?xml version=\"1.0\" encoding=\"windows-1251\"?>" + (string)qxml[0].GetValue("sanks");
                        using (ZipFile zip = new ZipFile(Encoding.GetEncoding("windows-1251")))
                        {
                            zip.AddEntry(saveFileDialog.SafeFileName.Replace(".zip",".xml"), result1);
                            string fnm = saveFileDialog.FileName;
                            zip.Save(fnm);
                        }
            }
                DXMessageBox.Show("Успешно выгружено!");
            }
    }

}