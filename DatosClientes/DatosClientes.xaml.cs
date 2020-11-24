using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
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

    public partial class DatosClientes : UserControl
    {


        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";


        public DatosClientes(dynamic tabitem1)
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
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
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
                tabitem.Logo(idLogo, ".png");
                tabitem.Title = "Datos de los Clientes(" + aliasemp + ")";

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void TBCliente_PreviewKeyDown(object sender, KeyEventArgs e){
            try
            {
                if (e.Key == Key.F8)
                {
                    //validacion para que se ingrese fijo el campo de una maestra
                    string idTab = ((TextBox)sender).Tag.ToString();
                    if (idTab.Length > 0)
                    {
                        string tag = ((TextBox)sender).Tag.ToString();
                        string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = false; string cmpwhere = "";
                        if (string.IsNullOrEmpty(tag)) return;

                        if (tag == "comae_ter")
                        {
                            cmptabla = tag; cmpcodigo = "cod_ter"; cmpnombre = "UPPER(nom_ter)"; cmporden = "cod_ter"; cmpidrow = "idrow"; cmptitulo = "Maestra de Terceros"; cmpconexion = cnEmp; mostrartodo = false; cmpwhere = "clasific=1";
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
                            if (tag == "comae_ter")
                            {
                                LB_cliente.Text = code; TBX_cliente.Text = nom;
                                Consultar.IsEnabled = true;

                            }

                            var uiElement = e.OriginalSource as UIElement;
                            uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        }
                        e.Handled = true;
                    }
                }
                
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


        private void Consultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cadena = "SELECT	rtrim(TER.cod_ter) as cod_ter, rtrim(TER.tdoc) as tdoc, rtrim(UPPER(IDENTIFICACION.nom_tdoc)) as nom_tdoc, rtrim(TER.nom_ter) as nom_ter,rtrim(UPPER(TER.nom1)) as nom1,rtrim(UPPER(TER.nom2)) as nom2,rtrim(UPPER(TER.apell1)) as apell1,rtrim(UPPER(TER.apell2)) as apell2,rtrim(TER.tel1) as tel1,rtrim(TER.tel2) as tel2,rtrim(TER.cel) as cel,rtrim(UPPER(TER.email)) as email,rtrim(UPPER(TER.dir1)) as dir1,rtrim(UPPER(TER.dir)) as dir,rtrim(UPPER(TER.dir2)) as dir2,rtrim(TER.cod_ciu) as cod_ciu, rtrim(UPPER(MUNICIPIO.nom_muni)) as nom_muni,rtrim(TER.cod_depa) as cod_depa, rtrim(UPPER(DEPARTAMENTO.nom_dep)) as nom_dep,CONVERT(varchar,fec_cump,103) as fec_cump, (cast(datediff(dd,TER.fec_cump,GETDATE()) / 365.25 as int)) as edad, ";
                cadena = cadena + "rtrim(CLIE.genero) as genero,IIF(est_civil ='1','SOLTERO',IIF(est_civil ='2','CASADO',IIF(est_civil ='3','UNION LIBRE',IIF(est_civil ='4','SEPARADO',IIF(est_civil ='5','VIUDO',''))))) AS est_civil,rtrim(UPPER(CLIE.nom_emp)) as nom_emp, rtrim(UPPER(CLIE.act_emp)) as act_emp,rtrim(UPPER(ACTIVIDAD.nom_actEmp)) as nom_actEmp,iif(CLIE.ct_cel='1','SI',iif(CLIE.ct_cel='0','NO','')) as ct_cel,iif(CLIE.ct_email='1','SI',iif(CLIE.ct_email='0','NO','')) as ct_email,iif(CLIE.ct_whats='1','SI',iif(CLIE.ct_whats='0','NO','')) as ct_whats,iif(CLIE.ct_sms='1','SI',iif(CLIE.ct_sms='0','NO','')) as ct_sms,iif(CLIE.ct_corres='1','SI',iif(CLIE.ct_corres='0','NO','')) as ct_corres,rtrim(UPPER(CARGO.cod_cargo)) as cod_cargo,rtrim(UPPER(CARGO.nom_cargo)) as nom_cargo, rtrim(UPPER(OCUPACION.cod_ocup)) as cod_ocup,rtrim(UPPER(OCUPACION.nom_ocup)) as nom_ocup, rtrim(UPPER(PROFESION.cod_prof)) as  cod_prof, rtrim(UPPER(PROFESION.nom_prof)) as nom_prof, rtrim(CLIE.num_doc) as num_doc, rtrim(UPPER(TER.observ)) as observ, rtrim(UPPER(CLIE.hobbies)) as hobbies, rtrim(UPPER(CLIE.image_name)) as image_name, CLIE.img_cli as img_cli, rtrim(CLIE.ran_edad) as ran_edad, rtrim(VENDEDOR.nom_mer) as nom_mer ";
                cadena = cadena + "FROM CrMae_cli as CLIE  ";
                cadena = cadena + "full join CrMae_cargo as CARGO on CLIE.cod_cargo = CARGO.cod_cargo ";
                cadena = cadena + "full join CrMae_ocupacion as OCUPACION on CLIE.cod_ocup = OCUPACION.cod_ocup ";
                cadena = cadena + "full join CrMae_profesion as PROFESION on CLIE.cod_prof = PROFESION.cod_prof ";
                cadena = cadena + "full join CrMae_ActEmp as ACTIVIDAD  on ACTIVIDAD.cod_actEmp = CLIE.act_emp, ";
                cadena = cadena + "COMAE_TER as TER ";
                cadena = cadena + "full join MmMae_muni as MUNICIPIO on TER.cod_ciu = MUNICIPIO.cod_muni ";
                cadena = cadena + "full join MmMae_depa as DEPARTAMENTO on TER.cod_depa = DEPARTAMENTO.cod_dep ";
                cadena = cadena + "full join MmMae_iden as IDENTIFICACION on TER.tdoc = IDENTIFICACION.cod_tdoc ";
                cadena = cadena + "full join InMae_mer as VENDEDOR on  VENDEDOR.cod_mer = TER.cod_ven ";
                cadena = cadena + "where TER.clasific = 1 and CLIE.cod_ter = TER.cod_ter and TER.cod_ter='" + LB_cliente.Text + "'  ORDER BY cod_ter";

                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;

                Exportar.IsEnabled = true;
            }
            catch (Exception w)
            {
                MessageBox.Show("Error al Cargar Cliente: "+w);
            }


        }

        private void Exportar_Click(object sender, RoutedEventArgs e)
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



    }
}
