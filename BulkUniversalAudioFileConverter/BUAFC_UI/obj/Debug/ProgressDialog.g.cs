﻿#pragma checksum "..\..\ProgressDialog.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6B7E0B8139F3584C8E740511AFAF379365DAD9C5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BUAFC_UI;
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


namespace BUAFC_UI {
    
    
    /// <summary>
    /// ProgressDialog
    /// </summary>
    public partial class ProgressDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\ProgressDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar PRGBR_PROGRESS;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\ProgressDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TXTB_Action;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\ProgressDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BUT_CANCEL;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\ProgressDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TXTB_Directory;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\ProgressDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BUT_OKAY;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\ProgressDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TXTB_Prog;
        
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
            System.Uri resourceLocater = new System.Uri("/BUAFC_UIPrototype;component/progressdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ProgressDialog.xaml"
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
            this.PRGBR_PROGRESS = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 2:
            this.TXTB_Action = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.BUT_CANCEL = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\ProgressDialog.xaml"
            this.BUT_CANCEL.Click += new System.Windows.RoutedEventHandler(this.BUT_CANCEL_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TXTB_Directory = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.BUT_OKAY = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\ProgressDialog.xaml"
            this.BUT_OKAY.Click += new System.Windows.RoutedEventHandler(this.BUT_OKAY_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.TXTB_Prog = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

