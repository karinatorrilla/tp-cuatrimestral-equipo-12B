using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class TurnoTrabajoNegocio
    {
        public List<TurnoTrabajo> Listar()
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();


            lista.Add(new TurnoTrabajo
            {
                Id = 1,
                Descripcion = "Turno Mañana",
                HoraInicioBase = new TimeSpan(8, 0, 0),  // 08:00:00
                HoraFinBase = new TimeSpan(13, 0, 0)   // 13:00:00
            });

            lista.Add(new TurnoTrabajo
            {
                Id = 2,
                Descripcion = "Turno Tarde",
                HoraInicioBase = new TimeSpan(14, 0, 0), // 14:00:00
                HoraFinBase = new TimeSpan(19, 0, 0)   // 19:00:00
            });

            lista.Add(new TurnoTrabajo
            {
                Id = 4,
                Descripcion = "Jornada Completa",
                HoraInicioBase = new TimeSpan(9, 0, 0),  // 09:00:00
                HoraFinBase = new TimeSpan(18, 0, 0)   // 18:00:00
            });

            return lista;
        }
    }
}
