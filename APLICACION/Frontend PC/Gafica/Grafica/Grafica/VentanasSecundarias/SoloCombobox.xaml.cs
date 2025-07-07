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
    /// Lógica de interacción para SoloCombobox.xaml
    /// </summary>
    public partial class SoloCombobox : Window
    {
        public SoloCombobox()
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

        public string AssignaturaName => AssignaturaComboBox.SelectedItem?.ToString() ?? "";

        /// <summary>
        /// Este método se llama desde fuera para inicializar el combo con asignaturas
        /// </summary>
        public void SetDialogMode(List<string> asignaturas)
        {
            AssignaturaComboBox.ItemsSource = asignaturas;
            AssignaturaComboBox.SelectedIndex = 0;
        }

        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AssignaturaName))
            {
                return;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

