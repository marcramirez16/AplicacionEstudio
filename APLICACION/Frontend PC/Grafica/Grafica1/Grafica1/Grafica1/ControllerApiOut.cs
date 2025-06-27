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

}
