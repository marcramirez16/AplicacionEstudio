using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grafica1
{
    public partial class MainWindow : Window
    {
        private double miniPanelOriginalWidth;

        public MainWindow()
        {
            InitializeComponent();

            miniPanelOriginalWidth = MiniPanelBox.Width;

        }

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
