using Grafica.entidades;
using Grafica.entidades;
using Grafica.Themes;
using Grafica.VentanasSecundarias;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Xml;


namespace Grafica
{
    /// <summary>
    /// Lógica de interacción para WordWindow.xaml
    /// </summary>
    public partial class WordWindow : Window
    {
        private ListBox lista;


        private bool isDraggingLeft = false;
        private bool isDraggingRight = false;
        private bool isDraggingFirstLine = false;
        private double canvasWidth;
        public WordWindow()
        {
            InitializeComponent();
            //Activar modo oscuro por defecto
            DarkTheme_Click();
            modoOscuroActivo = true;
            Tema_Icono.Source = new BitmapImage(new Uri("/Image/boton_negro_oscuro.png", UriKind.Relative));

            this.StateChanged += MainWindow_StateChanged;



            Titulo_Lista.Text = "RESUMEN: ..."; //AGREGAR TITULO ASIGNATURAS

            iniciarregla();
            ReiniciarXML();

        }

        /// <summary>
        /// Metodo para reinciar los estilos del word ...
        /// </summary>
        private void ReiniciarXML()
        {
            string directorioProyecto = System.IO.Path.GetFullPath(System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\.."
            ));
            string rutaXML = System.IO.Path.Combine(directorioProyecto, "Grafica", "VentanasSecundarias", "boton_funciones.xml");

            if (File.Exists(rutaXML))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(rutaXML);

                XmlNode nodoNegrita = xmlDoc.SelectSingleNode("/botones/negrita");
                XmlNode nodoCurvado = xmlDoc.SelectSingleNode("/botones/curvado");
                XmlNode nodoColorLetra = xmlDoc.SelectSingleNode("/botones/color_letra");
                XmlNode nodoColorFondo = xmlDoc.SelectSingleNode("/botones/color_fondo");
                XmlNode nodoSubrayado = xmlDoc.SelectSingleNode("/botones/subrallado");
                XmlNode nodoFuente = xmlDoc.SelectSingleNode("/botones/fuente");
                XmlNode nodoTamaño = xmlDoc.SelectSingleNode("/botones/tamaño");

                if (nodoNegrita != null) nodoNegrita.InnerText = "0";
                if (nodoCurvado != null) nodoCurvado.InnerText = "0";
                if (nodoColorLetra != null) nodoColorLetra.InnerText = "ff060606";
                if (nodoColorFondo != null) nodoColorFondo.InnerText = "0";
                if (nodoSubrayado != null) nodoSubrayado.InnerText = "0";
                if (nodoFuente != null) nodoFuente.InnerText = "Arial";
                if (nodoTamaño != null) nodoTamaño.InnerText = "15";

                xmlDoc.Save(rutaXML);
            }
            else
            {
                MessageBox.Show("No se encontró el archivo XML para reiniciar.");
            }
        }


        //---------------------------------------------------------
        //METODOS QUE REALIZAN ACCIONES
        //---------------------------------------------------------

        /// <summary>
        /// Metodo para iniciar los botones de la regla
        /// </summary>
        private void iniciarregla()
        {
            Loaded += (s, e) =>
            {
                canvasWidth = RulerCanvas.ActualWidth;

                // Posiciones iniciales
                Canvas.SetLeft(LeftMarginButton, 40);
                Canvas.SetLeft(RightMarginButton, canvasWidth - 50);
                Canvas.SetLeft(FirstLineIndentButton, 40);

                UpdateRichTextBoxMargins();
            };

            // LeftMarginButton events
            LeftMarginButton.PreviewMouseLeftButtonDown += (s, e) =>
            {
                isDraggingLeft = true;
                LeftMarginButton.CaptureMouse();
            };
            LeftMarginButton.PreviewMouseLeftButtonUp += (s, e) =>
            {
                isDraggingLeft = false;
                LeftMarginButton.ReleaseMouseCapture();
            };
            LeftMarginButton.PreviewMouseMove += (s, e) =>
            {
                if (isDraggingLeft)
                {
                    double pos = e.GetPosition(RulerCanvas).X;
                    double rightLimit = Canvas.GetLeft(RightMarginButton) - LeftMarginButton.ActualWidth - 10;

                    pos = Math.Max(0, Math.Min(pos, rightLimit));
                    Canvas.SetLeft(LeftMarginButton, pos);

                    // Mover FirstLineIndentButton si queda a la izquierda del margen izquierdo
                    double firstLinePos = Canvas.GetLeft(FirstLineIndentButton);
                    if (firstLinePos < pos)
                        Canvas.SetLeft(FirstLineIndentButton, pos);

                    UpdateRichTextBoxMargins();
                }
            };

            // RightMarginButton events
            RightMarginButton.PreviewMouseLeftButtonDown += (s, e) =>
            {
                isDraggingRight = true;
                RightMarginButton.CaptureMouse();
            };
            RightMarginButton.PreviewMouseLeftButtonUp += (s, e) =>
            {
                isDraggingRight = false;
                RightMarginButton.ReleaseMouseCapture();
            };
            RightMarginButton.PreviewMouseMove += (s, e) =>
            {
                if (isDraggingRight)
                {
                    double pos = e.GetPosition(RulerCanvas).X;
                    double leftLimit = Canvas.GetLeft(LeftMarginButton) + LeftMarginButton.ActualWidth + 10;

                    pos = Math.Max(leftLimit, Math.Min(pos, canvasWidth - RightMarginButton.ActualWidth));
                    Canvas.SetLeft(RightMarginButton, pos);
                    UpdateRichTextBoxMargins();
                }
            };

            // FirstLineIndentButton events
            FirstLineIndentButton.PreviewMouseLeftButtonDown += (s, e) =>
            {
                isDraggingFirstLine = true;
                FirstLineIndentButton.CaptureMouse();
            };
            FirstLineIndentButton.PreviewMouseLeftButtonUp += (s, e) =>
            {
                isDraggingFirstLine = false;
                FirstLineIndentButton.ReleaseMouseCapture();
            };
            FirstLineIndentButton.PreviewMouseMove += (s, e) =>
            {
                if (isDraggingFirstLine)
                {
                    double pos = e.GetPosition(RulerCanvas).X;
                    double leftLimit = Canvas.GetLeft(LeftMarginButton);
                    double rightLimit = Canvas.GetLeft(RightMarginButton);

                    pos = Math.Max(leftLimit, Math.Min(pos, rightLimit));
                    Canvas.SetLeft(FirstLineIndentButton, pos);
                    UpdateRichTextBoxMargins();
                }
            };

            // Detectar tecla Enter para aplicar sangría al nuevo párrafo
            Editor.PreviewKeyDown += Editor_PreviewKeyDown;
        }


        //METODO PARA APLICAR EL SALTO DE LINEA ANARQUICO
        /*
        private void Editor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Esperar a que se cree el nuevo párrafo
                Editor.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var caretPos = Editor.CaretPosition;
                    var currentParagraph = caretPos.Paragraph;

                    if (currentParagraph != null)
                    {
                        double leftMargin = Canvas.GetLeft(LeftMarginButton);
                        double posFirstLine = Canvas.GetLeft(FirstLineIndentButton);
                        if (double.IsNaN(posFirstLine))
                            posFirstLine = leftMargin;

                        double firstLineIndent = posFirstLine - leftMargin;
                        if (firstLineIndent < 0) firstLineIndent = 0;

                        currentParagraph.TextIndent = firstLineIndent;
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }*/

        //METODO PARA QUE EL SALTO DE LINEA SE AGA CON LOS BOTONES
        private void Editor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Bloquear Enter manual
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                e.Handled = true; // Evita crear un nuevo párrafo automáticamente

                Editor.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var caretPos = Editor.CaretPosition;
                    var currentParagraph = caretPos.Paragraph;

                    if (currentParagraph != null)
                    {
                        double leftMargin = Canvas.GetLeft(LeftMarginButton);
                        double posFirstLine = Canvas.GetLeft(FirstLineIndentButton);
                        if (double.IsNaN(posFirstLine))
                            posFirstLine = leftMargin;

                        double firstLineIndent = posFirstLine - leftMargin;
                        if (firstLineIndent < 0) firstLineIndent = 0;

                        currentParagraph.TextIndent = firstLineIndent;
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }


        private void UpdateRichTextBoxMargins()
        {
            double leftMargin = Canvas.GetLeft(LeftMarginButton);
            double rightMargin = canvasWidth - Canvas.GetLeft(RightMarginButton) - RightMarginButton.ActualWidth;

            Editor.Document.PagePadding = new Thickness(leftMargin, 20, rightMargin, 20);

            double posFirstLine = Canvas.GetLeft(FirstLineIndentButton);
            if (double.IsNaN(posFirstLine))
                posFirstLine = leftMargin;

            double firstLineIndent = posFirstLine - leftMargin;
            if (firstLineIndent < 0) firstLineIndent = 0;

            foreach (var block in Editor.Document.Blocks)
            {
                if (block is Paragraph p)
                {
                    p.TextIndent = firstLineIndent;
                }
            }
        }
        private async void deseleccionararchivoselec()
        {
            await ControllerApiOut.DeseleccionarArchivo();

        }


        //----metodos que cargan datos de la api en la ventana...
        /// <summary>
        /// Metodo para cargar las asignaturas des de la api al combobox
        /// </summary>
        /// <returns></returns>
        private async Task CargarAsignaturasAsync()
        {
            try
            {
                // Guardar el elemento seleccionado actual antes de actualizar la lista
                int seleccion = AsignaturaCombobox.SelectedIndex;


                List<string> asignaturas = await ControllerApiOut.ObtenerListaAsignaturas();

                asignaturas.Insert(0, "Assignatura");

                AsignaturaCombobox.ItemsSource = asignaturas;
                AsignaturaCombobox.SelectedIndex = seleccion;

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

                ComboboxTemas.ItemsSource = temas;
                ComboboxTemas.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar temas: " + ex.Message);
            }
        }






        private void Lista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listbox = sender as ListBox;
            if (listbox != null && listbox.SelectedItem != null)
            {

                string asignatura = AsignaturaCombobox.SelectedItem as string;

                string tema = ComboboxTemas.SelectedItem as string;

                string seleccionado = listbox.SelectedItem.ToString();

                agregarArchivoSeleccionadoAsync(asignatura, tema, seleccionado);
            }
        }

        private async Task agregarArchivoSeleccionadoAsync(String asignatura, String tema, String archivo)
        {

            await ControllerApiOut.SeleccionarArchivo(asignatura, tema, archivo);

            Archivo archivo2 = await ControllerApiOut.ObtenerArchivoSeleccionado();
            ArchivoSelect.Text = archivo2.nombreArchivo;
            //MessageBox.Show(archivo2.nombreArchivo);

        }





        //-----metodos login
        /// <summary>
        /// Metodo para cerrar la sesion de usuario. 1.cerrar usuario 2.cerrar sesion 3. Ir a login
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
                    LoginWindow inicio = new LoginWindow();
                    inicio.Show();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
            }

        }



        //----Abrir, Entrenar, carpetas y archivos...  
        /// <summary>
        /// Metodo para exportar una carpeta asignatura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Boton_Exportar_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SoloCombobox();
            dialog.Owner = this; // importante para centrar
            dialog.Title = "Exportar Asignatura";

            List<string> asignaturas = await ControllerApiOut.ObtenerListaAsignaturas();
            dialog.SetDialogMode(asignaturas);

            // Aplica desenfoque a la ventana principal
            var blurEffect = new System.Windows.Media.Effects.BlurEffect { Radius = 6 };
            this.Effect = blurEffect;

            // Mostrar diálogo modal, se abrirá centrado sobre la ventana padre
            bool? result = dialog.ShowDialog();

            // Quitar desenfoque
            this.Effect = null;



            if (result == true)
            {

                string asignatura = dialog.AssignaturaName;

                string rutaas = await ControllerApiOut.ObtenerRutaAsignatura(asignatura);

                if (!Directory.Exists(rutaas))
                {
                    MessageBox.Show("La ruta de la asignatura no existe: " + rutaas);
                    return;
                }

                // Seleccionar carpeta de destino
                var folderDialog = new CommonOpenFileDialog
                {
                    IsFolderPicker = true,
                    Title = "Selecciona la carpeta de destino para exportar la asignatura"
                };

                if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string destino = System.IO.Path.Combine(folderDialog.FileName, System.IO.Path.GetFileName(rutaas));
                    try
                    {
                        DirectoryCopy(rutaas, destino, true);
                        MessageBox.Show("Exportación completada con éxito.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al exportar: " + ex.Message);
                    }
                }

            }

        }


        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
                throw new DirectoryNotFoundException("No se encontró el directorio de origen: " + sourceDirName);

            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            // Copiar archivos
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // Copiar subdirectorios
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// Metodo para borrar asignatura, selecciona la asignatura que borrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void borrarAsingatura(object sender, RoutedEventArgs e)
        {
            var dialog = new SoloCombobox();
            dialog.Owner = this; // importante para centrar
            dialog.Title = "Borrar Asignatura";

            List<string> asignaturas = await ControllerApiOut.ObtenerListaAsignaturas();
            dialog.SetDialogMode(asignaturas);

            // Aplica desenfoque a la ventana principal
            var blurEffect = new System.Windows.Media.Effects.BlurEffect { Radius = 6 };
            this.Effect = blurEffect;

            // Mostrar diálogo modal, se abrirá centrado sobre la ventana padre
            bool? result = dialog.ShowDialog();

            // Quitar desenfoque
            this.Effect = null;

            if (result == true)
            {

                string asignatura = dialog.AssignaturaName;


                var result2 = MessageBox.Show("¿Deseas Borrar la Asingatura: " + dialog.AssignaturaName + "?", "Confirmar",
                               MessageBoxButton.YesNo,
                               MessageBoxImage.Question);

                if (result2 == MessageBoxResult.Yes)
                {
                    Boolean resultado = await ControllerApiOut.borrarAsignatura(asignatura);
                    if (!resultado)
                    {
                        MessageBox.Show("Error al borrar la asignatura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo para borrar tema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void borrarTema(object sender, RoutedEventArgs e)
        {
            var dialog = new SoloCombobox2();
            dialog.Owner = this; // importante para centrar
            dialog.Title = "Borrar Tema";

            List<string> asignaturas = await ControllerApiOut.ObtenerListaAsignaturas();
            dialog.SetAsignaturas(asignaturas);

            dialog.OnAsignaturaSeleccionada = async (asignaturaSeleccionada) =>
            {
                List<string> temas = await ControllerApiOut.ObtenerListaTemas(asignaturaSeleccionada);
                dialog.SetTemas(asignaturaSeleccionada, temas);
            };

            // Desenfoque
            this.Effect = new System.Windows.Media.Effects.BlurEffect { Radius = 6 };

            bool? result = dialog.ShowDialog();

            this.Effect = null; // Quitar desenfoque

            if (result == true)
            {

                string asignatura = dialog.AsignaturaName;
                string tema = dialog.TemaName;


                var result2 = MessageBox.Show("¿Deseas Borrar el Tema: " + dialog.TemaName + "?", "Confirmar",
                               MessageBoxButton.YesNo,
                               MessageBoxImage.Question);

                if (result2 == MessageBoxResult.Yes)
                {
                    Boolean resultado = await ControllerApiOut.borrarTema(asignatura, tema);
                    if (!resultado)
                    {
                        MessageBox.Show("Error al borrar el tema.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }

        /// <summary>
        /// Metodo para borrar Archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void borrarArchivo(object sender, RoutedEventArgs e)
        {
            var dialog = new SoloCombobox3();
            dialog.Owner = this; // importante para centrar
            dialog.Title = "Borrar Archivo";

            List<string> asignaturas = await ControllerApiOut.ObtenerListaAsignaturas();
            dialog.SetAsignaturas(asignaturas);

            dialog.OnAsignaturaSeleccionada = async (asignaturaSeleccionada) =>
            {
                List<string> temas = await ControllerApiOut.ObtenerListaTemas(asignaturaSeleccionada);
                dialog.SetTemas(asignaturaSeleccionada, temas);

            };

            dialog.OnTemaSeleccionado = async (asignatura, tema) =>
            {
                List<string> archivos = await ControllerApiOut.ObtenerListaArchivos(asignatura, tema);
                dialog.SetArchivos(asignatura, archivos);
            };

            // Desenfoque
            this.Effect = new System.Windows.Media.Effects.BlurEffect { Radius = 6 };

            bool? result = dialog.ShowDialog();

            this.Effect = null; // Quitar desenfoque

            if (result == true)
            {

                string asignatura = dialog.AsignaturaName;
                string tema = dialog.TemaName;
                string archivo = dialog.ArchivoName;

                var result2 = MessageBox.Show("¿Deseas Borrar el archivo: " + dialog.ArchivoName + "?", "Confirmar",
                               MessageBoxButton.YesNo,
                               MessageBoxImage.Question);

                if (result2 == MessageBoxResult.Yes)
                {
                    Boolean resultado = await ControllerApiOut.borrarArchivo(asignatura, tema, archivo);
                    if (!resultado)
                    {
                        MessageBox.Show("Error al borrar el archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }



        /// <summary>
        /// Metodo para abrir el archivo seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AbrirArch(object sender, RoutedEventArgs e)
        {
            Archivo archivo2 = await ControllerApiOut.ObtenerArchivoSeleccionado();

            if (archivo2 != null)
            {
                Process.Start(new ProcessStartInfo(archivo2.rutaArchivo) { UseShellExecute = true });
            }
            else { MessageBox.Show("Ningun Archivo Seleccionado!"); }

        }
  




        //-----
        /// <summary>
        /// Metodo que se activa al clickear un item del panel superior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }




        //----METODOS PARA ABRIR EL PANEL INTERMEDIO DINAMICO A PARTIR DE LOS BOTONES...
        //variable abrir panel por los bootnes "guardar el menu que esta abierto..."
        private string PanelMedioActivo = null;
        /// <summary>
        /// Metodo apra abrir menu1 al presionar el boton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void boton_menu1_Click(object sender, RoutedEventArgs e)
        {
            if (PanelMedioActivo == "menu1")
            {
                PanelIntermedio.Visibility = Visibility.Collapsed;
                PanelMedioActivo = null;
            }
            else
            {
                PanelIntermedio.Content = new PanelMenu1();
                PanelIntermedio.Visibility = Visibility.Visible;
                PanelMedioActivo = "menu1";
            }
        }
        /// <summary>
        /// Metodo para abrir el menu2 al presionar el boton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void boton_menu2_Click(object sender, RoutedEventArgs e)
        {
            if (PanelMedioActivo == "menu2")
            {
                PanelIntermedio.Visibility = Visibility.Collapsed;
                PanelMedioActivo = null;
            }
            else
            {
                PanelIntermedio.Content = new PanelMenu2();
                PanelIntermedio.Visibility = Visibility.Visible;
                PanelMedioActivo = "menu2";
            }
        }
        /// <summary>
        /// Metodo para abrir el menu3 para presionar el boton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void boton_menu3_Click(object sender, RoutedEventArgs e)
        {
            if (PanelMedioActivo == "menu3")
            {
                PanelIntermedio.Visibility = Visibility.Collapsed;
                PanelMedioActivo = null;
            }
            else
            {
                PanelIntermedio.Content = new PanelMenu3();
                PanelIntermedio.Visibility = Visibility.Visible;
                PanelMedioActivo = "menu3";
            }
        }

        //-----
        //Metodo para iniciar la lista grande con estilos
    

        //----METODOS TEMA COLOR
        //metodo para cambiar el tema oscuro o no
        private bool modoOscuroActivo = false;

        /// <summary>
        /// Metodo para cambiar el estilo al presionar el boton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Boton_Tema_Click(object sender, RoutedEventArgs e)
        {

            if (modoOscuroActivo)
            {//modo no oscuro

                ThemeManager.ApplyTheme("Light");
                Tema_Icono.Source = new BitmapImage(new Uri("/Image/boton_blanco_brillo.png", UriKind.Relative));

                //Inicializar todos los componentes con el usuario adecuado
                ComboboxTemas.SelectedIndex = 0;


            }


            else
            { //modo oscuro

                ThemeManager.ApplyTheme("Dark");
                Tema_Icono.Source = new BitmapImage(new Uri("/Image/boton_negro_oscuro.png", UriKind.Relative));

                //Inicializar todos los componentes con el usuario adecuado
                ComboboxTemas.SelectedIndex = 0;

            }

            modoOscuroActivo = !modoOscuroActivo;
        }


        //Metodos para cambiar de Thema
        /// <summary>
        /// Activiar tema light
        /// </summary>
        private void LightTheme_Click()
        {
            ThemeManager.ApplyTheme("Light");
            modoOscuroActivo = false;
        }
        /// <summary>
        /// Activar tema oscuro
        /// </summary>
        private void DarkTheme_Click()
        {
            ThemeManager.ApplyTheme("Dark");
            modoOscuroActivo = true;
        }
        //-----
        //metodo para agregar archivos y al cambiar de boton quitar los archivos...
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
                    Foreground = Brushes.Black,
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
            }*/
        }


        //------------------------------------------
        //METODOS PARA EL PANEL SUPERIOR 
        //_--------------------------------------------
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                var img = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Image/desagrandar_blanco.png")),
                    Width = 15,
                    Height = 15,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center

                };

                btnMaxRestore.Content = img;
            }
            else
            {
                var grid = new Grid { Width = 20, Height = 20, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                var rect = new Rectangle
                {
                    Stroke = Brushes.White,
                    StrokeThickness = 1,
                    Width = 10,
                    Height = 10
                };
                grid.Children.Add(rect);
                btnMaxRestore.Content = grid;
            }
        }


        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Menu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }





        private const double dragStartOffset = 350; // pixel minimo "a partir de aqui se puede arrastrar 'derecha'"

        private void rctHeader_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(rctHeader);
            if (IsAnySubmenuOpen(MainMenu))
            {

            }
            else
            {


                if (pos.X > dragStartOffset)
                {

                    if (e.OriginalSource is DependencyObject originalSource)
                    {
                        if (VisualUpwardSearch<System.Windows.Controls.Button>(originalSource) != null)
                            return;
                    }

                    if (e.ClickCount == 2)
                    {
                        MaximizeRestore_Click(sender, e);
                        e.Handled = true;
                        return;
                    }

                    if (WindowState == WindowState.Maximized)
                    {
                        // 1. Obtener posición del ratón relativa a la ventana maximizada
                        Point mouseRelative = e.GetPosition(this);

                        // 2. Obtener posición absoluta en pantalla
                        Point mouseScreenPos = PointToScreen(mouseRelative);

                        // 3. Restaurar ventana
                        WindowState = WindowState.Normal;
                        this.UpdateLayout();

                        // 4. Calcular compensación para los bordes del sistema (8px cada lado)
                        double systemBorderCompensation = 150;
                        double compensationX = (mouseRelative.X / ActualWidth) * (systemBorderCompensation * 2);

                        // 5. Posicionar ventana con compensación
                        Left = mouseScreenPos.X - mouseRelative.X - systemBorderCompensation + compensationX;
                        Top = mouseScreenPos.Y - mouseRelative.Y;

                        // 6. Iniciar arrastre
                        DragMove();
                    }
                    else
                    {
                        DragMove();
                    }
                }
            }
        }

        //Detectar si hay algun subitem del menu abierto...
        private bool IsAnySubmenuOpen(ItemsControl menu)
        {
            foreach (var item in menu.Items)
            {
                if (item is MenuItem mi)
                {
                    if (mi.IsSubmenuOpen)
                        return true;

                    // Revisa subitems recursivamente
                    if (IsAnySubmenuOpen(mi))
                        return true;
                }
            }
            return false;
        }


        private T VisualUpwardSearch<T>(DependencyObject source) where T : DependencyObject
        {
            while (source != null && !(source is T))
            {
                source = VisualTreeHelper.GetParent(source);
                // Detener la búsqueda si llegamos a un Menu (pero no confundirlo con el tipo)
                if (source is System.Windows.Controls.Primitives.MenuBase)
                    break;
            }
            return source as T;
        }

        private void rctHeader_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }
        private void rctHeader_PreviewMouseMove(object sender, MouseEventArgs e) { }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (bloqueado) return;

            bloqueado = true;

            if (ventanaBlocNotas != null && ventanaBlocNotas.IsLoaded)
            {
                ventanaBlocNotas.ActualizarDesdePrincipal();
            }

            bloqueado = false;
        }

        //----------------METODOS PARA CREAR BLOQUES DE TEMA O SUBTEMAS EN EL RICHBOX...

        private Paragraph bloqueActivo; // Párrafo actual donde se escribe
        private const int MAX_PALABRAS_BLOQUE = 50; // Límite de palabras por bloque

        private void CrearNuevoBloque()
        {
            Paragraph nuevoBloque = new Paragraph
            {
                Tag = "bloque", // identificador lógico para distinguirlos
                Margin = new Thickness(0),
                LineHeight = 20 // Espaciado visual (opcional)
            };

            Editor.Document.Blocks.Add(nuevoBloque);
            bloqueActivo = nuevoBloque;

            Editor.CaretPosition = nuevoBloque.ContentStart;
            Editor.Focus();
        }

        private void Boton_NuevoBloque_Click(object sender, RoutedEventArgs e)
        {
            CrearNuevoBloque();
        }


        //----------------METODOS PARA WORD BIDIRECCIONAL "AGRANDAR"
        private WordSuperiorBlocNotas ventanaBlocNotas;
        private bool bloqueado = false;

        private void boton_expandirword_Click(object sender, RoutedEventArgs e)
        {
            if (ventanaBlocNotas == null || !ventanaBlocNotas.IsLoaded)
            {
                ventanaBlocNotas = new WordSuperiorBlocNotas(Editor); 
                //ventanaBlocNotas.Owner = this;
                ventanaBlocNotas.Show();
            }
            else
            {
                ventanaBlocNotas.Focus();
            }
        }


        //----------------METODOS PARA EDITAR ESTILOS EN EL RICHBOX:

        /// <summary>
        /// Método para agregar estilos al escribir en el RichTextBox
        /// </summary>
        /// 
        /**
        public void Editor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;



            // Obtener todos los estilos desde XML, incluyendo subrayado
            var (colorLetra, colorFondo, esNegrita, esCursiva, fuente, tamaño, tipoSubrayado) = ObtenerEstilosDesdeXML();

            // Crear el nuevo Run con todos los estilos
            Run nuevoTexto = new Run(e.Text)
            {
                Foreground = colorLetra,
                Background = colorFondo,
                FontWeight = esNegrita ? FontWeights.Bold : FontWeights.Normal,
                FontStyle = esCursiva ? FontStyles.Italic : FontStyles.Normal,
                FontFamily = new FontFamily(fuente),
                FontSize = tamaño
            };

            // Aplicar subrayado según tipo
            if (tipoSubrayado > 0 && tipoSubrayado != 11)
            {
                nuevoTexto.TextDecorations = CrearTextDecoration(tipoSubrayado, colorLetra);
            }
            RichTextBox rtb = sender as RichTextBox;
            if (rtb != null)
            {
                TextPointer caretPos = rtb.CaretPosition;

                // Crear un nuevo párrafo si no existe
                Paragraph parrafo = caretPos.Paragraph;
                if (parrafo == null)
                {
                    parrafo = new Paragraph();
                    rtb.Document.Blocks.Add(parrafo);
                    caretPos = parrafo.ContentStart;
                }

                // Ajustar la altura de línea según el tamaño de fuente
                parrafo.LineStackingStrategy = LineStackingStrategy.MaxHeight;
                parrafo.LineHeight = tamaño * 1.2; // factor ajustable para menos espacio entre líneas

                // Agregar el Run
                caretPos.InsertTextInRun("");
                parrafo.Inlines.Add(nuevoTexto);
                rtb.CaretPosition = nuevoTexto.ElementEnd;
            }
        }
        */

        public void Editor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;

            var (colorLetra, colorFondo, esNegrita, esCursiva, fuente, tamaño, tipoSubrayado) = ObtenerEstilosDesdeXML();

            Run nuevoTexto = new Run(e.Text)
            {
                Foreground = colorLetra,
                Background = colorFondo,
                FontWeight = esNegrita ? FontWeights.Bold : FontWeights.Normal,
                FontStyle = esCursiva ? FontStyles.Italic : FontStyles.Normal,
                FontFamily = new FontFamily(fuente),
                FontSize = tamaño
            };

            if (tipoSubrayado > 0 && tipoSubrayado != 11)
            {
                nuevoTexto.TextDecorations = CrearTextDecoration(tipoSubrayado, colorLetra);
            }

            RichTextBox rtb = sender as RichTextBox;
            if (rtb == null) return;

            TextPointer caretPos = rtb.CaretPosition;
            Paragraph parrafo = caretPos.Paragraph;

            if (parrafo == null)
            {
                parrafo = new Paragraph();
                rtb.Document.Blocks.Add(parrafo);
                caretPos = parrafo.ContentStart;
            }

            // Asignar primer bloque si aún no existe
            if (bloqueActivo == null)
            {
                bloqueActivo = parrafo;
            }

            // Impedir escribir en bloques anteriores
            if (parrafo != bloqueActivo)
            {
                MessageBox.Show("Solo puedes escribir en el bloque activo. Crea un nuevo bloque si deseas continuar.");
                return;
            }

            // Verificar límite de palabras
            string textoActual = new TextRange(bloqueActivo.ContentStart, bloqueActivo.ContentEnd).Text;
            int palabras = textoActual.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

            if (palabras >= MAX_PALABRAS_BLOQUE)
            {
                MessageBox.Show($"Máximo de {MAX_PALABRAS_BLOQUE} palabras alcanzado en este bloque.");
                return;
            }

            // Aplicar altura de línea
            parrafo.LineStackingStrategy = LineStackingStrategy.MaxHeight;
            parrafo.LineHeight = tamaño * 1.2;

            // Insertar texto
            caretPos.InsertTextInRun("");
            parrafo.Inlines.Add(nuevoTexto);
            rtb.CaretPosition = nuevoTexto.ElementEnd;
        }



        /// <summary>
        /// Obtiene todos los estilos desde el XML, incluyendo subrayado
        /// </summary>
        private (SolidColorBrush, SolidColorBrush, bool, bool, string, double, int) ObtenerEstilosDesdeXML()
        {
            string rutaXML = System.IO.Path.Combine(
                System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")),
                "Grafica", "VentanasSecundarias", "boton_funciones.xml"
            );

            SolidColorBrush colorLetra = Brushes.Black;
            SolidColorBrush colorFondo = Brushes.Transparent;
            bool esNegrita = false;
            bool esCursiva = false;
            string fuente = "Arial";
            double tamaño = 12;
            int tipoSubrayado = 0;

            if (File.Exists(rutaXML))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(rutaXML);

                XmlNode nodoLetra = xmlDoc.SelectSingleNode("/botones/color_letra");
                XmlNode nodoFondo = xmlDoc.SelectSingleNode("/botones/color_fondo");
                XmlNode nodoNegrita = xmlDoc.SelectSingleNode("/botones/negrita");
                XmlNode nodoCurvado = xmlDoc.SelectSingleNode("/botones/curvado");
                XmlNode nodoFuente = xmlDoc.SelectSingleNode("/botones/fuente");
                XmlNode nodoTamaño = xmlDoc.SelectSingleNode("/botones/tamaño");
                XmlNode nodoSubrayado = xmlDoc.SelectSingleNode("/botones/subrallado");

                if (nodoLetra != null)
                    colorLetra = ConvertirHexABrush(nodoLetra.InnerText.Trim());

                if (nodoFondo != null)
                    colorFondo = ConvertirHexABrush(nodoFondo.InnerText.Trim());

                esNegrita = nodoNegrita?.InnerText.Trim() == "1";
                esCursiva = nodoCurvado?.InnerText.Trim() == "1";

                if (!string.IsNullOrWhiteSpace(nodoFuente?.InnerText))
                    fuente = nodoFuente.InnerText.Trim();

                if (double.TryParse(nodoTamaño?.InnerText.Trim(), out double tamañoParsed))
                    tamaño = tamañoParsed;

                if (int.TryParse(nodoSubrayado?.InnerText.Trim(), out int subParsed))
                    tipoSubrayado = subParsed;
            }
            else
            {
                MessageBox.Show("No se encontró el archivo XML.");
            }

            return (colorLetra, colorFondo, esNegrita, esCursiva, fuente, tamaño, tipoSubrayado);
        }

        /// <summary>
        /// Convierte un color hexadecimal AARRGGBB a SolidColorBrush
        /// </summary>
        private SolidColorBrush ConvertirHexABrush(string hex)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hex) && hex.Length == 8)
                {
                    byte a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    byte r = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    byte g = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    byte b = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                    return new SolidColorBrush(Color.FromArgb(a, r, g, b));
                }
            }
            catch
            {
                MessageBox.Show("Error al convertir color hexadecimal: " + hex);
            }

            return Brushes.Transparent;
        }

        /// <summary>
        /// Crea TextDecoration según el tipo de subrayado y color de letra
        /// </summary>
        private TextDecorationCollection CrearTextDecoration(int tipo, SolidColorBrush colorLetra)
        {
            TextDecorationCollection decoraciones = new TextDecorationCollection();

            switch (tipo)
            {
                case 1: // simple
                    decoraciones = TextDecorations.Underline;
                    break;
                case 2: // doble
                    var d1 = new TextDecoration { Location = TextDecorationLocation.Underline, Pen = new Pen(colorLetra, 1) };
                    var d2 = new TextDecoration { Location = TextDecorationLocation.Underline, Pen = new Pen(colorLetra, 1), PenOffset = 2 };
                    decoraciones.Add(d1);
                    decoraciones.Add(d2);
                    break;
                case 3: // punteado
                    var punteado = new TextDecoration
                    {
                        Location = TextDecorationLocation.Underline,
                        Pen = new Pen(colorLetra, 1) { DashStyle = DashStyles.Dot }
                    };
                    decoraciones.Add(punteado);
                    break;
                case 4: // dash
                    var dash = new TextDecoration
                    {
                        Location = TextDecorationLocation.Underline,
                        Pen = new Pen(colorLetra, 1) { DashStyle = new DashStyle(new double[] { 4, 2 }, 0) }
                    };
                    decoraciones.Add(dash);
                    break;
                case 5: // línea gruesa negra
                    decoraciones.Add(new TextDecoration { Location = TextDecorationLocation.Underline, Pen = new Pen(colorLetra, 2) });
                    break;
                case 6: // línea fina gris
                    decoraciones.Add(new TextDecoration { Location = TextDecorationLocation.Underline, Pen = new Pen(Brushes.Gray, 1) });
                    break;
                case 7: // ondulada
                    decoraciones.Add(new TextDecoration { Location = TextDecorationLocation.Underline, Pen = new Pen(colorLetra, 1), PenOffset = 1 });
                    break;
                case 8: // gruesa roja
                    decoraciones.Add(new TextDecoration { Location = TextDecorationLocation.Underline, Pen = new Pen(Brushes.Red, 3) });
                    break;
                case 9: // gruesa azul
                    decoraciones.Add(new TextDecoration { Location = TextDecorationLocation.Underline, Pen = new Pen(Brushes.Blue, 3) });
                    break;
                case 10: // gruesa amarilla
                    decoraciones.Add(new TextDecoration { Location = TextDecorationLocation.Underline, Pen = new Pen(Brushes.Yellow, 3) });
                    break;
                case 11:
                    break;
                default: // 0 = sin subrayado
                    break;
            }

            return decoraciones;
        }

        /// <summary>
        /// Actualiza el subrayado en el XML cuando se cambia el ComboBox
        /// </summary>
        private void A_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo == null) return;

            int seleccionado = combo.SelectedIndex;

            // Restaurar ComboBox visualmente
            combo.SelectedIndex = 0;

            string rutaXML = System.IO.Path.Combine(
                System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")),
                "Grafica", "VentanasSecundarias", "boton_funciones.xml"
            );

            if (File.Exists(rutaXML))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(rutaXML);

                XmlNode nodoSubrayado = xmlDoc.SelectSingleNode("/botones/subrallado");
                if (nodoSubrayado != null)
                {
                    nodoSubrayado.InnerText = seleccionado.ToString();
                    xmlDoc.Save(rutaXML);
                }
            }
            else
            {
                MessageBox.Show("Error: no se encontró el archivo XML de subrayado.");
            }
        }


    }
}