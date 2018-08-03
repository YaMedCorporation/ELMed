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
        private void ButtonEdit_OnDefaultButtonClick(object sender, RoutedEventArgs e)
        {
            _id = Convert.ToInt32(ButtonEdit.EditValue);
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
            using (SqlConnection sc = new SqlConnection(SprClass.LocalConnectionString))
            {
                sc.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter("select s.id as ID, case   when s.month = 1 then concat('Реестр счета (', s.id, ') Январь ', s.year,' г.')   when s.month = 2 then concat('Реестр счета (', s.id, ') Февраль ', s.year,' г.')   when s.month = 3 then concat('Реестр счета (', s.id, ') Март ', s.year,' г.')   when s.month = 4 then concat('Реестр счета (', s.id, ') Апрель ', s.year,' г.')   when s.month = 5 then concat('Реестр счета (', s.id, ') Май ', s.year,' г.')   when s.month = 6 then concat('Реестр счета (', s.id, ') Июнь ', s.year,' г.')   when s.month = 7 then concat('Реестр счета (', s.id, ') Июль ', s.year,' г.')   when s.month = 8 then concat('Реестр счета (', s.id, ') Август ', s.year,' г.')   when s.month = 9 then concat('Реестр счета (', s.id, ') Сентябрь ', s.year,' г.')   when s.month = 10 then concat('Реестр счета (', s.id, ') Октябрь ', s.year,' г.')   when s.month = 11 then concat('Реестр счета (', s.id, ') Ноябрь ', s.year,' г.')   when s.month = 12 then concat('Реестр счета (', s.id, ') Декабрь ', s.year,' г.') end nameSchet from d3_schet_oms s", sc))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        comboboxSchets.ItemsSource = dt;
                    }
                }
                sc.Close();
                comboboxSchets.SelectedIndex = -1;
            }
        }

        private void comboboxSchets_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            ButtonEdit.EditValue = ((DataRowView)comboboxSchets.SelectedItem)[comboboxSchets.ValueMember].ToString();
        }
    }
}