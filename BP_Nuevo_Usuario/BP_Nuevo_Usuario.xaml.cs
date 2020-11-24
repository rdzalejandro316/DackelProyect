using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SiasoftAppExt
{
    
    public partial class BP_Nuevo_Usuario : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";


        public BP_Nuevo_Usuario(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            LoadConfig();

            

            //accordion.AccentBrush = new SolidColorBrush() { Color = UserControl.UI.Colors.Red };
            //accordion.UnselectAll();
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
                tabitem.Title = "Nuevo Usuario (" + aliasemp + ")";


                //ImageNuevo.Source = new BitmapImage(new Uri(@"/Imagenes/4287.png", UriKind.Relative));
                //ImageNuevo.Source = "Imagenes/3.PNG";

                //Imagen.Source = new BitmapImage(new Uri("../Imagenes/4287.png", UriKind.Relative));
                //Source = "../../Imagenes/4287.png"

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }




        private void Button_Vis(object sender, RoutedEventArgs e)
        {
            string tag = ((ToggleButton)sender).Tag.ToString();

            if (tag == "1")
            {
                Thickness marginMenu = menu.Margin;
                marginMenu.Left = 0;
                menu.Margin = marginMenu;

                Thickness marginCont = conte.Margin;
                marginCont.Left = 200;
                conte.Margin = marginCont;
                MenuBTN.Tag = "2";
            }
            else{
                Thickness marginMenu = menu.Margin;
                marginMenu.Left = -200;
                menu.Margin = marginMenu;

                Thickness marginCont = conte.Margin;
                marginCont.Left = 0;
                conte.Margin = marginCont;
                MenuBTN.Tag = "1";
            }


            //menu_btnVis.Visibility = Visibility.Hidden;
            //menu_btnHid.Visibility = Visibility.Visible;


        }

        //void Button_Hid(object sender, RoutedEventArgs e)
        //{
        //    Thickness marginMenu = menu.Margin;
        //    marginMenu.Left = -200;
        //    menu.Margin = marginMenu;


        //    Thickness marginCont = conte.Margin;
        //    marginCont.Left = 0;
        //    conte.Margin = marginCont;

        //    menu_btnVis.Visibility = Visibility.Visible;
        //    menu_btnHid.Visibility = Visibility.Hidden;

        //}




        void Open_Nuevo(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    MessageBox.Show("cerrando tab");
            //    //tabitem
            //}
            //catch (Exception w)
            //{

            //    MessageBox.Show("error tab: " + w);
            //}
        }

        void Open_NuevaBici(object sender, RoutedEventArgs e)
        {
            
        }

        void Open_Salida(object sender, RoutedEventArgs e)
        {
            
        }
        void Open_Historial(object sender, RoutedEventArgs e)
        {
        
        }
        void Open_Ingreso(object sender, RoutedEventArgs e)
        {
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
