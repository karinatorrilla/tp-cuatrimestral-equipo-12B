﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Paciente : Persona     //hereda propiedades de persona para cargar datos
    {
        public string ObraSocial { get; set; }

        public string Observaciones { get; set; } ////////////////////////
    }
}
