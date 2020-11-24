using InformeEfectividad;
using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.Windows.Tools.Controls;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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


namespace SiasoftAppExt
{

    public partial class InformeEfectividad : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";
        


        public InformeEfectividad(dynamic tabitem1)
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;

            LoadConfig();
            cargarFechas();

           
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
                tabitem.Title = "Informe de Efectividad(" + aliasemp + ")";
       

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void cargarFechas(){

            fecha_ini_campa.Text = DateTime.Today.AddMonths(-1).ToString();
            fecha_fin_campa.Text = DateTime.Now.ToShortDateString();

            fecha_ini_cumple.Text = DateTime.Today.AddMonths(-1).ToString();
            fecha_fin_cumple.Text = DateTime.Now.ToShortDateString();

            fecha_ini_llama.Text = DateTime.Today.AddMonths(-1).ToString();
            fecha_fin_llama.Text = DateTime.Now.ToShortDateString();

            fecha_ini_whats.Text = DateTime.Today.AddMonths(-1).ToString();
            fecha_fin_whats.Text = DateTime.Now.ToShortDateString();

            fecha_ini_email.Text = DateTime.Today.AddMonths(-1).ToString();
            fecha_fin_email.Text = DateTime.Now.ToShortDateString();

            fecha_ini_sms.Text = DateTime.Today.AddMonths(-1).ToString();
            fecha_fin_sms.Text = DateTime.Now.ToShortDateString();

            fecha_ini_vis_camp.Text = DateTime.Today.AddMonths(-1).ToString();
            fecha_fin_vis_camp.Text = DateTime.Now.ToShortDateString();

            fecha_ini_vis_cumple.Text = DateTime.Today.AddMonths(-1).ToString();
            fecha_fin_vis_cumple.Text = DateTime.Now.ToShortDateString();

            fecha_ini_factu.Text = DateTime.Today.AddMonths(-1).ToString();
            fecha_fin_factu.Text = DateTime.Now.ToShortDateString();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            try
            {               
                string tag = ((TextBox)sender).Tag.ToString();
                string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                    
                if (tag == "inmae_mer")
                {
                    cmptabla = tag; cmpcodigo = "cod_mer"; cmpnombre = "nom_mer"; cmporden = "cod_mer"; cmpidrow = "idrow"; cmptitulo = "Maestra de vendedores"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
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
                    if (tag == "inmae_mer")
                    {
                        LBcode_ven.Text = code; LBnom_ven.Text = nom.Trim();
                        Consulta.IsEnabled = true;                        
                    }
                    var uiElement = e.OriginalSource as UIElement;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
                e.Handled = true;
                
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

        
        public DataSet devolverDS()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("CRMefectividad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_vendedor", LBcode_ven.Text);

                cmd.Parameters.AddWithValue("@FechaIni_campa", fecha_ini_campa.Text);
                cmd.Parameters.AddWithValue("@FechaFin_campa", fecha_fin_campa.Text);

                cmd.Parameters.AddWithValue("@FechaIni_cumpleaños", fecha_ini_cumple.Text);
                cmd.Parameters.AddWithValue("@FechaFin_cumpleaños", fecha_fin_cumple.Text);

                cmd.Parameters.AddWithValue("@FechaIni_llama", fecha_ini_llama.Text);
                cmd.Parameters.AddWithValue("@FechaFin_llama", fecha_fin_llama.Text);

                cmd.Parameters.AddWithValue("@FechaIni_whats", fecha_ini_whats.Text);
                cmd.Parameters.AddWithValue("@FechaFin_whats", fecha_fin_whats.Text);

                cmd.Parameters.AddWithValue("@FechaIni_email", fecha_ini_email.Text);
                cmd.Parameters.AddWithValue("@FechaFin_email", fecha_fin_email.Text);

                cmd.Parameters.AddWithValue("@FechaIni_sms", fecha_ini_sms.Text);
                cmd.Parameters.AddWithValue("@FechaFin_sms", fecha_fin_sms.Text);


                cmd.Parameters.AddWithValue("@FechaIni_vis_campa", fecha_ini_vis_camp.Text);
                cmd.Parameters.AddWithValue("@FechaFin_vis_campa", fecha_fin_vis_camp.Text);

                cmd.Parameters.AddWithValue("@FechaIni_vis_cumple", fecha_ini_vis_cumple.Text);
                cmd.Parameters.AddWithValue("@FechaFin_vis_cumple", fecha_fin_vis_cumple.Text);

                cmd.Parameters.AddWithValue("@FechaIni_factu", fecha_ini_factu.Text);
                cmd.Parameters.AddWithValue("@FechaFin_factu", fecha_fin_factu.Text);

                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                
            }
            catch (Exception)
            {

                MessageBox.Show("error al cargar la consulta programada");
            }

            return ds;
        }

             
        private void Consulta_Click(object sender, RoutedEventArgs e)
        {            
            TabControl1.SelectedIndex = 2;
            TabControl1.SelectedIndex = 1;
            
            cardCRM();
            cardCampañas();
            cardCumpleaños();
            cardllamadas();
            totalLLamdas();
            cardwhatsApp();

            cardEmail();
            cardSMS();

            CampaVisi();
            CumpleVisi();
            CardFacturado();
        }

        public void cardCRM() {
            DataSet data = new DataSet();
            data = devolverDS();
            TXT_vendedor.Text = data.Tables[0].Rows[0]["nom_mer"].ToString();
            TXT_totalCRM.Text = data.Tables[0].Rows[0]["totalCRM"].ToString();
        }

        public void cardCampañas()
        {
            DataSet data = new DataSet();
            data = devolverDS();

            TXT_vendedor2.Text = data.Tables[1].Rows[0]["vendedor"].ToString();
            TXT_cli_camp.Text = data.Tables[1].Rows[0]["Total_campa"].ToString();
            TXT_lla_camp.Text = data.Tables[1].Rows[0]["llamadas_campa"].ToString();

            Double valor1 = Convert.ToInt32(data.Tables[1].Rows[0]["Total_campa"]);
            Double valor2 = Convert.ToInt32(data.Tables[1].Rows[0]["llamadas_campa"]);
            Double operacion = (valor2 / valor1) * 100;

            TXT_por_camp.Text = operacion.ToString()+ " %";
        }

        public void cardCumpleaños() {
            try
            {
                DataSet data = new DataSet();
                data = devolverDS();

                TXT_vendedor3.Text = data.Tables[2].Rows[0]["nom_mer"].ToString();
                TXT_cli_cumple.Text = data.Tables[2].Rows[0]["total_cli_cump"].ToString();
                TXT_lla_cumple.Text = data.Tables[2].Rows[0]["llama_cump"].ToString();
                
                Double valor1 = Convert.ToInt32(data.Tables[2].Rows[0][2]);
                Double valor2 = Convert.ToInt32(data.Tables[2].Rows[0][3]);
                Double operacion = (valor2 / valor1) * 100;

                TXT_por_cumple.Text = operacion.ToString() + " %";

            }
            catch (Exception w)
            {
                MessageBox.Show("error 3:" + w);

            }
        }

        public void cardllamadas() {

            DataSet data = new DataSet();
            data = devolverDS();
            TXT_vendedor4.Text = data.Tables[3].Rows[0]["nom_mer"].ToString();
            TXT_llamadas.Text = data.Tables[3].Rows[0]["llamadas"].ToString();

        }

        public void totalLLamdas()
        {
            DataSet data = new DataSet();
            data = devolverDS();
            Total_llamadas.Text = data.Tables[4].Rows[0]["total"].ToString();
        }

        public void cardwhatsApp()
        {
            DataSet data = new DataSet();
            data = devolverDS();
           
            TXT_vendedor5.Text = data.Tables[5].Rows[0]["nom_mer"].ToString();
            TXT_whats.Text = data.Tables[5].Rows[0]["whatsapp"].ToString();            
        }

        public void cardEmail()
        {
            DataSet data = new DataSet();
            data = devolverDS();

            TXT_vendedor9.Text = data.Tables[9].Rows[0]["nom_mer"].ToString();
            TXT_emai.Text = data.Tables[9].Rows[0]["email"].ToString();
        }

        public void cardSMS()
        {
            DataSet data = new DataSet();
            data = devolverDS();

            TXT_vendedor10.Text = data.Tables[10].Rows[0]["nom_mer"].ToString();
            TXT_SMS.Text = data.Tables[10].Rows[0]["sms"].ToString();
        }

        public void CampaVisi()
        {
            DataSet data = new DataSet();
            data = devolverDS();

            TXT_vendedor6.Text = data.Tables[6].Rows[0]["nom_mer"].ToString();
            TXT_cli_camp_visi.Text = data.Tables[6].Rows[0]["visita_campa"].ToString();
            TXT_lla_camp2.Text = data.Tables[6].Rows[0]["llamadas_campa"].ToString();

            Double valor1 = Convert.ToInt32(data.Tables[6].Rows[0]["llamadas_campa"]);
            Double valor2 = Convert.ToInt32(data.Tables[6].Rows[0]["visita_campa"]);
            Double operacion = (valor2 / valor1) * 100;

            porcentaje_vis_camp.Text = operacion.ToString() + "%";

        }

        public void CumpleVisi()
        {
            DataSet data = new DataSet();
            data = devolverDS();

            TXT_vendedor7.Text = data.Tables[7].Rows[0]["nom_mer"].ToString();
            TXT_cli_cump_visi.Text = data.Tables[7].Rows[0]["visita_cumple"].ToString();
            TXT_lla_cump2.Text = data.Tables[7].Rows[0]["llama_cump"].ToString();

            Double valor1 = Convert.ToInt32(data.Tables[7].Rows[0]["llama_cump"]);
            Double valor2 = Convert.ToInt32(data.Tables[7].Rows[0]["visita_cumple"]);
            Double operacion = (valor2 / valor1) * 100;

            porcentaje_vis_cumple.Text = operacion.ToString() + "%";

        }

        public void CardFacturado()
        {
            DataSet data = new DataSet();
            data = devolverDS();
            
            TXT_vendedor8.Text = data.Tables[8].Rows[0]["nom_mer"].ToString();
            TXT_Total_Seg_Facturado.Text = data.Tables[8].Rows[0]["facturado_seg"].ToString();
            TXT_TotalFacturado.Text = data.Tables[8].Rows[0]["factu"].ToString();
        }


        //detalle-----------------------------------------

        private void BTN_crm_Click(object sender, RoutedEventArgs e)
        {
            Detalle_CRM windowsCRM = new Detalle_CRM();

            windowsCRM.cod_vendedor = LBcode_ven.Text;
            windowsCRM.nom_vendedor = LBnom_ven.Text.Trim();


            windowsCRM.ShowInTaskbar = false;
            windowsCRM.Owner = Application.Current.MainWindow;
            windowsCRM.ShowDialog();


        }

        private void BTN_camp_Click(object sender, RoutedEventArgs e)
        {
            
            Detalle_Campaña windowsCamp = new Detalle_Campaña();

            windowsCamp.cod_vendedor = LBcode_ven.Text;
            windowsCamp.nom_vendedor = LBnom_ven.Text.Trim();

            windowsCamp.fecha_ini = fecha_ini_campa.Text;
            windowsCamp.fecha_fin = fecha_fin_campa.Text + " 23:59:59";

            windowsCamp.ShowInTaskbar = false;
            windowsCamp.Owner = Application.Current.MainWindow;
            windowsCamp.ShowDialog();

           
        }

        private void BTN_cumple_Click(object sender, RoutedEventArgs e)
        {
            Detalle_Cumpleaños windowsCumpleaños = new Detalle_Cumpleaños();

            windowsCumpleaños.cod_vendedor = LBcode_ven.Text;
            windowsCumpleaños.nom_vendedor = LBnom_ven.Text.Trim();
            windowsCumpleaños.fecha_ini = fecha_ini_cumple.Text;

            string fe_fin = fecha_fin_cumple.Text + " 23:59:59";
            windowsCumpleaños.fecha_fin = fe_fin;

            windowsCumpleaños.ShowInTaskbar = false;
            windowsCumpleaños.Owner = Application.Current.MainWindow;
            windowsCumpleaños.ShowDialog();
        }

        private void BTN_llama_Click(object sender, RoutedEventArgs e)
        {
            Detalle_llamada llmadas = new Detalle_llamada();

            llmadas.cod_vendedor = LBcode_ven.Text;
            llmadas.nom_vendedor = LBnom_ven.Text.Trim();
            llmadas.fecha_ini = fecha_ini_llama.Text;            
            llmadas.fecha_fin = fecha_fin_llama.Text;

            llmadas.ShowInTaskbar = false;
            llmadas.Owner = Application.Current.MainWindow;
            llmadas.ShowDialog();
        }

        private void BTN_whats_Click(object sender, RoutedEventArgs e)
        {
            Detalle_WhatsApp wind_whats = new Detalle_WhatsApp();

            wind_whats.cod_vendedor = LBcode_ven.Text;
            wind_whats.nom_vendedor = LBnom_ven.Text.Trim();

            wind_whats.fecha_ini = fecha_ini_whats.Text;            
            wind_whats.fecha_fin = fecha_fin_whats.Text;

            wind_whats.ShowInTaskbar = false;
            wind_whats.Owner = Application.Current.MainWindow;
            wind_whats.ShowDialog();
        }

        private void BTN_Vist_Camp_Click(object sender, RoutedEventArgs e) {

            Visita_Camp wind_vis_camp = new Visita_Camp();

            wind_vis_camp.cod_vendedor = LBcode_ven.Text;
            wind_vis_camp.nom_vendedor = LBnom_ven.Text.Trim();

            wind_vis_camp.fecha_ini = fecha_ini_vis_camp.Text;
            wind_vis_camp.fecha_fin = fecha_fin_vis_camp.Text;

            wind_vis_camp.ShowInTaskbar = false;
            wind_vis_camp.Owner = Application.Current.MainWindow;
            wind_vis_camp.ShowDialog();

        }

        private void BTN_Vist_Cumple_Click(object sender, RoutedEventArgs e) {


            Visita_Cumple win_vis_cumple = new Visita_Cumple();

            win_vis_cumple.cod_vendedor = LBcode_ven.Text;
            win_vis_cumple.nom_vendedor = LBnom_ven.Text.Trim();

            win_vis_cumple.fecha_ini = fecha_ini_vis_cumple.Text;
            win_vis_cumple.fecha_fin = fecha_fin_vis_cumple.Text;

            win_vis_cumple.ShowInTaskbar = false;
            win_vis_cumple.Owner = Application.Current.MainWindow;
            win_vis_cumple.ShowDialog();
        }

        private void BTN_fac_seg_Click(object sender, RoutedEventArgs e)
        {


            facturado wind_facSeg = new facturado();

            wind_facSeg.cod_vendedor = LBcode_ven.Text;
            wind_facSeg.nom_vendedor = LBnom_ven.Text.Trim();

            wind_facSeg.fecha_ini = fecha_ini_factu.Text;
            wind_facSeg.fecha_fin = fecha_fin_factu.Text;

            wind_facSeg.ShowInTaskbar = false;
            wind_facSeg.Owner = Application.Current.MainWindow;
            wind_facSeg.ShowDialog();
        }

        private void BTN_Email_Click(object sender, RoutedEventArgs e)
        {

            Detalle_Email wind_email = new Detalle_Email();

            wind_email.cod_vendedor = LBcode_ven.Text;
            wind_email.nom_vendedor = LBnom_ven.Text.Trim();

            wind_email.fecha_ini = fecha_ini_factu.Text;
            wind_email.fecha_fin = fecha_fin_factu.Text;

            wind_email.ShowInTaskbar = false;
            wind_email.ShowDialog();
        }

        private void BTN_SMS_Click(object sender, RoutedEventArgs e)
        {

            Detalle_SMS wind_SMS = new Detalle_SMS();

            wind_SMS.cod_vendedor = LBcode_ven.Text;
            wind_SMS.nom_vendedor = LBnom_ven.Text.Trim();

            wind_SMS.fecha_ini = fecha_ini_factu.Text;
            wind_SMS.fecha_fin = fecha_fin_factu.Text;

            wind_SMS.ShowInTaskbar = false;
            wind_SMS.Owner = Application.Current.MainWindow;
            wind_SMS.ShowDialog();
        }





    }
}
