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
    /// <summary>
    /// Lógica de interacción para ClientesSinVendedor.xaml
    /// </summary>
    public partial class ClientesSinVendedor : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";

        public string codigo_ven = "";
        public string nombre_ven = "";

        public ClientesSinVendedor()
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;

            this.MinHeight = 600;
            this.MaxHeight = 600;
            this.MinWidth = 1200;
            this.MaxWidth = 1200;

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

                fecha_ini.Text = DateTime.Now.AddMonths(-1).ToString();
                fecha_fin.Text = DateTime.Now.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void BTNbuscar_Click(object sender, RoutedEventArgs e)
        {
            llenarGrid();
        }

        public void llenarGrid() {
            try
            {
                string fe_fin = fecha_fin.Text + " 23:59:59";

                string cadena = "select cliente.cod_ter,cliente.nom_ter as nom_ter,cliente.cod_ven as cod_ven, vendedor.nom_mer as nom_mer ,sum( iif(cabeza.cod_trn between '004' and '005',CAST(cuerpo.cantidad as int),CAST(-cuerpo.cantidad as int) ) ) as cantidad, sum((cantidad*val_uni)*iif(trn.tip_trn=1,-1,1)) as monto, max(iif(cabeza.cod_trn='005',fec_trn,'')) as ultfecha ";
                cadena = cadena + "from InCab_doc as cabeza  ";
                cadena = cadena + "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
                cadena = cadena + "inner join inmae_bod as bod on bod.cod_bod=cuerpo.cod_bod ";
                cadena = cadena + "inner join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
                cadena = cadena + "inner join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";
                cadena = cadena + "inner join comae_ter as cliente on cliente.cod_ter = cabeza.cod_cli ";
                cadena = cadena + "full join InMae_mer as vendedor on vendedor.cod_mer = cliente.cod_ven ";
                cadena = cadena + "inner join CrMae_cli as cliCamp on cliCamp.cod_ter = cliente.cod_ter ";
                cadena = cadena + "inner join InMae_trn trn on trn.cod_trn=cabeza.cod_trn and trn.ind_vtas=1 and trn.Tip_trn between 1 and 2 ";
                cadena = cadena + "where cabeza.cod_trn between '004' and '008' and cliente.clasific='1' ";
                cadena = cadena + "and cabeza.fec_trn  between '" + fecha_ini.Text + "' and '" + fe_fin + "'  ";
                cadena = cadena + "and cliente.cod_ven='' ";
                cadena = cadena + "group by cliente.nom_ter,cliente.cod_ter,cliente.cod_ven,vendedor.nom_mer ";
                cadena = cadena + "order by nom_ter ";

                DataTable dt = SiaWin.Func.SqlDT(cadena, "ClientesVendedores", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;
                totalCli.Text = dt.Rows.Count.ToString();
            }
            catch (Exception w)
            {
                MessageBox.Show("Eror al cargar: "+w);
            }
        }


        private void FirstDetailsViewGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            BTNasignar.IsEnabled = true;
        }


        private void Click_Asignar(object sender, RoutedEventArgs e)
        {
            try
            {

                string queryUPD_TER = "";
                string nombres_todos = "";

                var reflector = this.dataGridCxC.View.GetPropertyAccessProvider();
                foreach (var row in this.dataGridCxC.SelectedItems)
                {
                    foreach (var column in dataGridCxC.Columns)
                    {
                        var cellvalue = reflector.GetValue(row, column.MappingName);
                        var nombre = dataGridCxC.Columns["nom_ter"].MappingName;

                        var nombre_cli = reflector.GetValue(row, nombre.Trim());
                        nombres_todos = nombres_todos.Trim() + "- " + nombre_cli;

                        queryUPD_TER = queryUPD_TER + "update comae_ter set cod_ven='" + LBvendedor.Text + "' where cod_ter='" + cellvalue + "' ";
                        break;
                    }
                }

                SiaWin.Func.SqlDT(queryUPD_TER, "Clientes", idemp);                
                MessageBox.Show("Asignacion de Vendedor " + TBXvendedor.Text.Trim() + " a los Clientes " + nombres_todos.Trim() + " Exitosa");
                llenarGrid();
            }
            catch (Exception w)
            {
                MessageBox.Show("Error Seleciona un Cliente:" + w);
            }

        }

        private void BTNexportar_Click(object sender, RoutedEventArgs e) {

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
            LBvendedor.Text = codigo_ven;
            TBXvendedor.Text = nombre_ven.Trim();

        }






    }
}
