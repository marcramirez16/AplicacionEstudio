using Grafica.entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static Grafica.VentanasSecundarias.VentanaPreguntasManual;

namespace Grafica.VentanasSecundarias
{
    /// <summary>
    /// Lógica de interacción para VentanaPreguntasManual.xaml
    /// </summary>
    public partial class VentanaPreguntasManual : Window
    {
        public ObservableCollection<Pregunta> Preguntas { get; set; }
        //public ObservableCollection<Pregunta> Preguntas { get; set; } = new ObservableCollection<Pregunta>();

        public VentanaPreguntasManual()
        {
            InitializeComponent();

            ListaPreguntas.ItemsSource = Preguntas;


            Preguntas = new ObservableCollection<Pregunta>()
            {

            };

            ListaPreguntas.ItemsSource = Preguntas;

            obtenerPreguntasExistentes();
        }


        public class PreguntaItem
        {
            public string Texto { get; set; }
        }

        private async void obtenerPreguntasExistentes()
        {
            // Traemos las preguntas del backend
            ObservableCollection<EPregunta> preguntasE = await ControllerApiOut.ObtenerPreguntas();

            // Limpiamos la colección actual
            Preguntas = new ObservableCollection<Pregunta>();

            foreach (var pe in preguntasE)
            {
                string respuesta = await ControllerApiOut.ObtenerRespuesta(pe.id_pregunta.ToString());

                var p = new Pregunta
                {
                    Texto = pe.pregunta,
                    idpregunta = (int)pe.id_pregunta,
                    Guardado = true,
                    OpcionSeleccionada = pe.Tipo,
                    Respuesta = respuesta,
                    //imagen = pe.Imagen
                };


                Preguntas.Add(p);
            }

            // Actualizamos el ItemsSource
            ListaPreguntas.ItemsSource = Preguntas;
        }




        private void ConfigurarPregunta_Click(object sender, RoutedEventArgs e)
        {
            PreguntaConfiguracion ventana = new PreguntaConfiguracion();
            ventana.Owner = this;
            ventana.ShowDialog();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            foreach (var pregunta in Preguntas)
            {
                MessageBox.Show($"Pregunta: {pregunta.Texto}");
            }
        }

        /// <summary>
        /// Metodo para borrar una pregunta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ToggleExpander_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var pregunta = button?.DataContext as Pregunta;

            if (pregunta != null && Preguntas.Contains(pregunta))
            {
                Preguntas.Remove(pregunta);

                await ControllerApiOut.BorrarPregunta(pregunta.idpregunta.ToString());
                await ControllerApiOut.BorrarRespuesta(pregunta.idpregunta.ToString());
            }
        }

        /// <summary>
        /// Metodo para agregar una nueva pregunta, boton + 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarPregunta_Click(object sender, RoutedEventArgs e)
        {
            var nueva = new Pregunta { Texto = "Nueva pregunta", Guardado = false };

            Preguntas.Add(nueva);

            // Esperamos a que se genere el contenedor visual
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var container = ListaPreguntas.ItemContainerGenerator.ContainerFromItem(nueva) as FrameworkElement;
                if (container != null)
                {
                    // Buscar todos los botones dentro del contenedor
                    var botones = FindVisualChildren<Button>(container).ToList();
                    if (botones.Count >= 2)
                    {
                        var botonGuardar = botones[1]; // asumimos que el segundo botón es ✓
                        botonGuardar.IsEnabled = true;
                        botonGuardar.Background = Brushes.LightGreen;
                        botonGuardar.Foreground = Brushes.White;
                    }
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        /* <summary>
         * Metodo para actualizar el estado de los botones segun si estan guardados o no
         * </summary>
         */
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T t)
                        yield return t;

                    foreach (var childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        /**
         * <summary>
         * al clickear guardar desactivar el boton de guardado
         * </summary>
         */
        private async void GuardarPregunta_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pregunta = button?.DataContext as Pregunta;

            if (pregunta != null)
            {
                var container = ListaPreguntas.ItemContainerGenerator.ContainerFromItem(pregunta) as FrameworkElement;
                if (container != null)
                {
                    // Buscar todos los RadioButtons dentro del contenedor
                    var radios = FindVisualChildren<RadioButton>(container).ToList();

                    // Tomar el primero que esté seleccionado
                    var seleccionado = radios.FirstOrDefault(r => r.IsChecked == true);
                    if (seleccionado != null)
                    {
                        pregunta.OpcionSeleccionada = seleccionado.Tag.ToString(); 
                    }
                }

                pregunta.Guardado = true;

                // Deshabilitamos el botón visualmente
                button.IsEnabled = false;
            }

            //Guardar pregunta si el id es 0 "nueva pregunta-----------------"

            if (pregunta.idpregunta == 0)
            {
                long? idGenerado = await ControllerApiOut.AgregarPregunta(pregunta.Texto, pregunta.OpcionSeleccionada);
                //retornar el id agregado en la bd y agregarle a la pregunta existente
                pregunta.idpregunta = (int)idGenerado;

                //guardar respusta
                await ControllerApiOut.AgregarRespuesta(pregunta.idpregunta.ToString(), pregunta.Respuesta);
            }
            //Editar pregunta si no es 0 "pregunta ya existente-----------------"
            if (pregunta.idpregunta != 0)
            {
                await ControllerApiOut.EditarPregunta(pregunta.idpregunta.ToString(), pregunta.Texto, pregunta.OpcionSeleccionada);

                //editar respuesta
                await ControllerApiOut.EditarRespuesta(pregunta.idpregunta.ToString(), pregunta.Respuesta);
            }
        }


        /* <summary>
         * metodos para activar boton cuando alla un cambio
         * Tambien funciones de cambio de texto o de checkbox
         * </summary>
         */
        /*pregunta*/
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var pregunta = textBox?.DataContext as Pregunta;

            if (pregunta != null)
            {
                    // Si el texto cambia, la pregunta ya no está guardada
                    pregunta.Guardado = false;

                    // Esperamos un instante para que el árbol visual esté listo
                    Dispatcher.BeginInvoke(new Action(async () =>
                    {
                        var container = ListaPreguntas.ItemContainerGenerator.ContainerFromItem(pregunta) as FrameworkElement;

                        //obtener el tipo de pregunta "matematica, normal, etc"
                        if (container != null)
                        {
                            // Buscar todos los RadioButtons dentro del contenedor
                            var radios = FindVisualChildren<RadioButton>(container).ToList();

                            // Tomar el primero que esté seleccionado
                            var seleccionado = radios.FirstOrDefault(r => r.IsChecked == true);
                            if (seleccionado != null)
                            {
                                pregunta.OpcionSeleccionada = seleccionado.Tag.ToString();
                            }
                        }
                        //si la pregunta seleccionada es matematicas, abrir escritor de ecuaciones...
                        if (pregunta.OpcionSeleccionada == "mates")
                        {


                      

                            //            await ControllerApiOut.ObtenerListaArchivos();
                            //          MessageBox.Show("abriendo creador de formulas");





                        }


                }), System.Windows.Threading.DispatcherPriority.Background);
            }

        }

        /*Cambiar imagen mates*/
        private async void Imagen_Click(object sender, MouseButtonEventArgs e) { 
            string result = await ControllerApiOut.AbrirCalculadoraAsync();
            MessageBox.Show(result);

            String base64 = await ControllerApiOut.ObtenerImagenBase64Async();
            byte[] bytes = Convert.FromBase64String(base64);

            var bitmap = new BitmapImage();
            using (var ms = new MemoryStream(bytes))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze(); 
            }

            var imageControl = sender as Image;
            if (imageControl == null) return;

            imageControl.Source = bitmap;
        }


        /*respuesta*/
        private void TextBoxRespuesta_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var pregunta = textBox?.DataContext as Pregunta;

            if (pregunta != null)
            {
                // Si cambia la respuesta, también marcamos como no guardada
                pregunta.Guardado = false;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var container = ListaPreguntas.ItemContainerGenerator.ContainerFromItem(pregunta) as FrameworkElement;
                    if (container == null) return;

                    var botones = FindVisualChildren<Button>(container).ToList();
                    if (botones.Count < 2) return;

                    var botonGuardar = botones[1]; // el botón ✓
                    botonGuardar.IsEnabled = true;
                    botonGuardar.Background = Brushes.LightGreen;
                    botonGuardar.Foreground = Brushes.White;
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }



        /*RadioButton*/
        /*
        private void RadioButton_Changed(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            var pregunta = radio?.DataContext as Pregunta;

            //guardamos el tipo de pregunta
            if (pregunta != null && radio.IsChecked == true)
            {
                // Marcamos la pregunta como no guardada
                pregunta.Guardado = false;

                // Guardamos la opción seleccionada usando el Tag
                pregunta.OpcionSeleccionada = radio.Tag.ToString();
            }

            if (pregunta != null)
            {
                // Marcamos la pregunta como no guardada
                pregunta.Guardado = false;


                // Activamos el botón de guardar
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var container = ListaPreguntas.ItemContainerGenerator.ContainerFromItem(pregunta) as FrameworkElement;
                    if (container == null) return;

                    var image = container.FindName("ImagenMates") as Image;
                    if (image == null) return;

                    if (pregunta.OpcionSeleccionada == "mates")
                    {
                        image.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        image.Visibility = Visibility.Collapsed;
                    }


                    var botones = FindVisualChildren<Button>(container).ToList();
                    if (botones.Count < 2) return;


                    var botonGuardar = botones[1]; // botón ✓
                    botonGuardar.IsEnabled = true;
                    botonGuardar.Background = Brushes.LightGreen;
                    botonGuardar.Foreground = Brushes.White;
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        */

        /*RadioButon*/
        private void RadioButton_Changed(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            var pregunta = radio?.DataContext as Pregunta;
            if (pregunta == null) return;

            if (radio.IsChecked == true)
            {
                pregunta.Guardado = false;
                pregunta.OpcionSeleccionada = radio.Tag.ToString();

                // Refrescar binding de la imagen
                var container = ListaPreguntas.ItemContainerGenerator.ContainerFromItem(pregunta) as FrameworkElement;
                if (container != null)
                {
                    var image = FindVisualChildren<Image>(container).FirstOrDefault();
                    if (image != null)
                    {
                        // Forzar que el binding se actualice
                        var bindingExpression = image.GetBindingExpression(Image.VisibilityProperty);
                        bindingExpression?.UpdateTarget();
                    }
                }
            }

            // Activar botón guardar como antes
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var container = ListaPreguntas.ItemContainerGenerator.ContainerFromItem(pregunta) as FrameworkElement;
                if (container == null) return;
                var botones = FindVisualChildren<Button>(container).ToList();
                if (botones.Count >= 2)
                {
                    var botonGuardar = botones[1];
                    botonGuardar.IsEnabled = true;
                    botonGuardar.Background = Brushes.LightGreen;
                    botonGuardar.Foreground = Brushes.White;
                }
            }));
        }



        /*_-----------------------------------_*/
        /*-------------------------------------*/
        /*<summary>
         * Classe pregunta con sus atributos
         * </summary>
         */
        public class Pregunta
        {
            public string Texto { get; set; }
            public string Respuesta { get; set; }
            public bool Guardado { get; set; } = false;

            public string OpcionSeleccionada { get; set; } = "normal";

            public int idpregunta { get; set; } = 0;

            public Visibility MostrarImagen
            {
                get => OpcionSeleccionada == "mates" ? Visibility.Visible : Visibility.Collapsed;
            }

            public string ImagenPorDefecto => "pack://application:,,,/Image/ecu.png";

//            public string imagen { get; set; }

        }
    }
}
