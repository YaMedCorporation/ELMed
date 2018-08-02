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

    }
}
