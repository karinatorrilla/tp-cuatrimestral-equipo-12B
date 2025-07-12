using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using dominio;

namespace negocio
{
    public class DisponibilidadHorariaNegocio
    {
        // Método para agregar una nueva disponibilidad horaria
        public bool AgregarDisponibilidadHoraria(DisponibilidadHoraria nuevaDisponibilidad)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO MEDICOxDISPONIBILIDADHORARIA (MedicoId, DiaDeLaSemana, HoraInicioBloque, HoraFinBloque, Habilitado) VALUES (@MedicoId, @DiaDeLaSemana, @HoraInicioBloque, @HoraFinBloque, 1)");
                datos.setearParametro("@MedicoId", nuevaDisponibilidad.MedicoId);
                datos.setearParametro("@DiaDeLaSemana", nuevaDisponibilidad.DiaDeLaSemana);
                datos.setearParametro("@HoraInicioBloque", nuevaDisponibilidad.HoraInicioBloque);
                datos.setearParametro("@HoraFinBloque", nuevaDisponibilidad.HoraFinBloque);

                datos.ejecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar disponibilidad horaria: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Método para agregar una nueva disponibilidad horaria
        public bool ModificarDisponibilidadHoraria(DisponibilidadHoraria disponibilidadAEditar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE MEDICOxDISPONIBILIDADHORARIA SET " +
                                     "DiaDeLaSemana = @DiaDeLaSemana, " +
                                     "HoraInicioBloque = @HoraInicioBloque, " +
                                     "HoraFinBloque = @HoraFinBloque " +
                                     "WHERE Id = @Id");

                datos.setearParametro("@DiaDeLaSemana", disponibilidadAEditar.DiaDeLaSemana);
                datos.setearParametro("@HoraInicioBloque", disponibilidadAEditar.HoraInicioBloque);
                datos.setearParametro("@HoraFinBloque", disponibilidadAEditar.HoraFinBloque);
                datos.setearParametro("@Id", disponibilidadAEditar.Id);

                datos.ejecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar disponibilidad horaria: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Método para listar disponibilidades horarias de un médico 
        public List<DisponibilidadHoraria> ListarPorMedico(int medicoId)
        {
            List<DisponibilidadHoraria> lista = new List<DisponibilidadHoraria>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id, MedicoId, DiaDeLaSemana, HoraInicioBloque, HoraFinBloque FROM MEDICOxDISPONIBILIDADHORARIA WHERE MedicoId = @MedicoId AND Habilitado = 1 ORDER BY DiaDeLaSemana, HoraInicioBloque");
                datos.setearParametro("@MedicoId", medicoId);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    DisponibilidadHoraria aux = new DisponibilidadHoraria();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.MedicoId = (int)datos.Lector["MedicoId"];
                    aux.DiaDeLaSemana = (int)datos.Lector["DiaDeLaSemana"];
                    aux.HoraInicioBloque = datos.Lector.GetTimeSpan(datos.Lector.GetOrdinal("HoraInicioBloque"));
                    aux.HoraFinBloque = datos.Lector.GetTimeSpan(datos.Lector.GetOrdinal("HoraFinBloque"));
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar disponibilidades horarias: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Método para verificar si existe disponibilidad horaria que se solape con otra del mismo medico
        public bool ExisteSolapamiento(int medicoId, int diaSemana, TimeSpan horaInicio, TimeSpan horaFin, int? idAExcluir = null)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT COUNT(*) FROM MEDICOxDISPONIBILIDADHORARIA " +
                                  "WHERE MedicoId = @MedicoId AND DiaDeLaSemana = @DiaDeLaSemana " +
                                  "AND Habilitado = 1 " +
                                  "AND NOT (HoraFinBloque <= @HoraInicio OR HoraInicioBloque >= @HoraFin)";

                if (idAExcluir.HasValue)
                {
                    consulta += " AND Id <> @IdAExcluir";
                }

                datos.setearConsulta(consulta);
                datos.setearParametro("@MedicoId", medicoId);
                datos.setearParametro("@DiaDeLaSemana", diaSemana);
                datos.setearParametro("@HoraInicio", horaInicio);
                datos.setearParametro("@HoraFin", horaFin);

                if (idAExcluir.HasValue)
                {
                    datos.setearParametro("@IdAExcluir", idAExcluir.Value);
                }

                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    return (int)datos.Lector[0] > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar solapamiento: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Método para eliminar disponibilidad horaria
        public bool EliminarDisponibilidadHoraria(int idDisponibilidad)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE MEDICOxDISPONIBILIDADHORARIA SET Habilitado = 0 WHERE Id = @Id");
                datos.setearParametro("@Id", idDisponibilidad);
                datos.ejecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar lógicamente la disponibilidad horaria: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
