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


        public bool agregarPaciente(Paciente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO PACIENTES (Nombre, Apellido, DNI, Email, FechaNacimiento, ObraSocial)" +
                 "VALUES (@Nombre, @Apellido, @DNI, @Email, @FechaNacimiento, @ObraSocial);");

                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@DNI", nuevo.Dni);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@FechaNacimiento", nuevo.FechaNacimiento);
                datos.setearParametro("@ObraSocial", nuevo.ObraSocial);

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
    }
}
