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
using System.Windows.Shapes;
namespace SiasoftAppExt
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class SaldosBodegas : Window
    {
        dynamic SiaWin;
        string _conexion;
        DataTable bodCND = new DataTable();
        //string codigo, string nombre, int idrow, string conexion, string idbod, int idemp
        public SaldosBodegas(string codigo, string nombre, int idrow, string conexion, string idbod, int idemp)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            TxtCodigo.Text = codigo;
            //MessageBox.Show(nombre);
            TxtNombre.Text = nombre;
            _conexion = conexion;
           // GetDataTable(2);
            //GetDataTable(2);
            //loadData(codigo, idbod, idemp);
        }
        private void loadData(string idrow, string idbod, int idemp)
        {
            double sum = 0;
            foreach (System.Data.DataColumn col in bodCND.Columns) col.ReadOnly = false;
            foreach (DataRow dr in bodCND.Rows) // search whole table
            {
                string idbodx = dr["cod_bod"].ToString();
                double saldoin = SiaWin.Func.SaldoInv(idrow, idbodx, idemp);
                dr["saldo"] = saldoin; //change the name
                sum = sum + saldoin;
            }
            TotalCnd.Text = sum.ToString("N2");
            if (dataGrid.Items.Count == 0) return;
            //dataGrid.SelectedItem = dataGrid.Items[1];
            dataGrid.Focus();
            //dataGrid.SelectedIndex = 0;
            dataGrid.SelectedItem = dataGrid.Items[0];
            dataGrid.SelectedIndex = 0;
            dataGrid.Focus();
            dataGrid.SelectedIndex = 0;
            //TotalPv.Text = sum1.ToString("N2");
            //TotalGeneral.Text = (sum + sum1).ToString("N2");
        }
        public void GetDataTable(int tipobod)
        {
            try
            {
                string sql = "select idrow,cod_bod,nom_bod,0000000.00 as saldo,cod_emp from inmae_bod where tipo_bod=" + tipobod.ToString() +  " and estado=1 order by cod_bod";
                SqlConnection conn1 = new SqlConnection(_conexion);
                SqlCommand cmd1 = new SqlCommand(sql, conn1);
                conn1.Open();
                //MessageBox.Show(sql);
                SqlDataReader dr = cmd1.ExecuteReader();
                if (tipobod == 2)
                {
                    bodCND.Load(dr);
                    //  MessageBox.Show(bodCND.Rows.Count.ToString());
                    dataGrid.ItemsSource = bodCND.DefaultView;
                }
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
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                e.Handled = true;
            }
        }

    }
}
