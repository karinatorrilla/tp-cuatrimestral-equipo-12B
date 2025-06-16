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

            try
            {
                string consulta = "SELECT M.IDMedico, M.Nombre, M.Apellido, M.IDEspecialidad, M.Matricula, E.Nombre " +
                    "AS NombreEspecialidad FROM MEDICOS M " +
                    "INNER JOIN ESPECIALIDADES E ON M.IDEspecialidad = E.IDEspecialidad ";
                if (id > 0)
                {
                    consulta += "WHERE M.IDMedico = " + id;
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Medico aux = new Medico();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    //aux.Documento = (int)datos.Lector["Documento"]; to-do en bd
                    //aux.Email = (string)datos.Lector["Email"]; to-do en bd
                    //aux.Telefono = (int)datos.Lector["Telefono"]; to-do en bd
                    //aux.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"]; to-do en bd
                    //aux.Calle = (string)datos.Lector["Calle"]; to-do en bd
                    aux.EspecialidadSeleccionada = new Especialidad();
                    aux.EspecialidadSeleccionada.Id = (int)datos.Lector["IDEspecialidad"];
                    aux.EspecialidadSeleccionada.Descripcion = (string)datos.Lector["NombreEspecialidad"];
                    aux.Matricula = (int)datos.Lector["Matricula"];

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
                datos.setearConsulta("INSERT INTO MEDICOS (Nombre, Apellido, IDEspecialidad, Matricula)" +
                 "VALUES (@Nombre, @Apellido, @IDEspecialidad, @Matricula);");

                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                //datos.setearParametro("@Documento", nuevo.Documento); to-do en bd
                //datos.setearParametro("@Email", nuevo.Email); to-do en bd
                //datos.setearParametro("@Telefono", nuevo.Telefono); to-do en bd
                //datos.setearParametro("@FechaNacimiento", nuevo.FechaNacimiento); to-do en bd
                //datos.setearParametro("@Calle", nuevo.Calle); to-do en bd
                datos.setearParametro("@IDEspecialidad", nuevo.EspecialidadSeleccionada.Id);
                datos.setearParametro("@Matricula", nuevo.Matricula);

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
