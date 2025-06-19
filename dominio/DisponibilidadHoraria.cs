using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    // Clase que especifica los bloques horarios exactos en los que un médico
    // está disponible para atender pacientes, día por día.
    public class DisponibilidadHoraria
    {
        // Representa el día de la semana (ej., 1 para Lunes, 2 para Martes, hasta 7 para Domingo).
        public int DiaDeLaSemana { get; set; }

        // Hora de inicio exacta del bloque de atención del médico para ese día.
        public TimeSpan HoraInicioBloque { get; set; }

        // Hora de fin exacta del bloque de atención del médico para ese día.
        public TimeSpan HoraFinBloque { get; set; }
    }
}
