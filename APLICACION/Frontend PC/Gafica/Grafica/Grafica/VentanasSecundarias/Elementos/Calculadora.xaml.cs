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
using NCalc;

namespace Grafica.VentanasSecundarias.Elementos
{
    /// <summary>
    /// Lógica de interacción para Calculadora.xaml
    /// </summary>
    public partial class Calculadora : Window
    {
        private string lastResult = "";

        public string Resultado { get; private set; } = "";

        public Calculadora()
        {
            InitializeComponent();
        }

        // Click en cualquier botón para construir la expresión
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string value = btn.Content.ToString();

                // Reemplazos especiales
                switch (value)
                {
                    case "π": value = "π"; break;      // solo texto
                    case "e": value = "e"; break;      // solo texto
                    case "√": value = "√("; break;     // solo texto
                    case "Ans": value = "Ans"; break;   // solo texto
                    case "!": value = "!"; break;       // solo texto
                }

                txtDisplay.Text += value;
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay.Text = "";
        }

        private void BtnBackspace_Click(object sender, RoutedEventArgs e)
        {
            if (txtDisplay.Text.Length > 0)
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
        }

        // El botón "=" solo agrega el signo "=" al display, no evalúa
        private void BtnEqual_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay.Text += "=";
        }

        // Aceptar devuelve la expresión completa escrita
        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Resultado = txtDisplay.Text;
            DialogResult = true;
        }
    
}
}