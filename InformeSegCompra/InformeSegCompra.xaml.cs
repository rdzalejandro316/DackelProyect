using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid;
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
    
    public partial class InformeSegCompra : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";

        public InformeSegCompra(dynamic tabitem1)
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
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
                tabitem.Logo(idLogo, ".png");
                tabitem.Title = "Seguimiento de Compra (" + aliasemp + ")";

                fecha_ini.Text = DateTime.Now.AddMonths(-1).ToString();
                fecha_fin.Text = DateTime.Now.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void CargarGrid(object sender, RoutedEventArgs e) {

            string fe_fin = fecha_fin.Text + " 23:59:59";

            string cadena = "select seguimineto.fec_seg as fec_seg,seguimineto.cod_ter as cod_ter,tercero.nom_ter as nom_ter,seguimineto.cod_mer as cod_mer,vendedor.nom_mer as nom_mer,seguimineto.compra as compra,IIF(seguimineto.compra='SI',seguimineto.cod_detalle,'-'),IIF(seguimineto.compra='SI',detalle.nom_detalle,'-') as nom_detalle,IIF(seguimineto.compra='SI',seguimineto.tipo_compra,'-') as tipo_compra,IIF(seguimineto.compra='NO',seguimineto.no_compra,'-') as no_compra,seguimineto.observ as observ ";
            cadena = cadena + "from Crseg_Compra as seguimineto ";
            cadena = cadena + "full join CrMae_detalle as detalle on detalle.cod_detalle = seguimineto.cod_detalle ";
            cadena = cadena + "full join Comae_ter as tercero on tercero.cod_ter = seguimineto.cod_ter ";
            cadena = cadena + "full join inmae_mer as vendedor on vendedor.cod_mer = seguimineto.cod_mer ";            
            cadena = cadena + "where fec_seg between '" + fecha_ini.Text + "' and '" + fe_fin + "' ";

            DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
            if (dt.Rows.Count <= 0) { MessageBox.Show("No Hay registros"); }
            dataGridCxC.ItemsSource = dt.DefaultView;
            TotalResg.Text = dt.Rows.Count.ToString();

            BTNexpo.IsEnabled = true;
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


        // ROWS automatico ******************************************************************************
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();

        //To get the calculated height from GetAutoRowHeight method.    
        double autoHeight = double.NaN;

        // The list contains the column names that will excluded from the height calculation in GetAutoRowHeight method.
        List<string> excludeColumns = new List<string>() { "observ" };

        private void dataGridCxC_QueryRowHeight(object sender, Syncfusion.UI.Xaml.Grid.QueryRowHeightEventArgs e)
        {
            if (this.dataGridCxC.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
            {
                if (autoHeight > 24)
                {
                    e.Height = autoHeight;
                    e.Handled = true;
                }
            }
        }



    }
}
