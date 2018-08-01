using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Bars;
using Yamed.Control;
using Yamed.Server;

namespace Yamed.OmsExp.ExpEditors
{
    /// <summary>
    /// Логика взаимодействия для SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public SearchControl()
        {
            InitializeComponent();

            InitializeComponent();
            LpuComboBoxEdit.DataContext = SprClass.LpuList;
            ProfilComboBoxEdit.DataContext = SprClass.profile;
            PCelEdit.DataContext = SprClass.SprPCelList;
            UslOkEdit.DataContext = SprClass.conditionHelp;
            OsSluchEdit.DataContext = SprClass.OsobSluchDbs;

            var years = new object[] {2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025};
            var months = new object[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
            StartYearComboBoxEdit.Items.AddRange(years);
            EndYearComboBoxEdit.Items.AddRange(years);
            StartMonthComboBoxEdit.Items.AddRange(months);
            EndMonthComboBoxEdit.Items.AddRange(months);
        }

        private void SearchItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var rc = new ReestrControl();
            if (TabItem1.IsSelected)
            {
                rc.ElReestrTabNew11.BindDataSearch((string)LpuComboBoxEdit.EditValue, (int?)StartMonthComboBoxEdit.EditValue,(int?)EndMonthComboBoxEdit.EditValue,
                    (int?)StartYearComboBoxEdit.EditValue, (int?)EndYearComboBoxEdit.EditValue, (int?)ProfilComboBoxEdit.EditValue, (string)DsComboBoxEdit.EditValue,
                    (string)PCelEdit.EditValue, (int?)UslOkEdit.EditValue, (int?)OsSluchEdit.EditValue);
            }

            if (TabItem2.IsSelected)
            {
                rc.ElReestrTabNew11.BindDataPacient((string)FamBoxEdit.EditValue, (string)ImBoxEdit.EditValue, (string)OtBoxEdit.EditValue, (DateTime?)DrBoxEdit.EditValue, (string) PolisBoxEdit.EditValue);
            }

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Реестр счета",
                MyControl = rc,
                IsCloseable = "True",
                //TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
            });
        }
    }
}
