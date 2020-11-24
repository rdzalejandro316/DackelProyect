using Syncfusion.UI.Xaml.Grid;
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

namespace InformeEfectividad
{
    
    public partial class Detalle_CRM : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        //variables pasadas
        public string cod_vendedor = "";
        public string nom_vendedor = "";



        public Detalle_CRM()
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;

            LoadConfig();
            pantalla();
        }


        private void LoadConfig()
        {
            try
            {
                System.Data.DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);                
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();                                             
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void pantalla() {
            this.MinWidth = 1000;
            this.Height = 500;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                nom_ven.Text = nom_vendedor;

                string cadena = "select iif(LEN(tercero.cod_ter) > 0,'1','0') as filtro,tercero.cod_ter as cod_ter,tercero.nom_ter as nom_ter,tercero.nom1 as nom1,tercero.nom2 as nom2,tercero.apell1 as apell1, tercero.apell2 as apell2, tercero.tel1 as tel1,tercero.cel as cel,tercero.email as email, tercero.dir1 as dir1,tercero.fec_cump as fec_cump, ";
                cadena = cadena + "cliente.genero,IIF(cliente.est_civil ='1','SOLTERO',IIF(cliente.est_civil ='2','CASADO',IIF(cliente.est_civil ='3','UNION LIBRE',IIF(cliente.est_civil ='4','SEPARADO',IIF(cliente.est_civil ='5','VIUDO',''))))) AS est_civil ";
                cadena = cadena + "from Comae_ter as tercero ";
                cadena = cadena + "inner join crmae_cli as cliente on tercero.cod_ter = cliente.cod_ter ";
                cadena = cadena + "where tercero.cod_ven ='" + cod_vendedor + "' ";
                
                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridCRMtotal.ItemsSource = dt.DefaultView;

                total.Text = dt.Rows.Count.ToString();

            }
            catch (Exception w)
            {
                MessageBox.Show("error al cargar : "+w);
            }
            

        }

        private void dataGrid_FilterChanged(object sender, GridFilterEventArgs e)
        {
            var provider = (sender as SfDataGrid).View.GetPropertyAccessProvider();
            var records = (sender as SfDataGrid).View.Records;


            int totalX = 0;

            for (int i = 0; i < (sender as SfDataGrid).View.Records.Count; i++)
            {
                totalX += Convert.ToInt32(provider.GetValue(records[i].Data, "filtro").ToString());
            }

            total.Text = totalX.ToString();

        }





    }
}
