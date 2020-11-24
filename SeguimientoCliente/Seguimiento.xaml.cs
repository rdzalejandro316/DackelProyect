using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SeguimientoCliente;
using System.Data.SqlClient;
using Syncfusion.UI.Xaml.Grid;
using System.Threading;

namespace SeguimientoCliente
{

    public partial class Seguimiento : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";
        string codigoVendedor;
        string tipoUsuario;        
        public string Conexion;

        public string cod_ter = "";
        public string nom_comple = "";
        public string tel1 = "";
        public string tel2 = "";
        public string cel = "";
        public string email = "";
        public string dir = "";
        public string cod_mer = "";
        public string bodega = "";

        public string ct_email = "";
        public string ct_correspondencia = "";
        public string ct_whats = "";
        public string ct_sms = "";
        public string ct_celular = "";


        public int tipo = 0;
        public string CodigoCamp = "";
        public string NomCamp = "";

        public Seguimiento()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;
            
            codigoVendedor = SiaWin._UserTag1;
            tipoUsuario = SiaWin._UserTag;                       

            this.MinWidth = 1200;
            this.MinHeight = 550;
            this.MaxWidth = 1200;
            this.MaxHeight = 550;

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


                TextBx_obse.Text = "NINGUNA";
                fecha_ini.Text = DateTime.Now.AddYears(-1).ToString("dd/MM/yyyy");
                fecha_fin.Text = DateTime.Now.ToString("dd/MM/yyyy");

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

                    if (tag == "CrMae_concepto")
                    {
                        cmptabla = tag; cmpcodigo = "cod_con"; cmpnombre = "UPPER(nom_con)"; cmporden = "cod_con"; cmpidrow = "cod_con"; cmptitulo = "Maestra de Conceptos"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_concepto1")
                    {
                        cmptabla = "CrMae_concepto"; cmpcodigo = "cod_con"; cmpnombre = "UPPER(nom_con)"; cmporden = "cod_con"; cmpidrow = "cod_con"; cmptitulo = "Maestra de Conceptos"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
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
                        if (tag == "CrMae_concepto")
                        {
                            LB_con.Text = code; TextBx_con.Text = nom;
                        }
                        if (tag == "CrMae_concepto1")
                        {
                            LB_ActSig.Text = code; TextBx_ActSig.Text = nom;
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

        private void TextBox_PreviewKeyDown1(object sender, KeyEventArgs e)
        {
            try
            {               
                string idTab = ((TextBox)sender).Tag.ToString();
                if (idTab.Length > 0)
                {
                    string tag = ((TextBox)sender).Tag.ToString();
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                    if (string.IsNullOrEmpty(tag)) return;

                    if (tag == "inmae_tip")
                    {
                        cmptabla = tag; cmpcodigo = "cod_tip"; cmpnombre = "UPPER(nom_tip)"; cmporden = "cod_tip"; cmpidrow = "cod_tip"; cmptitulo = "Maestra de Tipos"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "inmae_importaciones")
                    {
                        cmptabla = tag; cmpcodigo = "cod_imp"; cmpnombre = "UPPER(nom_imp)"; cmporden = "cod_imp"; cmpidrow = "cod_imp"; cmptitulo = "Maestra de Importaciones"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "inmae_bod")
                    {
                        cmptabla = tag; cmpcodigo = "cod_bod"; cmpnombre = "UPPER(nom_bod)"; cmporden = "cod_bod"; cmpidrow = "cod_bod"; cmptitulo = "Maestra de Bodegas"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
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
                        if (tag == "inmae_tip")
                        {
                            LB_con.Text = code; TextBx_con.Text = nom;
                        }
                        if (tag == "inmae_importaciones")
                        {
                            LB_ActSig.Text = code; TextBx_ActSig.Text = nom;
                        }
                        if (tag == "inmae_bod")
                        {
                            LB_ActSig.Text = code; TextBx_ActSig.Text = nom;
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

        private void regitrarSeguimiento(object sender, RoutedEventArgs e)
        {


            var selectedTag = ((ComboBoxItem)TextBxCB_camp.SelectedItem).Tag.ToString();

            if (LB_con.Text.Length > 0 && TextBx_CodVen.Text.Length > 0 && selectedTag.Length > 0 && LB_ActSig.Text.Length > 0 && TextBx_contac.Text.Length > 0 && TextBx_obse.Text.Length > 0)
            {
                try
                {
                    //MessageBox.Show(DateTime.Now.ToString());
                    string cadena = "insert into crseg_cli (fec_seg, cod_ter, cod_bod,cod_mer, cod_con, cod_camp, cod_consig, contacto_cli, observ) values ( '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', '" + TextBx_codigo.Text.Trim() + "', '" + TextBx_bodega.Text.Trim() + "','" + TextBx_CodVen.Text.Trim() + "', '" + LB_con.Text.Trim() + "', '" + selectedTag.ToString() + "', '" + LB_ActSig.Text.Trim() + "', '" + TextBx_contac.Text + "', '" + TextBx_obse.Text + "') ";
                    //MessageBox.Show("XCAA:"+cadena);
                    SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                    MessageBox.Show("Seguimiento de Cliente Exitoso");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error en la insercion"+ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Faltan algunos campos por llenar");
            }

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        //segundo tab *********************************************************************************

        //private void cargarGrid(object sender, RoutedEventArgs e)
        //{

        //    string fe_fin = fecha_fin.Text + " 23:59:59";

        //    try
        //    {
        //        string where = "";
        //        string queryGrid = "select convert(datetime, Seguimi.fec_seg, 103) as fec_seg,rtrim(Seguimi.cod_ter) as cod_ter,rtrim(clientes.nom_ter) as nom_ter, rtrim(Seguimi.cod_mer) as cod_mer,rtrim(Vendedores.nom_mer) as nom_mer,rtrim(Seguimi.cod_bod) as cod_bod,rtrim(Bodega.nom_bod) as nom_bod ,rtrim(Seguimi.cod_con) as cod_con,rtrim(Concepto.nom_con) as nom_con,rtrim(Seguimi.cod_camp) as cod_camp,IIF(Seguimi.cod_camp=0 ,'Ninguna',Campa.nom_camp) as nom_camp,rtrim(Seguimi.cod_consig) as cod_consig, rtrim(Concepto1.nom_con) as nom_con1,rtrim(Seguimi.contacto_cli) as contacto_cli, rtrim(Seguimi.observ) as observ ";
        //        queryGrid = queryGrid + "from crseg_cli as Seguimi ";
        //        queryGrid = queryGrid + "full join COMAE_TER as Clientes on Clientes.cod_ter = Seguimi.cod_ter ";
        //        queryGrid = queryGrid + "full join InMae_mer as Vendedores on vendedores.cod_mer = Seguimi.cod_mer  ";
        //        queryGrid = queryGrid + "full join CrMae_concepto as Concepto on Concepto.cod_con = Seguimi.cod_con ";
        //        queryGrid = queryGrid + "full join CrMae_concepto as Concepto1 on Concepto1.cod_con = Seguimi.cod_consig ";
        //        queryGrid = queryGrid + "full join CrMae_campa as Campa on Campa.cod_camp = Seguimi.cod_camp ";
        //        queryGrid = queryGrid + "full join InMae_bod as Bodega on Seguimi.cod_bod = Bodega.cod_bod ";
        //        queryGrid = queryGrid + "where Seguimi.cod_con=Concepto.cod_con ";                               
        //        queryGrid = queryGrid + "and Clientes.cod_ter = Seguimi.cod_ter and Clientes.cod_ter ='" + TextBx_codigo.Text + "' ";
        //        queryGrid = queryGrid + "and fec_seg between '" + fecha_ini.Text + "' and '" + fe_fin + "' ";
        //        queryGrid = queryGrid + "order by convert(datetime, fec_seg, 103) desc ";

        //        DataTable dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
        //        dataGridCxC.ItemsSource = dt.DefaultView;
        //        TotalReg.Text = dt.Rows.Count.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        private async void cargarGrid(object sender, RoutedEventArgs e)
        {
            try
            {                
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                PanelCons.IsEnabled = false;
                sfBusyIndicator.IsBusy = true;
                dataGridCxC.ItemsSource = null;

                string fechaIni = fecha_ini.Text;
                string fechaFin = fecha_fin.Text;
                string codigo = TextBx_codigo.Text;

                var slowTask = Task<DataSet>.Factory.StartNew(() => SlowDude(fechaIni, fechaFin, codigo, source.Token), source.Token);
                await slowTask;

                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    dataGridCxC.ItemsSource = ((DataSet)slowTask.Result).Tables[0];
                    TotalReg.Text = ((DataSet)slowTask.Result).Tables[0].Rows.Count.ToString();                    
                }
                this.sfBusyIndicator.IsBusy = false;
                PanelCons.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Opacity = 1;
                MessageBox.Show("aqui 2" + ex);

            }
        }

        private DataSet SlowDude(string ffi, string fff, string codigo, CancellationToken cancellationToken)
        {
            try
            {

                DataSet jj = LoadData(ffi, fff, codigo, cancellationToken);
                return jj;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        private DataSet LoadData(string Fi, string Ff, string codigo, CancellationToken cancellationToken)
        {
            try
            {
                DataSet ds = new DataSet();
                string queryGrid = "select convert(datetime, Seguimi.fec_seg, 103) as fec_seg,rtrim(Seguimi.cod_ter) as cod_ter,rtrim(clientes.nom_ter) as nom_ter, rtrim(Seguimi.cod_mer) as cod_mer,rtrim(Vendedores.nom_mer) as nom_mer,rtrim(Seguimi.cod_bod) as cod_bod,rtrim(Bodega.nom_bod) as nom_bod ,rtrim(Seguimi.cod_con) as cod_con,rtrim(Concepto.nom_con) as nom_con,rtrim(Seguimi.cod_camp) as cod_camp,IIF(Seguimi.cod_camp=0 ,'Ninguna',Campa.nom_camp) as nom_camp,rtrim(Seguimi.cod_consig) as cod_consig, rtrim(Concepto1.nom_con) as nom_con1,rtrim(Seguimi.contacto_cli) as contacto_cli, rtrim(Seguimi.observ) as observ ";
                queryGrid = queryGrid + "from crseg_cli as Seguimi ";
                queryGrid = queryGrid + "full join COMAE_TER as Clientes on Clientes.cod_ter = Seguimi.cod_ter ";
                queryGrid = queryGrid + "full join InMae_mer as Vendedores on vendedores.cod_mer = Seguimi.cod_mer  ";
                queryGrid = queryGrid + "full join CrMae_concepto as Concepto on Concepto.cod_con = Seguimi.cod_con ";
                queryGrid = queryGrid + "full join CrMae_concepto as Concepto1 on Concepto1.cod_con = Seguimi.cod_consig ";
                queryGrid = queryGrid + "full join CrMae_campa as Campa on Campa.cod_camp = Seguimi.cod_camp ";
                queryGrid = queryGrid + "full join InMae_bod as Bodega on Seguimi.cod_bod = Bodega.cod_bod ";
                queryGrid = queryGrid + "where Seguimi.cod_con=Concepto.cod_con ";
                queryGrid = queryGrid + "and Clientes.cod_ter = Seguimi.cod_ter and Clientes.cod_ter ='" + codigo + "' ";
                queryGrid = queryGrid + "and fec_seg between '" +Fi+ "' and '"+Ff+" 23:59:59' ";
                queryGrid = queryGrid + "order by convert(datetime, fec_seg, 103) desc ";
                //MessageBox.Show("queryGrid" + queryGrid);
                DataTable dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
                ds.Tables.Add(dt);
                return ds;
                //dataGridCxC.ItemsSource = dt.DefaultView;
                //TotalReg.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }


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


        public void contactos_color() {
            if (TextBx_ct_email.Text == "SI") { TextBx_ct_email.Foreground = Brushes.Green; } else { TextBx_ct_email.Foreground = Brushes.Red; }
            if (TextBx_ct_corres.Text == "SI") { TextBx_ct_corres.Foreground = Brushes.Green; } else { TextBx_ct_corres.Foreground = Brushes.Red; }
            if (TextBx_ct_whats.Text == "SI") { TextBx_ct_whats.Foreground = Brushes.Green; } else { TextBx_ct_whats.Foreground = Brushes.Red; }
            if (TextBx_ct_sms.Text == "SI") { TextBx_ct_sms.Foreground = Brushes.Green; } else { TextBx_ct_sms.Foreground = Brushes.Red; }
            if (TextBx_ct_cel.Text == "SI") { TextBx_ct_cel.Foreground = Brushes.Green; } else { TextBx_ct_cel.Foreground = Brushes.Red; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string cadena = "select tercero.cod_ter,tercero.nom_ter,tercero.tel1,tercero.tel2,tercero.cel,tercero.email,tercero.dir,cliente.ct_email,cliente.ct_corres,cliente.ct_whats,cliente.ct_sms,cliente.ct_cel ";
                cadena = cadena + "from Comae_ter as tercero ";
                cadena = cadena + "inner join CrMae_cli as cliente on tercero.cod_ter=cliente.cod_ter ";                
                cadena = cadena + "where tercero.cod_ter='" + cod_ter + "' ";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                TextBx_codigo.Text = dt.Rows[0]["cod_ter"].ToString();
                TextBx_NomCom.Text = dt.Rows[0]["nom_ter"].ToString();
                TextBx_Dir.Text = dt.Rows[0]["dir"].ToString();
                TextBx_tel1.Text = dt.Rows[0]["tel1"].ToString();
                TextBx_tel2.Text = dt.Rows[0]["tel2"].ToString();
                TextBx_cel.Text = dt.Rows[0]["cel"].ToString();
                TextBx_email.Text = dt.Rows[0]["email"].ToString();
                TextBx_CodVen.Text = cod_mer;
                TextBx_bodega.Text = bodega;

                ct_email = dt.Rows[0]["ct_email"].ToString() == "" ? "..." : dt.Rows[0]["ct_email"].ToString();
                TextBx_ct_email.Text = ct_email == "1" ? "SI":"NO";

                ct_correspondencia = dt.Rows[0]["ct_corres"].ToString() == "" ? "..." : dt.Rows[0]["ct_corres"].ToString();
                TextBx_ct_corres.Text = ct_correspondencia == "1" ? "SI" : "NO"; 

                ct_whats = dt.Rows[0]["ct_whats"].ToString() == "" ? "..." : dt.Rows[0]["ct_whats"].ToString();
                TextBx_ct_whats.Text = ct_whats == "1" ? "SI" : "NO";

                ct_sms = dt.Rows[0]["ct_sms"].ToString() == "" ? "..." : dt.Rows[0]["ct_sms"].ToString();
                TextBx_ct_sms.Text = ct_sms == "1" ? "SI" : "NO";

                ct_celular = dt.Rows[0]["ct_cel"].ToString() == "" ? "..." : dt.Rows[0]["ct_cel"].ToString();
                TextBx_ct_cel.Text = ct_sms == "1" ? "SI" : "NO"; ;
                contactos_color();
                Cargarcampañas(tipo, cod_ter);
            }
            catch (Exception)
            {

                throw;
            }

            //TextBx_codigo.Text = cod_ter;
            //TextBx_NomCom.Text = nom_comple;
            //TextBx_Dir.Text = dir;
            //TextBx_tel1.Text = tel1;
            //TextBx_tel2.Text = tel2;
            //TextBx_cel.Text = cel;
            //TextBx_email.Text = email;
            //TextBx_CodVen.Text = cod_mer;
            //TextBx_bodega.Text = bodega;

            //TextBx_ct_email.Text = ct_email;
            //TextBx_ct_corres.Text = ct_correspondencia;
            //TextBx_ct_whats.Text = ct_whats;
            //TextBx_ct_sms.Text = ct_sms;
            //TextBx_ct_cel.Text = ct_celular;
            //contactos_color();
            //traer campañas del cliente
            //string cadena = "select temporal.cod_camp as cod_camp,campa.nom_camp as nom_camp from CrTemCampa as temporal ";
            //cadena = cadena + "inner join CrMae_campa as campa on campa.cod_camp = temporal.cod_camp ";
            //cadena = cadena + "where temporal.cod_ter = '" + cod_ter + "' and campa.estado='1' ";
            //cadena = cadena + "group by temporal.cod_camp,campa.nom_camp ";

            //SqlDataReader drCli = SiaWin.Func.SqlDR(cadena, idemp); ;
            //while (drCli.Read())
            //{
            //    var newItem = new ComboBoxItem();
            //    newItem.Content = drCli["nom_camp"].ToString().Trim();
            //    newItem.Tag = drCli["cod_camp"].ToString().Trim();
            //    TextBxCB_camp.Items.Add(newItem);
            //}

        }
        public void Cargarcampañas(int tipo,string tercero) {
            try
            {
                string cadena = "select temporal.cod_camp as cod_camp,campa.nom_camp as nom_camp from CrTemCampa as temporal ";
                cadena = cadena + "inner join CrMae_campa as campa on campa.cod_camp = temporal.cod_camp ";
                cadena = cadena + "where temporal.cod_ter = '" + tercero + "' and campa.estado='1' ";
                cadena = cadena + "group by temporal.cod_camp,campa.nom_camp ";

                SqlDataReader drCli = SiaWin.Func.SqlDR(cadena, idemp); ;
                while (drCli.Read())
                {
                    var newItem = new ComboBoxItem();
                    newItem.Content = drCli["nom_camp"].ToString().Trim();
                    newItem.Tag = drCli["cod_camp"].ToString().Trim();
                    TextBxCB_camp.Items.Add(newItem);
                }

                if (tipo == 2)
                {
                    int index = 0;
                    foreach (ComboBoxItem item in TextBxCB_camp.Items) {
                        //MessageBox.Show("valor:"+ item.Content);
                        //MessageBox.Show("valor1:" + item["cod_camp"]);
                        if (item.Content.ToString() == NomCamp.Trim())
                        {
                            TextBxCB_camp.SelectedIndex = index;
                        }
                        index++;
                    }

                    //MessageBox.Show("codigo campaña:"+ CodigoCamp);
                    //MessageBox.Show("nombre campaña:" + NomCamp);
                    //TextBxCB_camp.SelectedIndex = 1;
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("error al traer campañas:"+w);
            }
        }


        // ROWS automatico ******************************************************************************
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();

        //To get the calculated height from GetAutoRowHeight method.    
        double autoHeight = double.NaN;

        // The list contains the column names that will excluded from the height calculation in GetAutoRowHeight method.
        List<string> excludeColumns = new List<string>() { "observ" };         

        private void dataGridCxC_QueryRowHeight(object sender, Syncfusion.UI.Xaml.Grid.QueryRowHeightEventArgs e)
        {
            if (this.dataGridCxC.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
            {
                if (autoHeight > 24)
                {
                    e.Height = autoHeight;
                    e.Handled = true;
                }
            }
        }



    }
}
