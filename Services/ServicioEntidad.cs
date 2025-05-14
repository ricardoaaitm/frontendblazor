using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace FrontBlazor.Services
{
    public class EntidadGenerica
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }

    public class ServicioEntidad
    {
        private readonly HttpClient _clienteHttp;
        private readonly JsonSerializerOptions _opcionesJson;

        public ServicioEntidad(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
            _opcionesJson = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Dictionary<string, object>>?> ObtenerTodosAsync(string proyecto, string tabla)
        {
            try
            {
                var url = $"{proyecto}/{tabla}";
                return await _clienteHttp.GetFromJsonAsync<List<Dictionary<string, object>>>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener datos: {ex.Message}");
                return new List<Dictionary<string, object>>();
            }
        }

        public async Task<Dictionary<string, object>?> ObtenerPorClaveAsync(string proyecto, string tabla, string nombreClave, string valorClave)
        {
            try
            {
                var url = $"{proyecto}/{tabla}/{nombreClave}/{valorClave}";
                var resultado = await _clienteHttp.GetFromJsonAsync<List<Dictionary<string, object>>>(url);
                return resultado?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener entidad: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CrearAsync(string proyecto, string tabla, Dictionary<string, object> entidad)
        {
            try
            {
                var url = $"{proyecto}/{tabla}";
                var contenido = new StringContent(JsonSerializer.Serialize(entidad), Encoding.UTF8, "application/json");
                var respuesta = await _clienteHttp.PostAsync(url, contenido);
                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear entidad: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(string proyecto, string tabla, string nombreClave, string valorClave, Dictionary<string, object> entidad)
        {
            try
            {
                var url = $"{proyecto}/{tabla}/{nombreClave}/{valorClave}";
                var contenido = new StringContent(JsonSerializer.Serialize(entidad), Encoding.UTF8, "application/json");
                var respuesta = await _clienteHttp.PutAsync(url, contenido);
                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar entidad: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarAsync(string proyecto, string tabla, string nombreClave, string valorClave)
        {
            try
            {
                var url = $"{proyecto}/{tabla}/{nombreClave}/{valorClave}";
                var respuesta = await _clienteHttp.DeleteAsync(url);
                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar entidad: {ex.Message}");
                return false;
            }
        }

        public async Task<List<EntidadGenerica>> ObtenerFiltradosAsync(string tabla, string parametro, string valor)
        {
            try
            {
                var url = $"proyecto/sp{tabla}Por{parametro}?{parametro}={valor}";
                return await _clienteHttp.GetFromJsonAsync<List<EntidadGenerica>>(url, _opcionesJson) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener lista filtrada: {ex.Message}");
                return new();
            }
        }
    }
}
