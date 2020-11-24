using System;
using System.Collections.Generic;
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

namespace SeguimientoCliente
{
    
    public partial class Buscar : Window
    {
                     
        public Buscar()
        {
            InitializeComponent();

            pantalla();
        }

        public void pantalla() {
            this.MinHeight = 400;
            this.MaxHeight = 400;
            this.MinWidth = 500;
            this.MaxHeight = 500;
        }

        public event RoutedEventHandler BuscarTodoEventHandler;
        public event RoutedEventHandler CancelarEventHandler;
        public event RoutedEventHandler SigEventHandler;
        public event RoutedEventHandler AntEventHandler;


        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (BuscarTodoEventHandler != null)
            {
                BuscarTodoEventHandler(sender, e);
            }        
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e) {

            if (CancelarEventHandler != null)
            {
                CancelarEventHandler(sender, e);
            }
        }

        private void Sig_Click(object sender, RoutedEventArgs e)
        {
            if (SigEventHandler != null)
            {
                SigEventHandler(sender, e);
            }
        }

        private void Ant_Click(object sender, RoutedEventArgs e)
        {
            if (AntEventHandler != null)
            {
                AntEventHandler(sender, e);
            }
        }





        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }



    }    

}
