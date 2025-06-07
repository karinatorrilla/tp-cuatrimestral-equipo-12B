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
    public partial class FormularioPaciente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Cargar obra social en el DropDownList
            ObraSocialNegocio obraSocialNegocio = new ObraSocialNegocio();
            ddlObraSocial.DataSource = obraSocialNegocio.Listar();
            ddlObraSocial.DataValueField = "Id";       // Valor que se guarda (ID de la especialidad)
            ddlObraSocial.DataTextField = "Descripcion"; // Texto que se muestra en el DropDownList
            ddlObraSocial.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Paciente pacientenuevo = new Paciente();
            PacienteNegocio negocio = new PacienteNegocio();
            try
            {
                pacientenuevo.Nombre = txtNombre.Text;
                pacientenuevo.Apellido = txtApellido.Text;
                pacientenuevo.Dni = int.Parse(txtDni.Text);

                ////////////validacion de fecha nacimiento para que no sea fecha futura 
                DateTime fechaNacimiento;
                if (!DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento) || fechaNacimiento > DateTime.Now)
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "La fecha de nacimiento no es válida.";
                    divMensaje.Visible = true;
                    return;
                }
                
                pacientenuevo.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);

                //////////
                pacientenuevo.Direccion = txtDireccion.Text;
                pacientenuevo.Email = txtEmail.Text;
                pacientenuevo.ObraSocial = ddlObraSocial.SelectedItem.Text;   //Dropdownlist
                //pacientenuevo.ObraSocial = txtObraSocial.Text; //textbox

                if (negocio.agregarPaciente(pacientenuevo))
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