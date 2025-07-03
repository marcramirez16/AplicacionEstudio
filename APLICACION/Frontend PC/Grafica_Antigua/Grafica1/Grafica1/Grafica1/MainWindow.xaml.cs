using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Grafica1
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private double miniPanelOriginalWidth;

        public MainWindow(EUsuario usuario)
        {

            InitializeComponent();

            miniPanelOriginalWidth = MiniPanelBox.Width;

            ListaGrandealiniciar();

            //Inicializar todos los componentes con el usuario adecuado
            CargarAsignaturasAsync();


        }
        //METODOS PARA CARGAR INFORMACINO API
        /// <summary>
        /// Metodo para cargar las asignaturas des de la api al combobox
        /// </summary>
        /// <returns></returns>
        private async Task CargarAsignaturasAsync()
        {
            try
            {
                List<string> asignaturas = await ControllerApiOut.ObtenerListaAsignaturas();

                asignaturas.Insert(0, "Assignatura");

                AssignaturasComboBox.ItemsSource = asignaturas;
                AssignaturasComboBox.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo para cargar los temas des de la api al combobox
        /// </summary>
        /// <returns></returns>
        private async Task CargarTemasAsync(String asignatura)
        {
            try
            {
                List<string> temas = await ControllerApiOut.ObtenerListaTemas(asignatura);

                temas.Insert(0, "Tema");

                TemasComboBox.ItemsSource = temas;
                TemasComboBox.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo para cargar los archvios
        /// </summary>
        /// <param name="asignatura"></param>
        /// <returns></returns>
        private async Task CargarArchivosAsync(String asignatura, String tema)
        {
            try
            {
                Console.WriteLine("asignatura: " + asignatura + "tema: " + tema);
                List<string> archivos = await ControllerApiOut.ObtenerListaArchivos(asignatura, tema);


                if (archivos.Count < 26)  // cambia "10" según lo que necesite tu UI para mostrar scroll
                {
                    for (int i = archivos.Count; i < 27; i++)
                    {
                        archivos.Add(" ");
                    }
                }

                var list = new ListBox
                {
                    Margin = new Thickness(10),
                    Foreground = Brushes.White,
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    MaxHeight = 650, // Aquí limitas la altura para que aparezca la barra
                    FontSize = 14,
                    ItemsSource = archivos


                };

                ContentArea.Content = list;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
            }
        }
        //OTROS METODOS

        /// <summary>
        /// se activa al cambiar el tema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemasComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemasComboBox.SelectedIndex > 0)
            {

                string valor = AssignaturasComboBox.SelectedItem as string;
                string valor2 = TemasComboBox.SelectedItem as string;

                CargarArchivosAsync(valor, valor2);
            }
        }

        /// <summary>
        /// Metodo, se activa al cambiar la assignatura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssignaturasComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AssignaturasComboBox.SelectedIndex > 0)
            {
                //si se ha seleccionado uno que no es el 0 "Asignatura", se cargan los temas
                string valor = AssignaturasComboBox.SelectedItem as string;
                CargarTemasAsync(valor);
            }
            else
            {
                //Si se vuelve a poner El combobox assignaturas en 0 "Assignatura". Temas tambien se pone en 0
                TemasComboBox.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Metodo para agrandar la lista de archivos al iniciar para que se vea bien
        /// </summary>
        private void ListaGrandealiniciar()
        {
            var itemsVisuales = new List<object>();


            if (itemsVisuales.Count < 26)
            {
                for (int i = itemsVisuales.Count; i < 27; i++)
                {
                    itemsVisuales.Add(" ");
                }
            }


            var list = new ListBox
            {
                Margin = new Thickness(10),
                Foreground = Brushes.White,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Height = 650,
                FontSize = 14,
                ItemsSource = itemsVisuales
            };

            ContentArea.Content = list;

        }
        private void RemoveWindowBorder()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            var margins = new MARGINS() { cxLeftWidth = 0, cxRightWidth = 0, cyTopHeight = 0, cyBottomHeight = 0 };
            DwmExtendFrameIntoClientArea(hwnd, ref margins);
        }

        // Interop structs y métodos necesarios
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

        private void ToggleMiniMenu_Click(object sender, RoutedEventArgs e)
        {
            Grid parentGrid = (Grid)RightMenuPanel.Parent;

            if (RightMenuPanel.Visibility == Visibility.Collapsed)
            {
                parentGrid.ColumnDefinitions[2].Width = new GridLength(0.6, GridUnitType.Star);
                RightMenuPanel.Visibility = Visibility.Visible;
            }
            else
            {
                parentGrid.ColumnDefinitions[2].Width = new GridLength(0);
                RightMenuPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (!(sender is TabControl tabControl)) return;
            if (!(tabControl.SelectedItem is TabItem selectedTab)) return;

            if (selectedTab.Header.ToString() == "RESUMENES")
            {
                var list = new ListBox
                {
                    Margin = new Thickness(10),
                    Foreground = Brushes.White,
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    FontSize = 14,
                    ItemsSource = new List<string>
                    {
                        "Resumen 1",
                        "Resumen 2",
                        "Resumen 3"
                    }
                };

                ContentArea.Content = list;
            }
            else
            {
                ContentArea.Content = null;
            }
            */
        }

        private bool isPanelVisible = true;

        private void ToggleMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPanelVisible)
            {
                MiniPanelBox.Visibility = Visibility.Collapsed;
                ToggleMenuButton.Margin = new Thickness(0); // centrado junto al RightMenu
                ToggleMenuButton.Visibility = Visibility.Hidden;
                ToggleMenuButtonabrir.Visibility = Visibility.Visible;
            }
            else
            {
                MiniPanelBox.Visibility = Visibility.Visible;
                //ToggleMenuButton.Margin = new Thickness(0, 0, 5, 0); // botón entre caja y menú
                ToggleMenuButton.Visibility = Visibility.Visible;
                ToggleMenuButtonabrir.Visibility = Visibility.Hidden;

            }

            isPanelVisible = !isPanelVisible;
        }

        /// <summary>
        /// Metodo para cerrar la sesion de usuario. 1.cerrar usuario 2.cerrar sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Boolean cerrado = await ControllerApiOut.CerrarSession();

                if (cerrado)
                {
                    InicioSession inicio = new InicioSession();
                    inicio.Show();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
            }

        }

        //Menu de botones Principal "Arriva"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            // Acción para Nuevo
        }

        private void Abrir_Click(object sender, RoutedEventArgs e)
        {
            // Acción para Abrir
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Copiar_Click(object sender, RoutedEventArgs e)
        {
            // Acción para Copiar
        }

        private void Pegar_Click(object sender, RoutedEventArgs e)
        {
            // Acción para Pegar
        }

        private void AcercaDe_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplicación creada por ...", "Acerca de");
        }

        /// <summary>
        /// Metodo para crear la asignatura al clickear menu "ventana dialogo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CrearAsignatura_Click(object sender, RoutedEventArgs e)
        {
            var metroWindow = (MahApps.Metro.Controls.MetroWindow)this;
            var dialog = new CustomMessageDialog();

            bool accepted = await dialog.ShowAsync(metroWindow, CustomMessageDialog.DialogMode.Asignatura);

            if (accepted)
            {
                string asignatura = dialog.AssignaturaName;
                bool exito = await ControllerApiOut.AgregarAsignatura(asignatura);
                if (exito)
                {
                    CargarAsignaturasAsync();
                }
                else
                {
                    MessageBox.Show("Error al agregar la asignatura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("Operación cancelada.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        /// <summary>
        /// Metodo para crear el tema al clickear menu "ventana dialogo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CrearTema_Click(object sender, RoutedEventArgs e)
        {
            var metroWindow = (MahApps.Metro.Controls.MetroWindow)this;
            var dialog = new CustomMessageDialog();

            // Obtén la lista de asignaturas que quieres mostrar en el ComboBox

            List<string> asignaturas = await ControllerApiOut.ObtenerListaAsignaturas();

            bool accepted = await dialog.ShowAsync(metroWindow, CustomMessageDialog.DialogMode.Tema, asignaturas);

            if (accepted)
            {
                string asignaturaSeleccionada = dialog.AssignaturaName;

                string nombreTema = dialog.TemaName;

                bool exito = await ControllerApiOut.AgregarTema(asignaturaSeleccionada, nombreTema);
                if (exito)
                {

                    // Aquí recarga UI o actualiza lo necesario
                    string valor = AssignaturasComboBox.SelectedItem as string;
                    CargarTemasAsync(valor);
                }
                else
                {
                    MessageBox.Show("Error al agregar el tema.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("Operación cancelada.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}