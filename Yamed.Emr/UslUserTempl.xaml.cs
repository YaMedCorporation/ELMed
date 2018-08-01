﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using DevExpress.Xpf.Core;
using Yamed.Core;
using Yamed.Entity;
using Yamed.Server;

namespace Yamed.Emr
{
    /// <summary>
    /// Логика взаимодействия для UslUserTempl.xaml
    /// </summary>
    public partial class UslUserTempl : UserControl
    {
        private SluchTemplateD3 _sluchTemplateD3;
        public UslUserTempl(SluchTemplateD3 sluchTemplateD3)
        {
            InitializeComponent();

            _sluchTemplateD3 = sluchTemplateD3;

            Task.Factory.StartNew(() =>
            {
                return Reader2List.GetAnonymousTable("Yamed_Spr_UslCategory", SprClass.LocalConnectionString);
            }).ContinueWith(x =>
            {
                UslCategoryEdit.DataContext = x.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var lpu = _sluchTemplateD3._zsl.LPU;
            var slgid = ((D3_SL_OMS) _sluchTemplateD3.SlGridControl.SelectedItem).SL_ID;

            var ulist = UslUserTemplEdit.SelectedItems;
            foreach (DynamicBaseClass usl in ulist)
            {
                _sluchTemplateD3._uslList.Add(new D3_USL_OMS
                {
                    COMENTU = (string)usl.GetValue("Name"),
                    CODE_USL = (string)usl.GetValue("Code"),
                    VID_VME = (string)usl.GetValue("VM_Code"),
                    KOL_USL = (decimal?)usl.GetValue("Kol"),
                    TARIF = (decimal?)usl.GetValue("Tarif"),
                    PROFIL = (int?)usl.GetValue("Profil"),
                    DET = (int?)usl.GetValue("Det"),
                    PRVS = (int?)usl.GetValue("Spec"),
                    LPU = lpu,
                    D3_SLGID = slgid
                });
            }

            ((DXWindow) this.Parent).Close();
        }

        private void UslCategoryEdit_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var cid = ObjHelper.GetAnonymousValue(UslCategoryEdit.SelectedItem, "ID");
            Task.Factory.StartNew(() =>
            {
                return SqlReader.Select2($"Select * From Yamed_Spr_UslTemplate where CategoryID = {cid}", SprClass.LocalConnectionString);
            }).ContinueWith(x =>
            {
                UslUserTemplEdit.DataContext = x.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}
