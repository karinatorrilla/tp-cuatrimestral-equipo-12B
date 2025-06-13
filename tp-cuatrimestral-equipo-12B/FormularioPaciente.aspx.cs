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
            ///Config. inicial
            if (!IsPostBack)
            {
                // Cargar obra social en el DropDownList
                ObraSocialNegocio obraSocialNegocio = new ObraSocialNegocio();
                ddlObraSocial.DataSource = obraSocialNegocio.Listar();
                ddlObraSocial.DataValueField = "Descripcion";
                ddlObraSocial.DataTextField = "Descripcion";
                ddlObraSocial.DataBind();
            }

            ///Config. para modificar paciente
            if (Request.QueryString["id"] != null && !IsPostBack)
            {
                PacienteNegocio negocio = new PacienteNegocio();
                List<Paciente> lista = negocio.ListarPacientes(int.Parse(Request.QueryString["id"]));
                Paciente seleccionado = lista[0];

                //precarga de datos

                txtNombre.Text = seleccionado.Nombre;
                txtApellido.Text = seleccionado.Apellido;
                txtDni.Text = seleccionado.Dni.ToString();
                txtEmail.Text = seleccionado.Email;
                txtFechaNacimiento.Text = seleccionado.FechaNacimiento.ToString(("yyyy-MM-dd"));
                // to-do --> txtDireccion.Text = seleccionado.Direccion;              
                ddlObraSocial.SelectedValue = seleccionado.ObraSocial;
            }
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

                //validacion de fecha nacimiento para que no sea fecha futura 
                DateTime fechaNacimiento;
                if (!DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento) || fechaNacimiento > DateTime.Now)
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "La fecha de nacimiento no es válida.";
                    divMensaje.Visible = true;
                    return;
                }
                pacientenuevo.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                pacientenuevo.Direccion = txtDireccion.Text;
                pacientenuevo.Email = txtEmail.Text;
                pacientenuevo.ObraSocial = ddlObraSocial.SelectedItem.Text;

                if (Request.QueryString["id"] != null)
                {
                    pacientenuevo.Id = int.Parse(Request.QueryString["id"].ToString()); //le paso el id para modificar
                    negocio.modificarPaciente(pacientenuevo);
                    divMensaje.Attributes["class"] = "alert alert-success";
                    divMensaje.InnerText = "Modificación realizada con éxito.";
                    divMensaje.Visible = true;
                }
                else
                {
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
            catch (Exception ex)
            {

                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Ocurrió un error al procesar la operación.";
                divMensaje.Visible = true;
            }

        }
    }
}