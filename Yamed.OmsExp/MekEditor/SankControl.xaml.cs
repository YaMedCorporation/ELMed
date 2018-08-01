using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using Yamed.Control;
using Yamed.Core;
using Yamed.Entity;
using Yamed.Server;

namespace Yamed.OmsExp.MekEditor
{
    /// <summary>
    /// Логика взаимодействия для SankWindow.xaml
    /// </summary>
    public partial class SankControl : UserControl
    {
        private D3_SANK_OMS _sank;

        public SankControl(D3_SANK_OMS sank)
        {
            InitializeComponent();

            _sank = sank;
            

            KodOtkazaBox.DataContext = SprClass.Otkazs.Where(x=>x.Osn.StartsWith("5"));
            MekGrid.DataContext = _sank;
        }

        private bool _isGroupProcess;
        public SankControl(bool isGroupProcess)
        {
            InitializeComponent();
            _isGroupProcess = isGroupProcess;
            _sank = new D3_SANK_OMS() {S_DATE = SprClass.WorkDate};
            SankSumBox.IsEnabled = false;

            KodOtkazaBox.DataContext = SprClass.Otkazs.Where(x=>x.Osn.StartsWith("5"));
            MekGrid.DataContext = _sank;
        }

        //private string _tblName;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!_isGroupProcess)
            {
                Task.Factory.StartNew(() =>
                {
                    if (_sank.ID == 0)
                    {
                        _sank.ID = Reader2List.ObjectInsertCommand("D3_SANK_OMS", _sank, "ID",
                            SprClass.LocalConnectionString);
                    }
                    else
                    {
                        var upd = Reader2List.CustomUpdateCommand("D3_SANK_OMS", _sank, "ID");
                        Reader2List.CustomExecuteQuery(upd, SprClass.LocalConnectionString);
                    }

                    Reader2List.CustomExecuteQuery($@"
EXEC p_oms_calc_sank {_sank.D3_SCID}
EXEC p_oms_calc_schet {_sank.D3_SCID}
", SprClass.LocalConnectionString);

                }).ContinueWith(x =>
                {
                    (this.Parent as DXWindow)?.Close();

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    foreach (var row in DxHelper.LoadedRows)
                    {
                        var sank = ObjHelper.ClassConverter<D3_SANK_OMS>(_sank);
                        sank.S_CODE = Guid.NewGuid().ToString();
                        //sank.S_DATE = SprClass.WorkDate;
                        sank.S_SUM = (decimal) ObjHelper.GetAnonymousValue(row, "SUMV");
                        sank.D3_ZSLID = (int) ObjHelper.GetAnonymousValue(row, "ID");
                        sank.D3_SCID =  (int) ObjHelper.GetAnonymousValue(row, "D3_SCID");
                        sank.S_TIP = 1;

                        sank.ID = Reader2List.ObjectInsertCommand("D3_SANK_OMS", sank, "ID",
    SprClass.LocalConnectionString);
                        Reader2List.CustomExecuteQuery($@"
EXEC p_oms_calc_sank {sank.D3_SCID}
EXEC p_oms_calc_schet {sank.D3_SCID}
", SprClass.LocalConnectionString);

                    }


                }).ContinueWith(x =>
                {
                    (this.Parent as DXWindow)?.Close();

                }, TaskScheduler.FromCurrentSynchronizationContext());

                //cmd.AppendLine(
                //    String.IsNullOrWhiteSpace((string) ReqTextEdit.EditValue)
                //        ? $@"UPDATE D3_ZSL_OMS SET USER_COMENT = NULL WHERE ID = {ObjHelper.GetAnonymousValue(row,
                //            "ID")}"
                //        : $@"UPDATE D3_ZSL_OMS SET USER_COMENT = '{ReqTextEdit.EditValue}' WHERE ID = {ObjHelper
                //            .GetAnonymousValue(row, "ID")}");
            }


        }
        // private void BarButtonItem1_OnItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        // {
        //     if (_isGroupProcess)
        //     {
        //         List<SANK> list = new List<SANK>();
        //         foreach (var obj in (IEnumerable<dynamic>) _objs)
        //         {
        //             var o = obj;
        //             SANK s1 = new SANK();
        //             s1.S_CODE = Guid.NewGuid();
        //             s1.S_DATE = SprClass.WorkDate;
        //             s1.S_SUM = PublicVoids.GetAnonymousValue(o, "SUMV");
        //             s1.SLID = PublicVoids.GetAnonymousValue(o, "ID");
        //             s1.SCHET_ID = PublicVoids.GetAnonymousValue(o, "SCHET_ID");
        //             s1.S_TIP = 1;
        //             if (KodOtkazaBox.EditValue != null) s1.S_OSN = (string) KodOtkazaBox.EditValue;
        //             else s1.S_OSN = null;
        //             if (CommentBox.EditValue != null) s1.S_COM = (string) CommentBox.EditValue;
        //             else s1.S_COM = null;
        //             list.Add(s1);
        //             PublicVoids.SetAnonymousValue(ref o, "2", "OPLATA__Disp");
        //         }
        //         IsEnabled = false;
        //         //LogBox.Text = "Получение данных от сервера...";
        //         //object sanks = new object();
        //         TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext(); //get UI thread context 
        //         Task.Factory.StartNew(() =>
        //         {
        //             var ids = (from o in ((IEnumerable<dynamic>) _objs)
        //                 select new
        //                 {
        //                     IDS = PublicVoids.GetAnonymousValue(o, "ID"),
        //                     SCHET_IDS = PublicVoids.GetAnonymousValue(o, "SCHET_ID")
        //                 }).ToList();
        //             _tblName = "TempIds_" + Guid.NewGuid().ToString().Replace("-", "_");
        //             Reader2List.CustomExecuteQuery(
        //                 $"CREATE TABLE [dbo].[{_tblName}]([IDS] [int] NOT NULL, [SCHET_IDS] [int] NOT NULL)",
        //                 SprClass.LocalConnectionString);
        //             Reader2List.AnonymousInsertCommand(_tblName, ids, "",
        //                 SprClass.LocalConnectionString);
        //             Reader2List.CustomInsertCommand("SANK", list, "ID",
        //                 SprClass.LocalConnectionString);

        //         }).ContinueWith(a1 =>
        //         {
        //             Task.Factory.StartNew(() =>
        //             {
        //                 Reader2List.CustomExecuteQuery(
        //                 $@"Update SLUCH SET SANK_IT = t1.sum23 + t1.sum1, OPLATA = t1.oplata
        //                 --select t1.* 
        //                 from SLUCH sl
        //                 Join (select sl.ID,
        //                 (CASE	WHEN sum(ISNULL(sa1.S_SUM, 0.00)) >= max(ISNULL(sl.SUMV, 0.00)) THEN max(ISNULL(sl.SUMV, 0.00)) WHEN sum(ISNULL(sa1.S_SUM, 0.00)) < max(ISNULL(sl.SUMV, 0.00)) THEN sum(ISNULL(sa1.S_SUM, 0.00)) END) sum1,
        //                 SUM(ISNULL(sa23.S_SUM, 0.00)) sum23,
        //                 (CASE WHEN SUM(ISNULL(sa23.S_SUM, 0.00)) = 0 and count(sa1.ID) = 0 THEN 1 WHEN (SUM(ISNULL(sa23.S_SUM, 0.00)) + (CASE	WHEN sum(ISNULL(sa1.S_SUM, 0.00)) >= max(ISNULL(sl.SUMV, 0.00)) THEN max(ISNULL(sl.SUMV, 0.00)) WHEN sum(ISNULL(sa1.S_SUM, 0.00)) < max(ISNULL(sl.SUMV, 0.00)) THEN sum(ISNULL(sa1.S_SUM, 0.00)) END)) < max(ISNULL(sl.SUMV, 0.00)) THEN 3 ELSE 2 END) oplata
        //                 from sluch sl
        //                 left join SANK sa1 on sl.id = sa1.SLID and sa1.S_TIP = 1
        //                 left join SANK sa23 on sl.id = sa23.SLID and sa23.S_TIP in (2,3)
        //                 --WHERE sl.ID in (Select SLID FROM [dbo].[SANK_IMP_TT] WHERE SLID IS NOT NULL GROUP BY SLID)
        //                 GROUP BY sl.ID
        //                 ) t1 on sl.ID = t1.ID
        //                 where sl.ID in (select IDS from {_tblName})",
        //                 SprClass.LocalConnectionString);
        //             }).ContinueWith(a2 =>
        //             {
        //                 Task.Factory.StartNew(() =>
        //                 {
        //                     Reader2List.CustomExecuteQuery(
        //                         $@"UPDATE SLUCH SET SUMP = ISNULL(SUMV, 0.00) - ISNULL(SANK_IT, 0.00)
        //                         where ID in (select IDS from {_tblName})",
        //                         SprClass.LocalConnectionString);
        //                 }).ContinueWith(a3 =>
        //                 {
        //                     Task.Factory.StartNew(() =>
        //                     {
        //                         Reader2List.CustomExecuteQuery(
        //                     $@"UPDATE D3_SCHET_OMS SET SANK_MEK = t2.SANK_MEK, SANK_MEE = t2.SANK_MEE, SANK_EKMP = t2.SANK_EKMP
        //                     --select *
        //                     FROM D3_SCHET_OMS sc
        //                     JOIN
        //                     (
        //                     Select t1.SCHET_ID,
        //                     sum(ISNULL(t1.SANK_MEK, 0.00)) as SANK_MEK ,
        //                     sum(ISNULL(t1.SANK_MEE, 0.00)) as SANK_MEE ,
        //                     sum(ISNULL(t1.SANK_EKMP, 0.00)) as SANK_EKMP 
        //                     From
        //                     (SELECT sl.ID, min(sl.SCHET_ID) as SCHET_ID,
        //                     max(CASE WHEN sa.S_TIP = 1 THEN ISNULL(sa.S_SUM, 0.00) END) as SANK_MEK,
        //                     sum(CASE WHEN sa.S_TIP = 2 THEN ISNULL(sa.S_SUM, 0.00) ELSE 0.00 END) as SANK_MEE,
        //                     sum(CASE WHEN sa.S_TIP = 3 THEN ISNULL(sa.S_SUM, 0.00) ELSE 0.00 END) as SANK_EKMP
        //                     FROM SLUCH sl
        //                     left join SANK sa on sl.ID = sa.SLID
        //                     where sl.SCHET_ID in (select SCHET_IDS from {_tblName} GROUP BY SCHET_IDS)
        //group by sl.ID) as t1
        //                     Group by t1.SCHET_ID
        //                     ) as t2
        //                     on sc.ID = t2.SCHET_ID",
        //                     SprClass.LocalConnectionString);

        //                     }).ContinueWith(a4 =>
        //                     {
        //                         //LogBox.Text += " завершенно";
        //                         DXMessageBox.Show("Операция завершена");
        //                         //IsEnabled = true;
        //                         Task.Factory.StartNew(() =>
        //                         {
        //                             Reader2List.CustomExecuteQuery($"if exists (select * from INFORMATION_SCHEMA.TABLES where table_name ='{_tblName}') drop table {_tblName}", SprClass.LocalConnectionString);
        //                         });
        //                         ((DXWindow)this.Parent).Close();
        //                     }, taskScheduler);
        //                 }, taskScheduler);
        //             }, taskScheduler);
        //         }, taskScheduler);
        //     }
        //     else
        //     {
        //         SANK sank = new SANK();
        //         using (var dc = new ElmedDataClassesDataContext(SprClass.LocalConnectionString))
        //         {
        //             if (!_isNew) sank = dc.SANK.Single(x => x.ID == _sank.ID);
        //             sank.SLID = ((SLUCH)_elKard.sluchGrid.DataContext).ID;
        //             sank.SCHET_ID = ((SLUCH)_elKard.sluchGrid.DataContext).SCHET_ID;
        //             sank.S_TIP = 1;
        //             if (KodOtkazaBox.EditValue != null) sank.S_OSN = (string)KodOtkazaBox.EditValue;
        //             else sank.S_OSN = null;
        //             if (SankSumBox.EditValue != null) sank.S_SUM = (decimal)SankSumBox.EditValue;
        //             else sank.S_SUM = null;
        //             if (CommentBox.EditValue != null) sank.S_COM = (string)CommentBox.EditValue;
        //             else sank.S_COM = null;
        //             if (DateBox.EditValue != null) sank.S_DATE = (DateTime)DateBox.EditValue;
        //             else sank.S_DATE = null;
        //             if (_isNew)
        //             {
        //                 sank.S_CODE = Guid.NewGuid();
        //                 dc.SANK.InsertOnSubmit(sank);
        //             }
        //             dc.SubmitChanges();
        //         }
        //         if (_isNew)
        //         {
        //             //using (var dc = new ElmedDataClassesDataContext(SprClass.LocalConnectionString))
        //             //{
        //             //    dc.SLUCH.Single(x => x.ID == ((SLUCH)_elKard.sluchGrid.DataContext).ID).MEK_COMENT = CommentBox.EditValue != null ? (string)CommentBox.EditValue : null;
        //             //    dc.SubmitChanges();
        //             //}
        //         }
        //         _elKard.mekUpdate();

        //         ((DXWindow)this.Parent).Close();

        //     }


        // }
    }
}
