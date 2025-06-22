using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class EspecialidadNegocio
    {
        public List<Especialidad> Listar()
        {
            List<Especialidad> lista = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select ID, Descripcion from ESPECIALIDADES where Habilitado = 1");
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
                datos.setearConsulta("UPDATE ESPECIALIDADES SET Habilitado=0 WHERE ID = @ID");
                datos.setearParametro("@ID", idEspecialidad);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
                throw new Exception("Error al eliminar especialidad: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}