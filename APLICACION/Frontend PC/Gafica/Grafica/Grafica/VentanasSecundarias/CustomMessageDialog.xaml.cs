using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
    public partial class CustomMessageDialog : Window
    {
        public enum DialogMode
        {
            Asignatura,
            Tema
        }

        private DialogMode mode;

        public CustomMessageDialog()
        {
            InitializeComponent();
            SetupUIForMode();

            // Escucha el evento para bloquear movimiento
            this.LocationChanged += CustomMessageDialog_LocationChanged;
        }
        private bool _isLocationSet = false; // para fijar la posición solo la primera vez
        private double _left, _top;

        private void CustomMessageDialog_LocationChanged(object sender, EventArgs e)
        {
            if (!_isLocationSet)
            {
                // Guarda la posición inicial
                _left = this.Left;
                _top = this.Top;
                _isLocationSet = true;
            }
            else
            {
                // Bloquea la ventana en la posición inicial
                this.Left = _left;
                this.Top = _top;
            }
        }
        public string AssignaturaName
        {
            get
            {
                return mode == DialogMode.Asignatura
                    ? AssignaturaTextBox.Text.Trim()
                    : AssignaturaComboBox.SelectedItem?.ToString() ?? "";
            }
        }

        public string TemaName
        {
            get
            {
                return mode == DialogMode.Tema ? TemaTextBox.Text.Trim() : "";
            }
        }

        public void SetDialogMode(DialogMode dialogMode, List<string> asignaturas = null)
        {
            mode = dialogMode;

            if (mode == DialogMode.Tema && asignaturas != null)
            {
                AssignaturaComboBox.ItemsSource = asignaturas;
                AssignaturaComboBox.SelectedIndex = 0;
            }

            SetupUIForMode();
        }

        private void SetupUIForMode()
        {
            if (mode == DialogMode.Asignatura)
            {
                LabelAsignaturaTextBox.Visibility = Visibility.Visible;
                AssignaturaTextBox.Visibility = Visibility.Visible;

                LabelAsignaturaComboBox.Visibility = Visibility.Collapsed;
                AssignaturaComboBox.Visibility = Visibility.Collapsed;
                LabelTemaTextBox.Visibility = Visibility.Collapsed;
                TemaTextBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                LabelAsignaturaTextBox.Visibility = Visibility.Collapsed;
                AssignaturaTextBox.Visibility = Visibility.Collapsed;

                LabelAsignaturaComboBox.Visibility = Visibility.Visible;
                AssignaturaComboBox.Visibility = Visibility.Visible;
                LabelTemaTextBox.Visibility = Visibility.Visible;
                TemaTextBox.Visibility = Visibility.Visible;
            }
        }

        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            if (mode == DialogMode.Asignatura && string.IsNullOrWhiteSpace(AssignaturaName))
            {
                MessageBox.Show("Introduce un nombre válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (mode == DialogMode.Tema && (string.IsNullOrWhiteSpace(AssignaturaName) || string.IsNullOrWhiteSpace(TemaName)))
            {
                MessageBox.Show("Completa todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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

