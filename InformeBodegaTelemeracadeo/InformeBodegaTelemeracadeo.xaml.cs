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
    
    public partial class InformeBodegaTelemeracadeo : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";

        public InformeBodegaTelemeracadeo(dynamic tabitem1)
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
                tabitem.Title = "Informe Bodega(" + aliasemp + ")";

                TextBx_fecha_ini.Text = DateTime.Today.AddMonths(-1).ToString();
                TextBx_fecha_fin.Text = DateTime.Now.ToShortDateString();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void TBvendedor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string tag = ((TextBox)sender).Tag.ToString();
                string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                if (string.IsNullOrEmpty(tag)) return;

                if (tag == "inmae_bod")
                {
                    cmptabla = tag; cmpcodigo = "cod_bod"; cmpnombre = "UPPER(nom_bod)"; cmporden = "cod_bod"; cmpidrow = "cod_bod"; cmptitulo = "Maestra de Bodega"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
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
                    if (tag == "inmae_bod")
                    {
                        LBbodega.Text = code; TBbodega.Text = nom.Trim();
                        BTNconsultar.IsEnabled = true;
                        BTNexportar.IsEnabled = true;
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


        private void BTNconsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fe_fin = TextBx_fecha_fin.Text + " 23:59:59";

                string cadena = "select iif(LEN(seguimiento.cod_ter) > 0,'1','0') as filtro,seguimiento.fec_seg as fec_seg,seguimiento.cod_ter as cod_ter,cliente.nom_ter as nom_ter,seguimiento.cod_mer as cod_mer,vendedor.nom_mer as nom_mer,bodega.nom_bod as nom_bod,seguimiento.cod_con as cod_con,concepto.nom_con as nom_con,IIF(seguimiento.cod_camp='0','Ninguna',campaña.nom_camp) as nom_camp,seguimiento.contacto_cli as contacto_cli,seguimiento.observ as observ ";
                cadena = cadena + "from Crseg_cli as seguimiento ";
                cadena = cadena + "inner join Comae_ter as cliente on seguimiento.cod_ter = cliente.cod_ter ";
                cadena = cadena + "inner join InMae_mer as vendedor on seguimiento.cod_mer = vendedor.cod_mer ";
                cadena = cadena + "inner join CrMae_concepto as concepto on seguimiento.cod_con = concepto.cod_con ";
                cadena = cadena + "full join CrMae_campa as campaña on seguimiento.cod_camp  = campaña.cod_camp ";
                cadena = cadena + "inner join InMae_bod as bodega on seguimiento.cod_bod = bodega.cod_bod ";
                cadena = cadena + "where bodega.cod_bod ='" + LBbodega.Text + "' ";
                cadena = cadena + "and fec_seg between '" + TextBx_fecha_ini.Text + "' and '" + fe_fin + "' ";

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridTelemercadeo.ItemsSource = dt.DefaultView;
                Total.Text = dt.Rows.Count.ToString();

            }
            catch (Exception w)
            {
                MessageBox.Show("error al consultar:" + w);
            }

        }



        private void Exportar_Click(object sender, RoutedEventArgs e)
        {

            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = dataGridTelemercadeo.ExportToExcel(dataGridTelemercadeo.View, options);
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




        private void dataGrid_FilterChanged(object sender, GridFilterEventArgs e)
        {
            var provider = (sender as SfDataGrid).View.GetPropertyAccessProvider();
            var records = (sender as SfDataGrid).View.Records;


            double totalX = 0;

            for (int i = 0; i < (sender as SfDataGrid).View.Records.Count; i++)
            {
                totalX += Convert.ToDouble(provider.GetValue(records[i].Data, "filtro").ToString());
            }

            Total.Text = totalX.ToString();

        }




        // ROWS automatico ******************************************************************************
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();

        //To get the calculated height from GetAutoRowHeight method.    
        double autoHeight = double.NaN;

        // The list contains the column names that will excluded from the height calculation in GetAutoRowHeight method.
        List<string> excludeColumns = new List<string>() { "observ" };

        private void dataGridCxC_QueryRowHeight(object sender, Syncfusion.UI.Xaml.Grid.QueryRowHeightEventArgs e)
        {
            if (this.dataGridTelemercadeo.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
            {
                if (autoHeight > 20)
                {
                    e.Height = autoHeight;
                    e.Handled = true;
                }
            }
        }






    }
}
