using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
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
using System.Drawing;
using System.Data;

namespace SeguimientoCliente
{
    /// <summary>
    /// Lógica de interacción para editarCliente.xaml
    /// </summary>
    public partial class editarCliente : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string vendedor = "";
        string codigoUsuario = "";
        string cnEmp = "";
        public string Conexion;
        dynamic tabitem = "";

        public Boolean bandera = false;

        //imagen
        public string strName = "", imageName;

        // puntos -----------------------------------------
        int _documentoLB_p = 0, _num_doc_p = 0, _nom1_p = 0, _nom2_p = 0, _appe1_p = 0, _appe2_p = 0, _genero_p = 0, _tel_p = 0,
        _cel_p = 0, _email_p = 0, _fecha_nac_p = 0, _dir_p = 0, _muniLB_p = 0, _est_civil_p = 0,
        _cod_cargoLB_p = 0, _cod_profLB_p = 0, _cod_ocupLB_p = 0, _nom_emp_p = 0, _hobbies_p = 0,
        _ct_cel_p = 0, _ct_email_p = 0, _ct_whats_p = 0, _ct_sms_p = 0, _ct_corres_p = 0, _ran_edad_p = 0;
        //codigo puntos
        string _documentoLB_c = "", _num_doc_c = "", _nom1_c = "", _nom2_c = "", _appe1_c = "", _appe2_c = "", _genero_c = "", _tel_c = "",
         _cel_c = "", _email_c = "", _fecha_nac_c = "", _dir_c = "", _muniLB_c = "", _est_civil_c = "",
         _cod_cargoLB_c = "", _cod_profLB_c = "", _cod_ocupLB_c = "", _nom_emp_c = "", _hobbies_c = "",
         _ct_cel_c = "", _ct_email_c = "", _ct_whats_c = "", _ct_sms_c = "", _ct_corres_c = "", _ran_edad_c = "";





        //variables iniciales
        public string _cod_ter = "", _documentoLB = "", _nom1 = "", _nom2 = "", _appe1 = "", _appe2 = "", _tel1 = "", _tel2 = "",
        _cel = "", _email = "", _dir = "", _dir1 = "", _dir2 = "", _muniLB = "", _fecha_nac = "", _genero = "", _est_civil = "", _nom_emp = "", _ct_cel = "", _ct_celLB = "",
        _ct_email = "", _ct_whats = "", _ct_sms = "", _ct_corres = "", _cod_cargoLB = "", _cod_ocupLB = "", _cod_profLB = "",
        _num_doc = "", _hobbies = "", _cod_cargo = "", _cod_ocup = "", _cod_prof = "", _documento = "", _muni = "", _nom_comple = "",
        _depaLB = "", _depa = "", _act_emp = "", _obser = "", _facebook = "", _instagram = "", _image_name = "",
        _ran_edad = "", _talla_zap_tenn = "", _LB_talla_zap_tenn = "", _talla_pant_fald = "", _LB_talla_pant_fald = "", _talla_vest_traj = "",
        _LB_talla_vest_traj = "", _talla_camisa = "", _LB_talla_camisa = "", _talla_camisa_sport = "", _LB_talla_camisa_sport = "";

        public byte[] _img_cli;

        public editarCliente(dynamic tabitem1)
        {
            InitializeComponent();
            tabitem = tabitem1;

            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserTag1;

            this.MinWidth = 1000;
            this.MinHeight = 500;
            this.MaxWidth = 1000;
            this.MaxHeight = 500;


            LoadConfig();
            bloquearCampos(false);



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

                    if (tag == "MmMae_iden")
                    {
                        cmptabla = tag; cmpcodigo = "cod_tdoc"; cmpnombre = "UPPER(nom_tdoc)"; cmporden = "cod_tdoc"; cmpidrow = "cod_tdoc"; cmptitulo = "Maestra de Identificacion"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_cargo")
                    {
                        cmptabla = tag; cmpcodigo = "cod_cargo"; cmpnombre = "UPPER(nom_cargo)"; cmporden = "cod_cargo"; cmpidrow = "cod_cargo"; cmptitulo = "Maestra de Cargo"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_profesion")
                    {
                        cmptabla = tag; cmpcodigo = "cod_prof"; cmpnombre = "UPPER(nom_prof)"; cmporden = "cod_prof"; cmpidrow = "cod_prof"; cmptitulo = "Maestra de Profesion"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_ocupacion")
                    {
                        cmptabla = tag; cmpcodigo = "cod_ocup"; cmpnombre = "UPPER(nom_ocup)"; cmporden = "cod_ocup"; cmpidrow = "cod_ocup"; cmptitulo = "Maestra de Ocupacion"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_ActEmp")
                    {
                        cmptabla = tag; cmpcodigo = "cod_actEmp"; cmpnombre = "UPPER(nom_actEmp)"; cmporden = "cod_actEmp"; cmpidrow = "cod_actEmp"; cmptitulo = "Maestra de Actividad de Empresas"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_talla1")
                    {
                        cmptabla = "CrMae_talla"; cmpcodigo = "cod_talla"; cmpnombre = "UPPER(nom_talla)"; cmporden = "cod_talla"; cmpidrow = "cod_talla"; cmptitulo = "Maestra de Talla"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_talla2")
                    {
                        cmptabla = "CrMae_talla"; cmpcodigo = "cod_talla"; cmpnombre = "UPPER(nom_talla)"; cmporden = "cod_talla"; cmpidrow = "cod_talla"; cmptitulo = "Maestra de Talla"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_talla3")
                    {
                        cmptabla = "CrMae_talla"; cmpcodigo = "cod_talla"; cmpnombre = "UPPER(nom_talla)"; cmporden = "cod_talla"; cmpidrow = "cod_talla"; cmptitulo = "Maestra de Talla"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_talla4")
                    {
                        cmptabla = "CrMae_talla"; cmpcodigo = "cod_talla"; cmpnombre = "UPPER(nom_talla)"; cmporden = "cod_talla"; cmpidrow = "cod_talla"; cmptitulo = "Maestra de Talla"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_talla5")
                    {
                        cmptabla = "CrMae_talla"; cmpcodigo = "cod_talla"; cmpnombre = "UPPER(nom_talla)"; cmporden = "cod_talla"; cmpidrow = "cod_talla"; cmptitulo = "Maestra de Talla"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
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
                        if (tag == "MmMae_iden")
                        {
                            LB_docu.Text = code; TextBx_docu.Text = nom.Trim();
                        }
                        if (tag == "CrMae_cargo")
                        {
                            LB_cod_car.Text = code; TextBxCB_cod_car.Text = nom.Trim();
                        }
                        if (tag == "CrMae_profesion")
                        {
                            LB_cod_pro.Text = code; TextBxCB_cod_pro.Text = nom.Trim(); ;
                        }
                        if (tag == "CrMae_ocupacion")
                        {
                            LB_cod_ocup.Text = code; TextBxCB_cod_ocup.Text = nom.Trim(); ;
                        }
                        if (tag == "CrMae_ActEmp")
                        {
                            LB_act_emp.Text = code; TextBx_act_emp.Text = nom.Trim(); ;
                        }
                        if (tag == "CrMae_talla1") {
                            LB_zap_ten.Text = code; TextBx_zap_ten.Text = nom.Trim(); ;
                        }
                        if (tag == "CrMae_talla2")
                        {
                            LB_pan_fal.Text = code; TextBx_pan_fal.Text = nom.Trim(); ;
                        }
                        if (tag == "CrMae_talla3")
                        {
                            LB_ves_tra.Text = code; TextBx_ves_tra.Text = nom.Trim(); ;
                        }
                        if (tag == "CrMae_talla4")
                        {
                            LB_camisa.Text = code; TextBx_camisa.Text = nom.Trim(); ;
                        }
                        if (tag == "CrMae_talla5")
                        {
                            LB_camisa_sp.Text = code; TextBx_camisa_sp.Text = nom.Trim(); ;
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

        private void TXB_Busqueda_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string tag = ((TextBox)sender).Tag.ToString();
                int idr = 0; string code = ""; string nombre = "";
                dynamic xx = "";

                if (tag == "MmMae_muni")
                {
                    xx = SiaWin.WindowBuscar("MmMae_muni", "cod_muni", "UPPER(nom_muni)", "cod_muni", "idrow", "Maestra De Ciudades", cnEmp, false, "");
                }
                if (tag == "MmMae_depa")
                {
                    xx = SiaWin.WindowBuscar("MmMae_depa", "cod_dep", "UPPER(nom_dep)", "cod_dep", "idrow", "Maestra De Departamento", cnEmp, false, "");
                }

                xx.ShowInTaskbar = false;
                xx.Owner = Application.Current.MainWindow;
                xx.ShowDialog();
                idr = xx.IdRowReturn;
                code = xx.Codigo;
                nombre = xx.Nombre;
                xx = null;

                if (idr > 0)
                {
                    if (tag == "MmMae_muni")
                    {
                        LB_cod_muni.Text = code; TextBx_cod_muni.Text = nombre.Trim(); ;
                    }
                    if (tag == "MmMae_depa") {
                        LB_cod_depa.Text = code; TextBx_cod_depa.Text = nombre.Trim(); ;
                    }




                }
                if (string.IsNullOrEmpty(code)) e.Handled = false;
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //valiudacion de campos ----------------------------------------------------

        public void bloquearCampos(Boolean direc)
        {
            TextBx_docu.IsEnabled = false;
            TextBx_num_docu.IsEnabled = false;
            TextBx_nom1.IsEnabled = false;
            TextBx_nom2.IsEnabled = false;
            TextBx_apell1.IsEnabled = false;
            TextBx_apell2.IsEnabled = false;
            TextBx_tel1.IsEnabled = false;
            TextBx_tel2.IsEnabled = false;
            TextBx_cel.IsEnabled = false;
            TextBx_email.IsEnabled = false;
            TextBx_direcc.IsEnabled = direc;
            TextBx_direcc2.IsEnabled = direc;
            TextBx_cod_muni.IsEnabled = false;
            TextBx_cod_depa.IsEnabled = false;
            TextBx_descripcion.IsEnabled = false;
            TextBx_fecha_nac.IsEnabled = false;
            TextBxCB_genero.IsEnabled = false;
            TextBxCB_est_civ.IsEnabled = false;
            TextBx_nom_emp.IsEnabled = false;
            TextBx_act_emp.IsEnabled = false;
            TextBxCB_ct_cel.IsEnabled = false;
            TextBxCB_ct_email.IsEnabled = false;
            TextBxCB_ct_corres.IsEnabled = false;
            TextBxCB_ct_whats.IsEnabled = false;
            TextBxCB_ct_sms.IsEnabled = false;
            TextBxCB_cod_car.IsEnabled = false;
            TextBxCB_cod_pro.IsEnabled = false;
            TextBxCB_cod_ocup.IsEnabled = false;
            TextBx_hobbies.IsEnabled = false;


            actualiza.IsEnabled = false;
        }

        public void habilitarCampos()
        {
            TextBx_docu.IsEnabled = true;
            TextBx_num_docu.IsEnabled = true;
            TextBx_nom1.IsEnabled = true;
            TextBx_nom2.IsEnabled = true;
            TextBx_apell1.IsEnabled = true;
            TextBx_apell2.IsEnabled = true;
            TextBx_tel1.IsEnabled = true;
            TextBx_tel2.IsEnabled = true;
            TextBx_cel.IsEnabled = true;
            TextBx_email.IsEnabled = true;
            TextBx_direcc.IsEnabled = true;
            TextBx_direcc2.IsEnabled = true;
            TextBx_cod_muni.IsEnabled = true;
            TextBx_cod_depa.IsEnabled = true;
            TextBx_descripcion.IsEnabled = true;
            TextBx_fecha_nac.IsEnabled = true;
            TextBxCB_genero.IsEnabled = true;
            TextBxCB_est_civ.IsEnabled = true;
            TextBx_nom_emp.IsEnabled = true;
            TextBx_act_emp.IsEnabled = true;
            TextBxCB_ct_cel.IsEnabled = true;
            TextBxCB_ct_email.IsEnabled = true;
            TextBxCB_ct_corres.IsEnabled = true;
            TextBxCB_ct_whats.IsEnabled = true;
            TextBxCB_ct_sms.IsEnabled = true;
            TextBxCB_cod_car.IsEnabled = true;
            TextBxCB_cod_pro.IsEnabled = true;
            TextBxCB_cod_ocup.IsEnabled = true;
            TextBx_hobbies.IsEnabled = true;

            actualiza.IsEnabled = true;

        }


        //direcion*************************************************

        string TagDireccion = "";
        private void TextBx_direcc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TagDireccion = ((TextBox)sender).Tag.ToString();


            if (TagDireccion == "Dir1")
            {
                TX_direccion.Text = TextBx_direcc.Text;
            }
            if (TagDireccion == "Dir2")
            {
                TX_direccion.Text = TextBx_direcc2.Text;
            }

            panel.Opacity = 0.1;
            panel.IsEnabled = false;
            PAneldireccion.Visibility = Visibility.Visible;

        }

        private void salir_panel_direccion(object sender, RoutedEventArgs e)
        {
            panel.Opacity = 1;
            panel.IsEnabled = true;
            PAneldireccion.Visibility = Visibility.Hidden;
        }
        private void regitrar_direccion(object sender, RoutedEventArgs e) {


            if (TagDireccion == "Dir1")
            {
                TextBx_direcc.Text = TX_direccion.Text;
                TX_direccion.Text = "";
            }
            if (TagDireccion == "Dir2")
            {
                TextBx_direcc2.Text = TX_direccion.Text;
                TX_direccion.Text = "";
            }
            panel.Opacity = 1;
            panel.IsEnabled = true;
            PAneldireccion.Visibility = Visibility.Hidden;
        }


        private void Agregar_Nomenclatura(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedTag = ((ComboBoxItem)CBOX_nomenclatura.SelectedItem).Tag.ToString();
                TX_direccion.Text = TX_direccion.Text + selectedTag;
            }
            catch (Exception w)
            {
                MessageBox.Show("error1:" + w);
            }
        }

        private void Agregar_Digitos_Letras(object sender, RoutedEventArgs e) {
            try
            {
                string texto = ((Button)sender).Content.ToString();
                TX_direccion.Text = TX_direccion.Text + texto;
            }
            catch (Exception w)
            {
                MessageBox.Show("error2:" + w);
            }

        }


        private void eliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string c = TX_direccion.Text.Remove(TX_direccion.Text.Length - 1);
                TX_direccion.Text = c;
            }
            catch (Exception)
            {


            }


        }

        private void space_Click(object sender, RoutedEventArgs e)
        {
            TX_direccion.Text = TX_direccion.Text + " ";
        }

        private void clean_Click(object sender, RoutedEventArgs e)
        {
            TX_direccion.Text = "";
        }

        //********************************************************

        private void ValidacionNumeros(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract || e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("este campo solo admite valores numericos");
                e.Handled = true;
            }
        }

        //convertir campos -------------------------------------------------

        public string convertirCT(string _ct)
        {
            string a = "";

            switch (_ct)
            {
                case "0":
                    a = "NO";
                    break;
                case "1":
                    a = "SI";
                    break;
                default:
                    a = "";
                    break;
            }

            return a;
        }

        public string convertirCTCodi(string _ct)
        {
            string valor = "";
            switch (_ct)
            {
                case "SI":
                    valor = "1";
                    break;
                case "NO":
                    valor = "0";
                    break;
                default:
                    valor = "";
                    break;
            }
            return valor;
        }

        public string convertir_Est_civ(string _ec)
        {
            string valor = "";
            switch (_ec)
            {
                case "1":
                    valor = "SOLTERO";
                    break;
                case "2":
                    valor = "CASADO";
                    break;
                case "3":
                    valor = "UNION LIBRE";
                    break;
                case "4":
                    valor = "SEPARADO";
                    break;
                case "5":
                    valor = "VIUDO";
                    break;
                default:
                    valor = "";
                    break;
            }
            return valor;
        }

        public string convertir_Est_civCodi(string _ec)
        {
            string valor = "";
            switch (_ec)
            {
                case "SOLTERO":
                    valor = "1";
                    break;
                case "CASADO":
                    valor = "2";
                    break;
                case "UNION LIBRE":
                    valor = "3";
                    break;
                case "SEPARADO":
                    valor = "4";
                    break;
                case "VIUDO":
                    valor = "5";
                    break;
                default:
                    valor = "";
                    break;
            }
            return valor;
        }


        //query------------------------------------------------------------------------------


        public byte[] imgByteArr = null;

        private void actualizar_Click(object sender, RoutedEventArgs e)
        {
            //convierte a 1 o 0 para ingresarlo en la bd			
            LB_est_civ.Text = convertir_Est_civCodi(TextBxCB_est_civ.Text);
            LB_ct_cel.Text = convertirCTCodi(TextBxCB_ct_cel.Text);
            LB_ct_email.Text = convertirCTCodi(TextBxCB_ct_email.Text);
            LB_ct_corres.Text = convertirCTCodi(TextBxCB_ct_corres.Text);
            LB_ct_whats.Text = convertirCTCodi(TextBxCB_ct_whats.Text);
            LB_ct_sms.Text = convertirCTCodi(TextBxCB_ct_sms.Text);

            
            
            

            if (imageSave == true)
            {
                FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                imgByteArr = new byte[fs.Length];
                fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                fs.Close();
            }
                        
            if (_cod_ter.Length > 0)
            {


                try
                { 
                    string fecha = ",";
                    if (TextBx_fecha_nac.Text.Length > 0)
                    {
                        fecha = ",COMAE_TER.fec_cump = '" + TextBx_fecha_nac.Text + "', " ;
                    }
                    
                    string queryUPD_TER = " UPDATE COMAE_TER SET COMAE_TER.tdoc = '" + LB_docu.Text + "', COMAE_TER.nom1 = '" + TextBx_nom1.Text + "',COMAE_TER.nom2 = '" + TextBx_nom2.Text + "',COMAE_TER.apell1 = '" + TextBx_apell1.Text + "',COMAE_TER.apell2 = '" + TextBx_apell2.Text + "',COMAE_TER.tel1 = '" + TextBx_tel1.Text + "',COMAE_TER.tel2 = '" + TextBx_tel2.Text + "',COMAE_TER.cel = '" + TextBx_cel.Text + "',COMAE_TER.email = '" + TextBx_email.Text + "',COMAE_TER.dir = '" + TextBx_direcc.Text + "',COMAE_TER.dir2 = '" + TextBx_direcc2.Text + "',COMAE_TER.cod_ciu = '" + LB_cod_muni.Text + "',COMAE_TER.cod_depa = '" + LB_cod_depa.Text + "' " + fecha + " COMAE_TER.observ = '" + TextBx_descripcion.Text + "'  WHERE COMAE_TER.cod_ter = '" + _cod_ter + "' ";                    
                    SiaWin.Func.SqlDT(queryUPD_TER, "Clientes", idemp);

                    
                    using (SqlConnection connection = new SqlConnection(SiaWin.Func.DatosEmp(idemp)))
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        string cad_ima = "";
                        if (imageSave == true)
                        {
                            cad_ima = ",image_name=@image_name,img_cli=@img_cli,";
                        }
                        else
                        {
                            cad_ima =  ",";
                        }

                    
                        cmd.CommandText = "UPDATE CrMae_cli SET CrMae_cli.genero=@genero,CrMae_cli.est_civil=@est_civil,CrMae_cli.nom_emp=@nom_emp,CrMae_cli.act_emp=@act_emp,CrMae_cli.ct_cel=@ct_cel,CrMae_cli.ct_email=@ct_email,CrMae_cli.ct_whats=@ct_whats,CrMae_cli.ct_sms=@ct_sms,CrMae_cli.ct_corres=@ct_corres,CrMae_cli.cod_cargo=@cod_cargo,CrMae_cli.cod_ocup=@cod_ocup,CrMae_cli.cod_prof=@cod_prof,CrMae_cli.hobbies=@hobbies,CrMae_cli.num_doc=@num_doc"+ cad_ima + "ran_edad=@ran_edad,talla_zap_tenn=@talla_zap_tenn,talla_pant_fald=@talla_pant_fald,talla_vest_traj=@talla_vest_traj,talla_camisa=@talla_camisa,talla_camisa_sport=@talla_camisa_sport   WHERE CrMae_cli.cod_ter = '" + _cod_ter + "' ";                        

                        cmd.Parameters.AddWithValue("@genero", TextBxCB_genero.Text);
                        cmd.Parameters.AddWithValue("@est_civil", LB_est_civ.Text);
                        cmd.Parameters.AddWithValue("@nom_emp", TextBx_nom_emp.Text);                        
                        cmd.Parameters.AddWithValue("@act_emp", LB_act_emp.Text);
                        cmd.Parameters.AddWithValue("@ct_cel", LB_ct_cel.Text);
                        cmd.Parameters.AddWithValue("@ct_email", LB_ct_email.Text);
                        cmd.Parameters.AddWithValue("@ct_whats", LB_ct_whats.Text);
                        cmd.Parameters.AddWithValue("@ct_sms", LB_ct_sms.Text);                          
                        cmd.Parameters.AddWithValue("@ct_corres", LB_ct_corres.Text);
                        cmd.Parameters.AddWithValue("@cod_cargo", LB_cod_car.Text);
                        cmd.Parameters.AddWithValue("@cod_ocup", LB_cod_ocup.Text);
                        cmd.Parameters.AddWithValue("@cod_prof", LB_cod_pro.Text);
                        cmd.Parameters.AddWithValue("@hobbies", TextBx_hobbies.Text);                    
                        cmd.Parameters.AddWithValue("@num_doc", TextBx_num_docu.Text);
                        
                        if (imageSave == true)
                        {
                            cmd.Parameters.AddWithValue("@image_name", strName);
                            cmd.Parameters.AddWithValue("@img_cli", imgByteArr);
                        }                        
                        
                        cmd.Parameters.AddWithValue("@ran_edad", TextBxCB_ran_eda.Text);
                        cmd.Parameters.AddWithValue("@talla_zap_tenn", LB_zap_ten.Text);
                        cmd.Parameters.AddWithValue("@talla_pant_fald", LB_pan_fal.Text);
                        cmd.Parameters.AddWithValue("@talla_vest_traj", LB_ves_tra.Text);                        
                        cmd.Parameters.AddWithValue("@talla_camisa", LB_camisa.Text);
                        cmd.Parameters.AddWithValue("@talla_camisa_sport", LB_camisa_sp.Text);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    
                    }

                    
                    MessageBox.Show("actualizacion exitosa");
                    
                    Puntos();
                    actualizarCamposPuntos();

                    bandera = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("erro update:" + ex);
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("error al actualizar cliente");
             
            }

        }

        //actualiza los campos para verificar el valor inicial y el final
        public void actualizarCamposPuntos()
        {
            //maestra de terceros ------------------------------------
            string cadenaTra = "SELECT rtrim(TER.tdoc) as tdoc, rtrim(UPPER(IDENTIFICACION.nom_tdoc)) as nom_tdoc, rtrim(TER.nom_ter) as nom_ter,rtrim(UPPER(TER.nom1)) as nom1,rtrim(UPPER(TER.nom2)) as nom2,rtrim(UPPER(TER.apell1)) as apell1,rtrim(UPPER(TER.apell2)) as apell2,rtrim(TER.tel1) as tel1,rtrim(TER.cel) as cel,rtrim(UPPER(TER.email)) as email,rtrim(UPPER(TER.dir)) as dir,rtrim(TER.cod_ciu) as cod_ciu, rtrim(UPPER(MUNICIPIO.nom_muni)) as nom_muni,rtrim(TER.cod_depa) as cod_depa, rtrim(UPPER(DEPARTAMENTO.nom_dep)) as nom_dep,CONVERT(varchar,fec_cump,103) as fec_cump, (cast(datediff(dd,TER.fec_cump,GETDATE()) / 365.25 as int)) as edad ";
            cadenaTra = cadenaTra + "FROM COMAE_TER as TER ";
            cadenaTra = cadenaTra + "full join MmMae_muni as MUNICIPIO on TER.cod_ciu = MUNICIPIO.cod_muni ";
            cadenaTra = cadenaTra + "full join MmMae_depa as DEPARTAMENTO on TER.cod_depa = DEPARTAMENTO.cod_dep ";
            cadenaTra = cadenaTra + "full join MmMae_iden as IDENTIFICACION on TER.tdoc = IDENTIFICACION.cod_tdoc ";
            cadenaTra = cadenaTra + "where TER.cod_ter = '" + _cod_ter + "' ";

            try
            {
                SqlDataReader drTra = SiaWin.Func.SqlDR(cadenaTra, idemp);
                while (drTra.Read())
                {
                    _documentoLB = drTra["tdoc"].ToString().Trim();
                    _nom1 = drTra["nom1"].ToString().Trim();
                    _nom2 = drTra["nom2"].ToString().Trim();
                    _appe1 = drTra["apell1"].ToString().Trim();
                    _appe2 = drTra["apell2"].ToString().Trim();
                    _tel1 = drTra["tel1"].ToString().Trim();
                    _cel = drTra["cel"].ToString().Trim();
                    _email = drTra["email"].ToString().Trim();
                    _dir = drTra["dir"].ToString().Trim();
                    _muniLB = drTra["cod_ciu"].ToString().Trim();
                    _fecha_nac = drTra["fec_cump"].ToString().Trim();
                    _documento = drTra["nom_tdoc"].ToString().Trim();
                    _muni = drTra["nom_muni"].ToString().Trim();
                }
                drTra.Close();
            }
            catch (Exception w)
            {
                MessageBox.Show("actualizarCamposPuntos()1:" + w);
            }

            //maestra de clientes ------------------------------------
            string cadenaCli = "SELECT rtrim(CLIE.num_doc) as num_doc, rtrim(CLIE.genero) as genero, rtrim(CLIE.est_civil) as est_civil,rtrim(UPPER(CLIE.nom_emp)) as nom_emp, rtrim(CLIE.ct_cel) as ct_cel,rtrim(CLIE.ct_email) as ct_email,rtrim(CLIE.ct_whats) as ct_whats,rtrim(CLIE.ct_sms) as ct_sms,rtrim(CLIE.ct_corres) as ct_corres, rtrim(UPPER(CARGO.cod_cargo)) as cod_cargo,rtrim(UPPER(CARGO.nom_cargo)) as nom_cargo, rtrim(UPPER(OCUPACION.cod_ocup)) as cod_ocup,rtrim(UPPER(OCUPACION.nom_ocup)) as nom_ocup, rtrim(UPPER(PROFESION.cod_prof)) as  cod_prof, rtrim(UPPER(PROFESION.nom_prof)) as nom_prof, rtrim(UPPER(CLIE.hobbies)) as hobbies, rtrim(UPPER(CLIE.ran_edad)) as ran_edad ";
            cadenaCli = cadenaCli + "FROM CrMae_cli as CLIE ";
            cadenaCli = cadenaCli + "full join CrMae_cargo as CARGO on CLIE.cod_cargo = CARGO.cod_cargo ";
            cadenaCli = cadenaCli + "full join CrMae_ocupacion as OCUPACION on CLIE.cod_ocup = OCUPACION.cod_ocup ";
            cadenaCli = cadenaCli + "full join CrMae_profesion as PROFESION on CLIE.cod_prof = PROFESION.cod_prof ";
            cadenaCli = cadenaCli + "where CLIE.cod_ter = '" + _cod_ter + "' ";

            try
            {
                SqlDataReader drCli = SiaWin.Func.SqlDR(cadenaCli, idemp);
                while (drCli.Read())
                {
                    _genero = drCli["genero"].ToString().Trim();
                    _est_civil = drCli["est_civil"].ToString().Trim();
                    _nom_emp = drCli["nom_emp"].ToString().Trim();
                    _ct_cel = drCli["ct_cel"].ToString().Trim();
                    _ct_email = drCli["ct_email"].ToString().Trim();
                    _ct_whats = drCli["ct_whats"].ToString().Trim();
                    _ct_sms = drCli["ct_sms"].ToString().Trim();
                    _ct_corres = drCli["ct_corres"].ToString().Trim();
                    _cod_cargoLB = drCli["cod_cargo"].ToString().Trim();
                    _cod_ocupLB = drCli["cod_ocup"].ToString().Trim();
                    _cod_profLB = drCli["cod_prof"].ToString().Trim();
                    _num_doc = drCli["num_doc"].ToString().Trim();
                    _hobbies = drCli["hobbies"].ToString().Trim();
                    _cod_cargo = drCli["nom_cargo"].ToString().Trim();
                    _cod_ocup = drCli["nom_ocup"].ToString().Trim();
                    _cod_prof = drCli["nom_prof"].ToString().Trim();
                    _ran_edad = drCli["ran_edad"].ToString().Trim();
                }
                drCli.Close();
            }
            catch (Exception w)
            {
                MessageBox.Show("actualizarCamposPuntos()2:" + w);
            }

        }

        //puntos ---------------------------------------------------

        string cadenaPuntos = "TIENES PUNTOS POR LA ACTUALIZACION DE : ";

        int sumaPuntos = 0;

        public void traerPuntos()
        {
            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("CRMpuntos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();


                try
                {
                    if (LB_docu.Text != _documentoLB && LB_docu.Text.Length > 0){
                        _documentoLB_p = Convert.ToInt32(ds.Tables[0].Rows[0]["porcentaje"]);_documentoLB_c = ds.Tables[0].Rows[0]["cod_punto"].ToString();
                        sumaPuntos = sumaPuntos + System.Convert.ToInt32(_documentoLB_p);
                        cadenaPuntos = cadenaPuntos + "- tipo de documento ";
                    }
                }catch (Exception){MessageBox.Show("1");}


                try
                {
                    if (TextBx_num_docu.Text != _num_doc && TextBx_num_docu.Text.Length > 0){
                        _num_doc_p = Convert.ToInt32(ds.Tables[1].Rows[0]["porcentaje"]);_num_doc_c = ds.Tables[1].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + System.Convert.ToInt32(_num_doc_p);
                        cadenaPuntos = cadenaPuntos + "- numero de documento ";
                    }
                }catch (Exception) { MessageBox.Show("2");}


                try
                {
                    if (TextBx_nom1.Text != _nom1 && TextBx_nom1.Text.Length > 0)
                    {
                        _nom1_p = Convert.ToInt32(ds.Tables[2].Rows[0]["porcentaje"]); _nom1_c = ds.Tables[2].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + System.Convert.ToInt32(_nom1_p);
                        cadenaPuntos = cadenaPuntos + "- primer nombre ";
                    }
                }catch (Exception) { MessageBox.Show("3"); }

                try {
                    if (TextBx_apell1.Text != _appe1 && TextBx_apell1.Text.Length > 0) {
                        _appe1_p = Convert.ToInt32(ds.Tables[3].Rows[0]["porcentaje"]); _appe1_c = ds.Tables[3].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + System.Convert.ToInt32(_appe1_p);
                        cadenaPuntos = cadenaPuntos + "- primer apellido ";
                    }
                }catch (Exception){MessageBox.Show("4");}

                try {
                    if (TextBxCB_genero.Text != _genero && TextBxCB_genero.Text.Length > 0) {
                        _genero_p = Convert.ToInt32(ds.Tables[4].Rows[0]["porcentaje"]); _genero_c = ds.Tables[4].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + System.Convert.ToInt32(_genero_p);
                        cadenaPuntos = cadenaPuntos + "- genero ";
                    }
                }catch (Exception) { MessageBox.Show("5"); }


                try{
                    if (TextBx_tel1.Text != _tel1 && TextBx_tel1.Text.Length > 0){
                        _tel_p = Convert.ToInt32(ds.Tables[5].Rows[0]["porcentaje"]); _tel_c = ds.Tables[5].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + System.Convert.ToInt32(_tel_p);
                        cadenaPuntos = cadenaPuntos + "- telefono ";
                    }
                }catch (Exception) { MessageBox.Show("6"); }

                try {
                    if (TextBx_cel.Text != _cel && TextBx_cel.Text.Length > 0)
                    {
                        _cel_p = Convert.ToInt32(ds.Tables[6].Rows[0]["porcentaje"]); _cel_c = ds.Tables[6].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _cel_p;
                        cadenaPuntos = cadenaPuntos + "- celular ";
                    }                        
                }catch (Exception) { MessageBox.Show("7"); }

                try {
                    if (TextBx_email.Text != _email && TextBx_email.Text.Length > 0) {
                        _email_p = Convert.ToInt32(ds.Tables[7].Rows[0]["porcentaje"]); _email_c = ds.Tables[7].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _email_p;
                        cadenaPuntos = cadenaPuntos + "- email ";
                    }
                }catch (Exception) { MessageBox.Show("8"); }

                try
                {
                    if (TextBx_fecha_nac.Text.Length > 0){
                        string fecha_cumpleano = TextBx_fecha_nac.SelectedDate.Value.ToString("dd/MM/yyyy");
                        if (fecha_cumpleano != _fecha_nac.Trim() && TextBx_fecha_nac.Text.Length > 0)
                        {
                            _fecha_nac_p = Convert.ToInt32(ds.Tables[8].Rows[0]["porcentaje"]); _fecha_nac_c = ds.Tables[8].Rows[0]["cod_punto"].ToString().Trim();
                            sumaPuntos = sumaPuntos + _fecha_nac_p;
                            cadenaPuntos = cadenaPuntos + "- fecha de nacimiento ";                            
                        }
                    }

                }catch (Exception) { MessageBox.Show("9"); }

                try{
                    if (TextBx_direcc.Text != _dir && TextBx_direcc.Text.Length > 0){
                        _dir_p = Convert.ToInt32(ds.Tables[9].Rows[0]["porcentaje"]); _dir_c = ds.Tables[9].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _dir_p;
                        cadenaPuntos = cadenaPuntos + "- direccion ";                        
                    }
                }catch (Exception){MessageBox.Show("10");}

                try{
                    if (LB_cod_muni.Text != _muniLB && LB_cod_muni.Text.Length > 0) {
                        _muniLB_p = Convert.ToInt32(ds.Tables[10].Rows[0]["porcentaje"]); _muniLB_c = ds.Tables[10].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _muniLB_p;
                        cadenaPuntos = cadenaPuntos + "- ciudad ";
                    }
                }catch (Exception){MessageBox.Show("11");}


                try{
                    if (TextBxCB_est_civ.Text != convertir_Est_civ(_est_civil) && TextBxCB_est_civ.Text.Length > 0) {
                        _est_civil_p = Convert.ToInt32(ds.Tables[11].Rows[0]["porcentaje"]); _est_civil_c = ds.Tables[11].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _est_civil_p;
                        cadenaPuntos = cadenaPuntos + "- estado civil ";
                    }                    
                }
                catch (Exception){MessageBox.Show("12");}

                try{
                    if (LB_cod_car.Text.Trim() != _cod_cargoLB.Trim() && LB_cod_car.Text.Length > 0) {
                        _cod_cargoLB_p = Convert.ToInt32(ds.Tables[12].Rows[0]["porcentaje"]); _cod_cargoLB_c = ds.Tables[12].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _cod_cargoLB_p;
                        cadenaPuntos = cadenaPuntos + "- cargo ";
                    }                    
                }
                catch (Exception){MessageBox.Show("13");}

                try
                {
                    if (LB_cod_pro.Text.Trim() != _cod_profLB.Trim() && LB_cod_pro.Text.Length > 0)
                    {
                        _cod_profLB_p = Convert.ToInt32(ds.Tables[13].Rows[0]["porcentaje"]); _cod_profLB_c = ds.Tables[13].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _cod_profLB_p;
                        cadenaPuntos = cadenaPuntos + "- profesion ";
                    }
                }
                catch (Exception) { MessageBox.Show("14"); }

                try{
                    if (LB_cod_ocup.Text.Trim() != _cod_ocupLB.Trim() && LB_cod_ocup.Text.Length > 0){
                        _cod_ocupLB_p = Convert.ToInt32(ds.Tables[14].Rows[0]["porcentaje"]); _cod_ocupLB_c = ds.Tables[14].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _cod_ocupLB_p;
                        cadenaPuntos = cadenaPuntos + "- ocupacion ";                        
                    }
                }catch (Exception){MessageBox.Show("15");}

                try{
                    if (TextBx_nom_emp.Text != _nom_emp && TextBx_nom_emp.Text.Length > 0)
                    {
                        _nom_emp_p = Convert.ToInt32(ds.Tables[15].Rows[0]["porcentaje"]); _nom_emp_c = ds.Tables[15].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _nom_emp_p;
                        cadenaPuntos = cadenaPuntos + "- nombre de la empresa ";
                    }
                }catch (Exception){MessageBox.Show("16");}


                try
                {
                    if (TextBx_hobbies.Text != _hobbies && TextBx_hobbies.Text.Length > 0)
                    {
                        _hobbies_p = Convert.ToInt32(ds.Tables[16].Rows[0]["porcentaje"]); _hobbies_c = ds.Tables[16].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _hobbies_p;
                        cadenaPuntos = cadenaPuntos + "- hobbies ";
                    }
                }
                catch (Exception) { MessageBox.Show("17"); }


                try {
                    if (TextBxCB_ct_cel.Text != convertirCT(_ct_cel) && TextBxCB_ct_cel.Text.Length > 0)
                    {
                        _ct_cel_p = Convert.ToInt32(ds.Tables[17].Rows[0]["porcentaje"]); _ct_cel_c = ds.Tables[17].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _ct_cel_p;
                        cadenaPuntos = cadenaPuntos + "- contacto por celular ";                        
                    }
                }catch (Exception) { MessageBox.Show("18"); }

                try {
                    if (TextBxCB_ct_email.Text != convertirCT(_ct_email) && TextBxCB_ct_email.Text.Length > 0)
                    {
                        _ct_email_p = Convert.ToInt32(ds.Tables[18].Rows[0]["porcentaje"]); _ct_email_c = ds.Tables[18].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _ct_email_p;
                        cadenaPuntos = cadenaPuntos + "- contacto por email ";                        
                    }
                }catch (Exception) { MessageBox.Show("19"); }

                try {
                    if (TextBxCB_ct_corres.Text != convertirCT(_ct_corres) && TextBxCB_ct_corres.Text.Length > 0)
                    {
                        _ct_corres_p = Convert.ToInt32(ds.Tables[19].Rows[0]["porcentaje"]); _ct_corres_c = ds.Tables[19].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _ct_corres_p;
                        cadenaPuntos = cadenaPuntos + "- contacto por correspondensia ";                        
                    }
                }catch (Exception) { MessageBox.Show("20"); }

                try {
                    if (TextBxCB_ct_whats.Text != convertirCT(_ct_whats) && TextBxCB_ct_whats.Text.Length > 0) {
                        _ct_whats_p = Convert.ToInt32(ds.Tables[20].Rows[0]["porcentaje"]); _ct_whats_c = ds.Tables[20].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _ct_whats_p;
                        cadenaPuntos = cadenaPuntos + "- contacto por WhatsApp ";                        
                    }
                }catch (Exception) { MessageBox.Show("21"); }

                try {
                    if (TextBxCB_ct_sms.Text != convertirCT(_ct_sms) && TextBxCB_ct_sms.Text.Length > 0) {
                        _ct_sms_p = Convert.ToInt32(ds.Tables[21].Rows[0]["porcentaje"]); _ct_sms_c = ds.Tables[21].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _ct_sms_p;
                        cadenaPuntos = cadenaPuntos + "- contacto por SMS ";
                    }
                }catch (Exception) { MessageBox.Show("22"); }

                try {
                    if (TextBxCB_ran_eda.Text != _ran_edad && TextBxCB_ran_eda.Text.Length > 0) {
                        _ran_edad_p = Convert.ToInt32(ds.Tables[22].Rows[0]["porcentaje"]); _ran_edad_c = ds.Tables[22].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + _ran_edad_p;
                        cadenaPuntos = cadenaPuntos + "- rango de edad ";                        
                    }
                }catch (Exception) { MessageBox.Show("23"); }


                try
                {
                    if (TextBx_nom2.Text != _nom2 && TextBx_nom2.Text.Length > 0)
                    {
                        _nom2_p = Convert.ToInt32(ds.Tables[23].Rows[0]["porcentaje"]); _nom2_c = ds.Tables[23].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + System.Convert.ToInt32(_nom2_p);
                        cadenaPuntos = cadenaPuntos + "- segundo nombre ";
                    }
                }
                catch (Exception) { MessageBox.Show("24"); }


                try
                {
                    if (TextBx_apell2.Text != _appe2 && TextBx_apell2.Text.Length > 0)
                    {
                        _appe2_p = Convert.ToInt32(ds.Tables[24].Rows[0]["porcentaje"]); _appe2_c = ds.Tables[24].Rows[0]["cod_punto"].ToString().Trim();
                        sumaPuntos = sumaPuntos + System.Convert.ToInt32(_appe2_p);
                        cadenaPuntos = cadenaPuntos + "- segundo apellido ";
                    }
                }
                catch (Exception) { MessageBox.Show("25"); }

            }
            catch (Exception w)
            {
                MessageBox.Show("traerPuntos():" + w);
            }
        }


        public string insertPuntos(string codigoP,int punto, string valAnt, string valNew) {
            string cadena = "insert into CrAct_info(cod_ter, cod_mer, fecha_reg, cod_punto, porcentaje, val_ini, val_fin) values('" + _cod_ter + "','" + codigoUsuario + "','" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "','" + codigoP + "','" + punto + "','" + valAnt + "','" + valNew + "')"; 
            return cadena;            
        }
        
        public void Puntos()
        {
           
            traerPuntos();

            try
            {

                //tipo de documento
                if (LB_docu.Text != _documentoLB && LB_docu.Text.Length > 0) {                    
                    string queryPuntos = insertPuntos(_documentoLB_c, _documentoLB_p, _documento, TextBx_docu.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //numero documento
                if (TextBx_num_docu.Text != _num_doc && TextBx_num_docu.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_num_doc_c, _num_doc_p, _num_doc, TextBx_num_docu.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }


                //primer nombre 1
                if (TextBx_nom1.Text != _nom1 && TextBx_nom1.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_nom1_c, _nom1_p, _nom1, TextBx_nom1.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);

                }
                //primer apellido 1
                if (TextBx_apell1.Text != _appe1 && TextBx_apell1.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_appe1_c, _appe1_p, _appe1, TextBx_apell1.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //genero
                if (TextBxCB_genero.Text != _genero && TextBxCB_genero.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_genero_c, _genero_p, _genero, TextBxCB_genero.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //telefono            
                if (TextBx_tel1.Text != _tel1 && TextBx_tel1.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_tel_c, _tel_p, _tel1, TextBx_tel1.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //celular
                if (TextBx_cel.Text != _cel && TextBx_cel.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_cel_c, _cel_p, _cel, TextBx_cel.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }
                //email
                if (TextBx_email.Text != _email && TextBx_email.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_email_c, _email_p, _email, TextBx_email.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //fecha naciemiento
                if (TextBx_fecha_nac.Text.Length > 0)
                {
                    string fecha_cumpleano = TextBx_fecha_nac.SelectedDate.Value.ToString("dd/MM/yyyy");
                    if (fecha_cumpleano != _fecha_nac.Trim() && TextBx_fecha_nac.Text.Length > 0) {
                        string queryPuntos = insertPuntos(_fecha_nac_c, _fecha_nac_p, _fecha_nac.Trim(), fecha_cumpleano);
                        SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp); }
                }

                //direccion
                if (TextBx_direcc.Text != _dir && TextBx_direcc.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_dir_c, _dir_p , _dir, TextBx_direcc.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //municipio                
                if (LB_cod_muni.Text != _muniLB && LB_cod_muni.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_muniLB_c, _muniLB_p, _muni, TextBx_cod_muni.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //estado civil
                if (TextBxCB_est_civ.Text != convertir_Est_civ(_est_civil) && TextBxCB_est_civ.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_est_civil_c, _est_civil_p, convertir_Est_civ(_est_civil), TextBxCB_est_civ.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //cargo
                if (LB_cod_car.Text.Trim() != _cod_cargoLB.Trim() && LB_cod_car.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_cod_cargoLB_c, _cod_cargoLB_p, _cod_cargo.Trim(), TextBxCB_cod_car.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //profesion
                if (LB_cod_pro.Text.Trim() != _cod_profLB.Trim() && LB_cod_pro.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_cod_profLB_c, _cod_profLB_p, _cod_prof.Trim(), TextBxCB_cod_pro.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //ocupacion
                if (LB_cod_ocup.Text.Trim() != _cod_ocupLB.Trim() && LB_cod_ocup.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_cod_ocupLB_c, _cod_ocupLB_p, _cod_ocup.Trim(), TextBxCB_cod_ocup.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //nombre empresa
                if (TextBx_nom_emp.Text != _nom_emp && TextBx_nom_emp.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_nom_emp_c, _nom_emp_p, _nom_emp.Trim(), TextBx_nom_emp.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp); }                                

                //hobbies
                if (TextBx_hobbies.Text != _hobbies && TextBx_hobbies.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_hobbies_c, _hobbies_p, _hobbies.Trim(), TextBx_hobbies.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }

                //ct_cel
                if (TextBxCB_ct_cel.Text != convertirCT(_ct_cel) && TextBxCB_ct_cel.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_ct_cel_c, _ct_cel_p, convertirCT(_ct_cel), TextBxCB_ct_cel.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }
                //ct_email
                if (TextBxCB_ct_email.Text != convertirCT(_ct_email) && TextBxCB_ct_email.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_ct_email_c, _ct_email_p, convertirCT(_ct_email), TextBxCB_ct_email.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }
                //ct_corres
                if (TextBxCB_ct_corres.Text != convertirCT(_ct_corres) && TextBxCB_ct_corres.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_ct_corres_c, _ct_corres_p, convertirCT(_ct_corres), TextBxCB_ct_corres.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }
                //ct_whats
                if (TextBxCB_ct_whats.Text != convertirCT(_ct_whats) && TextBxCB_ct_whats.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_ct_whats_c, _ct_whats_p, convertirCT(_ct_whats), TextBxCB_ct_whats.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp); }
                //ct_sms
                if (TextBxCB_ct_sms.Text != convertirCT(_ct_sms) && TextBxCB_ct_sms.Text.Length > 0) {
                    string queryPuntos = insertPuntos(_ct_sms_c, _ct_sms_p, convertirCT(_ct_sms), TextBxCB_ct_sms.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }
                //rango de edad
                if (TextBxCB_ran_eda.Text != _ran_edad && TextBxCB_ran_eda.Text.Length > 0){
                    string queryPuntos = insertPuntos(_ran_edad_c, _ran_edad_p, _ran_edad, TextBxCB_ran_eda.Text);                    
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }
                //segundo nombre 2
                if (TextBx_nom2.Text != _nom2 && TextBx_nom2.Text.Length > 0)
                {
                    string queryPuntos = insertPuntos(_nom2_c, _nom2_p, _nom2, TextBx_nom2.Text);
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);

                }
                //segundo apellido 2
                if (TextBx_apell2.Text != _appe2 && TextBx_apell2.Text.Length > 0)
                {
                    string queryPuntos = insertPuntos(_appe2_c, _appe2_p, _appe2, TextBx_apell2.Text);
                    SiaWin.Func.SqlDT(queryPuntos, "Punto", idemp);
                }


                if (cadenaPuntos.Length > 40)
                {
                    MessageBox.Show(cadenaPuntos + "\n \nTOTAL DE PUNTOS ACUMULADOS: " + sumaPuntos.ToString().Replace(".", "") + "%");
                    //se inicializa de nuevo 
                    cadenaPuntos = "TIENES PUNTOS POR LA ACTUALIZACION DE : ";
                    sumaPuntos = 0;
                    //se inicializa de nuevo                 
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("Puntos():" + w);
            }

        }

        bool imageSave = false;
        void Open_Image(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif;*.png)|*.jpg;*.bmp;*.gif;*.png";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName;
                    imageName = fldlg.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    image1.SetValue(System.Windows.Controls.Image.SourceProperty, isc.ConvertFromString(imageName));
                    imageSave = true;
                }
                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            TextBx_codigo.Text = _cod_ter;
            TextBx_docu.Text = _documento;
            LB_docu.Text = _documentoLB;
            TextBx_num_docu.Text = _num_doc;
            TextBx_nom1.Text = _nom1;
            TextBx_nom2.Text = _nom2;
            TextBx_apell1.Text = _appe1;
            TextBx_apell2.Text = _appe2;
            TextBx_tel1.Text = _tel1;
            TextBx_tel2.Text = _tel2;

            TextBx_cel.Text = _cel;
            TextBx_email.Text = _email;
            TextBx_direcc.Text = _dir;
            TextBx_direcc2.Text = _dir2;

            LB_cod_muni.Text = _muniLB;
            TextBx_cod_muni.Text = _muni;
            LB_cod_depa.Text = _depaLB;
            TextBx_cod_depa.Text = _depa;
            TextBx_descripcion.Text = _obser;
            TextBx_fecha_nac.Text = _fecha_nac;
            TextBxCB_genero.Text = _genero;
            TextBxCB_est_civ.Text = convertir_Est_civ(_est_civil);
            TextBx_nom_emp.Text = _nom_emp;
            TextBx_act_emp.Text = _act_emp;
            LB_act_emp.Text = _ct_celLB;

            TextBxCB_ct_cel.Text = convertirCT(_ct_cel);
            TextBxCB_ct_email.Text = convertirCT(_ct_email);
            TextBxCB_ct_corres.Text = convertirCT(_ct_corres);
            TextBxCB_ct_whats.Text = convertirCT(_ct_whats);
            TextBxCB_ct_sms.Text = convertirCT(_ct_sms);
            LB_cod_car.Text = _cod_cargoLB.Trim();
            TextBxCB_cod_car.Text = _cod_cargo;
            LB_cod_pro.Text = _cod_profLB;
            TextBxCB_cod_pro.Text = _cod_prof;
            LB_cod_ocup.Text = _cod_ocupLB;
            TextBxCB_cod_ocup.Text = _cod_ocup;
            TextBx_hobbies.Text = _hobbies;            
            TextBxCB_ran_eda.Text = _ran_edad;

            TextBx_zap_ten.Text = _talla_zap_tenn;
            LB_zap_ten.Text = _LB_talla_zap_tenn;
            TextBx_pan_fal.Text = _talla_pant_fald;
            LB_pan_fal.Text = _LB_talla_pant_fald;
            TextBx_ves_tra.Text = _talla_vest_traj;
            LB_ves_tra.Text = _LB_talla_vest_traj;
            TextBx_camisa.Text = _talla_camisa;
            LB_camisa.Text = _LB_talla_camisa;
            TextBx_camisa_sp.Text = _talla_camisa_sport;
            LB_camisa_sp.Text = _LB_talla_camisa_sport;

            try
            {
                byte[] blob = _img_cli;
                MemoryStream stream = new MemoryStream();
                stream.Write(blob, 0, blob.Length);
                stream.Position = 0;

                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();

                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();
                image1.Source = bi;
            }
            catch (Exception)
            {
                
            }


            //habilitacion de campos para editar
            habilitarCampos();

        }



    }
}
