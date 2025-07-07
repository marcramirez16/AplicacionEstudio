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
    /// Lógica de interacción para CustomMessageDialogComboBox2.xaml
    /// </summary>

    public partial class CustomMessageDialogComboBox2 : Window
    {
        private Dictionary<string, List<string>> _temasPorAsignatura = new Dictionary<string, List<string>>();
        public CustomMessageDialogComboBox2()
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
        public string NombreArchivo => ArchivoTextBox.Text.Trim();

        public void SetAsignaturas(List<string> asignaturas)
        {
            AsignaturaComboBox.ItemsSource = asignaturas;
        }

        public void SetTemas(string asignatura, List<string> temas)
        {
            TemaComboBox.ItemsSource = temas;
            TemaComboBox.Visibility = Visibility.Visible;
            TemaComboBox.SelectedIndex = -1;
            ArchivoTextBox.IsEnabled = false;
            ArchivoTextBox.Text = "";
            ArchivoLabel.Visibility = Visibility.Collapsed;
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

        private void TemaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemaComboBox.SelectedItem != null)
            {
                ArchivoLabel.Visibility = Visibility.Visible;
                ArchivoTextBox.IsEnabled = true;
                ComprobarCampos();
            }
            else
            {
                ArchivoLabel.Visibility = Visibility.Collapsed;
                ArchivoTextBox.IsEnabled = false;
                AceptarButton.IsEnabled = false;
            }
        }

        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AsignaturaName) &&
                !string.IsNullOrWhiteSpace(TemaName) &&
                !string.IsNullOrWhiteSpace(NombreArchivo))
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
                !string.IsNullOrWhiteSpace(NombreArchivo);
        }

        private void ArchivoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComprobarCampos();
        }


    }

}
