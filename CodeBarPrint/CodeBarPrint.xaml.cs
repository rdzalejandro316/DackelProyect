using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
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

namespace SiasoftAppExt
{
    //Sia.PublicarPnt(9534, "CodeBarPrint");
    //dynamic WinDescto = ((Inicio)Application.Current.MainWindow).WindowExt(9534, "CodeBarPrint");
    //WinDescto.ShowInTaskbar = false;
    //WinDescto.Owner = Application.Current.MainWindow;
    //WinDescto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //WinDescto.ShowDialog();

    public partial class CodeBarPrint : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        public CodeBarPrint()
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
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "aquiio");
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.F8)
                {
                    int idr = 0; string code = ""; string nom = "";
                    dynamic winb = SiaWin.WindowBuscar("inmae_ref", "cod_ref", "nom_ref", "cod_ref", "idrow", "maestra de referencias ", cnEmp, false, "", idEmp: idemp);
                    winb.ShowInTaskbar = false;
                    winb.Owner = Application.Current.MainWindow;
                    winb.Height = 400;
                    winb.Width = 500;
                    winb.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    winb.ShowDialog();
                    idr = winb.IdRowReturn;
                    code = winb.Codigo;
                    nom = winb.Nombre;
                    if (!string.IsNullOrEmpty(code))
                    {
                        TxReferencia.Text = code;
                        GetRefer(code);
                    }
                }

            }
            catch (Exception w)
            {
                MessageBox.Show("error al abrir:" + w);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string code = (sender as TextBox).Text;
            if (!string.IsNullOrEmpty(code))
            {
                GetRefer(code);
            }
        }

        public void GetRefer(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    string query = "select cod_ref,nom_ref,nom_ref2,nom_tip,serial,val_ref,inmae_tall.desc_tall ";
                    query += "from inmae_ref ";
                    query += "inner join inmae_tip on inmae_tip.cod_tip = inmae_ref.cod_tip ";
                    query += "inner join inmae_tall on inmae_tall.cod_tall = inmae_ref.cod_tall ";
                    query += "where cod_ref='" + code + "';";

                    DataTable dt = SiaWin.Func.SqlDT(query, "temp", idemp);

                    if (dt.Rows.Count <= 0)
                    {
                        MessageBox.Show("la referencia que ingreso no existe", "alerrt", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        TxNombre.Text = "---";
                        TxLinea.Text = "---";
                        TxDesc.Text = "---";
                        TxTalla.Text = "---";
                        TxValor.Text = "---";
                    }
                    else
                    {
                        TxNombre.Text = dt.Rows[0]["nom_ref"].ToString();
                        TxLinea.Text = dt.Rows[0]["nom_tip"].ToString();
                        TxDesc.Text = dt.Rows[0]["nom_ref2"].ToString();
                        TxTalla.Text = dt.Rows[0]["desc_tall"].ToString();
                        TxValor.Text = dt.Rows[0]["val_ref"].ToString();
                    }

                }

            }
            catch (Exception w)
            {
                MessageBox.Show("error al validar:" + w);
            }
        }


        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SyncCopies.Value > 0 && !string.IsNullOrEmpty(TxReferencia.Text))
                {
                    int tot = (int)SyncCopies.Value;
                    for (int i = 1; i <= tot; i++)
                    {
                        ImprimeTicket();
                    }
                }
                else
                {
                    MessageBox.Show("el campo referencias debe de estar lleno o en numero de copias debe de ser mayor a 0");
                }

            }
            catch (Exception w)
            {
                MessageBox.Show("error al imprimir:" + w);
            }
        }

        private void ImprimeTicket()
        {
            try
            {

                PrintDocument pd = new PrintDocument();

                PaperSize ps = new PaperSize("F50x25", 500, 110);
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrintController = new StandardPrintController();

                pd.DefaultPageSettings.Margins.Left = 0;
                pd.DefaultPageSettings.Margins.Right = 0;
                pd.DefaultPageSettings.Margins.Top = 0;
                pd.DefaultPageSettings.Margins.Bottom = 5;
                pd.DefaultPageSettings.PaperSize = ps;
                System.Windows.Forms.PrintPreviewDialog ll = new System.Windows.Forms.PrintPreviewDialog();
                ll.Document = pd;
                pd.Print();
            }
            catch (System.Exception _error)
            {
                MessageBox.Show(_error.Message);
            }
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {

            Graphics g = e.Graphics;

            SolidBrush sb = new SolidBrush(System.Drawing.Color.Black);           
            Font CbarArial = new Font("Arial", 7, System.Drawing.FontStyle.Regular);
            Font CbarArialNombre = new Font("Arial", 7, System.Drawing.FontStyle.Regular);
            Font CbarArialBold = new Font("Arial", 8, System.Drawing.FontStyle.Bold);
            Font TituloArial = new Font("Arial", 12, System.Drawing.FontStyle.Bold);
            //Font Cbar = new Font("Codabar 123 LE", 18, System.Drawing.FontStyle.Regular);
            Font CbarFree = new Font("Free 3 of 9", 20, System.Drawing.FontStyle.Regular);
            //Font Cbar1 = new Font("3 of 9 Barcode", 18, System.Drawing.FontStyle.Regular);
            string code = TxReferencia.Text.Trim();
            string nombre = TxNombre.Text.Trim();
            string linea = TxLinea.Text.Trim();
            decimal valor = Convert.ToDecimal(TxValor.Text.Trim());
            string talla = TxTalla.Text.Trim();

            g.DrawString("LE COLEZIONI", TituloArial, sb, 60, 10);
            g.DrawString(linea, CbarArialBold, sb, 10, 30);            
            g.DrawString(valor.ToString(), CbarArial, sb, 150, 32);
            g.DrawString(nombre, CbarArialNombre, sb, 10, 50);
            g.DrawString("*" + code + "*", CbarFree, sb, 5, 65);
            g.DrawString(code, CbarArial, sb, 10, 96);            
            g.DrawString("TALLA:" + talla, CbarArial, sb, 200, 70);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
