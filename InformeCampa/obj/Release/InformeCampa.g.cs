﻿#pragma checksum "..\..\InformeCampa.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4396C2615419BA6DFDB58C3C0AACF7A59D65DF33"
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
using Syncfusion.UI.Xaml.Grid.Converter;
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
    /// InformeCampa
    /// </summary>
    public partial class InformeCampa : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\InformeCampa.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.Windows.Tools.Controls.TabControlExt TabControl1;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\InformeCampa.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.Windows.Tools.Controls.TabItemExt tabItemExt1;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\InformeCampa.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TBX_name_cam;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\InformeCampa.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LB_cod_cam;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\InformeCampa.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BTNejec;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\InformeCampa.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BTNexpor;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\InformeCampa.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.UI.Xaml.Grid.SfDataGrid dataGridCxC;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\InformeCampa.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TotalReg;
        
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
            System.Uri resourceLocater = new System.Uri("/InformeCampa;component/informecampa.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\InformeCampa.xaml"
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
            this.TBX_name_cam = ((System.Windows.Controls.TextBox)(target));
            
            #line 35 "..\..\InformeCampa.xaml"
            this.TBX_name_cam.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.LB_cod_cam = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.BTNejec = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\InformeCampa.xaml"
            this.BTNejec.Click += new System.Windows.RoutedEventHandler(this.CargarGrid);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BTNexpor = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\InformeCampa.xaml"
            this.BTNexpor.Click += new System.Windows.RoutedEventHandler(this.ExportaXLS_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.dataGridCxC = ((Syncfusion.UI.Xaml.Grid.SfDataGrid)(target));
            return;
            case 8:
            this.TotalReg = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

