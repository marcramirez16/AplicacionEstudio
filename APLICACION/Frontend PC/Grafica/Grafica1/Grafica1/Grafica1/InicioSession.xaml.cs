using Grafica1.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Grafica1
{
    /// <summary>
    /// Lógica de interacción para InicioSession.xaml
    /// </summary>
    public partial class InicioSession :  MahApps.Metro.Controls.MetroWindow
    {
        public InicioSession()
        {
            InitializeComponent();

        }

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
                else {
                    ErrorText.Text = "Usuario o contraseña incorrecta";
                    ErrorText.Visibility = Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                ErrorText.Text = "Error, No se pudo guardar el usuario";
                ErrorText.Visibility = Visibility.Visible;

            }

            /*
            // Simulación de validación
            if (usuario == "admin" && contrasena == "1234")
            {
                // Opcional: abrir ventana principal
                MainWindow main = new MainWindow();
                main.Show();
                this.Close(); // Cierra la ventana de login
            }
            else
            {
                ErrorText.Text = "Usuario o contraseña incorrectos";
                ErrorText.Visibility = Visibility.Visible;
            }*/
        }


        /// <summary>
        /// Abrir el formulario de registro...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Registro_Click(object sender, RoutedEventArgs e)
        {
            Registro registro = new Registro();
            registro.Owner = this;
            registro.ShowDialog();
  
        }
    }
}
