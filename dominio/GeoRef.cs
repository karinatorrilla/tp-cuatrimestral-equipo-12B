using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{

    // Clase base para las entidades(provincia, localidad)
    public class GeoRefEntity
    {
        public string id { get; set; }
        public string nombre { get; set; }
    }

    // Clase para la respuesta general de la API de Provincias
    public class GeoRefProvinciasResponse
    {
        public int cantidad { get; set; }
        public int inicio { get; set; }
        public object parametros { get; set; }
        public List<GeoRefEntity> provincias { get; set; } // Aquí están las provincias
    }

    // Clase para la respuesta general de la API de Localidades
    public class GeoRefLocalidadesResponse
    {
        public int cantidad { get; set; }
        public int inicio { get; set; }
        public object parametros { get; set; }
        public List<GeoRefEntity> localidades { get; set; } // Aquí están las localidades
    }
}
