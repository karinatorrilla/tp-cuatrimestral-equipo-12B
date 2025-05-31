using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public enum TipoUsuario
    {
        Administrador = 1,
        Recepcionista = 2,
        Medico = 3
    }
    public class Usuario
    {
        public int Id { get; set; }
        public int Nombre { get; set; }
        public int Password { get; set; }
        public TipoUsuario Tipo { get; set; }
    }
}
