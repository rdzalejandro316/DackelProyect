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

    public partial class InformeMejoresClientes : UserControl
    {
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";

        public InformeMejoresClientes(dynamic tabitem1)
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;
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
                tabitem.Title = "Mejores Clientes (" + aliasemp + ")";

                TXBnumero.Text = "5";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void ValidacionNumeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("este campo solo admite valores numericos");
                e.Handled = true;
            }
        }

        private void CargarGrid(object sender, RoutedEventArgs e)
        {
            if (TXBnumero.Text.Length > 0)
            {
                try
                {
                    
                    string cadena = "select top "+ TXBnumero.Text + " cliente.cod_ter,cliente.nom_ter as nom_ter,cliente.cod_ven as cod_ven, vendedor.nom_mer as nom_mer ,sum( iif(cabeza.cod_trn between '004' and '005',CAST(cuerpo.cantidad as int),CAST(-cuerpo.cantidad as int) ) ) as cantidad, sum((cantidad*val_uni)*iif(trn.tip_trn=1,-1,1)) as monto, max(iif(cabeza.cod_trn='005',fec_trn,'')) as ultfecha ";
                    cadena = cadena + "from InCab_doc as cabeza ";
                    cadena = cadena + "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg  ";
                    cadena = cadena + "inner join comae_ter as cliente on cliente.cod_ter = cabeza.cod_cli ";
                    cadena = cadena + "full join InMae_mer as vendedor on vendedor.cod_mer = cliente.cod_ven ";
                    cadena = cadena + "inner join InMae_trn trn on trn.cod_trn=cabeza.cod_trn and trn.ind_vtas=1 and trn.Tip_trn between 1 and 2 ";
                    cadena = cadena + "where cabeza.cod_trn between '004' and '008' and cliente.clasific='1' ";
                    cadena = cadena + "group by cliente.nom_ter,cliente.cod_ter,cliente.cod_ven,vendedor.nom_mer ";
                    cadena = cadena + "order by monto desc ";

                    DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                    if (dt.Rows.Count <= 0) { MessageBox.Show("No Hay registros"); }
                    dataGridCxC.ItemsSource = dt.DefaultView;
                    TotalCli.Text = dt.Rows.Count.ToString();

                    BTNexpo.IsEnabled = true;
                }
                catch (Exception w)
                {

                    MessageBox.Show("error:" + w);
                }
            }
            else {
                MessageBox.Show("Ingresar el Numero de Clientes Que deseas Consultar");
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

         
                if (MessageBox.Show("Usted quiere abrir el archivo en excel?", "Ver archvo",
                                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {         
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }


        }





    }
}