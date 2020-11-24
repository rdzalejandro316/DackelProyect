using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Converter;
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
    
    public partial class PuntosActulizacion : UserControl
    {
		dynamic SiaWin;
		dynamic tabitem;
		int idemp = 0;
		string vendedor = "";
		int codigoUsuario = 0;
		string cnEmp = "";
		public string Conexion;
		

		public PuntosActulizacion(dynamic tabitem1)
        {
            InitializeComponent();
			
			SiaWin = Application.Current.MainWindow;
			tabitem = tabitem1;
			idemp = SiaWin._BusinessId;
			vendedor = SiaWin._UserAlias;
			codigoUsuario = SiaWin._UserId;
            LoadConfig();
            DatosUsuario();

            fecha_ini.Text = DateTime.Now.ToString();
            fecha_fin.Text = DateTime.Now.ToString();
            bloquearBTN(0);

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
                tabitem.Title = "Puntos por Actulizacion(" + aliasemp + ")";                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void DatosUsuario()
        {            
            SqlDataReader drCli = SiaWin.Func.SqlDR("select * from InMae_mer where cod_mer='" + SiaWin._UserTag1 + "' ", idemp); ;

            while (drCli.Read())
            {
                TxtUser.Text = drCli["nom_mer"].ToString().Trim();
            }

            drCli.Close();
            

        }


        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {         
            try
            {
                //validacion para que se ingrese fijo el campo de una maestra
                string idTab = ((TextBox)sender).Tag.ToString();
        
                if (idTab.Length > 0)
                {        
                    string tag = ((TextBox)sender).Tag.ToString();
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                    if (string.IsNullOrEmpty(tag)) return;

                    if (tag == "inmae_mer")
                    {
                        cmptabla = tag; cmpcodigo = "cod_mer"; cmpnombre = "UPPER(nom_mer)"; cmporden = "cod_mer"; cmpidrow = "cod_mer"; cmptitulo = "Maestra de Vendedores"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "estado=1";
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
                            LBven.Text = code; TXBven.Text = nom.Trim();
                            bloquearBTN(1);
                        }

                        var uiElement = e.OriginalSource as UIElement;
                        uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                    }
                    e.Handled = true;
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


   

        public void bloquearBTN(int e){

            if (e == 0)
            {
                BTNejecutar.IsEnabled = false;
                BTNexportar.IsEnabled = false;
                BTNTodo.IsEnabled = false;
            }
            if (e == 1)
            {
                BTNejecutar.IsEnabled = true;
                BTNexportar.IsEnabled = true;
                BTNTodo.IsEnabled = true;
            }
        }

        private void cargarGrid(object sender, RoutedEventArgs e)
        {
                                    
            try
            {
                string fe_fin = fecha_fin.Text + " 23:59:59";
                string tag = ((Button)sender).Tag.ToString();
             

                string queryGrid = "select IIF(LEN(Puntos.cod_ter)>0,1,0) as regis,Puntos.cod_ter as cod_ter,Clientes.nom_ter as nom_ter,Puntos.cod_mer as cod_mer,vendedores.nom_mer as nom_mer,CONVERT(datetime, fecha_reg, 103) AS fecha_reg,Puntos.cod_punto as cod_punto,Maepuntos.nombre_p as nombre_p, CAST(Puntos.porcentaje AS INT) AS porcentaje,rtrim(Puntos.val_ini) as val_ini,rtrim(Puntos.val_fin) as val_fin ";
                queryGrid = queryGrid + "from CrAct_info as Puntos ";
                queryGrid = queryGrid + "full join Comae_ter as Clientes on Clientes.cod_ter = Puntos.cod_ter ";
                queryGrid = queryGrid + "full join InMae_mer as vendedores on vendedores.cod_mer = Puntos.cod_mer ";
                queryGrid = queryGrid + "full join CrMae_puntos as Maepuntos on Maepuntos.cod_punto = Puntos.cod_punto ";
                               
                if (tag == "BTven")
                {
                    queryGrid = queryGrid + "where fecha_reg  between '" + fecha_ini.Text + "' and '" + fe_fin + "' ";
                    queryGrid = queryGrid + "and Puntos.cod_mer = '" + LBven.Text + "' ";             
                    tag = "";
                }                
                if (tag == "BTtodo")
                {
                    queryGrid = queryGrid + "where Puntos.cod_mer = '" + LBven.Text + "' ";                    
                    tag = "";
                }

                queryGrid = queryGrid + "order by convert(datetime, fecha_reg  , 103) desc ";


                DataTable dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;
                                                
                TotalRegis.Text = dt.Rows.Count.ToString();


                int valor = Convert.ToInt32(dt.Compute("Sum(porcentaje)", "").ToString() );
                //int valor = Convert.ToInt32(dt.Compute("SUM(porcentaje)", int.TryParse));                
                total.Text = valor.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void ExportaXLS_Click(object sender, RoutedEventArgs e) {

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



        private void dataGrid_FilterChanged(object sender, GridFilterEventArgs e)
        {

            var provider = (sender as SfDataGrid).View.GetPropertyAccessProvider();
            var records = (sender as SfDataGrid).View.Records;


            int totalPuntos = 0;
            int registros = 0;            

            for (int i = 0; i < (sender as SfDataGrid).View.Records.Count; i++)
            {
                totalPuntos += Convert.ToInt32(provider.GetValue(records[i].Data, "porcentaje").ToString());
                registros += Convert.ToInt32(provider.GetValue(records[i].Data, "regis").ToString());                
            }

            TotalRegis.Text = registros.ToString();
            total.Text = totalPuntos.ToString();            

        }





    }
}

