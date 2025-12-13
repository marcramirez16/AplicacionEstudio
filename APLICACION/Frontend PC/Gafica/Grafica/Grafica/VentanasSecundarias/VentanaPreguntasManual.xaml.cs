using Grafica.entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public String idrespuesta;
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
            string texto = "";

            for (int i = 0; i < preguntasE.Count; i++)
            {
                texto += "|" + preguntasE[i].pregunta + " id: " + preguntasE[i].id_pregunta;
            }
            
            MessageBox.Show(texto);

            // Limpiamos la colección actual
            Preguntas = new ObservableCollection<Pregunta>();

            foreach (var pe in preguntasE)
            {
                string respuesta = await ControllerApiOut.ObtenerRespuesta(pe.id_pregunta.ToString());
                ERespuesta respuestaEntidad = await ControllerApiOut.ObtenerRespuestaCompleta(pe.id_pregunta.ToString());


                var p = new Pregunta
                {
                    Texto = pe.pregunta,
                    idpregunta = (int)pe.id_pregunta,
                    Guardado = true,
                    OpcionSeleccionada = pe.Tipo,
                    Respuesta = respuesta,
                    imagen = pe.Imagen,
                    idrespuesta = respuestaEntidad.id_respuesta
                };

                Preguntas.Add(p);
                MarcarTipoPregunta(p);

            }

            // Actualizamos el ItemsSource
            ListaPreguntas.ItemsSource = Preguntas;
        }


        private void MarcarTipoPregunta(Pregunta pregunta)
        {
            if (pregunta == null) return;

            void marcar()
            {
                var container = ListaPreguntas.ItemContainerGenerator.ContainerFromItem(pregunta) as FrameworkElement;
                if (container != null)
                {
                    var radios = FindVisualChildren<RadioButton>(container).ToList();
                    foreach (var r in radios)
                    {
                        if (r.Tag?.ToString() == pregunta.OpcionSeleccionada)
                        {
                            r.IsChecked = true;
                            break;
                        }
                    }

                    // Una vez marcado, quitar el evento
                    ListaPreguntas.ItemContainerGenerator.StatusChanged -= statusChangedHandler;
                }
            }

            void statusChangedHandler(object sender, EventArgs e)
            {
                if (ListaPreguntas.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                {
                    marcar();
                }
            }

            // Suscribirse al evento
            ListaPreguntas.ItemContainerGenerator.StatusChanged += statusChangedHandler;

            // Intentar marcar por si acaso ya está generado
            marcar();
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

                await ControllerApiOut.BorrarRespuesta(pregunta.idpregunta.ToString());

                await ControllerApiOut.BorrarPregunta(pregunta.idpregunta.ToString());

                await ControllerApiOut.BorrarPasoNormal(pregunta.idpregunta);

                //borrar los pasos y operaciones de todo tipo
                List<EPaso> pasoss = await ControllerApiOut.ObtenerPasos(pregunta.idpregunta);

                foreach (EPaso paso in pasoss)
                {
                    List<EOperacion> operacioness = await ControllerApiOut.ObtenerOperaciones(paso.id_paso);

                    foreach (EOperacion operacion in operacioness)
                    {
                        await ControllerApiOut.BorrarOperacion(operacion.id_operacion);
                    }
                    await ControllerApiOut.BorrarPaso(paso.id_paso);
                }}}

        /// <summary>
        /// Metodo para agregar una nueva pregunta, boton + 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarPregunta_Click(object sender, RoutedEventArgs e)
        {
            var nueva = new Pregunta { Texto = "Nueva pregunta", Guardado = false };

            Preguntas.Add(nueva);
            MarcarTipoPregunta(nueva);

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

                // Asignar imagen por defecto si no tiene ninguna
                if (string.IsNullOrEmpty(pregunta.imagen))
                {
                    try
                    {
                        var uri = new Uri(pregunta.ImagenPorDefecto); // ruta de la imagen embebida
                        using (var stream = Application.GetResourceStream(uri).Stream)
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            pregunta.imagen = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                    catch
                    {
                        // En caso de fallo, dejar null o manejar el error
                        pregunta.imagen = null;
                    }
                }

                pregunta.Guardado = true;

                // Deshabilitamos el botón visualmente
                button.IsEnabled = false;
            }

            //Guardar pregunta si el id es 0 "nueva pregunta-----------------"

            if (pregunta.idpregunta == 0)
            {
                long? idGenerado = await ControllerApiOut.AgregarPregunta(pregunta.Texto, pregunta.OpcionSeleccionada, pregunta.imagen);
                //retornar el id agregado en la bd y agregarle a la pregunta existente
                pregunta.idpregunta = (int)idGenerado;

                //guardar respusta
                int id = (int)await ControllerApiOut.AgregarRespuesta(pregunta.idpregunta.ToString(), pregunta.Respuesta);
                idrespuesta = id.ToString();
                pregunta.idrespuesta = id; //guardar la idrespuesta de la pregunta actual

            }
            //Editar pregunta si no es 0 "pregunta ya existente-----------------"
            if (pregunta.idpregunta != 0)
            {
                await ControllerApiOut.EditarPregunta(pregunta.idpregunta.ToString(), pregunta.Texto, pregunta.OpcionSeleccionada, pregunta.imagen);

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
                // La pregunta ya no está guardada
                pregunta.Guardado = false;

                // Actualizamos la opción seleccionada si hay un RadioButton marcado
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var container = ListaPreguntas.ItemContainerGenerator.ContainerFromItem(pregunta) as FrameworkElement;
                    if (container != null)
                    {
                        var radios = FindVisualChildren<RadioButton>(container).ToList();
                        var seleccionado = radios.FirstOrDefault(r => r.IsChecked == true);
                        if (seleccionado != null)
                        {
                            pregunta.OpcionSeleccionada = seleccionado.Tag.ToString();
                        }

                        // Activar el botón de guardar igual que en TextBoxRespuesta_TextChanged
                        var botones = FindVisualChildren<Button>(container).ToList();
                        if (botones.Count >= 2)
                        {
                            var botonGuardar = botones[1]; // el botón ✓
                            botonGuardar.IsEnabled = true;
                            botonGuardar.Background = Brushes.LightGreen;
                            botonGuardar.Foreground = Brushes.White;
                        }
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        /*Cambiar imagen mates*/
        private static Process calculadoraProceso = null;

        private async void Imagen_Click(object sender, MouseButtonEventArgs e)
        {
            // Si ya hay una calculadora abierta, no hacer nada
            if (calculadoraProceso != null && !calculadoraProceso.HasExited)
            {
                MessageBox.Show("La calculadora ya está abierta.");
                return;
            }

            // Abrir la calculadora y obtener el PID
            string pidString = await ControllerApiOut.AbrirCalculadoraAsync();
            if (!long.TryParse(pidString, out long pid) || pid == 0)
            {
                MessageBox.Show("No se pudo abrir la calculadora.");
                return;
            }

            try
            {
                calculadoraProceso = Process.GetProcessById((int)pid);

                // Deshabilitamos la ventana principal
                this.IsEnabled = false;

                // Esperamos a que el proceso se cierre (esto bloquea la UI de forma modal)
                calculadoraProceso.WaitForExit();

                // Una vez cerrada la calculadora, habilitamos la ventana
                this.IsEnabled = true;

                // Continuamos con la carga de la imagen
                string base64 = await ControllerApiOut.ObtenerImagenBase64Async();
                var imageControl = sender as Image;
                var pregunta = imageControl?.DataContext as Pregunta;
                if (pregunta != null)
                {
                    pregunta.imagen = base64;
                }

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

                if (imageControl != null)
                {
                    imageControl.Source = bitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al monitorear la calculadora: " + ex.Message);
                this.IsEnabled = true;
            }
            finally
            {
                calculadoraProceso = null;
            }
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

        /// <summary>
        /// Metodo para abrir ayuda con la respuesta "pasos calcular o otras ayudas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonExtra_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            // Obtener la pregunta asociada a este botón
            var pregunta = button.DataContext as Pregunta;
            if (pregunta == null) return;

            // Detectar el tipo de pregunta
            if (pregunta.Guardado)
            {
                if (pregunta.OpcionSeleccionada == "mates")
                {
                    //PasosOperacion ventanaPasos = new PasosOperacion(idrespuesta);
                    PasosOperacion ventanaPasos = new PasosOperacion(pregunta.idrespuesta.ToString());
                    ventanaPasos.ShowDialog();
                }
                if (pregunta.OpcionSeleccionada == "normal")
                {

                    PasoNormal ventanaPasonorm = new PasoNormal(pregunta.idrespuesta.ToString());
                    ventanaPasonorm.ShowDialog();
                }
            }
            else { MessageBox.Show("guarde la pregunta antes"); }

        }

        /*_-----------------------------------_*/
        /*-------------------------------------*/
        /*<summary>
         * Classe pregunta con sus atributos
         * </summary>
         */
        public class Pregunta
        {
            public long idrespuesta { get; set; } = 0;

            public string Texto { get; set; }
            public string Respuesta { get; set; }
            public bool Guardado { get; set; } = false;

            public string OpcionSeleccionada { get; set; } = "normal";
            public int idpregunta { get; set; } = 0;

            public string imagen { get; set; }

            public string ImagenPorDefecto => "pack://application:,,,/Image/ecu.png";

            // Propiedad que devuelve la imagen correcta
            public object FuenteImagen
            {
                get
                {
                    if (!string.IsNullOrEmpty(imagen))
                    {
                        try
                        {
                            byte[] bytes = Convert.FromBase64String(imagen);
                            using (var ms = new MemoryStream(bytes))
                            {
                                var bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.StreamSource = ms;
                                bitmap.EndInit();
                                bitmap.Freeze();
                                return bitmap;
                            }
                        }
                        catch
                        {
                            return ImagenPorDefecto; // si hay error con Base64, usar imagen por defecto
                        }
                    }
                    else
                    {
                        return ImagenPorDefecto; // si no hay imagen, usar por defecto
                    }
                }
            }

            public Visibility MostrarImagen
            {
                get => OpcionSeleccionada == "mates" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

    }


    namespace Grafica.VentanasSecundarias
    {
        public class TipoToBoolConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value?.ToString() == parameter?.ToString();
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if ((bool)value) return parameter?.ToString();
                return Binding.DoNothing;
            }
        }
    }
}