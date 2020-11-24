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

    public partial class UpdateValues : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";


        public string value = "";
        string valor1 = "";


        DataTable Referencias = new DataTable();


        public UpdateValues()
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
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void pantall()
        {
            this.MaxHeight = 350;
            this.MinHeight = 350;
            this.MaxWidth = 600;
            this.MinWidth = 600;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (value == "val_ref")
            {
                TitleGS.Header = "Valor Referencia";
                Tx_ingVal.Text = "Factor :";
                valor1 = "cost_bas";
            }
            if (value == "precio_us")
            {
                TitleGS.Header = "Precio Dolar";
                Tx_ingVal.Text = "Dolar :";
                valor1 = "val_ref";
            }

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string value = (sender as TextBox).Text.ToString().Trim();

            if (string.IsNullOrEmpty(value)) return;


            string query = "select cod_ref," + valor1 + ",'' as resultado from InMae_refTemp where im='" + value + "';";
            Referencias.Clear();
            Referencias = SiaWin.Func.SqlDT(query, "validar", idemp);

            if (Referencias.Rows.Count > 0)
            {
                if (valor1 == "cost_bas")
                {
                    column_cost_bas.IsHidden = false;
                    column_val_ref.IsHidden = true;
                }
                if (valor1 == "val_ref")
                {
                    column_cost_bas.IsHidden = true;
                    column_val_ref.IsHidden = false;
                }
                dataGridCxC.ItemsSource = Referencias.DefaultView;
                TB_v2.Text = "";
            }
            else
            {
                TX_imp.Text = "";
                MessageBox.Show("el codigo de importacion ingresado no existe");
            }
        }




        private void TB_v2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TB_v2.Text.Length > 0)
            {                
                decimal valor = Convert.ToDecimal(TB_v2.Text);
                foreach (DataRow item in Referencias.Rows)
                {
                    decimal valorBase = Convert.ToDecimal(item["" + valor1 + ""]);
                    item["resultado"] = valor1 == "cost_bas" ? valorBase * valor :  Math.Floor(valorBase / valor);
                }
            }
        }

        private void ValidacionNumeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("este campo solo admite valores numericos");
                e.Handled = true;
            }
        }


        public bool validarCampos()
        {
            bool flag = true;
            if (TX_imp.Text == "" || string.IsNullOrEmpty(TX_imp.Text.ToString())) flag = false;
            if (TB_v2.Text == "" || string.IsNullOrEmpty(TB_v2.Text.ToString())) flag = false;
            if (Referencias.Rows.Count==0) flag = false;
            return flag;
        }

        private void BTNclose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BTNupdate_Click(object sender, RoutedEventArgs e)
        {
            if (validarCampos() == false) { MessageBox.Show("llene todos los campos"); return; }

            string query = "";
            foreach (DataRow item in Referencias.Rows)
            {
                decimal val_up = Convert.ToDecimal(item["resultado"]);
                string referencia = item["cod_ref"].ToString().Trim();
                query += "update InMae_refTemp set "+value+"="+val_up+"  where cod_ref='"+referencia+"'; ";
            }            
            if (Referencias.Rows.Count>0) if (SiaWin.Func.SqlCRUD(query, idemp) == true) MessageBox.Show("actualizacion exitosa");                         
        }





    }
}
