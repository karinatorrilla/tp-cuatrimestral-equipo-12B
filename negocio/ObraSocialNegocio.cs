using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ObraSocialNegocio
    {

        public List<ObraSocial> Listar(bool incluirDeshabilitadas = false)
        {
            List<ObraSocial> lista = new List<ObraSocial>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = incluirDeshabilitadas
          ? "SELECT ID, Descripcion, Habilitado FROM OBRASOCIAL ORDER BY Descripcion ASC"
          : "SELECT ID, Descripcion, Habilitado FROM OBRASOCIAL WHERE Habilitado = 1 ORDER BY Descripcion ASC";
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    ObraSocial aux = new ObraSocial();
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
        public void habilitarObraSocial(int idObra, string descripcion)
        {

            AccesoDatos datos = new AccesoDatos();

            try
            {
                /*Verificamos que no exista obrasocial */
                datos.setearConsulta("SELECT COUNT(*) FROM OBRASOCIAL WHERE Descripcion = @Descripcion AND Habilitado =1");

                datos.setearParametro("@Descripcion", descripcion);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    int cantidad = (int)datos.Lector[0];
                    if (cantidad > 0)
                    {
                        throw new Exception("No se puede rehabilitar porque ya existe");
                    }
                }

                datos.cerrarConexion();
                datos.limpiarParametros();

                datos.setearConsulta("UPDATE OBRASOCIAL SET Habilitado=1 WHERE ID = @ID");
                datos.setearParametro("@ID", idObra);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {


                throw new Exception("Error al habilitar Obra Social: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public bool agregarObraSocial(ObraSocial nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("INSERT INTO OBRASOCIAL (Descripcion) VALUES(@ObraSocial)");

                datos.setearParametro("@ObraSocial", nuevo.Descripcion);

                datos.ejecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error al agregar Obra Social: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool modificarObraSocial(ObraSocial mod)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE OBRASOCIAL SET Descripcion = @Descripcion WHERE ID = @Id");
                datos.setearParametro("@Descripcion", mod.Descripcion);
                datos.setearParametro("@Id", mod.Id);
                datos.ejecutarAccion();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarObra(int idObra)   ////////////////////VALIDAMOS ACA???
        {

            AccesoDatos datos = new AccesoDatos();

            try
            {
                /*Verificamos que al querer 'borrar' obra social no este asociada a un paciente */
                datos.setearConsulta("SELECT COUNT(*) FROM MEDICOxESPECIALIDAD WHERE EspecialidadId = @ID");
                datos.setearConsulta("select COUNT(*) FROM PACIENTES WHERE ObraSocialId=@ID");
                datos.setearParametro("@ID", idObra);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    int cantidad = (int)datos.Lector[0];
                    if (cantidad > 0)
                    {
                        throw new Exception("No se puede eliminar la obra social porque está asociada a uno o más pacientes.");
                    }
                }

                datos.cerrarConexion();
                datos.limpiarParametros();

                datos.setearConsulta("update OBRASOCIAL set Habilitado=0 where ID=@id");
                datos.setearParametro("@id", idObra);

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
