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
    }
}
