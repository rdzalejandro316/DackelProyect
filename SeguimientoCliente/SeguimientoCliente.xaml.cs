using Microsoft.Win32;
using SeguimientoCliente;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SiasoftAppExt
{
    //Sia.PublicarPnt(9353,"SeguimientoCliente");
    //Sia.TabU(9353);
    public partial class SeguimientoCliente : UserControl
    {
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";
        public string Conexion;
        string codigoVendedor;
        string tipoUsuario;
        string bodega_vendedor;
        DataTable dt = new DataTable();


        public byte[] _img_cliPublica;


        public SeguimientoCliente(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;
            codigoVendedor = SiaWin._UserTag1;
            tipoUsuario = SiaWin._UserTag2;
            bodega_vendedor = SiaWin._UserTag;

            //SiaWin.Func.funcion();


            //carga los datos de los vendedores
            DatosUsuario();
            //cargar variales principales
            LoadConfig();

            SfSkinManager.SetVisualStyle(dataGridCxC, VisualStyles.Metro);
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
                tabitem.Title = "Seguimiento de cliente(" + aliasemp + ")";
                //TxtUser.Text = SiaWin._UserAlias;                

                //trae los datos del cliente
                if (tipoUsuario == "3" || tipoUsuario == "4")
                {
                    RefeshDataGrid(1);
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void DatosUsuario()
        {
            TxtUser.Text = SiaWin._UserTag1;
            TXTbodega.Text = bodega_vendedor;

            SqlDataReader drCli = SiaWin.Func.SqlDR("select * from InMae_mer where cod_mer='" + SiaWin._UserTag1 + "' ", idemp); ;

            while (drCli.Read())
            {
                TxtUserName.Text = drCli["nom_mer"].ToString().Trim();
            }

            drCli.Close();
            if (tipoUsuario == "1" || tipoUsuario == "2")
            {
                TxtTipUser.Text = "Administrador";
                panelBuscar.Visibility = Visibility.Visible;
            }
            else
            {
                TxtTipUser.Text = "Vendedor";
                panelBuscar.Visibility = Visibility.Hidden;
                //panelBuscar.IsEnabled = false;
            }

        }

        public void activarControles()
        {
            BtnEditar.IsEnabled = true;
            BtnSegCli.IsEnabled = true;
            BtnHisCom.IsEnabled = true;
            BtnSegComp.IsEnabled = true;
            BtnExpCli.IsEnabled = true;
        }

        //poner valores cada ves que se para sobre el usuario
        private void FirstDetailsViewGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            activarControles();
            try
            {
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];

                TBcliente.Text = row["nom_ter"].ToString();
                TBcodcliente.Text = row["cod_ter"].ToString();

                string cod_cliente = row["cod_ter"].ToString();
                CargarTallas(cod_cliente);
            }
            catch (Exception w)
            {
                // MessageBox.Show("error imagen:" + w);
                image1.Visibility = Visibility.Hidden;
            }

        }

        public void CargarTallas(string codter)
        {

            try
            {
                string cadena = "select talla_zap_tenn as cod_talla_1,talla1.nom_talla as talla1,talla_pant_fald as cod_talla_2, talla2.nom_talla as talla2,talla_vest_traj as cod_talla_3, talla3.nom_talla as talla3,talla_camisa as cod_talla_4, talla4.nom_talla as talla4,talla_camisa_sport as cod_talla_5, talla5.nom_talla as talla5,cliente.image_name as image_name, cliente.img_cli as img_cli from CrMae_cli as cliente ";
                cadena = cadena + "full join CrMae_talla as talla1 on cliente.talla_zap_tenn = talla1.cod_talla ";
                cadena = cadena + "full join CrMae_talla as talla2 on cliente.talla_pant_fald = talla2.cod_talla ";
                cadena = cadena + "full join CrMae_talla as talla3 on cliente.talla_vest_traj = talla3.cod_talla ";
                cadena = cadena + "full join CrMae_talla as talla4 on cliente.talla_camisa = talla4.cod_talla ";
                cadena = cadena + "full join CrMae_talla as talla5 on cliente.talla_camisa_sport = talla5.cod_talla ";
                cadena = cadena + "where cod_ter='" + codter + "' ";

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);

                CODtalla1.Text = dt.Rows[0]["cod_talla_1"].ToString();
                TBtalla1.Text = dt.Rows[0]["talla1"].ToString();

                CODtalla2.Text = dt.Rows[0]["cod_talla_2"].ToString();
                TBtalla2.Text = dt.Rows[0]["talla2"].ToString();

                CODtalla3.Text = dt.Rows[0]["cod_talla_3"].ToString();
                TBtalla3.Text = dt.Rows[0]["talla3"].ToString();

                CODtalla4.Text = dt.Rows[0]["cod_talla_4"].ToString();
                TBtalla4.Text = dt.Rows[0]["talla4"].ToString();

                CODtalla5.Text = dt.Rows[0]["cod_talla_5"].ToString();
                TBtalla5.Text = dt.Rows[0]["talla5"].ToString();


                image1.Visibility = Visibility.Visible;

                byte[] blob = (byte[])dt.Rows[0]["img_cli"];
                _img_cliPublica = (byte[])dt.Rows[0]["img_cli"];

                //byte[] blob = (byte[])row["img_cli"];
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
            catch (Exception w)
            {
                image1.Visibility = Visibility.Hidden;
                //MessageBox.Show("aqui:" + w);
            }


        }



        public void recargarImagen(byte[] blob)
        {
            try
            {
                image1.Visibility = Visibility.Visible;

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
        }
        // query ------------------------------------------------------------

        public void RefeshDataGrid(int tipo)
        {
            try
            {
                string queryGrid = "SELECT	rtrim(TER.cod_ter) as cod_ter, rtrim(TER.tdoc) as tdoc, rtrim(UPPER(IDENTIFICACION.nom_tdoc)) as nom_tdoc, rtrim(TER.nom_ter) as nom_ter,rtrim(UPPER(TER.nom1)) as nom1,rtrim(UPPER(TER.nom2)) as nom2,rtrim(UPPER(TER.apell1)) as apell1,rtrim(UPPER(TER.apell2)) as apell2,rtrim(TER.tel1) as tel1,rtrim(TER.tel2) as tel2,rtrim(TER.cel) as cel,rtrim(UPPER(TER.email)) as email,rtrim(UPPER(TER.dir1)) as dir1,rtrim(UPPER(TER.dir)) as dir,rtrim(UPPER(TER.dir2)) as dir2,rtrim(TER.cod_ciu) as cod_ciu, rtrim(UPPER(MUNICIPIO.nom_muni)) as nom_muni,rtrim(TER.cod_depa) as cod_depa, rtrim(UPPER(DEPARTAMENTO.nom_dep)) as nom_dep,CONVERT(varchar,fec_cump,103) as fec_cump, (cast(datediff(dd,TER.fec_cump,GETDATE()) / 365.25 as int)) as edad, ";
                queryGrid = queryGrid + "rtrim(CLIE.genero) as genero,IIF(est_civil ='1','SOLTERO',IIF(est_civil ='2','CASADO',IIF(est_civil ='3','UNION LIBRE',IIF(est_civil ='4','SEPARADO',IIF(est_civil ='5','VIUDO',''))))) AS est_civil,rtrim(UPPER(CLIE.nom_emp)) as nom_emp, rtrim(UPPER(CLIE.act_emp)) as act_emp,rtrim(UPPER(ACTIVIDAD.nom_actEmp)) as nom_actEmp,iif(CLIE.ct_cel='1','SI',iif(CLIE.ct_cel='0','NO','')) as ct_cel,iif(CLIE.ct_email='1','SI',iif(CLIE.ct_email='0','NO','')) as ct_email,iif(CLIE.ct_whats='1','SI',iif(CLIE.ct_whats='0','NO','')) as ct_whats,iif(CLIE.ct_sms='1','SI',iif(CLIE.ct_sms='0','NO','')) as ct_sms,iif(CLIE.ct_corres='1','SI',iif(CLIE.ct_corres='0','NO','')) as ct_corres,rtrim(UPPER(CARGO.cod_cargo)) as cod_cargo,rtrim(UPPER(CARGO.nom_cargo)) as nom_cargo, rtrim(UPPER(OCUPACION.cod_ocup)) as cod_ocup,rtrim(UPPER(OCUPACION.nom_ocup)) as nom_ocup, rtrim(UPPER(PROFESION.cod_prof)) as  cod_prof, rtrim(UPPER(PROFESION.nom_prof)) as nom_prof, rtrim(CLIE.num_doc) as num_doc, rtrim(UPPER(TER.observ)) as observ, rtrim(UPPER(CLIE.hobbies)) as hobbies, rtrim(CLIE.ran_edad) as ran_edad, rtrim(VENDEDOR.cod_mer) as cod_mer,rtrim(VENDEDOR.nom_mer) as nom_mer ";
                queryGrid = queryGrid + "FROM CrMae_cli as CLIE ";
                queryGrid = queryGrid + "full join CrMae_cargo as CARGO on CLIE.cod_cargo = CARGO.cod_cargo ";
                queryGrid = queryGrid + "full join CrMae_ocupacion as OCUPACION on CLIE.cod_ocup = OCUPACION.cod_ocup ";
                queryGrid = queryGrid + "full join CrMae_profesion as PROFESION on CLIE.cod_prof = PROFESION.cod_prof ";
                queryGrid = queryGrid + "full join CrMae_ActEmp as ACTIVIDAD  on ACTIVIDAD.cod_actEmp = CLIE.act_emp, ";
                queryGrid = queryGrid + "COMAE_TER as TER ";
                queryGrid = queryGrid + "left join MmMae_muni as MUNICIPIO on TER.cod_ciu = MUNICIPIO.cod_muni ";
                queryGrid = queryGrid + "left join MmMae_depa as DEPARTAMENTO on TER.cod_depa = DEPARTAMENTO.cod_dep ";
                queryGrid = queryGrid + "left join MmMae_iden as IDENTIFICACION on TER.tdoc = IDENTIFICACION.cod_tdoc ";
                queryGrid = queryGrid + "inner join InMae_mer as VENDEDOR on  VENDEDOR.cod_mer = TER.cod_ven ";


                if (tipo == 0)
                {
                    queryGrid = queryGrid + "where TER.clasific = 1 and CLIE.cod_ter = TER.cod_ter and TER.cod_ven='" + LB_cliente.Text + "' ORDER BY cod_ter  ";
                }
                else
                {
                    queryGrid = queryGrid + "where TER.clasific = 1 and CLIE.cod_ter = TER.cod_ter and TER.cod_ven='" + codigoVendedor + "'  ORDER BY cod_ter ";
                }

                dataGridCxC.ItemsSource = null;
                dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;
                TxtCantiCli.Text = dt.Rows.Count.ToString();

                //dataGridCxC.UpdateLayout();
            }
            catch (Exception w)
            {
                MessageBox.Show("eror al cargar BD:" + w);
            }
        }

        //convertir datos
        public string convertirCT(string _ct)
        {
            string a = "";
            if (_ct == "0")
            {
                a = "NO";
            }
            else
            {
                a = "SI";
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

        public string convertirEstaCiv(string _ct)
        {
            string valor = "";
            switch (_ct)
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

        // abrir ventana para editar
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {



            try
            {
                editarCliente editar = new editarCliente(tabitem);
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];


                editar._cod_ter = row["cod_ter"].ToString();
                editar._documentoLB = row["tdoc"].ToString();
                editar._documento = row["nom_tdoc"].ToString();
                editar._nom_comple = row["nom_ter"].ToString();
                editar._nom1 = row["nom1"].ToString();
                editar._nom2 = row["nom2"].ToString();
                editar._appe1 = row["apell1"].ToString();
                editar._appe2 = row["apell2"].ToString();
                editar._tel1 = row["tel1"].ToString();
                editar._tel2 = row["tel2"].ToString();
                editar._cel = row["cel"].ToString();
                editar._email = row["email"].ToString();
                editar._dir = row["dir"].ToString();
                editar._dir1 = row["dir1"].ToString();
                editar._dir2 = row["dir2"].ToString();
                editar._muniLB = row["cod_ciu"].ToString();
                editar._muni = row["nom_muni"].ToString();
                editar._depaLB = row["cod_depa"].ToString();
                editar._depa = row["nom_dep"].ToString();
                editar._fecha_nac = row["fec_cump"].ToString();
                editar._genero = row["genero"].ToString();


                editar._est_civil = convertirEstaCiv(row["est_civil"].ToString());


                editar._nom_emp = row["nom_emp"].ToString();
                editar._ct_celLB = row["act_emp"].ToString();
                editar._act_emp = row["nom_actEmp"].ToString();
                editar._ct_cel = convertirCTCodi(row["ct_cel"].ToString());
                editar._ct_email = convertirCTCodi(row["ct_email"].ToString());
                editar._ct_whats = convertirCTCodi(row["ct_whats"].ToString());
                editar._ct_sms = convertirCTCodi(row["ct_sms"].ToString());
                editar._ct_corres = convertirCTCodi(row["ct_corres"].ToString());
                editar._cod_cargoLB = row["cod_cargo"].ToString();
                editar._cod_cargo = row["nom_cargo"].ToString();
                editar._cod_ocupLB = row["cod_ocup"].ToString();
                editar._cod_ocup = row["nom_ocup"].ToString();
                editar._cod_profLB = row["cod_prof"].ToString();
                editar._cod_prof = row["nom_prof"].ToString();
                editar._num_doc = row["num_doc"].ToString();
                editar._obser = row["observ"].ToString();
                editar._hobbies = row["hobbies"].ToString();

                //editar._image_name = row["image_name"].ToString();
                //if (((byte[])row["img_cli"]).Length > 0) { editar._img_cli = (byte[])row["img_cli"]; } else { editar._img_cli = null; }
                editar._img_cli = _img_cliPublica;

                editar._ran_edad = row["ran_edad"].ToString();

                editar._LB_talla_zap_tenn = CODtalla1.Text;
                editar._talla_zap_tenn = TBtalla1.Text;

                editar._LB_talla_pant_fald = CODtalla2.Text;
                editar._talla_pant_fald = TBtalla2.Text;

                editar._LB_talla_vest_traj = CODtalla3.Text;
                editar._talla_vest_traj = TBtalla3.Text;

                editar._LB_talla_camisa = CODtalla4.Text;
                editar._talla_camisa = TBtalla4.Text;

                editar._LB_talla_camisa_sport = CODtalla5.Text;
                editar._talla_camisa_sport = TBtalla5.Text;


                editar.ShowInTaskbar = false;
                editar.Owner = Application.Current.MainWindow;
                editar.ShowDialog();


                if (tipoUsuario == "1" || tipoUsuario == "2")
                {
                    RefeshDataGrid(0);
                    //limpiar();                    
                }
                else
                {

                    if (editar.bandera == true)
                    {
                        actualizarGrid(row, editar);
                    }
                    //limpiar();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("seleccione un cliente");
            }

        }

        public void actualizarGrid(DataRowView row, editarCliente editar)
        {
            row["cod_ter"] = editar.TextBx_codigo.Text;
            row["tdoc"] = editar.LB_docu.Text;
            row["nom_tdoc"] = editar.TextBx_docu.Text;
            row["num_doc"] = editar.TextBx_num_docu.Text;
            //row["nom_ter"] = 
            row["nom1"] = editar.TextBx_nom1.Text;
            row["nom2"] = editar.TextBx_nom2.Text;
            row["apell1"] = editar.TextBx_apell1.Text;
            row["apell2"] = editar.TextBx_apell2.Text;
            row["tel1"] = editar.TextBx_tel1.Text;
            row["tel2"] = editar.TextBx_tel2.Text;
            row["cel"] = editar.TextBx_cel.Text;
            row["email"] = editar.TextBx_email.Text;
            row["dir"] = editar.TextBx_direcc.Text;
            //row["dir1"] = ""
            row["dir2"] = editar.TextBx_direcc2.Text;
            row["cod_ciu"] = editar.LB_cod_muni.Text;
            row["nom_muni"] = editar.TextBx_cod_muni.Text;
            row["cod_depa"] = editar.LB_cod_depa.Text;
            row["nom_dep"] = editar.TextBx_cod_depa.Text;
            row["fec_cump"] = editar.TextBx_fecha_nac.Text;
            row["genero"] = editar.TextBxCB_genero.Text;
            row["est_civil"] = editar.TextBxCB_est_civ.Text;
            row["nom_emp"] = editar.TextBx_nom_emp.Text;
            row["act_emp"] = editar.LB_act_emp.Text;
            row["nom_actEmp"] = editar.TextBx_act_emp.Text;

            row["ct_cel"] = editar.TextBxCB_ct_cel.Text;
            row["ct_email"] = editar.TextBxCB_ct_email.Text;
            row["ct_whats"] = editar.TextBxCB_ct_whats.Text;
            row["ct_sms"] = editar.TextBxCB_ct_sms.Text;
            row["ct_corres"] = editar.TextBxCB_ct_corres.Text;

            row["cod_cargo"] = editar.LB_cod_car.Text;
            row["nom_cargo"] = editar.TextBxCB_cod_car.Text;
            row["cod_ocup"] = editar.LB_cod_ocup.Text;
            row["nom_ocup"] = editar.TextBxCB_cod_ocup.Text;

            row["cod_prof"] = editar.LB_cod_pro.Text;
            row["nom_prof"] = editar.TextBxCB_cod_pro.Text;
            row["observ"] = editar.TextBx_descripcion.Text;
            row["hobbies"] = editar.TextBx_hobbies.Text;
            //row["image_name"] = editar.imageName;            
            //row["img_cli"] = editar.imgByteArr;

            row["ran_edad"] = editar.TextBxCB_ran_eda.Text;

            TBtalla1.Text = editar.TextBx_zap_ten.Text;
            TBtalla2.Text = editar.TextBx_pan_fal.Text;
            TBtalla3.Text = editar.TextBx_ves_tra.Text;
            TBtalla4.Text = editar.TextBx_camisa.Text;
            TBtalla5.Text = editar.TextBx_camisa_sp.Text;


            recargarImagen(editar.imgByteArr);

            dataGridCxC.View.Refresh();
        }


        public void limpiar()
        {

            TBtalla1.Text = "";
            TBtalla2.Text = "";
            TBtalla3.Text = "";
            TBtalla4.Text = "";
            TBtalla5.Text = "";
            TBcliente.Text = "";
        }

        // abrir ventana para seguimineto
        private void BtnSeg_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Seguimiento seguir = new Seguimiento();
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];

                seguir.cod_ter = row["cod_ter"].ToString();
                seguir.cod_mer = TxtUser.Text;
                seguir.bodega = bodega_vendedor;
                seguir.tipo = 1;
                //seguir.nom_comple = row["nom_ter"].ToString();
                //seguir.tel1 = row["tel1"].ToString();
                //seguir.tel2 = row["tel2"].ToString();
                //seguir.cel = row["cel"].ToString();
                //seguir.email = row["email"].ToString();
                //seguir.dir = row["dir"].ToString();                
                //if (row["ct_email"].ToString() == "") { seguir.ct_email = "..."; } else { seguir.ct_email = row["ct_email"].ToString(); }
                //if (row["ct_corres"].ToString() == "") { seguir.ct_correspondencia = "..."; } else { seguir.ct_correspondencia = row["ct_corres"].ToString(); }
                //if (row["ct_whats"].ToString() == "") { seguir.ct_whats = "..."; } else { seguir.ct_whats = row["ct_whats"].ToString(); }
                //if (row["ct_sms"].ToString() == "") { seguir.ct_sms = "..."; } else { seguir.ct_sms = row["ct_sms"].ToString(); }
                //if (row["ct_cel"].ToString() == "") { seguir.ct_celular = "..."; } else { seguir.ct_celular = row["ct_cel"].ToString(); }
                seguir.ShowInTaskbar = false;
                seguir.Owner = Application.Current.MainWindow;
                seguir.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("seleccione un cliente para el seguimiento");

            }

        }

        private void BTNsegCampa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Seguimiento seguir = new Seguimiento();
                DataRowView row = (DataRowView)dataGridCliCamp.SelectedItems[0];                
                seguir.cod_ter = row["cod_ter"].ToString();
                seguir.cod_mer = TxtUser.Text;
                seguir.bodega = bodega_vendedor;
                seguir.tipo = 2;
                seguir.CodigoCamp = row["cod_camp"].ToString();
                seguir.NomCamp = row["nom_camp"].ToString();
                seguir.ShowInTaskbar = false;
                seguir.Owner = Application.Current.MainWindow;
                seguir.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("seleccione un cliente para el seguimiento de campaña");

            }
        }


        // abrir ventana de historico
        private void BtnHis_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                HistoricoComercial historico = new HistoricoComercial();
                historico.cod_cliente = row["cod_ter"].ToString();
                historico.nom_cliente = row["nom_ter"].ToString();

                historico.ShowInTaskbar = false;
                historico.Owner = Application.Current.MainWindow;
                historico.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Selecciones un cliente");
            }

        }

        //abrir seguimiento de compra
        private void BtnSegCompra_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SeguimientoCompra seg_compra = new SeguimientoCompra();
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];

                seg_compra.nombre_cli = row["nom_ter"].ToString();
                seg_compra.cod_cli = row["cod_ter"].ToString();
                seg_compra.nombre_ven = TxtUserName.Text.Trim();
                seg_compra.cod_ven = TxtUser.Text.Trim();

                seg_compra.ShowInTaskbar = false;
                seg_compra.Owner = Application.Current.MainWindow;
                seg_compra.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Selecciones un cliente");

            }


        }


        //exportar a excel
        private void ExportaXLS_Click(object sender, RoutedEventArgs e)
        {
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = dataGridCxC.ExportToExcel(dataGridCxC.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];

            SaveFileDialog sfd = new SaveFileDialog
            {
                FilterIndex = 2,
                Filter = "Excel 97 to 2003 Files(*.xls)|*.xls|Excel 2007 to 2010 Files(*.xlsx)|*.xlsx|Excel 2013 File(*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog() == true)
            {
                using (Stream stream = sfd.OpenFile())
                {
                    if (sfd.FilterIndex == 1)
                        workBook.Version = ExcelVersion.Excel97to2003;
                    else if (sfd.FilterIndex == 2)
                        workBook.Version = ExcelVersion.Excel2010;
                    else
                        workBook.Version = ExcelVersion.Excel2013;
                    workBook.SaveAs(stream);
                }

                //Message box confirmation to view the created workbook.
                if (MessageBox.Show("Usted quiere abrir el archivo en excel?", "Ver archvo",
                                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }


        }


        //****************************** 2 tab ************************************************************

        private async void Button_CargarCampa_Click(object sender, RoutedEventArgs e)
        {       
            try
            {
                string codigoVendedor = "";

                if (tipoUsuario == "3" || tipoUsuario == "4")
                {
                    codigoVendedor = SiaWin._UserTag1;
                }
                else
                {
                    codigoVendedor = LB_cliente.Text;
                }

                if (codigoVendedor.Length <= 0)
                {
                    MessageBox.Show("ingresa el vendedor en la pestalla de BUSCAR VENDEDORES para poder realizar la consulta");
                    return;
                }

                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                TAB2.IsEnabled = false;
                sfBusyIndicator.IsBusy = true;

                dataGridCliCamp.ItemsSource = null;
                ChatTotSeg.ItemsSource = null;
                ChartDT.ItemsSource = null;
                
                var slowTask = Task<DataSet>.Factory.StartNew(() => SlowDude(codigoVendedor, source.Token), source.Token);
                await slowTask;
                
                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    dataGridCliCamp.ItemsSource = ((DataSet)slowTask.Result).Tables[0];
                    TBcampSi.Text = ((DataSet)slowTask.Result).Tables[0].Rows.Count.ToString();
                    ChatTotSeg.ItemsSource = ((DataSet)slowTask.Result).Tables[1];
                    ChartDT.ItemsSource = ((DataSet)slowTask.Result).Tables[2];                    
                }
                this.sfBusyIndicator.IsBusy = false;
                TAB2.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Opacity = 1;
                MessageBox.Show("aqui 2" + ex);

            }

        }
        
        private DataSet SlowDude(string codigoVendedor, CancellationToken cancellationToken)
        {
            try
            {
                DataSet jj = LoadData(codigoVendedor, cancellationToken);
                return jj;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        private DataSet LoadData(string codigoVendedor, CancellationToken cancellationToken)
        {
            
            try
            {
                DataSet ds = new DataSet();

                //clientes que estan en una campaña
                string cadena = "select cliente.cod_ter,cliente.nom_ter,temporal.cod_camp as cod_camp, campa.nom_camp as nom_camp,convert(varchar, campa.fecha_ini, 103) as fecha_ini,convert(varchar, campa.fecha_fin, 103) as fecha_fin from comae_ter as cliente ";
                cadena = cadena + "full join CrTemCampa as temporal on temporal.cod_ter = cliente.cod_ter ";
                cadena = cadena + "full join CrMae_campa as campa on campa.cod_camp  = temporal.cod_camp ";
                cadena = cadena + "where cliente.clasific=1 and cliente.cod_ven='" + codigoVendedor + "' ";
                cadena = cadena + "and campa.estado=1 ";
                cadena = cadena + "group by cliente.cod_ter,cliente.nom_ter,temporal.cod_camp,campa.nom_camp,campa.fecha_ini,campa.fecha_fin ";

                DataTable CliCamp = new DataTable();
                CliCamp = SiaWin.Func.SqlDT(cadena, "ClientCampa", idemp);
                ds.Tables.Add(CliCamp);                

                //total de seguimientos
                string total_seg = "select COUNT(cod_ter) as seguimiento,'Total de seguimientos' as 'total' from Crseg_cli as seguimiento  ";
                total_seg = total_seg + "inner join CrMae_campa as campa on seguimiento.cod_camp=campa.cod_camp ";
                total_seg = total_seg + "where seguimiento.cod_mer='"+codigoVendedor+"' ";

                DataTable graficoTotalSeg = SiaWin.Func.SqlDT(total_seg, "TotalSeguimiento", idemp);
                ds.Tables.Add(graficoTotalSeg);                

                //total de clientes en campaña
                string grafico = "select count(cliente.cod_ter) as suma,campa.nom_camp as nom_camp from comae_ter as cliente  ";
                grafico = grafico + "full join (select DISTINCT cod_ter,cod_camp from  CrTemCampa) as temporal on temporal.cod_ter = cliente.cod_ter ";
                grafico = grafico + "full join CrMae_campa as campa on campa.cod_camp  = temporal.cod_camp ";
                grafico = grafico + "where cliente.clasific=1 and cliente.cod_ven='" + codigoVendedor + "'  ";
                grafico = grafico + "and campa.estado=1  ";
                grafico = grafico + "group by campa.nom_camp ";
                DataTable graficoDT = SiaWin.Func.SqlDT(grafico, "Clientes", idemp);
                ds.Tables.Add(graficoDT);

                return ds;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }       

      
        //****************************** 2 tab ************************************************************

        //boton de admonistrador para buscar los clientes
        private void TXB_cliente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int idr = 0; string code = ""; string nombre = "";
                dynamic xx = SiaWin.WindowBuscar("inmae_mer", "cod_mer", "nom_mer", "cod_mer", "idrow", "Maestra De Clientes", cnEmp, false, "estado=1");
                xx.ShowInTaskbar = false;
                xx.Owner = Application.Current.MainWindow;
                xx.Width = 550;
                xx.Height = 500;
                xx.ShowDialog();
                idr = xx.IdRowReturn;
                code = xx.Codigo;
                nombre = xx.Nombre;
                xx = null;

                if (idr > 0)
                {
                    LB_cliente.Text = code;
                    TXB_cliente.Text = nombre.Trim();
                    BTNbuscar.IsEnabled = true;
                }
                if (string.IsNullOrEmpty(code)) e.Handled = false;
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BTNbuscar_Click(object sender, RoutedEventArgs e)
        {
            if (tipoUsuario == "1" || tipoUsuario == "2")
            {
                RefeshDataGrid(0);
            }

        }      

        //***************** buscar ********************************************

        Buscar win = new Buscar();

        private void dataGridCxC_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.F6)
                {

                    win.BuscarTodoEventHandler += new RoutedEventHandler(win_BuscarEventHandler);
                    win.CancelarEventHandler += new RoutedEventHandler(win_CancelarEventHandler);
                    win.SigEventHandler += new RoutedEventHandler(win_SigEventHandler);
                    win.AntEventHandler += new RoutedEventHandler(win_AntEventHandler);

                    win.ShowInTaskbar = false;
                    win.Owner = Application.Current.MainWindow;
                    win.Show();

                }
            }
            catch (Exception w)
            {
                MessageBox.Show("erro al abrir: " + w);
            }

        }

        void win_BuscarEventHandler(object sender, RoutedEventArgs e)
        {
            dataGridCxC.SearchHelper.Search(win.TextoSearch.Text);
        }

        void win_CancelarEventHandler(object sender, RoutedEventArgs e)
        {
            dataGridCxC.SearchHelper.ClearSearch();
        }

        void win_SigEventHandler(object sender, RoutedEventArgs e)
        {
            dataGridCxC.SearchHelper.FindNext(win.TextoSearch.Text);
        }

        void win_AntEventHandler(object sender, RoutedEventArgs e)
        {
            dataGridCxC.SearchHelper.FindPrevious(win.TextoSearch.Text);
        }

       


    }
}





