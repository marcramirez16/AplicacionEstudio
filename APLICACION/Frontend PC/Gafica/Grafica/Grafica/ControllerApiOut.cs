using Grafica1.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

            string url = RutaApi.ruta + "crearusuario"; //ruta de la api + llamada

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
        }






