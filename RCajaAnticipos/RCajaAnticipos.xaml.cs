using Syncfusion.XlsIO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Syncfusion.UI.Xaml.Grid.Converter;
using Microsoft.Win32;
using System.IO;
using RCajaAnticipos;

namespace SiasoftAppExt
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class RCajaAnticipos: Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string codbod = "";
        string codpvta = "";
        string nompvta = "";
        string codcco = "";
        string cnEmp = "";
        DataSet ds = new DataSet();
        DataTable dtVen = new DataTable();
        DataTable dtBan = new DataTable();
        //double valorCxC = 0;

        public RCajaAnticipos()
        {
            InitializeComponent();
            TextFecha.Text = DateTime.Now.ToString();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            codpvta = SiaWin._UserTag;
            LoadInfo();
            ActivaDesactivaControles(0);
            this.DataContext = this;
            FechaIni.Text = DateTime.Now.ToShortDateString();
            FechaFin.Text = DateTime.Now.ToShortDateString();
            BtbGrabar.Focus();

        }
        public void LoadInfo()
        {
            try
            {
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                //        System.Windows.Threading.Dispatcher.BeginInvoke(new A
                //Img.Source= AppDomain.CurrentDomain.BaseDirectory + "Imagenes\\" + idLogo.ToString() + "..png";
                //        ConfigCSource.PathImgBusiness=AppDomain.CurrentDomain.BaseDirectory + "Imagenes\\"+idLogo.ToString()+"..png";
                //        ConfigCSource.nomBuss = ((Inicio)Application.Current.MainWindow).Func.cmp("business","BusinessId","BusinessName",idEmp,0);
                TxtEmpresa.Text = SiaWin._BusinessName.ToString().Trim();
                TxtPVenta.Text = codpvta;
                TxtUser.Text = SiaWin._UserAlias;
                //        _usercontrol.Seg.Auditor(0,_usercontrol.ProjectId,idUser,_usercontrol.GroupId,idEmp,_usercontrol.ModuleId,_usercontrol.AccesoId,0,"Ingreso a: Punto de venta"+" - " +_titulo,"");
                if (codpvta == string.Empty)
                {
                    //_usercontrol.Opacity = 0.5;
                    MessageBox.Show("El usuario no tiene asignado un punto de venta, Pantalla Bloqueada");
                    //_usercontrol.IsEnabled=false;
                }
                else
                {
                    nompvta = SiaWin.Func.cmpCodigo("copventas", "cod_pvt", "nom_pvt", codpvta, idemp);
                    TxtPVenta.Text = codpvta + "-" + nompvta;
                    codbod = SiaWin.Func.cmpCodigo("copventas", "cod_pvt", "cod_bod", codpvta, idemp);
                    codcco = SiaWin.Func.cmpCodigo("copventas", "cod_pvt", "cod_cco", codpvta, idemp);
                    if (string.IsNullOrEmpty(codbod))
                    {
                        //_usercontrol.Opacity = 0.5;
                        MessageBox.Show("El punto de venta Asignado no tiene bodega , Pantalla Bloqueada");
                        //usercontrol.IsEnabled=false;
                    }
                    TxtBod.Text = codbod;
                }
                dtVen = SiaWin.Func.SqlDT("select cod_mer as cod_ven,cod_mer+'-'+nom_mer as nom_ven from inmae_mer where estado=1  order by cod_mer", "inmae_mer", idemp);
                dtVen.PrimaryKey = new DataColumn[] { dtVen.Columns["cod_mer"] };
                // establecer paths
                CmbVen.ItemsSource = dtVen.DefaultView;
                CmbVen.DisplayMemberPath = "nom_ven";
                CmbVen.SelectedValuePath = "cod_ven";
                //LlenaCombo(CmbBodDestino, dtComboBodDestino, "cod_bod", "nom_bod");
                //CmbBodOrigen.SelectedValue = codbod;
                dtBan = SiaWin.Func.SqlDT("select cod_ban,cod_ban+'-'+nom_ban as nom_ban,cod_cta from comae_ban  order by cod_ban", "comae_ban", idemp);
                dtBan.PrimaryKey = new DataColumn[] { dtBan.Columns["cod_ban"] };
                // establecer paths
                CmbBan.ItemsSource = dtBan.DefaultView;
                CmbBan.DisplayMemberPath = "nom_ban";
                CmbBan.SelectedValuePath = "cod_ban";
                //LlenaCombo(CmbBodDestino, dtComboBodDestino, "cod_bod", "nom_bod");
                //CmbBodOrigen.SelectedValue = codbod;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //if (dtCue.Rows.Count > 0) e.Cancel = true;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (BtbGrabar.Content.ToString().Trim() == "Nuevo") return;
            if (e.Key == Key.F5 && BtbGrabar.Content.ToString().Trim() == "Grabar")
            {
                if (e.Key == System.Windows.Input.Key.F5)
                {
                    BtbGrabar.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    return;
                }
            }

            if (e.Key == Key.Escape)
            {
                if (BtbGrabar.Content.ToString().Trim() == "Grabar")
                {
                    BtbCancelar.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    e.Handled = false;
                    return;
                }
            }
        }
        private void TextCodeCliente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                return;
            }
            if ((e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab))
            {
                TextBox textbox = ((TextBox)sender);
                if (textbox.Name == "TextCodeCliente" && !string.IsNullOrEmpty(textbox.Text.Trim()) && !ActualizaCampos(textbox.Text.Trim()))
                {
                    if (MessageBox.Show("El codigo de cliente no existe, Usted desea crear el codigo:" + textbox.Text.Trim() + " .....?", "Siasoft", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        SiaWin.Func.Var.Add("P9311codter", textbox.Text.Trim());
                        SiaWin.Tab(9231);
                        if (!ActualizaCampos(textbox.Text.Trim()))
                        {
                            MessageBox.Show("No se creo el codigo :" + textbox.Text.Trim());
                            textbox.Dispatcher.BeginInvoke((Action)(() => { textbox.Focus(); }));
                            textbox.Focus();
                            e.Handled = true;
                            return;
                        }
                    }
                    else
                    {
                        textbox.Dispatcher.BeginInvoke((Action)(() => { textbox.Focus(); }));
                        textbox.Focus();
                        e.Handled = true;
                        return;
                    }
                }



                TextBox s = e.Source as TextBox;
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                }

            }
        }
        private void TextCodeCliente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BtbCancelar.Content.ToString().Trim() == "Salir") return;
            TextBox textbox = ((TextBox)sender);
            
            //TextBox textbox = ((TextBox)sender);
            if (textbox.Text.Trim() == "")
            {
                int idr = 0; string code = ""; string nombre = "";
                dynamic xx = SiaWin.WindowBuscar("comae_ter", "cod_ter", "nom_ter", "nom_ter", "idrow", "Maestra de clientes", cnEmp, false, "");
                xx.ShowInTaskbar = false;
                xx.Owner = Application.Current.MainWindow;
                xx.ShowDialog();
                idr = xx.IdRowReturn;
                code = xx.Codigo;
                nombre = xx.Nombre;
                xx = null;
                if (idr > 0)
                {
                    TextCodeCliente.Text = code;
                    TextNomCliente.Text = nombre;
                }
                if (string.IsNullOrEmpty(code)) e.Handled = false;

                //ConsultaSaldoCartera();
                if (!string.IsNullOrEmpty(TextCodeCliente.Text.Trim())) TextCodeCliente.Focusable = false;
                //ActualizaCampos(ConfigCSource.cod_ter.ToString());
            }
            else
            {
                if (!ActualizaCampos(textbox.Text.Trim()))
                {
                    MessageBox.Show("El codigo de tercereo:" + textbox.Text.Trim() + " no existe");
                    textbox.Text = "";
                }
                else
                {
                    //ConsultaSaldoCartera();
                    if (!string.IsNullOrEmpty(TextCodeCliente.Text.Trim())) TextCodeCliente.Focusable = false;
                }
            }
            if (TextCodeCliente.Text.Trim().Length == 0)
            {
                textbox.Dispatcher.BeginInvoke((Action)(() => { textbox.Focus(); }));
                //e.Handled = true;
                return;
            }
        }
        private void CmbTipoDoc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ComboBox cs = e.Source as ComboBox;
                if (cs != null)
                {
                    if (cs.SelectedIndex >= 0) cs.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                base.OnPreviewKeyDown(e);
            }
        }
        private bool ActualizaCampos(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id)) return false;
                SqlDataReader dr = SiaWin.Func.SqlDR("SELECT idrow,cod_ter,nom_ter,dir1,tel1,observ FROM comae_ter where cod_ter='" + Id.ToString() + "' or idrow=" + Id.ToString(), idemp);
                int idrow = 0;
                string codter = "";
                string nomter = "";
                while (dr.Read())
                {
                    idrow = Convert.ToInt32(dr["idrow"]);
                    codter = dr["cod_ter"].ToString().Trim();
                    nomter = dr["nom_ter"].ToString().Trim();
                    TextNomCliente.Text = nomter;
                }
                dr.Close();
                if (idrow == 0) return false;
                if (idrow > 0) return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.Exception _error)
            {
                MessageBox.Show(_error.Message);
            }
            return false;
        }
        private void ExportaXLS_Click(object sender, RoutedEventArgs e)
        {
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = dataGridSF.ExportToExcel(dataGridSF.View, options);
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
                if (MessageBox.Show("Usted quiere abrir el archivo en excel?", "Ver archvo", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
        }
        private void ImprimirDoc(int idregcab, string tipoImp)
        {
            string[] strArrayParam = new string[] { idregcab.ToString(), idemp.ToString(), tipoImp };
            SiaWin.Tab(9291, strArrayParam);
            //((Inicio)Application.Current.MainWindow).Tab(9279);832005853
            //if(usercontrol.Tag.ToString()=="-1")
            //{
            // ((Inicio)Application.Current.MainWindow).Tab(9279);
            //MessageBox.Show("ddd");
            //   e.Handled = true;
            // return;
            //}
        }
        private void BtbCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (BtbCancelar.Content.ToString() == "Cancelar")
            {
                if (TextCodeCliente.Text!="")
                {
                    if (MessageBox.Show("Usted desea cancelar este documento..?", "Cancelar Recibo de Caja", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    {
                        e.Handled = true;
                        return;
                    }
                }
                ActivaDesactivaControles(0);
                BtbGrabar.Focus();
                e.Handled = true;
                return;
            }
            else
            {
                this.Close();
            }
        }
        public void ActivaDesactivaControles(int estado)
        {
            if (estado == 0)
            {
                TextCodeCliente.Text = "";
                TextNomCliente.Text = "";
                TextCheque.Text = "";
                TextNota.Text = "";
                TextNumeroDoc.Text = "";
                CmbVen.SelectedIndex = -1;
                CmbBan.SelectedIndex = -1;
                TextNota.IsEnabled = false;
                CmbVen.IsEnabled = false;
                CmbBan.IsEnabled = false;
                TextCodeCliente.IsEnabled = false;
                TextCheque.IsEnabled = false;
                TextValorAnticipo.IsEnabled = false;
                BtbGrabar.Content = "Nuevo";
                BtbCancelar.Content = "Salir";
                TextCodeCliente.Focusable = true;
                TextCodeCliente.Focus();
                TextBono.Text = "";
                CmbValorAnticipo.SelectedIndex = -1;
                TextValorAnticipo.Text = "0";
                TextBono.Visibility = Visibility.Collapsed;
                CmbValorAnticipo.Visibility = Visibility.Collapsed;
                LabolNoBono.Visibility = Visibility.Collapsed;
                LabolVlrBono.Visibility = Visibility.Collapsed;
                TextValorAnticipo.Focusable = true;
                CmbTipoDoc.SelectedIndex = -1;
            }
            if (estado == 1) //creando
            {
                
                TextCodeCliente.Text = "";
                TextNomCliente.Text = "";
                TextCheque.Text = "";
                TextNota.Text = "Anticipo";
                TextNumeroDoc.Text = "";
                CmbVen.SelectedIndex = -1;
                CmbVen.SelectedIndex = -1;
                CmbBan.IsEnabled = true;
                CmbVen.IsEnabled = true;
                TextCodeCliente.IsEnabled = true;
                TextCheque.IsEnabled = true;

                TextNota.IsEnabled = true;
                TextValorAnticipo.IsEnabled = true;
                BtbGrabar.Content = "Grabar";
                BtbCancelar.Content = "Cancelar";
                TextCodeCliente.IsEnabled = true;
                TextNumeroDoc.Text = SiaWin.Func.ConsecutivoPv(codpvta, 0, 10, idemp);
                TextCodeCliente.Focusable = true;
                TextCodeCliente.Focus();
                TextBono.Text = "";
                CmbValorAnticipo.SelectedIndex = -1;
                TextValorAnticipo.Text = "0";
                CmbTipoDoc.SelectedIndex = -1;
                TextBono.Visibility = Visibility.Collapsed;
                CmbValorAnticipo.Visibility = Visibility.Collapsed;
                LabolNoBono.Visibility = Visibility.Collapsed;
                LabolVlrBono.Visibility = Visibility.Collapsed;
                TextValorAnticipo.Focusable = true;

            }
        }
        private void BtbGrabar_Click(object sender, RoutedEventArgs e)
        {

            if (BtbGrabar.Content.ToString() == "Nuevo")
            {
                ActivaDesactivaControles(1);
            }
            else
            {
                if (string.IsNullOrEmpty(cnEmp))
                {
                    MessageBox.Show("Error - Cadena de Conexion nulla");
                    return;
                }
                string _CodeCliente = TextCodeCliente.Text;
                if (string.IsNullOrEmpty(_CodeCliente))
                {
                    MessageBox.Show("Falta Nit/cc del cliente..");
                    TextCodeCliente.Focus();
                    return;
                }
                if (CmbBan.SelectedIndex < 0)
                {
                    MessageBox.Show("Seleccione una codigo de Banco.....");
                    CmbBan.Focus();
                    return;
                }
                String ctaban = dtBan.Rows[CmbBan.SelectedIndex]["cod_cta"].ToString();
                if (CmbVen.SelectedIndex < 0)
                {
                    MessageBox.Show("Seleccione Vendedor.....");
                    CmbVen.Focus();
                    return;
                }
                double _abono = Convert.ToDouble(TextValorAnticipo.Value);
                //MessageBox.Show(_abono.ToString("C"));
                if (_abono < 0)
                {
                    MessageBox.Show("Valor Abono no puede ser menor o igual a  0");
                    return;
                }
                if (MessageBox.Show("Usted desea guardar el documento..?", "Guardar Recibo de Caja", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        int iddocumento = 0;
                        string ctaant = "280505";
                        //if (!ValidaSaldosDoc()) return;  //Valida que los documentos no fueron cancelados por otro usuario
                        iddocumento = ExecuteSqlTransaction(_CodeCliente,ctaant, ctaban, _abono);
                        //MessageBox.Show("iddocumet:" + iddocumento.ToString());
                        if (iddocumento < 0) return;
                        ImprimirDoc(iddocumento, "Impresion Original");
                        //MessageBox.Show("Documento Guardado:" + iddocumento.ToString());
                        ActivaDesactivaControles(0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    //dataGrid.Focus();
                }
            }
        }
        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox s = e.Source as TextBox;
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                }
            }
        }
        private void TextCheque_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab))
            {
                TextBox s = e.Source as TextBox;
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    //s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    //dataGrid.Focus();
                    //dataGrid.SelectedIndex = 0;
                    //dataGrid.CurrentCell = new DataGridCellInfo(dataGrid.Items[0], dataGrid.Columns[8]);
                    e.Handled = true;
                }
            }
        }
        private void CmbTipoDoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selec = CmbTipoDoc.SelectedIndex;
            int tipodoc = -1;
            if (selec == -1) TextNumeroDoc.Text = "";
            if (selec == 0) tipodoc = 7;
            if (selec == 1) tipodoc = 8;

            if (selec == 1)
            {
                TextBono.Visibility = Visibility.Visible;
                CmbValorAnticipo.Visibility = Visibility.Visible;
                LabolNoBono.Visibility = Visibility.Visible;
                LabolVlrBono.Visibility = Visibility.Visible;
                TextValorAnticipo.Text = "0";
                TextBono.Text = "";
                CmbValorAnticipo.SelectedIndex = -1;
                TextValorAnticipo.Focusable = false;
            }
            if (selec == 0)
            {
                TextBono.Visibility = Visibility.Collapsed;
                CmbValorAnticipo.Visibility = Visibility.Collapsed;
                LabolNoBono.Visibility = Visibility.Collapsed;
                LabolVlrBono.Visibility = Visibility.Collapsed;
                TextValorAnticipo.Text = "0";
                CmbValorAnticipo.SelectedIndex = -1;
                TextBono.Text = "";
                TextValorAnticipo.Focusable = true;
                
            }
            //if (selec != 2) BtnCargarEntradas.Visibility = Visibility.Hidden;
            //CmbBodOrigen.SelectedIndex = -1;
            //CmbBodDestino.SelectedIndex = -1;
            TextNumeroDoc.Text = SiaWin.Func.ConsecutivoPv(codpvta, 0, tipodoc, idemp);
        }

        private void ReImprimir_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = ds.Tables["RCAnticipos"].Rows[dataGridSF.SelectedIndex];
            if (dr != null)
            {
                string numtrn = dr["idreg"].ToString();
                ImprimirDoc(Convert.ToInt32(numtrn), "Reimpreso");
            }
        }
        private void Ejecutar_Click(object sender, RoutedEventArgs e)
        {
            // validar fecha
            LoadData();
        }
        private void CmbValorAnticipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbValorAnticipo.SelectedIndex < 0) return;
            ComboBoxItem cbi = (ComboBoxItem)CmbValorAnticipo.SelectedItem;
            string valselectedText = cbi.Content.ToString();
            //if (CmbValorAnticipo.SelectedIndex < 0) return;
            //string jj = CmbValorAnticipo.SelectedItem.ToString().Trim();
            //MessageBox.Show("1-"+jj);

            if (string.IsNullOrEmpty(valselectedText)) return;
            double xvalor = Convert.ToDouble(valselectedText);
            //MessageBox.Show("2-" + jj);

            //TextValorAnticipo.Text = xvalor.ToString("C");
            TextValorAnticipo.Value = Convert.ToDecimal(xvalor);
            //MessageBox.Show("3-" + jj);
        }
        private int ExecuteSqlTransaction(string codter,string ctaant, string cta, double abonoBco)
        {
            if(abonoBco<=0)
            {
                MessageBox.Show("Anticipo en 0 " + abonoBco.ToString());
                return -1;
            }
            if (string.IsNullOrEmpty(cnEmp))
            {
                MessageBox.Show("Error - Cadena de Conexion nulla");
                return -1;
            }
            string TipoConsecutivo = "rcaja";
            string codtrn = "01";
            using (SqlConnection connection = new SqlConnection(cnEmp))
            {
                connection.Open();
                StringBuilder errorMessages = new StringBuilder();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("Transaction");
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    string sqlConsecutivo = @"declare @fecdoc as datetime;set @fecdoc = getdate();declare @ini as char(4);declare @num as varchar(12);declare @iConsecutivo char(12) = '' ;declare @iFolioHost int = 0;UPDATE COpventas SET " + TipoConsecutivo + " = ISNULL(" + TipoConsecutivo + ", 0) + 1  WHERE cod_pvt='" + codpvta + "';SELECT @iFolioHost = " + TipoConsecutivo + ",@ini=rtrim(inicial) FROM Copventas  WHERE cod_pvt='" + codpvta + "';set @num=@iFolioHost;select @iConsecutivo=rtrim(@ini)+REPLICATE ('0',12-len(rtrim(@ini))-len(rtrim(convert(varchar,@num))))+rtrim(convert(varchar,@num));";
                    string sqlcab = sqlConsecutivo + @"INSERT INTO cocab_doc (cod_trn,fec_trn,num_trn,detalle,cod_ven) values ('" + codtrn + "',@fecdoc,@iConsecutivo,'" + TextNota.Text.Trim() + "','" + CmbVen.SelectedValue + "');DECLARE @NewID INT;SELECT @NewID = SCOPE_IDENTITY();";
                    string sql = "";
                    sql = sql + @"INSERT INTO cocue_doc (idregcab,cod_trn,num_trn,cod_cta,cod_cco,cod_ter,des_mov,cre_mov,doc_mov) values (@NewID,'" + codtrn + "',@iConsecutivo,'" + ctaant.ToString() + "','" +codcco + "','" + codter + "','Anticipo'" +  "," + abonoBco.ToString("F", CultureInfo.InvariantCulture) + ",'"+TextBono.Text.Trim()+"');";
                    string sqlban = "";
                        sqlban = @"INSERT INTO cocue_doc (idregcab,cod_trn,num_trn,cod_cta,cod_cco,cod_ter,des_mov,num_chq,deb_mov,doc_mov) values (@NewID,'" + codtrn + "',@iConsecutivo,'" + cta + "','" + codcco.ToString() + "','" + codter + "','Anticipo:" + TextNomCliente.Text.Trim() + "','" + TextCheque.Text.Trim() + "'," + abonoBco.ToString("F", CultureInfo.InvariantCulture) + ",'"+TextBono.Text.Trim()+"');";
                    command.CommandText = sqlcab + sql + sqlban + @"select CAST(@NewId AS int);";
                    MessageBox.Show(command.CommandText.ToString());
                    var r = new object();
                    r = command.ExecuteScalar();
                    transaction.Commit();
                    connection.Close();
                    return Convert.ToInt32(r.ToString());
                }
                catch (SqlException ex)
                {
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append(" SQL-Index #" + i + "\n" + "Message: " + ex.Errors[i].Message + "\n" + "LineNumber: " + ex.Errors[i].LineNumber + "\n" + "Source: " + ex.Errors[i].Source + "\n" + "Procedure: " + ex.Errors[i].Procedure + "\n");
                    }
                    transaction.Rollback();
                    MessageBox.Show(errorMessages.ToString());
                    return -1;
                }
                catch (Exception ex)
                {
                    errorMessages.Append("c Error:#" + ex.Message.ToString());
                    transaction.Rollback();
                    MessageBox.Show(errorMessages.ToString());
                    return -1;
                }
            }
        }
        ////// CONSULTA DE TRASLADOS
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(cnEmp))
            {
                connection.Open();
                //connectionString.Open();
                //DataSet ds = new DataSet();
                StringBuilder _sql = new StringBuilder();
                ds.Clear();
                //_sql.Append("select cab.cod_trn,cab.num_trn,cab.fec_trn,cab.bod_tra,cab.bod_tra+'-'+bod.ini_bod as bod_dest,cue.cod_bod,cue.cod_ref,rtrim(ref.nom_ref) as nom_ref,rtrim(tip.nom_tip) as nom_tip,iif(trn.tip_trn=1,cue.cantidad,-cue.cantidad) as cantidad,trn.tip_trn,iif(cab.tip_traslado=0,'Tienda',iif(cab.tip_traslado=1,'GerenteProducto',iif(cab.tip_traslado=2,'GerenteAdmon','Ninguno'))) as tipotraslado,cab.idreg from incue_doc as cue ");
                // _sql.Append(" inner join incab_doc as cab on cab.idreg = cue.idregcab inner join inmae_ref as ref on ref.cod_ref = cue.cod_ref inner join inmae_bod as bod on bod.cod_bod = cab.bod_tra ");
                // _sql.Append(" inner join inmae_trn as trn on trn.cod_trn=cab.cod_trn inner join inmae_tip as tip on tip.cod_tip =ref.cod_tip where convert(date,cab.fec_trn) between '" + FechaIni.Text + "' and '" + FechaFin.Text + "' and (cab.cod_trn = '051' or cab.cod_trn = '141')");
                //_sql.Append(" and cue.cod_bod = '" + codbod.Trim() + "' order by cab.fec_trn ");
                _sql.Append("select cab.cod_trn,cab.num_trn,cab.fec_trn,cue.cod_cco,cco.alias,cab.cod_ven,cab.detalle,cue.cod_cta,cue.cod_ter,rtrim(ter.nom_ter) as nom_ter,doc_cruc,deb_mov + cre_mov as valor,cab.idreg,");
                _sql.Append("CASE cta.tip_apli WHEN 3 THEN 'CxC'  ELSE 'CxCAnt' END as tipo,cta.tip_apli ");
                _sql.Append(" from cocue_doc as cue  inner join cocab_doc as cab on cab.idreg = cue.idregcab and cab.cod_trn = '01 ' ");
                _sql.Append("inner join comae_cta as cta on cta.cod_cta = cue.cod_cta and cta.tip_apli=4 ");
                _sql.Append("inner join comae_ter as ter on ter.cod_ter = cue.cod_ter inner join comae_cco as cco on cco.cod_cco = cue.cod_cco ");
                _sql.Append("inner join comae_trn as trn on trn.cod_trn = cab.cod_trn  where convert(date,cab.fec_trn) between '" + FechaIni.Text + "' and '" + FechaFin.Text + "' ");
                _sql.Append(" and cue.cod_cco = '" + codcco.Trim() + "' order by cab.fec_trn,cod_trn,num_trn ");
                SqlDataAdapter adapter = new SqlDataAdapter(_sql.ToString(), connection);
                adapter.Fill(ds, "RCaja");
                dataGridSF.ItemsSource = ds.Tables["RCaja"];
                double totcxc = 0;
                double totant = 0;
                double.TryParse(ds.Tables["RCaja"].Compute("Sum(valor)", "tip_apli=3").ToString(), out totcxc);
                double.TryParse(ds.Tables["RCaja"].Compute("Sum(valor)", "tip_apli=4").ToString(), out totant);
                TextTotalCxC.Text = totcxc.ToString("C");
                TextTotalAnticipos.Text = totant.ToString("C");
            }
        }

    }
}
