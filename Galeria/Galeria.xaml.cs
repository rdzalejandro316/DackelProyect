using Galeria;
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

    public partial class Galeria : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";


        public Galeria(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;


            LoadConfig();
            cargarDocuemto();
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
                tabitem.Title = "Galeria (" + aliasemp + ")";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                agregarImagen ventana = new agregarImagen();

                DataRowView row = (DataRowView)dataGridDocumentos.SelectedItems[0];

                ventana.nombre_doc = row["nom_doc"].ToString();
                ventana.codigo_doc = row["cod_doc"].ToString();

                ventana.ShowDialog();
                cargarDocuemto();
            }
            catch (Exception)
            {
                MessageBox.Show("erro 1");
            }

        }


        public void cargarDocuemto()
        {
            try
            {
                string cadena = "select cod_doc,nom_doc from mae_documento";
                DataTable dt = new DataTable();
                dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridDocumentos.ItemsSource = dt.DefaultView;
            }
            catch (Exception)
            {

                MessageBox.Show("error 2");
            }

        }


        private void FirstDetailsViewGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGridDocumentos.SelectedItems[0];
                string codigoDocumento = row["cod_doc"].ToString();

                string cadena = "select image_name, img_cli from documento_ima where cod_doc='" + codigoDocumento + "' ";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                dataGridDocImage.ItemsSource = dt.DefaultView;
            }
            catch (Exception w)
            {

                MessageBox.Show("error 3:" + w);
            }


        }


        private void FirstDetailsViewGrid_MostrarImage(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {

            //try
            //{
            //    DataRowView row = (DataRowView)dataGridDocumentos.SelectedItems[0];
            //    string codigoDocumento = row["cod_doc"].ToString();

            //    string cadena = "select cod_doc, image_name, img_cli from documento_ima where cod_doc = '" + codigoDocumento + "' ";
            //    SqlDataReader drTra = SiaWin.Func.SqlDR(cadena, idemp);

            //    while (drTra.Read())
            //    {
            //        _documentoLB = drTra["tdoc"].ToString().Trim();

            //        byte[] blob = (byte[])drTra["img_cli"];
            //        MemoryStream stream = new MemoryStream();
            //        stream.Write(blob, 0, blob.Length);
            //        stream.Position = 0;
            //        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            //        BitmapImage bi = new BitmapImage();
            //        bi.BeginInit();
            //        MemoryStream ms = new MemoryStream();
            //        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            //        ms.Seek(0, SeekOrigin.Begin);
            //        bi.StreamSource = ms;
            //        bi.EndInit();
            //        image1.Source = bi;
            //    }
            //}
            //catch (Exception w) 
            //{
            //    MessageBox.Show("ojo con eso" +w);
            //}
            try
            {
                DataRowView row = (DataRowView)dataGridDocImage.SelectedItems[0];
                image1.Visibility = Visibility.Visible;
                byte[] blob = (byte[])row["img_cli"];
                MemoryStream stream = new MemoryStream();
                stream.Write(blob, 0, blob.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();
                image1.Source = bi;

            }
            catch (Exception w)
            {
                MessageBox.Show("error imagen:" + w);
                image1.Visibility = Visibility.Hidden;
            }

        }








    }
}
