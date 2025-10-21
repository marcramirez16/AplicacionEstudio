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

namespace Grafica.VentanasSecundarias
{
    /// <summary>
    /// Lógica de interacción para PasosOperacion.xaml
    /// </summary>
    public partial class PasosOperacion : Window
    {
        public ObservableCollection<Paso> Pasos { get; set; } = new ObservableCollection<Paso>();

        public PasosOperacion()
        {
            InitializeComponent();
            this.DataContext = this;

        }
        private void AgregarPaso_Click(object sender, RoutedEventArgs e)
        {
            int numero = Pasos.Count + 1;
            Pasos.Add(new Paso { Numero = numero, NombrePaso = "", Operacion = "" });
        }

        public class Paso : INotifyPropertyChanged
        {
            private int numero;
            public int Numero { get => numero; set { numero = value; OnPropertyChanged(); } }

            private string nombrePaso;
            public string NombrePaso { get => nombrePaso; set { nombrePaso = value; OnPropertyChanged(); } }

            private string operacion;
            public string Operacion { get => operacion; set { operacion = value; OnPropertyChanged(); } }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
