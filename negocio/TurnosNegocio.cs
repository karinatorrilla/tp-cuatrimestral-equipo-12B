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

        public int AgregarTurno(Turno nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO TURNOS (IdMedico, IdPaciente, IdEspecialidad, Fecha, Hora, Observaciones, Estado) " +
                     "VALUES (@idMedico, @idPaciente, @idEspecialidad, @fecha, @hora, @observaciones, @estado); " +
                     "SELECT SCOPE_IDENTITY();");

                datos.setearParametro("@idMedico", nuevo.Medico.Id);
                datos.setearParametro("@idPaciente", nuevo.Paciente.Id);
                datos.setearParametro("@idEspecialidad", nuevo.Especialidad.Id);
                datos.setearParametro("@fecha", nuevo.Fecha.Date);
                datos.setearParametro("@hora", nuevo.Hora);
                datos.setearParametro("@observaciones", string.IsNullOrEmpty(nuevo.Observaciones) ? DBNull.Value : (object)nuevo.Observaciones);
                datos.setearParametro("@estado", (int)nuevo.Estado);

               return datos.ejecutarAccionScalar();
               
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el turno: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void ReprogramarTurno(Turno actualizado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE TURNOS SET IdMedico = @idMedico, IdEspecialidad = @idEspecialidad, Fecha = @fecha, Hora = @hora, Observaciones = @observaciones, Estado = @estado WHERE Id = @idTurno");
                datos.setearParametro("@idMedico", actualizado.Medico.Id);
                datos.setearParametro("@idEspecialidad", actualizado.Especialidad.Id);
                datos.setearParametro("@fecha", actualizado.Fecha.Date);
                datos.setearParametro("@hora", actualizado.Hora);
                datos.setearParametro("@observaciones", string.IsNullOrEmpty(actualizado.Observaciones) ? DBNull.Value : (object)actualizado.Observaciones);
                datos.setearParametro("@estado", (int)actualizado.Estado);
                datos.setearParametro("@idTurno", actualizado.Id);
                
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reprogramar el turno: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void CambiarEstadoTurno(int idTurno, EstadoTurno nuevoEstado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE TURNOS SET Estado = @estado WHERE Id = @idTurno");
                datos.setearParametro("@estado", (int)nuevoEstado);
                datos.setearParametro("@idTurno", idTurno);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar estado del turno: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Turno> ListarTurnos(int idMedico = 0, DateTime? fecha = null)
        {
            List<Turno> lista = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT T.Id, T.IdMedico, T.IdPaciente, T.IdEspecialidad, T.Fecha, T.Hora, T.Observaciones, T.Estado, " +
                                  "P.Nombre AS NombrePaciente, P.Apellido AS ApellidoPaciente, P.Documento,P.Email,P.FechaNacimiento, OS.Descripcion AS DescripcionObraSocial, " +
                                  "E.Descripcion AS DescripcionEspecialidad, " +
                                  "M.Nombre AS NombreMedico, M.Apellido AS ApellidoMedico " +
                                  "FROM TURNOS T " +
                                  "INNER JOIN PACIENTES P ON P.Id = T.IdPaciente " +
                                  "INNER JOIN OBRASOCIAL OS ON OS.Id = P.ObraSocialId " +
                                  "INNER JOIN ESPECIALIDADES E ON E.Id = T.IdEspecialidad " +
                                  "INNER JOIN MEDICOS M ON M.Id = T.IdMedico " +
                                  "WHERE 1=1 "; //se pone 1=1 para poder realizar consultas dinámicamente
                                  
                if (idMedico > 0)
                {
                    consulta += "AND T.IdMedico = @IdMedico ";
                }

                if (fecha.HasValue)
                {
                    consulta += "AND T.Fecha = @Fecha ";
                }

                consulta += "ORDER BY T.Fecha";
                datos.setearConsulta(consulta);

                if (idMedico > 0)
                {
                    datos.setearParametro("@IdMedico", idMedico);
                }

                if (fecha.HasValue)
                {
                    datos.setearParametro("@Fecha", fecha.Value.Date);
                }

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Turno aux = new Turno();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Hora = Convert.ToInt32(datos.Lector["Hora"]);
                    aux.Observaciones = datos.Lector["Observaciones"] is DBNull ? null : datos.Lector["Observaciones"].ToString();
                    aux.Estado = (EstadoTurno)(int)datos.Lector["Estado"];
                    aux.Paciente = new Paciente
                    {
                        Id = (int)datos.Lector["IdPaciente"],
                        Nombre = datos.Lector["NombrePaciente"].ToString(),
                        Apellido = datos.Lector["ApellidoPaciente"].ToString(),
                        Documento = (int)datos.Lector["Documento"],
                        FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"],
                        Email = datos.Lector["Email"].ToString(),
                        DescripcionObraSocial = datos.Lector["DescripcionObraSocial"].ToString()
                    }; 
                    aux.Especialidad = new Especialidad
                    {
                        Id = (int)datos.Lector["IdEspecialidad"],
                        Descripcion = datos.Lector["DescripcionEspecialidad"].ToString()
                    };
                    aux.Medico = new Medico
                    {
                        Id = (int)datos.Lector["IdMedico"],
                        Nombre = datos.Lector["NombreMedico"].ToString(),
                        Apellido = datos.Lector["ApellidoMedico"].ToString()
                    };

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar turnos por médico: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
