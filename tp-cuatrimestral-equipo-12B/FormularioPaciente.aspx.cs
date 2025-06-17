using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using dominio;
using negocio;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class FormularioPaciente : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar provincias
                await PopulateProvincias();


                // Cargar obra social en el DropDownList
                ObraSocialNegocio obraSocialNegocio = new ObraSocialNegocio();
                ddlObraSocial.DataSource = obraSocialNegocio.Listar();
                ddlObraSocial.DataValueField = "Id";
                ddlObraSocial.DataTextField = "Descripcion";
                ddlObraSocial.DataBind();


                // Deshabilitar localidad al inicio
                ddlLocalidad.Enabled = false;
            }

            if (Request.QueryString["id"] != null && !IsPostBack)
            {
                PacienteNegocio negocio = new PacienteNegocio();
                List<Paciente> lista = negocio.ListarPacientes(int.Parse(Request.QueryString["id"]));
                Paciente seleccionado = lista[0];

                //precarga de datos

                txtNombre.Text = seleccionado.Nombre;
                txtApellido.Text = seleccionado.Apellido;
                txtDni.Text = seleccionado.Documento.ToString();
                txtEmail.Text = seleccionado.Email;
                txtFechaNacimiento.Text = seleccionado.FechaNacimiento.ToString(("yyyy-MM-dd"));
                // to-do --> txtDireccion.Text = seleccionado.Direccion;              
            }
        }

        private async Task PopulateProvincias()
        {
            GeoRefNegocio geoRefNegocio = new GeoRefNegocio();
            try
            {
                List<GeoRefEntity> provincias = await geoRefNegocio.ObtenerProvinciasAsync();

                ddlProvincia.Items.Clear();
                ddlProvincia.Items.Add(new ListItem("Seleccione Provincia", "")); // Item por defecto

                foreach (var provincia in provincias)
                {
                    // Usar provincia.id (string) porque la API lo devuelve como string
                    ddlProvincia.Items.Add(new ListItem(provincia.nombre, provincia.id));
                }
            }
            catch (Exception)
            {
                divMensaje.Visible = true;
            }
        }

        // Este es el evento que se dispara cuando el usuario cambia la selección de provincia.
        // También debe ser async void para poder usar await dentro.
        protected async void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProvincia = ddlProvincia.SelectedValue;
            if (!string.IsNullOrEmpty(selectedProvincia))
            {
                // Si se selecciona una provincia válida, habilitar ddlLocalidad y cargar localidades
                ddlLocalidad.Enabled = true;
                await PopulateLocalidades(selectedProvincia); // Pasar el id de la provincia
            }
            else
            {
                // Si se selecciona el ítem "Seleccione Provincia", limpia las localidades
                ddlLocalidad.Items.Clear();
                ddlLocalidad.Items.Add(new ListItem("Seleccione Localidad", ""));
                ddlLocalidad.Enabled = false; //Deshabilita localidad
            }
        }

        // Este método se encarga de cargar las localidades según la provincia seleccionada.
        private async Task PopulateLocalidades(string idProvincia) // idProvincia es string según la API
        {
            GeoRefNegocio geoRefNegocio = new GeoRefNegocio();
            try
            {
                List<GeoRefEntity> localidades = await geoRefNegocio.ObtenerLocalidadesPorProvinciaAsync(idProvincia);

                ddlLocalidad.Items.Clear(); // Limpia    
                ddlLocalidad.Items.Add(new ListItem("Seleccione Localidad", "")); // Item por defecto

                foreach (var localidad in localidades)
                {
                    // Usar localidad.id (string)
                    ddlLocalidad.Items.Add(new ListItem(localidad.nombre, localidad.id));
                }
            }
            catch (Exception)
            {
                divMensaje.Visible = true;
                ddlLocalidad.Enabled = false;

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Paciente pacientenuevo = new Paciente();
            PacienteNegocio negocio = new PacienteNegocio();
            try
            {
                

                //validacion de fecha nacimiento para que no sea fecha futura 
                DateTime fechaNacimiento;
                if (!DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento) || fechaNacimiento > DateTime.Now)
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "La fecha de nacimiento no es válida.";
                    divMensaje.Visible = true;
                    return;
                }

                pacientenuevo.FechaNacimiento = fechaNacimiento;
                pacientenuevo.Nombre = txtNombre.Text;
                pacientenuevo.Apellido = txtApellido.Text;
                pacientenuevo.Documento = int.Parse(txtDni.Text);
                pacientenuevo.Email = txtEmail.Text;
                pacientenuevo.Telefono = txtTelefono.Text;
                pacientenuevo.Nacionalidad = txtNacionalidad.Text;
                pacientenuevo.Provincia = int.Parse(ddlProvincia.SelectedValue);
                pacientenuevo.Localidad = int.Parse(ddlLocalidad.SelectedValue);
                pacientenuevo.Calle = txtDireccion.Text;
                pacientenuevo.Altura = int.Parse(txtAltura.Text);
                pacientenuevo.CodPostal = txtCodPostal.Text;
                pacientenuevo.Depto = txtDepto.Text;
                pacientenuevo.ObraSocial = int.Parse(ddlObraSocial.SelectedValue);

                pacientenuevo.Observaciones = txtObservaciones.Text;

                // Habilitado: Por defecto en 1 (true) para nuevos pacientes
                pacientenuevo.Habilitado = 1;

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
            catch (Exception)
            {

                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Ocurrió un error al procesar la operación.";
                divMensaje.Visible = true;
            }

        }
    }
}