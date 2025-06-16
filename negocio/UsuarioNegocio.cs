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
                datos.setearConsulta("select Id, TipoUser from USUARIOS where usuario = @user and Pass = @Password");
                datos.setearParametro("@user", usuario.User);
                datos.setearParametro("@Password", usuario.Password);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    usuario.Id = (int)datos.Lector["Id"];

                    usuario.TipoUsuario = (int)(datos.Lector["TipoUser"]) == 1 ? TipoUsuario.ADMIN : ((int)(datos.Lector["TipoUser"]) == 2 ? TipoUsuario.RECEP : TipoUsuario.MED);

                    return true;

                }

                return false;

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
