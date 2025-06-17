using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using dominio;

namespace negocio
{
    public class GeoRefNegocio
    {
        public readonly HttpClient _httpClient;
        public const string GeoRefApiBaseUrl = "https://apis.datos.gob.ar/georef/api/";

        public GeoRefNegocio()
        {
            _httpClient = new HttpClient();
        }

        /// Obtiene una lista de todas las provincias de la API de GeoRef.
        /// <returns>Una lista de objetos GeoRefEntity (id, nombre) para provincias.</returns>
        public async Task<List<GeoRefEntity>> ObtenerProvinciasAsync()
        {
            try
            {
                string url = $"{GeoRefApiBaseUrl}provincias?campos=id,nombre";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Lanza una excepción si el código de estado HTTP es un error

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializa la respuesta usando la clase GeoRefProvinciasResponse
                var geoRefResponse = JsonConvert.DeserializeObject<GeoRefProvinciasResponse>(jsonResponse);

                if (geoRefResponse != null && geoRefResponse.provincias != null)
                {
                    // Ordenar las provincias alfabéticamente por nombre
                    return geoRefResponse.provincias.OrderBy(p => p.nombre).ToList();
                }
                return new List<GeoRefEntity>();
            }
            catch (HttpRequestException ex)
            {
                // Manejo de errores de conexión o HTTP
                throw new Exception($"Error al conectar con la API de provincias: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                // Manejo de errores de deserialización JSON
                throw new Exception($"Error al procesar la respuesta de provincias de la API: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Otros errores inesperados
                throw new Exception($"Error inesperado al obtener provincias: {ex.Message}", ex);
            }
        }

  
        /// Obtiene una lista de localidades para una provincia específica de la API de GeoRef.
        /// <param name="idProvincia">El ID de la provincia.</param>
        /// <returns>Una lista de objetos GeoRefEntity (id, nombre) para localidades.</returns>
        public async Task<List<GeoRefEntity>> ObtenerLocalidadesPorProvinciaAsync(string idProvincia) // idProvincia es string según la API
        {
            if (string.IsNullOrWhiteSpace(idProvincia))
            {
                return new List<GeoRefEntity>(); // No hay provincia seleccionada, retornar lista vacía
            }

            try
            {
                // Usamos 'id_provincia' como parámetro para filtrar localidades por provincia
                // También agregamos '&max=2000' para asegurarnos de obtener la mayoría de las localidades
                string url = $"{GeoRefApiBaseUrl}localidades?provincia={idProvincia}&campos=id,nombre&max=2000";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializa la respuesta usando la clase GeoRefLocalidadesResponse
                var geoRefResponse = JsonConvert.DeserializeObject<GeoRefLocalidadesResponse>(jsonResponse);

                if (geoRefResponse != null && geoRefResponse.localidades != null)
                {
                    // Ordenar las localidades alfabéticamente por nombre
                    return geoRefResponse.localidades.OrderBy(l => l.nombre).ToList();
                }
                return new List<GeoRefEntity>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error al conectar con la API de localidades para la provincia {idProvincia}: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error al procesar la respuesta de localidades de la API para la provincia {idProvincia}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener localidades para la provincia {idProvincia}: {ex.Message}", ex);
            }
        }
    }
}
