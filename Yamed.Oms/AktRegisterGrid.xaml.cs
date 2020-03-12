using System;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
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
using Yamed.Server;

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
        }

        private void AddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = (EconomyWindow)((TabElement)СommonСomponents.DxTabObject).MyControl;
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
                            Reader2List.CustomExecuteQuery($@"DELETE FROM D3_AKT_REGISTR_OMS where ID = {row.ID} ",
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
                        ErrorGlobalWindow.ShowError("Счет удален");
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



    }

}