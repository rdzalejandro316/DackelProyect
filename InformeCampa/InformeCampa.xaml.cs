using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SiasoftAppExt
{

    public partial class InformeCampa : UserControl
    {
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";
        public string Conexion;
        string codigoVendedor;
        string tipoUsuario;
        


        public InformeCampa(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;
            codigoVendedor = SiaWin._UserTag1;
            tipoUsuario = SiaWin._UserTag2;

            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                System.Data.DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
                tabitem.Logo(idLogo, ".png");
                tabitem.Title = "Informe Campaña (" + aliasemp + ")";

                //TextBx_fecha_ini.Text = DateTime.Today.AddYears(-1).ToString();
                //TextBx_fecha_fin.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //validacion para que se ingrese fijo el campo de una maestra
                string idTab = ((TextBox)sender).Tag.ToString();
                if (idTab.Length > 0)
                {
                    string tag = ((TextBox)sender).Tag.ToString();
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                    if (string.IsNullOrEmpty(tag)) return;

                    if (tag == "CrMae_campa")
                    {
                        cmptabla = tag; cmpcodigo = "cod_camp"; cmpnombre = "UPPER(nom_camp)"; cmporden = "cod_camp"; cmpidrow = "cod_camp"; cmptitulo = "Maestra de Campaña"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "estado='1'";
                    }
                    

                    int idr = 0; string code = ""; string nom = "";
                    dynamic winb = SiaWin.WindowBuscar(cmptabla, cmpcodigo, cmpnombre, cmporden, cmpidrow, cmptitulo, cnEmp, mostrartodo, cmpwhere);

                    winb.ShowInTaskbar = false;
                    winb.Owner = Application.Current.MainWindow;
                    winb.ShowDialog();
                    idr = winb.IdRowReturn;
                    code = winb.Codigo;
                    nom = winb.Nombre;
                    winb = null;
                    if (idr > 0)
                    {
                        if (tag == "CrMae_campa")
                        {
                            LB_cod_cam.Text = code; TBX_name_cam.Text = nom;
                            BTNejec.IsEnabled = true;                            
                        }
                        

                        var uiElement = e.OriginalSource as UIElement;
                        uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                    }
                    e.Handled = true;
                }
                if (e.Key == Key.Enter)
                {
                    var uiElement = e.OriginalSource as UIElement;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void CargarGrid(object sender, RoutedEventArgs e) {

            if (LB_cod_cam.Text.Length > 0)
            {
                try
                {

                    string cadena = "select seguimineto.fec_seg as fec_seg,seguimineto.cod_ter as cod_ter,cliente.nom_ter as nom_ter,seguimineto.cod_mer as cod_mer,vendedor.nom_mer as nom_mer,seguimineto.cod_con as cod_con, concepto.nom_con as nom_con,seguimineto.contacto_cli as contacto_cli, seguimineto.observ as observ ";
                    cadena = cadena + "from Crseg_cli as seguimineto ";
                    cadena = cadena + "inner join Comae_ter as cliente on seguimineto.cod_ter = cliente.cod_ter ";
                    cadena = cadena + "inner join InMae_mer as vendedor on seguimineto.cod_mer = vendedor.cod_mer ";
                    cadena = cadena + "inner join CrMae_concepto as concepto on seguimineto.cod_con = concepto.cod_con ";
                    cadena = cadena + "where seguimineto.cod_camp='" + LB_cod_cam.Text + "' ";


                    DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                    dataGridCxC.ItemsSource = dt.DefaultView;

                    if (dt.Rows.Count <= 0) { MessageBox.Show("No existe clinetes registrados en esta campaña"); }
                    TotalReg.Text = dt.Rows.Count.ToString();

                    BTNexpor.IsEnabled = true;
                }
                catch (Exception w)
                {
                    MessageBox.Show("error:" + w);
                }
            }
            else {
                MessageBox.Show("llene los campos correspondientes para ejecutar el informe");
            }
            
        }

        
        private void ExportaXLS_Click(object sender, RoutedEventArgs e)
        {
      
                var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
                options.ExcelVersion = ExcelVersion.Excel2013;
                var excelEngine = dataGridCxC.ExportToExcel(dataGridCxC.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];

                SaveFileDialog sfd = new SaveFileDialog
                {
                    FilterIndex = 2,
                    Filter = "Excel 97 to 2003 Files(*.xls)|*.xls|Excel 2007 to 2010 Files(*.xlsx)|*.xlsx|Excel 2013 File(*.xlsx)|*.xlsx"
                };

                if (sfd.ShowDialog() == true)
                {
                    using (Stream stream = sfd.OpenFile())
                    {
                        if (sfd.FilterIndex == 1)
                            workBook.Version = ExcelVersion.Excel97to2003;
                        else if (sfd.FilterIndex == 2)
                            workBook.Version = ExcelVersion.Excel2010;
                        else
                            workBook.Version = ExcelVersion.Excel2013;
                        workBook.SaveAs(stream);
                    }

                    //Message box confirmation to view the created workbook.
                    if (MessageBox.Show("Usted quiere abrir el archivo en excel?", "Ver archvo",
                                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                }
                 
            
        }






    }
}
