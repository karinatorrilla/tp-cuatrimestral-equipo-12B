using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TipoUsuario"] != null)
            {
                lblTipoUsuario.Text = "Bienvenido " + Session["TipoUsuario"].ToString() + "!";
            }
            else
            {
                Session.Add("error", "Debes loguearte para ingresar.");               
                Response.Redirect("Error.aspx", false);
            }

            if (!IsPostBack)
            {
                
                ddlFiltrarPor.Items.Clear();

                // Añade los items al DropDownList
                ddlFiltrarPor.Items.Add(new ListItem("Pacientes", "PACIENTE"));
                ddlFiltrarPor.Items.Add(new ListItem("Médicos", "MEDICO"));

                //Selecciona por defecto
                ddlFiltrarPor.SelectedIndex = 0;
            }
        }

        protected void btnIrTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Turnos.aspx", false);
        }

        protected void BtnIrMedicos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Medicos.aspx", false);
        }

        protected void btnIrPacientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pacientes.aspx", false);
        }
    }
}