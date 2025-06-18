using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class PacienteNegocio
    {

        public List<Paciente> ListarPacientes(int id = 0)
        {


            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT Id, Nombre, Apellido, Documento, Email, Telefono, Nacionalidad, " +
                                  "ProvinciaId, LocalidadId, Calle, Altura, CodPostal, Depto, " +
                                  "FechaNacimiento, ObraSocialId, Observaciones, Habilitado " +
                                  "FROM PACIENTES WHERE Habilitado = 1 ";
                if (id > 0)
                {
                    consulta += " AND Id = " + id;
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Paciente aux = new Paciente();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Documento = (int)datos.Lector["Documento"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.Telefono = datos.Lector["Telefono"] is DBNull ? null : (string)datos.Lector["Telefono"];
                    aux.Nacionalidad = datos.Lector["Nacionalidad"] is DBNull ? null : (string)datos.Lector["Nacionalidad"];
                    aux.Calle = datos.Lector["Calle"] is DBNull ? null : (string)datos.Lector["Calle"];
                    aux.Altura = datos.Lector["Altura"] is DBNull ? 0 : (int)datos.Lector["Altura"]; 
                    aux.CodPostal = datos.Lector["CodPostal"] is DBNull ? null : (string)datos.Lector["CodPostal"];
                    aux.Depto = datos.Lector["Depto"] is DBNull ? null : (string)datos.Lector["Depto"];
                    aux.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    aux.ObraSocial = datos.Lector["ObraSocialId"] is DBNull ? 0 : (int)datos.Lector["ObraSocialId"]; 
                    aux.Observaciones = datos.Lector["Observaciones"] is DBNull ? null : (string)datos.Lector["Observaciones"];
                    aux.Provincia = datos.Lector["ProvinciaId"] is DBNull ? null : (string)datos.Lector["ProvinciaId"];
                    aux.Localidad = datos.Lector["LocalidadId"] is DBNull ? null : (string)datos.Lector["LocalidadId"];
                    aux.Habilitado = datos.Lector["Habilitado"] is DBNull ? 1 : ((bool)datos.Lector["Habilitado"] ? 1 : 0);


                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar pacientes: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public bool agregarPaciente(Paciente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO PACIENTES (Nombre, Apellido, Documento, Email, Telefono, Nacionalidad, " +
                                     "ProvinciaId, LocalidadId, Calle, Altura, CodPostal, Depto, FechaNacimiento, ObraSocialId, Observaciones, Habilitado) " +
                                     "VALUES (@Nombre, @Apellido, @Documento, @Email, @Telefono, @Nacionalidad, " +
                                     "@ProvinciaId, @LocalidadId, @Calle, @Altura, @CodPostal, @Depto, @FechaNacimiento, @ObraSocialId, @Observaciones, @Habilitado);");

                // Setear parámetros para cada campo
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Documento", nuevo.Documento);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@Telefono", nuevo.Telefono);
                datos.setearParametro("@Nacionalidad", nuevo.Nacionalidad);

                datos.setearParametro("@ProvinciaId", nuevo.Provincia);
                datos.setearParametro("@LocalidadId", nuevo.Localidad);
                datos.setearParametro("@Calle", nuevo.Calle);
                datos.setearParametro("@Altura", nuevo.Altura);
                datos.setearParametro("@CodPostal", nuevo.CodPostal);

                datos.setearParametro("@Depto", string.IsNullOrEmpty(nuevo.Depto) ? (object)DBNull.Value : nuevo.Depto);

                datos.setearParametro("@FechaNacimiento", nuevo.FechaNacimiento);
                datos.setearParametro("@ObraSocialId", nuevo.ObraSocial);
                datos.setearParametro("@Observaciones", string.IsNullOrEmpty(nuevo.Observaciones) ? (object)DBNull.Value : nuevo.Observaciones);

                datos.setearParametro("@Habilitado", nuevo.Habilitado);

                datos.ejecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificarPaciente(Paciente mod)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Update PACIENTES set Nombre = @nombre, Apellido = @apellido, " +
                    "Documento = @documento, Email = @email, Telefono = @telefono, " +
                    "Nacionalidad = @nacionalidad, ProvinciaId = @provinciaid, LocalidadId = @localidadid, " +
                    "Calle = @calle, Altura = @altura, CodPostal = @codpostal, Depto = @depto, " +
                    "FechaNacimiento = @fechanacimiento, ObraSocialId = @obrasocialid, " +
                    "Observaciones = @observaciones, Habilitado = @habilitado WHERE Id = @id");
                datos.setearParametro("@id", mod.Id);
                datos.setearParametro("@nombre", mod.Nombre);
                datos.setearParametro("@apellido", mod.Apellido);
                datos.setearParametro("@documento", mod.Documento);
                datos.setearParametro("@email", mod.Email);
                datos.setearParametro("@telefono", mod.Telefono);
                datos.setearParametro("@nacionalidad", mod.Nacionalidad);
                datos.setearParametro("@provinciaid", mod.Provincia);
                datos.setearParametro("@localidadid", mod.Localidad);
                datos.setearParametro("@calle", mod.Calle);
                datos.setearParametro("@altura", mod.Altura);
                datos.setearParametro("@codpostal", mod.CodPostal);
                datos.setearParametro("@depto", mod.Depto);
                datos.setearParametro("@fechanacimiento", mod.FechaNacimiento);
                datos.setearParametro("@obrasocialid", mod.ObraSocial);
                datos.setearParametro("@observaciones", mod.Observaciones);
                datos.setearParametro("@habilitado", mod.Habilitado);
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
        public void eliminarPaciente(int idPaciente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Update PACIENTES set Habilitado = 0 WHERE Id = @id");
                datos.setearParametro("@id", idPaciente);

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
