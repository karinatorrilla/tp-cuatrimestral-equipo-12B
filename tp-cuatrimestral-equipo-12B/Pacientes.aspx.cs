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
    public partial class Pacientes : System.Web.UI.Page
    {
        public List<Paciente> listaPaciente;
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

            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            listaPaciente = pacienteNegocio.ListarPacientes();

        }

        protected void btnNuevoPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioPaciente.aspx", false);
        }
    }
}