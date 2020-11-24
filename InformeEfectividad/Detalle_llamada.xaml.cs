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
    
    public partial class Detalle_llamada : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        //variables pasadas
        public string cod_vendedor = "";
        public string nom_vendedor = "";
        public string fecha_ini = "";
        public string fecha_fin = "";

        public Detalle_llamada()
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

        public void pantalla()
        {
            this.MinWidth = 1000;
            this.Height = 500;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llamadas();
        }


        public void llamadas() {
            try
            {
                string cadena = "select iif(LEN(tercero.cod_ter) > 0,'1','0') as filtro,seguimiento.fec_seg as fec_seg, concepto.nom_con as nom_con,tercero.cod_ter as cod_ter, tercero.nom_ter as nom_ter,iif(cliente.ct_cel='1','SI',iif(cliente.ct_cel='0','NO','')) as ct_cel ";
                cadena += "from Comae_ter as tercero ";
                cadena += "inner join Crseg_cli as seguimiento on seguimiento.cod_ter = tercero.cod_ter ";
                cadena += "inner join CrMae_concepto as concepto on seguimiento.cod_con = concepto.cod_con ";
                cadena += "inner join CrMae_cli as cliente on tercero.cod_ter = cliente.cod_ter ";
                cadena += "where seguimiento.cod_mer='" + cod_vendedor + "' and seguimiento.cod_con='01' ";
                cadena += "and seguimiento.fec_seg between '" + fecha_ini +  "' and '" + fecha_fin + " 23:59:59' ";

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridllamadas.ItemsSource = dt.DefaultView;
                total.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("error al cargar1:");
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
