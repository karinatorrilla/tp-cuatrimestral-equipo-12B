using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    // Clase que especifica los bloques horarios exactos en los que un médico
    public class DisponibilidadHoraria
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        // Representa el día de la semana (ej., 1 para Lunes, 2 para Martes, hasta 7 para Domingo).
        public int DiaDeLaSemana { get; set; }

        public string DiaSemanaDescripcion
        {
            get
            {
                switch (DiaDeLaSemana)
                {
                    case 1: return "Lunes";
                    case 2: return "Martes";
                    case 3: return "Miércoles";
                    case 4: return "Jueves";
                    case 5: return "Viernes";
                    case 6: return "Sábado";
                    case 7: return "Domingo";
                    default: return "Desconocido";
                }
            }
        }

        // Hora de inicio exacta del bloque de atención del médico para ese día.
        public TimeSpan HoraInicioBloque { get; set; }

        // Hora de fin exacta del bloque de atención del médico para ese día.
        public TimeSpan HoraFinBloque { get; set; }
    }
}
