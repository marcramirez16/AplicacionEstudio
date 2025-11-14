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

namespace Grafica.VentanasSecundarias
{
    /// <summary>
    /// Lógica de interacción para PasoNormal.xaml
    /// </summary>
    public partial class PasoNormal : Window
    {
        long id_resumen;
        public PasoNormal(string id_resumen)
        {
            InitializeComponent();

            this.id_resumen =  long.Parse(id_resumen);
            agregartexto();
        }

        private async void GuardarTexto_Click(object sender, RoutedEventArgs e)
        {
            await ControllerApiOut.GuardarPasoNormal(this.id_resumen, TextoIngresado.Text);
        }

        private async void agregartexto() { 
            string texto = await ControllerApiOut.ObtenerPasoNormal(this.id_resumen);
            MessageBox.Show(texto);
            TextoIngresado.Text = await ControllerApiOut.ObtenerPasoNormal(this.id_resumen);
        }

 
    }
}
