using System;
using System.Collections.Generic;
using System.Data;
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

namespace RegistroBono
{

    public partial class Consultar : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        public Consultar()
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
                Tx_date.Text = DateTime.Now.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void BtnClick_Click(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            sfBusyIndicator.IsBusy = true;

            string fecha = Tx_date.Text;
            var slowTask = Task<DataTable>.Factory.StartNew(() => LoadData(fecha, source.Token), source.Token);
            await slowTask;

            if (((DataTable)slowTask.Result).Rows.Count > 0)
            {
                dataGridCxC.ItemsSource = ((DataTable)slowTask.Result).DefaultView;
                Tx_total.Text = ((DataTable)slowTask.Result).Rows.Count.ToString();
            }

            sfBusyIndicator.IsBusy = false;
        }

        private DataTable LoadData(string fecha, CancellationToken cancellationToken)
        {
            try
            {
                DataTable dt = SiaWin.Func.SqlDT("select * from inMae_bonos where fecha>='" + fecha + "';", "MaestraTalla", idemp);
                return dt;
            }
            catch (Exception w)
            {
                MessageBox.Show(w.Message);
                return null;
            }

        }



    }
}
