using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace Grafica.VentanasSecundarias
{
    /// <summary>
    /// Lógica de interacción para WordSuperiorBlocNotas.xaml
    /// </summary>
    public partial class WordSuperiorBlocNotas : Window
    {

        private RichTextBox editorPrincipal;
        private bool bloqueado = false;


        public WordSuperiorBlocNotas()
        {
            InitializeComponent();
        }
        public WordSuperiorBlocNotas(RichTextBox editor)
        {
            InitializeComponent();
            editorPrincipal = editor;

            // Compartir documento al inicio
            CopiaEditor.Document = CloneDocument(editorPrincipal.Document);
        }

        private void CopiaEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (bloqueado || editorPrincipal == null) return;

            bloqueado = true;
            editorPrincipal.Document = CloneDocument(CopiaEditor.Document);
            bloqueado = false;
        }

        public void ActualizarDesdePrincipal()
        {
            if (bloqueado) return;

            bloqueado = true;
            CopiaEditor.Document = CloneDocument(editorPrincipal.Document);
            bloqueado = false;
        }

        private FlowDocument CloneDocument(FlowDocument original)
        {
            // Clona el documento completo como XAML
            string xaml = System.Windows.Markup.XamlWriter.Save(original);
            using (var stringReader = new System.IO.StringReader(xaml))
            using (var xmlReader = System.Xml.XmlReader.Create(stringReader))
            {
                return (FlowDocument)System.Windows.Markup.XamlReader.Load(xmlReader);
            }
        
        }


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
        public void CopiaEditor_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
