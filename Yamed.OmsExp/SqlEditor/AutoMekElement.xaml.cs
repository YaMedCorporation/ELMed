using System;
using System.Windows.Controls;
using Yamed.Server;

namespace Yamed.OmsExp.SqlEditor
{
    /// <summary>
    /// Логика взаимодействия для MekElementControl.xaml
    /// </summary>
    public partial class AutoMekElement : UserControl
    {
        public AutoMekElement()
        {
            InitializeComponent();
            if (SprClass.Region == "37")
            {
                TextBlock1.Visibility = System.Windows.Visibility.Collapsed;
                TextBlock9.Visibility = System.Windows.Visibility.Collapsed;
                PolisCheckListBoxEdit.Visibility = System.Windows.Visibility.Collapsed;
                AttachedCheckListBoxEdit.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void LogBox_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LogBox.Focus();
            Dispatcher.BeginInvoke(new Action(() => LogBox.SelectionStart = LogBox.Text.Length));
        }

    }
}
