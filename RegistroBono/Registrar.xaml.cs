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

namespace RegistroBono
{
    public partial class Registrar : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";
        public Registrar()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void ValidacionNumeros(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract || e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("este campo solo admite valores numericos");
                e.Handled = true;
            }
        }  

        private void Tx_bono_LostFocus(object sender, RoutedEventArgs e)
        {
            vali_bono((sender as TextBox).Text.Trim());
        }   


        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            bool falg = vali_bono(Tx_Bono.Text.Trim());
            if (falg) return;

            bool validar = valdiarCampos();

            if (validar)
            {
                MessageBox.Show("llene todos los campos","Alerta",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }

            string query = "insert into inMae_bonos (num_bono,valor,fecha,estado) values ('"+ Tx_Bono.Text+ "',"+ Tx_valor.Text  + ",getdate(),1) ";



            if (SiaWin.Func.SqlCRUD(query, idemp) == true)
            {
                MessageBox.Show("registro de bono exitoso");
                clean();
            }
        }



        public void clean()
        {
            
            Tx_Bono.Text = "";
            Tx_valor.Text = "";
        }
        


        public bool vali_bono(string bono)
        {
            bool flag = false;
            try
            {
                DataTable dt = SiaWin.Func.SqlDT("select * from inMae_bonos where num_bono='" + bono + "';", "MaestraTalla", idemp);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("el bono ingresado" + Tx_Bono.Text + " ya esta registrado en la tabla", "Alerta");
                    flag = true;
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("error al buscar el bono:" + w);
            }
            return flag;
        }

        public bool valdiarCampos()
        {
            bool flag = false;
            if (string.IsNullOrEmpty(Tx_Bono.Text)) flag = true;
            if (string.IsNullOrEmpty(Tx_valor.Text)) flag = true;
            return flag;
        }






    }
}
