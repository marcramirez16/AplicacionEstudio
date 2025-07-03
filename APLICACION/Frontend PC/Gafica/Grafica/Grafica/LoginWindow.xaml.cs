using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Grafica
{
    /// <summary>
    /// Lógica de interacción para LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Loaded += LoginWindow_Loaded; //efecto de movimiento al iniciar
            BuscarSession();
        }

        /// <summary>
        /// Metodo para saver si ya hay un usuario logeado en el backend
        /// </summary>
        /// <returns></returns>
        private async Task BuscarSession()
        {
            try
            {
                Boolean iniciado = await ControllerApiOut.UsuarioIniciado();
                if (iniciado)
                {
                    EUsuario nulo = null;
                    MainWindow main = new MainWindow(nulo);
                    main.WindowState = WindowState.Maximized;
                    main.Show();
                    this.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las asignaturas: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodos para iniciar sesion usuario "presionar boton iniciar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Ingresar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = UsuarioTextBox.Text;
            string contraseña = ContrasenaBox.Password;

            //Crear usuario
            EUsuario usuarioe = new EUsuario
            {
                usuario = usuario,
                contraseña = contraseña,
                email = ""
            };

            ControllerApiOut controller = new ControllerApiOut();
            try
            {
                EUsuario respuesta = await ControllerApiOut.IniciarSesion(usuarioe);


                if (!string.IsNullOrEmpty(respuesta.usuario) &&
                    !string.IsNullOrEmpty(respuesta.contraseña) &&
                    !string.IsNullOrEmpty(respuesta.email))
                {
                    ErrorText.Text = respuesta.usuario;
                    ErrorText.Visibility = Visibility.Visible;

                    MainWindow main = new MainWindow(respuesta); // pasar el usuario como parametro
                    main.WindowState = WindowState.Maximized;
                    main.Show();
                    this.Close();
                }
                else
                {
                    ErrorText.Text = "Usuario o contraseña incorrecta";
                    ErrorText.Visibility = Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                ErrorText.Text = "Error, No se pudo guardar el usuario";
                ErrorText.Visibility = Visibility.Visible;

            }
        }



        /// <summary>
        /// Abrir el formulario de registro...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Registro_Click(object sender, RoutedEventArgs e)
        {
            // Cambiar fondo a transparente
            LoginBorder.Background = new SolidColorBrush(Colors.Transparent);

            // Abrir ventana de registro
            RegisterWindow registro = new RegisterWindow();
            registro.Owner = this;
            registro.ShowDialog();

            // Restaurar color original tras cerrar
            LoginBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#28323C"));

        }


        /// <summary>
        /// Efecto de movimiento al iniciar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var sb = (Storyboard)FindResource("SlideInFromRightStoryboard");
            sb.Begin();
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
