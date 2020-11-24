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

namespace TomaInventario
{

    public partial class DeleteCorte : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        public string bodega = "";
        public string usuario = "";
        public DeleteCorte()
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
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "aquiio");
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserTX.Tag = usuario;
                BodeTX.Tag = bodega;
                cargar();
            }
            catch (Exception w)
            {
                MessageBox.Show("error en el load:" + w);
            }
        }

        public void cargar()
        {            
            string query = "select corte from TomaInventario where id_usurio='" + UserTX.Tag + "' and bodega='" + BodeTX.Tag + "' group by corte";
            DataTable dt = SiaWin.Func.SqlDT(query, "inventario", idemp);
            CB_corte.ItemsSource = dt.DefaultView;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CB_corte.SelectedIndex >= 0)
                {
                    int corte = Convert.ToInt32(CB_corte.SelectedValue);
                    string delete = "delete TomaInventario where id_usurio='" + usuario + "' and bodega='" + bodega + "' and corte=" + corte + " ";        
                    if (SiaWin.Func.SqlCRUD(delete, idemp) == true) { MessageBox.Show("se elimino el corte"); cargar(); }
                }
                

            }
            catch (Exception w)
            {
                MessageBox.Show("ERROR AL ELIMINAR:" + w);
            }

        }


    }
}
