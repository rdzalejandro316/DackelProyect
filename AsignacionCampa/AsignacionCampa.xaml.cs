using AsignacionCampa;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SiasoftAppExt
{

    public partial class AsignacionCampa : UserControl
    {


        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";


        public AsignacionCampa(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;

            LoadConfig();
            GridCAmpa();

            BTNCliCam.IsEnabled = false;

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
                tabitem.Title = "Asignacion de Campaña(" + aliasemp + ")";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void GridCAmpa() {
            string cadena = "select cod_camp,nom_camp,iif(estado=1,'Activo' ,'Inactivo' ) as estado,fecha_ini,fecha_fin from CrMae_campa where estado=1";
            DataTable dt = SiaWin.Func.SqlDT(cadena, "Vendedores", idemp);
            dataGridCam.ItemsSource = dt.DefaultView;
            TotalGrid.Text = dt.Rows.Count.ToString();
        }


        public void BtnOpen_Click(object sender, RoutedEventArgs e) {
            try
            {
                Clientes cliente = new Clientes();
                DataRowView row = (DataRowView)dataGridCam.SelectedItems[0];
                string codCam = row[0].ToString();
                string nomCam = row[1].ToString();
                cliente.cod_camp = codCam;
                cliente.nom_camp = nomCam;

                cliente.ShowInTaskbar = false;
                cliente.Owner = Application.Current.MainWindow;
                cliente.ShowDialog();                
                cargarClientes();
            }
            catch (Exception)
            {
                MessageBox.Show("Selecione una Campaña");
            }

        }

        public void BtnOpenCumple_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cumpleaños win = new Cumpleaños();
                DataRowView row = (DataRowView)dataGridCam.SelectedItems[0];
                win.cod_camp = row[0].ToString();
                win.nom_camp = row[1].ToString();
                win.ShowInTaskbar = false;
                win.Owner = Application.Current.MainWindow;
                win.ShowDialog();                

                cargarClientes();

            }
            catch (Exception)
            {
                MessageBox.Show("Selecione una Campaña");
            }
            
        }

        public void BtnOpenVen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vendedor win = new vendedor();
                DataRowView row = (DataRowView)dataGridCam.SelectedItems[0];
                win.cod_camp = row[0].ToString();
                win.nom_camp = row[1].ToString();
                win.ShowInTaskbar = false;
                win.Owner = Application.Current.MainWindow;
                win.ShowDialog();                

                cargarClientes();

            }
            catch (Exception )
            {
                MessageBox.Show("Selecione una Campaña");
            }

        }
        


        private void desbloqueBTN(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e) {

            BTNCliCam.IsEnabled = true;
        }


        private void FirstDetailsViewGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            cargarClientes();
        }

        public void cargarClientes() {
            try
            {
                DataRowView row = (DataRowView)dataGridCam.SelectedItems[0];
                string cod_cam = row[0].ToString();

                string queryGrid = "select cliente.cod_ter,tercero.nom_ter from CrMae_cli as cliente ";
                queryGrid = queryGrid + "inner join Comae_ter as tercero on tercero.cod_ter = cliente.cod_ter ";
                queryGrid = queryGrid + "inner join CrTemCampa as campaña on campaña.cod_ter = cliente.cod_ter ";
                queryGrid = queryGrid + "where campaña.cod_camp='" + cod_cam + "' ";
                queryGrid = queryGrid + "group by cliente.cod_ter,tercero.nom_ter ";

                DataTable dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
                dataGridClientes.ItemsSource = dt.DefaultView;

                CampaGrid.Text = row[1].ToString();
                ClientesTotal.Text = dt.Rows.Count.ToString();
            }
            catch (Exception w)
            {
                MessageBox.Show("error : "+w);
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SiaWin.Tab(9412);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
                        
            DataRowView row1 = (DataRowView)dataGridCam.SelectedItems[0];
            string cod_cam = row1[0].ToString();
            string nom_cam = row1[1].ToString();

            try
            {
                string cadena = "";
                Boolean not = false;
                var reflector = this.dataGridClientes.View.GetPropertyAccessProvider();
                foreach (var row in this.dataGridClientes.SelectedItems)
                {
                    foreach (var column in dataGridClientes.Columns)
                    {
                        var cellvalue = reflector.GetValue(row, column.MappingName);

                        cadena = cadena + "delete from CrTemCampa where cod_ter='" + cellvalue + "' and cod_camp = '" + cod_cam + "' ";
                        not = true;                        
                        break;
                    }
                }

                if (not==true) {
                    SiaWin.Func.SqlDT(cadena, "Clientes", idemp);                    
                }
                                
                MessageBox.Show("Clientes eliminados de la campaña: "+ nom_cam);
                cargarClientes();
            }
            catch (Exception w)
            {
                MessageBox.Show("error" + w);
            }


        }





    }
}
