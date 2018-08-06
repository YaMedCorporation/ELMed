using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.DataAccess.Sql;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using FastReport;
using Yamed.Control;
using Yamed.Core;
using Yamed.Server;

namespace Yamed.Reports
{
    /// <summary>
    /// Логика взаимодействия для StatisticReports.xaml
    /// </summary>
    public partial class StatisticReports : UserControl
    {
        private int _repType;
        private string _smo;
        private object[] _scs;
        public StatisticReports(int repType, string smo, object[] scs)
        {
            InitializeComponent();
            _repType = repType;
            _smo = smo;
            _scs = scs;
            GridControl1.DataContext = Reader2List.CustomAnonymousSelect($"Select * from YamedReports where convert(nvarchar(12), Reptype) like '{_repType.ToString().Substring(0,1)}%'", SprClass.LocalConnectionString);
        }


        XRDesignMdiController _mdiController;
        private void EditReport_OnClick(object sender, RoutedEventArgs e)
        {
            //// Create a design form and get its MDI controller.
            //XRDesignRibbonForm form = new XRDesignRibbonForm();


            //_mdiController = form.DesignMdiController;

            //_mdiController.DataSourceWizardSettings.SqlWizardSettings.EnableCustomSql = true;


            //// Handle the DesignPanelLoaded event of the MDI controller,
            //// to override the SaveCommandHandler for every loaded report.
            //_mdiController.DesignPanelLoaded +=
            //    new DesignerLoadedEventHandler(mdiController_DesignPanelLoaded);

            //// Open an empty report in the form.
            //var rep = new XtraReport();
            //var rl = (string)ObjHelper.GetAnonymousValue(_row, "Template");

            //if (!string.IsNullOrWhiteSpace(rl))
            //{
            //    MemoryStream stream = new MemoryStream();
            //    StreamWriter writer = new StreamWriter(stream);

            //    writer.Write(rl);
            //    writer.Flush();
            //    stream.Seek(0, SeekOrigin.Begin);

            //    rep.LoadLayout(stream);

            //    stream.Close();
            //    writer.Close();
            //}
            //else
            //{
            //    var p = new Parameter
            //    {
            //        Name = "ID",
            //        Type = typeof(int)
            //    };

            //    rep.Parameters.Add(p);
            //}

            //_mdiController.OpenReport(rep);

            //// Show the form.
            //form.ShowDialog();
            //_mdiController.ActiveDesignPanel?.CloseReport();


            var window = new DXWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new Reports.ReportControl(_row),
                Title = "Редактор отчетных форм",
                SizeToContent = SizeToContent.WidthAndHeight,
            };
            window.ShowDialog();
            GridControl1.DataContext = Reader2List.CustomAnonymousSelect($"Select * from YamedReports where convert(nvarchar(12), Reptype) like '{_repType.ToString().Substring(0, 1)}%'", SprClass.LocalConnectionString);

        }

        void mdiController_DesignPanelLoaded(object sender, DesignerLoadedEventArgs e)
        {
            XRDesignPanel panel = (XRDesignPanel)sender;
            panel.AddCommandHandler(new SaveCommandHandler(panel, _row));
        }

        private static object _row;
        private void GridControl1_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            _row = ((GridControl)sender).SelectedItem;
        }

        private void CreateDocument_OnClick(object sender, RoutedEventArgs e)
        {
            var ids = _scs.OfType<int>().ToArray();

            if (new int[] {101,102,103}.Contains((int)ObjHelper.GetAnonymousValue(_row, "RepType")))
            {
                var window = new DXWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new Reports.ParametrControl(ids, _row),
                    Title = "Расширенные параметры",
                    Width = 600,
                    Height = 300
                };
                window.ShowDialog();
            }
            else

                //// Invoke the Ribbon Print Preview form  
                //// and load the report document into it. 
                //PrintHelper.ShowRibbonPrintPreview(this, report);

                //// Invoke the Ribbon Print Preview form modally. 
                //PrintHelper.ShowRibbonPrintPreviewDialog(Control.WindowUtils.FindOwnerWindow(), GetReport());

                //// Invoke the standard Print Preview form  
                //// and load the report document into it. 
                //PrintHelper.ShowPrintPreview(this, new XtraReport1());

                //// Invoke the standard Print Preview form modally. 
                PrintHelper.ShowPrintPreviewDialog(Control.WindowUtils.FindOwnerWindow(), GetReport());

            //// Invoke the standard Print Preview form modally. 
            //PrintHelper.PrintDirect(GetReport());


        }

        private XtraReport GetReport()
        {

            XtraReport report = new XtraReport();
            var rl = (string)ObjHelper.GetAnonymousValue(_row, "Template");
            if (!string.IsNullOrWhiteSpace(rl))
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);

                writer.Write(rl);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                report.LoadLayout(stream);

                stream.Close();
                writer.Close();
            }

            //report.Parameters[0].Value = (int) _scs.First();
            //report.Parameters[1].Value = _smo;
            foreach (var p in report.Parameters)
            {
                if (p.Name == "ID")
                    p.Value = _scs.First();
                if (p.Name == "smo")
                    p.Value = _smo;
                //if (p.Name == "ReqID")
                //    p.Value = req;
                //if (p.Name == "s_dates")
                //    p.Value = dates;
                //if (p.Name == "IDS")
                //    p.Value = ids;
                if (p.Name == "user")
                    p.Value = SprClass.userId;
            }

            //((SqlDataSource)report.DataSource).ConnectionParameters = new MsSqlConnectionParameters(@"ELMEDSRV\ELMEDSERVER", "elmedicine", "sa", "12345678", MsSqlAuthorizationType.Windows);
            //((SqlDataSource) report.DataSource).ConnectionOptions.CommandTimeout = 0;
            ((SqlDataSource)report.DataSource).ConnectionOptions.DbCommandTimeout = 0;

            report.CreateDocument();
            return report;
        }


        private void AddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var rl = (string)ObjHelper.GetAnonymousValue(_row, "Template");


            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Дизайнер отчета Fast Report",
                MyControl = new FRDesignerControl(_row),
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });

        }

        private void ReportExportItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var ids = _scs.OfType<int>().ToArray();

            if (new int[] { 101, 102, 103 }.Contains((int)ObjHelper.GetAnonymousValue(_row, "RepType")))
            {
                var window = new DXWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new Reports.ParametrControl(ids, _row, true),
                    Title = "Расширенные параметры",
                    Width = 600,
                    Height = 300
                };
                window.ShowDialog();
            }
       }
    }
}
