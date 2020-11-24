using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;

namespace SiasoftAppExt
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class PvReimprimeFacturaNC : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string codbod = "";
        string codpvta = "";
        string nompvta = "";
        string cnEmp = "";
        DataSet ds = new DataSet();
        DataTable dd = new DataTable();
        DataTable dtBod = new DataTable();
        public string codtrn = "";
        public int idrowcab = 0;
        

        public PvReimprimeFacturaNC()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            codpvta = SiaWin._UserTag;
            LoadInfo();
            this.DataContext = this;
            FechaIni.Text = DateTime.Now.ToShortDateString();
            FechaFin.Text = DateTime.Now.ToShortDateString();
           
        }
        private Ref RefgdcSource = new Ref();
        public Ref RefGDCSource
        {
            get { return RefgdcSource; }
            set { RefgdcSource = value; }
        }

        public void LoadInfo()
        {
            try
            {
                RefGDCSource.Add(new Referencia() { nom_ref = "--" });
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                //        System.Windows.Threading.Dispatcher.BeginInvoke(new A
                //Img.Source= AppDomain.CurrentDomain.BaseDirectory + "Imagenes\\" + idLogo.ToString() + "..png";
                //        ConfigCSource.PathImgBusiness=AppDomain.CurrentDomain.BaseDirectory + "Imagenes\\"+idLogo.ToString()+"..png";
                //        ConfigCSource.nomBuss = ((Inicio)Application.Current.MainWindow).Func.cmp("business","BusinessId","BusinessName",idEmp,0);
                //        _usercontrol.Seg.Auditor(0,_usercontrol.ProjectId,idUser,_usercontrol.GroupId,idEmp,_usercontrol.ModuleId,_usercontrol.AccesoId,0,"Ingreso a: Punto de venta"+" - " +_titulo,"");
                if (codpvta == string.Empty)
                {
                    //_usercontrol.Opacity = 0.5;
                    MessageBox.Show("El usuario no tiene asignado un punto de venta, Pantalla Bloqueada");
                    //_usercontrol.IsEnabled=false;
                }
                else
                {
                    nompvta = SiaWin.Func.cmpCodigo("copventas", "cod_pvt", "nom_pvt", codpvta, idemp);
                    codbod = SiaWin.Func.cmpCodigo("copventas", "cod_pvt", "cod_bod", codpvta, idemp);
                    if (string.IsNullOrEmpty(codbod))
                    {
                        //_usercontrol.Opacity = 0.5;
                        MessageBox.Show("El punto de venta Asignado no tiene bodega , Pantalla Bloqueada");
                        //usercontrol.IsEnabled=false;
                    }

                }

              //  dtBod = SiaWin.Func.SqlDT("select cod_bod,cod_bod+'-'+nom_bod as nom_bod,tipo_bod,aut_ent_trasl from inmae_bod where estado=1  order by cod_bod", "inmae_ref", idemp);
               // dtBod.PrimaryKey = new DataColumn[] { dtBod.Columns["cod_bod"] };
               // dd = dtBod.Select("cod_bod=" + codbod).CopyToDataTable();
               // DataTable d1 = dtBod.Select("cod_bod<>" + codbod).CopyToDataTable();

               // LlenaCombo(CmbBodOrigen, dd, "cod_bod", "nom_bod");
            
                // establecer paths
            
            

                //LlenaCombo(CmbBodDestino, dtComboBodDestino, "cod_bod", "nom_bod");
//                CmbBodOrigen.SelectedValue = codbod;
                //dd = null; d1 = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

            private void Ejecutar_Click(object sender, RoutedEventArgs e)
            {
                int _TipoDoc = CmbTipoDoc.SelectedIndex;
                if (_TipoDoc < 0)
                {
                    MessageBox.Show("Seleccione un Tipo de Documento..");
                    CmbTipoDoc.Focus();
                    CmbTipoDoc.IsDropDownOpen = true;
                    return;
                }
            string codtrn = "005";
            if (_TipoDoc == 0) codtrn = "004";
            if (_TipoDoc == 1) codtrn = "005";
            if (_TipoDoc == 2) codtrn = "007";
            if (_TipoDoc == 3) codtrn = "008";
            if (_TipoDoc == 4) codtrn = "010";
            if (_TipoDoc == 5) codtrn = "011";
            // validar fecha
            LoadData(codtrn);
            }

        private void ReImprimir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("USTED DESEA HACER CIERRE FINAL...?", "Siasoft?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            DataRow dr = ds.Tables["Traslados"].Rows[dataGridSF.SelectedIndex];
            if (dr != null)
            {
                string numtrn = dr["idreg"].ToString();
                string cod_trn = dr["cod_trn"].ToString();
                codtrn = cod_trn;
                idrowcab = Convert.ToInt32(numtrn);
                this.Close();
                //ImprimeDocumento(cod_trn,Convert.ToInt32(numtrn), "Reimpreso");
            }
        }
        private void LoadData(string tipodoc)
        {
            if (string.IsNullOrEmpty(tipodoc)) return;
            using (SqlConnection connection = new SqlConnection(cnEmp))
            {
                connection.Open();
                //connectionString.Open();
                //DataSet ds = new DataSet();
                StringBuilder _sql = new StringBuilder();
                ds.Clear();
                _sql.Append("select cab.idreg,cab.cod_trn,cab.num_trn,cab.fec_trn,cab.cod_cli,rtrim(ter.nom_ter) as nom_cli,cue.cod_bod,cue.cod_ref,rtrim(ref.nom_ref) as nom_ref,sum(cue.cantidad) as cantidad,max(trn.tip_trn) as tip_trn from incue_doc as cue ");
                _sql.Append(" inner join incab_doc as cab on cab.idreg = cue.idregcab and cab.cod_trn='"+tipodoc+"' inner join inmae_ref as ref on ref.cod_ref = cue.cod_ref inner join inmae_bod as bod on bod.cod_bod = cue.cod_bod ");
                _sql.Append(" inner join comae_ter as ter on cab.cod_cli = ter.cod_ter ");
                _sql.Append(" inner join inmae_trn as trn on trn.cod_trn=cab.cod_trn and trn.ind_vtas=1  where convert(date,cab.fec_trn) between '" + FechaIni.Text + "' and '" + FechaFin.Text + "'");
                _sql.Append(" and cue.cod_bod = '" + codbod.Trim() + "' group by cab.idreg,cab.cod_trn,cab.num_trn,cab.fec_trn,cab.cod_cli,ter.nom_ter,cue.cod_bod,cue.cod_ref,nom_ref order by cab.cod_trn,cab.fec_trn ");
                SqlDataAdapter adapter = new SqlDataAdapter(_sql.ToString(), connection);
                adapter.Fill(ds, "Traslados");
                dataGridSF.ItemsSource = ds.Tables["Traslados"];
                TextTotalEntradas.Text = ds.Tables["Traslados"].Compute("Sum(cantidad)", "").ToString();
                //TextTotalSalidas.Text = ds.Tables["Traslados"].Compute("Sum(cantidad)", "cod_trn<>'005'").ToString();
            }
        }
        private void ImprimeDocumento(string codtrn,int iddoc,string reimpreso)
        {
            string fpago = TraerFormasPago(iddoc);
            string[] strArrayParam = new string[] { iddoc.ToString(), idemp.ToString(), fpago, codtrn };
            SiaWin.Tab(9294, strArrayParam);
            //if(tipodoc=="x005")
            //        {
            //           string fpago = TraerFormasPago(ConfigCSource.cod_trn.ToString(),ConfigCSource.NumDoc.ToString());
            //          string[] strArrayParam = new string[] { ConfigCSource.cod_trn,ConfigCSource.NumDoc.ToString(),fpago, idEmp.ToString()};  
            //        ((Inicio)Application.Current.MainWindow).Tab(9219,strArrayParam);
            //   ((Inicio)Application.Current.MainWindow).ValReturn=null;
            //      }
        }
        private string TraerFormasPago(int iddoc)
        {
            List<string> cod_pag = new List<string> { };
            List<string> nom_pag = new List<string> { };
            List<string> valor = new List<string> { };
            List<string> doc_ref = new List<string> { };
            string _cFpago = "";
            string _Sql = "select InCab_doc.cod_cli,InCab_doc.num_trn, InMae_fpag.cod_pag,inmae_fpag.nom_pag,indet_fpag.vlr_pagado,indet_fpag.doc_ref from InDet_fpag inner join InMae_fpag on inmae_fpag.cod_pag=indet_fpag.cod_pag ";
            _Sql = _Sql + " inner join InCab_doc on InCab_doc.idreg=InDet_fpag.idregcab where incab_doc.idreg=" + iddoc.ToString() + "  order by cod_pag ";

            try
            {
                if (_Sql == string.Empty) return "Sin Forma de Pago";
                SqlDataReader dr = SiaWin.Func.SqlDR(_Sql, idemp);
                while (dr.Read())
                {
                    cod_pag.Add(dr["cod_pag"].ToString()); nom_pag.Add(dr["nom_pag"].ToString()); valor.Add(dr["vlr_pagado"].ToString()); doc_ref.Add(dr["doc_ref"].ToString());
                    if (_cFpago != "") _cFpago = _cFpago + System.Environment.NewLine;
                    _cFpago = _cFpago + dr["cod_pag"].ToString() + dr["nom_pag"].ToString() + dr["vlr_pagado"].ToString() + dr["doc_ref"].ToString();
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.Exception _error)
            {
                MessageBox.Show(_error.Message);
            }
            string[] _cod_pag = cod_pag.ToArray();
            string[] _nom_pag = nom_pag.ToArray();
            string[] _valor = valor.ToArray();
            string[] _doc_ref = doc_ref.ToArray();
            String s = String.Format("{0,-10} {1,10} {2,20} {3,30}\n", "Codigo", "Nombre", "Valor", "Documento");
            for (int index = 0; index < _cod_pag.ToArray().Length; index++)
                s += String.Format("{0,-10} {1,10}  {2,-10:N0} {3,30} \n", _cod_pag[index], _nom_pag[index], _valor[index], _doc_ref[index]);
            //        MessageBox.Show(s);
            return s;
        }



        private void ImprimirDoc(string cod_trn,int idregcab, string tipoImp)
        {
            string[] strArrayParam = new string[] { idregcab.ToString(), idemp.ToString(), tipoImp, };
            SiaWin.Tab(9291, strArrayParam);
            //((Inicio)Application.Current.MainWindow).Tab(9279);832005853
            //if(usercontrol.Tag.ToString()=="-1")
            //{
            // ((Inicio)Application.Current.MainWindow).Tab(9279);
            //MessageBox.Show("ddd");
            //   e.Handled = true;
            // return;
            //}
        }
        public class Referencia : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string property)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
            int _item;
            public int item { get { return _item; } set { _item = value; OnPropertyChanged("item"); } }
            int _idrow;
            public int idrow { get { return _idrow; } set { _idrow = value; OnPropertyChanged("idrow"); } }
            string _nom_ref;
            public string nom_ref { get { return _nom_ref; } set { _nom_ref = value; OnPropertyChanged("nom_ref"); } }
            string _cod_ref;
            public string cod_ref { get { return _cod_ref; } set { _cod_ref = value; OnPropertyChanged("cod_ref"); } }
            string _cod_bod;
            public string cod_bod { get { return _cod_bod; } set { _cod_bod = value; OnPropertyChanged("cod_bod"); } }
            double _val_ref;
            public double val_ref { get { return _val_ref; } set { _val_ref = value; OnPropertyChanged("val_ref"); OnPropertyChanged("subtotal"); subtotal = _cantidad * _val_ref; OnPropertyChanged("valdescto"); valdescto = (subtotal * _pordescto) / 100; OnPropertyChanged("valiva"); valiva = Math.Round(((subtotal - valdescto) * _poriva) / 100, 0); OnPropertyChanged("total"); total = Math.Round((subtotal - valdescto + valiva), 0); } }
            double _cantidad;
            public double cantidad { get { return _cantidad; } set { _cantidad = value; OnPropertyChanged("cantidad"); OnPropertyChanged("subtotal"); subtotal = _cantidad * _val_ref; OnPropertyChanged("valdescto"); valdescto = Math.Round((subtotal * _pordescto) / 100, 0); OnPropertyChanged("valiva"); valiva = Math.Round(((subtotal - valdescto) * _poriva) / 100, 0); OnPropertyChanged("total"); total = Math.Round((subtotal - valdescto + valiva), 0); } }
            double _subtotal;
            public double subtotal { get { return _subtotal; } set { _subtotal = value; OnPropertyChanged("subtotal"); } }
            double _pordescto;
            public double pordescto { get { return _pordescto; } set { _pordescto = value; OnPropertyChanged("pordescto"); OnPropertyChanged("subtotal"); subtotal = _cantidad * _val_ref; OnPropertyChanged("valdescto"); valdescto = Math.Round((subtotal * _pordescto) / 100, 0); OnPropertyChanged("valiva"); valiva = Math.Round(((subtotal - valdescto) * _poriva) / 100, 0); OnPropertyChanged("total"); total = Math.Round((subtotal - valdescto + valiva), 0); } }
            double _valdescto;
            public double valdescto { get { return _valdescto; } set { _valdescto = value; OnPropertyChanged("valdescto"); } }
            string _cod_tiva;
            public string cod_tiva { get { return _cod_tiva; } set { _cod_tiva = value; OnPropertyChanged("cod_tiva"); } }
            double _poriva;
            public double poriva { get { return _poriva; } set { _poriva = value; OnPropertyChanged("poriva"); OnPropertyChanged("subtotal"); subtotal = _cantidad * _val_ref; OnPropertyChanged("valdescto"); valdescto = Math.Round((subtotal * _pordescto) / 100, 0); OnPropertyChanged("valiva"); valiva = Math.Round(((subtotal - valdescto) * _poriva) / 100, 0); OnPropertyChanged("total"); total = Math.Round((subtotal - valdescto + valiva), 0); } }
            double _valiva;
            public double valiva { get { return _valiva; } set { _valiva = value; OnPropertyChanged("valiva"); } }
            double _total;
            public double total { get { return _total; } set { _total = value; OnPropertyChanged("total"); } }
            double _salref = 0.00;
            public double salref { get { return _salref; } set { _salref = value; OnPropertyChanged("salref"); } }
            DateTime _fechahora;
            public DateTime fechahora { get { return _fechahora; } set { _fechahora = value; OnPropertyChanged("fechahora"); } }
            bool _Estado = false;
            public bool Estado { get { return _Estado; } set { _Estado = value; OnPropertyChanged("Estado"); } }
            string _nom_tip;
            public string nom_tip { get { return _nom_tip; } set { _nom_tip = value; OnPropertyChanged("nom_tip"); } }
            string _nom_prv;
            public string nom_prv { get { return _nom_prv; } set { _nom_prv = value; OnPropertyChanged("nom_prv"); } }
        }
        public class Ref : ObservableCollection<Referencia>
        {
            //ObservableCollection<Referencia> Referencias = new ObservableCollection<Referencia>();
            public double Total()
            {
                double _tuni = 0;
                foreach (var item in this)
                {
                    _tuni += item.cantidad;
                }
                return _tuni;
            }
        }




    }
}
