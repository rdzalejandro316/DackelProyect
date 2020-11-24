﻿using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace AsignacionClientes
{
    
    public partial class Clientes : Window
    {

        dynamic SiaWin;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";

        public string Cven, Nven;

        public Clientes()
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;            
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;

            this.MinHeight = 550;
            this.MaxHeight = 550;
            this.MinWidth = 1200;
            this.MaxWidth = 1200;

            LoadConfig();
            //LlenarGrid();
            
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

                fecha_ini.Text = DateTime.Now.AddMonths(-1).ToString();
                fecha_fin.Text = DateTime.Now.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

       
        public void LlenarGrid(){

            try
            {
                string fe_fin = fecha_fin.Text + " 23:59:59";

                string queryGrid = "select cliente.cod_ter,cliente.nom_ter as nom_ter,cliente.cod_ven as cod_ven, vendedor.nom_mer as nom_mer ,sum( iif(cabeza.cod_trn between '004' and '005',CAST(cuerpo.cantidad as int),CAST(-cuerpo.cantidad as int) ) ) as cantidad, sum((cantidad*val_uni)*iif(trn.tip_trn=1,-1,1)) as monto, max(iif(cabeza.cod_trn='005',fec_trn,'')) as ultfecha ";
                queryGrid = queryGrid + "from InCab_doc as cabeza ";
                queryGrid = queryGrid + "inner join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
                queryGrid = queryGrid + "inner join inmae_bod as bod on bod.cod_bod=cuerpo.cod_bod ";
                queryGrid = queryGrid + "inner join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
                queryGrid = queryGrid + "inner join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";
                queryGrid = queryGrid + "inner join comae_ter as cliente on cliente.cod_ter = cabeza.cod_cli ";
                queryGrid = queryGrid + "full join InMae_mer as vendedor on vendedor.cod_mer = cliente.cod_ven ";
                queryGrid = queryGrid + "inner join CrMae_cli as cliCamp on cliCamp.cod_ter = cliente.cod_ter ";
                queryGrid = queryGrid + "inner join InMae_trn trn on trn.cod_trn=cabeza.cod_trn and trn.ind_vtas=1 and trn.Tip_trn between 1 and 2 ";
                queryGrid = queryGrid + "where cabeza.cod_trn between '004' and '008' and cliente.clasific='1' ";
                queryGrid = queryGrid + "and cabeza.fec_trn  between '" + fecha_ini.Text + "' and '" + fe_fin + "' ";
                queryGrid = queryGrid + "group by cliente.nom_ter,cliente.cod_ter,cliente.cod_ven,vendedor.nom_mer ";
                queryGrid = queryGrid + "order by nom_ter";


                DataTable dt = SiaWin.Func.SqlDT(queryGrid, "ClientesVendedores", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;
                totalCli.Text = dt.Rows.Count.ToString();
            }
            catch (Exception w)
            {
                MessageBox.Show("error grid: "+w);
            }

        }
                    
        private void Click_Asignar(object sender, RoutedEventArgs e) {
            try
            {
                
                string queryUPD_TER = "";
                string nombres_todos = "";

                var reflector = this.dataGridCxC.View.GetPropertyAccessProvider();
                foreach (var row in this.dataGridCxC.SelectedItems)
                {                    
                    foreach (var column in dataGridCxC.Columns)
                    {
                        var cellvalue = reflector.GetValue(row, column.MappingName);
                        var nombre = dataGridCxC.Columns[1].MappingName; 
                        
                        var nombre_cli = reflector.GetValue(row, nombre.Trim());
                        nombres_todos = nombres_todos.Trim() + "- " + nombre_cli;

                        queryUPD_TER = queryUPD_TER + "update comae_ter set cod_ven='" + LB_vededor.Text + "' where cod_ter='" + cellvalue + "' ";                        
                        break;
                    }
                }
                
                SiaWin.Func.SqlDT(queryUPD_TER, "Clientes", idemp);
                MessageBox.Show("Asignacion de Vendedor " + TBvededor.Text.Trim() + " a los Clientes " + nombres_todos.Trim() + " Exitosa");                
                LlenarGrid();                
            }
            catch (Exception w)
            {
                MessageBox.Show("Error Seleciona un Cliente:"+w);
            }
                                 
        }

        private void BTNbuscar_Click(object sender, RoutedEventArgs e)
        {
            LlenarGrid();
            BTNasignar.IsEnabled = true;
        }

        private void BTNexportar_Click(object sender, RoutedEventArgs e)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TBvededor.Text = Nven;
            LB_vededor.Text = Cven;            
        }






    }
}
