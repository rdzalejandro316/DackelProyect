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
using System.Windows.Shapes;

namespace AnalisisdeFormasdePago
{
    
    public partial class Detalle : Window
    {
        public string idpass = "";
        dynamic SiaWin;        
        int idemp = 0;
        string cnEmp = "";

        public Detalle()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cargar();
            }
            catch (Exception w)
            {
                MessageBox.Show("error al cargar formas de pago:"+w);
            }
        }

        public async void cargar()
        {
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            LoadConfig();

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;            
            sfBusyIndicator.IsBusy = true;

            string id = idpass;

            var slowTask = Task<DataSet>.Factory.StartNew(() => LoadData(id, source.Token), source.Token);
            await slowTask;

            if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
            {
                DataDocDetalle.ItemsSource = ((DataSet)slowTask.Result).Tables[0].DefaultView;
                tx_Tot.Text = ((DataSet)slowTask.Result).Tables[0].Rows.Count.ToString();
            }
            else
            {
                DataDocDetalle.ItemsSource = null;
                tx_Tot.Text = "0";
            }

            sfBusyIndicator.IsBusy = false;
        }


        private DataSet LoadData(string id, CancellationToken cancellationToken)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("AnalisisDetalleFormasPago", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);                
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
                var excelEngine = DataDocDetalle.ExportToExcel(DataDocDetalle.View, options);
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



    }
}
