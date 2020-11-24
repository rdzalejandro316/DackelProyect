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
using System.Windows.Shapes;


namespace GeneracionDeCodigosRef
{

    public partial class GrupoTallas : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";

        List<string> valores = new List<string>();

        List<string> valoresModi;

        public GrupoTallas()
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
                DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                valores.Add(CBX_Talla.Text);
                TXB_grupo_t.Text = String.Join(",", valores);

                desbloquearControloesInsert();
            }
            catch (Exception w)
            {
                MessageBox.Show("error1:" + w);
            }

        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                valores.RemoveAt(valores.Count - 1);
                TXB_grupo_t.Text = String.Join(",", valores);
            }
            catch (Exception)
            {
                MessageBox.Show("el grupo esta vacio");
            }

        }

        private void Insertar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cadena = "insert into grupo_tallas(grupo_talla) values ('" + TXB_grupo_t.Text + "')";
                SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                MessageBox.Show("Grupo de Tallas insertado Exitosamente");

                bloquearControloesInsert();
                TXB_grupo_t.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("Error al insertar grupo");
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cargarMaestraTallas();
            cargarGrupoTallas();
        }

        public void desbloquearControloesInsert()
        {
            BtnDel.IsEnabled = true;
            BtnInsert.IsEnabled = true;
        }

        public void bloquearControloesInsert()
        {
            BtnDel.IsEnabled = false;
            BtnInsert.IsEnabled = false;
        }

        public void cargarMaestraTallas()
        {
            try
            {
                string cadena = "select desc_tall from inmae_tall";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "MaestraTalla", idemp);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var newItem = new ComboBoxItem();
                    //newItem.Tag = dt.Rows[i]["cod_tall"].ToString().Trim();
                    newItem.Content = dt.Rows[i]["desc_tall"].ToString().Trim();
                    CBX_Talla.Items.Add(newItem);

                    var newItem2 = new ComboBoxItem();
                    newItem2.Content = dt.Rows[i]["desc_tall"].ToString().Trim();
                    CBX_Talla2.Items.Add(newItem2);
                }
            }
            catch (Exception w)
            {
                MessageBox.Show("errro cargar Tallas" + w);
            }

        }

        public void cargarGrupoTallas()
        {
            try
            {
                string cadena = "select * from grupo_tallas";
                DataTable dt = SiaWin.Func.SqlDT(cadena, "gruposTalla", idemp);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var newItem = new ComboBoxItem();
                    newItem.Tag = dt.Rows[i]["idrow"].ToString();
                    newItem.Content = dt.Rows[i]["grupo_talla"].ToString();
                    CB_grupo.Items.Add(newItem);
                }


            }
            catch (Exception w)
            {
                MessageBox.Show("errro cargar Grupo Tallas" + w);
            }


        }

        // ********* modificar  tab **************


        private void CB_Active_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                string tag = ((ComboBox)sender).Tag.ToString();

                if (tag == "1")
                {
                    BtnAdd.IsEnabled = true;
                }

                if (tag == "2")
                {
                    BTNAddMod.IsEnabled = true;
                }
            }
            catch (Exception w)
            {

                MessageBox.Show(w.ToString());
            }

        }


        private void AddMod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                valoresModi.Add(CBX_Talla2.Text);
                TXB_grup_edit.Text = String.Join(",", valoresModi);
            }
            catch (Exception w)
            {
                MessageBox.Show("error1:" + w);
            }

        }

        private void DelMod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                valoresModi.RemoveAt(valoresModi.Count - 1);
                TXB_grup_edit.Text = String.Join(",", valoresModi);
            }
            catch (Exception)
            {
                MessageBox.Show("el grupo esta vacio");
            }

        }

        private void CB_grupo_DropDownOpened(object sender, EventArgs e)
        {
            CB_grupo.Items.Clear();
            cargarGrupoTallas();
        }

        private void CB_grupo_DropDownClosed(object sender, EventArgs e)
        {
            string csv = TXB_grup_edit.Text;
            string[] parts = csv.Split(',');
            valoresModi = new List<string>(parts);

            desbloquearControles();

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var idrow = ((ComboBoxItem)CB_grupo.SelectedItem).Tag.ToString();

                string cadena = "update grupo_tallas set grupo_talla='" + TXB_grup_edit.Text + "' where idrow='" + idrow + "' ";
                SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                MessageBox.Show("actualizacion exitosa");

                CB_grupo.Items.Clear();
                cargarGrupoTallas();

                CBX_Talla2.SelectedIndex = -1;
                BtnActu.IsEnabled = false;
                bloquearControles();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al insertar grupo");
            }
        }

        public void bloquearControles()
        {
            BTNAddMod.IsEnabled = false;
            BTNDelMod.IsEnabled = false;
            BtnActu.IsEnabled = false;
        }

        public void desbloquearControles()
        {
            BTNDelMod.IsEnabled = true;
            BtnActu.IsEnabled = true;
        }



    }
}
