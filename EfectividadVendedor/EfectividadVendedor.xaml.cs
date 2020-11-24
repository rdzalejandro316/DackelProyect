using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SiasoftAppExt
{
    //Sia.PublicarPnt(9490,"EfectividadVendedor");
    //dynamic w = ((Inicio)Application.Current.MainWindow).WindowExt(9490,"EfectividadVendedor");
    //w.ShowInTaskbar = false;
    //w.Owner = Application.Current.MainWindow;
    //w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //w.ShowDialog();  



    public partial class EfectividadVendedor : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        public EfectividadVendedor()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            LoadConfig();
            pantalla();
            CargarBodega();
        }

        private void LoadConfig()
        {
            try
            {
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
                this.Title = "Efectividad de los vendedores";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "aquiio");
            }
        }

        public void pantalla()
        {
            this.MinWidth = 1300;
            this.MinHeight = 650;
        }
      
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                sfBusyIndicator.IsBusy = true;
                dataGridCxC.ItemsSource = null;
                var slowTask = Task<DataSet>.Factory.StartNew(() => SlowDude(source.Token), source.Token);
                await slowTask;

                if (((DataSet)slowTask.Result).Tables[0].Rows.Count > 0)
                {
                    dataGridCxC.ItemsSource = ((DataSet)slowTask.Result).Tables[0];                    
                }
                this.sfBusyIndicator.IsBusy = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("eror#01:"+ex.Message);                
            }
        }

        public void CargarBodega()
        {
            try
            {
                DataTable Bodegas = SiaWin.Func.SqlDT("select cod_bod,nom_bod from InMae_bod", "Empresas", idemp);
                CBX_bodega.ItemsSource = Bodegas.DefaultView;
            }
            catch (Exception)
            {                       
                MessageBox.Show("error al cargar las bodegas");
            }            
        }

        private DataSet SlowDude(CancellationToken cancellationToken)
        {
            try
            {
                DataSet jj = LoadData(cancellationToken);
                return jj;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        private DataSet LoadData(CancellationToken cancellationToken)
        {
            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("powerBiCRM", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception w)
            {
                MessageBox.Show(w.Message);
                return null;
            }

        }

        private void dataGridCxC_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                string vendedor = row["cod_mer"].ToString();

                string cadena = "select count(IIF(seguimiento.cod_con=concepto.cod_con,seguimiento.cod_ter,0)) as total,concepto.nom_con from Crseg_cli as seguimiento ";
                cadena = cadena + "inner join CrMae_concepto as concepto on seguimiento.cod_con= concepto.cod_con ";
                cadena = cadena + "where seguimiento.cod_mer='" + vendedor + "' ";
                cadena = cadena + "group by concepto.nom_con,seguimiento.cod_con ";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "Conceptos", idemp);
                ds.Tables.Add(dt);

                string MesPasado = "select count(IIF(seguimiento.cod_con=concepto.cod_con,seguimiento.cod_ter,0)) as total, 'Total de pasado' as 'totalseg' ";
                MesPasado = MesPasado + "from Crseg_cli as seguimiento ";
                MesPasado = MesPasado + "inner join CrMae_concepto as concepto on seguimiento.cod_con= concepto.cod_con  ";
                MesPasado = MesPasado + "where seguimiento.cod_mer='" + vendedor + "'  ";
                MesPasado = MesPasado + "and seguimiento.fec_seg between '01/12/2018' and '31/12/2018' ";
                DataTable dtMesPasado = SiaWin.Func.SqlDT(MesPasado, "SeguimietoMesPasado", idemp);
                ds.Tables.Add(dtMesPasado);

                string MesActual = "select count(IIF(seguimiento.cod_con=concepto.cod_con,seguimiento.cod_ter,0)) as total, 'Total de seguimientos' as 'totalseg' ";
                MesActual = MesActual + "from Crseg_cli as seguimiento  ";
                MesActual = MesActual + "inner join CrMae_concepto as concepto on seguimiento.cod_con= concepto.cod_con  ";
                MesActual = MesActual + "where seguimiento.cod_mer='" + vendedor + "'  ";
                MesActual = MesActual + "and datepart(YYYY, seguimiento.fec_seg) = datepart(YYYY, getdate()) ";
                MesActual = MesActual + "and datepart(mm, seguimiento.fec_seg) = datepart(mm, getdate()) ";
                DataTable dtMes = SiaWin.Func.SqlDT(MesActual, "SeguimietoMes", idemp);
                ds.Tables.Add(dtMes);
            
                ChartTotal.ItemsSource = ds.Tables[0];
                Chart1.ItemsSource = ds.Tables[1];
                Chart2.ItemsSource = ds.Tables[2];
            }
            catch (Exception w)
            {
                MessageBox.Show("error en la seleccion:" + w);
            }
        }

        private void CBX_bodega_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            chartCircle();
        }

        private void CBfiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            chartCircle();
        }

        public void chartCircle()
        {
            try
            {
                if (CBX_bodega.SelectedValue == null) return;
                
                string bodega = CBX_bodega.SelectedValue.ToString();
                string campaña = " ";                
                var tag = ((ComboBoxItem)CBfiltro.SelectedItem).Tag.ToString();                
                switch (tag)
                {
                    case "1":
                        campaña = " ";
                        break;
                    case "2":
                        campaña = " inner join CrMae_campa as camapa on seguimiento.cod_camp = camapa.cod_camp ";
                        break;
                    case "3":
                        campaña = " inner join CrMae_campa as camapa on seguimiento.cod_camp = camapa.cod_camp and camapa.estado=1 ";
                        break;
                }
                
                DataSet ds = new DataSet();
                string totLLamadas = "select count(cod_ter) as suma,concepto.nom_con from Crseg_cli as seguimiento ";
                totLLamadas = totLLamadas + "inner join CrMae_concepto as concepto on seguimiento.cod_con=concepto.cod_con ";
                totLLamadas = totLLamadas + campaña;
                totLLamadas = totLLamadas + "where seguimiento.cod_con between '01' and '03'  ";
                totLLamadas = totLLamadas + "and seguimiento.cod_bod='" + bodega + "'  ";
                totLLamadas = totLLamadas + "group by concepto.nom_con ";                
                DataTable totLLam = SiaWin.Func.SqlDT(totLLamadas, "SeguimietoLLamadas", idemp);
                ds.Tables.Add(totLLam);
                int SumLLamadas = Convert.ToInt32(ds.Tables[0].Compute("Sum(suma)", ""));
                
                string totVisitasa = "select count(cod_ter) as suma,concepto.nom_con from Crseg_cli as seguimiento ";
                totVisitasa = totVisitasa+ "inner join CrMae_concepto as concepto on seguimiento.cod_con=concepto.cod_con ";
                totVisitasa= totVisitasa+ campaña;
                totVisitasa= totVisitasa+ "where seguimiento.cod_con between '04' and '06'  ";
                totVisitasa= totVisitasa+ "and seguimiento.cod_bod='" + bodega + "'  ";
                totVisitasa= totVisitasa+ "group by concepto.nom_con ";
                DataTable totVis = SiaWin.Func.SqlDT(totVisitasa, "SeguimietoVisitas", idemp);
                ds.Tables.Add(totVis);
                int SumVisitas = Convert.ToInt32(ds.Tables[1].Compute("Sum(suma)", ""));

                TX_TotLLam.Text = SumLLamadas.ToString();
                ChartCircle.ItemsSource = ds.Tables[0];

                TX_TotVis.Text = SumVisitas.ToString();
                ChartCircleVis.ItemsSource = ds.Tables[1];


                string efectividadLLamadas = "select count(cod_ter) as suma,'Total' as 'Total' from Crseg_cli as seguimiento  inner join CrMae_concepto as concepto on seguimiento.cod_con=concepto.cod_con  where seguimiento.cod_con between '01' and '03' and seguimiento.cod_bod='"+bodega+"' ";
                DataTable EfecLLam = SiaWin.Func.SqlDT(efectividadLLamadas, "EfectLlamadas", idemp);
                ds.Tables.Add(EfecLLam);
                string efectividadVisitas = "select count(cod_ter) as suma,'Total' as 'Total' from Crseg_cli as seguimiento  inner join CrMae_concepto as concepto on seguimiento.cod_con=concepto.cod_con  where seguimiento.cod_con between '04' and '06' and seguimiento.cod_bod='" + bodega + "' ";
                DataTable EfecVis = SiaWin.Func.SqlDT(efectividadVisitas, "EfectVisi", idemp);
                ds.Tables.Add(EfecVis);

                double efectividad =  SumLLamadas / SumVisitas;
                TX_TotEfec.Text = efectividad.ToString("P", CultureInfo.InvariantCulture);

                ChartConLLam.ItemsSource = ds.Tables[2];
                ChartConVis.ItemsSource = ds.Tables[3];
            }
            catch (Exception w)
            {
                MessageBox.Show("error al cargar la consulta por bodegas"+w);
            }
        }


    }
}
