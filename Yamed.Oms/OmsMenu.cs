using System;
using System.Collections.ObjectModel;
using System.Windows;
using DevExpress.Xpf.Core;
using GalaSoft.MvvmLight.Command;
using Yamed.Control;
using Yamed.OmsExp.ExpEditors;

namespace Yamed.Oms
{
    public class OmsMenu
    {
        public ObservableCollection<MenuElement> MenuElements;

        public OmsMenu()
        {
            MenuElements = new ObservableCollection<MenuElement>()
            {
                new MenuElement
                {
                    Content = "Поиск по параметрам",
                    Glyph = new Uri("/Yamed.Icons;component/Icons/1472130348_134.png", UriKind.Relative),
                    Command = new RelayCommand(() =>
                        {
                            var window = new DXWindow
                            {
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                                Content = new SearchControl(),
                                Title = "Поиск по параметрам",
                                Width = 350,
                                SizeToContent = SizeToContent.Height
                            };
                            window.ShowDialog();
                        },
                        () => true)
                },
                //new MenuElement
                //{
                //    Content = "Выгрузка сведений об оплате",
                //    Glyph = new Uri("/Yamed.Icons;component/Icons/investor_money-512.png", UriKind.Relative),
                //    Command = new RelayCommand(() =>
                //        {
                //            SankExport_OnClick();
                //        },
                //        () => true)
                //},
            };

        }
    }
}
