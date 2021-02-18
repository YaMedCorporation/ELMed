using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Yamed.Entity;
using DevExpress.XtraGrid;
using System.Collections;
using System.Globalization;
using Yamed.Server;

namespace Yamed.OmsExp
{
    /// <summary>
    /// Логика взаимодействия для License.xaml
    /// </summary>
    public class StrToArr : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo ture)
        {

            int[] ev = null;
            //string[] ev1 = null;
            if (value != null)
            {
                ev = value.ToString().Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToArray();
                return ev;
            }
            else
            {
                return ev;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo ture)
        {
            string p1 = "";
            if (value == null)
            {
                return null;
            }
            else
            {
                var d1 = (ICollection)value;
                int[] dd1 = new int[d1.Count];
                string p = "";
                d1.CopyTo(dd1, 0);
                for (int i = 0; i < dd1.Count(); i++)
                {
                    p1 += (dd1[i] + ";");
                    p = p1.Substring(0, p1.Length - 1);
                }

                return p;
            }
        }
    }
    public partial class License : UserControl
    {
        private static ElmedDataClassesDataContext dc = new ElmedDataClassesDataContext(SprClass.LocalConnectionString);
        public License()
        {
            InitializeComponent();
            gridControl1.DataContext = dc.LIC_NUM_TBL.ToList();
            LpuEdit.DataContext = SprClass.LpuList;
            VidMPEdit.DataContext = SprClass.typeHelp;
            UslMpEdit.DataContext = SprClass.conditionHelp;
            Profil.DataContext = SprClass.profile;
            gridControl1.SelectedItem = GridControl.InvalidRowHandle;
            grid.DataContext = null;
        }

        private void DeleteRowItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var lic = dc.GetTable<LIC_NUM_TBL>().Where(x => x.ID == ((LIC_NUM_TBL)gridControl1.SelectedItem).ID).FirstOrDefault();
            if (lic != null)
            {
                dc.GetTable<LIC_NUM_TBL>().DeleteOnSubmit(lic);
                dc.SubmitChanges();
                gridControl1.DataContext = dc.LIC_NUM_TBL.ToList();
                grid.DataContext = null;
                //TableView view = (TableView)gridControl1.View;
                //view.DeleteRow(view.FocusedRowHandle);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pprof = "";
            string pvid = "";
            var prof = Profil.SelectedItems;
            var vid = VidMPEdit.SelectedItems;
            if (prof.Count != 0)
            {
                string p1 = "";
                var d1 = (ICollection)Profil.EditValue;
                int[] dd1 = new int[d1.Count];
                d1.CopyTo(dd1, 0);
                for (int i = 0; i < dd1.Count(); i++)
                {
                    p1 += (dd1[i] + ";");
                    pprof = p1.Substring(0, p1.Length - 1);
                }
            }
            if (vid.Count != 0)
            {
                string p1 = "";
                var d1 = (ICollection)VidMPEdit.EditValue;
                int[] dd1 = new int[d1.Count];
                d1.CopyTo(dd1, 0);
                for (int i = 0; i < dd1.Count(); i++)
                {
                    p1 += (dd1[i] + ";");
                    pvid = p1.Substring(0, p1.Length - 1);
                }
            }
            if (grid.DataContext == null)
            {
                var lic = new LIC_NUM_TBL
                {
                    LICN = nlic.EditValue.ToString(),
                    LPU = LpuEdit.EditValue.ToString(),
                    USL_MP = (int?)UslMpEdit.EditValue,
                    VID_MP = /*VidMPEdit.EditValue.ToString(),*/ pvid,
                    PROFIL = /*Profil.EditValue.ToString(),*/pprof,
                    DATE_1 = (DateTime?)dates.EditValue,
                    DATE_2 = (DateTime?)datee.EditValue
                };
                dc.GetTable<LIC_NUM_TBL>().InsertOnSubmit(lic);
                dc.SubmitChanges();
                gridControl1.DataContext = dc.LIC_NUM_TBL.ToList();
                grid.DataContext = null;
                nlic.EditValue=null;
                LpuEdit.EditValue = null;
                UslMpEdit.EditValue = null;
                VidMPEdit.EditValue=null;
                Profil.EditValue=null;
                dates.EditValue=null;
                datee.EditValue=null;
            }
            else
            {
                dc.GetTable<LIC_NUM_TBL>();
                dc.SubmitChanges();
                gridControl1.DataContext = dc.LIC_NUM_TBL.ToList();
                grid.DataContext = null;
            } 
        }

        private void GridControl1_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (gridControl1.SelectedItem.ToString() == GridControl.InvalidRowHandle.ToString())
            {
                grid.DataContext = null;
            }
            else
            {
                grid.DataContext = gridControl1.SelectedItem;
            }
        }
    }
}
