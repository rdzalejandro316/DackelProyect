using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SiasoftAppExt
{
    //antigua 
    //Sia.PublicarPnt(9484,"GoogleSheets");
    //dynamic WinDescto = ((Inicio)Application.Current.MainWindow).WindowExt(9484,"GoogleSheets");
    //WinDescto.ShowInTaskbar = false;
    //WinDescto.Owner = Application.Current.MainWindow;
    //WinDescto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //WinDescto.ShowDialog();

    //nueva
    //Sia.PublicarPnt(9501,"GoogleSheets");

    public partial class GoogleSheets : UserControl
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

        public GoogleSheets(dynamic tabitem1)
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
                MessageBox.Show("ssssssssssss" + w);
            }
        }
        //update
        public void ActualizarCampo(string cambiar, string posicion)
        {
            try
            {
                var range = $"{sheet}!Q" + posicion + ":Q" + posicion + "";
                var valueRange = new ValueRange();

                var oblist = new List<object>() { cambiar };
                valueRange.Values = new List<IList<object>> { oblist };

                var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                var appendReponse = updateRequest.Execute();
            }
            catch (Exception w)
            {

                MessageBox.Show("aaaaaaa" + w);
            }
        }
        //insertar
        static void CreateEntry()
        {
            int i = 0;
            do
            {

                var range = $"{sheet}!A:F";
                var valueRange = new ValueRange();

                var oblist = new List<object>() { i, (i + 1), (i + 2), (i + 3), (i + 4), (i + 5) };
                valueRange.Values = new List<IList<object>> { oblist };

                var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                var appendReponse = appendRequest.Execute();
                i++;
            } while (i < 10);
        }

        public void LeerHoja()
        {
            try
            {
                var range = $"{sheet}!N1:N";
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
                var response = request.Execute();
                IList<IList<object>> values = response.Values;
                List<object> valores = new List<object>(values);
                MessageBox.Show("Aun no entra en la validacion");
                if (values != null && values.Count > 0)
                {
                    MessageBox.Show("Entra en la primera validacion" + values.Count);
                    int a = 1;
                    foreach (var row in values)
                    {
                        string saldo = Saldo(row[0].ToString());
                        if (saldo != "999999999999")
                        {
                            ActualizarCampo(saldo, a.ToString());
                            //row[4] = "siiiisas";
                        }
                        a = a + 1;
                    }
                }
                else
                {
                    MessageBox.Show("leeeeeeeee");
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("aqiro troi" + w);
            }


        }


        public string Saldo(string referencia)
        {
            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("SaldosInventariosPorReferenciaBodegas", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ref", referencia);
                cmd.Parameters.AddWithValue("@Bod", "");
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                double sub = 999999999999;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sub = Convert.ToDouble(ds.Tables[0].Compute("Sum(saldo)", "").ToString());
                }
                return sub.ToString();
            }
            catch (Exception w)
            {
                MessageBox.Show("error al conseguir saldo" + w);
                return "";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            disp.Stop();
            //Navegador.Stop();
            Navegador.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LeerHoja();
        }
    }
}
