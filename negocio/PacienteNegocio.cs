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

        public List<Paciente> ListarPacientes(int id = 0)
        {


            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT IDPaciente, Nombre, Apellido, Dni, Email, FechaNacimiento, ObraSocial FROM PACIENTES where Habilitado = 1  ";
                if (id > 0)
                {
                    consulta += " AND WHERE IDPaciente = " + id;
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Paciente aux = new Paciente();
                    aux.Id = (int)datos.Lector["IDPaciente"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Dni = (int)datos.Lector["Dni"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    //aux.Direccion = (string)datos.Lector["Direccion"];
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

        public void modificarPaciente(Paciente mod)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Update PACIENTES set Nombre = @nombre, Apellido = @apellido, DNI = @dni, Email = @email, FechaNacimiento = @fechanacimiento, ObraSocial = @obrasocial WHERE IDPaciente = @id");
                datos.setearParametro("@id", mod.Id);
                datos.setearParametro("@Nombre", mod.Nombre);
                datos.setearParametro("@Apellido", mod.Apellido);
                datos.setearParametro("@DNI", mod.Dni);
                datos.setearParametro("@Email", mod.Email);
                datos.setearParametro("@FechaNacimiento", mod.FechaNacimiento);
                datos.setearParametro("@ObraSocial", mod.ObraSocial);
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

        public void eliminarPaciente(int idPaciente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Update PACIENTES set Habilitado=0 WHERE IDPaciente = @id");
                datos.setearParametro("@id", idPaciente);
              
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
