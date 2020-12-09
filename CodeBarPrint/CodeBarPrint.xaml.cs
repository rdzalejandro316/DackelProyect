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
        bool masivo = false;

        public string cod_ref = "";
        public double cantidad = 0;
        public string num_trn = "";
        public DateTime fecompra;

        DataTable dtcue = new DataTable();
        DataRow drow;

        public CodeBarPrint()
        {
            InitializeComponent();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SiaWin = Application.Current.MainWindow;
                idemp = SiaWin._BusinessId;
                LoadConfig();

                if (!string.IsNullOrEmpty(cod_ref))
                {
                    if (cantidad > 0)
                    {
                        TxFecha.Text = fecompra.ToString("dd/MM/yyyy");
                        TxReferencia.Text = cod_ref;
                        SyncCopies.Value = cantidad;
                        TxReferencia.Focus();
                    }
                }

                if (!string.IsNullOrEmpty(num_trn))
                {
                    TxFecha.Text = fecompra.ToString("dd/MM/yyyy");
                    TxCompra.Text = num_trn;
                }

            }
            catch (Exception w)
            {
                MessageBox.Show("error al cargar:" + w);
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.F8)
                {

                    string tabla = "";
                    string codigo = "";
                    string nombre = "";
                    string idrow = "";
                    string titulo = "";
                    string where = "";

                    string tx = (sender as TextBox).Name;

                    switch (tx)
                    {
                        case "TxReferencia":
                            tabla = "inmae_ref";
                            codigo = "cod_ref";
                            nombre = "nom_ref";
                            idrow = "idrow";
                            titulo = "maestra de referencias";
                            break;
                        case "TxCompra":
                            tabla = "incab_doc";
                            codigo = "cod_trn";
                            nombre = "num_trn";
                            idrow = "idreg";
                            titulo = "documentos";
                            where = "cod_trn='001' ";
                            break;
                    }


                    int idr = 0; string code = ""; string nom = "";
                    dynamic winb = SiaWin.WindowBuscar(tabla, codigo, nombre, codigo, idrow, titulo, cnEmp, false, where, idEmp: idemp);
                    winb.ShowInTaskbar = false;
                    winb.Owner = Application.Current.MainWindow;
                    winb.Height = 400;
                    winb.Width = 500;
                    winb.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    winb.ShowDialog();
                    idr = winb.IdRowReturn;
                    code = winb.Codigo;
                    nom = winb.Nombre;

                    switch (tx)
                    {
                        case "TxReferencia":
                            if (!string.IsNullOrEmpty(code))
                            {
                                TxReferencia.Text = code;
                                GetRefer(code);
                            }
                            break;
                        case "TxCompra":
                            TxCompra.Text = nombre;
                            GetDoc(nombre.ToString());
                            break;

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
            string tx = (sender as TextBox).Name;
            string code = (sender as TextBox).Text;

            switch (tx)
            {
                case "TxReferencia":
                    if (!string.IsNullOrEmpty(code)) GetRefer(code);
                    break;
                case "TxCompra":
                    if (!string.IsNullOrEmpty(code)) GetDoc(code);
                    break;
            }

        }

        public void GetRefer(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    string query = "select cod_ref,nom_ref,nom_ref2,nom_tip,serial,val_ref,precio_usd,inmae_tall.desc_tall ";
                    query += "from inmae_ref ";
                    query += "inner join inmae_tip on inmae_tip.cod_tip = inmae_ref.cod_tip ";
                    query += "inner join inmae_tall on inmae_tall.cod_tall = inmae_ref.cod_tall ";
                    query += "where cod_ref='" + code + "';";

                    DataTable dt = SiaWin.Func.SqlDT(query, "temp", idemp);

                    if (dt.Rows.Count <= 0)
                    {
                        MessageBox.Show("la referencia que ingreso no existe", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        TxNombre.Text = "---";
                        TxLinea.Text = "---";
                        TxDesc.Text = "---";
                        TxTalla.Text = "---";
                        TxValor.Text = "---";
                        TxUSD.Text = "---";
                        TxSerial.Text = "---";
                    }
                    else
                    {
                        TxNombre.Text = dt.Rows[0]["nom_ref"].ToString().Trim();
                        TxLinea.Text = dt.Rows[0]["nom_tip"].ToString().Trim();
                        TxDesc.Text = dt.Rows[0]["nom_ref2"].ToString().Trim();
                        TxTalla.Text = dt.Rows[0]["desc_tall"].ToString().Trim();
                        TxValor.Text = dt.Rows[0]["val_ref"].ToString().Trim();
                        TxUSD.Text = dt.Rows[0]["precio_usd"].ToString().Trim();
                        TxSerial.Text = dt.Rows[0]["serial"].ToString().Trim();
                    }

                }

            }
            catch (Exception w)
            {
                MessageBox.Show("error al validar:" + w);
            }
        }

        public void GetDoc(string num_trn)
        {
            try
            {
                if (!string.IsNullOrEmpty(num_trn))
                {
                    string query = "select cue.cod_ref,ref.nom_ref,cue.cantidad,cue.cos_uni,cue.cos_tot,ref.serial,tip.nom_tip,ref.val_ref,ref.precio_usd,tall.desc_tall ";
                    query += "from incue_doc cue ";
                    query += "inner join inmae_ref ref on  ref.cod_ref = cue.cod_ref ";
                    query += "inner join inmae_tip tip on  tip.cod_tip = ref.cod_tip ";
                    query += "inner join inmae_tall tall on tall.cod_tall = ref.cod_tall ";
                    query += "where cue.num_trn='" + num_trn + "'  ";

                    dtcue = SiaWin.Func.SqlDT(query, "temp", idemp);

                    if (dtcue.Rows.Count <= 0)
                    {
                        MessageBox.Show("el documento no existe", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        dataGridConsulta.ItemsSource = null;
                        TxDocTot.Text = "0";
                    }
                    else
                    {
                        dataGridConsulta.ItemsSource = dtcue.DefaultView;
                        TxDocTot.Text = dtcue.Rows.Count.ToString();
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
                string btn = (sender as Button).Name;

                switch (btn)
                {
                    case "BtnPrint":
                        if (SyncCopies.Value > 0 && !string.IsNullOrEmpty(TxReferencia.Text))
                        {
                            masivo = false;
                            int tot = (int)SyncCopies.Value;
                            for (int i = 1; i <= tot; i++) ImprimeTicket();
                        }
                        else MessageBox.Show("el campo referencias debe de estar lleno o en numero de copias debe de ser mayor a 0");
                        break;
                    case "BtnPrintDoc":
                        if (!string.IsNullOrEmpty(TxCompra.Text))
                        {
                            if (dataGridConsulta.View.Records.Count > 0)
                            {
                                masivo = true;

                                foreach (DataRow dr in dtcue.Rows)
                                {
                                    drow = dr;
                                    decimal cantidad = Convert.ToDecimal(dr["cantidad"]);
                                    if (cantidad > 0)
                                    {
                                        for (int i = 1; i <= cantidad; i++)
                                        {
                                            ImprimeTicket();
                                        }
                                    }

                                }
                            }
                            else
                            {
                                MessageBox.Show("el cuerpo no tiene ningun registro");
                            }
                        }

                        break;
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

            DateTime f;
            Graphics g = e.Graphics;


            #region fuente

            SolidBrush sb = new SolidBrush(System.Drawing.Color.Black);
            Font FontEmpresa = new Font("Arial", 14, System.Drawing.FontStyle.Bold);
            Font FontLinea = new Font("Arial", 8, System.Drawing.FontStyle.Bold);
            Font FontNombre = new Font("Arial", 6, System.Drawing.FontStyle.Regular);
            Font FontValor = new Font("Arial", 10, System.Drawing.FontStyle.Bold);
            Font FontCode = new Font("Arial", 7, System.Drawing.FontStyle.Regular);
            Font CbarArialFecha = new Font("Arial", 5, System.Drawing.FontStyle.Regular);
            Font CbarArialSerial = new Font("Arial", 7, System.Drawing.FontStyle.Bold);
            Font FontTalla = new Font("Arial", 10, System.Drawing.FontStyle.Regular);
            Font CbarFree = new Font("Free 3 of 9", 20, System.Drawing.FontStyle.Regular);
            #endregion

            string code = masivo == true ? drow["cod_ref"].ToString().Trim() : TxReferencia.Text.Trim();
            string nombre = masivo == true ? drow["nom_ref"].ToString().Trim() : TxNombre.Text.Trim();
            string linea = masivo == true ? drow["nom_tip"].ToString().Trim() : TxLinea.Text.Trim();
            DateTime fec_compra;
            if (masivo == true) fec_compra = fecompra;
            else
            {
                if (DateTime.TryParse(TxFecha.Text, out f) == false)
                    fec_compra = Convert.ToDateTime(TxFecha.Text);
                else
                    fec_compra = DateTime.Now;
            }

            decimal valor = masivo == true ? Convert.ToDecimal(drow["val_ref"]) : Convert.ToDecimal(TxValor.Text.Trim());
            decimal usd = masivo == true ? Convert.ToDecimal(drow["precio_usd"]) : Convert.ToDecimal(TxUSD.Text.Trim());
            string talla = masivo == true ? drow["desc_tall"].ToString().Trim() : TxTalla.Text.Trim();
            string serial = masivo == true ? drow["serial"].ToString().Trim() : TxSerial.Text.Trim();


            g.DrawString("LE COLEZIONI", FontEmpresa, sb, 35, 0);
            g.DrawString(fec_compra.ToString("MMyy"), CbarArialFecha, sb, 200, 5);

            g.DrawString(linea, FontLinea, sb, 10, 24);
            g.DrawString("PRECIO:" + valor.ToString("N0"), FontValor, sb, 150, 24);

            g.DrawString(nombre, FontNombre, sb, new RectangleF(10, 40, 150, 26));

            g.DrawString("US:" + usd.ToString("N0"), FontValor, sb, 160, 40);

            g.DrawString("*" + code + "*", CbarFree, sb, 5, 64);
            g.DrawString(code, FontCode, sb, 10, 87);
            g.DrawString("TALLA:" + talla, FontTalla, sb, 200, 70);

            g.DrawString("SERIAL:" + serial, CbarArialSerial, sb, 85, 97);


        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
