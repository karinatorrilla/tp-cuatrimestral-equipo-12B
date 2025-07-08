using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class EspecialidadNegocio
    {
        public List<Especialidad> Listar(bool incluirDeshabilitadas = false)
        {
            List<Especialidad> lista = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = incluirDeshabilitadas
            ? "SELECT ID, Descripcion, Habilitado FROM ESPECIALIDADES"
            : "SELECT ID, Descripcion, Habilitado FROM ESPECIALIDADES WHERE Habilitado = 1";
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Especialidad aux = new Especialidad();
                    aux.Id = (int)datos.Lector["ID"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Habilitado = Convert.ToInt32(datos.Lector["Habilitado"]);
                    lista.Add(aux);
                }

                return lista;
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

        public bool agregarEspecialidad(Especialidad nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO ESPECIALIDADES (Descripcion) VALUES(@Especialidad)");
                datos.setearParametro("@Especialidad", nuevo.Descripcion);
                datos.ejecutarAccion();

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error al agregar especialidad: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool modificarEspecialidad(Especialidad mod)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("UPDATE ESPECIALIDADES SET Descripcion = @Descripcion WHERE ID = @ID");
                datos.setearParametro("@Descripcion", mod.Descripcion);
                datos.setearParametro("@ID", mod.Id);
                datos.ejecutarAccion();

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error al modificar especialidad: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarEspecialidad(int idEspecialidad)
        {

            AccesoDatos datos = new AccesoDatos();

            try
            {

              /*Verificamos que al querer 'borrar' especialidad no este asociada a un medico */
                datos.setearConsulta("SELECT COUNT(*) FROM MEDICOxESPECIALIDAD WHERE EspecialidadId = @ID");
                datos.setearParametro("@ID", idEspecialidad);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    int cantidad = (int)datos.Lector[0];
                    if (cantidad > 0)
                    {
                        throw new Exception("No se puede eliminar la especialidad porque está asociada a uno o más médicos.");
                    }
                }
                                
                datos.cerrarConexion();
                datos.limpiarParametros();

                datos.setearConsulta("UPDATE ESPECIALIDADES SET Habilitado=0 WHERE ID = @ID");
                datos.setearParametro("@ID", idEspecialidad);
                datos.ejecutarAccion();
                
            }
            catch (Exception ex)
            {

              
                throw new Exception("Error al eliminar especialidad: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void habilitarEspecialidad(int idEspecialidad,string descripcion)
        {

            AccesoDatos datos = new AccesoDatos();

            try
            {
                /*Verificamos que no exista una especialidad igual */
                datos.setearConsulta("SELECT COUNT(*) FROM ESPECIALIDADES WHERE Descripcion = @descripcion AND Habilitado=1");
                datos.setearParametro("@descripcion", descripcion);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    int cantidad = (int)datos.Lector[0];
                    if (cantidad > 0)
                    {
                        throw new Exception("No se puede rehabilitar porque ya existe.");
                    }
                }

                datos.cerrarConexion();
                datos.limpiarParametros();


                datos.setearConsulta("UPDATE ESPECIALIDADES SET Habilitado=1 WHERE ID = @ID");
                datos.setearParametro("@ID", idEspecialidad);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {


                throw new Exception("Error al habilitar especialidad: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public bool agregarEspecialidadesMedico(int idmedico, List<int> especialidades)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //borramos las especialidades e ingresamos otra vez todo ///A OPTIMIZAR
                datos.setearConsulta("DELETE FROM MEDICOxESPECIALIDAD WHERE MedicoId = @MedicoId");
                datos.setearParametro("@MedicoId", idmedico);
                datos.ejecutarAccion();
                datos.limpiarParametros();
                datos.cerrarConexion();

                foreach (int idEspecialidad in especialidades)
                {
                    datos.setearConsulta("INSERT INTO MEDICOxESPECIALIDAD (MedicoId,EspecialidadId) VALUES(@MedicoId,@EspecialidadId)");

                    datos.setearParametro("@MedicoId", idmedico);
                    datos.setearParametro("@EspecialidadId", idEspecialidad);
                    datos.ejecutarAccion();
                    datos.limpiarParametros();
                    datos.cerrarConexion();

                }

                return true;
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

        public List<int> ListarIdsEspecialidadesPorMedico(int idmedico)
        {
            List<int> lista = new List<int>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT EspecialidadId FROM MEDICOxESPECIALIDAD WHERE MedicoId = @MedicoId");
                datos.setearParametro("@MedicoId", idmedico);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add((int)datos.Lector["EspecialidadId"]);
                }

                return lista;
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
        public List<Especialidad> ListaEspecialidadesAsignadas()
        {
            List<Especialidad> lista = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT DISTINCT E.ID, E.Descripcion FROM ESPECIALIDADES AS E INNER JOIN MEDICOxESPECIALIDAD AS ME ON E.ID = ME.EspecialidadId");
          
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Especialidad aux = new Especialidad();
                    aux.Id = (int)datos.Lector["ID"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    lista.Add(aux);
                }

                return lista;
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