using Grafica.entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


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

}






