using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class TurnosNegocio
    {
        //lista las horas ocupadas del médico en la fecha seleccionada
        public List<int> listarHorasOcupadas(int idMedico, DateTime fecha)
        {
            List<int> listaHoras = new List<int>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Hora FROM TURNOS WHERE IdMedico = @idMedico AND Fecha = @fecha");
                datos.setearParametro("@idMedico", idMedico);
                datos.setearParametro("@fecha", fecha);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    listaHoras.Add(Convert.ToInt32(datos.Lector["Hora"]));
                }

                return listaHoras;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener horas ocupadas: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Lista disponibilidad de los medicos en la semana
        public List<DisponibilidadHoraria> ListarDisponibilidadMedico(int idMedico, int diaDeLaSemana)
        {
            List<DisponibilidadHoraria> listaBloques = new List<DisponibilidadHoraria>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, MedicoId, DiaDeLaSemana, HoraInicioBloque, HoraFinBloque FROM MEDICOxDISPONIBILIDADHORARIA WHERE MedicoId = @idMedico AND DiaDeLaSemana = @diaDeLaSemana AND Habilitado = 1");
                datos.setearParametro("@idMedico", idMedico);
                datos.setearParametro("@diaDeLaSemana", diaDeLaSemana);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    dominio.DisponibilidadHoraria bloque = new dominio.DisponibilidadHoraria();
                    bloque.Id = (int)datos.Lector["Id"];
                    bloque.MedicoId = (int)datos.Lector["MedicoId"];
                    bloque.DiaDeLaSemana = (int)datos.Lector["DiaDeLaSemana"];
                    bloque.HoraInicioBloque = (TimeSpan)datos.Lector["HoraInicioBloque"];
                    bloque.HoraFinBloque = (TimeSpan)datos.Lector["HoraFinBloque"];
                    listaBloques.Add(bloque);
                }

                return listaBloques;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar disponibilidad del médico: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
