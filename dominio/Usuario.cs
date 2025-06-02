using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    //public enum TipoUsuario
    //{
    //    Administrador = 1,
    //    Recepcionista = 2,
    //    Medico = 3
    //}
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public TipoUsuario Tipo { get; set; }


        public Usuario(string nombre, string password)
        {
            Nombre = nombre;
            Password = password;
        }

    }
}
