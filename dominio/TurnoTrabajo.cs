using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class TurnoTrabajo
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } // Ej: "Mañana", "Tarde"
        public TimeSpan HoraInicioBase { get; set; } // Hora de inicio general del turno (ej. 08:00)
        public TimeSpan HoraFinBase { get; set; }  // Hora de fin general del turno (ej. 13:00)
    }
}
