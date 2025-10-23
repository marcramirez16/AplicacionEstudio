using Grafica.VentanasSecundarias.Elementos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static Grafica.VentanasSecundarias.PasosOperacion;
using static Xceed.Wpf.Toolkit.Calculator;


namespace Grafica.VentanasSecundarias
{
    public partial class PasosOperacion : Window
    {
        public ObservableCollection<Paso> Pasos { get; set; } = new ObservableCollection<Paso>();

        public PasosOperacion()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        // Agregar un nuevo paso
        private void AgregarPaso_Click(object sender, RoutedEventArgs e)
        {
            int numero = Pasos.Count + 1;
            Pasos.Add(new Paso { Numero = numero, NombrePaso = "", Operaciones = new ObservableCollection<Operacion>() });
        }

        // Agregar operación dentro de un paso
        private void AgregarOperacion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Paso paso)
            {
                paso.Operaciones.Add(new Operacion { Valor = "" });
            }
        }

        // Eliminar operación individual
        private void EliminarOperacion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Paso paso && btn.DataContext is Operacion operacion)
            {
                paso.Operaciones.Remove(operacion);
            }
        }

        // Eliminar paso completo y recalcular números
        private void EliminarPaso_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Paso paso)
            {
                Pasos.Remove(paso);

                // Recalcular números de pasos
                for (int i = 0; i < Pasos.Count; i++)
                {
                    Pasos[i].Numero = i + 1;
                }
            }
        }

        // Abrir calculadora al hacer click en operación
        private void OperacionTextBox_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (sender is TextBox tb)
            {
                Calculadora calc = new Calculadora();
                if (calc.ShowDialog() == true)
                {
                    tb.Text = calc.Resultado;
                }
            }
        }

        // Clase Paso
        public class Paso : INotifyPropertyChanged
        {
            private int numero;
            public int Numero { get => numero; set { numero = value; OnPropertyChanged(); } }

            private string nombrePaso;
            public string NombrePaso { get => nombrePaso; set { nombrePaso = value; OnPropertyChanged(); } }

            public ObservableCollection<Operacion> Operaciones { get; set; } = new ObservableCollection<Operacion>();

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        // Clase Operacion
        public class Operacion : INotifyPropertyChanged
        {
            private string valor;
            public string Valor { get => valor; set { valor = value; OnPropertyChanged(); } }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}