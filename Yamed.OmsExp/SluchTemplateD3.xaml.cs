using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Data;
using DevExpress.Data.Async.Helpers;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Yamed.Core;
using Yamed.Entity;
using Yamed.OmsExp.ExpEditors;
using Yamed.OmsExp.MekEditor;
using Yamed.Server;

namespace Yamed.OmsExp
{
    /// <summary>
    /// Логика взаимодействия для SluchTemplate.xaml
    /// </summary>
    public partial class SluchTemplateD3 : UserControl
    {
        public D3_ZSL_OMS _zsl;
        public List<D3_SL_OMS> _slList;
        public List<D3_USL_OMS> _uslList;
        public List<D3_SANK_OMS> _sankList;
        private GridControl _gc;
        private int rowIndex;

        public SluchTemplateD3(GridControl gc)
        {
            InitializeComponent();
            GetSpr();
            _gc = gc;

            rowIndex = _gc.GetSelectedRowHandles().Where(x => x >= 0).First();

            var row = _gc.GetRow(rowIndex);
            _row = ((ReadonlyThreadSafeProxyForObjectFromAnotherThread)row).OriginalRow;

            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            //    new Action(delegate()
            //    {
            //        polisBox.Focus();
            //    }));
        }

        public void ZslUpdate()
        {
            Task.Factory.StartNew(() =>
            {
                return Reader2List.CustomSelect<D3_ZSL_OMS>($"Select * from D3_ZSL_OMS where ID = {_zsl.ID}",
                    SprClass.LocalConnectionString)[0];
            }).ContinueWith((x) =>
            {
                _zsl = x.Result;
                ZSlGrid.DataContext = _zsl;

                x.Dispose();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        public void BindSluch(int slid)
        {
            //if (_sc == null) _sc = sc;

            Task.Factory.StartNew(() =>
            {
                return Reader2List.CustomSelect<D3_ZSL_OMS>($"Select * from D3_ZSL_OMS where ID = {slid}",
                    SprClass.LocalConnectionString)[0];
            }).ContinueWith((x) =>
            {
                _zsl = x.Result;
                ZSlGrid.DataContext = _zsl;

                var row =
                    SqlReader.Select2(
                        $"Select * from D3_PACIENT_OMS where ID = '{_zsl.D3_PID}' order by ID desc",
                        SprClass.LocalConnectionString);
                if (row.Count > 0)
                {
                    var pid = (int)ObjHelper.GetAnonymousValue(row[0], "ID");
                    var fam = (string)ObjHelper.GetAnonymousValue(row[0], "FAM");
                    var im = (string)ObjHelper.GetAnonymousValue(row[0], "IM");
                    var ot = (string)ObjHelper.GetAnonymousValue(row[0], "OT");
                    var dr = (DateTime)ObjHelper.GetAnonymousValue(row[0], "DR");
                    var polis = (string)ObjHelper.GetAnonymousValue(row[0], "NPOLIS");

                    PacientText.Text = $"{fam} {im} {ot} {dr:yyyy}";
                    PolisText.Text = $"{polis}";
                }

                Task.Factory.StartNew(() =>
                {
                    return Reader2List.CustomSelect<D3_SANK_OMS>($"Select * from D3_SANK_OMS where D3_ZSLID = {slid}",
                        SprClass.LocalConnectionString);
                }).ContinueWith((u) =>
                {
                    _sankList = u.Result;
                    if (_sankList == null) _uslList = new List<D3_USL_OMS>();
                    SankGridControl.DataContext = _sankList;
                    u.Dispose();
                }, TaskScheduler.FromCurrentSynchronizationContext());
                
                x.Dispose();
            }, TaskScheduler.FromCurrentSynchronizationContext());

            Task.Factory.StartNew(() =>
            {
                return Reader2List.CustomSelect<D3_SL_OMS>($"Select * from D3_SL_OMS where D3_ZSLID = {slid}",
                    SprClass.LocalConnectionString);
            }).ContinueWith((s) =>
            {
                _slList = s.Result;
                SlGrid.DataContext = _slList;
                s.Dispose();

                Task.Factory.StartNew(() =>
                {
                    return Reader2List.CustomSelect<D3_USL_OMS>($"Select * from D3_USL_OMS where D3_ZSLID = {slid}",
                        SprClass.LocalConnectionString);
                }).ContinueWith((u) =>
                {
                    _uslList = u.Result;
                    if (_uslList == null) _uslList=new List<D3_USL_OMS>();
                    UslGridControl.DataContext = _uslList;
                    u.Dispose();
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Task _task;

        private D3_SCHET_OMS _sc;
        public void BindEmptySluch2(D3_SCHET_OMS sc = null)
        {
            if (_sc == null) _sc = sc;
            _zsl = new D3_ZSL_OMS();
            _zsl.D3_SCID = _sc.ID;
            _zsl.LPU = _sc.CODE_MO;
            ZSlGrid.DataContext = _zsl;

            _slList = new List<D3_SL_OMS>();
            _slList.Add(new D3_SL_OMS()
            {
                SL_ID = Guid.NewGuid().ToString()
            });

            _uslList = new List<D3_USL_OMS>();

            SlGrid.DataContext = _slList;
            UslGridControl.DataContext = _uslList;

            //_task = Task.Factory.StartNew(() =>
            //{
            //    return SqlReader.Select2($"Select * from D3_ZSL_OMS where ID = 0",
            //        SprClass.LocalConnectionString);
            //}).ContinueWith((x) =>
            //{
            //    //_zsl = (DynamicBaseClass)Activator.CreateInstance(x.Result.GetDynamicType());
            //    _zsl.D3_SCID = sc.ID;
            //    SluchGrid.DataContext = _zsl;
            //    x.Dispose();
            //}, TaskScheduler.FromCurrentSynchronizationContext());
        }


        public void DoctDataFill(DynamicBaseClass data)
        {
            //_task.ContinueWith(x =>
            //{
            //    _zsl.SetValue("LPU", data.GetValue("LPU_ID"));
            //    _zsl.SetValue("USL_OK", data.GetValue("USL_OK_ID"));
            //    _zsl.SetValue("LPU_1", data.GetValue("PODR_ID"));
            //    _zsl.SetValue("PORD", data.GetValue("OTD_ID"));
            //    _zsl.SetValue("MSPID", data.GetValue("PRVS_ID"));
            //    _zsl.SetValue("PROFIL", data.GetValue("PROFIL_ID"));
            //    _zsl.SetValue("DET", Convert.ToByte(data.GetValue("DET_ID")));
            //    _zsl.SetValue("VIDPOM", data.GetValue("VID_POM_ID"));
            //    _zsl.SetValue("IDDOKT", data.GetValue("SNILS"));
            //    _zsl.SetValue("IDDOKTO", data.GetValue("DID"));
            //});
        }

        void GetSpr()
        {
            DoctGrid.DataContext = SprClass.MedicalEmployeeList;
            RsltEdit.DataContext = SprClass.helpResult;
            IshodEdit.DataContext = SprClass.helpExit;
            VidPomEdit.DataContext = SprClass.typeHelp;
            UslOkGrid.DataContext = SprClass.conditionHelp;
            ProfilGrid.DataContext = SprClass.profile;
            PrvsGrid.DataContext = SprClass.SpecAllList;
            //SluchTypeGrid.DataContext = SprClass.typeSluch;
            PCelTypeGrid.DataContext = SprClass.SprPCelList;
            DetGrid.DataContext = SprClass.SprDetProfilList;
            IdspEdit.DataContext = SprClass.tarifUsl;
            Ds1Edit.DataContext = SprClass.mkbSearching.Where(x => x.ISDELETE == false).ToList();
            //ds.DataContext = SprClass.mkbSearching.Where(x => x.ISDELETE == false).ToList();
            //ds2Box.DataContext = SprClass.mkbSearching.Where(x => x.ISDELETE == false).ToList();
            SluchOsRegionEdit.DataContext = SprClass.OsobSluchDbs;
            NaprMoEdit.DataContext = SprClass.medOrg;
            //NaprGrid.DataContext = SprClass.ExtrDbs;
            OtdelGrid.DataContext = SprClass.OtdelDbs;
            PodrGrid.DataContext = SprClass.Podr;
            ForPomEdit.DataContext = SprClass.ForPomList;
            Ds1PrEdit.DataContext = SprClass.SprBit;
            VozrEdit.DataContext = SprClass.VozrList;
            //Prvs21Edit.DataContext = SprClass.SpecV021List;

            UslOtdelColumnEdit.DataContext = SprClass.OtdelDbs;
            UslProfilColumnEdit.DataContext = SprClass.profile;
            UslPrvsColumnEdit.DataContext = SprClass.SpecAllList;
            UslDoctorColumnEdit.DataContext = SprClass.MedicalEmployeeList;
            UslDsColumnEdit.DataContext = SprClass.mkbSearching;
            UslVidVmeColumnEdit.DataContext = SprClass.SprUsl804;
            UslPOtkColumnEdit.DataContext = SprClass.SprBit;
            UslNplEdit.DataContext = SprClass.SprNpl;

        }

        private void mkbBox_GotFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
        }

        private void mkbBox_LostFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("ru-RU");
        }

        public int? SaveSluch()
        {
            int? zslid = null;
            if (_zsl.ID == 0)
            {
                zslid = Reader2List.ObjectInsertCommand("D3_ZSL_OMS", _zsl, "ID", SprClass.LocalConnectionString);
                _zsl.ID = (int)zslid;
            }
            else
            {
                var upd = Reader2List.CustomUpdateCommand("D3_ZSL_OMS", _zsl, "ID");
                Reader2List.CustomExecuteQuery(upd, SprClass.LocalConnectionString);
            }

            foreach (var sl in _slList)
            {
                if (sl.ID == 0)
                {
                    sl.D3_ZSLID = _zsl.ID;
                    var slid = Reader2List.ObjectInsertCommand("D3_SL_OMS", sl, "ID", SprClass.LocalConnectionString);
                    sl.ID = (int)slid;
                }
                else
                {
                    var upd = Reader2List.CustomUpdateCommand("D3_SL_OMS", sl, "ID");
                    Reader2List.CustomExecuteQuery(upd, SprClass.LocalConnectionString);
                }
            }

            foreach (var usl in _uslList)
            {

                if (usl.ID == 0)
                {
                    usl.D3_ZSLID = _zsl.ID;
                    usl.D3_SLID = _slList.Single(x => x.SL_ID == usl.D3_SLGID).ID;
                    var uslid = Reader2List.ObjectInsertCommand("D3_USL_OMS", usl, "ID", SprClass.LocalConnectionString);
                    usl.ID = (int)uslid;
                }
                else
                {
                    var upd = Reader2List.CustomUpdateCommand("D3_USL_OMS", usl, "ID");
                    Reader2List.CustomExecuteQuery(upd, SprClass.LocalConnectionString);
                }
            }

            return zslid;
        }

        public void SaveSluchAsync()
        {
            Task.Factory.StartNew(() =>
            {
                SaveSluch();
            });
        }

        private void dxEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            //if (sender is ComboBoxEdit)
            //{
            //    ComboBoxEdit ed = (sender as ComboBoxEdit);
            //    object item = ed.GetItemByKeyValue(e.Value);
            //    if (item == null)
            //    {
            //        e.ErrorContent = "Поле обязательно для заполнения";
            //        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Default;
            //        e.IsValid = false;
            //        e.Handled = true;
            //    }
            //}

            //if (sender is TextEdit)
            //{
            //    TextEdit ed = (sender as TextEdit);
            //    if (string.IsNullOrEmpty(ed.Text))
            //    {
            //        e.ErrorContent = "Поле обязательно для заполнения";
            //        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Default;
            //        e.IsValid = false;
            //        e.Handled = true;
            //    }
            //}

            //if (sender is DateEdit)
            //{
            //    DateEdit ed = (sender as DateEdit);
            //    if (ed.EditValue == null)
            //    {
            //        e.ErrorContent = "Поле обязательно для заполнения";
            //        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Default;
            //        e.IsValid = false;
            //        e.Handled = true;
            //    }
            //}
        }

        //PopupListBox lb;
        //private void combo_PopupOpened(object sender, RoutedEventArgs e)
        //{
        //    lb = (PopupListBox)LookUpEditHelper.GetVisualClient((ComboBoxEdit)sender).InnerEditor;
        //    ComboBoxEditItem item = (ComboBoxEditItem)lb.ItemContainerGenerator.ContainerFromIndex(0);
        //    if (item != null)
        //        item.IsSelected = true;
        //    lb.ItemContainerGenerator.ItemsChanged += new System.Windows.Controls.Primitives.ItemsChangedEventHandler(ItemContainerGenerator_ItemsChanged);
        //}
        //void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        //{
        //    Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        ComboBoxEditItem item = (ComboBoxEditItem)lb.ItemContainerGenerator.ContainerFromIndex(0);
        //        if (item != null)
        //            item.IsSelected = true;
        //    }), System.Windows.Threading.DispatcherPriority.Render);
        //}
        private void SlAddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            SlEditLock();

            var sl = new D3_SL_OMS()
            {
                SL_ID = Guid.NewGuid().ToString()
            };

            _slList.Add(sl);

            SlGridControl.RefreshData();
            SlGridControl.SelectedItem = sl;

            SlEditDefault();
        }

        private void SlDelItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var del = (D3_SL_OMS)SlGridControl.SelectedItem;

            if (del.ID == 0)
            {
                _slList.Remove(del);
            }
            else
            {
                Reader2List.CustomExecuteQuery($"DELETE D3_SL_OMS WHERE ID = {del.ID}", SprClass.LocalConnectionString);
                _slList.Remove(del);
            }
            SlGridControl.RefreshData();
        }

        private void Ds3AddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Ds3DelItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PolisEmr_OnClick(object sender, RoutedEventArgs e)
        {
            var emrPacient = new EmrPacientControl();
            emrPacient.BindPacient(_zsl.D3_PID);

            var window = new DXWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = emrPacient,
                Title = "Карат пациента",
                SizeToContent = SizeToContent.WidthAndHeight

                //WindowStyle = WindowStyle.None
            };
            window.ShowDialog();
        }
        private void PolisSearch_OnClick(object sender, RoutedEventArgs e)
        {
            PolisSearch();
        }

        void PolisSearch()
        {
    //        if (polisBox.EditValue != null && ((string) polisBox.EditValue).Length > 0)
    //        {
    //            var row =
    //SqlReader.Select2(
    //    $"Select * from D3_PACIENT_OMS where NPOLIS = '{polisBox.EditValue}' order by ID desc",
    //    SprClass.LocalConnectionString);
    //            if (row.Count > 0)
    //            {
    //                var pid = (int)ObjHelper.GetAnonymousValue(row[0], "ID");
    //                var fam = (string)ObjHelper.GetAnonymousValue(row[0], "FAM");
    //                var im = (string)ObjHelper.GetAnonymousValue(row[0], "IM");
    //                var ot = (string)ObjHelper.GetAnonymousValue(row[0], "OT");
    //                var dr = (DateTime)ObjHelper.GetAnonymousValue(row[0], "DR");

    //                _zsl.D3_PID = pid;
    //                PacientGroup.Header = $"Пациент: {fam} {im} {ot} {dr:yyyy}";
    //            }
    //            else
    //            {
    //                PacientEmr();
    //            }
    //        }
    //        else
    //        {
    //            PacientEmr();
    //        }
        }

        void PacientEmr()
        {
            var window = new DXWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //Content = new PacientTest(this),
                Title = "Регистрация пациента",

                //WindowStyle = WindowStyle.None
            };
            window.ShowDialog();

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SaveSluch();
        }

        private void PolisBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PolisSearch();

        }

        private void UslAddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var usl = new D3_USL_OMS {LPU = _zsl.LPU, D3_SLGID = ((D3_SL_OMS)SlGridControl.SelectedItem).SL_ID };
            _uslList.Add(usl);

            var window = new DXWindow
            {
                ShowIcon = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.Height,
                //Content = new UslTemplateD3(usl)
            };
            window.ShowDialog();
            UslGridControl.RefreshData();
        }

        private void UslEditItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var usl = (D3_USL_OMS)UslGridControl.SelectedItem;
            var window = new DXWindow
            {
                ShowIcon = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.Height,
                //Content = new UslTemplateD3(usl)
            };
            window.ShowDialog();
            UslGridControl.RefreshData();
        }

        private void UslDelItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var del = (D3_USL_OMS)UslGridControl.SelectedItem;

            if (del.ID == 0)
            {
                _uslList.Remove(del);
            }
            else
            {
                Reader2List.CustomExecuteQuery($"DELETE D3_USL_OMS WHERE ID = {del.ID}", SprClass.LocalConnectionString);
                _uslList.Remove(del);
            }
            UslGridControl.RefreshData();
        }

        private void SlGridControl_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            UslGridControl.FilterString = $"([D3_SLGID] = '{((D3_SL_OMS)SlGridControl.SelectedItem)?.SL_ID}')";
        }

        private void NewZSluch_OnClick(object sender, RoutedEventArgs e)
        {
            //ZSlEditLock();
            //SlEditLock();

            //BindEmptySluch2();
            //polisBox.EditValue = null;
            //PacientGroup.Header = "Пациент:";
            
            //ZSlEditDefault();
            //SlEditDefault();

            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            //    new Action(delegate()
            //    {
            //        polisBox.Focus();
            //    }));
        }


        private D3_ZSL_OMS _zslLock;
        private D3_SL_OMS _slLock;

        void ZSlEditDefault()
        {
            _zsl.VIDPOM = _zslLock.VIDPOM;
            _zsl.FOR_POM = _zslLock.FOR_POM;
            _zsl.NPR_MO = _zslLock.NPR_MO;
            _zsl.IDSP = _zslLock.IDSP;
            _zsl.OS_SLUCH_REGION = _zslLock.OS_SLUCH_REGION;
            _zsl.VOZR = _zslLock.VOZR;
            _zsl.DATE_Z_1 = _zslLock.DATE_Z_1;
            _zsl.DATE_Z_2 = _zslLock.DATE_Z_2;
            _zsl.RSLT = _zslLock.RSLT;
            _zsl.ISHOD = _zslLock.ISHOD;
        }

        void SlEditDefault()
        {
            ((D3_SL_OMS) SlGridControl.SelectedItem).USL_OK = _slLock.USL_OK;
            ((D3_SL_OMS) SlGridControl.SelectedItem).LPU_1 = _slLock.LPU_1;
            ((D3_SL_OMS) SlGridControl.SelectedItem).PODR = _slLock.PODR;
            ((D3_SL_OMS) SlGridControl.SelectedItem).IDDOKT = _slLock.IDDOKT;
            ((D3_SL_OMS) SlGridControl.SelectedItem).PRVS = _slLock.PRVS;
            ((D3_SL_OMS) SlGridControl.SelectedItem).PROFIL = _slLock.PROFIL;
            ((D3_SL_OMS) SlGridControl.SelectedItem).DET = _slLock.DET;
            ((D3_SL_OMS) SlGridControl.SelectedItem).P_CEL = _slLock.P_CEL;
            ((D3_SL_OMS) SlGridControl.SelectedItem).DS1 = _slLock.DS1;
            ((D3_SL_OMS) SlGridControl.SelectedItem).DATE_1 = _slLock.DATE_1;
            ((D3_SL_OMS) SlGridControl.SelectedItem).DATE_2 = _slLock.DATE_2;
        }

        void ZSlEditLock()
        {
            //if (_zslLock == null)
            //    _zslLock = new D3_ZSL_OMS();

            //_zslLock.VIDPOM = VidPomTb.IsChecked == true ? _zsl.VIDPOM : null;
            //_zslLock.FOR_POM = ForPomTb.IsChecked == true ? _zsl.FOR_POM : null;
            //_zslLock.NPR_MO = NaprMoTb.IsChecked == true ? _zsl.NPR_MO : null;
            //_zslLock.IDSP = IdspTb.IsChecked == true ? _zsl.IDSP : null;
            //_zslLock.OS_SLUCH_REGION = SluchOsRegionTb.IsChecked == true ? _zsl.OS_SLUCH_REGION : null;
            //_zslLock.VOZR = VozrTb.IsChecked == true ? _zsl.VOZR : null;
            //_zslLock.DATE_Z_1 = DateZ1Tb.IsChecked == true ? _zsl.DATE_Z_1 : null;
            //_zslLock.DATE_Z_2 = DateZ2Tb.IsChecked == true ? _zsl.DATE_Z_2 : null;
            //_zslLock.RSLT = RsltTb.IsChecked == true ? _zsl.RSLT : null;
            //_zslLock.ISHOD = IshodTb.IsChecked == true ? _zsl.ISHOD : null;
        }

        void SlEditLock()
        {
            if (_slLock == null)
                _slLock = new D3_SL_OMS();

            _slLock.USL_OK = UslOkTb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).USL_OK : null;
            _slLock.LPU_1 = PodrTb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).LPU_1 : null;
            _slLock.PODR = OtdelTb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).PODR : null;
            _slLock.IDDOKT = DoctTb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).IDDOKT : null;
            _slLock.PRVS = PrvsTb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).PRVS : null;
            _slLock.PROFIL = ProfilTb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).PROFIL : null;
            _slLock.DET = DetTb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).DET : null;
            _slLock.P_CEL = PCelTypeTb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).P_CEL : null;
            _slLock.DS1 = Ds1Tb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).DS1 : null;
            _slLock.DATE_1 = Date1Tb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).DATE_1 : null;
            _slLock.DATE_2 = Date2Tb.IsChecked == true ? ((D3_SL_OMS)SlGridControl.SelectedItem).DATE_2 : null;
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            GetZslRowId(++rowIndex);
        }

        private object _row;
        private void GetZslRowId(int index)
        {
            var row = _gc.GetRow(index);
            if (row == null) return;

            if (row is NotLoadedObject)
            {
                _gc.GetRowAsync(index).ContinueWith((x) =>
                {
                    _row = ((ReadonlyThreadSafeProxyForObjectFromAnotherThread) x.Result).OriginalRow;
                    BindSluch((int) ObjHelper.GetAnonymousValue(_row, "ID"));
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                _row = ((ReadonlyThreadSafeProxyForObjectFromAnotherThread) row).OriginalRow;
                BindSluch((int) ObjHelper.GetAnonymousValue(_row, "ID"));
            }
        }

        private void PrevButton_OnClick(object sender, RoutedEventArgs e)
        {
            GetZslRowId(--rowIndex);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveSluchAsync();
        }

        private void SankAddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var sank = new D3_SANK_OMS
            {
                S_TIP = 1,
                S_DATE = SprClass.WorkDate,
                S_SUM = _zsl.SUMV,
                D3_ZSLID = _zsl.ID,
                D3_SCID = _zsl.D3_SCID,
                S_CODE = Guid.NewGuid().ToString(),
                D3_ZSLGID = _zsl.ZSL_ID
            };
            _sankList.Add(sank);

            var window = new DXWindow
            {
                ShowIcon = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.Height,
                Width = 500,
                Content = new SankControl(sank)
            };
            window.ShowDialog();
            SankGridControl.RefreshData();
            ZslUpdate();
        }

        private void SankEditItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var sank = (D3_SANK_OMS) SankGridControl.SelectedItem;
            if (sank.S_TIP == 1)
            {
                var window = new DXWindow
                {
                    ShowIcon = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.Height,
                    Width = 500,
                    Content = new SankControl(sank)
                };
                window.ShowDialog();
            }
            else
            {
                var window = new DXWindow
                {
                    ShowIcon = false,
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Content = new MeeWindow((int)sank.S_TIP, sank.ID, _row),
                    Title = "Акт МЭЭ"
                };
                window.ShowDialog();
            }

            SankGridControl.RefreshData();
            ZslUpdate();

        }

        private void SankDelItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var result = DXMessageBox.Show("Удалить санкцию?", "Удаление", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes ) return;

            var s = (D3_SANK_OMS)SankGridControl.SelectedItem;

            Task.Factory.StartNew(() =>
            {
                Reader2List.CustomExecuteQuery($"DELETE D3_SANK_OMS WHERE ID = {s.ID}", SprClass.LocalConnectionString);
                _sankList.Remove(s);

                Reader2List.CustomExecuteQuery($@"
EXEC p_oms_calc_sank {_zsl.D3_SCID}
EXEC p_oms_calc_schet {_zsl.D3_SCID}
", SprClass.LocalConnectionString);

            }).ContinueWith(x =>
            {
                ZslUpdate();
                SankGridControl.RefreshData();

            }, TaskScheduler.FromCurrentSynchronizationContext());
            
        }
    }
}
