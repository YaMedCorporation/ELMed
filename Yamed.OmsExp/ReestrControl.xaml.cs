using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using Microsoft.Win32;
using Yamed.Control;
using Yamed.Core;
using Yamed.OmsExp.ExpEditors;
using Yamed.OmsExp.MekEditor;
using Yamed.Server;
using PreviewControl = Yamed.Reports.PreviewControl;

namespace Yamed.OmsExp
{
    /// <summary>
    /// Логика взаимодействия для PacientReserveControl.xaml
    /// </summary>
    public partial class ReestrControl : UserControl
    {
        private bool _isSaved;
        private string _reqCmd;

        public ReestrControl()
        {
            InitializeComponent();
        }

        public ReestrControl(List<int> scids)
        {
            InitializeComponent();


            ElReestrTabNew11.Scids = scids;
            ElReestrTabNew11.BindDataZsl(scids);

            if (scids.Any())
            {
                var ids = "(";
                foreach (var sc in scids)
                {
                    ids += sc + ", ";
                }
                ids += ")";
                ids = ids.Replace(", )", ")");
                _reqCmd = $@"
Select distinct r.* from YamedRequests r
join D3_ZSL_OMS zsl on r.ID = zsl.ReqID
where zsl.D3_SCID in {ids}";
                ReqBind();
            }
        }

        void ReqBind()
        {
            if (_reqCmd == null) return;

            ReqGridControl.DataContext = Reader2List.CustomAnonymousSelect(_reqCmd, SprClass.LocalConnectionString);
        }

        private void ZslRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            ElReestrTabNew11._linqInstantFeedbackDataSource.Refresh();
        }


        private void ZslEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var tab = ElReestrTabNew11;
            //var sl = Reader2List.CustomSelect<SLUCH>($"Select * From D3_ZSL_OMS Where ID={ObjHelper.GetAnonymousValue(DxHelper.GetSelectedGridRow(tab.gridControl1), "ID")}",
            //        SprClass.LocalConnectionString).Single();

            var id = (int) ObjHelper.GetAnonymousValue(DxHelper.GetSelectedGridRow(tab.gridControl1), "ID");
            var slt = new SluchTemplateD3(ElReestrTabNew11.gridControl1);
            slt.BindSluch(id);

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Поликлиника",
                MyControl = slt,
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }


        private void SankDel_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref ElReestrTabNew11.gridControl1);
            bool isLoaded = false;
            ElReestrTabNew11.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(200);

                    if (isLoaded) break;

                    Dispatcher.BeginInvoke((Action) delegate()
                    {
                        if (ElReestrTabNew11.gridControl1.IsAsyncOperationInProgress == false)
                        {
                            isLoaded = true;
                        }
                    });
                }

            }).ContinueWith(lr =>
            {
                var sankGroupDelete = new SankGroupDelete(DxHelper.LoadedRows);
                sankGroupDelete.ShowDialog();
                ElReestrTabNew11.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void Req_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref ElReestrTabNew11.gridControl1);
            bool isLoaded = false;
            ElReestrTabNew11.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (ElReestrTabNew11.gridControl1.IsAsyncOperationInProgress == false)
                        {
                            isLoaded = true;
                        }
                    });
                    if (isLoaded) break;
                    Thread.Sleep(200);
                }

            }).ContinueWith(lr =>
            {
                bool isMek = false;
                var sluids = new List<int>();//gridControl1.SelectedItems.OfType<SluPacClass>().Select(x=>x.ID).ToArray();
                var rows = (IEnumerable<object>)DxHelper.LoadedRows;
                Parallel.ForEach(rows, (x) =>
                {
                    if ((int?)ObjHelper.GetAnonymousValue(x, "OPLATA") == 1 || SprClass.IsTfoms)
                    {
                        sluids.Add((int)ObjHelper.GetAnonymousValue(x, "ID"));
                    }
                    else
                    {
                        isMek = true;
                    }
                });
                
                if (!sluids.Any())
                {
                    DXMessageBox.Show("Не выбрано ни одной записи или записи имеют некорректный статус оплаты");
                    ElReestrTabNew11.gridControl1.IsEnabled = true;
                    DxHelper.LoadedRows.Clear();
                    return;
                }

                if (isMek)
                {
                    DXMessageBox.Show("Внимание выбраны записи с некорректным статусом оплаты, которые исключены из запроса первичной документации");
                }

                var window = new DXWindow
                {
                    ShowIcon = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new AktMeeChoose(sluids.ToArray()),
                    Title = "Запрос на первичную документацию",
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                window.ShowDialog();

                ElReestrTabNew11.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ReqDel_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Удалить выбранные запросы первичной документации?", "Удаление",
    MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            DxHelper.GetSelectedGridRowsAsync(ref ElReestrTabNew11.gridControl1);
            bool isLoaded = false;
            ElReestrTabNew11.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (ElReestrTabNew11.gridControl1.IsAsyncOperationInProgress == false)
                        {
                            isLoaded = true;
                        }
                    });
                    if (isLoaded) break;
                    Thread.Sleep(200);
                }

            }).ContinueWith(lr =>
            {
                StringBuilder cmd = new StringBuilder();
                foreach (var row in DxHelper.LoadedRows)
                {
                    cmd.AppendLine(
                        string.Format(
                            @"UPDATE D3_ZSL_OMS SET EXP_COMENT = NULL, EXP_TYPE = NULL, EXP_DATE = NULL, USERID = NULL WHERE ID = {0}",
                            ObjHelper.GetAnonymousValue(row, "ID")));
                }
                Reader2List.CustomExecuteQuery(cmd.ToString(), SprClass.LocalConnectionString);


                ElReestrTabNew11.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void AddMee_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref ElReestrTabNew11.gridControl1);
            bool isLoaded = false;
            ElReestrTabNew11.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                    while (true)
                    {
                        Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            if (ElReestrTabNew11.gridControl1.IsAsyncOperationInProgress == false)
                            {
                                isLoaded = true;
                            }
                        });
                        if (isLoaded) break;
                        Thread.Sleep(200);
                    }
            }).ContinueWith(lr =>
            {

                var window = new DXWindow
                {
                    ShowIcon = false,
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Content = new MeeWindow(2),
                    Title = "Акт МЭЭ"
                };
                window.ShowDialog();

                ElReestrTabNew11.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void AddEkmp_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref ElReestrTabNew11.gridControl1);
            bool isLoaded = false;
            ElReestrTabNew11.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (ElReestrTabNew11.gridControl1.IsAsyncOperationInProgress == false)
                        {
                            isLoaded = true;
                        }
                    });
                    if (isLoaded) break;
                    Thread.Sleep(200);
                }
            }).ContinueWith(lr =>
            {

                var window = new DXWindow
                {
                    ShowIcon = false,
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Content = new MeeWindow(3),
                    Title = "Акт ЭКМП"
                };
                window.ShowDialog();

                ElReestrTabNew11.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ReqAddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref ElReestrTabNew11.gridControl1);
            bool isLoaded = false;
            ElReestrTabNew11.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (ElReestrTabNew11.gridControl1.IsAsyncOperationInProgress == false)
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
                    if (DxHelper.LoadedRows.GroupBy(x => ObjHelper.GetAnonymousValue(x, "LPU")).Select(gr => gr.Key).Count() > 1)
                    {
                        DXMessageBox.Show("Документ можно создать только в рамках одной МО");
                    }
                    else
                    {
                        var window = new DXWindow
                        {
                            ShowIcon = false,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            Content = new ReqControl(),
                            Title = "Запрос на первичную документацию",
                            SizeToContent = SizeToContent.Height,
                            Width = 300
                        };
                        window.ShowDialog();
                        ReqBind();
                    }
                }
                ElReestrTabNew11.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ReqDelItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var ans = DXMessageBox.Show("Удалить запрос?", "Удаление", MessageBoxButton.YesNo);
            if (ans != MessageBoxResult.Yes) return;
            var sc = ReqGridControl.SelectedItem;

            Reader2List.CustomAnonymousSelect($@"DELETE [dbo].[YamedRequests] Where ID = {ObjHelper.GetAnonymousValue(sc, "ID")}", SprClass.LocalConnectionString);
            Reader2List.CustomExecuteQuery($@"UPDATE D3_ZSL_OMS SET ReqID = NULL WHERE ReqID = {ObjHelper.GetAnonymousValue(sc, "ID")}", SprClass.LocalConnectionString);
            ReqBind();
        }

        private void RepReqItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var yr = SqlReader.Select($"Select * from YamedReports where RepName = '_reqAkt'", SprClass.LocalConnectionString);
            var rl = (string)ObjHelper.GetAnonymousValue(yr[0], "Template");
            var sc = ReqGridControl.SelectedItem;

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Отчет",
                MyControl = new PreviewControl(rl, 0, (int)ObjHelper.GetAnonymousValue(sc, "ID")),
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }

        private void ExcelButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == true)
                ElReestrTabNew11.gridControl1.View.ExportToXlsx(saveFileDialog.FileName);
        }

        private void AddMek_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref ElReestrTabNew11.gridControl1);
            bool isLoaded = false;
            ElReestrTabNew11.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (ElReestrTabNew11.gridControl1.IsAsyncOperationInProgress == false)
                        {
                            isLoaded = true;
                        }
                    });
                    if (isLoaded) break;
                    Thread.Sleep(200);
                }
            }).ContinueWith(lr =>
            {

                var window = new DXWindow
                {
                    ShowIcon = false,
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Content = new SankControl(true),
                    Title = "Акт ЭКМП"
                };
                window.ShowDialog();

                ElReestrTabNew11.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void RepReReqItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var yr = SqlReader.Select($"Select * from YamedReports where RepName = '_req2Akt'", SprClass.LocalConnectionString);
            var rl = (string)ObjHelper.GetAnonymousValue(yr[0], "Template");
            var sc = ReqGridControl.SelectedItem;

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Отчет",
                MyControl = new PreviewControl(rl, 0, (int)ObjHelper.GetAnonymousValue(sc, "ID")),
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }

        private void RepReqCustomItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var yr = SqlReader.Select($"Select * from YamedReports where RepName = '_reqCustomAkt'", SprClass.LocalConnectionString);
            var rl = (string)ObjHelper.GetAnonymousValue(yr[0], "Template");
            var sc = ReqGridControl.SelectedItem;

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Отчет",
                MyControl = new PreviewControl(rl, 0, (int)ObjHelper.GetAnonymousValue(sc, "ID")),
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }

        private void ReqEditItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
