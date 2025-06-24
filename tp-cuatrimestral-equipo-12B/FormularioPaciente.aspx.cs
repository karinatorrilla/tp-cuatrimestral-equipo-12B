using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class FormularioPaciente : System.Web.UI.Page
    {

        // Deshabilitar todos los textbox y ddl del formulario
        private void DeshabilitarCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtDni.Enabled = false;
            txtFechaNacimiento.Enabled = false;
            txtEmail.Enabled = false;
            txtTelefono.Enabled = false;
            txtNacionalidad.Enabled = false;
            ddlObraSocial.Enabled = false;
            ddlProvincia.Enabled = false;
            ddlLocalidad.Enabled = false;
            txtDireccion.Enabled = false;
            txtAltura.Enabled = false;
            txtCodPostal.Enabled = false;
            txtDepto.Enabled = false;
            txtObservaciones.Enabled = false;
        }
        protected async void Page_Load(object sender, EventArgs e)
        {
            ///config inicial
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

            ///config para modificar
            if (Request.QueryString["id"] != null && !IsPostBack)
            {

                PacienteNegocio negocio = new PacienteNegocio();
                List<Paciente> lista = negocio.ListarPacientes(int.Parse(Request.QueryString["id"]));
                Paciente seleccionado = lista[0];

                string idProvincia = seleccionado.Provincia;
                ddlProvincia.SelectedValue = idProvincia;

                await PopulateLocalidades(idProvincia);

                string idLocalidad = seleccionado.Localidad;
                ddlLocalidad.SelectedValue = idLocalidad;
                ddlLocalidad.Enabled = true;


                //precarga de datos

                txtNombre.Text = seleccionado.Nombre;
                txtApellido.Text = seleccionado.Apellido;
                txtDni.Text = seleccionado.Documento.ToString();
                txtFechaNacimiento.Text = seleccionado.FechaNacimiento.ToString(("yyyy-MM-dd"));
                txtEmail.Text = seleccionado.Email;
                txtTelefono.Text = seleccionado.Telefono;
                txtNacionalidad.Text = seleccionado.Nacionalidad;
                ddlObraSocial.SelectedValue = seleccionado.ObraSocial.ToString();
                txtDireccion.Text = seleccionado.Calle;
                txtAltura.Text = seleccionado.Altura.ToString();
                txtCodPostal.Text = seleccionado.CodPostal;
                txtDepto.Text = seleccionado.Depto;
                txtObservaciones.Text = seleccionado.Observaciones;

                if (Request.QueryString["modo"] == "ver")
                {
                    DeshabilitarCampos();
                    btnGuardar.Visible = false;  // Oculta botón de guardar
                }
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

        private bool validarPaciente()
        {
            //validar teléfono con expresión regular
            string telefono = txtTelefono.Text;
            if (!Regex.IsMatch(telefono, @"^\d{1,10}$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "El teléfono debe contener solo números y tener hasta 10 dígitos.";
                divMensaje.Visible = true;
                return false;
            }

            //validar que nacionalidad solo sea Argentina
            if (txtNacionalidad.Text.Trim().ToLower() != "argentina")
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Solo se permite nacionalidad Argentina.";
                divMensaje.Visible = true;
                return false;
            }

            //validar máximo de caracteres en codigo postal a 6 y que sea solo números
            string codigoPostal = txtCodPostal.Text;
            if (!Regex.IsMatch(codigoPostal, @"^\d{1,6}$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "El código postal debe contener solo números y tener hasta 6 dígitos.";
                divMensaje.Visible = true;
                return false;
            }

            //validar máximo de caracteres en altura a 7 y que sea solo números
            string altura = txtAltura.Text;
            if (!Regex.IsMatch(altura, @"^\d{1,7}$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "La altura debe contener solo números y tener hasta 7 dígitos.";
                divMensaje.Visible = true;
                return false;
            }

            //validar que calle sea solo números o letras
            if (!Regex.IsMatch(txtDireccion.Text, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "La calle debe contener solo números y letras.";
                divMensaje.Visible = true;
                return false;
            }

            //validar si el paciente existe mediante el documento
            PacienteNegocio negocio = new PacienteNegocio();
            List<Paciente> lista = negocio.ListarPacientes();
            if (lista.Any(p => p.Documento == int.Parse(txtDni.Text)))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Ya existe un paciente registrado con ese documento.";
                divMensaje.Visible = true;
                return false;
            }

            return true;
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

                if (validarPaciente() == true)
                {
                    pacientenuevo.FechaNacimiento = fechaNacimiento;
                    pacientenuevo.Nombre = txtNombre.Text;
                    pacientenuevo.Apellido = txtApellido.Text;
                    pacientenuevo.Documento = int.Parse(txtDni.Text);
                    pacientenuevo.Email = txtEmail.Text;
                    pacientenuevo.Telefono = txtTelefono.Text;
                    pacientenuevo.Nacionalidad = txtNacionalidad.Text;
                    pacientenuevo.Provincia = ddlProvincia.SelectedValue;
                    pacientenuevo.Localidad = ddlLocalidad.SelectedValue;
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