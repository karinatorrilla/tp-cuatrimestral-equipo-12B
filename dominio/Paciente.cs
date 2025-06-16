using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Paciente : Persona     //hereda propiedades de persona para cargar datos
    {
        public string ObraSocial { get; set; } // VER SI PUEDE SER SELECTOR, BUSCAR API

        public string Observaciones { get; set; } ////////////////////////
    }
}
