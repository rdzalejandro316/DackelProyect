using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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

namespace Galeria
{

    public partial class agregarImagen : Window
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";

        public string nombre_doc = "";
        public string codigo_doc = "";

        //imagen
        string strName = "", imageName;

        public agregarImagen()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        
        WebCam webcam;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Subir documento para : " + nombre_doc.Trim();
            webcam = new WebCam();
            webcam.InitializeWebCam(ref imgVideo);
        }

        //************************* card 1 ***********************************************************************

        //abrir imagen
        bool imageSave = false;

        private void BTNimage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif;*.png)|*.jpg;*.bmp;*.gif;*.png";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName;
                    imageName = fldlg.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    image1.SetValue(System.Windows.Controls.Image.SourceProperty, isc.ConvertFromString(imageName));
                    imageSave = true;
                    BTNsubirFoto.IsEnabled = true;
                }
                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //guardar imagen        
        private void BTNsubirFoto_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                byte[] imgByteArr = null;
                if (imageSave == true)
                {
                    FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                    imgByteArr = new byte[fs.Length];
                    fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                }

                using (SqlConnection connection = new SqlConnection(SiaWin.Func.DatosEmp(idemp)))
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "insert into documento_ima(cod_ima,cod_doc,image_name,img_cli) values (@cod_ima,@cod_doc,@image_name,@img_cli)";
                    cmd.Parameters.AddWithValue("@cod_ima", "1");
                    cmd.Parameters.AddWithValue("@cod_doc", codigo_doc);
                    cmd.Parameters.AddWithValue("@image_name", strName);
                    cmd.Parameters.AddWithValue("@img_cli", imgByteArr);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("imagen guardada en el documento : " + nombre_doc);

            }
            catch (Exception w)
            {
                MessageBox.Show("error al cargar la imagen: " + w);
            }

        }


        // card 2 ****************************************************************************************************

        bool imageSaveSql = false;

        private void bntStart_Click(object sender, RoutedEventArgs e)
        {
            webcam.Start();
        }

        private void bntCapture_Click(object sender, RoutedEventArgs e)
        {
            webcam.Stop();
            //imgCapture.Source = imgVideo.Source;

            imageSaveSql = true;

            bntSaveLocal.IsEnabled = true;
            bntSaveSQL.IsEnabled = true;
        }

        private void bntSaveLocal_Click(object sender, RoutedEventArgs e)
        {
            Helper.SaveImageCapture((BitmapSource)imgVideo.Source);
        }

        private void bntSaveSQL_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                byte[] imgByteArr = null;
                if (imageSaveSql == true)
                {
                    imgByteArr = ConvertBitmapSourceToByteArray((BitmapSource)imgVideo.Source);
                }

                using (SqlConnection connection = new SqlConnection(SiaWin.Func.DatosEmp(idemp)))
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "insert into documento_ima(cod_ima,cod_doc,image_name,img_cli) values (@cod_ima,@cod_doc,@image_name,@img_cli)";
                    cmd.Parameters.AddWithValue("@cod_ima", "2");
                    cmd.Parameters.AddWithValue("@cod_doc", codigo_doc);
                    cmd.Parameters.AddWithValue("@image_name", "prueba");
                    cmd.Parameters.AddWithValue("@img_cli", imgByteArr);



                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("imagen guardada en el documento : " + nombre_doc);

            }
            catch (Exception w)
            {
                MessageBox.Show("error al cargar la imagen: " + w);
            }


        }


        public static byte[] ConvertBitmapSourceToByteArray(ImageSource imageSource)
        {
            var image = imageSource as BitmapSource;
            byte[] data;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public byte[] BufferFromImage(BitmapImage imageSource)
        {
            if (imageSource == null) return null;

            int height = imageSource.PixelHeight;
            int width = imageSource.PixelWidth;
            int stride = width * ((imageSource.Format.BitsPerPixel + 7) / 8);

            byte[] bits = new byte[height * stride];
            imageSource.CopyPixels(bits, stride, 0);

            return bits;
        }

        public byte[] BufferFromImage_2(BitmapImage imageSource)
        {
            Stream stream = imageSource.StreamSource;
            byte[] buffer = null;
            if (stream != null && stream.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    buffer = br.ReadBytes((Int32)stream.Length);
                }
            }
            return buffer;
        }

        //public static BitmapImage ConvertPictureObjectToBitmap(object obj)
        //{
        //    if (obj == null || obj is System.DBNull) return null;

        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream((byte[])obj))
        //        {
        //            BitmapImage image = null;

        //            ms.Position = 0;
        //            image = new BitmapImage();
        //            image.BeginInit();
        //            image.StreamSource = ms;
        //            image.EndInit();
        //            return image;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("err:" + ex);
        //    }

        //    return null;
        //}
        private void bntResolution_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("sss");
            webcam.ResolutionSetting();
        }

        private void bntSetting_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("mnn");
            webcam.AdvanceSetting();
        }




    }
}
