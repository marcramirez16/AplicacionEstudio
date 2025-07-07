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
    /// Lógica de interacción para SoloCombobox3.xaml
    /// </summary>
    public partial class SoloCombobox3 : Window
    {
        private Dictionary<string, List<string>> _temasPorAsignatura = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> _archivosPorTemas = new Dictionary<string, List<string>>();

        public SoloCombobox3()
        {
            InitializeComponent();
            this.LocationChanged += CustomMessageDialog_LocationChanged;
        }

        private bool _isLocationSet = false;
        private double _left, _top;

        private void CustomMessageDialog_LocationChanged(object sender, EventArgs e)
        {
            if (!_isLocationSet)
            {
                _left = this.Left;
                _top = this.Top;
                _isLocationSet = true;
            }
            else
            {
                this.Left = _left;
                this.Top = _top;
            }
        }

        public string AsignaturaName => AsignaturaComboBox.SelectedItem?.ToString() ?? "";
        public string TemaName => TemaComboBox.SelectedItem?.ToString() ?? "";
        public string ArchivoName => ArchivoComboBox.SelectedItem?.ToString() ?? "";


        public void SetAsignaturas(List<string> asignaturas)
        {
            AsignaturaComboBox.ItemsSource = asignaturas;
        }

        public void SetTemas(string asignatura, List<string> temas)
        {
            TemaComboBox.ItemsSource = temas;
            TemaComboBox.Visibility = Visibility.Visible;
            TemaComboBox.SelectedIndex = -1;
            AceptarButton.IsEnabled = false;
        }

        public void SetArchivos(string asignatura, List<string> temas)
        {
            ArchivoComboBox.ItemsSource = temas;
            ArchivoComboBox.Visibility = Visibility.Visible;
            ArchivoComboBox.SelectedIndex = -1;
            AceptarButton.IsEnabled = false;
        }

        private void AsignaturaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AsignaturaComboBox.SelectedItem is string asignaturaSeleccionada)
            {
                // Notificar al exterior que debe cargar los temas
                OnAsignaturaSeleccionada?.Invoke(asignaturaSeleccionada);
            }
        }

        public Action<string> OnAsignaturaSeleccionada { get; set; }

        public Action<string, string> OnTemaSeleccionado { get; set; }


        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AsignaturaName) &&
                !string.IsNullOrWhiteSpace(TemaName) &&
                !string.IsNullOrWhiteSpace(ArchivoName))
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ComprobarCampos()
        {
            AceptarButton.IsEnabled =
                !string.IsNullOrWhiteSpace(AsignaturaName) &&
                !string.IsNullOrWhiteSpace(TemaName) &&
                !string.IsNullOrWhiteSpace(ArchivoName);
            ;
        }

        private void TemaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComprobarCampos();

            if (AsignaturaComboBox.SelectedItem is string asignatura &&
                TemaComboBox.SelectedItem is string tema)
            {
                OnTemaSeleccionado?.Invoke(asignatura, tema);
            }
        }

        private void ArchivoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComprobarCampos();
        }

    }
}
