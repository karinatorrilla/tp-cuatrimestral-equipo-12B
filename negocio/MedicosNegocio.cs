using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class MedicosNegocio
    {
        public List<Medico> ListarMedicos(int id = 0, DateTime? fechaTurno = null)
        {

            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();


            try
            {
                string consulta = "SELECT DISTINCT M.Id, M.Matricula, M.Nombre, M.Apellido, M.Documento, M.Email, M.Telefono, M.Nacionalidad, " +
                                  "M.Provincia, M.Localidad, M.Calle, M.Altura, M.CodPostal, M.Depto, " +
                                  "M.FechaNacimiento, M.Habilitado " +
                                  "FROM MEDICOS M ";

                if (fechaTurno.HasValue) //si se pasa parámetro de fecha, agregar INNER JOIN con tabla TURNOS
                {
                    consulta += "INNER JOIN TURNOS T ON T.IdMedico = M.Id ";
                }

                consulta += "WHERE M.Habilitado = 1 ";

                if (id > 0) //si se pasa parámetro de Id, agregarlo a la consulta
                {
                    consulta += "AND M.Id = " + id + " ";
                }

                if (fechaTurno.HasValue) //si se pasa parámetro de fecha, agregar condición 
                {
                    consulta += "AND T.Fecha = @Fecha ";
                }

                datos.setearConsulta(consulta);

                if (fechaTurno.HasValue)
                {
                    datos.setearParametro("@Fecha", fechaTurno.Value.Date);
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = new Medico();

                    // Carg tabla MEDICOS
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Documento = datos.Lector["Documento"] is DBNull ? 0 : (int)datos.Lector["Documento"];
                    aux.Email = datos.Lector["Email"] is DBNull ? null : (string)datos.Lector["Email"];
                    aux.Telefono = datos.Lector["Telefono"] is DBNull ? null : (string)datos.Lector["Telefono"];
                    aux.Nacionalidad = datos.Lector["Nacionalidad"] is DBNull ? null : (string)datos.Lector["Nacionalidad"];
                    aux.Provincia = datos.Lector["Provincia"] is DBNull ? null : (string)datos.Lector["Provincia"];
                    aux.Localidad = datos.Lector["Localidad"] is DBNull ? null : (string)datos.Lector["Localidad"];
                    aux.Calle = datos.Lector["Calle"] is DBNull ? null : (string)datos.Lector["Calle"];
                    aux.Altura = datos.Lector["Altura"] is DBNull ? 0 : (int)datos.Lector["Altura"];
                    aux.CodPostal = datos.Lector["CodPostal"] is DBNull ? null : (string)datos.Lector["CodPostal"];
                    aux.Depto = datos.Lector["Depto"] is DBNull ? null : (string)datos.Lector["Depto"];
                    aux.FechaNacimiento = datos.Lector["FechaNacimiento"] is DBNull ? DateTime.MinValue : (DateTime)datos.Lector["FechaNacimiento"];
                    aux.Matricula = (int)datos.Lector["Matricula"];
                    aux.Habilitado = datos.Lector["Habilitado"] is DBNull ? 1 : ((bool)datos.Lector["Habilitado"] ? 1 : 0);


                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar médicos: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public List<Medico> listarMedicosPorEspecialidad(int idEspecialidad)
        {
            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT M.Id, M.Nombre, M.Apellido FROM MEDICOS M " +
                    "INNER JOIN MEDICOxESPECIALIDAD ME ON M.Id = ME.MedicoId WHERE ME.EspecialidadId = @idEspecialidad " +
                    "AND M.Habilitado = 1 AND ME.Habilitado = 1");

                datos.setearParametro("@idEspecialidad", idEspecialidad);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = new Medico();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = datos.Lector["Nombre"].ToString();
                    aux.Apellido = datos.Lector["Apellido"].ToString();

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar médicos por especialidad: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public bool agregarMedico(Medico nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Actualiza la consulta INSERT para incluir todos los campos
                string consulta = "INSERT INTO MEDICOS (Matricula, Nombre, Apellido, Documento, Email, Telefono, Nacionalidad, " +
                                  "Provincia, Localidad, Calle, Altura, CodPostal, Depto, FechaNacimiento, Habilitado) " +
                                  "VALUES (@Matricula, @Nombre, @Apellido, @Documento, @Email, @Telefono, @Nacionalidad, " +
                                  "@Provincia, @Localidad, @Calle, @Altura, @CodPostal, @Depto, @FechaNacimiento, " +
                                  "@Habilitado)";

                datos.setearConsulta(consulta);

                // Asignar parámetros para todos los campos
                datos.setearParametro("@Matricula", nuevo.Matricula);
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Documento", nuevo.Documento);
                datos.setearParametro("@Email", (object)nuevo.Email ?? DBNull.Value);
                datos.setearParametro("@Telefono", (object)nuevo.Telefono ?? DBNull.Value);
                datos.setearParametro("@Nacionalidad", (object)nuevo.Nacionalidad ?? DBNull.Value);
                datos.setearParametro("@Provincia", (object)nuevo.Provincia ?? DBNull.Value);
                datos.setearParametro("@Localidad", (object)nuevo.Localidad ?? DBNull.Value);
                datos.setearParametro("@Calle", (object)nuevo.Calle ?? DBNull.Value);
                datos.setearParametro("@Altura", (object)nuevo.Altura ?? DBNull.Value);
                datos.setearParametro("@CodPostal", (object)nuevo.CodPostal ?? DBNull.Value);
                datos.setearParametro("@Depto", (object)nuevo.Depto ?? DBNull.Value);
                datos.setearParametro("@FechaNacimiento", (object)nuevo.FechaNacimiento ?? DBNull.Value);
                datos.setearParametro("@Habilitado", nuevo.Habilitado);

                datos.ejecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error al agregar médico: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificarMedico(Medico mod)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Update MEDICOS set Nombre = @nombre, Apellido = @apellido, Matricula = @matricula, Documento=@documento, Email=@email, Telefono=@telefono,Nacionalidad=@nacionalidad, Provincia=@provincia,Localidad=@localidad,Calle=@calle,Altura=@altura,Depto=@depto,FechaNacimiento=@fechanacimiento WHERE Id = @id");
                datos.setearParametro("@id", mod.Id);
                datos.setearParametro("@matricula", mod.Matricula);
                datos.setearParametro("@nombre", mod.Nombre);
                datos.setearParametro("@apellido", mod.Apellido);
                datos.setearParametro("@documento", mod.Documento);
                datos.setearParametro("@email", mod.Email);
                datos.setearParametro("@telefono", mod.Telefono);
                datos.setearParametro("@nacionalidad", mod.Nacionalidad);
                datos.setearParametro("@provincia", mod.Provincia);
                datos.setearParametro("@localidad", mod.Localidad);
                datos.setearParametro("@calle", mod.Calle);
                datos.setearParametro("@altura", mod.Altura);
                datos.setearParametro("@depto", mod.Depto);
                datos.setearParametro("@fechanacimiento", mod.FechaNacimiento);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }
        public void eliminarMedico(int idMedico)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Update MEDICOS set Habilitado = 0 WHERE Id = @id"); //to-do HABILITADO en BD
                datos.setearParametro("@id", idMedico);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public List<Especialidad> ListarEspecialidadesPorMedico(int idMedico)
        {
            List<Especialidad> listaEspecialidades = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Consulta para unir MEDICOxESPECIALIDAD con ESPECIALIDADES
                // para obtener los detalles de la especialidad
                string consulta = "SELECT E.Id, E.Descripcion " +
                                  "FROM ESPECIALIDADES E " +
                                  "INNER JOIN MEDICOxESPECIALIDAD ME ON E.Id = ME.EspecialidadId " +
                                  "WHERE ME.MedicoId = @idMedico AND ME.Habilitado = 1"; // Filtra solo las especialidades habilitadas

                datos.setearConsulta(consulta);
                datos.setearParametro("@idMedico", idMedico);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Especialidad especialidad = new Especialidad();
                    especialidad.Id = (int)datos.Lector["Id"];
                    especialidad.Descripcion = (string)datos.Lector["Descripcion"];

                    listaEspecialidades.Add(especialidad);
                }

                return listaEspecialidades;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar especialidades por médico: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void AgregarEspecialidadPorMedico(int medicoId, List<int> nuevasEspecialidadesIds)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Medico buscarCorreoUsuario(string datoAbuscar)
        {
            AccesoDatos datos = new AccesoDatos();
            Medico medico = new Medico();
            try
            {
                string consulta = "select U.IdMedico, M.Nombre,M.Apellido,M.Email from USUARIOS as U inner join MEDICOS AS M ON U.IDMedico=M.ID where (M.Email = @Dato OR U.Usuario=@Dato) AND M.Habilitado = 1";

                datos.setearConsulta(consulta);
                datos.setearParametro("@Dato", datoAbuscar);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    medico.Id = (int)datos.Lector["IdMedico"];
                    medico.Nombre = (string)datos.Lector["Nombre"];
                    medico.Apellido = (string)datos.Lector["Apellido"];
                    medico.Email=(string)datos.Lector["Email"];

                }
                return medico;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


    }
}

