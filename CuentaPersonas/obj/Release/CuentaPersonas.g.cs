﻿#pragma checksum "..\..\CuentaPersonas.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B5C49CD081F38C77FBBAACCC5EC61448A9CBCE26"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using SiasoftAppExt;
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


namespace SiasoftAppExt {
    
    
    /// <summary>
    /// CuentaPersonas
    /// </summary>
    public partial class CuentaPersonas : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\CuentaPersonas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TXB_bodega;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\CuentaPersonas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TXB_Cantidad;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\CuentaPersonas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TXB_observ;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\CuentaPersonas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cargarBTN;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\CuentaPersonas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button guardarBTN;
        
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
            System.Uri resourceLocater = new System.Uri("/CuentaPersonas;component/cuentapersonas.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CuentaPersonas.xaml"
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
            this.TXB_bodega = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.TXB_Cantidad = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.TXB_observ = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.cargarBTN = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\CuentaPersonas.xaml"
            this.cargarBTN.Click += new System.Windows.RoutedEventHandler(this.cargarBTN_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.guardarBTN = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\CuentaPersonas.xaml"
            this.guardarBTN.Click += new System.Windows.RoutedEventHandler(this.guardarBTN_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
