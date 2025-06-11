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
            ///Config. inicial
            if (!IsPostBack)
            {
                // Cargar especialidades en el DropDownList
                EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
                ddlEspecialidad.DataSource = especialidadNegocio.Listar();
                ddlEspecialidad.DataValueField = "Id";       // Valor que se guarda (ID de la especialidad)
                ddlEspecialidad.DataTextField = "Descripcion"; // Texto que se muestra en el DropDownList
                ddlEspecialidad.DataBind();
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Medico mediconuevo = new Medico();
            MedicosNegocio negocio = new MedicosNegocio();
            try
            {
                mediconuevo.Nombre = txtNombre.Text;
                mediconuevo.Apellido = txtApellido.Text;
                mediconuevo.Dni = int.Parse(txtDni.Text);
                mediconuevo.Email = txtEmail.Text;
                mediconuevo.Telefono = txtTelefono.Text;

                //validacion de fecha nacimiento para que no sea fecha futura 
                DateTime fechaNacimiento;
                if (!DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento) || fechaNacimiento > DateTime.Now)
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "La fecha de nacimiento no es válida.";
                    divMensaje.Visible = true;
                    return;
                }
                mediconuevo.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                //
                mediconuevo.Direccion = txtDireccion.Text;
                mediconuevo.Matricula = int.Parse(txtMatricula.Text);
                mediconuevo.EspecialidadSeleccionada = new Especialidad();
                mediconuevo.EspecialidadSeleccionada.Id = int.Parse(ddlEspecialidad.SelectedValue);

                if (negocio.agregarMedico(mediconuevo))
                {
                    divMensaje.Attributes["class"] = "alert alert-success";
                    divMensaje.InnerText = "Operación realizada con éxito.";
                }
                else
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "Ocurrió un error al procesar la operación.";
                }
                divMensaje.Visible = true;

            }
            catch (Exception ex)
            {

                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Ocurrió un error al procesar la operación.";
                divMensaje.Visible = true;
            }

        }

    }
}