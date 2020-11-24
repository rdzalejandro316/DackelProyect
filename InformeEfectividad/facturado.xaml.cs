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
    
    public partial class facturado : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        //variables pasadas
        public string cod_vendedor = "";
        public string nom_vendedor = "";
        public string fecha_ini = "";
        public string fecha_fin = "";

        public facturado()
        {
            InitializeComponent();
            
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;

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
            Fact_seg();
            TotalFact();
        }


        public void Fact_seg()
        {
            try
            {
                string cadena = "select tercero.cod_ter as cod_ter,tercero.nom_ter as nom_ter,iif(cabeza.cod_trn='005',cabeza.fec_trn,'') as ultfecha,referencia.nom_ref as nom_ref,linea.nom_tip as nom_tip,seguimineto.fec_seg as fec_seg,concepto.nom_con as nom_con ";
                cadena += "from comae_ter as tercero ";
                cadena += "inner join incab_doc as cabeza on  cabeza.cod_cli = tercero.cod_ter ";
                cadena += "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
                cadena += "inner join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
                cadena += "inner join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";
                cadena += "inner join Crseg_cli as seguimineto on seguimineto.cod_ter = tercero.cod_ter ";
                cadena += "inner join CrMae_concepto as concepto on seguimineto.cod_con = concepto.cod_con ";
                cadena += "where tercero.cod_ven = '" + cod_vendedor + "' and cabeza.cod_trn between '004' and '005'  ";
                cadena += "and cabeza.fec_trn between '" + fecha_ini + "' and '" + fecha_fin + " 23:59:59' ";                

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridTotalFactSeg.ItemsSource = dt.DefaultView;
                total1.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("error al cargar1:");
            }
        }

        public void TotalFact()
        {
            try
            {
                string cadena = "select tercero.cod_ter as cod_ter,tercero.nom_ter as nom_ter,iif(cabeza.cod_trn='005',cabeza.fec_trn,'') as ultfecha,referencia.nom_ref as nom_ref,linea.nom_tip as nom_tip ";
                cadena += "from comae_ter as tercero ";
                cadena += "inner join incab_doc as cabeza on  cabeza.cod_cli = tercero.cod_ter ";
                cadena += "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
                cadena += "inner join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
                cadena += "inner join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";                
                cadena += "where tercero.cod_ven = '" + cod_vendedor + "' and cabeza.cod_trn between '004' and '005'  ";
                cadena += "and cabeza.fec_trn between '" + fecha_ini + "' and '" + fecha_fin + " 23:59:59' ";

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridFact.ItemsSource = dt.DefaultView;
                total2.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("error al cargar1:");
            }
        }



        private void dataGrid_FilterChanged(object sender, GridFilterEventArgs e)
        {
            string tag = ((SfDataGrid)sender).Tag.ToString();

            if (tag == "grid1")
            {
                total1.Text = dataGridTotalFactSeg.View.Records.Count.ToString();                                
            }
            if (tag == "grid2")
            {
                total2.Text = dataGridFact.View.Records.Count.ToString();                
            }

        }







    }
}
