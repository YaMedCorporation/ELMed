﻿#pragma checksum "..\..\..\Editors\UniSprControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "38AFF893CC58504D3097C4C14CE9A161E1B664AA"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Core;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.DataSources;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Xpf.DXBinding;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.DataPager;
using DevExpress.Xpf.Editors.DateNavigator;
using DevExpress.Xpf.Editors.ExpressionEditor;
using DevExpress.Xpf.Editors.Filtering;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Editors.Popups;
using DevExpress.Xpf.Editors.Popups.Calendar;
using DevExpress.Xpf.Editors.RangeControl;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Editors.Settings.Extension;
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.ConditionalFormatting;
using DevExpress.Xpf.Grid.LookUp;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.WindowsUI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Yamed.Control.Editors {
    
    
    /// <summary>
    /// UniSprControl
    /// </summary>
    public partial class UniSprControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        /// <summary>
        /// GridControlSpr Name Field
        /// </summary>
        
        #line 13 "..\..\..\Editors\UniSprControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public DevExpress.Xpf.Grid.GridControl GridControlSpr;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Yamed.Control;component/editors/unisprcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Editors\UniSprControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.GridControlSpr = ((DevExpress.Xpf.Grid.GridControl)(target));
            
            #line 13 "..\..\..\Editors\UniSprControl.xaml"
            this.GridControlSpr.AutoGeneratingColumn += new DevExpress.Xpf.Grid.AutoGeneratingColumnEventHandler(this.GridControlSpr_OnAutoGeneratingColumn);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 15 "..\..\..\Editors\UniSprControl.xaml"
            ((DevExpress.Xpf.Grid.TableView)(target)).RowDoubleClick += new DevExpress.Xpf.Grid.RowDoubleClickEventHandler(this.TableView_OnRowDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

