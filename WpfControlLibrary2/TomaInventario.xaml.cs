using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TomaInventario;

namespace SiasoftAppExt
{
    //Sia.PublicarPnt(9502,"TomaInventario");
    //dynamic WinDescto = ((Inicio)Application.Current.MainWindow).WindowExt(9502,"TomaInventario");
    //WinDescto.bodega= "14";
    //WinDescto.ShowInTaskbar = false;
    //WinDescto.Owner = Application.Current.MainWindow;
    //WinDescto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //WinDescto.ShowDialog();   

    public partial class TomaInventario : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        public string bodega = "";

        DataTable Grilla = new DataTable();
        DataTable GrillaRefInexistente = new DataTable();

        DataTable consolidarDT = new DataTable();

        int contadorCort = 0;
        public TomaInventario()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            LoadConfig(SiaWin._UserId.ToString());
            cargarGRID();
            loadName();
        }

        private void LoadConfig(string usuario)
        {
            try
            {
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "aquiio");
            }
        }

        public void loadName()
        {
            SqlConnection con = new SqlConnection(SiaWin._cn);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable ds = new DataTable();
            string query = "select * from Seg_User where UserId=" + SiaWin._UserId + "";
            cmd = new SqlCommand(query, con);
            cmd.CommandType = System.Data.CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            if (ds.Rows.Count > 0)
            {
                TX_usuario.Text = ds.Rows[0]["UserName"].ToString();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TX_bodega.Text = bodega;
            BodNameCon.Text = SiaWin.Func.cmpCodigo("InMae_bod", "cod_bod", "nom_bod", bodega, idemp);
            NameBodega.Text = BodNameCon.Text;
            BodNameConInfo.Text = BodNameCon.Text;
            corteUpdate();
        }

        public void cargarGRID()
        {
            Grilla.Columns.Add("cod_ref");
            Grilla.Columns.Add("descripcion");
            Grilla.Columns.Add("cantidad", typeof(int));
            Grilla.Columns.Add("saldo", typeof(int));
            Grilla.Columns.Add("check");
            Grilla.TableName = "Grilla";
            dataGridCxC.ItemsSource = Grilla.DefaultView;

            GrillaRefInexistente.Columns.Add("cod_ref");
            GrillaRefInexistente.Columns.Add("cantidad", typeof(int));
            GrillaRefInexistente.TableName = "GrillaRefInexistente";
            dataGridRefInexi.ItemsSource = GrillaRefInexistente.DefaultView;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string referencia = TX_referencia.Text.Trim();

                string query = "select * from inmae_ref where cod_ref ='" + referencia + "' ";
                DataTable dt = SiaWin.Func.SqlDT(query, "Referenica", idemp);

                if (dt.Rows.Count > 0)
                {
                    if (addCantidad(referencia, Grilla) == false)
                        insertarGRID(referencia, "", 1, Grilla);
                }
                else
                {
                    if (addCantidad(referencia, GrillaRefInexistente) == false)
                        insertarGRID(referencia, "", 1, GrillaRefInexistente);
                }

                (sender as TextBox).Text = "";
                (sender as TextBox).Focus();
                e.Handled = true;
                updateTotale();
            }
        }

        void updateTotale()
        {
            TX_total.Text = Grilla.Rows.Count.ToString();

        }

        public void insertarGRID(string cod_ref, string descripcion, int cantidad, DataTable dt)
        {
            if (string.IsNullOrEmpty(cod_ref)) return;

            double saldoin = SiaWin.Func.SaldoInv(cod_ref, bodega, idemp);
            if (dt.TableName == "Grilla")
                Grilla.Rows.Add(cod_ref, descripcion, cantidad, saldoin, true);
            else
                GrillaRefInexistente.Rows.Add(cod_ref, cantidad);
        }


        public bool addCantidad(string referencia, DataTable gr)
        {
            bool flag = false;
            foreach (DataRow item in gr.Rows)
            {
                if (item["cod_ref"].ToString().Trim() == referencia)
                {
                    int cantidad = Convert.ToInt32(item["cantidad"]);
                    item["cantidad"] = cantidad + 1;
                    flag = true;
                }
            }
            actualizaCntCorte(true);
            return flag;
        }

        private void BtnConsolidar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Grilla.Rows.Count > 0)
                {
                    if (MessageBox.Show("Usted desea generar el corte?", "Corte", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        string query = "";
                        int corte = cort();
                        foreach (DataRow item in Grilla.Rows)
                        {
                            string cod_ref = item["cod_ref"].ToString().Trim();
                            string descripcion = item["descripcion"].ToString().Trim();
                            int cantidad = Convert.ToInt32(item["cantidad"]);
                            int saldo = Convert.ToInt32(item["saldo"]);
                            string idusuario = SiaWin._UserId.ToString();
                            string bodega = TX_bodega.Text.Trim();
                            if (Convert.ToBoolean(item["check"]) == true)
                                query += "insert into TomaInventario (cod_ref,descripcion,cantidad,id_usurio,bodega,saldo,corte,fec_ins) values " +
                                    "('" + cod_ref + "','" + descripcion + "'," + cantidad + ",'" + idusuario + "','" + bodega + "'," + saldo + "," + corte + ",GETDATE())";
                        }

                        if (query.Length > 0)
                            if (SiaWin.Func.SqlCRUD(query, idemp) == true)
                            {
                                MessageBox.Show("corte #" + corte + " insertado exitosamente");
                                actualizaCntCorte(false);
                                corteUpdate();
                            }
                    }
                }
                else
                {
                    MessageBox.Show("no ninguna referencia para consolidar");
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("error al consolidar:" + w);
            }

        }

        public void actualizaCntCorte(bool aum)
        {
            if (aum == true)
                contadorCort++;
            else
                contadorCort = 0;

            TX_ContadorCort.Text = contadorCort.ToString();
        }

        public void corteUpdate()
        {
            TX_corte.Text = cort().ToString();
            Grilla.Clear();
        }


        public int cort()
        {
            string query = "select ISNULL(MAX(corte)+1, 1) as corte from TomaInventario where id_usurio='" + SiaWin._UserId.ToString() + "' and bodega='" + TX_bodega.Text.Trim() + "'";
            DataTable dt = SiaWin.Func.SqlDT(query, "inventario", idemp);
            return Convert.ToInt32(dt.Rows[0]["corte"]);
        }



        private void Btneliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCxC.SelectedIndex >= 0)
            {
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                int cantidad = Convert.ToInt32(row["cantidad"]);
                contadorCort = contadorCort - cantidad;
                TX_ContadorCort.Text = contadorCort.ToString();
                row.Delete();
                updateTotale();
            }
        }

        private async void BtnConsulta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                sfBusyIndicatorConsulta.IsBusy = true;
                GridConsolidado.IsEnabled = false;
                string bodega = TX_bodega.Text.Trim();

                var slowTask = Task<DataSet>.Factory.StartNew(() => LoadDataConsulta(bodega, source.Token), source.Token);
                await slowTask;

                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    dataGridconsulta.ItemsSource = ((DataSet)slowTask.Result).Tables[0];
                }

                GridConsolidado.IsEnabled = true;
                this.sfBusyIndicatorConsulta.IsBusy = false;
            }
            catch (Exception w)
            {
                MessageBox.Show("error en la consulta:" + w);
            }
        }

        private DataSet LoadDataConsulta(string bodega, CancellationToken cancellationToken)
        {
            DataSet ds = new DataSet();
            string query = "select Lecollezioni_emp010.dbo.TomaInventario.idrow,Lecollezioni_emp010.dbo.TomaInventario.cod_ref,Lecollezioni_emp010.dbo.TomaInventario.fec_ins,Lecollezioni_emp010.dbo.TomaInventario.corte,";
            query += "Lecollezioni_emp010.dbo.TomaInventario.descripcion,Lecollezioni_emp010.dbo.TomaInventario.cantidad,";
            query += "Lecollezioni_emp010.dbo.TomaInventario.id_usurio,Lecollezioni_SiaApp.dbo.Seg_User.UserName,";
            query += "Lecollezioni_emp010.dbo.TomaInventario.bodega,Lecollezioni_emp010.dbo.InMae_Bod.nom_bod ";
            query += "from Lecollezioni_emp010.dbo.TomaInventario ";
            query += "inner join Lecollezioni_emp010.dbo.inmae_ref on Lecollezioni_emp010.dbo.inmae_ref.cod_ref = Lecollezioni_emp010.dbo.TomaInventario.cod_ref ";
            query += "inner join Lecollezioni_emp010.dbo.InMae_Bod on Lecollezioni_emp010.dbo.InMae_Bod.cod_bod = Lecollezioni_emp010.dbo.TomaInventario.bodega ";
            query += "inner join Lecollezioni_SiaApp.dbo.Seg_User on Lecollezioni_SiaApp.dbo.Seg_User.UserId = Lecollezioni_emp010.dbo.TomaInventario.id_usurio ";
            query += "where Lecollezioni_emp010.dbo.TomaInventario.bodega='" + bodega + "' ";
            DataTable dt = SiaWin.Func.SqlDT(query, "inventario", idemp);
            ds.Tables.Add(dt);
            return ds;
        }



        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = dataGridconsulta.ExportToExcel(dataGridconsulta.View, options);
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

        private async void BTNconsolidado_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                dataGridConsolidar.ItemsSource = null;
                consolidarDT = null;

                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                sfBusyIndicator.IsBusy = true;
                GridConsolidado.IsEnabled = false;
                sfBusyIndicator.Header = "cargando los saldo de la bodega " + BodNameCon.Text.Trim() + " con la toma de inventario";
                string bodega = TX_bodega.Text.Trim();

                var slowTask = Task<DataSet>.Factory.StartNew(() => LoadData(bodega, source.Token), source.Token);
                await slowTask;

                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    dataGridConsolidar.ItemsSource = ((DataSet)slowTask.Result).Tables[0];
                    consolidarDT = ((DataSet)slowTask.Result).Tables[0];
                }

                GridConsolidado.IsEnabled = true;
                this.sfBusyIndicator.IsBusy = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errror en el load" + ex);

            }
        }


        private DataSet LoadData(string bodega, CancellationToken cancellationToken)
        {
            SqlConnection con = new SqlConnection(cnEmp);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("TomaDeInventario", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bodega", bodega);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }


        private void BTNdocumento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (consolidarDT.Rows.Count > 0)
                {
                    if (isEntrenda() == true)
                        DocumentoEntrada();

                    if (isSalida() == true)
                        DocumentoSalida();

                    EliminarTemporalInv();
                }
                else
                {
                    MessageBox.Show("debe de traer el consolidado");
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("Hoooooo:" + w);
            }
        }

        public void EliminarTemporalInv()
        {
            if (MessageBox.Show("Usted desea eliminar la temporal de inventario para no ocacionar errores en las tomas de inventario futuras?", "Eliminar Traslado", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string bodega = TX_bodega.Text;
                string query = "delete TomaInventario where bodega='"+bodega+"' ";
                if (SiaWin.Func.SqlCRUD(query, idemp) == true) {
                    MessageBox.Show("eliminacion de la temporal exitoso -bodega:" + bodega);
                    BTNconsolidado.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }
                    
            }
        }

        public bool isEntrenda()
        {
            bool flag = false;
            foreach (DataRow item in consolidarDT.Rows)
            {
                int cantidad = Convert.ToInt32(item["cantidad"]);
                int saldo = Convert.ToInt32(item["saldo"]);
                if (cantidad > saldo) flag = true;
            }
            return flag;
        }

        public bool isSalida()
        {
            bool flag = false;
            foreach (DataRow item in consolidarDT.Rows)
            {
                int cantidad = Convert.ToInt32(item["cantidad"]);
                int saldo = Convert.ToInt32(item["saldo"]);
                if (cantidad < saldo) flag = true;
            }
            return flag;
        }

        public void DocumentoEntrada()
        {
            try
            {
                if (MessageBox.Show("Usted desea generar las entradas pertinentes?", "Generar Documento", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    string codtrn = "050";
                    string TipoConsecutivo = "num_act";
                    DateTime fechaActual = DateTime.Today;
                    string bodega = TX_bodega.Text.Trim();

                    using (SqlConnection connection = new SqlConnection(cnEmp))
                    {
                        connection.Open();
                        StringBuilder errorMessages = new StringBuilder();
                        SqlCommand command = connection.CreateCommand();
                        SqlTransaction transaction;

                        transaction = connection.BeginTransaction("Transaction");
                        command.Connection = connection;
                        command.Transaction = transaction;


                        string sqlConsecutivo = @"declare @fecdoc as datetime;
                        set @fecdoc = getdate();
                        declare @ini as char(4);
                        declare @num as varchar(12);
                        declare @iConsecutivo char(12) = '' ;
                        declare @iFolioHost int = 0;
                        " + "SELECT @iFolioHost = " + TipoConsecutivo + ",@ini=rtrim(Inicial) FROM InMae_trnT  WHERE cod_trn='" + codtrn + "';" +
                        "set @num=@iFolioHost;" +
                        "select @iConsecutivo=rtrim(@ini)+REPLICATE ('0',12-len(rtrim(@ini))-len(rtrim(convert(varchar,@num))))+rtrim(convert(varchar,@num));";



                        string sqlcab = sqlConsecutivo + @"INSERT INTO InCab_doc (ano_doc,per_doc,cod_trn,num_trn,fec_trn,fecha_aded)
                        values ('" + DateTime.Now.Year.ToString() + "','" + fechaActual.ToString("MM") + "','" + codtrn + "',@iConsecutivo,@fecdoc,@fecdoc);DECLARE @NewID INT;SELECT @NewID = SCOPE_IDENTITY();";

                        string sqlcue = "";

                        foreach (DataRow item in consolidarDT.Rows)
                        {
                            int cantidad = Convert.ToInt32(item["cantidad"]);
                            int saldo = Convert.ToInt32(item["saldo"]);


                            if (cantidad > saldo)
                            {
                                string cod_ref = item["cod_ref"].ToString();
                                int cntEnt = cantidad - saldo;
                                sqlcue = sqlcue + @"INSERT INTO InCue_doc(idregcab,cod_trn,num_trn,cod_ref,cod_bod,cantidad,fecha_aded) values
                                (@NewID,'" + codtrn + "',@iConsecutivo,'" + cod_ref + "','" + bodega + "'," + cntEnt + ",getdate());";
                            }
                        }
                        string actualzaConsecu = "UPDATE InMae_trn SET " + TipoConsecutivo + " = ISNULL(" + TipoConsecutivo + ", 0) + 1  where cod_trn='" + codtrn + "';";

                        command.CommandText = sqlcab + sqlcue + actualzaConsecu + @"select CAST(@NewId AS int);";
                        //MessageBox.Show(command.CommandText.ToString());
                        var r = new object();
                        r = command.ExecuteScalar();
                        transaction.Commit();
                        connection.Close();
                        MessageBox.Show("documento de entrada generado");
                    }
                }
                else
                {
                    MessageBox.Show("no se genero el d");
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("Error DocumentoEntrada():" + w);
            }
        }

        public void DocumentoSalida()
        {
            try
            {
                if (MessageBox.Show("Usted desea generar las salidas pertinentes?", "Generar Salida", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    string codtrn = "140";
                    string TipoConsecutivo = "num_act";
                    DateTime fechaActual = DateTime.Today;
                    string bodega = TX_bodega.Text.Trim();

                    using (SqlConnection connection = new SqlConnection(cnEmp))
                    {
                        connection.Open();
                        StringBuilder errorMessages = new StringBuilder();
                        SqlCommand command = connection.CreateCommand();
                        SqlTransaction transaction;

                        transaction = connection.BeginTransaction("Transaction");
                        command.Connection = connection;
                        command.Transaction = transaction;


                        string sqlConsecutivo = @"declare @fecdoc as datetime;
                        set @fecdoc = getdate();
                        declare @ini as char(4);
                        declare @num as varchar(12);
                        declare @iConsecutivo char(12) = '' ;
                        declare @iFolioHost int = 0;
                        " + "SELECT @iFolioHost = " + TipoConsecutivo + ",@ini=rtrim(Inicial) FROM InMae_trn  WHERE cod_trn='" + codtrn + "';" +
                       "set @num=@iFolioHost;" +
                       "select @iConsecutivo=rtrim(@ini)+REPLICATE ('0',12-len(rtrim(@ini))-len(rtrim(convert(varchar,@num))))+rtrim(convert(varchar,@num));";


                        string sqlcab = sqlConsecutivo + @"INSERT INTO InCab_doc (ano_doc,per_doc,cod_trn,num_trn,fec_trn,fecha_aded)
                        values ('" + DateTime.Now.Year.ToString() + "','" + fechaActual.ToString("MM") + "','" + codtrn + "',@iConsecutivo,@fecdoc,@fecdoc);DECLARE @NewID INT;SELECT @NewID = SCOPE_IDENTITY();";

                        string sqlcue = "";

                        foreach (DataRow item in consolidarDT.Rows)
                        {
                            int cantidad = Convert.ToInt32(item["cantidad"]);
                            int saldo = Convert.ToInt32(item["saldo"]);

                            if (cantidad < saldo)
                            {
                                string cod_ref = item["cod_ref"].ToString();
                                int cntSal = saldo - cantidad;
                                sqlcue = sqlcue + @"INSERT INTO InCue_doc(idregcab,cod_trn,num_trn,cod_ref,cod_bod,cantidad,fecha_aded) values
                                (@NewID,'" + codtrn + "',@iConsecutivo,'" + cod_ref + "','" + bodega + "'," + cntSal + ",getdate());";
                            }
                        }
                        string actualzaConsecu = "UPDATE InMae_trn SET " + TipoConsecutivo + " = ISNULL(" + TipoConsecutivo + ", 0) + 1  where cod_trn='" + codtrn + "';";

                        command.CommandText = sqlcab + sqlcue + actualzaConsecu + @"select CAST(@NewId AS int);";
                        //MessageBox.Show(command.CommandText.ToString());
                        var r = new object();
                        r = command.ExecuteScalar();
                        transaction.Commit();
                        connection.Close();
                        MessageBox.Show("documento de salida generado");
                    }
                }
                else
                {
                    MessageBox.Show("no se genero el d");
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("Error DocumentoEntrada():" + w);
            }
        }

        private async void BtnConsultaInforme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                sfBusyIndicatorInforme.IsBusy = true;

                string bodega = TX_bodega.Text.Trim();

                var slowTask = Task<DataSet>.Factory.StartNew(() => LoadData(bodega, source.Token), source.Token);
                await slowTask;

                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    dataGridInforme.ItemsSource = ((DataSet)slowTask.Result).Tables[0];
                }

                sfBusyIndicatorInforme.IsBusy = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errror en el load" + ex);

            }
        }

        private void BtnExportarInforme_Click(object sender, RoutedEventArgs e)
        {
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = dataGridInforme.ExportToExcel(dataGridInforme.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            workBook.Worksheets[0].UsedRange.BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);
            workBook.Worksheets[0].UsedRange.BorderAround(ExcelLineStyle.Thin, ExcelKnownColors.Black);
            workBook.Worksheets[0].AutoFilters.FilterRange = workBook.Worksheets[0].UsedRange;
            workBook.Worksheets[0].Range["A1:H1"].CellStyle.Color = System.Drawing.Color.Black;
            workBook.Worksheets[0].Range["A1:H1"].CellStyle.Font.Color = ExcelKnownColors.White;

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

        private void BtnEliminarCort_Click(object sender, RoutedEventArgs e)
        {
            DeleteCorte ventana = new DeleteCorte();
            ventana.bodega = TX_bodega.Text;
            ventana.usuario = SiaWin._UserId.ToString();

            ventana.UserTX.Text = TX_usuario.Text.Trim();
            ventana.BodeTX.Text = BodNameCon.Text.Trim();


            ventana.ShowInTaskbar = false;
            ventana.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ventana.Owner = Application.Current.MainWindow;
            ventana.ShowDialog();
            corteUpdate();
        }





    }
}
