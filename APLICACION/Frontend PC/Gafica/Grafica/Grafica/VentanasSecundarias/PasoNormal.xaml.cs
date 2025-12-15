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
using System.Windows.Shapes;

using System.Windows;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Grafica.VentanasSecundarias
{
    /// <summary>
    /// Lógica de interacción para PasoNormal.xaml
    /// </summary>
    public partial class PasoNormal : Window
    {
        private long id_respuesta;

        public PasoNormal(string id_resumen)
        {
            InitializeComponent();
            id_respuesta = long.Parse(id_resumen);
            CargarPasoNormal();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ActualizarModo();
        }

        private void ActualizarModo()
        {
            if (ModoToggle.IsChecked == true)
            {
                PanelPasoNormal.Visibility = Visibility.Visible;
                PanelPasoSelector.Visibility = Visibility.Collapsed;
            }
            else
            {
                PanelPasoNormal.Visibility = Visibility.Collapsed;
                PanelPasoSelector.Visibility = Visibility.Visible;
                ObtenerPasos();
            }
        }

        private void ModoToggle_Click(object sender, RoutedEventArgs e)
        {
            ActualizarModo();
        }

        private void ModoToggle_Checked(object sender, RoutedEventArgs e)
        {
            PanelPasoNormal.Visibility = Visibility.Visible;
            PanelPasoSelector.Visibility = Visibility.Collapsed;
        }

        private void ModoToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            PanelPasoNormal.Visibility = Visibility.Collapsed;
            PanelPasoSelector.Visibility = Visibility.Visible;
            ObtenerPasos();
        }

        // ================= CARGAR PASOS Y RESPUESTAS =================
        private async void ObtenerPasos()
        {
            ContenedorRelaciones.Children.Clear();

            List<EPasoSelector> listaPasos = await ControllerApiOut.ObtenerPasosSelectors(id_respuesta);
            if (listaPasos == null) return;

            foreach (var paso in listaPasos)
            {
                List<ERespuestaSelector> listaRespuestas = await ControllerApiOut.ObtenerRespuestasSelector(paso.id_paso);
                long? idRespuesta = listaRespuestas != null && listaRespuestas.Count > 0
                    ? listaRespuestas[0].id_respuesta
                    : (long?)null;

                Grid grid = new Grid
                {
                    Margin = new Thickness(0, 5, 0, 5),
                    Tag = new PasoTag { IdPaso = paso.id_paso, IdRespuesta = idRespuesta }
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Columna de eliminar

                TextBox txtPregunta = new TextBox
                {
                    Height = 35,
                    Margin = new Thickness(0, 0, 5, 0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Text = paso.texto
                };
                Grid.SetColumn(txtPregunta, 0);

                TextBox txtRespuesta = new TextBox
                {
                    Height = 35,
                    Margin = new Thickness(5, 0, 0, 0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Text = idRespuesta != null ? listaRespuestas[0].texto : ""
                };
                Grid.SetColumn(txtRespuesta, 1);

                Button btnEliminar = new Button
                {
                    Content = "🗑️",
                    Width = 35,
                    Height = 35,
                    Margin = new Thickness(5, 0, 0, 0),
                    Tag = grid
                };
                btnEliminar.Click += EliminarPaso_Click;
                Grid.SetColumn(btnEliminar, 2);

                grid.Children.Add(txtPregunta);
                grid.Children.Add(txtRespuesta);
                grid.Children.Add(btnEliminar);

                ContenedorRelaciones.Children.Add(grid);
            }
        }

        // ================= GUARDAR =================
        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (ModoToggle.IsChecked == true)
            {
                // MODO NORMAL
                await ControllerApiOut.GuardarPasoNormal(id_respuesta, TextoIngresado.Text);
                MessageBox.Show("Paso normal guardado correctamente ✔");
            }
            else
            {
                int numero = 0;
                foreach (Grid grid in ContenedorRelaciones.Children)
                {
                    numero++;

                    TextBox pregunta = grid.Children[0] as TextBox;
                    TextBox respuesta = grid.Children[1] as TextBox;

                    string textoPregunta = pregunta.Text;
                    string textoRespuesta = respuesta.Text;

                    var tag = grid.Tag as PasoTag;

                    // Guardamos o actualizamos paso
                    EPasoSelector paso = await ControllerApiOut.GuardarPasoSelector(
                        tag?.IdPaso,
                        id_respuesta,
                        numero.ToString(),
                        textoPregunta
                    );

                    if (paso != null)
                    {
                        // Guardamos o actualizamos respuesta
                        await ControllerApiOut.GuardarRespuestaSelector(
                            tag?.IdRespuesta,
                            paso.id_paso,
                            textoRespuesta
                        );

                        // Actualizamos Tag para futuras modificaciones
                        grid.Tag = new PasoTag { IdPaso = paso.id_paso, IdRespuesta = tag?.IdRespuesta };
                    }
                }

                MessageBox.Show("Pasos selector guardados correctamente ✔");
            }
        }

        private async void EliminarPaso_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Grid grid = btn.Tag as Grid;
            if (grid == null) return;

            var tag = grid.Tag as PasoTag;

            // Confirmación de eliminación
            var result = MessageBox.Show("¿Desea eliminar este paso?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            // Llamada API para eliminar respuesta si existe
            if (tag?.IdRespuesta != null)
                await ControllerApiOut.EliminarRespuestaSelector(tag.IdRespuesta.Value);

            // Llamada API para eliminar paso si existe
            if (tag?.IdPaso != null)
                await ControllerApiOut.EliminarPasoSelector(tag.IdPaso.Value);

            // Removemos el Grid de la UI
            ContenedorRelaciones.Children.Remove(grid);
        }


        // ================= PASO NORMAL =================
        private async void CargarPasoNormal()
        {
            TextoIngresado.Text = await ControllerApiOut.ObtenerPasoNormal(id_respuesta);
        }

        // ================= AGREGAR RELACIÓN MANUAL =================
        private void AgregarRelacion_Click(object sender, RoutedEventArgs e)
        {
            Grid grid = new Grid { Margin = new Thickness(0, 5, 0, 5) };
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            TextBox txtPregunta = new TextBox
            {
                Height = 35,
                Margin = new Thickness(0, 0, 5, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                ToolTip = "Pregunta"
            };
            Grid.SetColumn(txtPregunta, 0);

            TextBox txtRespuesta = new TextBox
            {
                Height = 35,
                Margin = new Thickness(5, 0, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                ToolTip = "Respuesta"
            };
            Grid.SetColumn(txtRespuesta, 1);

            grid.Children.Add(txtPregunta);
            grid.Children.Add(txtRespuesta);

            // Inicializamos Tag como null
            grid.Tag = new PasoTag { IdPaso = null, IdRespuesta = null };

            ContenedorRelaciones.Children.Add(grid);
        }

        // ================= CLASE AUXILIAR PARA GUARDAR TAG =================
        private class PasoTag
        {
            public long? IdPaso { get; set; }
            public long? IdRespuesta { get; set; }
        }
    }
}
