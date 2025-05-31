﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public enum EstadoTurno
    {
        Nuevo = 1,
        Reprogramado = 2,
        Cancelado = 3,
        Inasistencia = 4,
        Cerrado = 5
    }
    public class Turno
    {
        public int Id { get; set; }
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }
        public Especialidad Especialidad { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Observaciones { get; set; }
        public EstadoTurno Estado { get; set; }
    }
}
