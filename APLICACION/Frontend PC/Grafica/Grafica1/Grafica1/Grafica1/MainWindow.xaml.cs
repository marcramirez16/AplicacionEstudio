using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grafica1
{
    public partial class MainWindow : Window
    {
        private bool menuOpen = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Grid parentGrid = (Grid)RightMenuPanel.Parent;

            if (!menuOpen)
            {
                parentGrid.ColumnDefinitions[1].Width = new GridLength(0.6, GridUnitType.Star);
                RightMenuPanel.Visibility = Visibility.Visible;
            }
            else
            {
                parentGrid.ColumnDefinitions[1].Width = new GridLength(0);
                RightMenuPanel.Visibility = Visibility.Collapsed;
            }

            menuOpen = !menuOpen;
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
    }
}
