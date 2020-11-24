using System;
using System.Collections.Generic;
using System.Data;
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

namespace AsignacionCampa
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Clientes : Window
    {


        dynamic SiaWin;        
        int idemp = 0;
        string cnEmp = "";

        public string cod_camp;
        public string nom_camp;

        public Clientes()
        {
            InitializeComponent();
            
            SiaWin = Application.Current.MainWindow;            
            idemp = SiaWin._BusinessId;

            this.MaxHeight = 650;
            this.MinHeight = 650;
            this.MinWidth = 1200;
            this.MaxWidth = 1200;

            LoadConfig();
            //GridCliente();

          
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

                fecha_ini.Text = DateTime.Now.AddMonths(-1).ToString();
                fecha_fin.Text = DateTime.Now.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        public void GridCliente()
        {
            try
            {
                string fe_fin = fecha_fin.Text + " 23:59:59";

                string cadena = "select cliente.cod_ter,cliente.nom_ter as nom_ter,cliente.tel1,cliente.email,linea.cod_tip as cod_tip,linea.nom_tip as nom_tip, sum( iif(cabeza.cod_trn between '004' and '005',CAST(cuerpo.cantidad as int),CAST(-cuerpo.cantidad as int) ) ) as cantidad_linea, sum( iif(cabeza.cod_trn between '004' and '005',cuerpo.subtotal,-cuerpo.subtotal) ) as total_Linea, vendedor.nom_mer as nom_mer,max(iif(cabeza.cod_trn='005',CONVERT(varchar,fec_trn,103),'')) as ultfecha, max(iif(cabeza.cod_trn='005',cuerpo.cod_bod,'')) as bodega,max(bod.nom_bod) as nom_bod ";
                cadena = cadena + "from InCab_doc as cabeza ";
                cadena = cadena + "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
                cadena = cadena + "inner join inmae_bod as bod on bod.cod_bod=cuerpo.cod_bod ";
                cadena = cadena + "inner join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
                cadena = cadena + "inner join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";
                cadena = cadena + "inner join comae_ter as cliente on cliente.cod_ter = cabeza.cod_cli ";
                cadena = cadena + "full join InMae_mer as vendedor on vendedor.cod_mer = cliente.cod_ven ";
                cadena = cadena + "inner join CrMae_cli as cliCamp on cliCamp.cod_ter = cliente.cod_ter ";
                cadena = cadena + "where cabeza.cod_trn between '004' and '008' and cliente.clasific='1' ";
                cadena = cadena + "and linea.cod_tip = '" + LB_linea.Text + "' ";                
                cadena = cadena + "and cabeza.fec_trn  between '" + fecha_ini.Text + "' and '" + fe_fin + "' ";
                cadena = cadena + "group by cliente.nom_ter,cliente.tel1,cliente.email,linea.nom_tip,vendedor.nom_mer,cliente.cod_ter,linea.cod_tip ";
                cadena = cadena + "order by ultfecha ";

                
                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridCli.ItemsSource = dt.DefaultView;

                totalCli.Text = dt.Rows.Count.ToString();


            }
            catch (Exception e)
            {
                MessageBox.Show("error:"+e);                
            }
            
        }

        public void cargarTotalFilas(int e)
        {
            if (e == 1)
            {
                try
                {
                    string cadena = "select cod_ter from CrTemCampa where cod_camp = '" + cod_camp + "' ";
                    cadena = cadena + "group by cod_ter ";

                    DataTable dt = SiaWin.Func.SqlDT(cadena, "ClientesVendedores", idemp);
                    Total.Text = "Total de Clientes : " + dt.Rows.Count;

                }
                catch (Exception w)
                {
                    MessageBox.Show("error:" + w);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TBcamp.Text = nom_camp;
            LB_camp.Text = cod_camp;
            cargarTotalFilas(1);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {    
            string queryUPD_TER = "";
            string nombres_todos = "";

            try
            {
                var reflector = this.dataGridCli.View.GetPropertyAccessProvider();
                foreach (var row in this.dataGridCli.SelectedItems)
                {
                    foreach (var column in dataGridCli.Columns)
                    {
                        var cellvalue = reflector.GetValue(row, column.MappingName);

                        var nombre = dataGridCli.Columns[1].MappingName;
                        var nombre_cli = reflector.GetValue(row, nombre.Trim());

                        nombres_todos = nombres_todos.Trim() + "- " + nombre_cli;
                        queryUPD_TER = queryUPD_TER + "insert into CrTemCampa (cod_camp,cod_ter) values ('" + cod_camp + "','" + cellvalue + "') ";
                        break;
                    }
                }
                SiaWin.Func.SqlDT(queryUPD_TER, "Clientes", idemp);
                MessageBox.Show("Asignacion del Cliente " + nombres_todos.Trim() + " a la Campaña " + nom_camp.Trim() + " Exitosa");
                cargarCampa();
                cargarTotalFilas(1);

            }
            catch (Exception w)
            {

                MessageBox.Show("error"+w);
            }
            

        }

        private void FirstDetailsViewGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            cargarCampa();
            BTNasignar.IsEnabled = true;
        }

        public void cargarCampa() {
            try
            {
                DataRowView row = (DataRowView)dataGridCli.SelectedItems[0];
                string cod_cli = row[0].ToString();

                string queryGrid = "select temporal.cod_camp as cod_camp,campa.nom_camp as nom_camp from CrTemCampa as temporal ";
                queryGrid = queryGrid + "inner join CrMae_campa as campa on campa.cod_camp = temporal.cod_camp ";
                queryGrid = queryGrid + "where temporal.cod_ter = '" + cod_cli + "' ";
                queryGrid = queryGrid + "and campa.estado = 1 ";                
                queryGrid = queryGrid + "group by temporal.cod_camp,campa.nom_camp ";

                DataTable dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
                dataGriCam.ItemsSource = dt.DefaultView;
            }
            catch (Exception)
            {

                
            }
            

        }

        private void BTNbuscar_Click(object sender, RoutedEventArgs e)
        {
            GridCliente();
        }

        private void TXB_linea_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int idr = 0; string code = ""; string nombre = "";
                dynamic xx = SiaWin.WindowBuscar("InMae_tip", "cod_tip", "nom_tip", "cod_tip", "idrow", "Maestra De Lineas", cnEmp, false, "");
                xx.ShowInTaskbar = false;
                xx.Owner = Application.Current.MainWindow;
                xx.ShowDialog();
                idr = xx.IdRowReturn;
                code = xx.Codigo;
                nombre = xx.Nombre;
                xx = null;

                if (idr > 0)
                {
                    LB_linea.Text = code;
                    TXB_linea.Text = nombre.Trim();
                    BTNbuscar.IsEnabled = true;                    
                }
                if (string.IsNullOrEmpty(code)) e.Handled = false;
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }



    }
}
