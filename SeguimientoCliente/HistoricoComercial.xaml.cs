using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Threading;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.XlsIO;
using Syncfusion.UI.Xaml.Grid.Converter;
using Microsoft.Win32;
using System.IO;

namespace SeguimientoCliente
{
    /// <summary>
    /// Lógica de interacción para HistoricoComercial.xaml
    /// </summary>
    /// p
    public partial class HistoricoComercial : Window
    {

        dynamic SiaWin;        
        int idemp = 0;        
        string cnEmp = "";
        public string cod_cliente;
        public string nom_cliente;

        public HistoricoComercial()
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            

            this.MinWidth = 1200;
            this.MinHeight = 600;
            this.MaxWidth = 1200;
            this.MaxHeight = 600;

            ActivaDesactivaControles(1);
            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {

                System.Data.DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                //cnEmp = foundRow["BusinessCn"].ToString().Trim();
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
                //tabitem.Logo(idLogo, ".png");
                //tabitem.Title = "Analisis de Venta(" + aliasemp + ")";
                //GroupId = 0;
                //ProjectId = 0;
                //BusinessId = 0;
                FecIni.Text = DateTime.Today.AddYears(-2).ToString();

                FecFin.Text = DateTime.Now.ToShortDateString();

                TabControl1.SelectedIndex = 0;
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
                if (e.Key == System.Windows.Input.Key.F8)
                {
                    string tag = ((TextBox)sender).Tag.ToString();

                    if (string.IsNullOrEmpty(tag)) return;
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                    if (tag == "inmae_ref")
                    {
                        cmptabla = tag; cmpcodigo = "cod_ref"; cmpnombre = "nom_ref"; cmporden = "nom_ref"; cmpidrow = "idrow"; cmptitulo = "Maestra de productos"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "estado=1";
                    }
                    if (tag == "inmae_bod")
                    {
                        cmptabla = tag; cmpcodigo = "cod_bod"; cmpnombre = "nom_bod"; cmporden = "cod_bod"; cmpidrow = "idrow"; cmptitulo = "Maestra de bodegas"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "estado=1 and ind_vta=1";
                    }
                    if (tag == "comae_ter")
                    {
                        cmptabla = tag; cmpcodigo = "cod_ter"; cmpnombre = "nom_ter"; cmporden = "nom_ter"; cmpidrow = "idrow"; cmptitulo = "Maestra de terceros"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "";
                    }
                    if (tag == "inmae_mer")
                    {
                        cmptabla = tag; cmpcodigo = "cod_mer"; cmpnombre = "nom_mer"; cmporden = "cod_mer"; cmpidrow = "idrow"; cmptitulo = "Maestra de vendedores"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "inmae_tip")
                    {
                        cmptabla = tag; cmpcodigo = "cod_tip"; cmpnombre = "nom_tip"; cmporden = "cod_tip"; cmpidrow = "idrow"; cmptitulo = "Maestra de lineas"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }

                    //MessageBox.Show(cmptabla + "-" + cmpcodigo + "-" + cmpnombre + "-" + cmporden + "-" + cmpidrow + "-" + cmptitulo + "-" + cmpconexion + "-" + cmpwhere);
                    int idr = 0; string code = "";
                    dynamic winb = SiaWin.WindowBuscar(cmptabla, cmpcodigo, cmpnombre, cmporden, cmpidrow, cmptitulo, cnEmp, mostrartodo, cmpwhere);
                    winb.ShowInTaskbar = false;
                    winb.Owner = Application.Current.MainWindow;
                    winb.ShowDialog();
                    idr = winb.IdRowReturn;
                    code = winb.Codigo;
                    winb = null;
                    if (idr > 0)
                    {
                        ((TextBox)sender).Text = code;
                        if (tag == "inmae_ref") TextBoxRefF.Text = code;
                        if (tag == "inmae_bod") TextBoxBodF.Text = code;
                        if (tag == "inmae_tip") TextBoxTipF.Text = code;
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


        private void ConsultaAnalisis()
        {
            SqlConnection con = new SqlConnection(cnEmp);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("SpConsultaInAnalisisDeVentas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FechaIni", FecIni.Text);//if you have parameters.
            cmd.Parameters.AddWithValue("@FechaFin", FecFin.Text);//if you have parameters.

            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            foreach (DataTable table in ds.Tables)

            {
                //            newColumn.DefaultValue = "Your DropDownList value";
                System.Data.DataColumn newColumn = new System.Data.DataColumn("ven_net", typeof(System.Double));
                System.Data.DataColumn newColumn1 = new System.Data.DataColumn("util", typeof(System.Double));
                System.Data.DataColumn newColumn2 = new System.Data.DataColumn("por_util", typeof(System.Double));
                System.Data.DataColumn newColumn3 = new System.Data.DataColumn("por_parti", typeof(System.Double));
                System.Data.DataColumn newColumn4 = new System.Data.DataColumn("can_net", typeof(System.Double));
                ds.Tables[table.TableName].Columns.Add(newColumn);
                ds.Tables[table.TableName].Columns.Add(newColumn1);
                ds.Tables[table.TableName].Columns.Add(newColumn2);
                ds.Tables[table.TableName].Columns.Add(newColumn3);
                ds.Tables[table.TableName].Columns.Add(newColumn4);
            }
            //VentasPorProducto.ItemsSource = ds.Tables[0];
            //VentaPorBodega.ItemsSource = ds.Tables[1];
            //VentasPorCliente.ItemsSource = ds.Tables[2];
        }

        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            //this.Opacity = 0.5;
            
            try
            {
                string where = ArmaWhere();
                //if (where==null) return;
                //MessageBox.Show(where);
                // carmar where
                if (string.IsNullOrEmpty(where)) where = " ";

                //               busy.IsBusy = true;
                //       busy.Visibility=Visibility.Visible;
                //dataGrid.Opacity = 0.5;
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                GridConfiguracion.IsEnabled = false;
                sfBusyIndicator.IsBusy = true;
                //    LoadData(recordChanged());
                //dataGrid.Model.View.Refresh();
                VentasPorProducto.ItemsSource = null;
                VentaPorBodega.ItemsSource = null;
                VentasPorCliente.ItemsSource = null;
                VentasPorLinea.ItemsSource = null;
                VentasPorGrupo.ItemsSource = null;
                CharVentasBodega.DataContext = null;
                AreaSeriesVta.ItemsSource = null;
                BtnEjecutar.IsEnabled = false;
                source.CancelAfter(TimeSpan.FromSeconds(1));
                //tabitem.Progreso(true);
                string ffi = FecIni.Text.ToString();
                string fff = FecFin.Text.ToString();
                var slowTask = Task<DataSet>.Factory.StartNew(() => SlowDude(ffi, fff, where, source.Token), source.Token);
                await slowTask;
                //MessageBox.Show(slowTask.Result.ToString());
                BtnEjecutar.IsEnabled = true;
                //tabitem.Progreso(false);
                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    VentasPorProducto.ItemsSource = ((DataSet)slowTask.Result).Tables[0];
                    VentaPorBodega.ItemsSource = ((DataSet)slowTask.Result).Tables[1];
                    VentasPorCliente.ItemsSource = ((DataSet)slowTask.Result).Tables[2];
                    VentasPorVendedor.ItemsSource = ((DataSet)slowTask.Result).Tables[3];
                    VentasPorLinea.ItemsSource = ((DataSet)slowTask.Result).Tables[4];
                    VentasPorGrupo.ItemsSource = ((DataSet)slowTask.Result).Tables[5];
                    VentasPorFPago.ItemsSource = ((DataSet)slowTask.Result).Tables[6];
                    CharVentasBodega.DataContext = ((DataSet)slowTask.Result).Tables[1];
                    AreaSeriesVta.ItemsSource = ((DataSet)slowTask.Result).Tables[1];
                    VentasPorClienteRef.ItemsSource = ((DataSet)slowTask.Result).Tables[7];
                    TabControl1.SelectedIndex = 2;
                    TabControl1.SelectedIndex = 1;
                    double sub = Convert.ToDouble(((DataSet)slowTask.Result).Tables[0].Compute("Sum(subtotal)", "").ToString());
                    double descto = Convert.ToDouble(((DataSet)slowTask.Result).Tables[0].Compute("Sum(val_des)", "").ToString());
                    double iva = Convert.ToDouble(((DataSet)slowTask.Result).Tables[0].Compute("Sum(val_iva)", "").ToString());
                    double total = Convert.ToDouble(((DataSet)slowTask.Result).Tables[0].Compute("Sum(total)", "").ToString());

                    TextSubtotal.Text = sub.ToString("C");
                    TextDescuento.Text = descto.ToString("C");
                    TextIva.Text = iva.ToString("C");
                    TextTotal.Text = total.ToString("C");
                    //TextTotalEntradas.Text = ds.Tables["Traslados"].Compute("Sum(cantidad)", "").ToString();
                }

                //dataGrid.ItemsSource = Referencias;
                //        return;
                //    recordChanged();
                //    updateRow(9619);

                //    var slowTask = Task<string>.Factory.StartNew(() => LoadData(""));

                //     await slowTask;
                //     Txt.Content += slowTask.Result.ToString();
                //        busy.IsBusy = false;
                //    busy.Visibility=Visibility.Collapsed;
                //this.Opacity = 1;
                this.sfBusyIndicator.IsBusy = false;
                GridConfiguracion.IsEnabled = true;
                //   dataGrid.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Opacity = 1;
            }            
        }

        private string ArmaWhere()
        {
            string cadenawhere = null;
            string RefI = TextBoxRefI.Text.Trim();
            string RefF = TextBoxRefF.Text.Trim();
            string BodI = TextBoxBodI.Text.Trim();
            string BodF = TextBoxBodF.Text.Trim();
            string TerI = TextBoxTerI.Text.Trim();
            string VenI = TextBoxVenI.Text.Trim();
            string TipI = TextBoxTipI.Text.Trim();
            string TipF = TextBoxTipF.Text.Trim();
            string ImpI = TextBoxImpI.Text.Trim();
            if (!string.IsNullOrEmpty(RefI) && !string.IsNullOrEmpty(RefF))
            {
                cadenawhere += " and  cue.cod_ref between '" + RefI + "' and '" + RefF + "'";
            }
            if (!string.IsNullOrEmpty(BodI) && !string.IsNullOrEmpty(BodF))
            {
                cadenawhere += " and  cue.cod_bod between '" + BodI + "' and '" + BodF + "'";
            }
            if (!string.IsNullOrEmpty(TerI))
            {
                cadenawhere += " and  cab.cod_cli='" + TerI + "'";
            }
            if (!string.IsNullOrEmpty(VenI))
            {
                cadenawhere += " and  cab.cod_Ven='" + VenI + "'";
            }
            if (!string.IsNullOrEmpty(TipI) && !string.IsNullOrEmpty(TipF))
            {
                cadenawhere += " and  ref.cod_tip between '" + TipI + "' and '" + TipF + "'";
            }
            if (!string.IsNullOrEmpty(ImpI))
            {
                cadenawhere += " and  ref.im='" + ImpI + "'";
            }


            return cadenawhere;
        }

        private DataSet SlowDude(string ffi, string fff, string where, CancellationToken cancellationToken)
        {
            try
            {

                DataSet jj = LoadData(ffi, fff, where, cancellationToken);
                return jj;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        private DataSet LoadData(string Fi, string Ff, string where, CancellationToken cancellationToken)
        {
            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("SpConsultaInAnalisisDeVentas", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaIni", Fi);//if you have parameters.
                cmd.Parameters.AddWithValue("@FechaFin", Ff);//if you have parameters.
                cmd.Parameters.AddWithValue("@Where", where);//if you have parameters.
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                foreach (DataTable table in ds.Tables)
                {
                    //            newColumn.DefaultValue = "Your DropDownList value";
                    System.Data.DataColumn newColumn = new System.Data.DataColumn("ven_net", typeof(System.Double));
                    System.Data.DataColumn newColumn1 = new System.Data.DataColumn("util", typeof(System.Double));
                    System.Data.DataColumn newColumn2 = new System.Data.DataColumn("por_util", typeof(System.Double));
                    System.Data.DataColumn newColumn3 = new System.Data.DataColumn("por_parti", typeof(System.Double));
                    System.Data.DataColumn newColumn4 = new System.Data.DataColumn("can_net", typeof(System.Double));
                    ds.Tables[table.TableName].Columns.Add(newColumn);
                    ds.Tables[table.TableName].Columns.Add(newColumn1);
                    ds.Tables[table.TableName].Columns.Add(newColumn2);
                    ds.Tables[table.TableName].Columns.Add(newColumn3);
                    ds.Tables[table.TableName].Columns.Add(newColumn4);
                    
                    
                }
                return ds;
                //VentasPorProducto.ItemsSource = ds.Tables[0];
                //VentaPorBodega.ItemsSource = ds.Tables[1];
                //VentasPorCliente.ItemsSource = ds.Tables[2];
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        private void TextBoxRefI_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.Key.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            //            MessageBox.Show(((Button)sender).Tag.ToString());
            SfDataGrid sfdg = new SfDataGrid();
            if (((Button)sender).Tag.ToString() == "1") sfdg = VentasPorProducto;
            if (((Button)sender).Tag.ToString() == "2") sfdg = VentaPorBodega;
            if (((Button)sender).Tag.ToString() == "3") sfdg = VentasPorCliente;
            if (((Button)sender).Tag.ToString() == "4") sfdg = VentasPorVendedor;
            if (((Button)sender).Tag.ToString() == "5") sfdg = VentasPorLinea;
            if (((Button)sender).Tag.ToString() == "6") sfdg = VentasPorGrupo;
            if (((Button)sender).Tag.ToString() == "7") sfdg = VentasPorFPago;

            var excelEngine = sfdg.ExportToExcel(sfdg.View, options);
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
        //*******************************************
        

        public void ActivaDesactivaControles(int estado)
        {
            if (estado == 0)
            {
                FecIni.IsEnabled = false;
                FecFin.IsEnabled = false;
                TextBoxTerI.IsEnabled = false;

                BtnEjecutar.IsEnabled = false;
                
            }
            if (estado == 1) 
            {
                FecIni.IsEnabled = true;
                FecFin.IsEnabled = true;
                TextBoxTerI.IsEnabled = false;

                BtnEjecutar.IsEnabled = true;               
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            TextBoxTerI.Text = cod_cliente;
            TextBoxTerNom.Text = nom_cliente;
        }



    }
}
