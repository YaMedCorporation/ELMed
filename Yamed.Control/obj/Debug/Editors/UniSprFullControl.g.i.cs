﻿#pragma checksum "..\..\..\Editors\UniSprFullControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "777143B83A3EEABE8267DDE4A5C794ED355E5263"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Xpf.Bars;
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
using Yamed.Control.Editors;


namespace Yamed.Control.Editors {
    
    
    /// <summary>
    /// UniSprFullControl
    /// </summary>
    public partial class UniSprFullControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\Editors\UniSprFullControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Bars.BarButtonItem UpdateItem;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\Editors\UniSprFullControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Bars.BarButtonItem NewItem;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Editors\UniSprFullControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Bars.BarButtonItem EditItem;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Editors\UniSprFullControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Bars.BarButtonItem DeleteItem;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Editors\UniSprFullControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Yamed.Control.Editors.UniSprControl UniSprControl1;
        
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
            System.Uri resourceLocater = new System.Uri("/Yamed.Control;component/editors/unisprfullcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Editors\UniSprFullControl.xaml"
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
            this.UpdateItem = ((DevExpress.Xpf.Bars.BarButtonItem)(target));
            
            #line 17 "..\..\..\Editors\UniSprFullControl.xaml"
            this.UpdateItem.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.UpdateItem_OnItemClick);
            
            #line default
            #line hidden
            return;
            case 2:
            this.NewItem = ((DevExpress.Xpf.Bars.BarButtonItem)(target));
            
            #line 19 "..\..\..\Editors\UniSprFullControl.xaml"
            this.NewItem.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.NewItem_OnItemClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.EditItem = ((DevExpress.Xpf.Bars.BarButtonItem)(target));
            
            #line 20 "..\..\..\Editors\UniSprFullControl.xaml"
            this.EditItem.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.EditItem_OnItemClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DeleteItem = ((DevExpress.Xpf.Bars.BarButtonItem)(target));
            
            #line 21 "..\..\..\Editors\UniSprFullControl.xaml"
            this.DeleteItem.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.DeleteItem_OnItemClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.UniSprControl1 = ((Yamed.Control.Editors.UniSprControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

