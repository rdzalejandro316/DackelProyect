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
    
    public partial class vendedor : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        public string cod_camp = "";
        public string nom_camp = "";


        public vendedor()
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            LoadConfig();

            pantalla();
        }
        public void pantalla()
        {
            this.MaxHeight = 600;
            this.MinHeight = 600;
            this.MinWidth = 1000;
            this.MaxWidth = 1000;
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

        private void TXB_vendedor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int idr = 0; string code = ""; string nombre = "";
                dynamic xx = SiaWin.WindowBuscar("inmae_mer", "cod_mer", "nom_mer", "cod_mer", "idrow", "Maestra De Vendedores", cnEmp, false, "");
                xx.ShowInTaskbar = false;
                xx.Owner = Application.Current.MainWindow;
                xx.ShowDialog();
                idr = xx.IdRowReturn;
                code = xx.Codigo;
                nombre = xx.Nombre;
                xx = null;

                if (idr > 0)
                {
                    TBX_nom.Text = nombre.Trim();
                    LB_ven.Text = code;
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


        private void BTNbuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cadena = "select cliente.cod_ter,cliente.nom_ter as nom_ter,CONVERT(varchar,cliente.fec_cump,103) as fec_cump,(cast(datediff(dd,cliente.fec_cump,GETDATE()) / 365.25 as int)) as edad,departamento.nom_dep as nom_dep,municipio.nom_muni as nom_muni ";
                cadena += "from comae_ter as cliente ";
                cadena += "left join MmMae_muni as municipio on cliente.cod_ciu = municipio.cod_muni ";
                cadena += "left join MmMae_depa as departamento on cliente.cod_depa = departamento.cod_dep ";
                cadena += "where cliente.cod_ven = '" + LB_ven.Text + "'";
                                                
                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridCli.ItemsSource = dt.DefaultView;

                Total.Text = dt.Rows.Count.ToString();
            }
            catch (Exception w)
            {
                MessageBox.Show("error al consultar " + w);
            }
        }

        private void dataGridCli_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            cargarCampa();
            Asignar.IsEnabled = true;
        }


        public void cargarCampa()
        {
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TBcamp.Text = nom_camp;
            LB_camp.Text = cod_camp;
        }

        private void Asignar_Click(object sender, RoutedEventArgs e)
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
                //MessageBox.Show(queryUPD_TER);
                SiaWin.Func.SqlDT(queryUPD_TER, "Clientes", idemp);
                MessageBox.Show("Asignacion del Cliente " + nombres_todos.Trim() + " a la Campaña " + nom_camp.Trim() + " Exitosa");
                cargarCampa();                
            }
            catch (Exception w)
            {
                MessageBox.Show("Seleciona un cliente" + w);
            }
        }





    }
}
