using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class TokenRecuperacion
    {
        public int ID { get; set; }
        public int IDMedico { get; set; }
        public string Token { get; set; }
        public DateTime FechaGenerado { get; set; }
        public bool Usado { get; set; }
    }

}
