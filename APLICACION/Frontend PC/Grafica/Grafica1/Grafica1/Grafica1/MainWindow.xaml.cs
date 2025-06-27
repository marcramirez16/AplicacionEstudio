using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
namespace Grafica1
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private double miniPanelOriginalWidth;

        public MainWindow(EUsuario usuario)
        {

            InitializeComponent();

            miniPanelOriginalWidth = MiniPanelBox.Width;

            //Inicializar todos los componentes con el usuario adecuado
            CargarAsignaturasAsync();

        }

        private async Task CargarAsignaturasAsync()
        {
            try
            {
                List<string> asignaturas = await ControllerApiOut.ObtenerListaAsignaturas();

                AssignaturasComboBox.ItemsSource = asignaturas;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
            }
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

    }


}
