﻿#pragma checksum "..\..\ExpControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6161B5AEFA420453CD4A53F00A38944E5C44E1CE"
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
using DevExpress.Xpf.Bars.Themes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.DataSources;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Xpf.DXBinding;
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
using Yamed.Oms;


namespace Yamed.Oms {
    
    
    /// <summary>
    /// ExpControl
    /// </summary>
    public partial class ExpControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 59 "..\..\ExpControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Yamed.Oms.EconomyTabOMS EconomyTabOMS1;
        
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
            System.Uri resourceLocater = new System.Uri("/Yamed.Oms;component/expcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ExpControl.xaml"
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
            
            #line 25 "..\..\ExpControl.xaml"
            ((DevExpress.Xpf.Bars.BarButtonItem)(target)).ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.Amb_OnClick);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 29 "..\..\ExpControl.xaml"
            ((DevExpress.Xpf.Bars.BarButtonItem)(target)).ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.SchetRep_OnClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 33 "..\..\ExpControl.xaml"
            ((DevExpress.Xpf.Bars.BarButtonItem)(target)).ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.AutoMek_OnClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 37 "..\..\ExpControl.xaml"
            ((DevExpress.Xpf.Bars.BarButtonItem)(target)).ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.AutoMee_OnItemClick);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 42 "..\..\ExpControl.xaml"
            ((DevExpress.Xpf.Bars.BarButtonItem)(target)).ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.ButtonBase_OnClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.EconomyTabOMS1 = ((Yamed.Oms.EconomyTabOMS)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

