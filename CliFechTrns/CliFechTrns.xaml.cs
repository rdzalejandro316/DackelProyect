using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
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

    //Sia.PublicarPnt(9465,"CliFechTrns");
    //Sia.TabU(9465);

    public partial class CliFechTrns : UserControl
    {
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string vendedor = "";        
        string cnEmp = "";
        public string Conexion;        


        public CliFechTrns(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            
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
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
                tabitem.Logo(idLogo, ".png");
                tabitem.Title = "Informe Fecha de Trns (" + aliasemp + ")";

                TextBx_fecha_ini.Text = DateTime.Today.AddMonths(-1).ToString();
                TextBx_fecha_fin.Text = DateTime.Now.ToShortDateString();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private async void CargarGrid(object sender, RoutedEventArgs e)
        {
            
            try
            {                
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;                
                sfBusyIndicator.IsBusy = true;

                dataGridCxC.ItemsSource = null;
                BTnconsultar.IsEnabled = false;                
                

                string ffi = TextBx_fecha_ini.Text.ToString();
                string fff = TextBx_fecha_fin.Text.ToString();
                var slowTask = Task<DataTable>.Factory.StartNew(() => SlowDude(ffi, fff,source.Token), source.Token);
                await slowTask;
                
                BTnconsultar.IsEnabled = true;                
                if (((DataTable)slowTask.Result).Rows.Count > 0)
                {
                    dataGridCxC.ItemsSource = ((DataTable)slowTask.Result).DefaultView;
                    TotalReg.Text = ((DataTable)slowTask.Result).Rows.Count.ToString();                    
                }

                this.sfBusyIndicator.IsBusy = false;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Opacity = 1;
                MessageBox.Show("aqui 2" + ex);

            }

            

        }


        private DataTable SlowDude(string ffi, string fff,  CancellationToken cancellationToken)
        {
            try
            {

                DataTable jj = LoadData(ffi, fff, cancellationToken);
                return jj;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        private DataTable LoadData(string Fi, string Ff, CancellationToken cancellationToken)
        {
            try
            {

                string cadena = "select TER.cod_ter as cod_ter,TER.nom_ter as nom_ter,rtrim(UPPER(TER.nom1)) as nom1,rtrim(UPPER(TER.nom2)) as nom2,rtrim(UPPER(TER.apell1)) as apell1,rtrim(UPPER(TER.apell2)) as apell2,rtrim(TER.tel1) as tel1,rtrim(TER.tel2) as tel2,rtrim(TER.cel) as cel,rtrim(UPPER(TER.email)) as email,rtrim(UPPER(TER.dir1)) as dir1,CONVERT(varchar,fec_cump,103) as fec_cump, (cast(datediff(dd,TER.fec_cump,GETDATE()) / 365.25 as int)) as edad, rtrim(CLIE.genero) as genero,IIF(est_civil ='1','SOLTERO',IIF(est_civil ='2','CASADO',IIF(est_civil ='3','UNION LIBRE',IIF(est_civil ='4','SEPARADO',IIF(est_civil ='5','VIUDO',''))))) AS est_civil,iif(CLIE.ct_cel='1','SI',iif(CLIE.ct_cel='0','NO','')) as ct_cel,iif(CLIE.ct_email='1','SI',iif(CLIE.ct_email='0','NO','')) as ct_email,iif(CLIE.ct_whats='1','SI',iif(CLIE.ct_whats='0','NO','')) as ct_whats,iif(CLIE.ct_sms='1','SI',iif(CLIE.ct_sms='0','NO','')) as ct_sms,iif(CLIE.ct_corres='1','SI',iif(CLIE.ct_corres='0','NO','')) as ct_corres,TER.cod_ven as cod_ven, vendedor.nom_mer as nom_mer,sum( iif(cabeza.cod_trn between '004' and '005',CAST(cuerpo.cantidad as int),CAST(-cuerpo.cantidad as int) ) ) as cantidad, sum((cantidad*val_uni)*iif(trn.tip_trn=1,-1,1)) as monto, max(iif(cabeza.cod_trn='005',fec_trn,'')) as ultfecha ";
                cadena += "from InCab_doc as cabeza ";
                cadena += "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
                cadena += "inner join inmae_bod as bod on bod.cod_bod=cuerpo.cod_bod ";
                cadena += "inner join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
                cadena += "inner join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";
                cadena += "inner join comae_ter as TER on TER.cod_ter = cabeza.cod_cli ";
                cadena += "full join InMae_mer as vendedor on vendedor.cod_mer = TER.cod_ven ";
                cadena += "inner join CrMae_cli as CLIE on CLIE.cod_ter = TER.cod_ter ";
                cadena += "inner join InMae_trn trn on trn.cod_trn=cabeza.cod_trn and trn.ind_vtas=1 and trn.Tip_trn between 1 and 2 ";
                cadena += "where cabeza.cod_trn between '004' and '008' and TER.clasific='1' ";
                cadena += "and cabeza.fec_trn  between '"+Fi+"' and '"+Ff+" 23:59:59'  ";
                cadena += "group by TER.cod_ter,TER.nom_ter,TER.cod_ven,vendedor.nom_mer,TER.nom1,TER.nom2,TER.apell1,TER.apell2,TER.tel1,TER.tel2,TER.cel,TER.email,TER.dir1,TER.fec_cump,CLIE.genero,CLIE.est_civil,CLIE.ct_cel,CLIE.ct_email,CLIE.ct_whats,CLIE.ct_sms,CLIE.ct_corres ";
                cadena += "order by nom_ter ";

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                return dt;               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }


        private void ExportaXLS_Click(object sender, RoutedEventArgs e)
        {

            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = dataGridCxC.ExportToExcel(dataGridCxC.View, options);
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


    //    try
    //        {
                
    //            string cadena = "select TER.cod_ter as cod_ter,TER.nom_ter as nom_ter,rtrim(UPPER(TER.nom1)) as nom1,rtrim(UPPER(TER.nom2)) as nom2,rtrim(UPPER(TER.apell1)) as apell1,rtrim(UPPER(TER.apell2)) as apell2,rtrim(TER.tel1) as tel1,rtrim(TER.tel2) as tel2,rtrim(TER.cel) as cel,rtrim(UPPER(TER.email)) as email,rtrim(UPPER(TER.dir1)) as dir1,CONVERT(varchar,fec_cump,103) as fec_cump, (cast(datediff(dd,TER.fec_cump,GETDATE()) / 365.25 as int)) as edad, rtrim(CLIE.genero) as genero,IIF(est_civil ='1','SOLTERO',IIF(est_civil ='2','CASADO',IIF(est_civil ='3','UNION LIBRE',IIF(est_civil ='4','SEPARADO',IIF(est_civil ='5','VIUDO',''))))) AS est_civil,iif(CLIE.ct_cel='1','SI',iif(CLIE.ct_cel='0','NO','')) as ct_cel,iif(CLIE.ct_email='1','SI',iif(CLIE.ct_email='0','NO','')) as ct_email,iif(CLIE.ct_whats='1','SI',iif(CLIE.ct_whats='0','NO','')) as ct_whats,iif(CLIE.ct_sms='1','SI',iif(CLIE.ct_sms='0','NO','')) as ct_sms,iif(CLIE.ct_corres='1','SI',iif(CLIE.ct_corres='0','NO','')) as ct_corres,TER.cod_ven as cod_ven, vendedor.nom_mer as nom_mer,sum( iif(cabeza.cod_trn between '004' and '005',CAST(cuerpo.cantidad as int),CAST(-cuerpo.cantidad as int) ) ) as cantidad, sum((cantidad*val_uni)*iif(trn.tip_trn=1,-1,1)) as monto, max(iif(cabeza.cod_trn='005',fec_trn,'')) as ultfecha ";
    //    cadena += "from InCab_doc as cabeza ";
    //            cadena += "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
    //            cadena += "inner join inmae_bod as bod on bod.cod_bod=cuerpo.cod_bod ";
    //            cadena += "inner join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
    //            cadena += "inner join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";
    //            cadena += "inner join comae_ter as TER on TER.cod_ter = cabeza.cod_cli ";
    //            cadena += "full join InMae_mer as vendedor on vendedor.cod_mer = TER.cod_ven ";
    //            cadena += "inner join CrMae_cli as CLIE on CLIE.cod_ter = TER.cod_ter ";
    //            cadena += "inner join InMae_trn trn on trn.cod_trn=cabeza.cod_trn and trn.ind_vtas=1 and trn.Tip_trn between 1 and 2 ";
    //            cadena += "where cabeza.cod_trn between '004' and '008' and TER.clasific='1' ";
    //            cadena += "and cabeza.fec_trn  between '" + TextBx_fecha_ini.Text + "' and '" + TextBx_fecha_fin.Text + " 23:59:59'  ";
    //            cadena += "group by TER.cod_ter,TER.nom_ter,TER.cod_ven,vendedor.nom_mer,TER.nom1,TER.nom2,TER.apell1,TER.apell2,TER.tel1,TER.tel2,TER.cel,TER.email,TER.dir1,TER.fec_cump,CLIE.genero,CLIE.est_civil,CLIE.ct_cel,CLIE.ct_email,CLIE.ct_whats,CLIE.ct_sms,CLIE.ct_corres ";
    //            cadena += "order by nom_ter ";

    //            DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
    //    dataGridCxC.ItemsSource = dt.DefaultView;

    //            if (dt.Rows.Count <= 0) { MessageBox.Show("No existe registros en el rango de fecha estipulado"); }
    //TotalReg.Text = dt.Rows.Count.ToString();

    //            BTNexpo.IsEnabled = true;
    //        }
    //        catch (Exception w)
    //        {

    //            MessageBox.Show("error:" + w);
    //        }



    }
}

