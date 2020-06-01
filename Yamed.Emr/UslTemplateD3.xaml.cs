﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Yamed.Entity;
using Yamed.Server;

namespace Yamed.Emr
{
    /// <summary>
    /// Логика взаимодействия для HospitalEmrUslugiPanel.xaml
    /// </summary>
    public partial class UslTemplateD3 : UserControl
    {
        private D3_USL_OMS _usl;
        public DateTime? dvmp;
        //private List<USL_ASSIST> _assists;
        public UslTemplateD3(D3_USL_OMS usl)
        {
            InitializeComponent();

            _usl = usl;
            dvmp = _usl.DATE_OUT == null ? SprClass.WorkDate : _usl.DATE_OUT;
            //if (_usl.ID != 0)
            //    _assists = Reader2List.CustomSelect<USL_ASSIST> ($"SELECT * FROM USL_ASSIST WHERE UID = {_usl.ID}", SprClass.LocalConnectionString);
            //else
            //{
            //    _assists = new List<USL_ASSIST>();
            //}

            GridUsl.DataContext = _usl;
            //AssistGridControl.DataContext = _assists;

            OtdelBox.DataContext = SprClass.OtdelDbs;
            ProfilBox.DataContext = SprClass.profile;
            DetBox.DataContext = SprClass.SprDetProfilList;
            DoctorBox.DataContext = SprClass.MedicalEmployeeList;
            SpecBox.DataContext = SprClass.SpecV021List;
            DsBox.DataContext = SprClass.mkbSearching;
           
            //UslOslBox.DataContext = SprClass.OslList;
            //AnestBox.DataContext = SprClass.AnestList;
            //AssistColumnEdit.DataContext = SprClass.DoctList;
            VidVmeBox.DataContext = SprClass.SprUsl804;
            POtkBox.DataContext = SprClass.SprBit;
            NplBox.DataContext = SprClass.SprNpl;
            if (SprClass.Region == "37")
            {
                DoljnostBox.Visibility = Visibility.Visible;
                dolj.Visibility = Visibility.Visible;
                DoljnostBox.DataContext = Reader2List.CustomAnonymousSelect($@"Select distinct convert(int,KOD_SP) as KOD_SP,convert(nvarchar,KOD_SP)+' '+NSP as NameWithID from rg012 where '{dvmp}' between dt_beg and isnull(dt_fin,'20530101') and kod_lpu='{usl.LPU}'", SprClass.LocalConnectionString);
                Kod_uslBox.Visibility = Visibility.Visible;
                kod_usl.Visibility = Visibility.Visible;
                Kod_uslBox.DataContext = Reader2List.CustomAnonymousSelect($@"Select distinct kodusl,convert(nvarchar,KODUSL) as CODE_USL,convert(nvarchar,KODUSL)+' '+NUSL as NameWithID from rg012 where '{dvmp}' between dt_beg and isnull(dt_fin,'20530101') and kod_lpu='{usl.LPU}'", SprClass.LocalConnectionString);
            }
            else if (SprClass.Region != "37")
            {
                Kod_uslBox.DataContext = Reader2List.CustomAnonymousSelect($@"SELECT ID as CODE_USL,NameWithID from Yamed_Spr_UslCode", SprClass.LocalConnectionString);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            ((DXWindow)this.Parent).Close();
        }

        private void DateInBox_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            _usl.DATE_OUT = _usl.DATE_IN;
        }

        private void mkbBox_GotFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
        }

        private void mkbBox_LostFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("ru-RU");
        }

        private void AssistAddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //_assists.Add(new USL_ASSIST());
            //AssistGridControl.RefreshData();
        }

        private void HospitalEmrUslugiPanel_OnUnloaded(object sender, RoutedEventArgs e)
        {
            //foreach (var assist in _assists)
            //{
            //    _usl.USL_ASSIST.Add(assist);
            //}
        }

        private void VidVmeBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
        }

        private void VidVmeBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("ru-RU");
        }

        private void DoctorBox_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (SprClass.Region == "37" && SprClass.ProdSett.OrgTypeStatus == Core.OrgType.Lpu)
            {
                if (DoctorBox.EditValue != null)
                {
                    ProfilBox.EditValue = Reader2List.SelectScalar($@"select PROFIL_ID from Yamed_Spr_MedicalEmployee where snils='{DoctorBox.EditValue}'", SprClass.LocalConnectionString).ToString();
                    SpecBox.EditValue = Reader2List.SelectScalar($@"select PRVS_ID from Yamed_Spr_MedicalEmployee where snils='{DoctorBox.EditValue}'", SprClass.LocalConnectionString).ToString();
                    DetBox.EditValue = Reader2List.SelectScalar($@"select DET_ID from Yamed_Spr_MedicalEmployee where snils='{DoctorBox.EditValue}'", SprClass.LocalConnectionString).ToString();
                    DoljnostBox.EditValue = Reader2List.SelectScalar($@"select KOD_SP from Yamed_Spr_MedicalEmployee where snils='{DoctorBox.EditValue}'", SprClass.LocalConnectionString).ToString();
                }
            }
        }
    }
}
