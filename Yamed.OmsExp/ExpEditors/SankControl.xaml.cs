﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using Yamed.Control;
using Yamed.Core;
using Yamed.Entity;
using Yamed.Server;

namespace Yamed.OmsExp.ExpEditors
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
                    List<int> zslid = new List<int>();
                    foreach (var row in DxHelper.LoadedRows)
                    {
                        if (zslid.Contains((int)ObjHelper.GetAnonymousValue(row, "ID")) == false)
                        {
                            var sank = ObjHelper.ClassConverter<D3_SANK_OMS>(_sank);
                            sank.S_CODE = Guid.NewGuid().ToString();
                            //sank.S_DATE = SprClass.WorkDate;
                            sank.S_SUM = (decimal)ObjHelper.GetAnonymousValue(row, "SUMV");
                            sank.D3_ZSLID = (int)ObjHelper.GetAnonymousValue(row, "ID");
                            sank.D3_SCID = (int)ObjHelper.GetAnonymousValue(row, "D3_SCID");
                            sank.S_TIP = 1;
                            sank.S_TIP2 = 1;
                            sank.ID = Reader2List.ObjectInsertCommand("D3_SANK_OMS", sank, "ID",
        SprClass.LocalConnectionString);
                            Reader2List.CustomExecuteQuery($@"
EXEC p_oms_calc_sank {sank.D3_SCID}
EXEC p_oms_calc_schet {sank.D3_SCID}
", SprClass.LocalConnectionString);
                            zslid.Add(sank.D3_ZSLID);
                        }
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
    }
}
