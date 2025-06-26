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
    /// Lógica de interacción para InicioSession.xaml
    /// </summary>
    public partial class InicioSession :  MahApps.Metro.Controls.MetroWindow
    {
        public InicioSession()
        {
            InitializeComponent();

        }

        private void Ingresar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = UsuarioTextBox.Text;
            string contrasena = ContrasenaBox.Password;

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
            }
        }

        private void UsuarioTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Registro_Click(object sender, RoutedEventArgs e)
        {
            Registro registro = new Registro();
            registro.Owner = this;
            registro.ShowDialog();
  
        }
    }
}
