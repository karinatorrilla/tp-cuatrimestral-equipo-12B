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
                datos.setearConsulta("select Id, TipoUser,IDMedico from USUARIOS where usuario = @user and Pass = @Password");
                datos.setearParametro("@user", usuario.User);
                datos.setearParametro("@Password", usuario.Password);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    usuario.Id = (int)datos.Lector["Id"];

                    usuario.TipoUsuario = (int)(datos.Lector["TipoUser"]) == 1 ? TipoUsuario.ADMIN : ((int)(datos.Lector["TipoUser"]) == 2 ? TipoUsuario.RECEP : TipoUsuario.MED);

                    //guardamos el idmedico si es med sino le asignamos 0
                    usuario.IDMedico = (usuario.TipoUsuario == TipoUsuario.MED && datos.Lector["IDMedico"] != DBNull.Value)
       ? (int)datos.Lector["IDMedico"]
       : 0;
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

        public bool agregarUsuario(string usuario,string password,int idmedico)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {                
                string consulta = "insert into USUARIOS(Usuario,Pass,TipoUser,IDMedico)values(@Usuario,@Password,3,@IdMedico)";

                datos.setearConsulta(consulta);

                // Asignar parámetros para todos los campos
                datos.setearParametro("@Usuario", usuario);
                datos.setearParametro("@Password", password);
                datos.setearParametro("@IdMedico", idmedico);

                datos.ejecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error al agregar usuario: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }



        }
    }
}
