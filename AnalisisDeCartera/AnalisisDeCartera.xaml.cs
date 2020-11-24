using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AnalisisDeCartera;
using Syncfusion.XlsIO;
using Microsoft.Win32;
using System.IO;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Data;
using System.Linq;
namespace SiasoftAppExt
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class AnalisisDeCartera : UserControl
    {
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";
        DataSet ds = new DataSet();
        DataTable Cuentas = new DataTable();
        public AnalisisDeCartera(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            tabitem.Title = "Analisis de Cartera";
            tabitem.Logo(9, ".png");
            tabitem.MultiTab = false;
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
                tabitem.Title = "Analisis de Cartera(" + aliasemp + ")";
                //GroupId = 0;
                //ProjectId = 0;
                //BusinessId = 0;
                Cuentas = SiaWin.Func.SqlDT("SELECT rtrim(cod_cta) as cod_cta,rtrim(cod_cta)+'('+rtrim(nom_cta)+')' as nom_cta FROM COMAE_CTA WHERE ind_mod = 1 and (tip_apli = 3 or tip_apli = 4 ) ORDER BY COD_CTA", "Cuentas", idemp);
                comboBoxCuentas.ItemsSource = Cuentas.DefaultView;
                //comboBoxCuentas.DataContext = Cuentas;
                comboBoxCuentas.DisplayMemberPath = "nom_cta";
                comboBoxCuentas.SelectedValuePath = "cod_cta";
               FechaIni.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Salir de cartera");
            tabitem.Cerrar(0);
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.F8)
                {
                    string tag = ((TextBox)sender).Tag.ToString();
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                    if (string.IsNullOrEmpty(tag)) return;
                    if (tag == "inmae_mer")
                    {
                        cmptabla = tag; cmpcodigo = "cod_mer"; cmpnombre = "nom_mer"; cmporden = "cod_mer"; cmpidrow = "idrow"; cmptitulo = "Maestra de vendedores"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "inmae_bod")
                    {
                        cmptabla = tag; cmpcodigo = "cod_bod"; cmpnombre = "nom_bod"; cmporden = "cod_bod"; cmpidrow = "idrow"; cmptitulo = "Maestra de bodegas"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "estado=1 and ind_vta=1";
                    }
                    //MessageBox.Show(cmptabla + "-" + cmpcodigo + "-" + cmpnombre + "-" + cmporden + "-" + cmpidrow + "-" + cmptitulo + "-" + cmpconexion + "-" + cmpwhere);
                    int idr = 0; string code = "";string nom = "";
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
                        //((TextBox)sender).Text = code;
                        if (tag == "inmae_mer")
                        {
                            TextCod_Ven.Text = code; TextNombre.Text = nom;
                        }
                        if (tag == "inmae_bod")
                        {
                            TextCod_bod.Text = code; TextNombreBod.Text = nom;
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

        private void TextCod_Ven_LostFocus(object sender, RoutedEventArgs e)
        {
            string tag = ((TextBox)sender).Tag.ToString();
            if (tag == "inmae_mer")
            {
                if (TextCod_Ven.Text.Trim() == "") TextNombre.Text = "F8=Consultar";
            }
            if (tag == "inmae_bod")
            {
                if (TextCod_bod.Text.Trim() == "") TextNombreBod.Text = "F8=Consultar";
            }

        }

        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            //if(comboBoxCuentas.SelectedIndex<0)
            //{
              //  MessageBox.Show("Seleccione una cuenta...");
                //comboBoxCuentas.Focus();
                //comboBoxCuentas.IsDropDownOpen = true;
                //return;
            //}
//            DataRowView drv = (DataRowView)comboBoxCuentas.SelectedItem;
//            String valueOfItem = drv["cod_cta"].ToString();
//            MessageBox.Show(valueOfItem);
            string Cta = "";
            if (comboBoxCuentas.SelectedIndex >= 0)
            {
                foreach (DataRowView ob in comboBoxCuentas.SelectedItems)
                {
                    //dr["cod_ter"].ToString();
                    String valueCta = ob["cod_cta"].ToString();

                    Cta += valueCta + ",";
                    //MessageBox.Show(valueOfItem1.ToString());
                }
                string ss = Cta.Trim().Substring(Cta.Trim().Length - 1);
                if (ss == ",") Cta = Cta.Substring(0, Cta.Trim().Length - 1);

            }
            //MessageBox.Show(Cta);
//            Cta = "";
            //this.Opacity = 0.5;
            try
            {
                //string where = ArmaWhere();
                string where = "";
                //if (where==null) return;
                //MessageBox.Show(where);
                // carmar where
                if (string.IsNullOrEmpty(where)) where = " ";

                //               busy.IsBusy = true;
                //       busy.Visibility=Visibility.Visible;
                //dataGrid.Opacity = 0.5;
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                this.IsEnabled = false;
                sfBusyIndicator.IsBusy = true;
                //    LoadData(recordChanged());
                //dataGrid.Model.View.Refresh();
                dataGridCxC.ClearFilters();
                dataGridCxC.ItemsSource = null;
                
                //CharVentasBodega.DataContext = null;
                //AreaSeriesVta.ItemsSource = null;
                ds.Clear();
                BtnEjecutar.IsEnabled = false;
                source.CancelAfter(TimeSpan.FromSeconds(1));
                tabitem.Progreso(true);
                string ffi = FechaIni.Text.ToString();
                string cco = TextCod_bod.Text.Trim();
                
                var slowTask = Task<DataSet>.Factory.StartNew(() => SlowDude(ffi, Cta,cco, where, source.Token), source.Token);
                await slowTask;
                //MessageBox.Show(slowTask.Result.ToString());
                BtnEjecutar.IsEnabled = true;
                tabitem.Progreso(false);
                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    //ds.Tables.Add(((DataSet)slowTask.Result).Tables[0]);
                    //ds.Tables[0] = ((DataSet)slowTask.Result).Tables[0];
                    //dataGridCxC.ItemsSource = ((DataSet)slowTask.Result).Tables[0];
                    dataGridCxC.ItemsSource = ds.Tables[0];
                    //((DataSet)slowTask.Result).Tables[0];
                    //CharVentasBodega.DataContext = ((DataSet)slowTask.Result).Tables[1];
                    // AreaSeriesVta.ItemsSource = ((DataSet)slowTask.Result).Tables[1];
                    double valorCxC, valorCxCAnt = 0;
                    //double valorCxCAnt = 0;
                    double valorCxP = 0;
                    double valorCxPAnt = 0;
                    double saldoCxC = 0;
                    double saldoCxCAnt = 0;
                    double saldoCxP = 0;
                    double saldoCxPAnt = 0;
                    double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=3").ToString(), out valorCxC);
                    double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=4").ToString(), out valorCxCAnt);
                    double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=1").ToString(), out valorCxP);
                    double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=2").ToString(), out valorCxPAnt);
                    double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=3").ToString(), out saldoCxC);
                    double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=4").ToString(), out saldoCxCAnt);
                    double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=1").ToString(), out saldoCxP);
                    double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=2").ToString(), out saldoCxPAnt);
                    //double valorA = Convert.ToDouble(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=1 or tip_apli=4").ToString());
                    //double saldo = Convert.ToDouble(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=2 or tip_apli=3").ToString());
                    TextCxC.Text=valorCxC.ToString("C");
                    TextCxCAnt.Text = valorCxCAnt.ToString("C");
                    TextCxCAbono.Text = (valorCxC-saldoCxC).ToString("C");
                    TextCxCAntAbono.Text = (valorCxCAnt-saldoCxCAnt).ToString("C");
                    TextCxCSaldo.Text = saldoCxC.ToString("C");
                    TextCxCAntSaldo.Text = saldoCxCAnt.ToString("C");
                    TotalCxc.Text = (valorCxC - valorCxCAnt - valorCxP + valorCxPAnt).ToString("C");
                    TotalAbono.Text = ((valorCxC - saldoCxC) - (valorCxCAnt - saldoCxCAnt)).ToString("C");
                    TotalSaldo.Text = (saldoCxC - saldoCxCAnt - saldoCxP + saldoCxPAnt).ToString("C");
                    //double saldoA = Convert.ToDouble(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=1 or tip_apli=4").ToString());
                    //TextTotalDoc.Text = (valor-valorA).ToString("C");
                    //TextSaldo.Text = (saldo-saldoA).ToString("C");
                }
                else
                {
                    //TextTotalDoc.Text = "0";
                    //TextSaldo.Text = "0";
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

                this.IsEnabled = true;
                //   dataGrid.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Opacity = 1;
            }
        }
        private DataSet SlowDude(string ffi, string ctas,string cco, string where, CancellationToken cancellationToken)
        {
            try
            {
                DataSet jj = LoadData(ffi, ctas,cco, where, cancellationToken);
                return jj;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }
        private DataSet LoadData(string Fi, string ctas, string cco, string where, CancellationToken cancellationToken)
        {
            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                //DataSet ds1 = new DataSet();
                //cmd = new SqlCommand("ConsultaCxcCxpAll", con);
                cmd = new SqlCommand("SpCoAnalisisCxc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ter", "");//if you have parameters.
                cmd.Parameters.AddWithValue("@Cta", ctas);//if you have parameters.
                cmd.Parameters.AddWithValue("@TipoApli", 1);//if you have parameters.
                cmd.Parameters.AddWithValue("@Resumen", 0);//if you have parameters.
                cmd.Parameters.AddWithValue("@Fecha", Fi);//if you have parameters.
                cmd.Parameters.AddWithValue("@TrnCo", "");//if you have parameters.
                cmd.Parameters.AddWithValue("@NumCo", "");//if you have parameters.
                cmd.Parameters.AddWithValue("@Cco", cco);//if you have parameters.
                //cmd.Parameters.AddWithValue("@Where", where);//if you have parameters.
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
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
      
        private void BtnDetalle_Click(object sender, RoutedEventArgs e)
        {
            //if (comboBoxCuentas.SelectedIndex < 0)
           // {
             //   MessageBox.Show("Seleccione una cuenta...");
               // comboBoxCuentas.Focus();
                //comboBoxCuentas.IsDropDownOpen = true;
                //return;
            //}
            //            DataRowView drv = (DataRowView)comboBoxCuentas.SelectedItem;
            //            String valueOfItem = drv["cod_cta"].ToString();
            //            MessageBox.Show(valueOfItem);
            string Cta = "";
            if (comboBoxCuentas.SelectedIndex > 0)
            {
                foreach (DataRowView ob in comboBoxCuentas.SelectedItems)
                {
                    //dr["cod_ter"].ToString();
                    String valueCta = ob["cod_cta"].ToString();
                    Cta += valueCta + ",";
                    //MessageBox.Show(valueOfItem1.ToString());
                }
                string ss = Cta.Trim().Substring(Cta.Trim().Length - 1);
                if (ss == ",") Cta = Cta.Substring(0, Cta.Trim().Length - 1);
            }
            try
            {
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                if (row == null)
                {
                    MessageBox.Show("Registro sin datos");
                    return;
                }
                string cod_cli = row[0].ToString();
                string cod_cta = row[2].ToString();
                //                var dr1 = dataGridCxC.SelectedItems;

//                    string cod_cli = dr["cod_ter"].ToString();
  //                  if (string.IsNullOrEmpty(cod_cli)) return;
    //                string cod_cta = dr["cod_cta"].ToString();
                    SqlConnection con = new SqlConnection(cnEmp);
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter da = new SqlDataAdapter();
                    DataSet ds1 = new DataSet();
                    //cmd = new SqlCommand("ConsultaCxcCxpDeta", con);
                    cmd = new SqlCommand("SpCoAnalisisCxc", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ter", cod_cli);//if you have parameters.
                    cmd.Parameters.AddWithValue("@Cta", Cta);//if you have parameters.
                    cmd.Parameters.AddWithValue("@TipoApli", 1);//if you have parameters.
                    cmd.Parameters.AddWithValue("@Resumen", 1);//if you have parameters.
                    cmd.Parameters.AddWithValue("@Fecha", FechaIni.Text);//if you have parameters.
                    cmd.Parameters.AddWithValue("@TrnCo", "");//if you have parameters.
                    cmd.Parameters.AddWithValue("@NumCo", "");//if you have parameters.
                    cmd.Parameters.AddWithValue("@Cco", TextCod_bod.Text.Trim());//if you have parameters.
                                                          //cmd.Parameters.AddWithValue("@Where", where);//if you have parameters.
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds1);
                    con.Close();
                    if(ds1.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Sin informacion de cartera");
                        return;
                    }
                    AnalisisDeCarteraDetalle WinDetalle = new AnalisisDeCarteraDetalle();
                    WinDetalle.TextCodigo.Text = cod_cli;
                    WinDetalle.TextNombre.Text = row["nom_ter"].ToString();
                    WinDetalle.TextCuenta.Text = cod_cta;
                    WinDetalle.Title = "Detalle de cartera - Fecha De Corte:" + FechaIni.Text.ToString();
                    WinDetalle.dataGridCxC.ItemsSource = ds1.Tables[0];
                // TOTALIZA 

                double valorCxC, valorCxCAnt = 0;
                //double valorCxCAnt = 0;
                double valorCxP = 0;
                double valorCxPAnt = 0;
                double saldoCxC = 0;
                double saldoCxCAnt = 0;
                double saldoCxP = 0;
                double saldoCxPAnt = 0;
                double.TryParse(ds1.Tables[0].Compute("Sum(valor)", "tip_apli=3").ToString(), out valorCxC);
                double.TryParse(ds1.Tables[0].Compute("Sum(valor)", "tip_apli=4").ToString(), out valorCxCAnt);
                double.TryParse(ds1.Tables[0].Compute("Sum(valor)", "tip_apli=1").ToString(), out valorCxP);
                double.TryParse(ds1.Tables[0].Compute("Sum(valor)", "tip_apli=2").ToString(), out valorCxPAnt);
                double.TryParse(ds1.Tables[0].Compute("Sum(saldo)", "tip_apli=3").ToString(), out saldoCxC);
                double.TryParse(ds1.Tables[0].Compute("Sum(saldo)", "tip_apli=4").ToString(), out saldoCxCAnt);
                double.TryParse(ds1.Tables[0].Compute("Sum(saldo)", "tip_apli=1").ToString(), out saldoCxP);
                double.TryParse(ds1.Tables[0].Compute("Sum(saldo)", "tip_apli=2").ToString(), out saldoCxPAnt);
                WinDetalle.TextCxC.Text = valorCxC.ToString("C");
                WinDetalle.TextCxCAnt.Text = valorCxCAnt.ToString("C");
                WinDetalle.TextCxCAbono.Text = (valorCxC - saldoCxC).ToString("C");
                WinDetalle.TextCxCAntAbono.Text = (valorCxCAnt - saldoCxCAnt).ToString("C");
                WinDetalle.TextCxCSaldo.Text = saldoCxC.ToString("C");
                WinDetalle.TextCxCAntSaldo.Text = saldoCxCAnt.ToString("C");
                WinDetalle.TotalCxc.Text = (valorCxC - valorCxCAnt - valorCxP + valorCxPAnt).ToString("C");
                WinDetalle.TotalAbono.Text = ((valorCxC - saldoCxC) - (valorCxCAnt - saldoCxCAnt)).ToString("C");
                WinDetalle.TotalSaldo.Text = (saldoCxC - saldoCxCAnt - saldoCxP + saldoCxPAnt).ToString("C");
                WinDetalle.Owner = SiaWin;
                
                //WinDetalle.dataGridCxC_FilterChanged1();
                WinDetalle.ShowDialog();

                WinDetalle = null;
                    //ImprimirDoc(Convert.ToInt32(numtrn), "Reimpreso");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }
        private void ExportarXls_Click(object sender, RoutedEventArgs e)
        {
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExportMode = ExportMode.Value;
            options.ExcelVersion = ExcelVersion.Excel2013;
            options.CellsExportingEventHandler = CellExportingHandler;


            var excelEngine = dataGridCxC.ExportToExcel(dataGridCxC.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            workBook.ActiveSheet.Columns[4].NumberFormat = "0.0";
            workBook.ActiveSheet.Columns[5].NumberFormat = "0.0";
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
                if (MessageBox.Show("Usted quiere abrir el archivo en excel?", "Ver archvo",MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
        }
        private static void CellExportingHandler(object sender, GridCellExcelExportingEventArgs e)
        {
            e.Range.CellStyle.Font.Size = 12;
            e.Range.CellStyle.Font.FontName = "Segoe UI";

            if (e.ColumnName == "valor" || e.ColumnName == "sinvenc" || e.ColumnName == "ven01" || e.ColumnName == "ven02" || e.ColumnName == "ven03" || e.ColumnName == "ven04" || e.ColumnName == "ven05" || e.ColumnName == "saldo")
            {
                double value = 0;
                if (double.TryParse(e.CellValue.ToString(), out value))
                {
                    e.Range.Number = value;
                }
                e.Handled = true;
            }
        }


        private void comboBoxCuentas_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            dataGridCxC.ClearFilters();
           dataGridCxC.ItemsSource = null;
        }
        private void dataGridCxC_FilterChanged(object sender, GridFilterEventArgs e)
        {
            //MessageBox.Show("1");
           // MessageBox.Show("filter:"+( sender as SfDataGrid).View.Records.Count.ToString());
//            var columnName = e.Column.MappingName;
  //          var filteredResult =(sender as SfDataGrid).View.Records.Select(recordentry => recordentry.Data);
    //        var recordEntry = (sender as SfDataGrid).View.Records;
            var provider = (sender as SfDataGrid).View.GetPropertyAccessProvider();
            var records = (sender as SfDataGrid).View.Records;
            //Gets the value for frozen rows count of corresponding column and removes it from FilterElement collection.
            double valorCxC = 0;
            double valorCxCAnt = 0;
            double valorCxP = 0;
            double valorCxPAnt = 0;
            double saldoCxC = 0;
            double saldoCxCAnt = 0;
            double saldoCxP = 0;
            double saldoCxPAnt = 0;

            for (int i = 0; i < (sender as SfDataGrid).View.Records.Count; i++)
            {
                int tipapli = Convert.ToInt32(provider.GetValue(records[i].Data, "tip_apli").ToString());
                if (tipapli == 3)
                {
                    valorCxC += Convert.ToDouble(provider.GetValue(records[i].Data, "valor").ToString());
                    saldoCxC += Convert.ToDouble(provider.GetValue(records[i].Data, "saldo").ToString());
                    //                    valordoc += Convert.ToDouble(provider.GetValue(records[i].Data, "valor").ToString());
                    //                    saldodoc += Convert.ToDouble(provider.GetValue(records[i].Data, "saldo").ToString());
                }
                if (tipapli == 4)
                {
                    valorCxCAnt += Convert.ToDouble(provider.GetValue(records[i].Data, "valor").ToString());
                    saldoCxCAnt += Convert.ToDouble(provider.GetValue(records[i].Data, "saldo").ToString());
                    //                    valordoc += Convert.ToDouble(provider.GetValue(records[i].Data, "valor").ToString());
                    //                    saldodoc += Convert.ToDouble(provider.GetValue(records[i].Data, "saldo").ToString());
                }

            }
            //double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=3").ToString(), out valorCxC);
            //double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=4").ToString(), out valorCxCAnt);
            //double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=1").ToString(), out valorCxP);
            //double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=2").ToString(), out valorCxPAnt);
            //double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=3").ToString(), out saldoCxC);
            //double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=4").ToString(), out saldoCxCAnt);
            //double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=1").ToString(), out saldoCxP);
            //double.TryParse(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=2").ToString(), out saldoCxPAnt);


            //double valorA = Convert.ToDouble(((DataSet)slowTask.Result).Tables[0].Compute("Sum(valor)", "tip_apli=1 or tip_apli=4").ToString());
            //double saldo = Convert.ToDouble(((DataSet)slowTask.Result).Tables[0].Compute("Sum(saldo)", "tip_apli=2 or tip_apli=3").ToString());
            TextCxC.Text = valorCxC.ToString("C");
            TextCxCAnt.Text = valorCxCAnt.ToString("C");
            TextCxCAbono.Text = (valorCxC - saldoCxC).ToString("C");
            TextCxCAntAbono.Text = (valorCxCAnt - saldoCxCAnt).ToString("C");
            TextCxCSaldo.Text = saldoCxC.ToString("C");
            TextCxCAntSaldo.Text = saldoCxCAnt.ToString("C");
            TotalCxc.Text = (valorCxC - valorCxCAnt - valorCxP + valorCxPAnt).ToString("C");
            TotalAbono.Text = ((valorCxC - saldoCxC) - (valorCxCAnt - saldoCxCAnt)).ToString("C");
            TotalSaldo.Text = (saldoCxC - saldoCxCAnt - saldoCxP + saldoCxPAnt).ToString("C");



            //TextTotalDoc.Text = (valordoc-valordocA).ToString("C");
            //TextSaldo.Text = (saldodoc-saldodocA).ToString("C");
        }

        private void BtnRCaja_Click(object sender, RoutedEventArgs e)
        {
            SiaWin.ValReturn = null;
            DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
            if (row == null)
            {
                MessageBox.Show("Registro sin datos");
                return;
            }
            string cod_cli = row[0].ToString();
            string cod_cta = row[2].ToString();
            if (string.IsNullOrEmpty(cod_cli)) return;
            //MessageBox.Show(cod_cli + "-" + cod_cta);

            SiaWin.ValReturn = cod_cli;
            Window ww = SiaWin.WindowExt(9299, "RecibosDeCaja");  //carga desde sql
            
            ww.ShowInTaskbar = false;
            ww.Owner = Application.Current.MainWindow;
            ww.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ww.ShowDialog();
            ww = null;
            //Application.Current.MainWindow.Effect = null;
            //string valorr = ((Inicio)Application.Current.MainWindow).ValReturn;
            //if (valorr != null) MessageBox.Show(valorr.ToString());

        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            tabitem.Cerrar(0);


        }
    }
}

