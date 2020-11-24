using RegistroBono;
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


    //Sia.PublicarPnt(9506,"RegistroBono");
    //dynamic WinDescto = ((Inicio)Application.Current.MainWindow).WindowExt(9506,"RegistroBono");
    //WinDescto.ShowInTaskbar = false;
    //WinDescto.Owner = Application.Current.MainWindow;
    //WinDescto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //WinDescto.ShowDialog();


    public partial class RegistroBono : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        public RegistroBono()
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
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());                
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "aquiio");
            }
        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            Consultar win = new Consultar();
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowInTaskbar = false;
            win.Owner = Application.Current.MainWindow;
            win.ShowDialog();
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Registrar win = new Registrar();
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowInTaskbar = false;
            win.Owner = Application.Current.MainWindow;
            win.ShowDialog();
        }









    }
}
