using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;
using dominio;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Medicos : System.Web.UI.Page
    {
        public List<Medico> listaMedico;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session["TipoUsuario"] == null)
                {
                    Session.Add("error", "Debes loguearte para ingresar.");
                    Response.Redirect("Error.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    MedicosNegocio negocio = new MedicosNegocio();
                    listaMedico = negocio.ListarMedicos();
                }

            }
            catch (Exception ex)
            {

                Session.Add("error", "Error al cargar el listado de médicos: " + ex.Message);
                listaMedico = new List<Medico>();
            }

        }



        protected void btnNuevoMedico_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioMedico.aspx", false);
        }
    }
}