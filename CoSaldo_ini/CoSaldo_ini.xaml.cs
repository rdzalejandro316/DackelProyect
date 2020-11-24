using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    
    public partial class CoSaldo_ini : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";

        public CoSaldo_ini(dynamic tabitem1)
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            LoadConfig();

            

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
                tabitem.Title = "Saldos(" + aliasemp + ")";

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void BTNgenerar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("SpCoSaldo_ini", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ano", V_Fecha.Text);                
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                string cadena = "select Ano,Cod_cta,Cod_ter,Cod_Cco,Saldo_Ini,tipo,idrow from CoSaldos_cta";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes1", idemp);
                dataGridConsulta.ItemsSource = dt.DefaultView;



                MessageBox.Show("Generacion de Saldos Iniciales para el año " + V_Fecha.Text  + " exitoso");
                BTNgenerar.IsEnabled = false;

            }
            catch (Exception w)
            {
                MessageBox.Show("error grid: " + w);
            }

 
        }







    }
}
