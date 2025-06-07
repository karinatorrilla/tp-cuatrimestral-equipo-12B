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
            //SE HARCODEA ObraSociales  proximo hacer tabla y traer de DB !!!!!!!!!!!!!!!!
            lista.Add(new ObraSocial { Id = 1, Descripcion = "OSDE" });
            lista.Add(new ObraSocial { Id = 2, Descripcion = "Swiss Medical" });
            lista.Add(new ObraSocial { Id = 3, Descripcion = "Galeno" });
            lista.Add(new ObraSocial { Id = 4, Descripcion = "Medicus" });

            return lista;
        }
        //public List<ObraSocial> Listar()
        //{


        //    List<ObraSocial> lista = new List<ObraSocial>();
        //    AccesoDatos datos = new AccesoDatos();

        //    try
        //    {
        //        datos.setearConsulta("select Nombre from OBRASOCIAL");
        //        datos.ejecutarLectura();
        //        while (datos.Lector.Read())
        //        {
        //            ObraSocial aux = new ObraSocial();
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
