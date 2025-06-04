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

        public List<Paciente> ListarPacientes()
        {


            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IDPaciente, Nombre, Apellido, Dni, Email, ObraSocial FROM PACIENTES");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Paciente aux = new Paciente();
                    aux.Id = (int)datos.Lector["IDPaciente"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Dni = (int)datos.Lector["Dni"];
                    aux.Email = (string)datos.Lector["Email"];
                    //aux.Direccion = (string)datos.Lector["Direccion"];
                    //aux.FechaNacimiento = (string)datos.Lector["FechaNacimiento"];
                    aux.ObraSocial = (string)datos.Lector["ObraSocial"];


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

    }
}
