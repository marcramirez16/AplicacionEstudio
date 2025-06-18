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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ventana_Principal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool menuOpen = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (!menuOpen)
            {
                // Abrir menú: asignar ancho a la columna derecha
                this.FindName("RightMenuPanel"); // asegurar carga
                Grid parentGrid = (Grid)RightMenuPanel.Parent;
                parentGrid.ColumnDefinitions[1].Width = new GridLength(0.60, GridUnitType.Star);
                RightMenuPanel.Visibility = Visibility.Visible;
                menuOpen = true;
            }
            else
            {
                // Cerrar menú
                Grid parentGrid = (Grid)RightMenuPanel.Parent;
                parentGrid.ColumnDefinitions[1].Width = new GridLength(0);
                RightMenuPanel.Visibility = Visibility.Collapsed;
                menuOpen = false;
            }
        }

        private void CloseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Cerrar menú
            Grid parentGrid = (Grid)RightMenuPanel.Parent;
            parentGrid.ColumnDefinitions[1].Width = new GridLength(0);
            RightMenuPanel.Visibility = Visibility.Collapsed;
            menuOpen = false;
        }

    }
}
