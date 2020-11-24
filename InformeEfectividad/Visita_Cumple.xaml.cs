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

    public partial class Visita_Cumple : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        //variables pasadas
        public string cod_vendedor = "";
        public string nom_vendedor = "";
        public string fecha_ini = "";
        public string fecha_fin = "";

        public Visita_Cumple()
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
            visitas();
            nom_ven.Text = nom_vendedor;
        }


        public void visitas()
        {
            try
            {
                string cadena = "select iif(LEN(tercero.cod_ter) > 0,'1','0') as filtro, seguimineto.fec_seg as fec_seg,concepto.nom_con as nom_con,tercero.cod_ter as cod_ter,tercero.nom_ter as nom_ter,tercero.fec_cump as fec_cump from comae_ter as tercero ";
                cadena += "inner join (select distinct cod_ter,cod_con,fec_seg from Crseg_cli) as seguimineto on seguimineto.cod_ter = tercero.cod_ter ";
                cadena += "inner join CrMae_concepto as concepto on seguimineto.cod_con = concepto.cod_con ";
                cadena += "where tercero.cod_ven='" + cod_vendedor + "' and seguimineto.cod_con='06' ";
                cadena += "and seguimineto.fec_seg  between '" + fecha_ini + "' and '" + fecha_fin + " 23:59:59' ";                

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridVisitas.ItemsSource = dt.DefaultView;
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
