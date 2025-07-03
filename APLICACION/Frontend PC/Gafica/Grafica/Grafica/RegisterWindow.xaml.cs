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

namespace Grafica
{
    /// <summary>
    /// Lógica de interacción para RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Meotod para Registrar el usuario en la bd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Registrar_Click(object sender, RoutedEventArgs e)
        {
            String usuario = NombreUsuarioTextBox.Text;
            String correo = CorreoTextBox.Text;
            String contraseña = ContrasenaBox.Password;

            //Crear usuario
            EUsuario usuarioe = new EUsuario
            {
                usuario = usuario,
                contraseña = contraseña,
                email = correo
            };

            ControllerApiOut controller = new ControllerApiOut();
            try
            {
                string respuesta = await ControllerApiOut.EnviarUsuario(usuarioe);
                MensajeText.Text = respuesta;
                MensajeText.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                MensajeText.Text = "Error, No se pudo guardar el usuario";
                MensajeText.Visibility = Visibility.Visible;

            }

        }

        //------------------------------------------
        //METODOS PARA EL PANEL SUPERIOR 
        //_--------------------------------------------

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Menu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private const double dragStartOffset = 350; // pixel minimo "a partir de aqui se puede arrastrar 'derecha'"


        //Detectar si hay algun subitem del menu abierto...
        private bool IsAnySubmenuOpen(ItemsControl menu)
        {
            foreach (var item in menu.Items)
            {
                if (item is MenuItem mi)
                {
                    if (mi.IsSubmenuOpen)
                        return true;

                    // Revisa subitems recursivamente
                    if (IsAnySubmenuOpen(mi))
                        return true;
                }
            }
            return false;
        }


        private T VisualUpwardSearch<T>(DependencyObject source) where T : DependencyObject
        {
            while (source != null && !(source is T))
            {
                source = VisualTreeHelper.GetParent(source);
                // Detener la búsqueda si llegamos a un Menu (pero no confundirlo con el tipo)
                if (source is System.Windows.Controls.Primitives.MenuBase)
                    break;
            }
            return source as T;
        }

        private void rctHeader_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }
        private void rctHeader_PreviewMouseMove(object sender, MouseEventArgs e) { }


    }
}
