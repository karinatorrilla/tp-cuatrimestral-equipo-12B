using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class UsuarioNegocio
    {
        public bool Loguear(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
               // datos.setearConsulta("Select U.IDUsuario from USUARIOS AS U where U.Usuario=@Nombre and U.Pass=@Password");
                datos.setearConsulta("select U.IDUsuario,TU.Nombre from USUARIOS AS U INNER JOIN TIPOUSUARIOS AS TU ON U.IdTipoUsuario=TU.IdTipoUsuario where U.Usuario=@Nombre and Pass=@Password");
                datos.setearParametro("@Nombre", usuario.Nombre);
                datos.setearParametro("@Password",usuario.Password);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    usuario.Id = (int)datos.Lector["IDUsuario"];

                    usuario.Tipo = new TipoUsuario();
                    
                    usuario.Tipo.NombreTipo = (string)datos.Lector["Nombre"];
                                        

                   




                    return true;

                }

                return false;

            }

            catch (Exception ex)
            {


                throw ex;

            }
            finally { datos.cerrarConexion(); }

        }
    }
}
