using GeneracionDeCodigosRef;
using Syncfusion.UI.Xaml.Grid.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace SiasoftAppExt
{

    //Sia.PublicarPnt(9485,"GeneracionDeCodigosRef");
    //dynamic WinDescto = ((Inicio)Application.Current.MainWindow).WindowExt(9485,"GeneracionDeCodigosRef");
    //WinDescto.ShowInTaskbar = false;
    //WinDescto.Owner = Application.Current.MainWindow;
    //WinDescto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //WinDescto.ShowDialog();    


    public partial class GeneracionDeCodigosRef : Window
    {


        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        List<string> lista;
        int incre = 0;
        Boolean check = false;

        GrupoTallas venta = new GrupoTallas();
        DataTable Grilla = new DataTable();


        string[] referencia = new string[6];
        string[] nombreReferencia = new string[5];


        public GeneracionDeCodigosRef()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            LoadConfig();
            cargarGRID();
        }

        private void LoadConfig()
        {
            try
            {
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                //Boolean a = Convert.ToBoolean(foundRow["BusinessId"]);
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "aquiio");
            }
        }

        public void cargarGRID()
        {
            Grilla.Columns.Add("cod_ref");
            Grilla.Columns.Add("nom_ref");
            Grilla.Columns.Add("cantidad1");
            Grilla.Columns.Add("tip_ref");
            Grilla.Columns.Add("cod_tip");
            Grilla.Columns.Add("cod_gru");
            Grilla.Columns.Add("cod_sgr");
            Grilla.Columns.Add("cod_col");
            Grilla.Columns.Add("cod_tall");
            Grilla.Columns.Add("im");
            Grilla.Columns.Add("serial");
            Grilla.Columns.Add("sexo");
            Grilla.Columns.Add("estado");
            Grilla.Columns.Add("cod_tiva");
            Grilla.Columns.Add("tipo_prv");
            Grilla.Columns.Add("fec_crea");
            Grilla.Columns.Add("cod_med");
            Grilla.Columns.Add("ind_ped");
            Grilla.Columns.Add("Ind_iva");
            Grilla.Columns.Add("fecha_aded");
            Grilla.Columns.Add("precio_us");
            Grilla.Columns.Add("cost_bas");
            Grilla.Columns.Add("desc_tall");
            Grilla.Columns.Add("cod_imp");

            dataGridCxC.ItemsSource = Grilla.DefaultView;
        }

        public void insertarGRID(string cod_ref, string nom_ref, string cantidad1, string cod_tip, string cod_gru, string cod_sgr, string cod_col, string cod_tall, string im, string serial, string sexo, string precio_us, string cost_bas, string desc_tall)
        {
            string mes = DateTime.Now.ToString("MMM");
            string año = DateTime.Now.ToString("yy");
            string mesaño = año;

            Grilla.Rows.Add(cod_ref, nom_ref, cantidad1, "1", cod_tip, cod_gru, cod_sgr, cod_col, cod_tall, im, serial, sexo, "1", "A", "2", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), mesaño, "0", "1", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), precio_us, cost_bas, desc_tall, im);

        }

        private void BTNelimReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                row.Delete();
            }
            catch (Exception)
            {

                MessageBox.Show("seleccione una celda para eliminar");
            }

        }

        private void BTNcurva_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (camposLLenos() == false) return;

                for (int i = 0; i < lista.Count; i++)
                {
                    //if (ValidarExistencia(TXBoxCod_ref.Text) == false)
                    //{
                    //    MessageBox.Show("la referencia ya existe en el sistema");
                    //    return;
                    //}

                    decimal valorDolar = Convert.ToDecimal(TX_precio.Text.ToString());
                    decimal valorBase = Convert.ToDecimal(TX_Dolar.Text.ToString());
                    decimal precioUS = valorDolar * valorBase;

                    insertarGRID(TXBoxCod_ref.Text, TXBoxNom_Ref.Text, ListCantidad.Value.ToString(), LB_tip.Text.Trim(), LB_gru.Text.Trim(), LB_sgr.Text.Trim(), LB_col.Text.Trim(), TX_Cod.Text.Trim(),
                        NumeroImp.Text.Trim(), TXserial.Text.Trim(), TextBxCB_sexo.Text.Trim(), precioUS.ToString().Trim(), valorDolar.ToString(), TX_talla.Text.Trim());

                    if (incre < lista.Count - 1) siguienteTalla();
                }

                reinicirTalla();
            }
            catch (Exception w)
            {
                MessageBox.Show("no se pudo crear curva:" + w);
            }


        }

        public void reinicirTalla()
        {
            try
            {
                incre = 0;
                TX_talla.Text = lista[incre].ToString();
                cargarCodigoTalla(lista[incre].ToString());
            }
            catch (Exception) { MessageBox.Show("error al reiniciar tallas"); }
        }

        private void ValidacionNumeros(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract || e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("este campo solo admite valores numericos");
                e.Handled = true;
            }
        }

        public string CargarSexo(string codigo_tip)
        {
            try
            {
                string cadena = "select sexo from InMae_tip where cod_tip='" + codigo_tip + "' ";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "sexo", idemp);
                return dt.Rows[0]["sexo"].ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("error al buscar el sexo de la linea");
                throw;
            }
        }

        private void TexBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text == "") return;

                string tag = ((TextBox)sender).Tag.ToString();
                string texto = ((TextBox)sender).Text;
                string codigo = "";
                string nombre = "";
                TextBox campoNombre = new TextBox();

                switch (tag)
                {
                    case "inmae_tip":
                        codigo = "cod_tip";
                        nombre = "nom_tip";
                        campoNombre = (TextBox)this.FindName("TX_tip");
                        break;
                    case "inmae_gru":
                        codigo = "cod_gru";
                        nombre = "nom_gru";
                        campoNombre = (TextBox)this.FindName("TX_gru");
                        break;
                    case "inmae_sgr":
                        codigo = "cod_sgr";
                        nombre = "nom_sgr";
                        campoNombre = (TextBox)this.FindName("TX_sgr");
                        break;
                    case "inmae_col":
                        codigo = "cod_col";
                        nombre = "nom_col";
                        campoNombre = (TextBox)this.FindName("TX_col");
                        break;
                }

                string buscar = "select * from " + tag + " where  " + codigo + "='" + texto + "'  ";
                DataTable dt = SiaWin.Func.SqlDT(buscar, "BUscardor", idemp);
                if (dt.Rows.Count > 0)
                {
                    ((TextBox)sender).Text = dt.Rows[0][codigo].ToString().Trim();
                    campoNombre.Text = dt.Rows[0][nombre].ToString().Trim();

                    if (tag == "inmae_tip") TextBxCB_sexo.Text = CargarSexo(dt.Rows[0][codigo].ToString().Trim());

                    formarReferencia(dt.Rows[0][codigo].ToString().Trim(), tag);
                    formarNombreReferencia(dt.Rows[0][nombre].ToString().Trim(), tag);
                }
                else
                {
                    MessageBox.Show("el codigo ingresado no existe");
                    ((TextBox)sender).Text = "";
                    campoNombre.Text = "";
                }

                //MessageBox.Show("buscar:" + buscar);

            }
            catch (Exception w)
            {
                MessageBox.Show("error al salir del texbox:" + w);
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                string tag = ((TextBox)sender).Tag.ToString();
                if (string.IsNullOrEmpty(tag)) return;

                if (e.Key == Key.F8 || e.Key == Key.Enter || tag == "grupo_tallas")
                {
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = false; string cmpwhere = "";

                    if (tag == "inmae_tip")
                    {
                        cmptabla = tag; cmpcodigo = "cod_tip"; cmpnombre = "nom_tip"; cmporden = "nom_tip"; cmpidrow = "idrow"; cmptitulo = "Maestra de Marca"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "";
                    }
                    if (tag == "inmae_gru")
                    {
                        cmptabla = tag; cmpcodigo = "cod_gru"; cmpnombre = "nom_gru"; cmporden = "nom_gru"; cmpidrow = "idrow"; cmptitulo = "Maestra de Grupos"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "";
                    }
                    if (tag == "inmae_sgr")
                    {
                        cmptabla = tag; cmpcodigo = "cod_sgr"; cmpnombre = "nom_sgr"; cmporden = "nom_sgr"; cmpidrow = "idrow"; cmptitulo = "Maestra de Sub Grupo"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "";
                    }
                    if (tag == "inmae_col")
                    {
                        cmptabla = tag; cmpcodigo = "cod_col"; cmpnombre = "nom_col"; cmporden = "nom_col"; cmpidrow = "idrow"; cmptitulo = "Maestra de Color"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "";
                    }
                    if (tag == "grupo_tallas")
                    {
                        cmptabla = tag; cmpcodigo = "idrow"; cmpnombre = "grupo_talla"; cmporden = "grupo_talla"; cmpidrow = "idrow"; cmptitulo = "Maestra de Color"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }

                    int idr = 0; string code = ""; string nom = "";
                    dynamic winb = SiaWin.WindowBuscar(cmptabla, cmpcodigo, cmpnombre, cmporden, cmpidrow, cmptitulo, cnEmp, mostrartodo, cmpwhere);
                    winb.ShowInTaskbar = false;
                    winb.Owner = Application.Current.MainWindow;
                    winb.Width = 500;
                    winb.Height = 400;
                    winb.ShowDialog();
                    idr = winb.IdRowReturn;
                    code = winb.Codigo;
                    nom = winb.Nombre;

                    winb = null;
                    if (idr > 0)
                    {
                        if (tag == "inmae_tip")
                        {
                            LB_tip.Text = code.Trim(); TX_tip.Text = nom.Trim();
                            formarReferencia(code.Trim(), "inmae_tip");
                            formarNombreReferencia(nom.Trim(), "inmae_tip");

                            TextBxCB_sexo.Text = CargarSexo(code);
                        }
                        if (tag == "inmae_gru")
                        {
                            LB_gru.Text = code.Trim(); TX_gru.Text = nom.Trim();
                            formarReferencia(code.Trim(), "inmae_gru");
                            formarNombreReferencia(nom.Trim(), "inmae_gru");
                        }
                        if (tag == "inmae_sgr")
                        {
                            LB_sgr.Text = code.Trim(); TX_sgr.Text = nom.Trim();
                            formarReferencia(code.Trim(), "inmae_sgr");
                            formarNombreReferencia(nom.Trim(), "inmae_sgr");
                        }
                        if (tag == "inmae_col")
                        {
                            LB_col.Text = code.Trim(); TX_col.Text = nom.Trim();
                            formarReferencia(code.Trim(), "inmae_col");
                            formarNombreReferencia(nom.Trim(), "inmae_col");
                        }
                        if (tag == "grupo_tallas")
                        {
                            TXB_gruTall.Text = nom.Trim();
                            eliminarLista(check);
                            cargarTallas(nom);
                            check = true;
                        }

                        var uiElement = e.OriginalSource as UIElement;
                        uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        e.Handled = true;
                    }
                    if (e.Key == Key.Enter)
                    {
                        var uiElement = e.OriginalSource as UIElement;
                        uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OpenGrup_Click(object sender, RoutedEventArgs e)
        {
            venta.ShowInTaskbar = false;
            venta.Owner = Application.Current.MainWindow;
            venta.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            venta.Close();
        }

        public void eliminarLista(Boolean check)
        {
            if (check == true)
            {
                incre = 0;
            }
        }

        public void cargarTallas(string tallasList)
        {
            lista = new List<string>(tallasList.Split(','));
            TX_talla.Text = lista.First();
            cargarCodigoTalla(lista.First().ToString());

        }
        public void cargarCodigoTalla(string nom_tall)
        {
            try
            {
                string cadena = "select cod_tall from inmae_tall where desc_tall='" + nom_tall + "' ";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "MaestraTalla", idemp);

                formarReferencia(dt.Rows[0]["cod_tall"].ToString(), "inmae_tall");
                formarNombreReferencia(nom_tall, "inmae_tall");

                TX_Cod.Text = referencia[5];
            }
            catch (Exception w)
            {
                MessageBox.Show("erro al cargar la talla" + w);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            siguienteTalla();
        }

        public void siguienteTalla()
        {
            try
            {
                TX_talla.Text = lista[incre + 1].ToString();
                cargarCodigoTalla(lista[incre + 1].ToString());
                incre += 1;
            }
            catch (Exception) { MessageBox.Show("no hay mas valores siguientes"); }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TX_talla.Text = lista[incre - 1].ToString();
                cargarCodigoTalla(lista[incre - 1].ToString());
                incre -= 1;

            }
            catch (Exception) { MessageBox.Show("no hay mas valores anteriores"); }
        }

        public void formarReferencia(string codigo, string maestra)
        {

            if (maestra == "inmae_tip")
            {
                referencia[0] = codigo;
            }
            if (maestra == "inmae_gru")
            {
                referencia[1] = codigo;
            }
            if (maestra == "inmae_sgr")
            {
                referencia[2] = codigo;
            }
            if (maestra == "importacion")
            {
                referencia[3] = codigo;
            }
            if (maestra == "inmae_col")
            {
                referencia[4] = codigo;
            }
            if (maestra == "inmae_tall")
            {
                referencia[5] = codigo;
            }

            string result = string.Join("", referencia);
            string replacement = Regex.Replace(result, @"\s", "");
            TXBoxCod_ref.Text = replacement;

        }

        public void formarNombreReferencia(string nombre, string maestra)
        {
            if (maestra == "inmae_gru")
            {
                nombreReferencia[0] = nombre;
            }
            if (maestra == "inmae_tip")
            {
                nombreReferencia[1] = nombre;
            }
            if (maestra == "inmae_sgr")
            {
                nombreReferencia[2] = nombre;
            }
            if (maestra == "inmae_col")
            {
                nombreReferencia[3] = nombre;
            }
            if (maestra == "inmae_tall")
            {
                nombreReferencia[4] = "T-" + nombre;
            }
            string result = string.Join(",", nombreReferencia);
            //string replacement = Regex.Replace(result, @"\s", "");
            TXBoxNom_Ref.Text = result.Replace(",", " ");

        }

        private void TX_impor_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            formarReferencia(NumeroImp.Text, "importacion");
        }

        //actualizar nombre subgrupo de nom_ref
        private void TX_sgr_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            formarNombreReferencia(TX_sgr.Text, "inmae_sgr");
        }

        //actualizar campo de color de nom_ref
        private void TX_col_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            formarNombreReferencia(TX_col.Text, "inmae_col");
        }

        private void TXserial_LostFocus(object sender, RoutedEventArgs e)
        {

            if (TXserial.Text == "") return;
            if (buscarSerial(TXserial.Text) == false)
            {
                Serial ventana = new Serial
                {
                    serialExt = TXserial.Text
                };
                ventana.ShowDialog();

            }
        }

        public Boolean buscarSerial(string serial)
        {
            Boolean bandera = false;

            try
            {
                string cadena = "select * from InMae_ref where serial='" + serial + "' ";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "validar", idemp);

                if (dt.Rows.Count == 0)
                {
                    bandera = true;
                }
                else
                {
                    bandera = false;
                }

            }
            catch (Exception w)
            {
                MessageBox.Show("error al cargar serial:" + w);
            }

            return bandera;
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (camposLLenos() == false) return;


                //if (ValidarExistencia(TXBoxCod_ref.Text) == false)
                //{
                //    MessageBox.Show("la referencia ya existe en el sistema");
                //    return;
                //}

                decimal valorDolar = Convert.ToDecimal(TX_precio.Text.ToString());
                decimal valorBase = Convert.ToDecimal(TX_Dolar.Text.ToString());
                decimal precioUS = valorDolar * valorBase;

                insertarGRID(TXBoxCod_ref.Text, TXBoxNom_Ref.Text, ListCantidad.Value.ToString(), LB_tip.Text.Trim(), LB_gru.Text.Trim(), LB_sgr.Text.Trim(), LB_col.Text.Trim(), TX_Cod.Text.Trim(),
                    NumeroImp.Text.Trim(), TXserial.Text.Trim(), TextBxCB_sexo.Text.Trim(), precioUS.ToString().Trim(), valorDolar.ToString(), TX_talla.Text.Trim());


                siguienteTalla();
            }
            catch (Exception w)
            {
                MessageBox.Show("error:" + w);
            }

        }

        public bool camposLLenos()
        {
            bool bandera = true;
            if (TXB_gruTall.Text.Length <= 0 || TXB_gruTall.Text == "Buscar" || TXB_gruTall.Text == "")
            {
                MessageBox.Show("ingrese el grupo de tallas para insertar a las referencias");
                return false;
            }
            if (NumeroImp.Text.Length <= 0 || NumeroImp.Text == "")
            {
                MessageBox.Show("ingrese el numero de importacion");
                return false;
            }
            if (LB_tip.Text.Length <= 0 || LB_tip.Text == "")
            {
                MessageBox.Show("ingrese una marca");
                return false;
            }
            if (LB_gru.Text.Length <= 0 || LB_gru.Text == "")
            {
                MessageBox.Show("ingrese un grupo");
                return false;
            }
            if (TXserial.Text.Length <= 0 || TXserial.Text == "")
            {
                MessageBox.Show("ingrese un serial");
                return false;
            }
            if (LB_sgr.Text.Length <= 0 || LB_sgr.Text == "")
            {
                MessageBox.Show("ingrese un subgrupo");
                return false;
            }
            if (LB_col.Text.Length <= 0 || LB_col.Text == "")
            {
                MessageBox.Show("ingrese un color");
                return false;
            }
            if (TX_precio.Text.Length <= 0 || TX_precio.Text == "")
            {
                MessageBox.Show("Ingrese un costo base");
                return false;
            }
            if (TX_Dolar.Text.Length <= 0 || TX_Dolar.Text == "")
            {
                MessageBox.Show("Ingrese valor de dolar");
                return false;
            }

            return bandera;
        }

        public Boolean ValidarExistencia(string cod_ref)
        {
            Boolean bandera = false;

            try
            {
                string cadena = "select cod_ref from InMae_ref where cod_ref = '" + cod_ref + "' ";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "validar", idemp);

                if (dt.Rows.Count == 0)
                {
                    bandera = true;
                }
                else
                {
                    bandera = false;
                }

            }
            catch (Exception) { MessageBox.Show("error al verificar existencais"); }


            return bandera;
        }

        private void BTNgenerarDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Grilla.Rows.Count <= 0)
                {
                    MessageBox.Show("Ingrese una referencia para generar");
                    return;
                }

                foreach (System.Data.DataRow row in Grilla.Rows)
                {
                    string cod_ref = row["cod_ref"].ToString();
                    if (ValidarExistencia(cod_ref) == false) MessageBox.Show("la referencia :" + cod_ref + " ya existe en la maestra de referencias");
                    else
                    {

                        string cadena = "";
                        //string cod_ref = row["cod_ref"].ToString();
                        string nom_ref = row["nom_ref"].ToString();
                        decimal cantidad1 = Convert.ToDecimal(row["cantidad1"]);
                        Int16 tip_ref = Convert.ToInt16(row["tip_ref"]);
                        string cod_tip = row["cod_tip"].ToString();
                        string cod_gru = row["cod_gru"].ToString();
                        string cod_sgr = row["cod_sgr"].ToString();
                        string cod_col = row["cod_col"].ToString();
                        string cod_tall = row["cod_tall"].ToString();
                        string im = row["im"].ToString();
                        string serial = row["serial"].ToString();
                        string sexo = row["sexo"].ToString();
                        decimal estado = Convert.ToDecimal(row["estado"]); ;
                        string cod_tiva = row["cod_tiva"].ToString();
                        Int16 tipo_prv = Convert.ToInt16(row["tipo_prv"]);
                        string fec_crea = row["fec_crea"].ToString();
                        string cod_med = row["cod_med"].ToString();
                        decimal ind_ped = Convert.ToDecimal(row["ind_ped"]);
                        decimal Ind_iva = Convert.ToDecimal(row["Ind_iva"]);
                        string fecha_aded = row["fec_crea"].ToString();
                        decimal precio_us = Convert.ToDecimal(row["precio_us"]);
                        decimal cost_bas = Convert.ToDecimal(row["cost_bas"]);
                        string desc_tall = row["desc_tall"].ToString();
                        string cod_imp = row["cod_imp"].ToString();

                        cadena += "insert into InMae_ref (cod_ref, nom_ref, cantidad1, tip_ref, cod_tip, cod_gru, cod_sgr, cod_col, cod_tall, im, serial, sexo, estado, cod_tiva, tipo_prv, fec_crea, cod_med, ind_ped, Ind_iva, fecha_aded, precio_us, cost_bas, desc_tall, cod_imp) values ('" + cod_ref + "', '" + nom_ref + "', " + cantidad1 + ", " + tip_ref + ", '" + cod_tip + "','" + cod_gru + "','" + cod_sgr + "', '" + cod_col + "', '" + cod_tall + "', '" + im + "', '" + serial + "', '" + sexo + "', " + estado + ",'" + cod_tiva + "', " + tipo_prv + ", '" + fec_crea + "','" + cod_med + "', " + ind_ped + ", " + Ind_iva + ", '" + fecha_aded + "', " + precio_us + ", " + cost_bas + ", '" + desc_tall + "', '" + cod_imp + "');";

                        if (SiaWin.Func.SqlCRUD(cadena, idemp) == false) { MessageBox.Show("error al insertart referencia"); }
                    }
                }

                generarDocumento();
            }
            catch (Exception w)
            {
                MessageBox.Show("error al insertar datos:" + w);
            }
        }



        public void generarDocumento()
        {
            try
            {
                //bool bandera = true;
                if (MessageBox.Show("Usted desea guardar el documento..?", "Guardar Traslado", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    string codtrn = "001";
                    string TipoConsecutivo = "num_act";
                    string cod_bod = "01";
                    DateTime fechaActual = DateTime.Today;
                    //MessageBox.Show("cnEmp:" + cnEmp);

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


                        string sqlConsecutivo = @"declare @fecdoc as datetime;
                        set @fecdoc = getdate();
                        declare @ini as char(4);
                        declare @num as varchar(12);
                        declare @iConsecutivo char(12) = '' ;
                        declare @iFolioHost int = 0;
                        " + "SELECT @iFolioHost = " + TipoConsecutivo + ",@ini=rtrim(Inicial) FROM InMae_trn  WHERE cod_trn='" + codtrn + "';" +
                        "set @num=@iFolioHost;" +
                        "select @iConsecutivo=rtrim(@ini)+REPLICATE ('0',12-len(rtrim(@ini))-len(rtrim(convert(varchar,@num))))+rtrim(convert(varchar,@num));";


                        string sqlcab = sqlConsecutivo + @"INSERT INTO InCab_doc (ano_doc,per_doc,cod_trn,num_trn,fec_trn,cod_prv,suc_rem,fecha_aded,im)
                        values ('" + DateTime.Now.Year.ToString() + "','" + fechaActual.ToString("MM") + "','" + codtrn + "',@iConsecutivo,@fecdoc,'1','1',@fecdoc,'" + NumeroImp.Text + "');DECLARE @NewID INT;SELECT @NewID = SCOPE_IDENTITY();";

                        string sqlcue = "";
                        var reflector = this.dataGridCxC.View.GetPropertyAccessProvider();
                        int a = 1;
                        foreach (var row in dataGridCxC.View.Records)
                        {
                            foreach (var column in dataGridCxC.Columns)
                            {

                                if (column.MappingName == "cantidad1")
                                {
                                    var rowData = dataGridCxC.GetRecordAtRowIndex(a);
                                    var cantidad = reflector.GetValue(rowData, "cantidad1");
                                    var referencias = reflector.GetValue(rowData, "cod_ref");
                                    decimal cost_uni = Convert.ToDecimal(reflector.GetValue(rowData, "cost_bas"));
                                    decimal subtotal = cost_uni * Convert.ToDecimal(cantidad);//reflector.GetValue(rowData, "subt_ped").ToString();

                                    if (cantidad.ToString() != "0.00" && cantidad.ToString() != "0")
                                    {
                                        bool valCuer = BuscarReferenciaEnCompra(referencias.ToString());

                                        if (valCuer == false)
                                        {
                                            var documento = CompraYaGenerada(Grilla);
                                            //string valor = CompraYaGenerada(Grilla);
                                            string num_trn = documento.Item1 == string.Empty ? "@iConsecutivo" : "'" + documento.Item1 + "'";
                                            string idcabeza = documento.Item2 == 0 ? "@NewID" : "'" + documento.Item2 + "'";

                                            //string fecha = @"declare @fecdoc as datetime;set @fecdoc = getdate(); ";
                                            sqlcue = sqlcue + @"INSERT INTO InCue_doc (idregcab,cod_trn,num_trn,cod_ref,cod_bod,cantidad,cos_uni,cos_tot,fecha_aded) values ("+ idcabeza + ",'" + codtrn + "'," + num_trn + ",'" + referencias + "','" + cod_bod + "'," + cantidad + "," + cost_uni.ToString("F", CultureInfo.InvariantCulture) + "," + subtotal.ToString("F", CultureInfo.InvariantCulture) + ",getdate());";


                                        }


                                    }

                                    break;
                                }
                            }
                            a = a + 1;
                        }

                        string actualzaConsecu = "UPDATE InMae_trn SET " + TipoConsecutivo + " = ISNULL(" + TipoConsecutivo + ", 0) + 1  where cod_trn='001';";


                        if (!string.IsNullOrEmpty(sqlcue))
                        {
                            var documento = CompraYaGenerada(Grilla);
                            //string num_trn = CompraYaGenerada(Grilla);
                            if (string.IsNullOrEmpty(documento.Item1))
                            {
                                command.CommandText = sqlcab + sqlcue + actualzaConsecu + @"select CAST(@NewId AS int);";
                                MessageBox.Show(command.CommandText.ToString());
                                var r = new object();
                                r = command.ExecuteScalar();
                                transaction.Commit();
                                connection.Close();
                                MessageBox.Show("documento generado");
                            }
                            else
                            {
                                command.CommandText = sqlcue;
                                MessageBox.Show(command.CommandText.ToString());
                                var r = new object();
                                r = command.ExecuteScalar();
                                transaction.Commit();
                                connection.Close();
                                MessageBox.Show("se inserto la referencia faltante en el cuerpo de la compra de esta importacion");
                            }

                        }
                        else
                        {
                            MessageBox.Show("las referencias que se encuetran en lista ya esta generadas en un documento de compra ingrese en la pantalla de serial para poder modificar sus items");
                        }

                    }

                }
                else
                {
                    MessageBox.Show("No Genero El Documento");
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("error en el documento:" + w);
            }
        }


        public bool BuscarReferenciaEnCompra(string referencia)
        {
            bool flag = false;
            DataTable dt = SiaWin.Func.SqlDT("select * from InCue_doc where cod_trn='001' and cod_ref='" + referencia + "'", "tabla", idemp);
            if (dt.Rows.Count > 0) flag = true;
            return flag;
        }

        public Tuple<string, int> CompraYaGenerada(DataTable grilla)
        {
            string num_trn = string.Empty;
            int idcabeza = 0;
            foreach (DataRow item in grilla.Rows)
            {
                DataTable dt = SiaWin.Func.SqlDT("select * from InCue_doc where cod_trn='001' and cod_ref='" + item["cod_ref"] + "'", "tabla", idemp);
                if (dt.Rows.Count > 0)
                {
                    num_trn = dt.Rows[0]["num_trn"].ToString().Trim();
                    idcabeza = Convert.ToInt32(dt.Rows[0]["idregcab"]);
                }
            }

            var tuple = new Tuple<string, int>(num_trn, idcabeza);
            return tuple;

        }

        private void BTNchangeValue_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name.ToString().Trim();
            UpdateValues ventana = new UpdateValues();
            string valuepass = "";
            switch (name)
            {
                case "BTNval_ref":
                    valuepass = "val_ref";
                    break;
                case "BTNprecio_us":
                    valuepass = "precio_us";
                    break;
            }

            ventana.value = valuepass;
            ventana.ShowInTaskbar = false;
            ventana.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ventana.Owner = Application.Current.MainWindow;
            ventana.ShowDialog();

        }
   




    }

}




