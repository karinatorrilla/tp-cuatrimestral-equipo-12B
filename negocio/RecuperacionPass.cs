using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class RecuperacionPass
    {
        public void guardarTokenRecuperacion(int idMedico, string token)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "INSERT INTO TokensRecuperacion (idMedico, Token) VALUES (@IDMedico, @Token)";
                datos.setearConsulta(consulta);
                datos.setearParametro("@IDMedico", idMedico);
                datos.setearParametro("@Token", token);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el token de recuperación", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public int verificarToken(string token)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT IDMedico FROM TokensRecuperacion WHERE Token = @Token AND Usado = 0 AND FechaGenerado >= DATEADD(MINUTE, -30, GETDATE())";
                datos.setearConsulta(consulta);

                datos.setearParametro("@Token", token);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    return (int)datos.Lector["IDMedico"];
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void cambiarContraseña(string contraseña, int idMedico)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "UPDATE USUARIOS SET Pass = @contraseña WHERE IDMedico = @idmedico";
                datos.setearConsulta(consulta);
                datos.setearParametro("@contraseña", contraseña);
                datos.setearParametro("@idmedico", idMedico);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar la contraseña", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }



    }
}