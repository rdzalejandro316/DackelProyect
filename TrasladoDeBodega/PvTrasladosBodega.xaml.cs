using Syncfusion.Windows.Controls.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Syncfusion.UI.Xaml.Grid.Converter;
using Microsoft.Win32;
using System.IO;
using PvTrasladosBodega;
using System.Drawing.Printing;

namespace SiasoftAppExt
{
    //((Inicio)Application.Current.MainWindow).PublicarPnt(9303,"PvTrasladosBodega")
    public partial class PvTrasladosBodega : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        int numregcab = 0;  //idreg a imprimir
        int idLogo = 0;
        DataSet dsImprimir = new DataSet();
        string codbod = "";
        string codpvta = "";
        string nompvta = "";
        string cnEmp = "";
        DataView dtComboBodDestino;
        DataSet ds = new DataSet();
        DataTable dd = new DataTable();
        DataTable dtBod = new DataTable();
        int CantidadMaxRegEnFactura = 0;
        bool SaltoAutomaticoAlSiguienteRegistro = true;
        static string codcco = "";
        static string BusinessName = "";
        static string BusinessNit = "";

        public PvTrasladosBodega()
        {
            InitializeComponent();
            TextFecha.Text = DateTime.Now.ToString();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            codpvta = SiaWin._UserTag;
            LoadInfo();
            ActivaDesactivaControles(0);
            this.DataContext = this;
            FechaIni.Text= DateTime.Now.ToShortDateString();
            FechaFin.Text = DateTime.Now.ToShortDateString();
            DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
            idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
            BusinessName = foundRow["BusinessName"].ToString().Trim();
            BusinessNit = foundRow["BusinessNit"].ToString().Trim();
            BtbGrabar.Focus();
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
                TxtEmpresa.Text = SiaWin._BusinessName.ToString().Trim();
                TxtPVenta.Text = codpvta;
                TxtUser.Text = SiaWin._UserAlias;
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
                    TxtPVenta.Text = codpvta + "-" + nompvta;
                    codbod = SiaWin.Func.cmpCodigo("copventas", "cod_pvt", "cod_bod", codpvta, idemp);
                    if (string.IsNullOrEmpty(codbod))
                    {
                        //_usercontrol.Opacity = 0.5;
                        MessageBox.Show("El punto de venta Asignado no tiene bodega , Pantalla Bloqueada");
                        //usercontrol.IsEnabled=false;
                    }
                    TxtBod.Text = codbod;
                }
                dtBod = SiaWin.Func.SqlDT("select cod_bod,cod_bod+'-'+nom_bod as nom_bod,tipo_bod,aut_ent_trasl from inmae_bod where estado=1  order by cod_bod", "inmae_ref", idemp);
                dtBod.PrimaryKey = new DataColumn[] { dtBod.Columns["cod_bod"] };
                dd = dtBod.Select("cod_bod=" + codbod).CopyToDataTable();
                DataTable d1 = dtBod.Select("cod_bod<>" + codbod).CopyToDataTable();
                dtComboBodDestino = new DataView(d1);
                LlenaCombo(CmbBodOrigen, dd, "cod_bod", "nom_bod");
                CmbBodDestino.ItemsSource = d1.DefaultView; //dtComboBodDestino;
                // establecer paths
                CmbBodDestino.DisplayMemberPath = "nom_bod";
                CmbBodDestino.SelectedValuePath = "cod_bod";
                //LlenaCombo(CmbBodDestino, dtComboBodDestino, "cod_bod", "nom_bod");
                CmbBodOrigen.SelectedValue = codbod;
                dd = null; d1 = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        void LlenaCombo(ComboBox _Combo, DataTable dt, string cmpId, string cmpName)
        {
            _Combo.Items.Clear();
            _Combo.DisplayMemberPath = cmpName;
            _Combo.SelectedValuePath = cmpId;
            _Combo.ItemsSource = dt.DefaultView;
        }
        public void ActivaDesactivaControles(int estado)
        {
            if (estado == 0)
            {
                TextNota.Text = "";
                TextNumeroDoc.Text = "";
                CmbBodDestino.SelectedIndex = -1;
                CmbBodOrigen.SelectedIndex = -1;
                CmbTipoDoc.SelectedIndex = -1;
                TextNota.IsEnabled = false;
                CmbTipoDoc.IsEnabled = false;
                CmbBodOrigen.IsEnabled = false;
                CmbBodDestino.IsEnabled = false;
                BtbGrabar.Content = "Nuevo";
                BtbCancelar.Content = "Salir";
                dataGrid.IsReadOnly = true;
                RefGDCSource.Clear();
                TextItem.Text = "0";
                TextCantidades.Text = "0";
                TextSaldoU.Text = "0";
                LabelBodegaDestino.Text = "Bodega Destino:";
                LabelBodegaOrigen.Text = "Bodega Origen:";
                CmbTipoTraslado.IsEnabled = false;
                CmbTipoTraslado.SelectedIndex = -1;
            }
            if (estado == 1) //creando
            {
                LabelBodegaDestino.Text = "Bodega Destino:";
                LabelBodegaOrigen.Text = "Bodega Origen:";
                TextNota.Text = "Traslado Bodega";
                TextNumeroDoc.Text = "";
                CmbBodDestino.SelectedIndex = -1;
                CmbBodOrigen.SelectedIndex = -1;
                CmbTipoDoc.SelectedIndex = -1;
                CmbTipoDoc.IsEnabled = true;
                CmbBodOrigen.IsEnabled = true;
                CmbBodDestino.IsEnabled = true;
                TextNota.IsEnabled = true;
                BtbGrabar.Content = "Grabar";
                BtbCancelar.Content = "Cancelar";
                dataGrid.IsReadOnly = false;
                RefGDCSource.Add(new Referencia() { nom_ref = "--" });
                dataGrid.CommitEdit();
                dataGrid.UpdateLayout();
                dataGrid.SelectedIndex = 0;
                TextItem.Text = "0";
                TextCantidades.Text = "0";
                TextSaldoU.Text = "0";
                CmbBodOrigen.SelectedValue = codbod;
                CmbTipoTraslado.IsEnabled = true;
                CmbTipoTraslado.SelectedIndex = -1;
            }
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
        private void BtbGrabar_Click(object sender, RoutedEventArgs e)
        {
            if (BtbGrabar.Content.ToString() == "Nuevo")
            {
                ActivaDesactivaControles(1);
                CmbTipoDoc.Focus();
                CmbTipoDoc.IsDropDownOpen = true;
            }
            else
            {
                if (string.IsNullOrEmpty(cnEmp))
                {
                    MessageBox.Show("Error - Cadena de Conexion nulla");
                    return;
                }

                if (MessageBox.Show("Usted desea guardar el documento..?", "Guardar Traslado", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        int _TipoDoc = CmbTipoDoc.SelectedIndex;
                        if (_TipoDoc < 0)
                        {
                            MessageBox.Show("Seleccione un Tipo de Documento..");
                            CmbTipoDoc.Focus();
                            CmbTipoDoc.IsDropDownOpen = true;
                            return;
                        }
                        if (CmbBodOrigen.SelectedIndex < 0)
                        {
                            MessageBox.Show("Seleccione Bodega de Origen..");
                            CmbBodOrigen.Focus();
                            CmbBodOrigen.IsDropDownOpen = true;
                            return;
                        }
                        if (CmbBodDestino.SelectedIndex < 0)
                        {
                            MessageBox.Show("Seleccione Bodega de Origen..");
                            CmbBodDestino.Focus();
                            CmbBodDestino.IsDropDownOpen = true;
                            return;
                        }
                        if (RefGDCSource.Count == 0)
                        {
                            MessageBox.Show("No hay registros de productos...");
                            dataGrid.Focus();
                            return;
                        }
                        int _TipoTrasl = CmbTipoTraslado.SelectedIndex;
                        if (_TipoTrasl < 0)
                        {
                            MessageBox.Show("Seleccione un Tipo de Traslado..");
                            CmbTipoTraslado.Focus();
                            CmbTipoTraslado.IsDropDownOpen = true;
                            return;
                        }
                        var _bodOrigen = CmbBodOrigen.SelectedValue.ToString();
                        var _bodDestino = CmbBodDestino.SelectedValue;
                        if (TotalCnt(0) <= 0) return;
                        if (TotalCnt(1) <= 0) return;
                        int iddocumento = 0;
                        if(CmbTipoDoc.SelectedIndex==0 || CmbTipoDoc.SelectedIndex==1) // valida si es 0 y 1 salidas
                        {
                            if (!ValidaExistencias()) return;
                            iddocumento = ExecuteSqlTransaction(_bodOrigen.ToString(), _bodDestino.ToString());
                        }
                        if(CmbTipoDoc.SelectedIndex==2)// entrada traslado manul
                        {
                            iddocumento = ExecuteSqlTransactionEntrada(_bodOrigen.ToString(), _bodDestino.ToString());
                        }
                                                
                        if (iddocumento < 0) return;
                        ImprimeDocumento(iddocumento);
                        //ImprimirDoc(iddocumento,"Impresion Original");
                        //MessageBox.Show("Documento Guardado:" + iddocumento.ToString());
                        ActivaDesactivaControles(0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    dataGrid.Focus();
                }
            }
        }
        public double TotalCnt(int tipo) //tipo = 0 suma cantidades, tipo=1 cuenta items
        {
            double _cnt = 0;
            foreach (var item in RefGDCSource)
            {
                if (tipo == 0) _cnt += item.cantidad;
                if (tipo == 1 && item.cantidad > 0) _cnt++;
            }
            if (tipo == 0) TextCantidades.Text = _cnt.ToString("N2");
            if (tipo == 1) TextItem.Text = _cnt.ToString("N2");
            return _cnt;
        }
        private bool ValidaExistencias()
        {
            try
            {
                var q = from b in RefGDCSource  group b by b.idrow into g 
                        select new
                        {
                            idrow = g.Key,
                            cod_ref = g.Max(item => item.cod_ref),
                            cantidad = g.Sum(item => item.cantidad)
                        };
                StringBuilder errorMessages = new StringBuilder();
                foreach (var item in q)
                {
                    if (item.cantidad > 0)
                    {
                        double saldoin = SiaWin.Func.SaldoInv(item.cod_ref, codbod, idemp);
                        if (item.cantidad > saldoin) errorMessages.Append("Codigo:" + item.cod_ref.ToString() + " /Cantidad a Facturar:" + item.cantidad.ToString() + " /Saldo Inv:" + saldoin.ToString() + "\n");
                    }
                }
                if (errorMessages.ToString() != string.Empty)
                {
                    MessageBox.Show(errorMessages.ToString());
                    dataGrid.Focus();
                    dataGrid.SelectedIndex = 0;
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return true;
        }
        private void BtbCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (BtbCancelar.Content.ToString() == "Cancelar")
            {
                if (RefGDCSource.Count > 0)
                {
                    if (MessageBox.Show("Usted desea cancelar este documento..?", "Cancelar Traslado", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    {
                        e.Handled = true;
                        return;
                   }
                }
                ActivaDesactivaControles(0);
                BtbGrabar.Focus();
                e.Handled = true;
                return;
            }
            else
            {
                this.Close();
            }
        }
        private bool IsNumberKey(Key inKey)
        {
            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsDelOrBackspaceOrTabKey(Key inKey)
        {
            return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || inKey == Key.Up || inKey == Key.Left || inKey == Key.Right || inKey == Key.Up || inKey == Key.Down || inKey == Key.Home || inKey == Key.End;
        }
        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (dataGrid.IsReadOnly == true) return;
            if (e.Key == System.Windows.Input.Key.F5)
            {
                BtbGrabar.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                return;
            }
            dataGrid.UpdateLayout();
            var data = ((DataGrid)sender).SelectedItem as Referencia;
            if (data == null)
            {
                e.Handled = true;
            }
            var uiElement = e.OriginalSource as UIElement;
            if ((e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Right || e.Key == Key.Tab) && ((DataGrid)sender).CurrentColumn.DisplayIndex == 0)
            {
                if (string.IsNullOrEmpty(data.cod_ref))
                {
                    /////////////
                    dynamic ww = SiaWin.WindowExt(9326, "InBuscarReferencia");  //carga desde sql
                    ww.Conexion = SiaWin.Func.DatosEmp(idemp);
                    ww.idEmp = idemp;
                    ww.idBod = codbod;
                    ww.UltBusqueda = "";
                    ww.ShowInTaskbar = false;
                    ww.Owner = Application.Current.MainWindow;
                    ww.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ww.ShowDialog();
//                    UltBusquedaRef = ww.UltBusqueda;
                    if (!string.IsNullOrEmpty(ww.Codigo))
                    {
                        data.cod_ref = ww.Codigo.ToString();
                    }
                    ww = null;
                    if (string.IsNullOrEmpty(data.cod_ref)) e.Handled = false;
                    data.cantidad = 0; data.val_ref = 0; data.Estado = false;
                    if (!ActualizaCamposRef(data.cod_ref, sender)) e.Handled = false;
                    e.Handled = true;
                }
                else
                {
                    if (!ActualizaCamposRef(data.cod_ref, sender))
                    {
                        MessageBox.Show("Codigo :" + data.cod_ref + " No existe...");
                        data.cantidad = 0; data.val_ref = 0; data.Estado = false;
                        e.Handled = true;
                        return;
                    }
                }
                if (SaltoAutomaticoAlSiguienteRegistro == true)
                {
                    int add = 0;
                    if (CantidadMaxRegEnFactura == 0)
                    {
                        if (dataGrid.SelectedIndex == RefGDCSource.Count - 1)
                        {
                            RefGDCSource.Add(new Referencia() { nom_ref = "--" });
                        }
                        add = 1;
                    }
                    if (CantidadMaxRegEnFactura > 0)
                    {
                        if (RefGDCSource.Count < CantidadMaxRegEnFactura)
                        {
                            RefGDCSource.Add(new Referencia() { nom_ref = "--" });
                            add = 1;
                        }
                    }
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                    dataGrid.CurrentCell = new DataGridCellInfo(dataGrid.Items[dataGrid.SelectedIndex + add], dataGrid.Columns[0]);
                    dataGrid.CommitEdit();
                    dataGrid.SelectedIndex = dataGrid.SelectedIndex + add;
                    e.Handled = true;
                    return;
                }
            }
            if (((DataGrid)sender).CurrentColumn.DisplayIndex != 0)
            {
                e.Handled = !IsNumberKey(e.Key) && !IsDelOrBackspaceOrTabKey(e.Key);
            }
            int column = ((DataGrid)sender).CurrentColumn.DisplayIndex + 1;
            int columntot = ((DataGrid)sender).Columns.Count;
            int fila1 = ((DataGrid)sender).SelectedIndex;
            int fila = ((DataGrid)sender).Items.IndexOf(((DataGrid)sender).SelectedItem);
            if ((e.Key == Key.Enter || e.Key == Key.Return) && uiElement != null && (column < columntot))
            {
                if (((DataGrid)sender).CurrentColumn.DisplayIndex >= 0)
                {
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                }
            }
            if (e.Key == Key.Right && ((DataGrid)sender).CurrentColumn.DisplayIndex == 0 && !string.IsNullOrEmpty(data.cod_ref))
            {
                uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                e.Handled = true;
            }
            if (e.Key == Key.Left && uiElement != null && (column > 1))
            {
                uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                e.Handled = true;
            }
            if ((e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Right || e.Key == Key.Tab) && uiElement != null && (column == columntot))
            {
                dataGrid.CommitEdit();
                dataGrid.UpdateLayout();

                int add = 0;
                if (fila + 1 == RefGDCSource.Count)
                {
                    if (CantidadMaxRegEnFactura == 0)
                    {
                        RefGDCSource.Add(new Referencia() { nom_ref = "--" });
                        add = 1;
                    }
                    if (CantidadMaxRegEnFactura > 0)
                    {
                        if (RefGDCSource.Count < CantidadMaxRegEnFactura)
                        {
                            RefGDCSource.Add(new Referencia() { nom_ref = "--" });
                            add = 1;
                        }
                    }
                }
                if (add > 0) uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                dataGrid.CurrentCell = new DataGridCellInfo(dataGrid.Items[dataGrid.SelectedIndex + add], dataGrid.Columns[0]);
                dataGrid.CommitEdit();
                dataGrid.UpdateLayout();
                dataGrid.SelectedIndex = dataGrid.SelectedIndex + add;
                e.Handled = true;
            }
            if (e.Key == Key.Down && dataGrid.CurrentColumn.DisplayIndex == 0 && !string.IsNullOrEmpty(data.cod_ref))
            {
                //CurrentCell.RowIndex.ToString
                //        maingrid.SelectedIndex
                Int32 columnIndex = dataGrid.SelectedIndex;
                Int32 countref = RefGDCSource.Count;
                //        if(columnIndex==countref-1) 
                if (fila == countref - 1)
                {
                    if (CantidadMaxRegEnFactura == 0) RefGDCSource.Add(new Referencia() { nom_ref = "--" });
                    if (CantidadMaxRegEnFactura > 0)
                    {
                        if (RefGDCSource.Count < CantidadMaxRegEnFactura) RefGDCSource.Add(new Referencia() { nom_ref = "--" });
                        dataGrid.CommitEdit();
                        dataGrid.UpdateLayout();
                    }
                }
            }
            if (e.Key == Key.Up && dataGrid.CurrentColumn.DisplayIndex == 0 && string.IsNullOrEmpty(data.cod_ref))
            {
                var selectedItem = dataGrid.SelectedItem as Referencia;
                if (selectedItem != null)
                {
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                    dataGrid.SelectedIndex = dataGrid.SelectedIndex - 1;
                    dataGrid.CommitEdit();
                    RefGDCSource.Remove(selectedItem);
                    dataGrid.CommitEdit();
                    dataGrid.UpdateLayout();
                    e.Handled = true;
                }
            }
            if (e.Key == Key.F8)
            {
                uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
            }
            if (e.Key == Key.F3)  //eliminar registro
            {
                if (((DataGrid)sender).SelectedIndex == 0 && RefGDCSource.Count == 1) return;
                if (MessageBox.Show("Borrar Registro actual?", "Siasoft", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var selectedItem = dataGrid.SelectedItem as Referencia;
                    if (selectedItem != null)
                    {
                        int fila1x = ((DataGrid)sender).SelectedIndex;
                        Int32 countrefx = RefGDCSource.Count;
                        if (((DataGrid)sender).SelectedIndex == 0 && RefGDCSource.Count > 1)
                        {
                            uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        }
                        else
                        {
                            if (((DataGrid)sender).SelectedIndex > 0 && RefGDCSource.Count > 1) uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                            if (((DataGrid)sender).SelectedIndex == RefGDCSource.Count - 1) uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                        }
                        RefGDCSource.Remove(selectedItem);
                    }
                    e.Handled = true;
                }
            }
            TotalCnt(0);
            TotalCnt(1);
        }
        private bool ActualizaCamposRef(string Id, object datagrid)
        {
            bool Resp = false;
            try
            {
                if (string.IsNullOrEmpty(Id)) return false;
                SqlDataReader dr = SiaWin.Func.SqlDR("select inmae_ref.idrow,cod_ref,rtrim(nom_ref) as nom_ref,val_ref,inmae_ref.cod_tiva,inmae_tiva.por_iva,nom_tip,nom_prv,inmae_tip.por_des as tippor_des,inmae_tip.por_desc as tippor_desc FROM inmae_ref inner join inmae_tiva on inmae_tiva.cod_tiva=inmae_ref.cod_tiva inner join inmae_tip on inmae_tip.cod_tip=inmae_ref.cod_tip left join inmae_prv on inmae_prv.cod_prv=inmae_ref.cod_prv where  inmae_ref.cod_ref='" + Id.ToString() + "'", idemp);
                while (dr.Read())
                {
                    ((Referencia)((DataGrid)datagrid).SelectedItem).idrow = Convert.ToInt32(dr["idrow"]);
                    ((Referencia)((DataGrid)datagrid).SelectedItem).cod_ref = dr["cod_ref"].ToString().Trim();
                    ((Referencia)((DataGrid)datagrid).SelectedItem).nom_ref = dr["nom_ref"].ToString().Trim();
                    if (((Referencia)((DataGrid)datagrid).SelectedItem).cantidad == 0 && ((Referencia)((DataGrid)datagrid).SelectedItem).Estado == false)
                    {
                        ((Referencia)((DataGrid)datagrid).SelectedItem).cantidad = 1;
                        ((Referencia)((DataGrid)datagrid).SelectedItem).val_ref = Convert.ToDouble(dr["val_ref"]);
                        ((Referencia)((DataGrid)datagrid).SelectedItem).cod_tiva = dr["cod_tiva"].ToString().Trim();
                        ((Referencia)((DataGrid)datagrid).SelectedItem).poriva = Convert.ToDouble(dr["por_iva"]);
                        ((Referencia)((DataGrid)datagrid).SelectedItem).Estado = true;
                        ((Referencia)((DataGrid)datagrid).SelectedItem).nom_tip = dr["nom_tip"].ToString().Trim();
                        ((Referencia)((DataGrid)datagrid).SelectedItem).nom_prv = dr["nom_prv"].ToString().Trim();
                    }
                    int filaindex = ((DataGrid)datagrid).SelectedIndex;
                    TotalCnt(0);
                    TotalCnt(1);
                    if (((Referencia)((DataGrid)datagrid).SelectedItem).cod_ref != string.Empty)
                    {
                        double saldoin = SiaWin.Func.SaldoInv(((Referencia)((DataGrid)datagrid).SelectedItem).cod_ref, codbod, idemp);
                        TextSaldoU.Text = saldoin.ToString("N2");
                    }
                    else
                    {
                        TextSaldoU.Text = "0";
                    }
                    Resp = true;
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
            return Resp;
        }
        private void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.DisplayIndex == 0)
            {
                //MessageBox.Show(e.Column.DisplayIndex.ToString());
                if (((Referencia)((DataGrid)sender).SelectedItem).cantidad > 0 || ((Referencia)((DataGrid)sender).SelectedItem).Estado == true)
                {
                    ((Referencia)((DataGrid)sender).SelectedItem).cantidad = 0;
                    ((Referencia)((DataGrid)sender).SelectedItem).Estado = false;
                    //MessageBox.Show("finedit");
                }
            }
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem == null) return;
            var _RefDG = dataGrid.SelectedItem as Referencia;
            if (_RefDG != null)
            {
                string reg = _RefDG.cod_ref;
                if (!string.IsNullOrEmpty(reg))
                {
                    double saldoin = SiaWin.Func.SaldoInv(_RefDG.cod_ref, codbod, 1);
                    TextSaldoU.Text = saldoin.ToString("N2");
                }
            }
            else
            {
                TextSaldoU.Text = "0";
            }
        }
        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox s = e.Source as TextBox;
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                }
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (RefGDCSource.Count > 0) e.Cancel = true;
        }
        private void CmbTipoDoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selec = CmbTipoDoc.SelectedIndex;
            int tipodoc = -1;
            if (selec == -1) TextNumeroDoc.Text = "";
            if (selec == 0) tipodoc = 7;
            if (selec == 1) tipodoc = 8;
            if (selec == 2) BtnCargarEntradas.Visibility = Visibility.Visible;
            if (selec != 2) BtnCargarEntradas.Visibility = Visibility.Hidden;
            CmbBodOrigen.SelectedIndex = -1;
            CmbBodDestino.SelectedIndex = -1;
            TextNumeroDoc.Text = SiaWin.Func.ConsecutivoPv(codpvta, 0, tipodoc, idemp);
        }
        private void CmbTipoDoc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ComboBox cs = e.Source as ComboBox;
                if (cs != null)
                {
                  if(cs.SelectedIndex>=0) cs.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                base.OnPreviewKeyDown(e);
            }
        }

        private int ExecuteSqlTransaction(string bodori,string boddes)
        {
            DataRow foundBod = dtBod.Rows.Find(boddes);
            bool indtrasladoauto = Convert.ToBoolean(foundBod["aut_ent_trasl"].ToString().Trim());
            if (string.IsNullOrEmpty(cnEmp))
            {
                MessageBox.Show("Error - Cadena de Conexion nulla");
                return -1;
            }
            string TipoConsecutivo = "";
            string codtrn = "141";
            string codtrncontra = "051";
            if (CmbTipoDoc.SelectedIndex == 1) codtrn = "145";
            if (CmbTipoDoc.SelectedIndex == 1) codtrncontra = "052";
            if (codtrn == "141") TipoConsecutivo = "sal_trasl";
            if (codtrn == "145") TipoConsecutivo = "sal_consg";
            using (SqlConnection connection = new SqlConnection(cnEmp))
            {
                connection.Open();
                StringBuilder errorMessages = new StringBuilder();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("Transaction");
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    string sqlcabContra = "";
                    
                    string sqlConsecutivo = @"declare @fecdoc as datetime;set @fecdoc = getdate();declare @ini as char(4);declare @num as varchar(12);declare @iConsecutivo char(12) = '' ;declare @iFolioHost int = 0;UPDATE COpventas SET " + TipoConsecutivo + " = ISNULL(" + TipoConsecutivo + ", 0) + 1  WHERE cod_pvt='"+ codpvta+"';SELECT @iFolioHost = " + TipoConsecutivo + ",@ini=rtrim(inicial) FROM Copventas  WHERE cod_pvt='"+ codpvta+"';set @num=@iFolioHost;select @iConsecutivo=rtrim(@ini)+REPLICATE ('0',12-len(rtrim(@ini))-len(rtrim(convert(varchar,@num))))+rtrim(convert(varchar,@num));";
                    string sqlcab = sqlConsecutivo + @"INSERT INTO incab_doc (cod_trn,fec_trn,num_trn,doc_ref,des_mov,bod_tra,tip_traslado,est_imp) values ('" + codtrn + "',@fecdoc,@iConsecutivo,@iConsecutivo,'" +TextNota.Text.Trim()+ "','" +boddes+ "',"+CmbTipoTraslado.SelectedIndex.ToString()+",1);DECLARE @NewID INT;SELECT @NewID = SCOPE_IDENTITY();";
                    if (indtrasladoauto == true)
                    {
                        sqlcabContra = @"INSERT INTO incab_doc (cod_trn,fec_trn,num_trn,doc_ref,des_mov,bod_tra,tip_traslado) values ('" + codtrncontra + "',@fecdoc,@iConsecutivo,@iConsecutivo,'" + TextNota.Text.Trim() + "','" + bodori + "',"+CmbTipoTraslado.SelectedIndex.ToString()+");DECLARE @NewIDContra INT;SELECT @NewIDContra = SCOPE_IDENTITY();";
                    }
                    string sql = "";
                    string sqlcontra = "";

                    var q = from b in RefGDCSource
                            group b by b.cod_ref into g
                            select new
                            {
                                cod_ref = g.Key,
                                cantidad = g.Sum(item => item.cantidad)
                            };


                        //                        foreach (var item in RefGDCSource)
                    foreach (var item in q)
                        {
                        if (item.cantidad > 0)
                        {
                            sql = sql + @"INSERT INTO incue_doc (idregcab,cod_trn,num_trn,cod_ref,cod_bod,cantidad) values (@NewID,'" + codtrn + "',@iConsecutivo,'" + item.cod_ref.ToString() + "','" + codbod.ToString() + "'," + item.cantidad.ToString("F", CultureInfo.InvariantCulture)+");";
                            if (indtrasladoauto == true)
                            {
                                sqlcontra = sqlcontra + @"INSERT INTO incue_doc (idregcab,cod_trn,num_trn,cod_ref,cod_bod,cantidad) values (@NewIDContra,'" + codtrncontra + "',@iConsecutivo,'" + item.cod_ref.ToString() + "','" + boddes.ToString() + "'," + item.cantidad.ToString("F", CultureInfo.InvariantCulture) + ");";
                            }
                        }
                    }
                    command.CommandText = sqlcab + sql +sqlcabContra+sqlcontra+  @"select CAST(@NewId AS int);";
                    MessageBox.Show(command.CommandText.ToString());
                    var r = new object();
                    r = command.ExecuteScalar();
                    transaction.Commit();
                    connection.Close();
                    return Convert.ToInt32(r.ToString());
                }
                catch (SqlException ex)
                {
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append(" SQL-Index #" + i + "\n" + "Message: " + ex.Errors[i].Message + "\n" + "LineNumber: " + ex.Errors[i].LineNumber + "\n" + "Source: " + ex.Errors[i].Source + "\n" + "Procedure: " + ex.Errors[i].Procedure + "\n");
                    }
                    transaction.Rollback();
                    MessageBox.Show(errorMessages.ToString());
                    return -1;
                }
                catch (Exception ex)
                {
                    errorMessages.Append("c Error:#" + ex.Message.ToString());
                    transaction.Rollback();
                    MessageBox.Show(errorMessages.ToString());
                    return -1;
                }
            }
        }
        private int ExecuteSqlTransactionEntrada(string bodori, string boddes)
        {
            if (string.IsNullOrEmpty(cnEmp))
            {
                MessageBox.Show("Error - Cadena de Conexion nulla");
                return -1;
            }
            string codtrn = "051";
            using (SqlConnection connection = new SqlConnection(cnEmp))
            {
                connection.Open();
                StringBuilder errorMessages = new StringBuilder();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("Transaction");
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    string sqlConsecutivo = @"declare @fecdoc as datetime;set @fecdoc = getdate();";
                    string sqlcab = sqlConsecutivo + @"INSERT INTO incab_doc (cod_trn,fec_trn,num_trn,doc_ref,des_mov,bod_tra,tip_traslado) values ('" + codtrn + "',@fecdoc,'"+TextNumeroDoc.Text.Trim()+"','"+TextNumeroDoc.Text.Trim()+"','" + TextNota.Text.Trim() + "','" + CmbBodDestino.SelectedValue + "',"+CmbTipoTraslado.SelectedIndex.ToString()+");DECLARE @NewID INT;SELECT @NewID = SCOPE_IDENTITY();";
                    string sql = "";
                    var q = from b in RefGDCSource
                            group b by b.cod_ref into g
                            select new
                            {
                                cod_ref = g.Key,
                                cantidad = g.Sum(item => item.cantidad)
                            };
                    //                        foreach (var item in RefGDCSource)
                    foreach (var item in q)
                    {
                        if (item.cantidad > 0)
                        {
                            sql = sql + @"INSERT INTO incue_doc (idregcab,cod_trn,num_trn,cod_ref,cod_bod,cantidad) values (@NewID,'" + codtrn + "','"+TextNumeroDoc.Text.Trim()+"','" + item.cod_ref.ToString() + "','" + codbod.ToString() + "'," + item.cantidad.ToString("F", CultureInfo.InvariantCulture) + ");";

                        }
                    }
                    command.CommandText = sqlcab + sql + @"select CAST(@NewId AS int);";
                    //MessageBox.Show(command.CommandText.ToString());
                    var r = new object();
                    r = command.ExecuteScalar();
                    transaction.Commit();
                    connection.Close();
                    return Convert.ToInt32(r.ToString());
                }
                catch (SqlException ex)
                {
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append(" SQL-Index #" + i + "\n" + "Message: " + ex.Errors[i].Message + "\n" + "LineNumber: " + ex.Errors[i].LineNumber + "\n" + "Source: " + ex.Errors[i].Source + "\n" + "Procedure: " + ex.Errors[i].Procedure + "\n");
                    }
                    transaction.Rollback();
                    MessageBox.Show(errorMessages.ToString());
                    return -1;
                }
                catch (Exception ex)
                {
                    errorMessages.Append("c Error:#" + ex.Message.ToString());
                    transaction.Rollback();
                    MessageBox.Show(errorMessages.ToString());
                    return -1;
                }
            }
        }

        ////// CONSULTA DE TRASLADOS
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(cnEmp))
            {
                connection.Open();
                //connectionString.Open();
                //DataSet ds = new DataSet();
                StringBuilder _sql = new StringBuilder();
                ds.Clear();
                _sql.Append("select cab.cod_trn,cab.num_trn,cab.fec_trn,cab.bod_tra,cab.bod_tra+'-'+bod.ini_bod as bod_dest,cue.cod_bod,cue.cod_ref,rtrim(ref.nom_ref) as nom_ref,rtrim(tip.nom_tip) as nom_tip,iif(trn.tip_trn=1,cue.cantidad,-cue.cantidad) as cantidad,trn.tip_trn,iif(cab.tip_traslado=0,'Tienda',iif(cab.tip_traslado=1,'GerenteProducto',iif(cab.tip_traslado=2,'GerenteAdmon','Ninguno'))) as tipotraslado,cab.idreg from incue_doc as cue ");
                _sql.Append(" inner join incab_doc as cab on cab.idreg = cue.idregcab inner join inmae_ref as ref on ref.cod_ref = cue.cod_ref inner join inmae_bod as bod on bod.cod_bod = cab.bod_tra ");
                _sql.Append(" inner join inmae_trn as trn on trn.cod_trn=cab.cod_trn inner join inmae_tip as tip on tip.cod_tip =ref.cod_tip where convert(date,cab.fec_trn) between '" + FechaIni.Text + "' and '" + FechaFin.Text + "' and (cab.cod_trn = '051' or cab.cod_trn = '141')");
                _sql.Append(" and cue.cod_bod = '" + codbod.Trim() + "' order by cab.fec_trn ");
                SqlDataAdapter adapter = new SqlDataAdapter(_sql.ToString(), connection);
                adapter.Fill(ds, "Traslados");
                dataGridSF.ItemsSource = ds.Tables["Traslados"];
                TextTotalEntradas.Text=ds.Tables["Traslados"].Compute("Sum(cantidad)", "tip_trn=1").ToString();
                TextTotalSalidas.Text = ds.Tables["Traslados"].Compute("Sum(cantidad)", "tip_trn=2").ToString();
            }
        }

        private void Ejecutar_Click(object sender, RoutedEventArgs e)
        {
            // validar fecha
            LoadData();
        }

        private void ReImprimir_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = ds.Tables["Traslados"].Rows[dataGridSF.SelectedIndex];
            if(dr!=null)
            {
                string numtrn = dr["idreg"].ToString();
                ImprimeDocumento(Convert.ToInt32(numtrn));
                //ImprimirDoc(Convert.ToInt32(numtrn),"Reimpreso");
            }
        }

        private void ExportaXLS_Click(object sender, RoutedEventArgs e)
        {
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = dataGridSF.ExportToExcel(dataGridSF.View, options);
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

        private void CmbBodDestino_GotFocus(object sender, RoutedEventArgs e)
        {
                if (CmbTipoDoc.SelectedIndex < 0)
                {
                    CmbTipoDoc.Dispatcher.BeginInvoke((Action)(() => { CmbTipoDoc.Focus(); }));
                    return;
                }
                else
                {
                //          dtComboGru.RowFilter = "idrowtip="+DRef.idrowtip.ToString();
                    if (CmbTipoDoc.SelectedIndex == 0) //bodegas empresa
                    {
                        dtComboBodDestino.RowFilter = "tipo_bod=0";
                    }
                if (CmbTipoDoc.SelectedIndex == 1) //bodegas empresa
                {
                    dtComboBodDestino.RowFilter = "tipo_bod=1";
                }

            }

        }
        private void ImprimirDoc(int idregcab,string tipoImp)
        {
            string[] strArrayParam = new string[] { idregcab.ToString(), idemp.ToString(), tipoImp };
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

        private void BtnCargarEntradas_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(cnEmp))
            {
                connection.Open();
                DataSet dsEntradas = new DataSet();
                string sql = @"select cab.idreg,cab.num_trn + '(' + rtrim(convert(char, convert(int, sum(cantidad)))) + ')' + 'BodOrigen:' + incue_doc.cod_bod as numero from incab_doc as cab inner join incue_doc on incue_doc.idregcab = cab.idreg";
                sql += @" where estado = 0 and cab.cod_trn = '141' and bod_tra = '" + codbod + "' and not exists(select * from incab_doc where cod_trn = '051' and num_trn = cab.num_trn) group by cab.idreg,cab.num_trn,incue_doc.cod_bod order by numero ";
                SqlDataAdapter adapter = new SqlDataAdapter(sql.ToString(), connection);
                adapter.Fill(dsEntradas, "EntradasTraslados");
                if (dsEntradas.Tables["EntradasTraslados"].Rows.Count == 0)
                {
                    MessageBox.Show("No hay documentos pendientes en traslados de entrada");
                }
                else
                {
                    EntradasLista WinEntradas = new EntradasLista(codbod);
                    WinEntradas.CmbListaEntrada.ItemsSource = dsEntradas.Tables["EntradasTraslados"].DefaultView;
                    // establecer paths
                    WinEntradas.CmbListaEntrada.DisplayMemberPath = "numero";
                    WinEntradas.CmbListaEntrada.SelectedValuePath = "idreg";
                    WinEntradas.Owner = this;
                    WinEntradas.ShowDialog();
                    int idregcabeza = WinEntradas.idregcab;
                    WinEntradas = null;
                    if (idregcabeza > 0)
                    {
                        RefGDCSource.Clear();
                        DataTable __cab = new DataTable();
                        DataTable __cue = new DataTable();
                        __cab = SiaWin.Func.SqlDT("select * from incab_doc where idreg="+ idregcabeza.ToString(), "cab", idemp);
                        __cue = SiaWin.Func.SqlDT("select cue.cod_ref,ref.nom_ref,cod_bod,sum(cantidad) as cantidad from incue_doc as cue inner join inmae_ref as ref on ref.cod_ref=cue.cod_ref where idregcab=" + idregcabeza.ToString()+" group by cue.cod_ref,ref.nom_ref,cod_bod order by cue.cod_ref,cod_bod", "cab", idemp);
                        if(__cab.Rows.Count>0 && __cue.Rows.Count>0)
                        {
                            string numtrn = __cab.Rows[0]["num_trn"].ToString();
                            string boddes = __cab.Rows[0]["bod_tra"].ToString();
                            string bodorig = __cue.Rows[0]["cod_bod"].ToString();
                            int tiptraslado = Convert.ToInt32(__cab.Rows[0]["tip_traslado"].ToString());
                            TextNumeroDoc.Text = numtrn;
                            TextNota.Text = "Entrada de Bod:" + bodorig;
                            CmbBodOrigen.SelectedValue = boddes;
                            CmbBodDestino.SelectedValue = bodorig;
                            CmbTipoTraslado.SelectedIndex = tiptraslado;
                            foreach (DataRow itemcue in __cue.Rows)
                            {

                                //MessageBox.Show(itemcue["cod_ref"].ToString()+"-"+ itemcue["cod_bod"].ToString()+"-"+ itemcue["cantidad"].ToString());
                                if (Convert.ToDouble(itemcue["cantidad"].ToString().Trim()) > 0)
                                {
                                    RefGDCSource.Add(new Referencia()
                                    {
                                        cod_ref = itemcue["cod_ref"].ToString().Trim(),
                                        nom_ref = itemcue["nom_ref"].ToString().Trim(),
                                        cantidad = Convert.ToDouble(itemcue["cantidad"].ToString().Trim())
                                    }
                                        );
                                }
                            }
                            //desactiva controles
                            dataGrid.IsReadOnly = true;
                            CmbTipoDoc.IsEnabled = false;
                            CmbBodDestino.IsEnabled = false;
                            CmbBodOrigen.IsEnabled = false;
                            CmbTipoTraslado.IsEnabled = false;
                            string labelname = LabelBodegaDestino.Text;
                            LabelBodegaDestino.Text = LabelBodegaOrigen.Text;
                            LabelBodegaOrigen.Text = labelname;
                            BtnCargarEntradas.Visibility = Visibility.Hidden;
                        }

                   }
                }
                

            }

            //            ww.ShowInTaskbar = false;
            //          ww.Owner = Application.Current.MainWindow;
            //        ww.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //      var objBlur = new System.Windows.Media.Effects.BlurEffect();
            //    objBlur.Radius = 3;
            //  ((Inicio)Application.Current.MainWindow).Effect = objBlur;
            //ww.ShowDialog();
            //ww = null;
        }
        private void ImprimeDocumento(int iddoc)
        {
            // **** IMPRESION DE ENTRADA Y SALIDA DE TRASLADO
            numregcab = iddoc;
            //MessageBox.Show(ConfigCSource.numregcab.ToString());
            SqlConnection con = new SqlConnection(cnEmp);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            //DataSet dsImprimir = new DataSet();
            cmd = new SqlCommand("PvTraslados", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_NumRegCab", numregcab);//if you have parameters.
            da = new SqlDataAdapter(cmd);
            dsImprimir.Clear();
            da.Fill(dsImprimir);
            int nItems = dsImprimir.Tables[1].Rows.Count;
            int nItemsFpago = dsImprimir.Tables[1].Rows.Count;

            PrintDocument pd = new PrintDocument();

            System.Drawing.Printing.PaperSize ps = new PaperSize("", 290, 600 + (nItems * 20) + (nItemsFpago * 20));
            pd.PrintPage += new PrintPageEventHandler(pd_imprimefactura);

            pd.PrintController = new StandardPrintController();
            pd.DefaultPageSettings.Margins.Left = 0;
            pd.DefaultPageSettings.Margins.Right = 0;
            pd.DefaultPageSettings.Margins.Top = 0;
            pd.DefaultPageSettings.Margins.Bottom = 0;
            pd.DefaultPageSettings.PaperSize = ps;
            pd.Print();
            ExecuteSqlTransactionCabReeimprime(numregcab);
        }

        //********** IMPRIME FACTURAS

        private void ExecuteSqlTransactionCabReeimprime(int idcab)
        {
            using (SqlConnection connection = new SqlConnection(cnEmp))
            {
                connection.Open();
                StringBuilder errorMessages = new StringBuilder();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("Transaction");
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    string sqlcab = @"update incab_doc set est_imp=est_imp+1 where idreg=" + idcab.ToString();
                    command.CommandText = sqlcab;
                    command.ExecuteScalar();
                    transaction.Commit();
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append(" SQL-Index #" + i + "\n" + "Message: " + ex.Errors[i].Message + "\n" + "LineNumber: " + ex.Errors[i].LineNumber + "\n" + "Source: " + ex.Errors[i].Source + "\n" + "Procedure: " + ex.Errors[i].Procedure + "\n");
                    }
                    transaction.Rollback();
                    MessageBox.Show(errorMessages.ToString());

                }
                catch (Exception ex)
                {
                    errorMessages.Append("c Error:#" + ex.Message.ToString());
                    transaction.Rollback();
                    MessageBox.Show(errorMessages.ToString());
                }

            }
        }


        private void pd_imprimefactura(object sender, PrintPageEventArgs e)
        {
            try
            {
                string rowValue1 = "";
                int pos1 = 0;

                System.Drawing.Graphics g = e.Graphics;

                System.Drawing.Font fBody = new System.Drawing.Font("Lucida Console", 7, System.Drawing.FontStyle.Bold);
                System.Drawing.Font fBody1 = new System.Drawing.Font("Lucida Console", 7, System.Drawing.FontStyle.Bold);
                System.Drawing.Font fTitulo1 = new System.Drawing.Font("Lucida Console", 12, System.Drawing.FontStyle.Bold);

                System.Drawing.SolidBrush sb = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                /// alinear valores derecha-izquierda
                System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                drawFormat.Alignment = System.Drawing.StringAlignment.Far;
                drawFormat.LineAlignment = System.Drawing.StringAlignment.Near;

                /// alinear al centro
                System.Drawing.StringFormat drawFormatCenter = new System.Drawing.StringFormat();
                drawFormatCenter.Alignment = System.Drawing.StringAlignment.Center;
                drawFormatCenter.LineAlignment = System.Drawing.StringAlignment.Near;


                string pathlogo = SiaWin._PathApp + @"\imagenes\" + idLogo.ToString() + "..png";
                e.Graphics.DrawImage(System.Drawing.Image.FromFile(pathlogo), 100, 1, 75, 75);

                String s = BusinessName.Trim();
                //      s += "Nit:"+BusinessNit.Trim();
                System.Drawing.Font f = new System.Drawing.Font("Arial", 12);
                System.Drawing.StringFormat sf = new System.Drawing.StringFormat();

                sf.Alignment = System.Drawing.StringAlignment.Center;        // horizontal alignment
                sf.LineAlignment = System.Drawing.StringAlignment.Near;    // vertical alignment
                pos1 = 15;
                System.Drawing.Rectangle r = new System.Drawing.Rectangle(10, 75, 270, f.Height * 1);
                g.DrawRectangle(System.Drawing.Pens.Black, r);
                g.DrawString(s, f, System.Drawing.Brushes.Black, r, sf);


                //     g.DrawString(BusinessName.Trim(),  fTitulo1,sb,100,pos1);
                //     pos1=25;
                //    int ancho=4+BusinessNit.Trim().Length;   
                //    g.DrawString("Nit:"+BusinessNit.Trim(), fTitulo1,sb,(300-ancho)/2,pos1);

                int _Reimpresion = Convert.ToInt32(dsImprimir.Tables[0].Rows[0]["est_imp"].ToString());
                string _TipoDoc = dsImprimir.Tables[0].Rows[0]["cod_trn"].ToString();
                string _BodTra = dsImprimir.Tables[0].Rows[0]["bod_tra"].ToString();
                string _NumDocAnula = dsImprimir.Tables[0].Rows[0]["des_mov"].ToString();
                string _TituloDoc = "SALIDA TRASLADO:";
                if (_TipoDoc == "051") _TituloDoc = "ENTRADA TRASLADO:";
                //if (_TipoDoc == "008") _TituloDoc = "DEVOLUCION ITEM:";

                pos1 = 85;
                pos1 = pos1 + 10;
                g.DrawString("                 Nit:" + BusinessNit, fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;
                g.DrawString("          Agente de retencion de IVA", fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;
                g.DrawString("              GRAN CONTRIBUYENTE", fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;
                g.DrawString("               RES 0076 DE 2016", fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;

                g.DrawString("----------------------------------------------", fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;
                g.DrawString(_TituloDoc, fBody1, sb, 1, pos1);
                rowValue1 = dsImprimir.Tables[0].Rows[0]["num_trn"].ToString();
                g.DrawString(rowValue1, fBody1, sb, 100, pos1);
                pos1 = pos1 + 10;
                g.DrawString("FECHA          :", fBody1, sb, 1, pos1);
                rowValue1 = dsImprimir.Tables[0].Rows[0]["fec_trn"].ToString();
                g.DrawString(rowValue1, fBody1, sb, 100, pos1);
                pos1 = pos1 + 10;
                g.DrawString("TIENDA ORIGEN   :", fBody1, sb, 1, pos1);
                g.DrawString(nompvta.Trim(), fBody1, sb, 100, pos1);
                pos1 = pos1 + 10;
                if (_TipoDoc == "141")
                {
                    g.DrawString("TIENDA DESTINO :", fBody1, sb, 1, pos1);
                    g.DrawString(_BodTra + "/" + CmbBodDestino.Text, fBody1, sb, 100, pos1);
                    pos1 = pos1 + 10;
                }
                g.DrawString("----------------------------------------------", fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;
                g.DrawString("REFERENCIA", fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;
                g.DrawString("CANT    DESCRIPCION       V/UNIT        TOTAL", fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;
                g.DrawString("----------------------------------------------", fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;

                int itemCount = 0;
                foreach (DataRow row in dsImprimir.Tables[1].Rows)
                {
                    itemCount = itemCount + 1;

                    rowValue1 = row["cantidad"].ToString() + " -" + row["cod_ref"].ToString();
                    g.DrawString(rowValue1, fBody1, sb, 1, pos1);
                    pos1 = pos1 + 10;
                    //         rowValue1 =row["cantidad"].ToString()+" "+row["nom_ref"].ToString()+" "+row["val_uni"].ToString()+" "+row["total_"].ToString();
                    rowValue1 = row["nom_ref"].ToString();
                    g.DrawString(rowValue1, fBody1, sb, 1, pos1);
                    if (dsImprimir.Tables[1].Rows.Count > 1)
                    {
                        if (itemCount < dsImprimir.Tables[1].Rows.Count)
                        {
                            pos1 = pos1 + 10;
                            g.DrawString("- - - - - - - - - - - - - - - - - - - - - - ", fBody1, sb, 1, pos1);
                        }
                    }

                    pos1 = pos1 + 10;
                }

                g.DrawString("----------------------------------------------", fBody1, sb, 1, pos1);
                pos1 = pos1 + 10;
                g.DrawString("NUMERO DE ARTICULOS TRASLADADOS ", fBody1, sb, 1, pos1);
                rowValue1 = dsImprimir.Tables[2].Rows[0]["gcantidad"].ToString();
                g.DrawString(rowValue1, fBody1, sb, 211, pos1);
                pos1 = pos1 + 10;
                g.DrawString("----------------------------------------------", fBody1, sb, 1, pos1);
                g.DrawString("", fBody1, sb, 5, pos1 + 20);
                pos1 = pos1 + 10;
                g.DrawString("*", fBody1, sb, 5, pos1 + 10);
                if (_Reimpresion > 1) g.DrawString("*** REIMPRESA *** ", fTitulo1, sb, 55, pos1 + 10);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Imprime Factura:" + ex.ToString());
            }
        }
    }
}
