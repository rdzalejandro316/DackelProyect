#pragma checksum "..\..\UserControl1.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D202FFC5F518B860F04174848BD5585A57DE814E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using GeneracionPedidosProvedores;
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


namespace SiasoftAppExt
{


    /// <summary>
    /// UserControl1
    /// </summary>
    public partial class GeneracionPedidosProvedores : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GeneracionPedidosProvedores;component/usercontrol1.xaml", System.UriKind.Relative);

#line 1 "..\..\UserControl1.xaml"
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
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.Grid Panel;
        internal System.Windows.Controls.DatePicker FechaConsul;
        internal System.Windows.Controls.TextBox TextBox_Meses;
        internal System.Windows.Controls.TextBox TextBox_Minimo;
        internal System.Windows.Controls.TextBox TextBox_Maximo;
        internal System.Windows.Controls.TextBox TextCod_bod;
        internal System.Windows.Controls.TextBlock TextNombreBod;
        internal System.Windows.Controls.TextBox TextCod_Pro;
        internal System.Windows.Controls.TextBlock TextNombrePro;
        internal System.Windows.Controls.TextBox TextCod_Lin;
        internal System.Windows.Controls.TextBlock TextNombreLin;
        internal System.Windows.Controls.DataGrid dataGridGrup;
    }
}

