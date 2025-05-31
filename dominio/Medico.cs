using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Medico : Paciente    //hereda propiedades de paciente para cargar datos
    {
       public TurnoTrabajo TurnoTrabajo { get; set; }   //cada médico está asociado a un turno de trabajo
       public List<Especialidad> Especialidades { get; set; }  //cada médico tiene varias especialidades
       public List<Paciente> Pacientes { get; set; }  //cada médico tiene varios pacientes
    }
}
