using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Xceed.Wpf.Toolkit.Primitives;

namespace Grafica
{
    /// <summary>
    /// Lógica de interacción para PanelMenu2.xaml
    /// </summary>
    public partial class PanelMenu2 : UserControl
    {
        public PanelMenu2()
        {
            InitializeComponent();
            fuentes();
        }

        // DependencyProperty para enlazar desde la ventana padre el RichTextBox donde se escribe el texto
        public static readonly DependencyProperty TargetRichTextBoxProperty =
            DependencyProperty.Register(nameof(TargetRichTextBox), typeof(RichTextBox), typeof(PanelMenu2), new PropertyMetadata(null));

        public RichTextBox TargetRichTextBox
        {
            get => (RichTextBox)GetValue(TargetRichTextBoxProperty);
            set => SetValue(TargetRichTextBoxProperty, value);
        }

        // --- Formato básico (B/I/U) usando EditingCommands para respetar selección/estilos ---
        private void BtnBold_Click(object sender, RoutedEventArgs e)
        {
            string directorioProyecto = System.IO.Path.GetFullPath(System.IO.Path.Combine(
             AppDomain.CurrentDomain.BaseDirectory,
             @"..\..\.."
                 ));

            // Verificar si existe una carpeta específica
            string carpetaBuscada = System.IO.Path.Combine(directorioProyecto, "Grafica", "VentanasSecundarias", "boton_funciones.xml");
            /**
            if (Directory.Exists(carpetaBuscada))
            {
                MessageBox.Show(carpetaBuscada, "✅ Carpeta Frontend encontrada");
            }
            else {
                MessageBox.Show("no encontrado");
            }**/

            if (System.IO.File.Exists(carpetaBuscada))
            {
                // Cargar el documento XML
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(carpetaBuscada);

                XmlNode negritaNode = xmlDoc.SelectSingleNode("/botones/negrita");

                string valorActual = negritaNode.InnerText;
                string nuevoValor = "";

                // Cambiar 0 por 1 y viceversa
                if (valorActual == "0")
                {
                    nuevoValor = "1";
                }
                else if (valorActual == "1")
                {
                    nuevoValor = "0";
                }

                // Actualizar el valor
                negritaNode.InnerText = nuevoValor;

                // Guardar los cambios
                xmlDoc.Save(carpetaBuscada);

            }
            else
            {
                MessageBox.Show("Error con el boton negrita");
            }
        }

        private void BtnItalic_Click(object sender, RoutedEventArgs e)
        {

            string directorioProyecto = System.IO.Path.GetFullPath(System.IO.Path.Combine(
             AppDomain.CurrentDomain.BaseDirectory,
             @"..\..\.."
                 ));

            // Verificar si existe una carpeta específica
            string carpetaBuscada = System.IO.Path.Combine(directorioProyecto, "Grafica", "VentanasSecundarias", "boton_funciones.xml");

            if (System.IO.File.Exists(carpetaBuscada))
            {
                // Cargar el documento XML
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(carpetaBuscada);

                XmlNode negritaNode = xmlDoc.SelectSingleNode("/botones/curvado");

                string valorActual = negritaNode.InnerText;
                string nuevoValor = "";

                // Cambiar 0 por 1 y viceversa
                if (valorActual == "0")
                {
                    nuevoValor = "1";
                }
                else if (valorActual == "1")
                {
                    nuevoValor = "0";
                }

                // Actualizar el valor
                negritaNode.InnerText = nuevoValor;

                // Guardar los cambios
                xmlDoc.Save(carpetaBuscada);

            }
            else
            {
                MessageBox.Show("Error con el boton curvado");
            }
        }

        private void BtnUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (TargetRichTextBox == null) return;
            EditingCommands.ToggleUnderline.Execute(null, TargetRichTextBox);
            TargetRichTextBox.Focus();
        }

        // --- Fuente / Tamaño ---
        private void FontFamilyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TargetRichTextBox == null || FontFamilyCombo.SelectedItem == null) return;
            if (FontFamilyCombo.SelectedItem is ComboBoxItem item)
            {
                var family = item.Content.ToString();
                System.Windows.Media.FontFamily miFuente = new System.Windows.Media.FontFamily("Arial");
                TargetRichTextBox.Focus();
            }
        }

        private void FontSizeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tamañoseleccionado = "";
            if (FontSizeCombo.SelectedItem is ComboBoxItem selectedItem)
            {
                tamañoseleccionado = selectedItem.Content?.ToString() ?? "";
            }
            else
            {
                tamañoseleccionado = FontSizeCombo.SelectedValue?.ToString() ?? "";
            }

            string directorioProyecto = System.IO.Path.GetFullPath(System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\.."
                ));

            // Verificar si existe una carpeta específica
            string carpetaBuscada = System.IO.Path.Combine(directorioProyecto, "Grafica", "VentanasSecundarias", "boton_funciones.xml");

            if (System.IO.File.Exists(carpetaBuscada))
            {
                // Cargar el documento XML
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(carpetaBuscada);

                XmlNode negritaNode = xmlDoc.SelectSingleNode("/botones/tamaño");

                string nuevoValor = tamañoseleccionado;


                // Actualizar el valor
                negritaNode.InnerText = nuevoValor;

                // Guardar los cambios
                xmlDoc.Save(carpetaBuscada);

            }
            else
            {
                MessageBox.Show("Error con el boton color_letra");
            }
        }

        // --- Alineación ---
        private void AlignLeft_Click(object sender, RoutedEventArgs e) => SetSelectionParagraphAlignment(TextAlignment.Left);
        private void AlignCenter_Click(object sender, RoutedEventArgs e) => SetSelectionParagraphAlignment(TextAlignment.Center);
        private void AlignRight_Click(object sender, RoutedEventArgs e) => SetSelectionParagraphAlignment(TextAlignment.Right);

        private void SetSelectionParagraphAlignment(TextAlignment alignment)
        {
            if (TargetRichTextBox == null) return;

            // Intenta obtener el párrafo de la selección
            Paragraph p = TargetRichTextBox.Selection.Start.Paragraph ?? TargetRichTextBox.Selection.End.Paragraph;
            if (p != null)
            {
                p.TextAlignment = alignment;
            }
            TargetRichTextBox.Focus();
        }

        // --- Handlers de tus botones grandes (personalízalos) ---
        private void boton_menu1_Click(object sender, RoutedEventArgs e)
        {
            // Ejemplo: insertar texto o ejecutar acción
            if (TargetRichTextBox == null) return;
            TargetRichTextBox.CaretPosition.InsertTextInRun("Acción MENÚ 1");
            TargetRichTextBox.Focus();
        }

        private void boton_menu2_Click(object sender, RoutedEventArgs e)
        {
            if (TargetRichTextBox == null) return;
            TargetRichTextBox.CaretPosition.InsertTextInRun("Acción MENÚ 2");
            TargetRichTextBox.Focus();
        }

        private void boton_menu3_Click(object sender, RoutedEventArgs e)
        {
            if (TargetRichTextBox == null) return;
            TargetRichTextBox.CaretPosition.InsertTextInRun("Acción MENÚ 3");
            TargetRichTextBox.Focus();
        }

        private void BtnHighlightColor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnTextColor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbSubrayado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo != null)
            {
               
                combo.SelectedIndex = 0;
            }
        }

        private void A_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            var seleccionado = "";

            if (combo.SelectedIndex > 0)
            {
                var selectedItem = combo.SelectedItem as ComboBoxItem;

                var seleccion = combo.SelectedIndex + 1;

                seleccionado = seleccion.ToString();

                var value = selectedItem?.Content;

                // Y vuelves a dejar la imagen
                combo.SelectedIndex = 0;

            }
    
            string directorioProyecto = System.IO.Path.GetFullPath(System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\.."
                ));

            // Verificar si existe una carpeta específica
            string carpetaBuscada = System.IO.Path.Combine(directorioProyecto, "Grafica", "VentanasSecundarias", "boton_funciones.xml");

            if (System.IO.File.Exists(carpetaBuscada))
            {
                // Cargar el documento XML
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(carpetaBuscada);

                XmlNode negritaNode = xmlDoc.SelectSingleNode("/botones/subrallado");

                string valorActual = negritaNode.InnerText;
                string nuevoValor = seleccionado;

                // Actualizar el valor
                negritaNode.InnerText = nuevoValor;

                // Guardar los cambios
                xmlDoc.Save(carpetaBuscada);

            }
            else
            {
                MessageBox.Show("Error con el boton subrallado");
            }
        }

        public void fuentes() {
            FontFamilyCombo.ItemsSource = Fonts.SystemFontFamilies.Take(25);
            FontFamilyCombo.SelectedIndex = 0;
            FontSizeCombo.SelectedIndex = 1;
        }

        private void BtnUnderlineColor_Click(object sender, RoutedEventArgs e)
        {
            var nombrecolor = "";
            using (var cd = new System.Windows.Forms.ColorDialog())
            {
                cd.FullOpen = true;
                if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var color = cd.Color;

                    nombrecolor = color.Name;
                }
            }

            string directorioProyecto = System.IO.Path.GetFullPath(System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\.."
                ));

            // Verificar si existe una carpeta específica
            string carpetaBuscada = System.IO.Path.Combine(directorioProyecto, "Grafica", "VentanasSecundarias", "boton_funciones.xml");

            if (System.IO.File.Exists(carpetaBuscada))
            {
                // Cargar el documento XML
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(carpetaBuscada);

                XmlNode negritaNode = xmlDoc.SelectSingleNode("/botones/color_letra");

                string valorActual = negritaNode.InnerText;
                string nuevoValor = nombrecolor;
                   

                // Actualizar el valor
                negritaNode.InnerText = nuevoValor;

                // Guardar los cambios
                xmlDoc.Save(carpetaBuscada);

            }
            else
            {
                MessageBox.Show("Error con el boton color_letra");
            }
        }

        private void colorfondo(object sender, RoutedEventArgs e)
        {
            var valorActual = "";
            using (var cd = new System.Windows.Forms.ColorDialog())
            {
                cd.FullOpen = true;
                if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var color = cd.Color;
                    valorActual = color.Name;   
                }
            }

            string directorioProyecto = System.IO.Path.GetFullPath(System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\.."
                ));

            // Verificar si existe una carpeta específica
            string carpetaBuscada = System.IO.Path.Combine(directorioProyecto, "Grafica", "VentanasSecundarias", "boton_funciones.xml");

            if (System.IO.File.Exists(carpetaBuscada))
            {
                // Cargar el documento XML
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(carpetaBuscada);

                XmlNode negritaNode = xmlDoc.SelectSingleNode("/botones/color_fondo");

                string nuevoValor = valorActual;


                // Actualizar el valor
                negritaNode.InnerText = nuevoValor;

                // Guardar los cambios
                xmlDoc.Save(carpetaBuscada);

            }
            else
            {
                MessageBox.Show("Error con el boton color_letra");
            }
        }

        private void FontFamilyCombo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string fuenteSeleccionada = FontFamilyCombo.SelectedItem.ToString();

            string directorioProyecto = System.IO.Path.GetFullPath(System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\.."
                ));

            // Verificar si existe una carpeta específica
            string carpetaBuscada = System.IO.Path.Combine(directorioProyecto, "Grafica", "VentanasSecundarias", "boton_funciones.xml");

            if (System.IO.File.Exists(carpetaBuscada))
            {
                // Cargar el documento XML
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(carpetaBuscada);

                XmlNode negritaNode = xmlDoc.SelectSingleNode("/botones/fuente");

                string nuevoValor = fuenteSeleccionada;


                // Actualizar el valor
                negritaNode.InnerText = nuevoValor;

                // Guardar los cambios
                xmlDoc.Save(carpetaBuscada);

            }
            else
            {
                MessageBox.Show("Error con el boton color_letra");
            }
        }

        private void boton_menu4_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

