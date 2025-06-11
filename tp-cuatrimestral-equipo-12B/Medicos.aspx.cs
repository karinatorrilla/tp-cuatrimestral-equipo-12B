using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Medicos : System.Web.UI.Page
    {
        public List<Medico> listaMedico;
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString() == "Médico")
            {
                Session.Add("error", "Tenes que tener permisos de Administrador o Recepcionista para ver esta pantalla.");
                Response.Redirect("Error.aspx", false);
            }

            if (Session["TipoUsuario"] == null)
            {
                Session.Add("error", "Debes loguearte para ingresar.");
                Response.Redirect("Error.aspx", false);
            }


            ///se carga la lista actualizada por primera vez o despues de una eliminacion
            MedicosNegocio negocio = new MedicosNegocio();
            listaMedico = negocio.ListarMedicos();

        }

        protected void btnNuevoMedico_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioMedico.aspx", false);
        }
    }
}