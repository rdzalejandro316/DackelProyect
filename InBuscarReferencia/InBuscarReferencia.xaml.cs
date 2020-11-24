using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace SiasoftAppExt
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    /// 
    public partial class InBuscarReferencia : Window
    {
        dynamic SiaWin;
        string cmptabla; string cmpcodigo; string cmpnombre; string cmporden; string cmpIdRow;bool mostrartodo; string where;
        DataTable dt = new DataTable();
        private bool TiboBusqueda = true; //false= codigo,true=nombre


        private string codigo;
        private string nombre;
        private int idrowreturn;
        private int idemp;
        private string idbod;
        public string UltBusqueda="";
        public  string Conexion;
        public  DataSet ds1 = new DataSet() ;
        public int IdRowReturn
        {
            set { idrowreturn = value; }
            get { return idrowreturn; }
        }
        public string Codigo
        {
            set { codigo = value; }
            get { return codigo; }
        }
        public string Nombre
        {
            set { nombre = value; }
            get { return nombre; }
        }
        public string CmpTabla = "inmae_ref";
        public string CmpCodigo = "cod_ref";
        public string CmpNombre = "nom_ref";
        public string CmpOrden = "nom_ref";
        public string CmpIdRow = "idrow";
        public string CmpTitulo = "Maestra de Referencias";
        public bool MostrarTodo = false;
        public string Where = "";
        public int idEmp = -1;
        public string idBod = "";
        public string ultbusqueda = "";
        public InBuscarReferencia()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idEmp = SiaWin._BusinessId;
            cmptabla = CmpTabla;
            cmpcodigo = CmpCodigo;
            cmpnombre = CmpNombre;
            cmporden = CmpOrden;
            cmpIdRow = CmpIdRow;

            mostrartodo = MostrarTodo;
            where = Where;
            idemp = idEmp;
            idbod = idBod;
            this.Title = CmpTitulo;
            TxtTipoBusqueda.Text = "Busqueda por Nombre";
            //dataGrid.PreviewKeyDown += new KeyEventHandler(mainGrid_PreviewKeyDown);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dt.Clear();
            try
            {
                if (TxtShear.Text.Trim() == string.Empty) TxtShear.Focus();
                if (TxtShear.Text.Trim() == string.Empty) return;
                string bb = TxtShear.Text.Trim();
                dataGrid.ItemsSource = null;
                string www = string.Empty;
                if (TiboBusqueda)  www = TxtConvertText(bb);
                if (!TiboBusqueda) www = "  substring(cod_ref,1,13)=substring('" + bb + "',1,13) ";
                dt = GetDataTable(" where " + www);
                foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
                //dv = new DataView(dt);
                //        dv.Sort = "nom_ref ASC,cod_ant ASC";
                //dv.Sort = "nombre ASC";
                dataGrid.ItemsSource = dt.DefaultView; ;

                //dataGrid.ItemsSource = GetDataTable(" where " + www).DefaultView;
                if (dataGrid.Items.Count == 0) return;
                //dataGrid.SelectedItem = dataGrid.Items[1];
                dataGrid.Focus();
                //dataGrid.SelectedIndex = 0;

                var uiElement = e.OriginalSource as UIElement;
                dataGrid.SelectedItem = dataGrid.Items[0];
                uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                dataGrid.CurrentCell = new DataGridCellInfo(dataGrid.Items[dataGrid.SelectedIndex], dataGrid.Columns[0]);
                dataGrid.CommitEdit();
                dataGrid.UpdateLayout();
                dataGrid.SelectedIndex = dataGrid.SelectedIndex;
                //foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
                //dataGrid.ScrollIntoView(dataGrid.SelectedItem, dataGrid.Columns[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public DataTable GetDataTable(string _where)
        {
            try
            {
                string sql = "select top 500 "  + cmpnombre + " as nombre," + cmpcodigo + " as codigo,val_ref,00000000.00 as saldo  from " + cmptabla + _where + " order by " + cmpnombre;
                SqlConnection conn1 = new SqlConnection(Conexion);
                SqlCommand cmd1 = new SqlCommand(sql, conn1);
                conn1.Open();
                SqlDataReader dr = cmd1.ExecuteReader();
                dt.Load(dr);
                TxtTotal.Content = "Total registros :" + dt.Rows.Count;
                conn1.Close();
            }
            catch (SqlException SQLex)
            {
                MessageBox.Show("Error:" + SQLex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            return dt;
        }
        private string TxtConvertText(string txt)
        {
            string s = txt;
            // Split string on spaces.
            int inicount = 0;
            string cadena = "";
            string[] words = s.Split(' ');
            foreach (string word in words)
            {
                if (inicount == 0)
                {
                    cadena = "rtrim(" + cmporden + ") like '%" + word + "%'";
                }
                else
                {
                    cadena = cadena + " and rtrim(" + cmporden + ") like '%" + word + "%'";
                }
                inicount = inicount + 1;
            }
            return cadena ;
        }
        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectItem();
            e.Handled = true;
        }
        private void SelectItem()
        {
            DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
            if (row != null)
            {
                //int nPnt = Int32.Parse(row[0].ToString());
                this.Codigo = row[1].ToString();
                this.Nombre = row[0].ToString();
                //this.IdRowReturn = nPnt;
                UltBusqueda = TxtShear.Text;
            }
            else
            {
                this.IdRowReturn = -1;
            }
            this.Close();
        }
        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                SelectItem();
                e.Handled = true;
            }
            if (e.Key == Key.Left)
            {
                if (mostrartodo == false)
                {
                    TxtShear.Focus();
                    e.Handled = true;
                }
            }
            if (e.Key == Key.F2)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                if (row != null)
                {
                   // int nPnt = Int32.Parse(row[0].ToString());
                    this.Codigo = row[1].ToString();
                    this.Nombre = row[0].ToString();
                    //this.IdRowReturn = nPnt;
                    /// valida si hay saldos en bodega
                    DataSet ds1 = LoadData(codigo, idBod);
                    if(ds1.Tables[0].Rows.Count==0)
                    {
                        MessageBox.Show("Producto:" + codigo + "-" + Nombre.Trim() + " Sin saldos en bodegas..");
                        return;
                    }
                    //MessageBox.Show(ds1.Tables[0].Rows.Count.ToString());
                    SaldosBodegas xx = new SaldosBodegas(this.Codigo, this.Nombre, 0, Conexion, idbod, idemp);
                    double sum = 0;
                    foreach (System.Data.DataColumn col in ds1.Tables[0].Columns) col.ReadOnly = false;
                    foreach (DataRow dr in ds1.Tables[0].Rows) // search whole table
                    {
                        string idbodx = dr["cod_bod"].ToString();
                        double saldoin = Convert.ToDouble(dr["saldo"]);
                        //dr["saldo"] = saldoin; //change the name
                        sum = sum + saldoin;
                    }
                    xx.TotalCnd.Text = sum.ToString("N2");


                    xx.dataGrid.ItemsSource = ds1.Tables[0].DefaultView;
                    xx.ShowInTaskbar = false;
                    xx.Owner = Application.Current.MainWindow;

                    xx.dataGrid.Focus();
                    //dataGrid.SelectedIndex = 0;
                    xx.dataGrid.SelectedItem = dataGrid.Items[0];
                    xx.dataGrid.SelectedIndex = 0;
                    xx.dataGrid.Focus();
                    xx.dataGrid.SelectedIndex = 0;
                    xx.ShowDialog();
                    e.Handled = true;
                }
            }
        }
        private void TxtShear_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnBuscar.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                if (dataGrid.Items.Count == 0) return;
                dataGrid.Focus();
                var uiElement = e.OriginalSource as UIElement;
                dataGrid.SelectedItem = dataGrid.Items[0];
                uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                dataGrid.CurrentCell = new DataGridCellInfo(dataGrid.Items[dataGrid.SelectedIndex], dataGrid.Columns[0]);
                dataGrid.CommitEdit();
                dataGrid.SelectedIndex = dataGrid.SelectedIndex;
                e.Handled = true;
            }
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dt.Rows.Count == 0) return;
            DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
            if (row != null)
            {
                //int nReturn = Int32.Parse(row[0].ToString());
                //if (nReturn < 0) return;
                string codref = row[1].ToString();
                double saldoin = SiaWin.Func.SaldoInv(codref, idbod, idEmp);
                SaldoInv.Text = saldoin.ToString();
                DataRowView DRV = (DataRowView)dataGrid.SelectedItem;
                DataRow DR = DRV.Row;
                DR.BeginEdit();
                DR["saldo"] = saldoin;
                DR.EndEdit();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            idbod = idBod;
            ultbusqueda = UltBusqueda;
            if (TiboBusqueda) TxtTipoBusqueda.Text = "Busqueda por Nombre";
            if (TiboBusqueda==false) TxtTipoBusqueda.Text = "Busqueda por Codigo";
            if (MostrarTodo == true)
            {
                if (where != string.Empty)
                {
                    where = " where " + where;
                }
                dataGrid.ItemsSource = GetDataTable(where).DefaultView;
                BtnBuscar.Visibility = Visibility.Collapsed;
                TxtShear.Visibility = Visibility.Collapsed;
                dataGrid.SelectedIndex = 0;
                dataGrid.Focus();
            }
            else
            {
                if (ultbusqueda != string.Empty) TxtShear.Text = ultbusqueda;
                TxtShear.Focus();
            }
        }
        private DataSet LoadData(string refe, string bod)
        {
            try
            {
                ds1.Clear();
                SqlConnection con = new SqlConnection(Conexion);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                //DataSet ds1 = new DataSet();
                //cmd = new SqlCommand("ConsultaCxcCxpAll", con);
                cmd = new SqlCommand("SaldosInventariosPorReferenciaBodegas", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ref", refe);//if you have parameters.
                cmd.Parameters.AddWithValue("@Bod", "");//if you have parameters.
                //cmd.Parameters.AddWithValue("@Where", where);//if you have parameters.
                da = new SqlDataAdapter(cmd);
                da.Fill(ds1);
                con.Close();
                return ds1;
                //VentasPorProducto.ItemsSource = ds.Tables[0];
                //VentaPorBodega.ItemsSource = ds.Tables[1];
                //VentasPorCliente.ItemsSource = ds.Tables[2];
            }
            catch (SqlException SQLex)
            {
                MessageBox.Show("Error SQL:" + SQLex.Message);

            }
            catch (Exception e)
            {
                MessageBox.Show("Error App:"+e.Message);
            }
            return null;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.F8)
            {
                if (TiboBusqueda)
                {
                    TiboBusqueda = false;
                    TxtTipoBusqueda.Text = "Busqueda por Codigo";
                }
                else
                {
                    TiboBusqueda = true;
                    TxtTipoBusqueda.Text = "Busqueda por Nombre";
                }
            }
        }
    }
}
