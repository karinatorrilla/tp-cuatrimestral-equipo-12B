﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Medico : Persona    //hereda propiedades de persona para cargar datos
    {
        public int Matricula { get; set; }
        public Especialidad EspecialidadSeleccionada { get; set; }
        public string Especialidad
        {
            get { return EspecialidadSeleccionada != null ? EspecialidadSeleccionada.Descripcion : "N/A"; }
        }
        public TurnoTrabajo TurnoTrabajo { get; set; }   //cada médico está asociado a un turno de trabajo
        public List<Especialidad> Especialidades { get; set; }  //cada médico tiene varias especialidades
    }
}
