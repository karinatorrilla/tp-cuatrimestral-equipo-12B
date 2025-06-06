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

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Paciente pacientenuevo = new Paciente();
            PacienteNegocio negocio = new PacienteNegocio();

            pacientenuevo.Nombre = txtNombre.Text;
            pacientenuevo.Apellido = txtApellido.Text;
            pacientenuevo.Dni = int.Parse(txtDni.Text);
            //VALIDAR HASTA 8 DIGITOS FALTA
            pacientenuevo.FechaNacimiento = DateTime.Parse( txtFechaNacimiento.Text);
            pacientenuevo.Direccion = txtDireccion.Text;
            pacientenuevo.Email = txtEmail.Text;
            pacientenuevo.ObraSocial = txtObraSocial.Text;


        

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
    }
}