using System;
using System.Collections;
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
using DevExpress.Mvvm.Native;
using Yamed.Control;
using Yamed.Core;
using Yamed.Server;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Yamed.Oms
{
    /// <summary>
    /// Логика взаимодействия для ReestrChooseControl.xaml
    /// </summary>
    public partial class ReestrChooseControl : UserControl
    {
        public ReestrChooseControl()
        {
            InitializeComponent();
        }
        

        private int _id;

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            _id = (int) cbSchets.EditValue;
            int selectCbOne = cbOperation.SelectedIndex;
            if (selectCbOne == 1)
            {
                var ids = GetIds(DxHelper.LoadedRows.Select(x => ObjHelper.GetAnonymousValue(x, "ID")).OfType<int>().ToArray());

                var updObj = (IList)Reader2List.CustomAnonymousSelect($"Select * from D3_ZSL_OMS Where ID in ({ids})",
                    SprClass.LocalConnectionString);
                //updObj.ForEach(x=> x.SetValue("D3_SCID", _id));
                foreach (var row in updObj)
                {
                    var r = row;
                    ObjHelper.SetAnonymousValue(ref r, _id, "D3_SCID");
                }

                var upd = Reader2List.CustomUpdateCommand("D3_ZSL_OMS", updObj, "ID");
                Reader2List.CustomExecuteQuery(upd, SprClass.LocalConnectionString);
                MessageBox.Show("Перенос записей выполнен успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            list.Add("1 Копирование");
            list.Add("2 Перенос");
            cbOperation.ItemsSource = list;
            //using (SqlConnection sc = new SqlConnection(SprClass.LocalConnectionString))
            //{
            //    sc.Open();
            //    using (SqlDataAdapter sda = new SqlDataAdapter("select (((('—счет(' + CONVERT([varchar](16), [ID])  + ')/период '+CONVERT([varchar](2),[MONTH],(0)))+'.')+CONVERT([char](6),[YEAR],(0)))+isnull(('('+[COMENTS])+')','')) nameSchet from d3_schet_oms s", sc))
            //    {
            //        using (DataTable dt = new DataTable())
            //        {
            //            sda.Fill(dt);
            //            cbSchets.ItemsSource = dt;
            //        }
            //    }
            //    sc.Close();
            //    cbSchets.SelectedIndex = -1;
            //}
            cbSchets.ItemsSource =
                Reader2List.CustomAnonymousSelect(
                    "select(((('—счет(' + CONVERT([varchar](16), [ID]) + ')/период ' + CONVERT([varchar](2),[MONTH], (0)))+'.')+CONVERT([char](6),[YEAR],(0)))+isnull(('('+[COMENTS])+')','')) nameSchet from d3_schet_oms s",
                    SprClass.LocalConnectionString);
        }

        private void comboboxSchets_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //_id = Convert.ToInt32(((DataRowView)cbSchets.SelectedItem)[cbSchets.ValueMember]);
        }
    }
}