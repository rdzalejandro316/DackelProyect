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
using System.Windows.Shapes;

namespace AsignacionClientes
{

    public partial class Reasignar : Window
    {

        dynamic SiaWin;        
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";

        public string CodVendedor, NomVendedor;

        public Reasignar()
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;            
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;

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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //validacion para que se ingrese fijo el campo de una maestra
                string idTab = ((TextBox)sender).Tag.ToString();

                if (idTab.Length > 0)
                {
                    string tag = ((TextBox)sender).Tag.ToString();
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                    if (string.IsNullOrEmpty(tag)) return;

                    if (tag == "inmae_mer")
                    {
                        cmptabla = tag; cmpcodigo = "cod_mer"; cmpnombre = "UPPER(nom_mer)"; cmporden = "cod_mer"; cmpidrow = "cod_mer"; cmptitulo = "Maestra de Vendedores"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "estado=1";
                    }
                   

                    int idr = 0; string code = ""; string nom = "";
                    dynamic winb = SiaWin.WindowBuscar(cmptabla, cmpcodigo, cmpnombre, cmporden, cmpidrow, cmptitulo, cnEmp, mostrartodo, cmpwhere);

                    winb.ShowInTaskbar = false;
                    winb.Owner = Application.Current.MainWindow;
                    winb.ShowDialog();
                    idr = winb.IdRowReturn;
                    code = winb.Codigo;
                    nom = winb.Nombre;
                    winb = null;
                    if (idr > 0)
                    {
                        if (tag == "inmae_mer")
                        {
                            Cod_ven.Text = code; Name_ven.Text = nom.Trim();
                            BTNreasignar.IsEnabled = true;
                            BTNreasignar.Content = "Asignar clientes al vendedor " + nom.Trim();
                        }


                        var uiElement = e.OriginalSource as UIElement;
                        uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                    }
                    e.Handled = true;
                }
                if (e.Key == Key.Enter)
                {
                    var uiElement = e.OriginalSource as UIElement;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           VenActual.Text = NomVendedor;
           CodVenActual.Text = CodVendedor;
        }

        private void BTNreasignar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Cod_ven.Text.Length > 0)
                {
                    string cadena = "update comae_ter  set cod_ven='" + Cod_ven.Text + "' where cod_ven='" + CodVenActual.Text + "' ";
                    SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                    MessageBox.Show("asiganacion de los clientes del vendor " + VenActual.Text.Trim() + " al Vendedor " + Name_ven.Text.Trim() + " Existosa");
                    this.Close();
                }
                else {
                    MessageBox.Show("Selecciona el vendedor al cual le quieres realizar la reasignacion de los clientes");
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("error en la reasignacion");                
            }
            
        }

        //TXB_vend






    }
}
