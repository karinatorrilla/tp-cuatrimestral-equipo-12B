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
            //SE HARCODEA ESPECIALEDADES
            lista.Add(new Especialidad { Id = 1, Descripcion = "Cardiología" });
            lista.Add(new Especialidad { Id = 2, Descripcion = "Pediatría" });
            lista.Add(new Especialidad { Id = 3, Descripcion = "Dermatología" });
            lista.Add(new Especialidad { Id = 4, Descripcion = "Neurología" });
            lista.Add(new Especialidad { Id = 5, Descripcion = "Ginecología" });
            return lista;
        }

        //public List<Especialidad> Listar()
        //{


        //    List<Especialidad> lista = new List<Especialidad>();
        //    AccesoDatos datos = new AccesoDatos();

        //    try
        //    {
        //        datos.setearConsulta("select Nombre from ESPECIALIDADES");
        //        datos.ejecutarLectura();
        //        while (datos.Lector.Read())
        //        {
        //            Especialidad aux = new Especialidad();
        //            aux.Nombre = (string)datos.Lector["Nombre"];

        //            lista.Add(aux);
        //        }

        //        return lista;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        datos.cerrarConexion();
        //    }

        //}
    }
}