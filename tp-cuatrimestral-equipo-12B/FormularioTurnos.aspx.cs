using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class FormularioTurnos : System.Web.UI.Page
    {

        // Deshabilitar todos los textbox y ddl del formulario
        private void DeshabilitarCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtDni.Enabled = false;
            txtFechaNacimiento.Enabled = false;
            //txtEmail.Enabled = false;
            //txtTelefono.Enabled = false;
            //ddlNacionalidad.Enabled = false;
            //ddlObraSocial.Enabled = false;
            //ddlProvincia.Enabled = false;
            //ddlLocalidad.Enabled = false;
            //txtDireccion.Enabled = false;
            //txtAltura.Enabled = false;
            //txtCodPostal.Enabled = false;
            //txtDepto.Enabled = false;
            //txtObservaciones.Enabled = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TipoUsuario"] == null)
            {
                Session.Add("error", "Debes loguearte para ingresar.");
                Response.Redirect("Error.aspx", false);
            }
            //Asignacion de turno
            if (!IsPostBack && Request["darturno"] != null)
            {
                try
                {
                    /* Datos del paciente */

                    DeshabilitarCampos();
                    PacienteNegocio negocio = new PacienteNegocio();
                    List<Paciente> lista = negocio.ListarPacientes(int.Parse(Request.QueryString["darturno"]));
                    Paciente seleccionado = lista[0];

                    txtNombre.Text = seleccionado.Nombre;
                    txtApellido.Text = seleccionado.Apellido;
                    txtDni.Text = seleccionado.Documento.ToString();
                    txtFechaNacimiento.Text = seleccionado.FechaNacimiento.ToString(("yyyy-MM-dd"));
                    /* Datos del paciente */

                    /* Especialidad */
                    panelAsignarTurno.Visible = true;
                    EspecialidadNegocio negocioEsp = new EspecialidadNegocio();
                    List<Especialidad> listaEsp = negocioEsp.Listar();
                    repEspecialidades.DataSource = listaEsp;
                    repEspecialidades.DataBind();
                    /* Especialidad */

                    /* Sugerencias de horarios */

                    /* Sugerencias de horarios */


                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

    }
}
