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

    //Sia.PublicarPnt(9483,"CambioBodegaFactura");
    //dynamic WinDescto = ((Inicio)Application.Current.MainWindow).WindowExt(9483,"CambioBodegaFactura");
    //WinDescto.ShowInTaskbar = false;
    //WinDescto.Owner = Application.Current.MainWindow;
    //WinDescto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //WinDescto.ShowDialog(); 


    public partial class CambioBodegaFactura : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";


        public CambioBodegaFactura()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            LoadConfig();
            pantalla();
        }

        public void pantalla(){
            this.MaxHeight = 500;
            this.MinHeight = 500;
            this.MaxWidth = 800;
            this.MinWidth = 800;
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

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
               string idTab = ((TextBox)sender).Tag.ToString();               
               if (idTab.Length > 0)
                    {
                        string tag = ((TextBox)sender).Tag.ToString();
                        string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = false; string cmpwhere = "";
                        if (string.IsNullOrEmpty(tag)) return;

                        if (tag == "incab_doc")
                        {
                            cmptabla = tag; cmpcodigo = "idreg"; cmpnombre = "num_trn"; cmporden = "idreg"; cmpidrow = "idreg"; cmptitulo = "Transacciones"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "cod_trn='141' ";
                        }
                        if (tag == "inmae_bod")
                        {
                            cmptabla = tag; cmpcodigo = "cod_bod"; cmpnombre = "nom_bod"; cmporden = "cod_bod"; cmpidrow = "cod_bod"; cmptitulo = "Maestra de Bodegas"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                        }
                        if (tag == "incab_doc2")
                        {
                            cmptabla = "incab_doc"; cmpcodigo = "idreg"; cmpnombre = "num_trn"; cmporden = "idreg"; cmpidrow = "idreg"; cmptitulo = "Transacciones"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "cod_trn='141' ";
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
                            if (tag == "incab_doc")
                            {
                                TX_idReg.Text = code; TX_Trans.Text = nom.Trim();
                                BuscarBodega(nom);
                            }
                            if (tag == "inmae_bod")
                            {
                                TX_bod.Text = code; Act_bod.Text = nom.Trim();
                            }
                            if (tag == "incab_doc2")
                            {
                            TXdocumento.Text = nom.Trim();
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

        public void BuscarBodega(string codigo)
        {
            try
            {
                string cadena = "select cabeza.idreg,cabeza.num_trn,cabeza.bod_tra,bodega.nom_bod,cabeza.cod_trn from InCab_doc as cabeza ";
                cadena = cadena + "inner join inmae_bod as bodega on cabeza.bod_tra=bodega.cod_bod ";
                cadena = cadena + "where num_trn='"+codigo+"' and cod_trn='051' ";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "GET", idemp);
                if (dt.Rows.Count>0)
                {
                    Act_Bod.Text = dt.Rows[0]["nom_bod"].ToString();
                    Act_Trns.Text = dt.Rows[0]["num_trn"].ToString();
                }
                
            }
            catch (Exception w)
            {
                MessageBox.Show("erro en traer los datos:"+w);
            }

        }
        
        private void BTNcambiar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TX_Trans.Text == "" && string.IsNullOrEmpty(TX_Trans.Text))
                {
                    MessageBox.Show("Debe de Ingresar El Documento Realizar la Actualizacion");
                    return;
                }
                if (TX_bod.Text == "" && string.IsNullOrEmpty(TX_bod.Text))
                {
                    MessageBox.Show("Debe de Ingresar la Bodega de Cambio para Realizar la Actualizacion");
                    return;
                }
                string cadena = "update InCab_doc set bod_tra='"+TX_bod.Text+"' where idreg='"+TX_idReg.Text+"' ";
                SiaWin.Func.SqlDT(cadena, "GET", idemp);                
                MessageBox.Show("cambio de bodega en el documento" + Act_Trns.Text.Trim() + " Exitoso");
                clean();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void clean() {
            Act_Bod.Text = "...";
            Act_Trns.Text = "...";
            Act_bod.Text = "...";
            TX_Trans.Text = "";
            TX_idReg.Text = "";
            TX_bod.Text = "";
        }

        private void BTNcancelar_Click(object sender, RoutedEventArgs e)
        {
            clean();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TXdocumento.Text == "" && string.IsNullOrEmpty(TXdocumento.Text))
                {
                    MessageBox.Show("Debe de Ingresar El Documento Realizar la Consulta");
                }

                string cadena = "select cabeza.fec_trn,cabeza.idreg,cabeza.num_trn,cabeza.bod_tra,bodega.nom_bod,cabeza.cod_trn from InCab_doc as cabeza ";
                cadena = cadena + "inner join inmae_bod as bodega on cabeza.bod_tra=bodega.cod_bod ";
                cadena = cadena + "where num_trn='"+TXdocumento.Text+"' and cod_trn='141'";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "GET", idemp);
                GridConsulta.ItemsSource = dt.DefaultView;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
