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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SeguimientoCliente
{
    /// <summary>
    /// Lógica de interacción para SeguimientoCompra.xaml
    /// </summary>
    public partial class SeguimientoCompra : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string vendedor = "";
        string codigoUsuario = "";
        string cnEmp = "";
        public string Conexion;
        dynamic tabitem = "";



        public string cod_cli = "";
        public string nombre_cli = "";
        public string nombre_ven = "";
        public string cod_ven = "";

        public SeguimientoCompra()
        {
            InitializeComponent();

            this.MinWidth = 1000;
            this.MinHeight = 500;
            this.MaxWidth = 1000;
            this.MaxHeight = 500;

            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserTag1;

            LoadConfig();

            fecha_ini.Text = DateTime.Now.AddMonths(-1).ToString();
            fecha_fin.Text = DateTime.Now.ToString();
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

                    if (tag == "CrMae_detalle")
                    {
                        cmptabla = tag; cmpcodigo = "cod_detalle"; cmpnombre = "UPPER(nom_detalle)"; cmporden = "cod_detalle"; cmpidrow = "cod_detalle"; cmptitulo = "Maestra de Detalle"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
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
                        if (tag == "CrMae_detalle")
                        {
                            LB_1.Text = code; TB_1.Text = nom;
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

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int selectedIndex = CB_compro.SelectedIndex;

            if (selectedIndex == 0)
            {
                TX_1.Visibility = Visibility.Visible;
                TB_1.Visibility = Visibility.Visible;
                TX_2.Visibility = Visibility.Visible;
                CB_tipo_compra.Visibility = Visibility;

                TX_3.Visibility = Visibility.Hidden;                
                CB_no_compra.Visibility = Visibility.Hidden;
                CB_no_compra.Text = "";
                TextBx_obse.IsEnabled = true;
                TextBx_obse.Text = "NINGUNA";

                BTNcancelar.IsEnabled = true;
                BTNregistrar.IsEnabled = true;

            }
            if (selectedIndex == 1)
            {
                TX_1.Visibility = Visibility.Hidden;
                TB_1.Visibility = Visibility.Hidden;
                TB_1.Text = "";
                LB_1.Text = "";

                TX_2.Visibility = Visibility.Hidden;
                CB_tipo_compra.Visibility = Visibility.Hidden;
                CB_tipo_compra.Text = "";

                TX_3.Visibility = Visibility.Visible;
                CB_no_compra.Visibility = Visibility.Visible;

                TextBx_obse.IsEnabled = true;
                TextBx_obse.Text = "NINGUNA";

                BTNcancelar.IsEnabled = true;
                BTNregistrar.IsEnabled = true;
            }



        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBx_ComCli.Text = cod_cli;
            TextBx_NomCli.Text = nombre_cli;
            TextBx_Vendedor.Text = nombre_ven;
            LB_vendedor.Text = cod_ven;
            
        }

        
        private void Click_Registrar(object sender, RoutedEventArgs e)
        {
            
            int selectedIndex = CB_compro.SelectedIndex;

            if (selectedIndex == 0)
            {
                if (CB_compro.Text.Length > 0 && LB_1.Text.Length > 0 && CB_tipo_compra.Text.Length > 0 && TextBx_obse.Text.Length > 0)
                {
                    try
                    {
                        string cadena = "insert into Crseg_Compra (fec_seg,cod_ter,cod_mer, compra, cod_detalle, tipo_compra ,observ) values ('"+ DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', '" + cod_cli + "', '" + cod_ven + "', '" + CB_compro.Text + "', '" + LB_1.Text + "', '" + CB_tipo_compra.Text + "', '" + TextBx_obse.Text + "' )";                       
                        SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                        MessageBox.Show("Seguimiento de la compra exitosa");
                        this.Close();
                    }
                    catch (Exception w)
                    {
                        MessageBox.Show("error en el registro del seguimiento"+w);
                    }                    
                }
                else {
                    MessageBox.Show("completa los campos del seguimiento de la compra");
                }

            }
            else {

                if (CB_compro.Text.Length > 0 && CB_no_compra.Text.Length > 0 && TextBx_obse.Text.Length > 0)
                {
                    try
                    {
                       string cadena = "insert into Crseg_Compra (fec_seg,cod_ter,cod_mer, compra, no_compra ,observ) values ('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', '" + cod_cli + "', '" + cod_ven + "', '" + CB_compro.Text + "', '" + CB_no_compra.Text + "',  '" + TextBx_obse.Text + "' )";                       
                       SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                       MessageBox.Show("Seguimiento exitoso");
                       this.Close();
                    }
                    catch (Exception w)
                    {
                        MessageBox.Show("error en el registro del seguimiento"+w);
                    }
                }
                else {
                    MessageBox.Show("completa los campos del por que no compro");
                }
                
            }    

        }

        private void Click_Cancelar(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        //segundo tab ********************************


        private void CB_compro_bus_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {            
            int selectedIndex = CB_compro_bus.SelectedIndex;

            if (selectedIndex == 0)
            {
                BTNeje.IsEnabled = true;
                BTNxls.IsEnabled = true;
            }
            if (selectedIndex == 1)
            {
                BTNeje.IsEnabled = true;
                BTNxls.IsEnabled = true;
            }

        }

        //private void cargarGrid(object sender, RoutedEventArgs e) {

        //    try
        //    {

        //        string fe_fin = fecha_fin.Text + " 23:59:59";

        //        string cadena = "select seguimineto.fec_seg as fec_seg,seguimineto.cod_ter as cod_ter,tercero.nom_ter as nom_ter,seguimineto.cod_mer as cod_mer,vendedor.nom_mer as nom_mer,seguimineto.compra as compra,seguimineto.cod_detalle as cod_detalle,detalle.nom_detalle as nom_detalle,seguimineto.tipo_compra as tipo_compra,seguimineto.no_compra as no_compra,seguimineto.observ as observ ";
        //        cadena = cadena + "from Crseg_Compra as seguimineto ";
        //        cadena = cadena + "full join CrMae_detalle as detalle on detalle.cod_detalle = seguimineto.cod_detalle ";
        //        cadena = cadena + "full join Comae_ter as tercero on tercero.cod_ter = seguimineto.cod_ter ";
        //        cadena = cadena + "full join inmae_mer as vendedor on vendedor.cod_mer = seguimineto.cod_mer ";
        //        cadena = cadena + "where seguimineto.compra='" + CB_compro_bus.Text  + "' and tercero.cod_ter='" + TextBx_ComCli.Text + "' ";
        //        cadena = cadena + "and fec_seg between '" + fecha_ini.Text + "' and '" + fe_fin + "' ";

        //        if (CB_compro_bus.Text == "SI")
        //        {
        //            _nom_detalle.IsHidden = false;
        //            _tipo_compra.IsHidden = false;
        //            _no_compra.IsHidden = true;

        //            DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
        //            dataGridCxC.ItemsSource = dt.DefaultView;
        //        }            
        //        else {
        //            _nom_detalle.IsHidden = true;
        //            _tipo_compra.IsHidden = true;
        //            _no_compra.IsHidden = false;

        //            DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
        //            dataGridCxC.ItemsSource = dt.DefaultView;
        //        }

        //    }
        //    catch (Exception w)
        //    {
        //        MessageBox.Show("error cargar:"+w);
        //    }

        //}


        private async void cargarGrid(object sender, RoutedEventArgs e)
        {
            try
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                PanelCons.IsEnabled = false;
                sfBusyIndicator.IsBusy = true;
                dataGridCxC.ItemsSource = null;

                string fechaIni = fecha_ini.Text;
                string fechaFin = fecha_fin.Text;
                string compraCbx = CB_compro_bus.Text;
                string codigo = TextBx_ComCli.Text;

                var slowTask = Task<DataSet>.Factory.StartNew(() => SlowDude(fechaIni, fechaFin, compraCbx, codigo, source.Token), source.Token);
                await slowTask;

                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    dataGridCxC.ItemsSource = ((DataSet)slowTask.Result).Tables[0];
                    TotalReg.Text = ((DataSet)slowTask.Result).Tables[0].Rows.Count.ToString();

                    if (compraCbx == "SI")
                    {
                        _nom_detalle.IsHidden = false;
                        _tipo_compra.IsHidden = false;
                        _no_compra.IsHidden = true;
                    }
                    else
                    {
                        _nom_detalle.IsHidden = true;
                        _tipo_compra.IsHidden = true;
                        _no_compra.IsHidden = false;
                    }
                }
                this.sfBusyIndicator.IsBusy = false;
                PanelCons.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Opacity = 1;
                MessageBox.Show("aqui 2" + ex);

            }
        }

        private DataSet SlowDude(string ffi, string fff, string comp, string codigo, CancellationToken cancellationToken)
        {
            try
            {
                DataSet jj = LoadData(ffi, fff, comp, codigo, cancellationToken);
                return jj;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }


        private DataSet LoadData(string Fi, string Ff,string compra,string codigo, CancellationToken cancellationToken)
        {
            try
            {
                DataSet ds = new DataSet();

                string cadena = "select seguimineto.fec_seg as fec_seg,seguimineto.cod_ter as cod_ter,tercero.nom_ter as nom_ter,seguimineto.cod_mer as cod_mer,vendedor.nom_mer as nom_mer,seguimineto.compra as compra,seguimineto.cod_detalle as cod_detalle,detalle.nom_detalle as nom_detalle,seguimineto.tipo_compra as tipo_compra,seguimineto.no_compra as no_compra,seguimineto.observ as observ ";
                cadena = cadena + "from Crseg_Compra as seguimineto ";
                cadena = cadena + "full join CrMae_detalle as detalle on detalle.cod_detalle = seguimineto.cod_detalle ";
                cadena = cadena + "full join Comae_ter as tercero on tercero.cod_ter = seguimineto.cod_ter ";
                cadena = cadena + "full join inmae_mer as vendedor on vendedor.cod_mer = seguimineto.cod_mer ";
                cadena = cadena + "where seguimineto.compra='" + compra + "' and tercero.cod_ter='" + codigo + "' ";
                cadena = cadena + "and fec_seg between '" + Fi + "' and '" + Ff + " 23:59:59 ' ";
                //MessageBox.Show("cadena:" + cadena);              
                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);                
                ds.Tables.Add(dt);                
                return ds;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
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
