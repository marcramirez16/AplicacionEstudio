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
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Grafica1
{
    public partial class CustomMessageDialog : BaseMetroDialog
    {
        private TaskCompletionSource<bool> tcs;

        public enum DialogMode
        {
            Asignatura,
            Tema
        }

        private DialogMode mode;

        // Para modo Tema: lista de asignaturas para el ComboBox
        private List<string> assignaturas;

        public CustomMessageDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Mostrar diálogo según modo, y opcionalmente lista asignaturas para modo Tema
        /// </summary>
        public Task<bool> ShowAsync(MetroWindow window, DialogMode dialogMode, List<string> assignaturas = null)
        {
            mode = dialogMode;
            this.assignaturas = assignaturas;

            SetupUIForMode();

            tcs = new TaskCompletionSource<bool>();
            window.ShowMetroDialogAsync(this);
            return tcs.Task;
        }

        private void SetupUIForMode()
        {
            switch (mode)
            {
                case DialogMode.Asignatura:
                    // Mostrar TextBox de asignatura
                    LabelAsignaturaTextBox.Visibility = Visibility.Visible;
                    AssignaturaTextBox.Visibility = Visibility.Visible;

                    // Ocultar controles de tema
                    LabelAsignaturaComboBox.Visibility = Visibility.Collapsed;
                    AssignaturaComboBox.Visibility = Visibility.Collapsed;
                    LabelTemaTextBox.Visibility = Visibility.Collapsed;
                    TemaTextBox.Visibility = Visibility.Collapsed;
                    break;

                case DialogMode.Tema:
                    // Ocultar TextBox de asignatura simple
                    LabelAsignaturaTextBox.Visibility = Visibility.Collapsed;
                    AssignaturaTextBox.Visibility = Visibility.Collapsed;

                    // Mostrar ComboBox asignaturas y TextBox tema
                    LabelAsignaturaComboBox.Visibility = Visibility.Visible;
                    AssignaturaComboBox.Visibility = Visibility.Visible;
                    LabelTemaTextBox.Visibility = Visibility.Visible;
                    TemaTextBox.Visibility = Visibility.Visible;

                    // Cargar lista asignaturas al ComboBox
                    AssignaturaComboBox.ItemsSource = assignaturas ?? new List<string>();
                    if (assignaturas?.Count > 0)
                        AssignaturaComboBox.SelectedIndex = 0;

                    break;
            }
        }

        public string AssignaturaName
        {
            get
            {
                if (mode == DialogMode.Asignatura)
                    return AssignaturaTextBox.Text.Trim();
                else if (mode == DialogMode.Tema)
                    return AssignaturaComboBox.SelectedItem?.ToString() ?? "";
                else
                    return "";
            }
        }

        public string TemaName
        {
            get
            {
                if (mode == DialogMode.Tema)
                    return TemaTextBox.Text.Trim();
                else
                    return "";
            }
        }

        private async void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            if (mode == DialogMode.Asignatura)
            {
                if (string.IsNullOrWhiteSpace(AssignaturaName))
                {
                    MessageBox.Show("Por favor, introduce un nombre válido para la asignatura.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else if (mode == DialogMode.Tema)
            {
                if (string.IsNullOrWhiteSpace(AssignaturaName))
                {
                    MessageBox.Show("Por favor, selecciona una asignatura.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(TemaName))
                {
                    MessageBox.Show("Por favor, introduce un nombre válido para el tema.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            await CloseDialog(true);
        }

        private async void OnCancelClick(object sender, RoutedEventArgs e)
        {
            await CloseDialog(false);
        }

        private async Task CloseDialog(bool accepted)
        {
            var window = Window.GetWindow(this) as MetroWindow;
            if (window != null)
            {
                await window.HideMetroDialogAsync(this);
            }
            tcs.SetResult(accepted);
        }
    }
}
