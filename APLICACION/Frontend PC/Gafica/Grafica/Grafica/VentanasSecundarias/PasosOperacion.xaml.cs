using Grafica.VentanasSecundarias.Elementos;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static Grafica.VentanasSecundarias.PasosOperacion;
using static Xceed.Wpf.Toolkit.Calculator;


namespace Grafica.VentanasSecundarias
{
    public partial class PasosOperacion : Window
    {
        public string idrespuesta;
        public ObservableCollection<Paso> Pasos { get; set; } = new ObservableCollection<Paso>();

        public PasosOperacion(string idrespuesta)
        {
            InitializeComponent();
            this.DataContext = this;
            this.idrespuesta = idrespuesta;
            _ = inicializarPasos();
        }

        private async Task inicializarPasos()
        {
            long idresp = long.Parse(this.idrespuesta);
            List<EPaso> pasoList = await ControllerApiOut.ObtenerPasos(idresp);

            foreach (EPaso p in pasoList)
            {
                var paso = new Paso
                {
                    IdPaso = p.id_paso,
                    Numero = p.numero,
                    NombrePaso = p.textopaso,
                    Guardado = true,
                    Operaciones = new ObservableCollection<Operacion>()
                };

                List<EOperacion> operaciones = await ControllerApiOut.ObtenerOperaciones(p.id_paso);
                foreach (EOperacion o in operaciones)
                {
                    paso.Operaciones.Add(new Operacion
                    {
                        IdOperacion = o.id_operacion,
                        Valor = o.operacion,
                        Guardado = true
                    });
                }

                Pasos.Add(paso);
            }
        }

        // Agregar un nuevo paso
        private void AgregarPaso_Click(object sender, RoutedEventArgs e)
        {
            int numero = Pasos.Count + 1;
            Pasos.Add(new Paso
            {
                Numero = numero,
                NombrePaso = "",
                Guardado = false,
                Operaciones = new ObservableCollection<Operacion>()
            });
        }

        // Guardar pasos (insertar o actualizar)
        private async void GuardarPaso_Click(object sender, RoutedEventArgs e)
        {
            long idresp = Convert.ToInt64(this.idrespuesta);

            // 1️⃣ Borrar operaciones marcadas
            foreach (var idOp in operacionesBorrar)
                await ControllerApiOut.BorrarOperacion(idOp);

            operacionesBorrar.Clear();

            // 2️⃣ Borrar pasos marcados
            foreach (var idPaso in pasosBorrar)
                await ControllerApiOut.BorrarPaso(idPaso);

            pasosBorrar.Clear();

            foreach (var paso in Pasos)
            {
                // Crear EPaso para enviar al backend
                var pasoEntity = new EPaso
                {
                    id_paso = (int)paso.IdPaso,
                    id_respuesta = idresp,
                    numero = paso.Numero,
                    textopaso = paso.NombrePaso
                };

                // Si ya existe en BD -> actualizar
                if (paso.Guardado && paso.IdPaso > 0)
                {
                    Console.WriteLine($"Actualizando Paso {paso.Numero}: {paso.NombrePaso}");
                    await ControllerApiOut.ActualizarPaso(paso.IdPaso, pasoEntity);
                }
                else
                {
                    // Si es nuevo -> insertar
                    Console.WriteLine($"Agregando Paso {paso.Numero}: {paso.NombrePaso}");
                    long nuevoIdPaso = await ControllerApiOut.AgregarPasoAsync(idresp, paso.Numero, paso.NombrePaso);
                    paso.IdPaso = nuevoIdPaso;
                    paso.Guardado = true;
                }

                // Guardar o actualizar operaciones de cada paso
                int contador = 0;
                foreach (var operacion in paso.Operaciones)
                {
                    contador++;

                    // Crear entidad EOperacion
                    var operEntity = new EOperacion
                    {
                        id_operacion = (int)operacion.IdOperacion,
                        id_paso = (int)paso.IdPaso,
                        operacion = operacion.Valor,
                        numero = contador
                    };

                    if (operacion.Guardado && operacion.IdOperacion > 0)
                    {
                        await ControllerApiOut.ActualizarOperacion(operacion.IdOperacion, operEntity);
                    }
                    else
                    {
                        long? nuevoIdOp = await ControllerApiOut.AgregarOperacionAsync(paso.IdPaso, operacion.Valor, contador);
                        operacion.IdOperacion = nuevoIdOp ?? 0;
                        operacion.Guardado = true;
                    }
                }
            }

        }

        private List<long> pasosBorrar = new List<long>();  //guardar aqui los pasos borrados
        private List<long> operacionesBorrar = new List<long>(); //guardar aqui las operaciones borradas

        // Eliminar operación 
        private void EliminarOperacion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Paso paso && btn.DataContext is Operacion operacion)
            {
                if (operacion.Guardado && operacion.IdOperacion > 0)
                    operacionesBorrar.Add(operacion.IdOperacion);

                paso.Operaciones.Remove(operacion);
            }
        }

        //Eliminar paso
        private void EliminarPaso_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Paso paso)
            {
                if (paso.Guardado && paso.IdPaso > 0)
                    pasosBorrar.Add(paso.IdPaso);

                Pasos.Remove(paso);

                // Recalcular numeración
                for (int i = 0; i < Pasos.Count; i++)
                    Pasos[i].Numero = i + 1;
            }
        }


        // Agregar operación dentro de un paso
        private void AgregarOperacion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Paso paso)
            {
                paso.Operaciones.Add(new Operacion { Valor = "", Guardado = false });
            }
        }


        // Abrir calculadora al hacer click en operación
        private void OperacionTextBox_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (sender is TextBox tb)
            {
                Calculadora calc = new Calculadora();
                if (calc.ShowDialog() == true)
                {
                    tb.Text = calc.Resultado;
                }
            }
        }

        // ================== CLASES INTERNAS ==================
        public class Paso : INotifyPropertyChanged
        {
            private int numero;
            public int Numero { get => numero; set { numero = value; OnPropertyChanged(); } }

            private string nombrePaso;
            public string NombrePaso { get => nombrePaso; set { nombrePaso = value; OnPropertyChanged(); } }

            public long IdPaso { get; set; }
            public bool Guardado { get; set; } = false;
            public ObservableCollection<Operacion> Operaciones { get; set; } = new ObservableCollection<Operacion>();

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public class Operacion : INotifyPropertyChanged
        {
            private string valor;
            public string Valor { get => valor; set { valor = value; OnPropertyChanged(); } }

            public long IdOperacion { get; set; }
            public bool Guardado { get; set; } = false;

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}