using Grafica.entidades;
using Grafica.VentanasSecundarias;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


public class ControllerApiOut
{
    private static readonly HttpClient client = new HttpClient();
//-----------------------METODOS USUARIO
    /// <summary>
    /// Metodo apra crear usuario api
    /// </summary>
    /// <param name="usuario"></param>
    /// <returns></returns>
    public static async Task<String> EnviarUsuario(EUsuario usuario)
    {
 
        string json = JsonConvert.SerializeObject(usuario);

        using (HttpClient client = new HttpClient())
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            string url = RutaApi.ruta + "crearusuario"; 

            HttpResponseMessage response = await client.PostAsync(url, content);

            string resultado = await response.Content.ReadAsStringAsync();

            return resultado;
            //Console.WriteLine($"Respuesta del servidor: {resultado}");
        }
    }

    /// <summary>
    /// Metodo para iniciar sesion de usuario api
    /// </summary>
    /// <param name="usuario"></param>
    /// <returns></returns>
    public static async Task<EUsuario> IniciarSesion(EUsuario usuario)
    {
        string json = JsonConvert.SerializeObject(usuario);

        using (HttpClient client = new HttpClient())
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = RutaApi.ruta + "iniciarusuario"; //ruta de la api + llamada

            HttpResponseMessage response = await client.PostAsync(url, content);
            string resultado = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<EUsuario>(resultado);
            }
            else
            {
                return new EUsuario(); // Devuelve un objeto vacío en lugar de null
            }
        }
    }

    /// <summary>
    /// Metodo para saver si hay un usuario iniciado...
    /// </summary>
    /// <returns></returns>
    public static async Task<bool> UsuarioIniciado(){

        string url = RutaApi.ruta + "usuarioiniciado";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string resultado = await response.Content.ReadAsStringAsync();
                return bool.Parse(resultado);

            }
            else
            {
                return false;
            }
        }
    }


    /// <summary>
    /// Metodo para cerrar usuario
    /// </summary>
    /// <returns></returns>
    public static async Task<bool> CerrarSession()
    {

        string url = RutaApi.ruta + "cerrarUsuario";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    //------------------------METODOS DEVOLVER SERVIDOR ARCHIVOS
    /// <summary>
    /// Obtener las asignaturas en una lista
    /// </summary>
    /// <returns></returns>
    public static async Task<List<string>> ObtenerListaAsignaturas()
    {
        string url = RutaApi.ruta + "DevolverListaAssignaturas";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string resultado = await response.Content.ReadAsStringAsync();
                // Deserializar lista de strings
                return JsonConvert.DeserializeObject<List<string>>(resultado);
            }
            else
            {
                return new List<string>();
            }
        }
    }
    /// <summary>
    /// Obtener los temas en una lista
    /// </summary>
    /// <returns></returns>
    public static async Task<List<string>> ObtenerListaTemas(String asignatura)
    {
        //string url = RutaApi.ruta + "DevolverListaTemas";
        //hacer llamada con parametro
        string url = RutaApi.ruta + "DevolverListaTemas?Assignatura=" + Uri.EscapeDataString(asignatura);

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string resultado = await response.Content.ReadAsStringAsync();
                // Deserializar lista de strings
                return JsonConvert.DeserializeObject<List<string>>(resultado);
            }
            else
            {
                return new List<string>();
            }
        }
    }

    /// <summary>
    /// Metodo para obtener la lista de temas a partir de asignatura y tema, des de la api
    /// </summary>
    /// <param name="asignatura"></param>
    /// <param name="tema"></param>
    /// <returns></returns>
    public static async Task<List<string>> ObtenerListaArchivos(String asignatura, String tema)
    {
        //string url = RutaApi.ruta + "DevolverListaArchivos";
        //hacer llamada con parametros
        string url = RutaApi.ruta + "DevolverListaArchivos" + "?Assignatura=" + Uri.EscapeDataString(asignatura) + "&Tema=" + Uri.EscapeDataString(tema);
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string resultado = await response.Content.ReadAsStringAsync();
                // Deserializar lista de strings
                return JsonConvert.DeserializeObject<List<string>>(resultado);
            }
            else
            {
                return new List<string>();
            }
        }
    }


    /// <summary>
    /// Metodo para agregar asignatura nueva
    /// </summary>
    /// <returns></returns>
    public static async Task<bool> AgregarAsignatura(String nombre)
    {

        string url = RutaApi.ruta + "crearAsignatura";

        var parametros = new Dictionary<string, string>
        {
            { "nombre", nombre }
        };

        var content = new FormUrlEncodedContent(parametros);
        HttpResponseMessage response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    


    /// <summary>
    /// Metodo para agregar Tema nuevo
    /// </summary>
    /// <returns></returns>
    public static async Task<bool> AgregarTema(String nombreAssignatura, String nombreTema)
    {

        string url = RutaApi.ruta + "crearTema";

        var parametros = new Dictionary<string, string>
            {
                { "nombreAssignatura", nombreAssignatura },
                { "nombreTema", nombreTema}
            };

        var content = new FormUrlEncodedContent(parametros);
        HttpResponseMessage response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            await response.Content.ReadAsStringAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Metodo para agregar Archivo nuevo
    /// </summary>
    /// <returns></returns>
    public static async Task<bool> AgregarArchivo(String nombreAssignatura, String nombreTema, String nombreArchivo)
    {

        string url = RutaApi.ruta + "crearArchivo";

        var parametros = new Dictionary<string, string>
            {
                { "nombreAssignatura", nombreAssignatura },
                { "nombreTema", nombreTema},
                { "nombreArchivo", nombreArchivo}
            };

        var content = new FormUrlEncodedContent(parametros);
        HttpResponseMessage response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            await response.Content.ReadAsStringAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Metdo para insertar el archivo seleccionado
    /// </summary>
    /// <param name="nombreAsignatura"></param>
    /// <param name="nombreTema"></param>
    /// <param name="nombreArchivo"></param>
    /// <returns>true/false</returns>
    public static async Task<bool> SeleccionarArchivo(string nombreAsignatura, string nombreTema, string nombreArchivo)
    {
        string url = RutaApi.ruta + "SeleccionarArchivo";

        var parametros = new Dictionary<string, string>
    {
        { "nombreAsignatura", nombreAsignatura },
        { "nombreTema", nombreTema },
        { "nombreArchivo", nombreArchivo }
    };

        var content = new FormUrlEncodedContent(parametros);

        HttpResponseMessage response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            return bool.Parse(result); 
        }

        return false;
    }

    /// <summary>
    /// Metodo para deseleccionar el archivo que esta seleccionado actualmente
    /// </summary>
    /// <returns>true/false</returns>
    public static async Task<string> DeseleccionarArchivo()
    {
        string url = RutaApi.ruta + "DeseleccionarArchivo";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(url, null); 

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null; 
            }
        }
    }

   

    /// <summary>
    /// Metodo para obtener el archivo seleccionado
    /// </summary>
    /// <returns>Archivo/null</returns>
    public static async Task<Archivo> ObtenerArchivoSeleccionado()
    {
        string url = RutaApi.ruta + "ArchivoSeleccionado";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Archivo>(json);
            }
            else
            {
                return null;
            }
        }

    }

    /// <summary>
    /// Abrir archivo seleccionado
    /// </summary>
    /// <returns></returns>
    public static async Task<bool> AbrirArchivo()
    {

        string url = RutaApi.ruta + "AbrirArchivo";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Metodo para borrar asignatura "carpeta"
    /// </summary>
    /// <param name="nombreAsignatura"></param>
    /// <returns></returns>
    public static async Task<bool> borrarAsignatura(string nombreAsignatura)
    {
        string url = RutaApi.ruta + "BorrarAsignatura";

        var parametros = new Dictionary<string, string>
    {
        { "nombreAsignatura", nombreAsignatura}
    };

        var content = new FormUrlEncodedContent(parametros);

        HttpResponseMessage response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            return bool.Parse(result);
        }

        return false;
    }

    /// <summary>
    /// Metodo para borrar el tema
    /// </summary>
    /// <param name="nombreAsignatura"></param>
    /// <param name="nombreTema"></param>
    /// <returns></returns>
    public static async Task<bool> borrarTema(string nombreAsignatura, string nombreTema)
    {
        string url = RutaApi.ruta + "BorrarTema";

        var parametros = new Dictionary<string, string>
    {
        { "nombreAsignatura", nombreAsignatura},
         { "nombreTema", nombreTema}
    };

        var content = new FormUrlEncodedContent(parametros);

        HttpResponseMessage response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            return bool.Parse(result);
        }

        return false;
    }

    /// <summary>
    /// Metodo para borrar el archivo
    /// </summary>
    /// <param name="nombreAsignatura"></param>
    /// <param name="nombreTema"></param>
    /// <returns></returns>
    public static async Task<bool> borrarArchivo(string nombreAsignatura, string nombreTema, string nombreArchivo)
    {
        string url = RutaApi.ruta + "BorrarArchivo";

        var parametros = new Dictionary<string, string>
    {
        { "nombreAsignatura", nombreAsignatura},
         { "nombreTema", nombreTema},
          { "nombreArchivo", nombreArchivo}

    };

        var content = new FormUrlEncodedContent(parametros);

        HttpResponseMessage response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            return bool.Parse(result);
        }

        return false;
    }

    /// <summary>
    /// Metodo para obtener la ruta de una asignatura
    /// </summary>
    /// <param name="nombreAsignatura"></param>
    /// <returns>ruta asignatura</returns>
    public static async Task<string> ObtenerRutaAsignatura(String nombreAsignatura)
    {

        string url = RutaApi.ruta + "RutaAsignatura";

        var parametros = new Dictionary<string, string>
        {
            { "nombreAsignatura", nombreAsignatura }
        };

        var content = new FormUrlEncodedContent(parametros);
        HttpResponseMessage response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Metodo para agregar el servidor de archivos al mysql "ejecutar al iniciar sesion"
    /// </summary>
    /// insertarcarpetasusuarioensql
    public static async Task<bool> InsertarServidorMysql()
    {
        string url = RutaApi.ruta + "InsertarServidorMysql";

        using (HttpClient client = new HttpClient())
        {
            var content = new StringContent("");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
                MessageBox.Show("true");
            }
            else
            {
                return false;
                MessageBox.Show("false");

            }
        }
    }

    /// <summary>
    /// Metodo para agregar una pregunta al mysql
    /// </summary>
    /// <summary>
    /// Metodo para agregar una pregunta al mysql y obtener el ID generado
    /// </summary>
    public static async Task<long?> AgregarPregunta(string texto, String tipo, String imagen)
    {
        string url = RutaApi.ruta + "AgregarPregunta";

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "preguntat", texto },
            { "tipo", tipo },
            { "imagen", imagen }
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                if (long.TryParse(result, out long idGenerado))
                {
                    return idGenerado;
                }
                else
                {
                    // Si algo raro pasa con el parseo
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Metodo para obtener una lista de el objeto de todas las preguntas del archivo actual...
    /// </summary>
    /// <returns></returns>
    public static async Task<ObservableCollection<EPregunta>> ObtenerPreguntas()
    {
        string url = RutaApi.ruta + "ObtenerPreguntas";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                // Deserializamos la lista de EPregunta
                var lista = JsonConvert.DeserializeObject<List<EPregunta>>(json);

                // Convertimos a ObservableCollection para binding WPF
                return new ObservableCollection<EPregunta>(lista);
            }
            else
            {
                return new ObservableCollection<EPregunta>();
            }
        }
    }

    /// <summary>
    /// Metodo para borrar una pregunta a partir de su id
    /// </summary>
    /// <param name="texto"></param>
    /// <param name="tipo"></param>
    /// <returns></returns>
    public static async Task<bool> BorrarPregunta(string idpregunta)
    {
        string url = RutaApi.ruta + "BorrarPregunta";

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "idpregunta", idpregunta }
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Metodo para editar una pregunta a partir de su id
    /// </summary>
    /// <param name="idpregunta"></param>
    /// <param name="pregunta"></param>
    /// <param name="tipo"></param>
    /// <returns></returns>
    public static async Task<bool> EditarPregunta(string idpregunta, String pregunta, String tipo, String imagen)
    {
        string url = RutaApi.ruta + "EditarPregunta";

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "idpregunta", idpregunta },
            { "pregunta", pregunta },
            { "tipo", tipo },
            { "imagen", imagen}
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    /// <summary>
    /// Metodo para agregar una respuesta a partir de su id de pregunta
    /// </summary>
    /// <param name="idpregunta"></param>
    /// <param name="texto"></param>
    /// <returns></returns>
    public static async Task<long?> AgregarRespuesta(String idpregunta, String texto)
    {
        string url = RutaApi.ruta + "AgregarRespuesta";

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "idpregunta", idpregunta},
            { "texto", texto }
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                if (long.TryParse(result, out long idGenerado))
                {
                    return idGenerado;
                }
                else
                {
                    // Si algo raro pasa con el parseo
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Metodo para borrar la respuesta por su id de la pregunta
    /// </summary>
    /// <param name="idpregunta"></param>
    /// <returns></returns>
    public static async Task<bool> BorrarRespuesta(string idpregunta)
    {
        string url = RutaApi.ruta + "BorrarRespuesta"; 

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "idpregunta", idpregunta }
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Metodo para obtener respuestas
    /// </summary>
    /// <param name="idpregunta"></param>
    /// <returns></returns>
    public static async Task<string> ObtenerRespuesta(string idpregunta)
    {
        string url = RutaApi.ruta + "ObtenerRespuesta";

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "idpregunta", idpregunta }
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Metodo para editar la respuesta por el id de la pregunta
    /// </summary>
    /// <param name="idpregunta">Id de la pregunta</param>
    /// <param name="nuevaRespuesta">Texto de la nueva respuesta</param>
    /// <returns></returns>
    public static async Task<bool> EditarRespuesta(string idpregunta, string nuevaRespuesta)
    {
        string url = RutaApi.ruta + "EditarRespuesta"; 

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "idpregunta", idpregunta },
            { "nuevaRespuesta", nuevaRespuesta } 
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }
    }


    /// <summary>
    /// Borra todas las preguntas y respuestas de una asignatura y la asignatura misma
    /// </summary>
    /// <param name="nombreAsignatura">Formato esperado: "id.Nombre"</param>
    public static async Task<bool> BorrarPreguntasAsignatura(string nombreAsignatura)
    {
        string url = RutaApi.ruta + "BorrarPreguntasAsignatura";

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "nombreassignatura", nombreAsignatura }
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }
    }

    /// <summary>
    /// Borra todas las preguntas y respuestas de un tema, el tema y sus resúmenes
    /// </summary>
    /// <param name="nombreAsignatura">Formato: "id.Nombre"</param>
    /// <param name="nombreTema">Formato: "id.Nombre"</param>
    public static async Task<bool> BorrarPreguntasTema(string nombreAsignatura, string nombreTema)
    {
        string url = RutaApi.ruta + "BorrarPreguntasTema";

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "nombreassignatura", nombreAsignatura },
            { "nombretema", nombreTema }
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }
    }

    /// <summary>
    /// Borra todas las preguntas y respuestas de un resumen y el resumen mismo
    /// </summary>
    /// <param name="nombreAsignatura">Formato: "id.Nombre"</param>
    /// <param name="nombreTema">Formato: "id.Nombre"</param>
    /// <param name="nombreArchivo">Formato: "id.Nombre"</param>
    public static async Task<bool> BorrarPreguntasArchivo(string nombreAsignatura, string nombreTema, string nombreArchivo)
    {
        string url = RutaApi.ruta + "BorrarPreguntasArchivo";

        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
        {
            { "nombreassignatura", nombreAsignatura },
            { "nombretema", nombreTema },
            { "nombrearchivo", nombreArchivo }
        };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }
    }

    /// <summary>
    /// Metodos para obtener la ecuacion resuelta por la api
    /// </summary>
    /// <param name="ecuacion"></param>
    /// <returns></returns>
    /// 
    public static async Task<string> AbrirCalculadoraAsync()
    {
        string url = "http://localhost:8080/api/abrirCalculadora";
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "Error al abrir calculadora";
            }
        }
    }

    public static async Task<string> ObtenerImagenBase64Async()
    {
        string url = $"http://localhost:8080/api/ObtenerImagenBase64?nombre={"ecuacion.png"}";
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            string base64 = await response.Content.ReadAsStringAsync();
            return base64;
        }
    }

    /**
    public static async Task<string> ObtenerEcuacion(string ecuacion)
    {
        string url = "http://localhost:8080/api/ObtenerEcuacion"; 

        using (HttpClient client = new HttpClient())
        {
            // Crear los valores que se enviarán en x-www-form-urlencoded
            var values = new Dictionary<string, string>
        {
            { "ecuacion", ecuacion }
        };

            var content = new FormUrlEncodedContent(values);

            // Hacer la petición POST
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como string
                string resultado = await response.Content.ReadAsStringAsync();
                return resultado;
            }
            else
            {
                // Opcional: manejar error
                return null;
            }
        }
    }
    */



}






