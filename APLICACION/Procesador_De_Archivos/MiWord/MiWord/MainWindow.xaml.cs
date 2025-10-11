using Fluent;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace MiWord
{
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ---- Portapapeles ----
        private void Copy_Click(object sender, RoutedEventArgs e) => editor.Copy();
        private void Paste_Click(object sender, RoutedEventArgs e) => editor.Paste();
        private void Cut_Click(object sender, RoutedEventArgs e) => editor.Cut();

        // ---- Formato ----
        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleBold.Execute(null, editor);
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleItalic.Execute(null, editor);
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleUnderline.Execute(null, editor);
        }

        // ---- Botones de ventana ----
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // ---- Arrastrar ventana ----
        private void rctHeader_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try { DragMove(); }
                catch { /* ignorar */ }
            }
        }

        // ---- Doble clic en header para maximizar/restaurar ----
        private void rctHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                MaximizeRestore_Click(sender, e);
            }
        }
    }
}
