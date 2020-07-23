using System;
using System.Windows.Controls;

namespace Yamed.OmsExp.SqlEditor
{
    /// <summary>
    /// Логика взаимодействия для MekElementControl.xaml
    /// </summary>
    public partial class AutoFlkElement : UserControl
    {
        public AutoFlkElement()
        {
            InitializeComponent();
        }

        private void LogBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LogBox.Focus();
            Dispatcher.BeginInvoke(new Action(() => LogBox.SelectionStart = LogBox.Text.Length));
        }

    }
}
