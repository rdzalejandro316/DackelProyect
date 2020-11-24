using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    //Sia.PublicarPnt(9468,"SALINVENYESID");
    //Sia.TabU(9468);
    public partial class SALINVENYESID : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";

        public SALINVENYESID(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;


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
                tabitem.Logo(idLogo, ".png");
                tabitem.Title = "Saldos (" + aliasemp + ")";

                FecIni.Text = DateTime.Now.ToString();
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
                if (e.Key == System.Windows.Input.Key.F8)
                {
                    string tag = ((TextBox)sender).Tag.ToString();

                    if (string.IsNullOrEmpty(tag)) return;
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";

                    if (tag == "inmae_bod")
                    {
                        cmptabla = tag; cmpcodigo = "cod_bod"; cmpnombre = "nom_bod"; cmporden = "cod_bod"; cmpidrow = "idrow"; cmptitulo = "Maestra de bodegas"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "estado=1";
                    }
                    if (tag == "inmae_gru")
                    {
                        cmptabla = tag; cmpcodigo = "cod_gru"; cmpnombre = "nom_gru"; cmporden = "cod_gru"; cmpidrow = "idrow"; cmptitulo = "Maestra de grupo"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "inmae_tip")
                    {
                        cmptabla = tag; cmpcodigo = "cod_tip"; cmpnombre = "nom_tip"; cmporden = "cod_tip"; cmpidrow = "idrow"; cmptitulo = "Maestra de Linea"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }


                    //MessageBox.Show(cmptabla + "-" + cmpcodigo + "-" + cmpnombre + "-" + cmporden + "-" + cmpidrow + "-" + cmptitulo + "-" + cmpconexion + "-" + cmpwhere);
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
                        ((TextBox)sender).Text = code;

                        if (tag == "inmae_bod")
                        {
                            TextBoxbod.Text = code;
                        }
                        if (tag == "inmae_gru")
                        {
                            TextBoxGru.Text = code;
                        }
                        if (tag == "inmae_tip")
                        {
                            TextBoxtip.Text = code;
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                //SqlConnection con = new SqlConnection(cnEmp);
                //SqlCommand cmd = new SqlCommand();
                //SqlDataAdapter da = new SqlDataAdapter();
                //DataSet ds = new DataSet();
                //cmd = new SqlCommand("SaldosInventariosPorBodegaGrupoLinea_yesid", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Fecha", FecIni.Text);
                //cmd.Parameters.AddWithValue("@Bod", TextBoxbod.Text);
                //cmd.Parameters.AddWithValue("@Gru", TextBoxGru.Text);
                //cmd.Parameters.AddWithValue("@Tip", TextBoxtip.Text);
                //cmd.Parameters.AddWithValue("@Sexo", CBlinea.Text);
                //da = new SqlDataAdapter(cmd); ;
                //da.Fill(ds);
                //con.Close();
                //GridKardex.ItemsSource = ds.Tables[0];


                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;                
                sfBusyIndicator.IsBusy = true;


                string Fi = FecIni.Text;
                string bod = TextBoxbod.Text;
                string gru = TextBoxGru.Text;
                string linea = TextBoxtip.Text;
                string sexo = CBlinea.Text;

                var slowTask = Task<DataSet>.Factory.StartNew(() => LoadData(Fi, bod, gru, linea, sexo, source.Token), source.Token);
                await slowTask;


                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {

                    GridKardex.ItemsSource = ((DataSet)slowTask.Result).Tables[0].DefaultView;
                    Tx_rows.Text = ((DataSet)slowTask.Result).Tables[0].Rows.Count.ToString();
                }
                else
                {
                    MessageBox.Show("sin registros");
                    GridKardex.ItemsSource = null;
                    Tx_rows.Text = "0";
                }

                sfBusyIndicator.IsBusy = false;                
            }
            catch (Exception w)
            {
                MessageBox.Show("error al cargar la consulta programada" + w);
            }

        }

        private DataSet LoadData(string Fi, string bod, string gru, string linea, string sexo, CancellationToken cancellationToken)
        {

            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("SaldosInventariosPorBodegaGrupoLinea_yesid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fecha", Fi);
                cmd.Parameters.AddWithValue("@Bod", bod);
                cmd.Parameters.AddWithValue("@Gru", gru);
                cmd.Parameters.AddWithValue("@Tip", linea);
                cmd.Parameters.AddWithValue("@Sexo", sexo);
                da = new SqlDataAdapter(cmd); 
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(ds);
                con.Close();

                return ds;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }



        private void ExportaXLS_Click(object sender, RoutedEventArgs e)
        {
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = GridKardex.ExportToExcel(GridKardex.View, options);
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




    }




}
