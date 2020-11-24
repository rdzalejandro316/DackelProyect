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
    
    public partial class Cumpleaños : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";
        public string cod_camp;
        public string nom_camp;


        public Cumpleaños()
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            LoadConfig();

            pantalla();
        }
        public void pantalla() {
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

        private void BTNbuscar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string cadena = "select cliente.cod_ter,cliente.nom_ter as nom_ter,CONVERT(varchar,cliente.fec_cump,103) as fec_cump,cliente.cod_ven as cod_ven, vendedor.nom_mer as nom_mer ,sum( iif(cabeza.cod_trn between '004' and '005',CAST(cuerpo.cantidad as int),CAST(-cuerpo.cantidad as int) ) ) as cantidad, sum((cantidad*val_uni)*iif(trn.tip_trn=1,-1,1)) as monto, max(iif(cabeza.cod_trn='005',fec_trn,'')) as ultfecha ";
                cadena += "from InCab_doc as cabeza ";
                cadena += "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
                cadena += "inner join inmae_bod as bod on bod.cod_bod=cuerpo.cod_bod ";
                cadena += "inner join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
                cadena += "inner join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";
                cadena += "inner join comae_ter as cliente on cliente.cod_ter = cabeza.cod_cli ";
                cadena += "left join InMae_mer as vendedor on vendedor.cod_mer = cliente.cod_ven ";
                cadena += "inner join InMae_trn trn on trn.cod_trn=cabeza.cod_trn and trn.ind_vtas=1 and trn.Tip_trn between 1 and 2 ";
                cadena += "where cabeza.cod_trn between '004' and '008' and cliente.clasific='1' ";
                cadena += "and datepart(mm, cliente.fec_cump ) = datepart(mm, '" + fecha_ini.Value.ToString() + "')  ";
                cadena += "group by cliente.nom_ter,cliente.cod_ter,cliente.fec_cump,cliente.cod_ven,vendedor.nom_mer ";
                cadena += "order by nom_ter ";

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
                //cargarTotalFilas(1);

            }
            catch (Exception w)
            {
                MessageBox.Show("Seleciona un cliente" + w);
            }
        }





    }
}
