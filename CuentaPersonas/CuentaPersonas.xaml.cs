using System;
using System.Collections.Generic;
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

    public partial class CuentaPersonas : Window
    {

        public string cod_bod;
        dynamic SiaWin;
        int idemp = 0;

        public CuentaPersonas()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;

            this.MinWidth = 500;
            this.MinHeight = 300;
            this.MaxWidth = 500;
            this.MaxHeight = 300;

            bloquearCampos(0);
        }

        public void bloquearCampos(int estado) {
            if (estado == 0)
            {
                TXB_bodega.IsEnabled = false;
                TXB_Cantidad.IsEnabled = false;
                TXB_observ.IsEnabled = false;
                cargarBTN.IsEnabled = true;
                guardarBTN.IsEnabled = false;
            }

            if (estado == 1)
            {
                TXB_bodega.IsEnabled = false;
                TXB_Cantidad.IsEnabled = true;
                TXB_observ.IsEnabled = true;
                cargarBTN.IsEnabled = false;
                guardarBTN.IsEnabled = true;

                TXB_Cantidad.Text = "0";
                TXB_observ.Text = "Niguna";

            }

        }

        private void cargarBTN_Click(object sender, RoutedEventArgs e)
        {
            TXB_bodega.Text = cod_bod;
            bloquearCampos(1);
        }

        private void guardarBTN_Click(object sender, RoutedEventArgs e)
        {

            if (TXB_bodega.Text.Length > 0 && TXB_Cantidad.Text.Length > 0 && TXB_observ.Text.Length > 0)
            {
                try
                {
                    SiaWin.Func.SqlDT("insert into crcuentapersonas (cod_bod, fecha, cantidad, observ) values ('" + TXB_bodega.Text + "' ,'" + DateTime.Now.ToString() + "', '" + TXB_Cantidad.Text  + "', '" + TXB_observ.Text + "')", "Clientes", idemp);
                    MessageBox.Show("insercion exitosa");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else {
                MessageBox.Show("llena todos los campos");
            }

        }




    }
}
