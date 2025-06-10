using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class FormularioMedico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Cargar especialidades en el DropDownList
             EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
             ddlEspecialidad.DataSource = especialidadNegocio.Listar();
             ddlEspecialidad.DataValueField = "Id";       // Valor que se guarda (ID de la especialidad)
             ddlEspecialidad.DataTextField = "Descripcion"; // Texto que se muestra en el DropDownList
             ddlEspecialidad.DataBind();
        }
    }
}