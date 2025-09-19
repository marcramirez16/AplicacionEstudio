using Grafica.entidades;
using Grafica.Themes;
using Grafica.VentanasSecundarias;
using MahApps.Metro.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Grafica
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ListBox lista;

        public MainWindow(EUsuario eusuario)
        {
            InitializeComponent();
            //Activar modo oscuro por defecto
            DarkTheme_Click();
            modoOscuroActivo = true;
            Tema_Icono.Source = new BitmapImage(new Uri("/Image/boton_negro_oscuro.png", UriKind.Relative));

            this.StateChanged += MainWindow_StateChanged;

            //Crear lista
            ListaGrandealiniciar();

            //Inicializar todos los componentes con el usuario adecuado
            CargarAsignaturasAsync(); //Cargar asignaturas en combobox
            CargarAsignaurasEnLista(); //Cargar asignaturas en la lista

            Titulo_Lista.Text = "Lista Asignaturas"; //AGREGAR TITULO ASIGNATURAS

            deseleccionararchivoselec();

        }

        //---------------------------------------------------------
        //METODOS QUE REALIZAN ACCIONES
        //---------------------------------------------------------

        private async void deseleccionararchivoselec() {
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

        /// <summary>
        /// Metodo, se activa al cambiar la assignatura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssignaturasComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AsignaturaCombobox.SelectedIndex > 0)
            {
                //si se ha seleccionado uno que no es el 0 "Asignatura", se cargan los temas
                string valor = AsignaturaCombobox.SelectedItem as string;
                CargarAsignaturasAsync();
                CargarTemasAsync(valor);
                CargarTemasEnLista(valor);
                Titulo_Lista.Text = "Lista Temas de " + valor;

            }
            else
            {
                //Si se vuelve a poner El combobox assignaturas en 0 "Assignatura". Temas tambien se pone en 0
                ComboboxTemas.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// se activa al cambiar el tema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemasComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboboxTemas.SelectedIndex > 0)
            {

                string valor = AsignaturaCombobox.SelectedItem as string;
                string valor2 = ComboboxTemas.SelectedItem as string;

                CargarArchivosAsync(valor, valor2);
                Titulo_Lista.Text = "Lista Archivos de " + valor2;

            }
        }

        /// <summary>
        /// Metodo para cargar los archvios y metodo para seleccionar archivo
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

                lista = new ListBox
                {
                    Margin = new Thickness(10),
                    Foreground = Brushes.White,
                    //Background = Brushes.Transparent, // Aquí para que no tape el color del ContentArea
                    Background = (Brush)FindResource("ColorFondoLista"),

                    BorderThickness = new Thickness(0),
                    Height = 600,
                    Width = 900,
                    FontSize = 14,
                    ItemsSource = archivos
                };

                lista.MouseDoubleClick += Lista_MouseDoubleClick; //Crear evento para la lista

                ContentArea.Content = lista;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
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

        private async Task agregarArchivoSeleccionadoAsync(String asignatura, String tema, String archivo) {
           
            await ControllerApiOut.SeleccionarArchivo(asignatura, tema, archivo);
            
            Archivo archivo2 = await ControllerApiOut.ObtenerArchivoSeleccionado();
            ArchivoSelect.Text = archivo2.nombreArchivo;
            //MessageBox.Show(archivo2.nombreArchivo);

        }

        /// <summary>
        /// Cargar todas las asignaturas en la lista grande, tambien metodo para seleccionar asignatura ymostrar temas
        /// </summary>
        /// <returns></returns>
        public async Task CargarAsignaurasEnLista() {

            try
            {
                List<string> asignaturas = await ControllerApiOut.ObtenerListaAsignaturas();


                if (asignaturas.Count < 26)  // cambia "10" según lo que necesite tu UI para mostrar scroll
                {
                    for (int i = asignaturas.Count; i < 27; i++)
                    {
                        asignaturas.Add(" ");
                    }
                }

                lista = new ListBox
                {
                    Margin = new Thickness(10),
                    Foreground = Brushes.White,
                    //Background = Brushes.Transparent, // Aquí para que no tape el color del ContentArea
                    Background = (Brush)FindResource("ColorFondoLista"),

                    BorderThickness = new Thickness(0),
                    Height = 600,
                    Width = 900,
                    FontSize = 14,
                    ItemsSource = asignaturas
                };

                lista.MouseDoubleClick += Lista_MouseDoubleClickAsignatura; //Crear evento para la lista

                ContentArea.Content = lista;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
            }
        }
        private void Lista_MouseDoubleClickAsignatura(object sender, MouseButtonEventArgs e)
        {
            var listbox = sender as ListBox;
            if (listbox != null && listbox.SelectedItem != null)
            {
                string seleccionado = listbox.SelectedItem.ToString();
                
                //Cargar temas en lista
                CargarAsignaturasAsync();
                CargarTemasAsync(seleccionado);
                CargarTemasEnLista(seleccionado);
                AsignaturaCombobox.SelectedItem = seleccionado;
                Titulo_Lista.Text = "Lista Temas de " + seleccionado;

   
            }
        }

        /// <summary>
        /// Cargar todos los temas en la lista grande, tambien seleccionar el tema para cargar sus archivos
        /// </summary>
        /// <returns></returns>
        public async Task CargarTemasEnLista(String asignatura)
        {

            try
            {
                List<string> temas = await ControllerApiOut.ObtenerListaTemas(asignatura);



                if (temas.Count < 26)  // cambia "10" según lo que necesite tu UI para mostrar scroll
                {
                    for (int i = temas.Count; i < 27; i++)
                    {
                        temas.Add(" ");
                    }
                }

                lista = new ListBox
                {
                    Margin = new Thickness(10),
                    Foreground = Brushes.White,
                    //Background = Brushes.Transparent, // Aquí para que no tape el color del ContentArea
                    Background = (Brush)FindResource("ColorFondoLista"),

                    BorderThickness = new Thickness(0),
                    Height = 600,
                    Width = 900,
                    FontSize = 14,
                    ItemsSource = temas
                };

                lista.MouseDoubleClick += Lista_MouseDoubleClickTema; //Crear evento para la lista

                ContentArea.Content = lista;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asignaturas: " + ex.Message);
            }
        }

        private void Lista_MouseDoubleClickTema(object sender, MouseButtonEventArgs e)
        {
            var listbox = sender as ListBox;
            if (listbox != null && listbox.SelectedItem != null)
            {
                string seleccionado = listbox.SelectedItem.ToString();

                string valor = AsignaturaCombobox.SelectedItem as string;

                CargarArchivosAsync(valor, seleccionado);

                ComboboxTemas.SelectedItem = seleccionado;

                Titulo_Lista.Text = "Lista Archivos de " + seleccionado;
            }
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
                }  }
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
        private async void AbrirArch(object sender, RoutedEventArgs e) {
            Archivo archivo2 = await ControllerApiOut.ObtenerArchivoSeleccionado();

            if (archivo2 != null)
            {
                Process.Start(new ProcessStartInfo(archivo2.rutaArchivo) { UseShellExecute = true });
            }
            else { MessageBox.Show("Ningun Archivo Seleccionado!"); }

        }
        //----Crear asignaturas, temas, archivos...
        /// <summary>
        /// Metodo para crear neuva asignatura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CrearAsignatura_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new CustomMessageDialog();
            dialog.Owner = this; // importante para centrar

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
                bool exito = await ControllerApiOut.AgregarAsignatura(asignatura);

                if (exito)
                {
                    AsignaturaCombobox.SelectedItem = asignatura;

                    CargarAsignaturasAsync();
                    CargarAsignaurasEnLista();

                    Titulo_Lista.Text = "Lista asignautras";

                }
                else
                {
                    MessageBox.Show("Error al agregar la asignatura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //MessageBox.Show("Asignatura creada: " + asignatura, "Éxito", MessageBoxButton.OK, MessageBoxImage.None);
            }

           
        }

        /// <summary>
        /// Metodo para crear nuevo tema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CrearTema_Click(object sender, RoutedEventArgs e)
        { 
            var dialog = new CustomMessageDialogComboBox();
            dialog.Owner = this; // importante para centrar

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
                String tema = dialog.TemaName;
                bool exito = await ControllerApiOut.AgregarTema(asignatura, tema);
                if (exito)
                {

                    AsignaturaCombobox.SelectedItem = asignatura;
                    ComboboxTemas.SelectedItem = tema;

                    CargarAsignaturasAsync();
                    CargarTemasAsync(asignatura);
                    CargarTemasEnLista(asignatura);
                    Titulo_Lista.Text = "Lista Temas de " + asignatura;
                }
                else
                {
                    MessageBox.Show("Error al agregar el tema.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
        }

        /// <summary>
        /// Metodo para crear nuevo archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CrearArchivo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CustomMessageDialogComboBox2();
            dialog.Owner = this;

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
                string nombreArchivo = dialog.NombreArchivo;

                // Intentamos agregar el archivo.
                bool exito = await ControllerApiOut.AgregarArchivo(asignatura, tema, nombreArchivo);

                if (exito)
                {
                    AsignaturaCombobox.SelectedItem = asignatura;
                    ComboboxTemas.SelectedItem = tema;

                    CargarAsignaturasAsync();
                    CargarTemasAsync(asignatura);
                    CargarTemasEnLista(asignatura);
                    Titulo_Lista.Text = "Lista Temas de " + asignatura;
                }
                else
                {

                    // Si el archivo no se creó correctamente, mostramos el mensaje de error.
                    MessageBox.Show("Error al crear el archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
        /// <summary>
        /// Metodo para crear la lista "lista resumenes" con los estilos adecuados al iniciar
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
                //Background = Brushes.Transparent, // Aquí para que no tape el color del ContentArea
                Background = (Brush)FindResource("ColorFondoLista"),

                BorderThickness = new Thickness(0),
                Height = 600,
                Width = 900,
                FontSize = 14,
                ItemsSource = itemsVisuales
            };

            ContentArea.Content = list;

        }

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
                ListaGrandealiniciar();

                //Inicializar todos los componentes con el usuario adecuado
                CargarAsignaturasAsync(); //Cargar asignaturas en combobox
                CargarAsignaurasEnLista(); //Cargar asignaturas en la lista
                ComboboxTemas.SelectedIndex = 0;
                Titulo_Lista.Text = "Lista Asignaturas"; //AGREGAR TITULO ASIGNATURAS


            }


            else
            { //modo oscuro

                ThemeManager.ApplyTheme("Dark");
                Tema_Icono.Source = new BitmapImage(new Uri("/Image/boton_negro_oscuro.png", UriKind.Relative));
                ListaGrandealiniciar();

                //Inicializar todos los componentes con el usuario adecuado
                CargarAsignaturasAsync(); //Cargar asignaturas en combobox
                CargarAsignaurasEnLista(); //Cargar asignaturas en la lista
                ComboboxTemas.SelectedIndex = 0;
                Titulo_Lista.Text = "Lista Asignaturas"; //AGREGAR TITULO ASIGNATURAS

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
    

    }
}
