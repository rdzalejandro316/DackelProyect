using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SiasoftAppExt
{
    //nueva
    //Sia.PublicarPnt(9501,"Shopify");
    //Sia.TabU(9501);

    public partial class Shopify : UserControl
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Presupuesto mensual";
        static readonly string SpreadsheetId = "1ae_Mnbu7Y-e5pNAQJe-kmTlmIidEkWzSJu0EVbiKFYA";
        static readonly string sheet = "Resumen";
        static SheetsService service;

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";
        int tiempo = 30;

        DispatcherTimer disp = new DispatcherTimer();

        public Shopify(dynamic tabitem1)
        {
            try
            {


                InitializeComponent();
                SiaWin = Application.Current.MainWindow;
                tabitem = tabitem1;
                idemp = SiaWin._BusinessId;
                LoadConfig();
                IniciarCredenciales();

                Navegador.Navigate("https://docs.google.com/spreadsheets/d/1ae_Mnbu7Y-e5pNAQJe-kmTlmIidEkWzSJu0EVbiKFYA/edit#gid=0");

                disp.Interval = TimeSpan.FromMilliseconds(1000);
                disp.Tick += fecha;
                disp.Start();
                tabitem.CerrarInactivo = true;
            }
            catch (Exception w)
            {
                MessageBox.Show("MMM" + w);
            }
        }


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            tabitem.Cerrar(0);
            disp.Stop();
            Navegador.Close();
        }

        public void fecha(object sender, EventArgs e)
        {
            tiempo -= 1;
            if (tiempo == 0)
            {
                //LeerHoja();
                tiempo = 60;
            }
            Cronometro.Text = tiempo.ToString();
        }


        private void LoadConfig()
        {
            try
            {
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "Aqui");
            }
        }
        public void IniciarCredenciales()
        {
            try
            {
                GoogleCredential credential;
                using (var stream = new FileStream(@"Library/credenYo.json", FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
                }

                service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            catch (Exception w)
            {
                MessageBox.Show("error en IniciarCredenciales()" + w);
            }
        }
        //update
        //public void ActualizarCampo(string cambiar, string posicion)
        //{
        //    try
        //    {
        //        var range = $"{sheet}!Q" + posicion + ":Q" + posicion + "";
        //        var valueRange = new ValueRange();

        //        var oblist = new List<object>() { cambiar };
        //        valueRange.Values = new List<IList<object>> { oblist };

        //        var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
        //        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
        //        var appendReponse = updateRequest.Execute();
        //    }
        //    catch (Exception w)
        //    {

        //        MessageBox.Show("error en ActualizarCampo:" + w);
        //    }
        //}

        ////insertar
        //static void CreateEntry()
        //{
        //    int i = 0;
        //    do
        //    {

        //        var range = $"{sheet}!A:F";
        //        var valueRange = new ValueRange();

        //        var oblist = new List<object>() { i, (i + 1), (i + 2), (i + 3), (i + 4), (i + 5) };
        //        valueRange.Values = new List<IList<object>> { oblist };

        //        var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
        //        appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        //        var appendReponse = appendRequest.Execute();
        //        i++;
        //    } while (i < 10);
        //}

        //public async void LeerHoja()
        //{
        //    CancellationTokenSource source = new CancellationTokenSource();
        //    CancellationToken token = source.Token;
        //    GridConfiguracion.IsEnabled = false;
        //    sfBusyIndicator.IsBusy = true;

        //    var slowTask = Task<bool>.Factory.StartNew(() => SlowDude(source.Token), source.Token);
        //    await slowTask;

        //    if (((bool)slowTask.Result) == true)
        //    {
        //        sfBusyIndicator.IsBusy = false;
        //    }
            
        //}
        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        LeerHoja();
        //    }
        //    catch (Exception w)
        //    {
        //        MessageBox.Show("TARES ERRONEAS:" + w);
        //    }

        //}

        //public bool Saldos()
        //{
        //    bool bandera = true;
        //    var range = $"{sheet}!N1:N";
        //    SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
        //    var response = request.Execute();
        //    IList<IList<object>> values = response.Values;
        //    List<object> valores = new List<object>(values);

        //    if (values != null && values.Count > 0)
        //    {
        //        int a = 1;
        //        foreach (var row in values)
        //        {
        //            string saldo = 1;
        //            // var slowTask = Task<DataSet>.Factory.StartNew(() => SlowDude(row[0].ToString(), source.Token), source.Token);
        //            //await slowTask;
        //            if (saldo != "999999999999")
        //            {
        //                var rangeChange = $"{sheet}!Q" + a + ":Q" + a + "";
        //                var valueRange = new ValueRange();
        //                var oblist = new List<object>() { saldo };
        //                valueRange.Values = new List<IList<object>> { oblist };
        //                var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, rangeChange);
        //                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
        //                var appendReponse = updateRequest.Execute();
        //            }

        //            a = a + 1;
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("leeeeeeeee");
        //        bandera = false;
        //    }

        //    return bandera;
        //}


        //private DataSet SlowDude(string referencia, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        DataSet jj = LoadData(referencia, cancellationToken);
        //        return jj;
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }
        //    return null;
        //}

        //private DataSet LoadData(string referencia, CancellationToken cancellationToken)
        //{

        //    SqlConnection con = new SqlConnection(cnEmp);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    DataSet ds = new DataSet();
        //    cmd = new SqlCommand("SaldosInventariosPorReferenciaBodegas", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Ref", referencia);
        //    cmd.Parameters.AddWithValue("@Bod", "");
        //    da = new SqlDataAdapter(cmd);
        //    da.Fill(ds);
        //    con.Close();

        //    //double sub = 999999999999;
        //    //if (ds.Tables[0].Rows.Count > 0)
        //    //{
        //    //  sub = Convert.ToDouble(ds.Tables[0].Compute("Sum(saldo)", "").ToString());
        //    //}            
        //    //return sub.ToString();

        //    return ds;
        //}





        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    LeerHoja();
        }


    }
}
