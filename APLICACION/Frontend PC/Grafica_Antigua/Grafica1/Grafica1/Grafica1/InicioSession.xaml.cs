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
            BuscarSession();

        }

        /// <summary>
        /// Metodo para saver si ya hay un usuario logeado
        /// </summary>
        /// <returns></returns>
        private async Task BuscarSession()
        {
            try{
                Boolean iniciado = await ControllerApiOut.UsuarioIniciado();
                if (iniciado){
                    EUsuario nulo = null;
                    MainWindow main = new MainWindow(nulo);
                    main.WindowState = WindowState.Maximized;
                    main.Show();
                    this.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
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
