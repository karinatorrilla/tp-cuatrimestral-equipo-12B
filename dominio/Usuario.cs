using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public enum TipoUsuario
    {
        ADMIN = 1, //Administrador (PUEDE VER Y MANIPULAR TODO)
        RECEP = 2, //Recepcionista (ADMINISTRAR PACIENTES, MEDICOS Y TURNOS)
        MED = 3 //Medico (PUEDE VER SUS TURNOS ASOCIADOS Y MODIFICAR DEL PACIENTE LAS OBSERVACIONES DEL DIAGNOSTICO)
    }
    public class Usuario
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public TipoUsuario TipoUsuario { get; set; }

        public Usuario(string user, string password)
        {
            User = user;
            Password = password;
        }

    }
}
