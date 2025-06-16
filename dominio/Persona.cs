using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Documento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Nacionalidad { get; set; }

        /// DOMICILIO DATOS EN ARGENTINA YA QUE ES UNA APLICACION PARA ARGENTINA
        public string Provincia { get; set; } // VER SI PUEDE SER SELECTOR, BUSCAR API

        public string Localidad { get; set; } // VER SI PUEDE SER SELECTOR, BUSCAR API

        public string Calle { get; set; }

        public int Altura { get; set; }

        public int CodPostal { get; set; }

        public string Depto { get; set; } //Opcional, va null en la DB

        public DateTime FechaNacimiento { get; set; }

        public int Habilitado { get; set; } //Estado para eliminarlo logicamente 

    }
}
