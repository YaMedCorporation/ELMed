using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using Yamed.Control;
using Yamed.Core;
using Yamed.Server;

namespace Yamed.Reports
{
    /// <summary>
    /// Логика взаимодействия для ParametrControl.xaml
    /// </summary>
    public partial class ParametrControl : UserControl
    {
        private readonly int[] _sc;
        private static object _row;
        private bool _isExport;

        public ParametrControl(int[] sc, object row, bool isExport = false)
        {
            InitializeComponent();

            _isExport = isExport;
            _sc = sc;
            _row = row;

            DateSankGenerate();
        }

        private void DateSankGenerate()
        {
            int st = 0;
            var type = (int) ObjHelper.GetAnonymousValue(_row, "RepType");
            switch (type)
            {
                case 101:
                    st = 1;
                    break;
                case 102:
                    st = 2;
                    break;
                case 103:
                    st = 3;
                    break;
            }



            var dates = Reader2List.CustomAnonymousSelect($@"
Select S_DATE, convert(nvarchar(10), S_DATE, 104) S_DATE_RUS FROM D3_SANK_OMS
WHERE D3_SCID in ({GetIds(_sc)}) and S_TIP = {st}
group by S_DATE
ORDER BY S_DATE", SprClass.LocalConnectionString);

            DateListBoxEdit.DataContext = dates;
            DateListBoxEdit.SelectedIndex = ((IList) dates).Count - 1;
        }

        string GetStringOfDates(ObservableCollection<object> collection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in collection.Select(x => ObjHelper.GetAnonymousValue(x, "S_DATE")))
            {
                //sb.Append("'");
                sb.Append(((DateTime) item).ToString("yyyyMMdd"));
                //sb.Append("'");
                sb.Append(",");
            }

            var dates = sb.ToString();
            dates = dates.Remove(dates.Length - 1);
            return dates;
        }

        string GetIds(int[] collection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in collection)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rl = (string)ObjHelper.GetAnonymousValue(_row, "Template");
            var rn = (string)ObjHelper.GetAnonymousValue(_row, "RepName");
            var dates = DateListBoxEdit.SelectedItems.Any() ? GetStringOfDates(DateListBoxEdit.SelectedItems) : null;

            if (!_isExport)
            {
                СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
                {
                    Header = "Отчет",
                    MyControl = new PreviewControl(rl, _sc.First(), dates: dates, ids: GetIds(_sc)),
                    IsCloseable = "True",
                    //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
                });
            }
            else
            {
                foreach (var sc in _sc)
                {
                    if (!string.IsNullOrWhiteSpace(rl))
                    {
                        MemoryStream stream = new MemoryStream();
                        StreamWriter writer = new StreamWriter(stream);

                        writer.Write(rl);
                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        var report = XtraReport.FromStream(stream, true);

                        stream.Close();
                        writer.Close();
                        var subReps = report.AllControls<XRSubreport>();

                        foreach (var sr in subReps)
                        {
                            var row = SqlReader.Select($"Select * from YamedReports where RepName = '{sr.Name}'",
                                SprClass.LocalConnectionString);
                            var t = (string) row[0].GetValue("Template");

                            MemoryStream ms = new MemoryStream();
                            StreamWriter sw = new StreamWriter(ms);

                            sw.Write(t);
                            sw.Flush();
                            ms.Seek(0, SeekOrigin.Begin);

                            var rep = XtraReport.FromStream(ms, true);
                            ((SqlDataSource) rep.DataSource).ConnectionOptions.DbCommandTimeout = 0;

                            sr.ReportSource = rep;

                            ms.Close();
                            sw.Close();
                        }


                        var fn =
                            SqlReader.Select(
                                $"SELECT [CODE_MO], [YEAR], [MONTH] FROM [dbo].[D3_SCHET_OMS] WHERE ID={sc}",
                                SprClass.LocalConnectionString);

                        foreach (var p in report.Parameters)
                        {
                            if (p.Name == "ID")
                                p.Value = sc;
                            if (p.Name == "s_dates")
                                p.Value = dates;
                            if (p.Name == "user")
                                p.Value = SprClass.userId;
                        }

                        if (report.DataSource != null)
                            ((SqlDataSource)report.DataSource).ConnectionOptions.DbCommandTimeout = 0;

                        //report.ExportToPdf($@"D:\out\{(string)fn[0].GetValue("CODE_MO") + "_" + rn + "_" + fn[0].GetValue("YEAR") + fn[0].GetValue("MONTH")}" + ".pdf");
                        report.ExportToRtf(
                            $@"D:\out\{(string) fn[0].GetValue("CODE_MO") + "_" + rn + "_" + fn[0].GetValue("YEAR") +
                                       fn[0].GetValue("MONTH")}" + ".rtf");

                    }
                }
            }
        }
    }
}
