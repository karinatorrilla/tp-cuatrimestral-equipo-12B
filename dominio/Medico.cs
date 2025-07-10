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
        public List<DisponibilidadHoraria> Disponibilidades { get; set; }              
        public List<Especialidad> Especialidades { get; set; }  //cada médico tiene varias especialidades          
    }
}
