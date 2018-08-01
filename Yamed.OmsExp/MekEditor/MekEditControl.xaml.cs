using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Win32;

namespace Yamed.OmsExp.MekEditor
{
    /// <summary>
    /// Логика взаимодействия для MekEditControl.xaml
    /// </summary>
    public partial class MekEditControl : UserControl
    {
        public MekEditControl()
        {
            InitializeComponent();
        }

        private void SqlScriptRun_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = ((TabElement)СommonСomponents.DxTabObject).MyControl;
            //if (tab is MedicalEconomicControl.SqlEditorControl)
            //{
                //var tab1 = (MedicalEconomicControl.SqlEditorControl)tab;
                SqlExecute.GetData(ControlRule.GridControl1, ControlRule.sqlEditor.Text);
            //}
            //else if (tab is CustomStatistic)
            //{
            //    var tab1 = (CustomStatistic)tab;
            //    Statistic.SqlExecute.GetData(tab1.GridControl1, tab1.sqlEditor.Text);
            //}
            //else
            //{
            //    var tab1 = (MedicalExpertQuery)tab;
            //    MedicalExperts.SqlExecute.GetData(tab1.GridControl1, tab1.sqlEditor.Text);

            //}

        }

        private void RefreshTbl_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = ((TabElement)СommonСomponents.DxTabObject).MyControl;
            //if (tab is MedicalEconomicControl.SqlEditorControl)
            //{
            //    var tab1 = (MedicalEconomicControl.SqlEditorControl)tab;
                SqlExecute.UpdateData(ControlRule.MekList);
            //}
            //else if (tab is CustomStatistic)
            //{
            //    var tab1 = (CustomStatistic)tab;
            //    Statistic.SqlExecute.UpdateData(tab1.StatList);
            //}
            //else if (tab is MedicalExpertQuery)
            //{
            //    var tab1 = (MedicalExpertQuery)tab;
            //    MedicalExperts.SqlExecute.UpdateData(tab1.MeeQueryList1);
            //}

        }

        private void AddItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = ((TabElement)СommonСomponents.DxTabObject).MyControl;

            //if (tab is MedicalEconomicControl.SqlEditorControl)
            //{
            //var tab1 = (MedicalEconomicControl.SqlEditorControl)tab;
            SqlExecute.AddData(ControlRule.MekList);
            //}
            //else if (tab is CustomStatistic)
            //{
            //    var tab1 = (CustomStatistic)tab;
            //    Statistic.SqlExecute.AddData(tab1.StatList);
            //}
            //else if (tab is MedicalExpertQuery)
            //{
            //    var tab1 = (MedicalExpertQuery)tab;
            //    MedicalExperts.SqlExecute.AddData(tab1.MeeQueryList1);
            //}
        }

        private void CopyItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = ((TabElement)СommonСomponents.DxTabObject).MyControl;
            //if (tab is MedicalEconomicControl.SqlEditorControl)
            //{
            //    var tab1 = (MedicalEconomicControl.SqlEditorControl)tab;
                SqlExecute.CopyData(ControlRule.MekList);
            //}
            //else if (tab is CustomStatistic)
            //{
            //    var tab1 = (CustomStatistic)tab;
            //    Statistic.SqlExecute.CopyData(tab1.StatList);
            //}
            //else if (tab is MedicalExpertQuery)
            //{
            //    var tab1 = (MedicalExpertQuery)tab;
            //    MedicalExperts.SqlExecute.CopyData(tab1.MeeQueryList1);
            //}
        }


        private void DelItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = ((TabElement)СommonСomponents.DxTabObject).MyControl;
            //if (tab is MedicalEconomicControl.SqlEditorControl)
            //{
            //    var tab1 = (MedicalEconomicControl.SqlEditorControl)tab;
                SqlExecute.DeleteData(ControlRule.MekList);
            //}
            //else if (tab is CustomStatistic)
            //{
            //    var tab1 = (CustomStatistic)tab;
            //    Statistic.SqlExecute.DeleteData(tab1.StatList);
            //}
            //else if (tab is MedicalExpertQuery)
            //{
            //    var tab1 = (MedicalExpertQuery)tab;
            //    MedicalExperts.SqlExecute.DeleteData(tab1.MeeQueryList1);
            //}
        }

        private void SaveItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = ((TabElement)СommonСomponents.DxTabObject).MyControl;
            //if (tab is MedicalEconomicControl.SqlEditorControl)
            //{
            //    var tab1 = (MedicalEconomicControl.SqlEditorControl)tab;
                SqlExecute.SaveData(ControlRule.MekList);
            //}
            //else if (tab is CustomStatistic)
            //{
            //    var tab1 = (CustomStatistic)tab;
            //    Statistic.SqlExecute.SaveData(tab1.StatList);
            //}
            //else if (tab is MedicalExpertQuery)
            //{
            //    var tab1 = (MedicalExpertQuery)tab;
            //    MedicalExperts.SqlExecute.SaveData(tab1.MeeQueryList1);
            //}
        }

        private void SaveAllItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = ((TabElement)СommonСomponents.DxTabObject).MyControl;
            //if (tab is MedicalEconomicControl.SqlEditorControl)
            //{
            //    var tab1 = (MedicalEconomicControl.SqlEditorControl)tab;
                SqlExecute.SaveAllData(ControlRule.MekList);
            //}
            //else if (tab is CustomStatistic)
            //{
            //    var tab1 = (CustomStatistic)tab;
            //    Statistic.SqlExecute.SaveAllData(tab1.StatList);
            //}
            //else if (tab is MedicalExpertQuery)
            //{
            //    var tab1 = (MedicalExpertQuery)tab;
            //    MedicalExperts.SqlExecute.SaveAllData(tab1.MeeQueryList1);
            //}
        }

        private void ExportXlsxItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //var tab = ((TabElement)СommonСomponents.DxTabObject).MyControl;
            //if (tab is MedicalEconomicControl.SqlEditorControl)
            //{
                //var tab1 = (MedicalEconomicControl.SqlEditorControl)tab;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";

                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    ControlRule.TableView1.ExportToXlsx(saveFileDialog.FileName);
                    Process.Start(saveFileDialog.FileName);
                }
            //}
            //else if (tab is MedicalExperts.MedicalExpertQuery)
            //{
            //    var tab1 = (MedicalExpertQuery)tab;
            //    SaveFileDialog saveFileDialog = new SaveFileDialog();
            //    saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";

            //    bool? result = saveFileDialog.ShowDialog();
            //    if (result == true)
            //    {
            //        tab1.TableView1.ExportToXlsx(saveFileDialog.FileName);
            //        Process.Start(saveFileDialog.FileName);
            //    }
            //}
            //else
            //{
            //    var tab1 = (CustomStatistic)tab;
            //    SaveFileDialog saveFileDialog = new SaveFileDialog();
            //    saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";

            //    bool? result = saveFileDialog.ShowDialog();
            //    if (result == true)
            //    {
            //        tab1.TableView1.ExportToXlsx(saveFileDialog.FileName);
            //        Process.Start(saveFileDialog.FileName);
            //    }
            //}
        }


    }
}
