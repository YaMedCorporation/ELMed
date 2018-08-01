using System;
using System.Collections;
using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Yamed.Core;
using Yamed.Entity;
using Yamed.Server;

namespace Yamed.OmsExp.MekEditor
{
    class SqlExecute
    {
        public static void GetData(GridControl gridControl, string sqlQuery)
        {
            gridControl.DataContext = Reader2List.CustomAnonymousSelect(sqlQuery,
                SprClass.LocalConnectionString);
        }

        public static void UpdateData(ListBoxEdit listBoxEdit)
        {
            listBoxEdit.DataContext = SqlReader.Select(MekQuery, SprClass.LocalConnectionString);
        }

        private const string MekQuery = "Select * From Yamed_ExpSpr_ExpAlg";
        public static void AddData(ListBoxEdit listBoxEdit)
        {
            var empty = SqlReader.Select2("Select * From Yamed_ExpSpr_ExpAlg where ID = 0",
                SprClass.LocalConnectionString);
            var obj = Activator.CreateInstance(empty.GetDynamicType());
            ((DynamicBaseClass)obj).SetValue("ExpName", "Новая запись");
            ((DynamicBaseClass)obj).SetValue("ExpType", 1);

            Reader2List.ObjectInsertCommand("Yamed_ExpSpr_ExpAlg", obj, "ID", SprClass.LocalConnectionString);
            UpdateData(listBoxEdit);
            listBoxEdit.SelectedIndex = ((IList)listBoxEdit.DataContext).Count - 1;
            listBoxEdit.ScrollIntoView(listBoxEdit.SelectedItem);
        }

        public static void CopyData(ListBoxEdit listBoxEdit)
        {
            var item = (DynamicBaseClass)listBoxEdit.SelectedItem;
            item.SetValue("ExpName", (string)item.GetValue("ExpName") + " (копия)");
            Reader2List.ObjectInsertCommand("Yamed_ExpSpr_ExpAlg", listBoxEdit.SelectedItem, "ID", SprClass.LocalConnectionString);
            UpdateData(listBoxEdit);
            listBoxEdit.SelectedIndex = ((IList)listBoxEdit.DataContext).Count - 1;
            listBoxEdit.ScrollIntoView(listBoxEdit.SelectedItem);
        }

        public static void DeleteData(ListBoxEdit listBoxEdit)
        {
            MessageBoxResult result = MessageBox.Show("Удалить МЭК - " + ((DynamicBaseClass)listBoxEdit.SelectedItem).GetValue("ExpName"), "Удаление МЭКа" + "?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Reader2List.CustomExecuteQuery($"Delete Yamed_ExpSpr_ExpAlg Where Id = {((DynamicBaseClass) listBoxEdit.SelectedItem).GetValue("ID")}",
                    SprClass.LocalConnectionString);
                UpdateData(listBoxEdit);
            }
        }

        public static void SaveAllData(ListBoxEdit listBoxEdit)
        {
            MessageBoxResult result = MessageBox.Show("Сохранить изменения в МЭК?", "Сохранение МЭКа", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Reader2List.CustomExecuteQuery(Reader2List.CustomUpdateCommand("Yamed_ExpSpr_ExpAlg", (IList)listBoxEdit.DataContext, "Id"), SprClass.LocalConnectionString);
                UpdateData(listBoxEdit);
            }
        }

        public static void SaveData(ListBoxEdit listBoxEdit)
        {
            Reader2List.CustomExecuteQuery(Reader2List.CustomUpdateCommand("Yamed_ExpSpr_ExpAlg", listBoxEdit.SelectedItem, "ID"), SprClass.LocalConnectionString);
            UpdateData(listBoxEdit);
        }
    }
}
