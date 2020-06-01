using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DevExpress.Xpf.Core;
using Yamed.Core;
using Yamed.Entity;
using Yamed.Server;

namespace Yamed.Hospital
{
    /// <summary>
    /// Логика взаимодействия для HospitalEmrTab.xaml
    /// </summary>
    public partial class HospitalEmrPacientPanel : UserControl
    {
        private PACIENT _pacient;
        public object ter;
        public HospitalEmrPacientPanel(PACIENT pacient)
        {
            InitializeComponent();
            _pacient = pacient;
            _pacient.NOVOR = "0";
            GridPacient.DataContext = _pacient;

            typeUdlBox.DataContext = SprClass.passport;
            //smoOkatoBox.DataContext = SprClass.smoOkato;
            ter = Reader2List.CustomAnonymousSelect($@"
declare @reg varchar(2)
declare @tf_okato nvarchar(5) /* получаем окато текущей СМО. */
SELECT @tf_okato = tf_okato FROM [F002] where smocod = (select Parametr from Settings where name='CodeSMO')
select @reg=Parametr from Settings where name='Region'
if @tf_okato is null /* берём и МО. */
	SELECT @tf_okato = tf_okato FROM [F003] where mcod = (select Parametr from Settings where name='MedicalOrganization')
select left(@tf_okato,2)", SprClass.LocalConnectionString);
            okatoTerBox.DataContext = Reader2List.CustomAnonymousSelect($"Select * from O002 where kod1<>'000' and ter='{ter}'", SprClass.LocalConnectionString);

            wBox.DataContext = SprClass.sex;
            policyTypeBox.DataContext = SprClass.policyType;
            smoBox.DataContext = SprClass.smo;

            kodTerBox.DataContext = SprClass.KodTers;
            SocStatBox.DataContext = SprClass.SocStats;
            KatLgotBox.DataContext = SprClass.KatLgots;

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate()
                {
                    polisBox.Focus();
                }));

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ((DXWindow)this.Parent).Close();
        }

        private void PolisBox_OnDefaultButtonClick(object sender, RoutedEventArgs e)
        {
            ObservableCollection<DynamicBaseClass> srz = new ObservableCollection<DynamicBaseClass>();
            var enp = (string) polisBox.EditValue;

            //using (var dc = new ElmedOnLineDataContext(connectionString))
            //{
            //    var test = dc.AttachedPeople_ONLINE.First(x => x.RBODY != null);
            //    string b64 = Encoding.ASCII.GetString(test.RBODY.ToArray());
            //    var decr = Decrypt(b64);
            //}

            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext(); //get UI thread context 
            var peopTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    srz = SqlReader.Select(
                        $@"SELECT [FAM],[IM],[OT],[W],[DR],[DS],[Q],[SPOL],[NPOL],[ENP],[OPDOC] FROM [dbo].[PEOPLE] where npol = '{enp}' or enp = '{enp}' order by ID desc
", SprClass.GlobalSrzConnectionString);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                Dispatcher.BeginInvoke((Action)delegate ()
                {

                });
            });
            peopTask.ContinueWith(x =>
            {
                if (srz.Any())
                {
                    FamBox.EditValue = (string) srz[0].GetValue("FAM");
                    ImBox.EditValue = (string) srz[0].GetValue("IM");
                    OtBox.EditValue = (string) srz[0].GetValue("OT");
                    wBox.EditValue = (int?) srz[0].GetValue("W");
                    drBox.EditValue = (DateTime?) srz[0].GetValue("DR");
                    novorBox.EditValue = "0";
                    smoBox.EditValue = (string) srz[0].GetValue("Q");
                    policyTypeBox.EditValue = (int?) srz[0].GetValue("OPDOC");
                    polisBox.EditValue =
                        (int?) srz[0].GetValue("OPDOC") == 3
                            ? (string) srz[0].GetValue("ENP")
                            : (string) srz[0].GetValue("NPOL");
                }
            }, uiScheduler);

        }
    }
}
