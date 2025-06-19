using System;
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

        // La lista de los bloques horarios específicos en los que el médico
        // está realmente disponible para atender pacientes.
        // Esto permite definir que un médico de "Turno Mañana" solo atienda
        // los lunes de 9 a 12 y los miércoles de 8 a 11, por ejemplo.
        // La lista privada que contiene los horarios, ahora con una propiedad pública
        public List<DisponibilidadHoraria> HorariosDisponibles { get; set; } = new List<DisponibilidadHoraria>();


        public string Especialidad
        {
            get { return EspecialidadSeleccionada != null ? EspecialidadSeleccionada.Descripcion : "N/A"; }
        }

        // El tipo de Turno de Trabajo general al que está asignado el médico.
        // Ej: "Turno Mañana" o "Turno Tarde".
        public TurnoTrabajo TurnoDeTrabajoAsignado { get; set; } //cada médico está asociado a un turno de trabajo

        

        public List<Especialidad> Especialidades { get; set; }  //cada médico tiene varias especialidades

    }
}
