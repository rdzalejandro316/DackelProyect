using System;
using System.Collections.Generic;
using System.Data;
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
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>


   public partial class RecibosDeCaja : Window
    {
        string cnEmpresa;
        dynamic SiaWin; 
        public RecibosDeCaja()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            txt2.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            string parametro = SiaWin.ValReturn.ToString();

            MessageBox.Show("Hola Mundo:" + SiaWin.Name.ToString()+" - "+parametro);
            SiaWin.TituloApp = "Recuerdos de constantinopla";
            string sss = ((String)Application.Current.Properties["TiTuloApp"]);
            MessageBox.Show("jjj:" + sss);
            SiaWin.ValReturn = "Fin del proceso";
            DataRow foundRow = SiaWin.Empresas.Rows.Find(SiaWin._BusinessId);
            cnEmpresa = foundRow["BusinessCn"].ToString().Trim();
            SiaWin.ValReturn = cnEmpresa;
            MessageBox.Show(cnEmpresa);
            SiaWin.SiaWindows("WpfControlLibrary1");
            //Window parentWindow = Window.GetWindow(this);

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Prueba de velocidadxxx");
            string __Sql = @"select top 1000 CONVERT(VARCHAR(10),fec_trn,103) AS fecha, RIGHT(fec_trn,7) AS hora ,trn.cod_trn ,cab.num_trn,rtrim(mer.nom_mer) as nom_mer,ter.cod_ter,rtrim(ter.nom_ter) as nom_ter,rtrim(bod.nom_bod) as nom_bod,ref.idrow,ref.cod_ref,rtrim(ref.nom_ref) as nom_ref,case when trn.cod_trn between '004' and '005' then cue.cantidad else cue.cantidad*-1 end as cantidad,cue.val_uni,case when trn.cod_trn between '004' and '005' then cue.subtotal else cue.subtotal*-1 end as subtotal,cue.por_des,case when trn.cod_trn between '004' and '005' then cue.val_des else cue.val_des*-1 end as val_des,cue.por_iva,case when trn.cod_trn between '004' and '005' then cue.val_iva else cue.val_iva*-1 end as val_iva,case when trn.cod_trn between '004' and '005' then cue.subtotal-cue.val_des+cue.val_iva else (cue.subtotal-cue.val_des+cue.val_iva)*-1 end as tot_tot from incab_doc as cab inner join incue_doc as cue on cab.idreg=cue.idregcab inner join inmae_trn as trn on trn.cod_trn=cab.cod_trn inner join inmae_mer as mer on mer.cod_mer=cab.cod_ven inner join comae_ter ter on ter.cod_ter=cab.cod_cli inner join inmae_ref as ref on ref.cod_ref=cue.cod_ref inner join inmae_bod as bod on bod.cod_bod=cue.cod_bod ";
            dataGrid.ItemsSource = null;
            DataTable dt = SiaWin.Func.SqlDT(__Sql, "maestra", 1);
            dataGrid.ItemsSource = dt.DefaultView;
            txt1.Focus();

        }
    }
}
