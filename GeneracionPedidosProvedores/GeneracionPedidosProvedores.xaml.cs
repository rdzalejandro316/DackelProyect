using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using Syncfusion.XlsIO;
using System.IO;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Data;
using System.Globalization;
using Microsoft.Win32;

namespace SiasoftAppExt
{

    //Sia.PublicarPnt(9457, "GeneracionPedidosProvedores");
    //Sia.TabU(9457);
    public partial class GeneracionPedidosProvedores : UserControl
    {
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";        
        string cod_empresa = "";

        public GeneracionPedidosProvedores(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            tabitem.Title = "generacion de pedidos";
            tabitem.Logo(9, ".png");
            tabitem.MultiTab = false;
            idemp = SiaWin._BusinessId;
            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                System.Data.DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);

                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());                
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                cod_empresa = foundRow["BusinessCode"].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
                tabitem.Logo(idLogo, ".png");
                tabitem.Title = "generacion de pedidos (" + aliasemp + ")";

                FechaConsul.Text = DateTime.Now.ToString();                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void LoadGrupos(){            
            DataTable dt = SiaWin.Func.SqlDT("select rtrim(InMae_gru.cod_gru) AS CodigoGrupo, rtrim(InMae_sgr.Nom_sgr) AS NombreSubgrupo from InMae_sgr,InMae_gru where InMae_gru.Cod_tip = " + TextCod_Lin.Text + " and InMae_sgr.Cod_gru = InMae_gru.cod_gru order by InMae_gru.cod_gru", "Clientes", idemp);            
            dataGridGrup.ItemsSource = dt.DefaultView;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.F8)
                {
                    string tag = ((TextBox)sender).Tag.ToString();
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                    if (string.IsNullOrEmpty(tag)) return;


                    if (tag == "inmae_bod")
                    {
                        cmptabla = tag; cmpcodigo = "cod_bod"; cmpnombre = "nom_bod"; cmporden = "cod_bod"; cmpidrow = "idrow"; cmptitulo = "Maestra de bodegas"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";//estado=1 and ind_vta=1
                    }
                    if (tag == "inmae_prv")
                    {
                        cmptabla = tag; cmpcodigo = "cod_prv"; cmpnombre = "nom_prv"; cmporden = "cod_prv"; cmpidrow = "idrow"; cmptitulo = "Maestra de provedores"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "inmae_tip")
                    {
                        cmptabla = tag; cmpcodigo = "cod_tip"; cmpnombre = "nom_tip"; cmporden = "cod_tip"; cmpidrow = "idrow"; cmptitulo = "Maestra de linea"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }

                    int idr = 0; string code = ""; string nom = "";

                    dynamic winb = SiaWin.WindowBuscar(cmptabla, cmpcodigo, cmpnombre, cmporden, cmpidrow, cmptitulo, SiaWin.Func.DatosEmp(idemp), mostrartodo, cmpwhere, idEmp: idemp);
                    winb.ShowInTaskbar = false;
                    winb.Owner = Application.Current.MainWindow;
                    winb.ShowDialog();
                    idr = winb.IdRowReturn;
                    code = winb.Codigo;
                    nom = winb.Nombre;
                    winb = null;
                    if (idr > 0)
                    {
                        
                        if (tag == "inmae_bod") {
                            TextCod_bod.Text = code;
                            TextNombreBod.Text = nom;
                        }
                        if (tag == "inmae_prv"){
                            TextCod_Pro.Text = code;
                            TextNombrePro.Text = nom;
                        }
                        if (tag == "inmae_tip"){
                            TextCod_Lin.Text = code;
                            TextNombreLin.Text = nom;
                            LoadGrupos();
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

        private void Consultar(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime fechaConsulta = Convert.ToDateTime(FechaConsul.Text);
                int monthFechaCon = fechaConsulta.Month;
                int mesIni = Int32.Parse(TextBox_Meses.Text);
                DateTime _mesini = fechaConsulta.AddMonths(-mesIni);
                int monthMesCon = _mesini.Month;
                int prom = monthFechaCon - monthMesCon;

                string queryFecha = ""; int _mes = 1; int _aum_con = 0; int _aum_mesi = 1; int _dia_um = 1;

                for (int i = 0; i < mesIni; i++)
                {
                    DateTime _fec_con = _mesini.AddMonths(_aum_con);//07-
                    DateTime _fec_con_day = _fec_con.AddDays(_dia_um);
                    var f = _fec_con_day.ToString("dd/MM/yyyy");

                    DateTime _fec_mes = _mesini.AddMonths(_aum_mesi);// 01/06/2018 - 01/07/2018
                    var m = _fec_mes.ToString("dd/MM/yyyy");
                    queryFecha += "sum(IIF(cab.fec_trn BETWEEN '" + f + "' and '" + m + "' , IIF(cab.cod_trn BETWEEN '004' and '005', cantidad, -cantidad), 00000000000.00)) as mes" + _mes + ", ";
                    _mes++; _aum_con++; _aum_mesi++;
                }

           
                SqlConnection con = new SqlConnection(SiaWin._cn);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("GeneracionPedidosProvedores", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_bod", TextCod_bod.Text);
                cmd.Parameters.AddWithValue("@cod_prv", TextCod_Pro.Text);
                cmd.Parameters.AddWithValue("@cod_Tip", TextCod_Lin.Text);
                cmd.Parameters.AddWithValue("@ArmaFech", queryFecha);
                cmd.Parameters.AddWithValue("@mesIni", _mesini.ToString());
                cmd.Parameters.AddWithValue("@fechaConsulta", FechaConsul.Text);
                cmd.Parameters.AddWithValue("@cod_empresa", cod_empresa);            
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                dataGridCxC.ItemsSource = ds.Tables[0];
           
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Export_excel(object sender, RoutedEventArgs e) {
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
                
                if (MessageBox.Show("Usted quiere abrir el archivo en excel?", "Ver archvo",MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {                    
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
        }




    }

}
