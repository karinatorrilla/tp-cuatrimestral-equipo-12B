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
        public List<Medico> ListarMedicos(int id = 0)
        {

            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();

            EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
            List<Especialidad> todasLasEspecialidades = especialidadNegocio.Listar();

            try
            {
                string consulta = "SELECT Id, Matricula, Nombre, Apellido, Documento, Email, Telefono, Nacionalidad, " +
                                  "Provincia, Localidad, Calle, Altura, CodPostal, Depto, " +
                                  "FechaNacimiento, EspecialidadesIDs, IDTurnoTrabajo, DiasDisponiblesIDs, " +
                                  "HoraInicioBloque, HoraFinBloque, Habilitado " +
                                  "FROM MEDICOS WHERE Habilitado = 1 ";

                if (id > 0)
                {
                    consulta += " AND Id = " + id;
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
                    aux.Provincia = datos.Lector["ProvinciaId"] is DBNull ? null : (string)datos.Lector["ProvinciaId"];
                    aux.Localidad = datos.Lector["LocalidadId"] is DBNull ? null : (string)datos.Lector["LocalidadId"];
                    aux.Calle = datos.Lector["Calle"] is DBNull ? null : (string)datos.Lector["Calle"];
                    aux.Altura = datos.Lector["Altura"] is DBNull ? 0 : (int)datos.Lector["Altura"];
                    aux.CodPostal = datos.Lector["CodPostal"] is DBNull ? null : (string)datos.Lector["CodPostal"];
                    aux.Depto = datos.Lector["Depto"] is DBNull ? null : (string)datos.Lector["Depto"];
                    aux.FechaNacimiento = datos.Lector["FechaNacimiento"] is DBNull ? DateTime.MinValue : (DateTime)datos.Lector["FechaNacimiento"];
                    aux.Matricula = (int)datos.Lector["Matricula"];
                    aux.Habilitado = datos.Lector["Habilitado"] is DBNull ? 1 : ((bool)datos.Lector["Habilitado"] ? 1 : 0);

                    //procesar EspecialidadesIDs y obtener los nombres
                    aux.EspecialidadesIDs = datos.Lector["EspecialidadesIDs"] is DBNull ? null : (string)datos.Lector["EspecialidadesIDs"];
                    aux.IDTurnoTrabajo = (int)datos.Lector["IDTurnoTrabajo"];
                    aux.DiasDisponiblesIDs = datos.Lector["DiasDisponiblesIDs"] is DBNull ? null : (string)datos.Lector["DiasDisponiblesIDs"];
                    aux.HoraInicioBloque = datos.Lector["HoraInicioBloque"] is DBNull ? (TimeSpan?)null : (TimeSpan)datos.Lector["HoraInicioBloque"];
                    aux.HoraFinBloque = datos.Lector["HoraFinBloque"] is DBNull ? (TimeSpan?)null : (TimeSpan)datos.Lector["HoraFinBloque"];

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

        public bool agregarMedico(Medico nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Actualiza la consulta INSERT para incluir todos los campos
                string consulta = "INSERT INTO MEDICOS (Matricula, Nombre, Apellido, Documento, Email, Telefono, Nacionalidad, " +
                                  "Provincia, Localidad, Calle, Altura, CodPostal, Depto, FechaNacimiento, " +
                                  "EspecialidadesIDs, IDTurnoTrabajo, DiasDisponiblesIDs, HoraInicioBloque, HoraFinBloque, Habilitado) " +
                                  "VALUES (@Matricula, @Nombre, @Apellido, @Documento, @Email, @Telefono, @Nacionalidad, " +
                                  "@Provincia, @Localidad, @Calle, @Altura, @CodPostal, @Depto, @FechaNacimiento, " +
                                  "@EspecialidadesIDs, @IDTurnoTrabajo, @DiasDisponiblesIDs, @HoraInicioBloque, @HoraFinBloque, @Habilitado)";

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

                // Parámetros para los selectores múltiples y la disponibilidad
                datos.setearParametro("@EspecialidadesIDs", (object)nuevo.EspecialidadesIDs ?? DBNull.Value); // string con IDs separados por comas
                datos.setearParametro("@IDTurnoTrabajo", (object)nuevo.IDTurnoTrabajo ?? DBNull.Value);
                datos.setearParametro("@DiasDisponiblesIDs", (object)nuevo.DiasDisponiblesIDs ?? DBNull.Value); // string con IDs separados por comas
                datos.setearParametro("@HoraInicioBloque", (object)nuevo.HoraInicioBloque ?? DBNull.Value); // TimeSpan 
                datos.setearParametro("@HoraFinBloque", (object)nuevo.HoraFinBloque ?? DBNull.Value);     // TimeSpan
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
                datos.setearConsulta("Update MEDICOS set Nombre = @nombre, Apellido = @apellido, IDEspecialidad = @idespecialidad, Matricula = @matricula WHERE IDMedico = @id");
                datos.setearParametro("@id", mod.Id);
                datos.setearParametro("@nombre", mod.Nombre);
                datos.setearParametro("@apellido", mod.Apellido);
                //datos.setearParametro("@Documento", mod.Documento); to-do en bd
                //datos.setearParametro("@Email", mod.Email); to-do en bd
                //datos.setearParametro("@Telefono", mod.Telefono); to-do en bd
                //datos.setearParametro("@FechaNacimiento", mod.FechaNacimiento); to-do en bd
                //datos.setearParametro("@Calle", mod.Calle); to-do en bd
                datos.setearParametro("@idespecialidad", mod.EspecialidadSeleccionada.Id);
                datos.setearParametro("@matricula", mod.Matricula);
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

    }
}
