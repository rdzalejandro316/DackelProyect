using AnalisisdeFormasdePago;
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

    /// Sia.PublicarPnt(9510,"AnalisisdeFormasdePago");
    /// Sia.TabU(9510);
    public partial class AnalisisdeFormasdePago : UserControl
    {
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";
        public AnalisisdeFormasdePago(dynamic tabitem1)
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
                //cnEmp = foundRow["BusinessCn"].ToString().Trim();
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
                tabitem.Logo(idLogo, ".png");
                tabitem.Title = "Analisis de Venta(" + aliasemp + ")";
                Tx_fecini.Text = DateTime.Now.ToShortDateString();
                Tx_fecfin.Text = DateTime.Now.ToShortDateString();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        private async void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                GridConfiguracion.IsEnabled = false;
                sfBusyIndicator.IsBusy = true;
                string ffi = Tx_fecini.Text;
                string ffinal = Tx_fecfin.Text;

                var slowTask = Task<DataSet>.Factory.StartNew(() => LoadData(ffi, ffinal, source.Token), source.Token);
                await slowTask;

                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    DataDoc.ItemsSource = ((DataSet)slowTask.Result).Tables[0].DefaultView;
                    tx_Tot.Text = ((DataSet)slowTask.Result).Tables[0].Rows.Count.ToString();
                }
                else
                {
                    DataDoc.ItemsSource = null;
                    tx_Tot.Text = "0";
                }

                GridConfiguracion.IsEnabled = true;
                sfBusyIndicator.IsBusy = false;

            }
            catch (Exception w)
            {
                MessageBox.Show("error en la consulta:" + w);
            }


        }


        private DataSet LoadData(string Fi, string Ff, CancellationToken cancellationToken)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("AnalisisFormasPago", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fec_ini", Fi);
                cmd.Parameters.AddWithValue("@fec_fin", Ff);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            return ds;
        }



        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
                options.ExcelVersion = ExcelVersion.Excel2013;
                var excelEngine = DataDoc.ExportToExcel(DataDoc.View, options);
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
            catch (Exception w)
            {
                MessageBox.Show("error al exportar");
            }
        }


        private void BtnDetalle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)DataDoc.SelectedItems[0];                
                string id = row["idreg"].ToString();

                Detalle ventana = new Detalle();
                ventana.idpass = id;
                ventana.ShowInTaskbar = false;
                ventana.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                ventana.Owner = Application.Current.MainWindow;
                ventana.ShowDialog();


            }
            catch (Exception w)
            {
                MessageBox.Show("error al ver el detalle");
            }
        }



    }
}
