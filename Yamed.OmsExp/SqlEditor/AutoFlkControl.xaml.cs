using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Yamed.Control;
using Yamed.Core;
using Yamed.Entity;
using Yamed.Server;

namespace Yamed.OmsExp.SqlEditor
{

    public class FlcProcess
    {
        public static string Obrezka(string str, int count)
        {
            if (str != null && str.Length > count)
                return str.Substring(0, count);
            return str;
        }

        public static MemoryStream ExportToXml(object sankExpList, string ifn)
        {
            XmlWriterSettings writerSettings
                = new XmlWriterSettings()
                {
                    Encoding = Encoding.GetEncoding("windows-1251"),
                    Indent = true,
                    IndentChars = "     ",
                    NewLineChars = Environment.NewLine,
                    ConformanceLevel = ConformanceLevel.Document
                };

            MemoryStream ms = new MemoryStream();

            using (ZipFile zip = new ZipFile())
            {
                var stream1 = new MemoryStream();

                using (XmlWriter writer1 = XmlWriter.Create(stream1, writerSettings))
                {
                    // writer1
                    writer1.WriteStartElement("FLK_P");
                    writer1.WriteElementString("FNAME", "V" + ifn.Remove(0, 1));
                    writer1.WriteElementString("FNAME_I", ifn);

                    List2Xml("PR", (IEnumerable<dynamic>)sankExpList, writer1);

                    writer1.WriteEndElement();
                    writer1.Flush();
                    writer1.Close();
                }

                string result1 = Encoding.Default.GetString(stream1.ToArray());
                zip.AddEntry("V" + ifn.Remove(0, 1) + ".xml", result1);

                zip.Save(ms);
            }
            return ms;
        }

        public static MemoryStream ExportOspToXml(object sankExpList, string ifn)
        {
            XmlWriterSettings writerSettings
                = new XmlWriterSettings()
                {
                    Encoding = Encoding.GetEncoding("windows-1251"),
                    Indent = true,
                    IndentChars = "     ",
                    NewLineChars = Environment.NewLine,
                    ConformanceLevel = ConformanceLevel.Document
                };

            MemoryStream ms = new MemoryStream();

            using (ZipFile zip = new ZipFile())
            {
                var stream1 = new MemoryStream();

                using (XmlWriter writer1 = XmlWriter.Create(stream1, writerSettings))
                {
                    // writer1
                    writer1.WriteStartElement("ELMEDICINE");
                    //writer1.WriteElementString("FNAME", "O" + ifn.Remove(0, 1));
                    //writer1.WriteElementString("FNAME_I", ifn);

                    List2Xml("SLUCH", (IEnumerable<dynamic>)sankExpList, writer1);

                    writer1.WriteEndElement();
                    writer1.Flush();
                    writer1.Close();
                }

                string result1 = Encoding.Default.GetString(stream1.ToArray());
                zip.AddEntry("O" + ifn.Remove(0, 1) + ".xml", result1);

                zip.Save(ms);
            }
            return ms;
        }

        private static void List2Xml(string startElement, IEnumerable<dynamic> objects, XmlWriter writer)
        {

            foreach (var exp in objects)
            {
                writer.WriteStartElement(startElement);
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
                    if (!string.IsNullOrEmpty(value)) writer.WriteElementString(name, value);
                }
                writer.WriteEndElement();
            }
        }
    }

    /// <summary>
    /// Логика взаимодействия для AutoMekControl.xaml
    /// </summary>
    public partial class AutoFlkControl : UserControl
    {
        private bool _isLpu;
        private object[] _schets;

        public AutoFlkControl(object[] schets)
        {
            InitializeComponent();
            _schets = schets;

            try
            {
                //using (var dc = new ElmedDataClassesDataContext(SprClass.LocalConnectionString))
                {
                    AutoFlkElement.FlkList.DataContext = SqlReader.Select("Select * From FLK_Algorithms where prenable = 1 ", SprClass.LocalConnectionString);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

        }




        private void AutoFlkStart()
        {
            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext(); //get UI thread context 
            List<object> flkList;
            flkList = AutoFlkElement.FlkList.SelectedItems.Select(x => x).ToList();
            AutoFlkStartButton.IsEnabled = false;
            AutoFlkElement.FlkList.IsEnabled = false;

            var flkTask = Task.Factory.StartNew(() =>
            {
                foreach (var sc in _schets)
                {
                    var rid = ObjHelper.ClassConverter<D3_SCHET_OMS>(sc);
                    var ifn = rid.OmsFileName;
                    var id = rid.ID;
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        AutoFlkElement.LogBox.Text += "Проверка " + "#" +
                                                   SprClass.LpuList.SingleOrDefault(
                                                       x => x.mcod == rid.CODE_MO)?.NameWithID + "# запущена" + Environment.NewLine + Environment.NewLine;
                    });
                    try
                    {

                        Reader2List.CustomExecuteQuery($@"
                UPDATE sc SET FLK_STATUS = 100
                From D3_SCHET_OMS sc
                where sc.ID = {id}", SprClass.LocalConnectionString);

                        int flkcnt = 0;


                        foreach (DynamicBaseClass flk in flkList)
                        {
                            var alg = (string)flk.GetValue("AlgSql");
                            Dispatcher.BeginInvoke((Action)delegate ()
                            {
                                AutoFlkElement.LogBox.Text += $"Файл: { ifn}, ID: { id}. { flk.GetValue("AlgName")}" + Environment.NewLine;
                            });
                            //Console.WriteLine($"Файл: {ifn}, ID: {id}. {flk.GetValue("AlgName")}");

                            bool deadlock;
                            IList errList = null;

                            do
                            {
                                try
                                {
                                    //if (alg.Contains("where USL_TIP in (2,4) and lp.ID is null"))
                                    //    {
                                    //    var x = 1;
                                    //}
                                    errList =
                                        (IList)Reader2List.CustomAnonymousSelect(alg.Replace("@p1", id.ToString()), SprClass.LocalConnectionString);
                                    flkcnt += errList.Count;

                                    deadlock = false;

                                }
                                catch (SqlException ex) // This example is for SQL Server, change the exception type/logic if you're using another DBMS
                                {
                                    if (ex.Number == 1205)  // SQL Server error code for deadlock
                                    {
                                        deadlock = true;
                                    }
                                    else
                                    {
                                        Dispatcher.BeginInvoke((Action)delegate ()
                                        {
                                            AutoFlkElement.LogBox.Text += $"Файл: {ifn}, ID: {id} " +
                                                          $"Ошибка при проверке {flk.GetValue("AlgName")}" + ex + Environment.NewLine + Environment.NewLine;
                                        });

                                        Reader2List.CustomExecuteQuery($@"Update D3_SCHET_OMS SET FLK_STATUS=17, FLK_COMMENT='{flk.GetValue("AlgName") + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException?.Message}' 
                            WHERE ID = {id}", SprClass.LocalConnectionString);

                                        //deadlock = false;
                                        return;
                                    }
                                }
                            } while (deadlock);

                            try
                            {
                                if (errList?.Count > 0)
                                {
                                    string ids = "";
                                    foreach (var err in errList)
                                    {
                                        ids = ids + ObjHelper.GetAnonymousValue(err, "ID").ToString() + ",";
                                    }
                                    ids = ids.Substring(0, ids.Length - 1);

                                    var errIns = $@"
	INSERT INTO FLK_RSLT 
		(
			SCHET_ID,
			OSHIB,IM_POL,BAS_EL,
			N_ZAP,IDCASE,IDSERV,
			COMMENT,COMMENT2
		)

SELECT {id} SCHET_ID,
        '{flk.GetValue("FlkErrId")}' OSHIB,
        '{flk.GetValue("XmlItem")}' IM_POL,
        '{flk.GetValue("XmlNode")}' BAS_EL,
        pa.N_ZAP, z.IDCASE, NULL IDSERV,
        '{flk.GetValue("AlgComment")}' COMMENT, 
        'ZSL_ID: ' + z.ZSL_ID COMMENT2
    from D3_ZSL_OMS z
	join D3_PACIENT_OMS pa on z.D3_PID = pa.ID
	where z.ID in ({ids})";
                                    //   Reader2List.CustomExecuteQuery(errIns, _connectionString);
                                    string result = inDataBase(errIns, "Ошибка добавления FLK_RSLT");
                                    if (result != "Done")
                                    {
                                        throw new Exception(result);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Dispatcher.BeginInvoke((Action)delegate ()
                                {
                                    AutoFlkElement.LogBox.Text += $"Файл: {ifn}, ID: {id} " +
                                    $"Ошибка при записи ошибок {flk.GetValue("AlgName")}" + ex + Environment.NewLine + Environment.NewLine;
                                });

                                Reader2List.CustomExecuteQuery(
            $@"Update D3_SCHET_OMS SET FLK_STATUS=18, FLK_COMMENT='{flk.GetValue("AlgName") + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException?.Message}' 
                            WHERE ID = {id}", SprClass.LocalConnectionString);

                                return;
                            }

                        }
                        var status = flkcnt > 0 ? 20 : 800;

                        Reader2List.CustomExecuteQuery($@"
                UPDATE sc SET FLK_STATUS = {status}
                From D3_SCHET_OMS sc
                where sc.ID = {id}"
                , SprClass.LocalConnectionString);

                        if (status == 20)
                        {
                            FlkRecord((int)id, ifn);
                        }

                        Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            AutoFlkElement.LogBox.Text += $"!!! Файл: {ifn}, ID: {id}. Обработан ФЛК. Ошибок ФЛК: {flkcnt}" + Environment.NewLine + Environment.NewLine;
                        });



                    }
                    finally
                    {

                    }
                }
            });
            flkTask.ContinueWith(x =>
            {
                AutoFlkStartButton.IsEnabled = true;
                AutoFlkElement.FlkList.IsEnabled = true;
                MessageBox.Show("Проверка завершена. Файл(ы) с ошибками записан(ы) в базу данных.");

            }, uiScheduler);
            GC.WaitForPendingFinalizers();
            GC.Collect();
            //---------------------------------------------------------------------------------------------------------------------------------
            //            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext(); //get UI thread context 
            //            //List<object> flkList;
            //            //flkList = AutoFlkElement.FlkList.SelectedItems.Select(x => x).ToList();

            //            AutoFlkStartButton.IsEnabled = false;
            //            AutoFlkElement.FlkList.IsEnabled = false;

            //            var mekTask = Task.Factory.StartNew(() =>
            //            {
            //                try
            //                {
            //                    foreach (var sc in _schets)
            //                    {
            //                        var rid = ObjHelper.ClassConverter<D3_SCHET_OMS>(sc);


            //                            Dispatcher.BeginInvoke((Action)delegate ()
            //                            {
            //                                AutoFlkElement.LogBox.Text += "Проверка " + "#" +
            //                                                           SprClass.LpuList.SingleOrDefault(
            //                                                               x => x.mcod == rid.CODE_MO)?.NameWithID +
            //                                                           "# запущена" + Environment.NewLine + Environment.NewLine;
            //                            });



            //                            foreach (DynamicBaseClass flk in flkList)
            //                            {
            //                                List<MekGuid> listGuid = new List<MekGuid>();

            //                                Dispatcher.BeginInvoke((Action)delegate ()
            //                                {
            //                                    AutoFlkElement.LogBox.Text += "Запущена экспертиза - " + (string)flk.GetValue("AlgName") +
            //                                                               Environment.NewLine;
            //                                });
            //                                SqlConnection con =
            //                                    new SqlConnection(SprClass.LocalConnectionString);
            //                                SqlCommand cmd = new SqlCommand(((string)flk.GetValue("AlgSql")).Replace("@p1", rid.ID.ToString()), con);
            //                                //cmd.CommandType = CommandType.StoredProcedure;
            //                                con.Open();

            //                                cmd.CommandTimeout = 0;
            //                                cmd.Prepare();
            //                                IDataReader dr = cmd.ExecuteReader();
            //                                while (dr.Read())
            //                                {
            //                                    MekGuid mekGuid = new MekGuid();
            //                                    mekGuid.ID = dr.GetInt32(0);
            //                                    object sumv = dr.GetValue(1);
            //                                    mekGuid.SUMV = sumv as decimal? ?? 0;
            //                                    if (dr.FieldCount == 3)
            //                                        mekGuid.Com = (string)dr.GetValue(2);
            //                                    listGuid.Add(mekGuid);
            //                                }
            //                                dr.Close();
            //                                con.Close();

            //                                //listGuid = dc.ExecuteQuery<MekGuid>(mek.AlgSql.Replace("@p1", schet_id.ToString())
            //                                //    .Replace("@srz1", srz)).ToList();
            //                                int count = listGuid.Count;
            //                                Dispatcher.BeginInvoke((Action)delegate ()
            //                                {
            //                                    AutoMekElement1.LogBox.Text += "Найдено - " + count + Environment.NewLine;
            //                                    if (count > 0 && (string)mek.GetValue("AlgSqlParametr") != "nosank") AutoMekElement1.LogBox.Text += "Идет снятие" + Environment.NewLine;
            //                                });
            //                                if (count > 0 && (string)mek.GetValue("AlgSqlParametr") != "nosank")
            //                                {
            //                                    //sb = new StringBuilder();
            //                                    List<D3_SANK_OMS> sankList = new List<D3_SANK_OMS>();
            //                                    foreach (MekGuid mekGuid in listGuid)
            //                                    {
            //                                        D3_SANK_OMS sank = new D3_SANK_OMS();
            //                                        sank.S_CODE = Guid.NewGuid().ToString();
            //                                        sank.D3_ZSLID = mekGuid.ID;
            //                                        sank.S_SUM = mekGuid.SUMV;
            //                                        sank.S_OSN = (string)mek.GetValue("MekOsn");
            //                                        sank.S_TIP = 1;
            //                                        sank.S_TIP2 = 1;
            //                                        sank.S_IST = 1;
            //                                        sank.USER_ID = SprClass.userId;
            //                                        sank.S_DATE = SprClass.WorkDate;
            //                                        sank.S_COM = !string.IsNullOrEmpty(mekGuid.Com)
            //                                            ? (string)mek.GetValue("AlgComment") + Environment.NewLine + mekGuid.Com
            //                                            : (string)mek.GetValue("AlgComment");
            //                                        sank.S_ZAKL = !string.IsNullOrEmpty(mekGuid.Com)
            //                                            ? (string)mek.GetValue("AlgComment") + Environment.NewLine + mekGuid.Com
            //                                            : (string)mek.GetValue("AlgComment");
            //                                        sank.D3_SCID = rid.ID;
            //                                        sankList.Add(sank);
            //                                        //SluchUpdateContructor(mek.AlgComment, mekGuid.ID);
            //                                    }

            //                                    Reader2List.BulkInsert(sankList, 100, SprClass.LocalConnectionString);
            //                                    //if (sb.Length > 0) SluchUpdate(sb.ToString());
            //                                }
            //                                if (count > 0 && (string)mek.GetValue("AlgSqlParametr") == "nosank")
            //                                {
            //                                    sb = new StringBuilder();
            //                                    foreach (MekGuid mekGuid in listGuid)
            //                                    {
            //                                        SluchUpdateContructor((string)mek.GetValue("AlgComment"), mekGuid.ID);
            //                                    }
            //                                    if (sb.Length > 0) SluchUpdate(sb.ToString());
            //                                }
            //                                Dispatcher.BeginInvoke((Action)delegate ()
            //                                {
            //                                    AutoMekElement1.LogBox.Text += "Экспертиза завершена" + Environment.NewLine +
            //                                                               Environment.NewLine;
            //                                });
            //                                //}
            //                            }


            //                        if (!rMek && !_isLpu)
            //                        {
            //                            Dispatcher.BeginInvoke((Action)delegate ()
            //                            {
            //                                AutoMekElement1.LogBox.Text += "Перерасчет сумм счета" + Environment.NewLine;
            //                            });
            //                            decimal? ss = 0;

            //                            Reader2List.CustomExecuteQuery($@"
            //EXEC p_oms_calc_sank {rid.ID}
            //EXEC p_oms_calc_schet {rid.ID}
            //", SprClass.LocalConnectionString);
            //                            //using (var dc = new ElmedDataClassesDataContext(SprClass.LocalConnectionString))
            //                            //{
            //                                //dc.CommandTimeout = 0;
            //                                //dc.Sank_Calc(rid.ID);
            //                                //dc.Sluch_Calc(rid.ID);
            //                                //dc.Mek_Calc(rid.ID);
            //                                //dc.Schet_Calc(rid.ID);
            //                                //var sch = dc.D3_SCHET_OMS.Single(x => x.ID == rid.ID);
            //                                //ss = sch.SANK_MEK;
            //                            //}



            //                            Dispatcher.BeginInvoke((Action)delegate ()
            //                            {
            //                                AutoMekElement1.LogBox.Text += "Сумма санкций - " + (ss) + Environment.NewLine +
            //                                                           Environment.NewLine;
            //                            });
            //                        }
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    Dispatcher.BeginInvoke((Action)delegate ()
            //                    {
            //                        ErrorGlobalWindow.ShowError(ex.Message);
            //                    });
            //                }
            //            });
            //            mekTask.ContinueWith(x =>
            //            {
            //                AutoMekStartButton.IsEnabled = true;
            //                AutoMekElement1.MekList.IsEnabled = true;
            //                AutoMekElement1.PolisCheckListBoxEdit.IsEnabled = true;
            //                AutoMekElement2.MekList.IsEnabled = true;
            //                AutoMekElement2.PolisCheckListBoxEdit.IsEnabled = true;
            //                MessageBox.Show("Проверка завершена");
            //                //_elReestrWindow.linqInstantFeedbackDataSource.Refresh();
            //            }, uiScheduler);
        }
        private void FlkRecord(int id, string ifn)
        {
            //var flkTask = Task.Factory.StartNew(() =>
            //{
            var schet =
            (IList)
            Reader2List.CustomAnonymousSelect($@"Select [OSHIB],[IM_POL],[BAS_EL],[N_ZAP],[IDCASE],[IDSERV],[COMMENT],[COMMENT2]
from FLK_RSLT where SCHET_ID={id}", SprClass.LocalConnectionString);


            if (schet.Count == 0)
            {
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    AutoFlkElement.LogBox.Text += "Не найден реестр " + id + Environment.NewLine;
                });

                return;
            }

            var flkms = FlcProcess.ExportToXml(schet, ifn.Replace(".xml", ""));
            Reader2List.CustomExecuteQuery(
                $@"delete REG_PROTOCOLS_FILES where SCID = {id} and CHK = 'F'", SprClass.LocalConnectionString);

            Reader2List.CustomExecuteQuery(
                $@"declare @mo NVARCHAR(6)
                    select @mo = CODE_MO FROM d3_schet_oms where ID = {id}
                    insert into
                       REG_PROTOCOLS_FILES
                            (SCID, MO, FNAME, FZIP, CHK, STATUS)
                        VALUES
                        (
                            {id}
                            , @mo
                            , '{"V" + ifn.Remove(0, 1).Replace(".xml", "")}'
                            , {"0x" + BitConverter.ToString(flkms.ToArray()).Replace("-", "")}
                            , 'F'
                            , 20 
                        )
                    update D3_SCHET_OMS set FLK_STATUS = 20 where ID = {id}"
                , SprClass.LocalConnectionString);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                AutoFlkElement.LogBox.Text += "Записан ФЛК" + id + Environment.NewLine;
            });


            //Console.WriteLine("Записан ФЛК" + id);

            flkms.Dispose();
            flkms.Close();

            GC.WaitForPendingFinalizers();
            GC.Collect();
            //});
        }

        private static string inDataBase(string Command, string ErrorMessage)
        {
            int countIteration = 0;
            while (true)
            {
                SqlTransaction tr = null;
                try
                {
                    //   Reader2List.CustomExecuteQuery(errIns, _connectionString);
                    using (SqlConnection connect = new SqlConnection(SprClass.LocalConnectionString))
                    {

                        connect.Open();
                        tr = connect.BeginTransaction();
                        SqlCommand command = new SqlCommand(Command, connect);
                        command.CommandTimeout = 0;
                        command.Transaction = tr;
                        command.ExecuteNonQuery();
                        tr.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    tr.Rollback();
                    if (ex.Number == 1205)  // SQL Server error code for deadlock
                    {

                        if (countIteration > 10)
                        {
                            return ex.Message;
                        }
                        countIteration++;
                        //Console.WriteLine($"{ErrorMessage}. Попытка № {countIteration}");
                        Thread.Sleep(10000);
                        continue;
                    }
                    else
                    {
                        return ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                break;

            }
            return "Done";
        }




        private StringBuilder sb;
        private StringBuilder sb2;
        private void SluchUpdateContructor(string coment, int id)
        {
            sb.AppendLine(string.Format(@"UPDATE D3_ZSL_OMS SET MEK_COMENT = '{0}' WHERE ID = {1} AND (MEK_COMENT IS NULL OR MEK_COMENT = '')", coment, id));
        }

        private void SluchUpdateContructor2(string coment, int id)
        {
            sb2.AppendLine(string.Format(@"UPDATE D3_ZSL_OMS SET OSP_COMENT = '{0}' WHERE ID = {1}", coment, id));
        }
        static public void SluchUpdate(string command)
        {
            using (SqlConnection con = new SqlConnection(SprClass.LocalConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(command, con))
                {
                    con.Open();
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        static public string EFix(string eFix)
        {
            var ef = eFix.Replace("Е", "@").Replace("Ё", "@");
            return ef.Replace("@", "[ЕЁ]");
        }

        public static string ConvertSmo(string smoCod)
        {
            switch (smoCod)
            {
                case "46001":
                    return "Недействующий полис";
                case "46002":
                    return "ВТБ МС";
                case "46003":
                    return "ИНГОССТРАХ-М";
                case "46004":
                    return "СПАССКИЕ ВОРОТА-М";
                case "46006":
                    return "ИНКО-МЕД";
                default:
                    return smoCod;
            }
        }

        private void AutoMekStartButton_Click(object sender, RoutedEventArgs e)
        {
            //AutoMekStart();
        }
        private void AutoFlkStartButton_Click(object sender, RoutedEventArgs e)
        {
            AutoFlkStart();
        }
    }
}
