﻿#pragma checksum "..\..\ConfiguracionCRM.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E0A143CE7ACAD2E673A864D2496BEC0E55D4DF1B"
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
using Syncfusion;
using Syncfusion.UI.Xaml.Controls.DataPager;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.RowFilter;
using Syncfusion.UI.Xaml.TreeGrid;
using Syncfusion.Windows;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools;
using Syncfusion.Windows.Tools.Controls;
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
    /// ConfiguracionCRM
    /// </summary>
    public partial class ConfiguracionCRM : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\ConfiguracionCRM.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.Windows.Tools.Controls.TabControlExt TabControl1;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\ConfiguracionCRM.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.Windows.Tools.Controls.TabItemExt tabItemExt1;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\ConfiguracionCRM.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.UI.Xaml.Grid.SfDataGrid dataGridConfig;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\ConfiguracionCRM.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BTNeditar;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\ConfiguracionCRM.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TXB_User_Correo;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\ConfiguracionCRM.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox TXB_Con_Correo;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\ConfiguracionCRM.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox TXB_Con_Correo_repetir;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\ConfiguracionCRM.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BTNmostrarPass;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\ConfiguracionCRM.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BTNactualizar;
        
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
            System.Uri resourceLocater = new System.Uri("/ConfiguracionCRM;component/configuracioncrm.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ConfiguracionCRM.xaml"
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
            this.TabControl1 = ((Syncfusion.Windows.Tools.Controls.TabControlExt)(target));
            return;
            case 2:
            this.tabItemExt1 = ((Syncfusion.Windows.Tools.Controls.TabItemExt)(target));
            return;
            case 3:
            this.dataGridConfig = ((Syncfusion.UI.Xaml.Grid.SfDataGrid)(target));
            
            #line 45 "..\..\ConfiguracionCRM.xaml"
            this.dataGridConfig.SelectionChanged += new Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventHandler(this.dataGridConfig_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.BTNeditar = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\ConfiguracionCRM.xaml"
            this.BTNeditar.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.TXB_User_Correo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.TXB_Con_Correo = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 7:
            this.TXB_Con_Correo_repetir = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 8:
            this.BTNmostrarPass = ((System.Windows.Controls.Button)(target));
            
            #line 81 "..\..\ConfiguracionCRM.xaml"
            this.BTNmostrarPass.Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 9:
            this.BTNactualizar = ((System.Windows.Controls.Button)(target));
            
            #line 82 "..\..\ConfiguracionCRM.xaml"
            this.BTNactualizar.Click += new System.Windows.RoutedEventHandler(this.BTNactualizar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

