﻿#pragma checksum "..\..\..\SqlEditor\AlgEditControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A95A99516850F227A24BA105852B056541CD62C6"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.View.WPF.DatabaseSchemaView;
using ActiveQueryBuilder.View.WPF.ExpressionEditor;
using ActiveQueryBuilder.View.WPF.NavigationBar;
using ActiveQueryBuilder.View.WPF.QueryView;
using DevExpress.Core;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.DataSources;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Xpf.DXBinding;
using DevExpress.Xpf.Docking;
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
using DevExpress.Xpf.LayoutControl;
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
using Yamed.OmsExp.SqlEditor;


namespace Yamed.OmsExp.SqlEditor {
    
    
    /// <summary>
    /// AlgEditControl
    /// </summary>
    public partial class AlgEditControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\SqlEditor\AlgEditControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MekGrid;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\SqlEditor\AlgEditControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.LayoutControl.LayoutItem ListLayoutItem;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\SqlEditor\AlgEditControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridControl AlgList;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\SqlEditor\AlgEditControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ComboBoxEdit OsnBoxEdit;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\..\SqlEditor\AlgEditControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ActiveQueryBuilder.View.WPF.ExpressionEditor.SqlTextEditor sqlEditor;
        
        #line default
        #line hidden
        
        
        #line 158 "..\..\..\SqlEditor\AlgEditControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Yamed.OmsExp.SqlEditor.ErrorBox ErrorControl;
        
        #line default
        #line hidden
        
        
        #line 289 "..\..\..\SqlEditor\AlgEditControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridControl GridControl1;
        
        #line default
        #line hidden
        
        
        #line 291 "..\..\..\SqlEditor\AlgEditControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.TableView TableView1;
        
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
            System.Uri resourceLocater = new System.Uri("/Yamed.OmsExp;component/sqleditor/algeditcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SqlEditor\AlgEditControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 18 "..\..\..\SqlEditor\AlgEditControl.xaml"
            ((Yamed.OmsExp.SqlEditor.AlgEditControl)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.AlgEditControl_OnUnloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MekGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.ListLayoutItem = ((DevExpress.Xpf.LayoutControl.LayoutItem)(target));
            return;
            case 4:
            this.AlgList = ((DevExpress.Xpf.Grid.GridControl)(target));
            
            #line 34 "..\..\..\SqlEditor\AlgEditControl.xaml"
            this.AlgList.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.AlgList_SelectedItemChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.OsnBoxEdit = ((DevExpress.Xpf.Editors.ComboBoxEdit)(target));
            return;
            case 6:
            this.sqlEditor = ((ActiveQueryBuilder.View.WPF.ExpressionEditor.SqlTextEditor)(target));
            
            #line 141 "..\..\..\SqlEditor\AlgEditControl.xaml"
            this.sqlEditor.TextChanged += new System.EventHandler(this.SqlEditor_OnTextChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 144 "..\..\..\SqlEditor\AlgEditControl.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandUndo_OnExecuted);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 145 "..\..\..\SqlEditor\AlgEditControl.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandRedo_OnExecuted);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 146 "..\..\..\SqlEditor\AlgEditControl.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandCopy_OnExecuted);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 147 "..\..\..\SqlEditor\AlgEditControl.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandPaste_OnExecuted);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 148 "..\..\..\SqlEditor\AlgEditControl.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandCut_OnExecuted);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 149 "..\..\..\SqlEditor\AlgEditControl.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandSelectAll_OnExecuted);
            
            #line default
            #line hidden
            return;
            case 13:
            this.ErrorControl = ((Yamed.OmsExp.SqlEditor.ErrorBox)(target));
            return;
            case 14:
            this.GridControl1 = ((DevExpress.Xpf.Grid.GridControl)(target));
            return;
            case 15:
            this.TableView1 = ((DevExpress.Xpf.Grid.TableView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

