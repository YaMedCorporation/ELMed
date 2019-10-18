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
using Yamed.Emr;
using Yamed.OmsExp.ExpEditors;
using Yamed.Reports;
using Yamed.Server;
using PreviewControl = Yamed.Reports.PreviewControl;

namespace Yamed.Oms
{
    /// <summary>
    /// Логика взаимодействия для PacientReserveControl.xaml
    /// </summary>
    public partial class SchetRegisterControl : UserControl
    {
        private bool _isSaved;
        private string _reqCmd;

        public SchetRegisterControl()
        {
            InitializeComponent();
        }

        private List<int> _scids;
        public SchetRegisterControl(List<int> scids)
        {
            InitializeComponent();
            _scids = scids;

            SchetRegisterGrid1.Scids = scids;
            SchetRegisterGrid1.BindDataZsl();

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

                ZaprosCount();
            }
        }

        void ReqBind()
        {
            if (_reqCmd == null) return;

            ReqGridControl.DataContext = Reader2List.CustomAnonymousSelect(_reqCmd, SprClass.LocalConnectionString);
        }

        private void ZslRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            SchetRegisterGrid1._linqInstantFeedbackDataSource.Refresh();
        }


        //private void ZslEdit_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var tab = SchetRegisterGrid1;
        //    //var sl = Reader2List.CustomSelect<SLUCH>($"Select * From D3_ZSL_OMS Where ID={ObjHelper.GetAnonymousValue(DxHelper.GetSelectedGridRow(tab.gridControl1), "ID")}",
        //    //        SprClass.LocalConnectionString).Single();

        //    var id = (int) ObjHelper.GetAnonymousValue(DxHelper.GetSelectedGridRow(tab.gridControl1), "ID");
        //    var slt = new SluchTemplateD3(SchetRegisterGrid1.gridControl1);
        //    slt.BindSluch(id);

        //    СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
        //    {
        //        Header = "Карта пациента",
        //        MyControl = slt,
        //        IsCloseable = "True",
        //        //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
        //    });
        //}


        private void SankDel_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(200);

                    if (isLoaded) break;

                    Dispatcher.BeginInvoke((Action) delegate()
                    {
                        if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
                        {
                            isLoaded = true;
                        }
                    });
                }

            }).ContinueWith(lr =>
            {
                var sankGroupDelete = new SankGroupDelete(DxHelper.LoadedRows);
                sankGroupDelete.ShowDialog();
                SchetRegisterGrid1.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void Req_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
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
                foreach (var row in DxHelper.LoadedRows)
                // Parallel.ForEach(rows, (x) =>
                {
                    if ((int?)ObjHelper.GetAnonymousValue(row, "OPLATA") == 1 || SprClass.ProdSett.OrgTypeStatus == OrgType.Tfoms)
                    {
                        sluids.Add((int)ObjHelper.GetAnonymousValue(row, "ID"));
                    }
                    else
                    {
                        isMek = true;
                    }
                }//});
                
                if (!sluids.Any())
                {
                    DXMessageBox.Show("Не выбрано ни одной записи или записи имеют некорректный статус оплаты");
                    SchetRegisterGrid1.gridControl1.IsEnabled = true;
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

                SchetRegisterGrid1.gridControl1.IsEnabled = true;
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

            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
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


                SchetRegisterGrid1.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void AddMee_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                    while (true)
                    {
                        Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
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

                SchetRegisterGrid1.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void AddEkmp_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
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

                SchetRegisterGrid1.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AddRMee_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
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
                    Content = new MeeWindow(2, re: 1),
                    Title = "Акт МЭЭ"
                };
                window.ShowDialog();

                SchetRegisterGrid1.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void AddREkmp_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
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
                    Content = new MeeWindow(3, re:1),
                    Title = "Акт ЭКМП"
                };
                window.ShowDialog();

                SchetRegisterGrid1.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void ReqAddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
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
                SchetRegisterGrid1.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ReqDelItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var ans = DXMessageBox.Show("Удалить запрос?", "Удаление", MessageBoxButton.YesNo);
            if (ans != MessageBoxResult.Yes) return;
            var sc = ReqGridControl.SelectedItem;

            Reader2List.CustomExecuteQuery($@"UPDATE D3_ZSL_OMS SET ReqID = NULL WHERE ReqID = {ObjHelper.GetAnonymousValue(sc, "ID")}", SprClass.LocalConnectionString);
            Reader2List.CustomAnonymousSelect($@"DELETE [dbo].[YamedRequests] Where ID = {ObjHelper.GetAnonymousValue(sc, "ID")}", SprClass.LocalConnectionString);
            
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
                MyControl = new PreviewControl(rl, null, (int)ObjHelper.GetAnonymousValue(sc, "ID")),
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }

        private void ExcelButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == true)
                SchetRegisterGrid1.gridControl1.View.ExportToXlsx(saveFileDialog.FileName);
        }

        private void AddMek_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
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
                    Content = new Yamed.OmsExp.ExpEditors.SankControl(true),
                    Title = "Акт ЭКМП"
                };
                window.ShowDialog();

                SchetRegisterGrid1.gridControl1.IsEnabled = true;
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
                MyControl = new PreviewControl(rl, null, (int)ObjHelper.GetAnonymousValue(sc, "ID")),
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }

        private void RepReqCustomItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var yr = SqlReader.Select($"Select * from YamedReports where RepName = '_reqCustomAkt'", SprClass.LocalConnectionString);
            //var rl = (string)ObjHelper.GetAnonymousValue(yr[0], "Template");
            //var sc = ReqGridControl.SelectedItem;

            //СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            //{
            //    Header = "Отчет",
            //    MyControl = new PreviewControl(rl, null, (int)ObjHelper.GetAnonymousValue(sc, "ID")),
            //    IsCloseable = "True",
            //    //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            //});


                var yr = SqlReader.Select($"Select * from YamedReports where RepName = '_reqAkt_5_in_1'", SprClass.LocalConnectionString);
                var rl = (string)ObjHelper.GetAnonymousValue(yr[0], "Template");
                var sc = ReqGridControl.SelectedItem;

                СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
                {
                    Header = "Отчет",
                    MyControl = new FRPreviewControl(rl, new ReportParams { ReqID = (int)ObjHelper.GetAnonymousValue(sc, "ID") }),
                    IsCloseable = "True",
                    //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
                });

     


            //_reqAkt_app
            //_reqAkt_stac
            //_reqAkt_disp
            //_reqAkt_onko
            //_reqAkt_vmp
        }

        private void ReqEditItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ZaprosCount()
        {
            var sc = ObjHelper.GetIds(_scids.ToArray());
            var q = Reader2List.CustomAnonymousSelect($@"exec GetExpCount '{sc}'", SprClass.LocalConnectionString);
            ExpGridControl.DataContext = q;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DxHelper.GetSelectedGridRowsAsync(ref SchetRegisterGrid1.gridControl1);
            bool isLoaded = false;
            SchetRegisterGrid1.gridControl1.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (SchetRegisterGrid1.gridControl1.IsAsyncOperationInProgress == false)
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
                        var rows = DxHelper.LoadedRows.Select(x => ObjHelper.GetAnonymousValue(x, "ID")).ToArray();

                        var window = new DXWindow
                        {
                            ShowIcon = false,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            Content = new StatisticReports(rows, 1000),
                            Title = "Печатные формы",
                            SizeToContent = SizeToContent.Width,
                            Height = 600
                        };
                        window.ShowDialog();
                        ReqBind();
                    }
                }
                SchetRegisterGrid1.gridControl1.IsEnabled = true;
                DxHelper.LoadedRows.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Zsl31Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var tab = SchetRegisterGrid1;

            var id = (int)ObjHelper.GetAnonymousValue(DxHelper.GetSelectedGridRow(tab.gridControl1), "ID");
            var slt = new SluchTemplateD31(SchetRegisterGrid1.gridControl1);
            slt.BindSluch(id, new Entity.D3_SCHET_OMS());

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Карта пациента",
                MyControl = slt,
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });

        }

        private void MtrCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (((BarCheckItem)sender).IsChecked == true)
            {
                if (string.IsNullOrEmpty(SchetRegisterGrid1.gridControl1.FilterString))
                {
                    SchetRegisterGrid1.gridControl1.FilterString = $"([SMO] Is Null Or [SMO] Not Like '46%')";

                }
                else
                {
                    SchetRegisterGrid1.gridControl1.FilterString += $"And ([SMO] Is Null Or [SMO] Not Like '46%')";

                }
            }
            else
            {
                SchetRegisterGrid1.gridControl1.FilterString =
                    SchetRegisterGrid1.gridControl1.FilterString.
                        Replace($" And ([SMO] Is Null Or [SMO] Not Like '46%')", "").
                        Replace($"([SMO] Is Null Or [SMO] Not Like '46%') And ", "").

                        Replace($"([SMO] Is Null Or [SMO] Not Like '46%')", "").
                        Replace($"[SMO] Is Null Or [SMO] Not Like '46%'", "");

            }

        }

        private void ViewCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (SchetRegisterGrid1 == null) return;

            var tag = (string)((BarCheckItem)sender).Tag;
            var isChecked = (bool)((BarCheckItem)sender).IsChecked;
            if (tag == "ZSL" && isChecked)
            {
                SchetRegisterGrid1.BindDataZsl();
            }
            else if(tag == "SL" && isChecked)
            {
                SchetRegisterGrid1.BindDataSl();
            }
            else if(tag == "USL" && isChecked)
            {
                SchetRegisterGrid1.BindDataUsl();
            }
            else if (tag == "SANK" && isChecked)
            {
                SchetRegisterGrid1.BindDataSank();
            }
        }

        private void Spisok_Form_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var sc = ReqGridControl.SelectedItem;
                var ReqID = (int)ObjHelper.GetAnonymousValue(sc, "ID");
                var window = new DXWindow
                {
                    ShowIcon = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new StatisticReportsRequest(ReqID, 2000),
                    Title = "Печатные формы",
                    SizeToContent = SizeToContent.Width,
                    Height = 600
                };
                window.ShowDialog();
                ReqBind();
            }
            catch
            {
                MessageBox.Show("Не выбран запрос");
            }
        }
    }
}
