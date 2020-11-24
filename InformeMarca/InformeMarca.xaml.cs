using Microsoft.Win32;
using Syncfusion.XlsIO;
using Syncfusion.UI.Xaml.Grid.Converter;
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
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class InformeMarca : UserControl
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




        public InformeMarca(dynamic tabitem1)
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
                tabitem.Title = "Informe Marca (" + aliasemp + ")";

                TextBx_fecha_ini.Text = DateTime.Today.AddMonths(-1).ToString();
                TextBx_fecha_fin.Text = DateTime.Now.ToShortDateString();


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        
        private void CargarGrid(object sender, RoutedEventArgs e)
        {

            try
            {
                string fe_fin = TextBx_fecha_fin.Text + " 23:59:59";

                string cadena = "select cliente.cod_ter,cliente.nom_ter as nom_ter,cliente.tel1,cliente.email,linea.nom_tip as nom_tip, sum( iif(cabeza.cod_trn between '004' and '005',CAST(cuerpo.cantidad as int),CAST(-cuerpo.cantidad as int) ) ) as cantidad_linea, sum( iif(cabeza.cod_trn between '004' and '005',cuerpo.subtotal,-cuerpo.subtotal) ) as total_Linea, vendedor.nom_mer as nom_mer,max(iif(cabeza.cod_trn='005',fec_trn,'')) as ultfecha, max(iif(cabeza.cod_trn='005',cuerpo.cod_bod,'')) as bodega,max(bod.nom_bod) as nom_bod  ";
                cadena = cadena + "from InCab_doc as cabeza ";
                cadena = cadena + "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
                cadena = cadena + "inner join inmae_bod as bod on bod.cod_bod=cuerpo.cod_bod ";
                cadena = cadena + "inner join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
                cadena = cadena + "inner join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";
                cadena = cadena + "inner join comae_ter as cliente on cliente.cod_ter = cabeza.cod_cli ";
                cadena = cadena + "full join InMae_mer as vendedor on vendedor.cod_mer = cliente.cod_ven ";
                cadena = cadena + "inner join CrMae_cli as cliCamp on cliCamp.cod_ter = cliente.cod_ter ";
                cadena = cadena + "where cabeza.cod_trn between '004' and '008' and cliente.clasific='1' ";
                cadena = cadena + "and cabeza.fec_trn between '" + TextBx_fecha_ini.Text + "' and '" + fe_fin + "' ";
                cadena = cadena + "group by cliente.nom_ter,cliente.tel1,cliente.email,linea.nom_tip,vendedor.nom_mer,cliente.cod_ter ";
                cadena = cadena + "order by linea.nom_tip";

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;

                if (dt.Rows.Count <= 0) { MessageBox.Show("No existe registros en el rango de fecha estipulado"); }
                TotalReg.Text = dt.Rows.Count.ToString();

                BTNexpo.IsEnabled = true;
            }
            catch (Exception w)
            {

                MessageBox.Show("error:"+w);
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
