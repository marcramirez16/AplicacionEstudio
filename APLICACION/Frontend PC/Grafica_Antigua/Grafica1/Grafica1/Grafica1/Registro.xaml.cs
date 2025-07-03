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

namespace Grafica1
{
    /// <summary>
    /// Lógica de interacción para Registro.xaml
    /// </summary>
    public partial class Registro : MahApps.Metro.Controls.MetroWindow
    {
        public Registro()
        {
            InitializeComponent();
        }

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
            try {
                string respuesta = await ControllerApiOut.EnviarUsuario(usuarioe);
                MensajeText.Text = respuesta;
                MensajeText.Visibility = Visibility.Visible;
                
            } catch (Exception ex)
            {
                MensajeText.Text = "Error, No se pudo guardar el usuario";
                MensajeText.Visibility = Visibility.Visible;

            }

        }
    }
}
