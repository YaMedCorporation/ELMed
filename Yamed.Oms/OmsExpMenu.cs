using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using DevExpress.Xpf.Core;
using GalaSoft.MvvmLight.Command;
using Ionic.Zip;
using Microsoft.Win32;
using Yamed.Control;
using Yamed.OmsExp;
using Yamed.OmsExp.SqlEditor;
using Yamed.Server;

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
                    Content = "Алгоритмы МЭК",
                    Glyph = new Uri("/Yamed.Icons;component/Icons/1472140320_settings-24.png", UriKind.Relative),
                    Command = new RelayCommand(() =>
                        {
                            var window = new DXWindow
                            {
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                                Content = new SqlEditControl(),
                                Title = "Алгоритмы МЭК",
                                Width = 1024,
                                Height = 768
                            };
                            window.ShowDialog();
                        },
                        () => true)
                },
                new MenuElement
                {
                    Content = "Лицензии",
                    Glyph = new Uri("/Yamed.Icons;component/Icons/certificate_diploma_folded_achievement_license_graduation_graduate-256.png", UriKind.Relative),
                    Command = new RelayCommand(() =>
                        {
                            var window = new DXWindow
                            {
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                                Content = new LicSpr(),
                                Title = "Лицензии",
                                Width = 1024,
                                Height = 768
                            };
                            window.ShowDialog();
                        },
                        () => true)
                },
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
                new MenuElement
                {
                    Content = "Выгрузка сведений об оплате",
                    Glyph = new Uri("/Yamed.Icons;component/Icons/investor_money-512.png", UriKind.Relative),
                    Command = new RelayCommand(() =>
                        {
                            SankExport_OnClick();
                        },
                        () => true)
                },
            };

    }
        private void SankExport_OnClick()
        {
            ExportToXml(ExportExp(2019, "1,2,3,4,5,6,7,8,9,10,11,12"));
        }

        static object ExportExp(int year, string month)
        {
            string tempQuery = String.Format(@"
DROP TABLE SANK_EXP_TT
SELECT sl.LPU CODE_MO, sc.[YEAR], sc.[MONTH], sc.NSCHET, sc.DSCHET, sc.PLAT,
pa.NPOLIS, sl.USL_OK, OS_SLUCH_REGION,
sl.ZSL_ID, s.S_CODE, (CASE WHEN s.S_TIP = 3 THEN (SELECT TOP 1 KOD FROM ExpertsDB WHERE id = k.ExpertID) ELSE NULL END)S_EXP_CODE,
s.S_SUM S_SUM,
s.S_SUM2 SUM_MULCT,
s.S_TIP, (CASE WHEN s.S_OSN IS NOT NULL THEN (SELECT TOP 1 Kod FROM F014 WHERE Osn = s.S_OSN) ELSE NULL END) S_OSN, S_OSN S_OSN_TS,
s.S_COM, s.S_DATE, (CASE WHEN s.S_TIP = 3 THEN k.ZAKL WHEN s.S_TIP = 2 THEN k.ZAKL ELSE NULL END) S_ZAKL, EXP_TYPE S_IST
--into SANK_EXP_TT
FROM D3_ZSL_OMS sl
JOIN D3_PACIENT_OMS pa ON sl.D3_PID = pa.ID
JOIN D3_SCHET_OMS sc ON sc.ID = sl.D3_SCID
JOIN D3_SANK_OMS s ON s.D3_ZSLID = sl.ID --and s.S_TIP = 1
LEFT JOIN D3_AKT_MEE_TBL k ON k.SANKID = s.ID
WHERE sc.YEAR in (2018, {0}) AND sc.MONTH in ({1})
SELECT * FROM SANK_EXP_TT", year, month);
            return Reader2List.CustomAnonymousSelect(tempQuery, SprClass.LocalConnectionString);
        }

        static void ExportToXml(object sankExpList)
        {
            int nzap = 1;
            int idcase = 1;
            XmlWriterSettings writerSettings
                = new XmlWriterSettings()
                {
                    Encoding = Encoding.GetEncoding("windows-1251"),
                    Indent = true,
                    IndentChars = "     ",
                    NewLineChars = Environment.NewLine,
                    ConformanceLevel = ConformanceLevel.Document
                };
            using (ZipFile zip = new ZipFile())
            {
                var stream1 = new MemoryStream();

                using (XmlWriter writer1 = XmlWriter.Create(stream1, writerSettings))
                {
                    // writer1
                    writer1.WriteStartElement("ZL_LIST");
                    writer1.WriteStartElement("ZGLV");
                    writer1.WriteElementString("VERSION", "EXP01");
                    writer1.WriteElementString("DATA", DateTime.Today.ToString("yyyy-MM-dd"));
                    writer1.WriteElementString("FILENAME", "test");
                    writer1.WriteEndElement();

                    foreach (var exp in (IEnumerable<dynamic>)sankExpList)
                    {
                        writer1.WriteStartElement("EXP");
                        var obj = exp.GetType().GetProperties();
                        foreach (var property in obj)
                        {
                            var name = property.Name;
                            string value = null;
                            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            if (type == Type.GetType("System.String")) value = (string)property.GetValue(exp, null);
                            else if (type == Type.GetType("System.DateTime"))
                            {
                                var dv = property.GetValue(exp, null);
                                if (dv != null) value = ((DateTime)dv).ToString("yyyy-MM-dd");
                            }
                            else value = Convert.ToString(property.GetValue(exp, null), CultureInfo.InvariantCulture);
                            if (!string.IsNullOrEmpty(value)) writer1.WriteElementString(name, value);
                        }
                        writer1.WriteEndElement();

                    }
                    writer1.WriteEndElement();
                    writer1.Flush();
                    writer1.Close();
                }

                string result1 = Encoding.Default.GetString(stream1.ToArray());
                zip.AddEntry("test" + ".xml", result1);


                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "OMS File (*.oms)|*.oms";
                saveFileDialog.FileName = "test" + ".oms";

                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                    zip.Save(saveFileDialog.FileName);
            }
            //            ZL_LIST	ZGLV	О	S	Заголовок файла	Информация о передаваемом файле
            //    EXP	OV	S	Экспертизы	Сведения об экспертизах
            //Заголовок файла
            //ZGLV	VERSION	O	T(5)	Версия взаимодействия 	Текущей редакции соответствует значение «EXP01».
            //    DATA	О	D	Дата	В формате ГГГГ-ММ-ДД
            //    FILENAME	О	T(26)	Имя файла	Имя файла без расширения.
            //    SANK_MEK	У	N(15.2)	Финансовые санкции (МЭК)	Сумма, снятая с оплаты по результатам МЭК, заполняется после проведения МЭК.
            //    SANK_MEE	У	N(15.2)	Финансовые санкции (МЭЭ)	Сумма, снятая с оплаты по результатам МЭЭ, заполняется после проведения МЭЭ.
            //    SANK_EKMP	У	N(15.2)	Финансовые санкции (ЭКМП)	Сумма, снятая с оплаты по результатам ЭКМП, заполняется после проведения ЭКМП.

            //Сведения об экспертизах
            //EXP	CODE_MO	О	T(6)	Реестровый номер медицинской организации	Код МО – юридического лица. Заполняется в соответствии со справочником F003.
            //    YEAR	O	N(4)	Отчетный год	
            //    MONTH	O	N(2)	Отчетный месяц	В счёт могут включаться случаи лечения за предыдущие периоды, если ранее они были отказаны по результатам МЭК, МЭЭ, ЭКМП
            //    NSCHET	О	T(15)	Номер счёта	
            //    DSCHET	О	D	Дата выставления счёта	В формате ГГГГ-ММ-ДД
            //    PLAT	У	T(5)	Плательщик. Реестровый номер СМО. 	Заполняется в соответствии со справочником F002. 
            //При подаче в ТФОМС может не заполняться.
            //    ID	O	N(16)	Идентификатор случая	Уникален в пределах одной ЛПУ
            //    ID_SL	O	Т(36)	Уникальный  идентификатор случая	Уникален в пределах Курской области
            //    S_CODE	О	Т(36)	Идентификатор санкции	Уникален в пределах Курской области
            //    S_EXP_CODE	У	N(7)	Код эксперта	
            //    S_SUM	О	N(15.2)	Финансовая санкция	
            //    S_TIP	О	N(2)	Тип санкции	1 – МЭК,
            //2 – МЭЭ,
            //3 – ЭКМП.
            //11 – Реэкспертиза МЭК
            //12 – Реэкспертиза МЭЭ
            //13 – Реэкспертиза ЭКМП
            //    S_OSN	У	N(3)	Код причины отказа (частичной) оплаты	F014 Классификатор причин отказа в оплате медицинской помощи.
            //    S_OSN_TS	У	T(20)	Код причины отказа (частичной) оплаты по тарифному соглашению	
            //    S_COM	У	Т(250)	Комментарий	Комментарий к санкции.
            //    S_DATE	О	D	Дата экспертизы	Дата проведения экспертизы
            //    S_ZAKL	У	Т(2000)	Заключение	Заключение эксперта
            //    S_IST	О	N(1)	Источник	1 – СМО/ТФОМС к МО.


        }

    }
}
