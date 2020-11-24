using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
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

    public partial class ClienteCelebracion : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";
        public string Conexion;

        public ClienteCelebracion(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;
            LoadConfig();

            loadGridDia();
            loadGridMes();

        
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
                tabitem.Title = "Cumpleaños de los clientes(" + aliasemp + ")";

                //TxtUser.Text = SiaWin._UserAlias;                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        public void loadGridDia()
        {
            string queryGrid = "SELECT	rtrim(TER.cod_ter) as cod_ter, rtrim(TER.nom_ter) as nom_ter,rtrim(UPPER(TER.nom1)) as nom1,rtrim(UPPER(TER.nom2)) as nom2,rtrim(UPPER(TER.apell1)) as apell1,rtrim(UPPER(TER.apell2)) as apell2,rtrim(UPPER(TER.email)) as email,CONVERT(varchar,fec_cump,103) as fec_cump, (cast(datediff(dd,TER.fec_cump,GETDATE()) / 365.25 as int)) as edad ";
            queryGrid = queryGrid + "FROM CrMae_cli as CLIE ,COMAE_TER as TER ";
            queryGrid = queryGrid + "where TER.clasific = 1 ";
            queryGrid = queryGrid + "and CLIE.cod_ter = TER.cod_ter ";
            queryGrid = queryGrid + "and datepart(dd, fec_cump) = datepart(dd, getdate()) ";
            queryGrid = queryGrid + "and datepart(mm, fec_cump) = datepart(mm, getdate()) ";            
            queryGrid = queryGrid + "ORDER BY fec_cump ";

            DataTable dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
            dataGridDia.ItemsSource = dt.DefaultView;



        }


        private void dataGridDia_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGridDia.SelectedItems[0];
            string cliente = row["nom_ter"].ToString();
            string correo = row["email"].ToString();

            cumplCliente.Text = cliente;
            emailCliente.Text = correo;


            if (correo.Length > 0)
            {
                BTNenviarCump.IsEnabled = true;
                habilitado.Text = "SI";
                habilitado.Foreground = Brushes.Green;
            }
            else
            {
                BTNenviarCump.IsEnabled = false;
                habilitado.Text = "NO";
                habilitado.Foreground = Brushes.Red;

            }

        }

        public void loadGridMes() {

            string queryGrid = "SELECT	rtrim(TER.cod_ter) as cod_ter, rtrim(TER.nom_ter) as nom_ter,rtrim(UPPER(TER.nom1)) as nom1,rtrim(UPPER(TER.nom2)) as nom2,rtrim(UPPER(TER.apell1)) as apell1,rtrim(UPPER(TER.apell2)) as apell2,rtrim(UPPER(TER.email)) as email,CONVERT(varchar,fec_cump,103) as fec_cump, (cast(datediff(dd,TER.fec_cump,GETDATE()) / 365.25 as int)) as edad ";
            queryGrid = queryGrid + "FROM CrMae_cli as CLIE ,COMAE_TER as TER ";
            queryGrid = queryGrid + "where TER.clasific = 1 ";
            queryGrid = queryGrid + "and CLIE.cod_ter = TER.cod_ter ";
            queryGrid = queryGrid + "and datepart(mm, fec_cump) = datepart(mm, getdate()) ";            
            queryGrid = queryGrid + "ORDER BY fec_cump ";

            DataTable dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
            dataGridMes.ItemsSource = dt.DefaultView;
        }

        private void dataGridMes_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e) {

            DataRowView row = (DataRowView)dataGridMes.SelectedItems[0];
            string cliente = row["nom_ter"].ToString();
            string correo = row["email"].ToString();

            ClienteMes.Text = cliente;
            EmailMes.Text = correo;

            if (correo.Length > 0)
            {
                BTNenviarMes.IsEnabled = true;
                habilitadoMes.Text = "SI";
                habilitadoMes.Foreground = Brushes.Green;
            }
            else
            {
                BTNenviarMes.IsEnabled = false;
                habilitadoMes.Text = "NO";
                habilitadoMes.Foreground = Brushes.Red;

            }

        }




        public void BtnEnviarCorreoCumple_Click(object sender, RoutedEventArgs e) {

            try
            {
                DataRowView row = (DataRowView)dataGridDia.SelectedItems[0];

                string nombre = row["nom_ter"].ToString();
                string correo = row["email"].ToString();

                SqlDataReader drCli = SiaWin.Func.SqlDR("select nom_configuracion,con_configuracion from CrMae_configuracion where idrow=1", idemp); ;

                string correoLecollezioni = "";
                string contraseLecollezioni = "";

                while (drCli.Read()){correoLecollezioni = drCli["nom_configuracion"].ToString().Trim();contraseLecollezioni = drCli["con_configuracion"].ToString().Trim();}

                if (correo.Length > 0)
                {                    
                    SiaWin.Func.funcionCorreoCumple(nombre, correoLecollezioni, contraseLecollezioni, correo);                    
                }
                else
                {
                    MessageBox.Show("El cliente no tiene correo electronico");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
           

        }

        public void BtnEnviarCorreoPromocion_Click(object sender, RoutedEventArgs e) {

            try
            {
                SqlDataReader drCli = SiaWin.Func.SqlDR("select nom_configuracion,con_configuracion from CrMae_configuracion where idrow=1", idemp); 
                string correoLecollezioni = "";string contraseLecollezioni = "";
                while (drCli.Read()){correoLecollezioni = drCli["nom_configuracion"].ToString().Trim();contraseLecollezioni = drCli["con_configuracion"].ToString().Trim();}


                var reflector = this.dataGridMes.View.GetPropertyAccessProvider();
                foreach (var row in this.dataGridMes.SelectedItems)
                {
                    foreach (var column in dataGridMes.Columns)
                    {
                        var cellvalue = reflector.GetValue(row, column.MappingName);
                        var email = dataGridMes.Columns["email"].MappingName;
                        var nombre = dataGridMes.Columns["nom_ter"].MappingName;


                        var email_cli = reflector.GetValue(row, email.Trim());
                        var nombre_cli = reflector.GetValue(row, nombre.Trim());

                        //MessageBox.Show("nombre_cli:" + nombre_cli);
                        //MessageBox.Show("correoLecollezioni:" + correoLecollezioni);
                        //MessageBox.Show("contraseLecollezioni:" + contraseLecollezioni);
                        //MessageBox.Show("email_cli:" + email_cli);
                        SiaWin.Func.funcionCorreoDescuento(nombre_cli.ToString(), correoLecollezioni, contraseLecollezioni, email_cli.ToString());
                        break;
                        
                    }
                }
            }            
            catch (Exception w)
            {
                MessageBox.Show("Error correo promocional" + w);                
            }
            


            /*            
            try
            {
                DataRowView row = (DataRowView)dataGridMes.SelectedItems[0];

                string nombre = row["nom_ter"].ToString();
                string correo = row["email"].ToString();

                SqlDataReader drCli = SiaWin.Func.SqlDR("select nom_configuracion,con_configuracion from CrMae_configuracion where idrow=1", idemp); ;

                string correoLecollezioni = "";
                string contraseLecollezioni = "";

                while (drCli.Read())
                {
                    correoLecollezioni = drCli["nom_configuracion"].ToString().Trim();
                    contraseLecollezioni = drCli["con_configuracion"].ToString().Trim();
                }

                if (correo.Length > 0)
                {
                    //MessageBox.Show("simulacion de correo enviada MES DESCUENTO");
                    //SiaWin.Func.funcionCumple(nombre, correoLecollezioni, contraseLecollezioni);
                    //MessageBox.Show("nombre:"+nombre);
                    //MessageBox.Show("correoLecollezioni:" + correoLecollezioni);
                    //MessageBox.Show("contraseLecollezioni:" + contraseLecollezioni);
                    //MessageBox.Show("correo:" + correo);
                    SiaWin.Func.funcionCumple(nombre, correoLecollezioni, contraseLecollezioni, correo);
                }
                else
                {
                    MessageBox.Show("El cliente no tiene correo electronico");
                }
            }
            catch (Exception)
            {

                throw;
            }
            */

        }



      
    }
}
