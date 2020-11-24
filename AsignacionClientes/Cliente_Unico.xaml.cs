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
using System.Windows.Shapes;

namespace AsignacionClientes
{
    
    public partial class Cliente_Unico : Window
    {

        dynamic SiaWin;        
        int idemp = 0;
        string cnEmp = "";

        public string codigo_ven = "";
        public string nombre_ven = "";




        public Cliente_Unico()
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;        
            idemp = SiaWin._BusinessId;
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

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void TBCliente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string tag = ((TextBox)sender).Tag.ToString();
                string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = false; string cmpwhere = "";
                if (string.IsNullOrEmpty(tag)) return;

                if (tag == "comae_ter")
                {
                    cmptabla = tag; cmpcodigo = "cod_ter"; cmpnombre = "UPPER(nom_ter)"; cmporden = "cod_ter"; cmpidrow = "idrow"; cmptitulo = "Maestra de Terceros"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "clasific=1";
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
                    if (tag == "comae_ter")
                    {
                        LB_cliente.Text = code; TBX_cliente.Text = nom.Trim();
                        Consultar.IsEnabled = true;
                        Exportar.IsEnabled = true;
                    }


                    var uiElement = e.OriginalSource as UIElement;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
                e.Handled = true;

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

        private void Consultar_Click(object sender, RoutedEventArgs e)
        {
            cargarGrid();
        }

        public void cargarGrid()
        {
            try
            {
                string cadena = "SELECT	rtrim(TER.cod_ter) as cod_ter, rtrim(TER.tdoc) as tdoc, rtrim(TER.nom_ter) as nom_ter,rtrim(UPPER(TER.nom1)) as nom1,rtrim(UPPER(TER.nom2)) as nom2,rtrim(UPPER(TER.apell1)) as apell1,rtrim(UPPER(TER.apell2)) as apell2, rtrim(TER.cod_ven) as cod_ven,rtrim(VENDEDOR.nom_mer) as nom_mer ";
                cadena = cadena + "FROM CrMae_cli as CLIE,COMAE_TER as TER ";
                cadena = cadena + "left join InMae_mer as VENDEDOR on  VENDEDOR.cod_mer = TER.cod_ven ";
                cadena = cadena + "where TER.clasific = 1 and CLIE.cod_ter=TER.cod_ter and TER.cod_ter='" + LB_cliente.Text + "' ";

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;
            }
            catch (Exception w)
            {
                MessageBox.Show("Error al Cargar Cliente: " + w);
            }
        }

        private void Exportar_Click(object sender, RoutedEventArgs e)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tab1.Header = nombre_ven.Trim();
            LB_vendedor.Text = codigo_ven;
        }

        private void AsigCli_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cadena = "update Comae_ter set cod_ven='" + LB_vendedor.Text + "' where cod_ter='" +LB_cliente.Text+ "' ";               
                SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                MessageBox.Show("asignacion del cliente " + TBX_cliente .Text.Trim() + " al vendedor(a) " + nombre_ven.Trim());
                cargarGrid();
            }
            catch (Exception)
            {
                MessageBox.Show("error al agrearle el vendedor al cliete");
            }
        }

        private void dataGridCxC_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            AsigCli.IsEnabled = true;
        }


        




    }
}
