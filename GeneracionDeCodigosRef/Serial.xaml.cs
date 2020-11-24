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

namespace GeneracionDeCodigosRef
{

    public partial class Serial : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        public string serialExt = "";

        public Serial()
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            LoadConfig();
            pantalla();
        }

        public void pantalla()
        {
            this.MinHeight = 400;
            this.MaxHeight = 400;
            this.MinWidth = 700;
            this.MaxWidth = 700;
        }

        private void LoadConfig()
        {
            try
            {
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void BTsalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TXserial.Text = serialExt;
                string cadena = "select cod_ref,nom_ref,fec_crea,val_ref from InMae_refTemp where serial='" + TXserial.Text + "' ";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "validar", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;
                TXtotal.Text = dt.Rows.Count.ToString();
            }
            catch (Exception w)
            {
                MessageBox.Show("error al cargar seriales" + w);
            }
        }



    }
}
