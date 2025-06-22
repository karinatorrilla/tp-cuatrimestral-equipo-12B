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

              public List<ObraSocial> Listar()
        {
            List<ObraSocial> lista = new List<ObraSocial>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select ID,Descripcion from OBRASOCIAL where Habilitado=1 ORDER BY Descripcion ASC");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    ObraSocial aux = new ObraSocial();
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
