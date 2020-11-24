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
    // original
    //Sia.PublicarPnt(9508,"AplicacionBono");
    //dynamic WinDescto = ((Inicio)Application.Current.MainWindow).WindowExt(9508,"AplicacionBono");
    //WinDescto.ShowInTaskbar = false;
    //WinDescto.Owner = Application.Current.MainWindow;
    //WinDescto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //WinDescto.ShowDialog();

    // prueba
    //Sia.PublicarPnt(9490,"AplicacionBono");
    //dynamic WinDescto = ((Inicio)Application.Current.MainWindow).WindowExt(9490,"AplicacionBono");
    //WinDescto.ShowInTaskbar = false;
    //WinDescto.Owner = Application.Current.MainWindow;
    //WinDescto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //WinDescto.ShowDialog();

    public partial class AplicacionBono : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        public string tercero_cod = "";
        public string tercero_nom = "";


        public string numero_bono = "";
        public decimal valor_bono = 0;

        public AplicacionBono()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;


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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tercero_cod))
            {
                win.IsEnabled = false;
            }

            Tx_user.Tag = tercero_cod.Trim();
            Tx_user.Text = tercero_nom.Trim();
            LoadConfig();
        }

        private void BtnAplicar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var vali = FindBono(Tx_bono.Text);

                if (vali.Item1 == false)
                {
                    MessageBox.Show("el bono ingresado : " + Tx_bono.Text + " no existe", "Alerta");
                    return;
                }

                if (vali.Item2 == false)
                {
                    MessageBox.Show("el bono : " + Tx_bono.Text + " tiene el estado inactivo", "Alerta");
                    return;
                }


                bool flag = vali_bono(Tx_bono.Text);
                if (flag) return;

                DataTable dt = SiaWin.Func.SqlDT("select * from inMae_bonos where num_bono='" + Tx_bono.Text + "';", "Bonos", idemp);
                if (dt.Rows.Count > 0)
                {
                    numero_bono = dt.Rows[0]["num_bono"].ToString();
                    //valor_bono = Convert.ToDouble(dt.Rows[0]["valor"]);                    
                    valor_bono = Convert.ToDecimal(Tx_valorBono.Value);
                    
                    this.Close();
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("error al aplicar:"+w);
            }
        }

        private void Tx_bono_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Tx_bono.Text)) return;

            var vali = FindBono(Tx_bono.Text);
            if (vali.Item1 == false)
            {
                MessageBox.Show("el bono ingresado:" + Tx_bono.Text + " no existe", "Alerta");
                return;
            }
            if (vali.Item2 == false)
            {
                MessageBox.Show("el bono : " + Tx_bono.Text + " tiene el estado inactivo", "Alerta");
                return;
            }

            Tx_valorBono.Value = vali.Item3;


            vali_bono(Tx_bono.Text);
        }

        public Tuple<bool, bool, decimal> FindBono(string bono)
        {
            bool flag = false;
            bool estado = false;
            decimal valor = 0;
            try
            {
                DataTable dt = SiaWin.Func.SqlDT("select * from inMae_bonos where num_bono='" + bono + "';", "Bonos", idemp);
                if (dt.Rows.Count > 0)
                {
                    flag = true;
                    estado = Convert.ToBoolean(dt.Rows[0]["estado"]);
                    valor = Convert.ToDecimal(dt.Rows[0]["valor"]);
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("error al buscar el bono:" + w);
            }
            return new Tuple<bool, bool, decimal>(flag, estado, valor);
        }




        public bool vali_bono(string bono)
        {
            bool flag = false;
            try
            {
                DataTable dt = SiaWin.Func.SqlDT("select * from InCab_doc where num_bono='" + bono + "';", "Bonos", idemp);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("el bono ingresado" + bono + " ya esta registrado en un compra", "Alerta");
                    flag = true;
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("error al buscar el bono:" + w);
            }
            return flag;
        }











    }
}
