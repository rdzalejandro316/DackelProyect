using AsignacionClientes;
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

    public partial class AsignacionClientes : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";
        DataTable dt = new DataTable();

        public AsignacionClientes(dynamic tabitem1)
        {

            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;


            LoadConfig();

            cargarVendedores();

            BTNCliAsi.IsEnabled = false;
            BTNCliReasig.IsEnabled = false;
            BTNexportar.IsEnabled = false;
            BTNCliSinVen.IsEnabled = false;
            BTNCliNuevos.IsEnabled = false;
            BTNCliUni.IsEnabled = false;
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
                tabitem.Title = "Asignacion de Clientes(" + aliasemp + ")";

                //TxtUser.Text = SiaWin._UserAlias;                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void cargarVendedores() {

            string queryGrid = "select inmae_mer.cod_mer as cod_mer,inmae_mer.nom_mer as nom_mer from inmae_mer where inmae_mer.estado='1'  ";

            dt = SiaWin.Func.SqlDT(queryGrid, "Vendedores", idemp);
            dataGridCxC.ItemsSource = dt.DefaultView;
            TotalGrid.Text = dt.Rows.Count.ToString();
        }

        private void FirstDetailsViewGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e) {
            cargarClientes();
            BTNCliReasig.IsEnabled = true;
            BTNexportar.IsEnabled = true;
            BTNCliSinVen.IsEnabled = true;
            BTNCliNuevos.IsEnabled = true;
            BTNCliUni.IsEnabled = true;
        }

        public void cargarClientes() {
            try
            {
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                string cod_ven = row[0].ToString();

                string queryGrid = "select cod_ter,nom_ter from comae_ter where cod_ven='" + cod_ven + "' ";

                dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
                dataGridClientes.ItemsSource = dt.DefaultView;

                VendedorGrid.Text = row[1].ToString();
                ClientesTotal.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {

            }
        }


        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
            string codVen = row[0].ToString();
            string nomVen = row[1].ToString();

            Clientes cliente = new Clientes();
            cliente.Nven = nomVen;
            cliente.Cven = codVen;


            cliente.ShowInTaskbar = false;
            cliente.Owner = Application.Current.MainWindow;
            cliente.ShowDialog();            

            cargarClientes();
        }

        private void desbloqueBTN(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {

            BTNCliAsi.IsEnabled = true;
        }

        private void BTNCliAsi_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row1 = (DataRowView)dataGridCxC.SelectedItems[0];
            string nom_mer = row1[1].ToString();

            try
            {
                string cadena = "";
                Boolean not = false;
                var reflector = this.dataGridClientes.View.GetPropertyAccessProvider();
                foreach (var row in this.dataGridClientes.SelectedItems)
                {
                    foreach (var column in dataGridClientes.Columns)
                    {
                        var cellvalue = reflector.GetValue(row, column.MappingName);

                        cadena = cadena + " update comae_ter set cod_ven = '' where cod_ter = '" + cellvalue + "' ";
                        not = true;
                        break;
                    }
                }

                if (not == true)
                {
                    SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                }
                MessageBox.Show("Clientes eliminados del vendedor : " + nom_mer);
                cargarClientes();
            }
            catch (Exception w)
            {

                MessageBox.Show("error" + w);
            }

        }

        private void BTNCliReasig_Click(object sender, RoutedEventArgs e) {
            DataRowView row1 = (DataRowView)dataGridCxC.SelectedItems[0];
            string cod_mer = row1["cod_mer"].ToString();
            string nom_mer = row1["nom_mer"].ToString();

            
            Reasignar windows_reasignar = new Reasignar();
            windows_reasignar.CodVendedor = cod_mer;
            windows_reasignar.NomVendedor = nom_mer;

            windows_reasignar.ShowInTaskbar = false;
            windows_reasignar.Owner = Application.Current.MainWindow;
            windows_reasignar.ShowDialog();

            cargarClientes();

        }

        private void BTNexportar_Click(object sender, RoutedEventArgs e)
        {
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = dataGridClientes.ExportToExcel(dataGridClientes.View, options);
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

        private void BTNCliSinVen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row1 = (DataRowView)dataGridCxC.SelectedItems[0];
                string cod_mer = row1["cod_mer"].ToString();
                string nom_mer = row1["nom_mer"].ToString();

                ClientesSinVendedor sin_ven = new ClientesSinVendedor();
                sin_ven.codigo_ven = cod_mer;
                sin_ven.nombre_ven = nom_mer;

                sin_ven.ShowInTaskbar = false;
                sin_ven.Owner = Application.Current.MainWindow;
                sin_ven.ShowDialog();
                

                cargarClientes();
            }
            catch (Exception)
            {
                MessageBox.Show("seleccione un vendedor");
            }
            

        }


        private void BTNCliNue_Click(object sender, RoutedEventArgs e) {
            try
            {
                DataRowView row1 = (DataRowView)dataGridCxC.SelectedItems[0];
                string cod_mer = row1["cod_mer"].ToString();
                string nom_mer = row1["nom_mer"].ToString();

                ClientesNuevos nuevos_cli = new ClientesNuevos();
                nuevos_cli.cod_vendedor = cod_mer;
                nuevos_cli.nom_vendedor = nom_mer;

                nuevos_cli.ShowInTaskbar = false;
                nuevos_cli.Owner = Application.Current.MainWindow;
                nuevos_cli.ShowDialog();               

                cargarClientes();

            }
            catch (Exception){MessageBox.Show("error al cargar clientes nuevos");}


        }

        private void BTNCliUni_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente_Unico windows = new Cliente_Unico();
                DataRowView row1 = (DataRowView)dataGridCxC.SelectedItems[0];                

                windows.codigo_ven = row1["cod_mer"].ToString();
                windows.nombre_ven = row1["nom_mer"].ToString();

                windows.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                windows.ShowInTaskbar = false;
                windows.Owner = Application.Current.MainWindow;

                windows.ShowInTaskbar = false;
                windows.Owner = Application.Current.MainWindow;
                windows.ShowDialog();                
                cargarClientes();

            }
            catch (Exception w)
            {
                MessageBox.Show("selecione un vendedor"+ w);
            }
            



        }




    }
}
