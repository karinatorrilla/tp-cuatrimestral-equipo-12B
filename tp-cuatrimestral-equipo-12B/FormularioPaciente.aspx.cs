using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
            ddlNacionalidad.Enabled = false;
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
            try
            {

                /*Solo puede ver esta pagina un usuario tipo admin(1) o recepcionista (2) */
                if (Session["TipoUsuario"] == null)
                {
                    Session.Add("error", "Debes loguearte para ingresar.");
                    Response.Redirect("Error.aspx", false);
                    return;
                }
                else if ((int)Session["TipoUsuario"] != 1 &&  (int)Session["TipoUsuario"] != 2)
                {
                    Session.Add("error", "No tenes los permisos para acceder");
                    Response.Redirect("Error.aspx", false);
                    return;
                }
                /*Solo puede ver esta pagina un usuario tipo admin(1) o recepcionista (2) */

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

                    // Cargar nacionalidades en el DropDownList (lista hardcodeada)
                    List<string> listanacionalidades = new List<string>()
                {
                "Afgana", "Albana", "Alemana", "Andorrana", "Angoleña", "Antiguana", "Saudí", "Argelina", "Argentina", "Armenia",
                "Australiana", "Austriaca", "Azerbaiyana", "Bahamesa", "Bangladesí", "Bareiní", "Belga", "Beliceña", "Beninesa",
                "Bielorrusa", "Birmana", "Boliviana", "Bosnia", "Botsuana", "Brasileña", "Bruneana", "Búlgara", "Burkinesa",
                "Burundesa", "Butanesa", "Cabuverdiana", "Camboyana", "Camerunesa", "Canadiense", "Catarí", "Chadiana", "Chilena",
                "China", "Chipriota", "Colombiana", "Comorense", "Norcoreana", "Surcoreana", "Marfileña", "Costarricense", "Croata",
                "Cubana", "Danesa", "Dominica", "Ecuatoriana", "Egipcia", "Salvadoreña", "Emiratí", "Eritrea", "Eslovaca",
                "Eslovena", "Española", "Estadounidense", "Estonia", "Etíope", "Filipina", "Finlandesa", "Fiyiana", "Francesa",
                "Gabonense", "Gambiana", "Georgiana", "Ghanesa", "Granadina", "Griega", "Guatemalteca", "Guyanesa", "Guineana",
                "Bisauguineana", "Ecuatoguineana", "Haitiana", "Hondureña", "Húngara", "India", "Indonesia", "Irakí", "Iraní",
                "Irlandesa", "Islandesa", "Marshallesa", "Salomonense", "Israelí", "Italiana", "Jamaicana", "Japonesa", "Jordana",
                "Kazaja", "Keniata", "Kirguisa", "Kiribatiana", "Kuwaití", "Laosiana", "Lesotense", "Letona", "Libanesa", "Liberiana",
                "Libia", "Liechtensteiniana", "Lituana", "Luxemburguesa", "Macedonia", "Malgache", "Malasia", "Malauí", "Maldiva",
                "Maliense", "Maltesa", "Marroquí", "Mauriciana", "Mauritana", "Mexicana", "Micronesia", "Moldava", "Monegasca",
                "Mongola", "Montenegrina", "Mozambiqueña", "Namibia", "Nauruana", "Nepalí", "Nicaragüense", "Nigerina", "Nigeriana",
                "Noruega", "Neozelandesa", "Omana", "Neerlandesa", "Pakistaní", "Palaosiana", "Panameña", "Papú", "Paraguaya",
                "Peruana", "Polaca", "Portuguesa", "Británica", "Centroafricana", "Checa", "Congoleña", "Congoleña (Rep. Dem.)",
                "Dominicana", "Ruandesa", "Rumana", "Rusa", "Samoana", "Sancristobaleña", "Sanmarinense", "Sanvicentina", "Santalucense",
                "Santotomense", "Senegalesa", "Serbia", "Seychellense", "Sierraleonesa", "Singapurense", "Siria", "Somalí",
                "Sri Lanka", "Suazi", "Sudafricana", "Sudanesa", "Sursudanesa", "Sueca", "Suiza", "Surinamesa", "Tailandesa",
                "Tanzana", "Tayika", "Timorense", "Togolesa", "Tongana", "Trinitense", "Tunecina", "Turcomana", "Turca", "Tuvaluana",
                "Ucraniana", "Ugandesa", "Uruguaya", "Uzbeca", "Vanuatense", "Vaticana", "Venezolana", "Vietnamita", "Yemení",
                "Yibutiana", "Zambiana", "Zimbabuense"
                };
                    ddlNacionalidad.DataSource = listanacionalidades;
                    ddlNacionalidad.DataBind();


                    // Deshabilitar localidad al inicio
                    ddlLocalidad.Enabled = false;
                }

                ///config para modificar
                if (Request.QueryString["id"] != null && !IsPostBack)
                {
                    try
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
                        ddlNacionalidad.SelectedValue = seleccionado.Nacionalidad;
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
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }




            }
            catch (Exception ex)
            {

                Session.Add("error", "Error al cargar la pagina " + ex.Message);
                Response.Redirect("Error.aspx", false);
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
            //validar nombre solo letras y no mayor a 100
            string nombre = txtNombre.Text;
            if (nombre.Length > 100)
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "El nombre no puede superar los 100 caracteres.";
                divMensaje.Visible = true;
                return false;
            }
            if (!Regex.IsMatch(nombre, "^(?!.* {2})(?! )[A-Za-zÁÉÍÓÚáéíóúÑñ]+( [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*(?<! )$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "El nombre debe contener solo letras, sin espacios ni al principio ni al final.";
                divMensaje.Visible = true;
                return false;
            }

            //validar apellido solo letras y no mayor a 100
            string apellido = txtApellido.Text;
            if (apellido.Length > 100)
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "El apellido no puede superar los 100 caracteres.";
                divMensaje.Visible = true;
                return false;
            }
            if (!Regex.IsMatch(apellido, @"^(?!.* {2})(?! )[A-Za-zÁÉÍÓÚáéíóúÑñ]+( [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*(?<! )$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "El apellido debe contener solo letras, sin espacios ni al principio ni al final.";
                divMensaje.Visible = true;
                return false;
            }

            //validar documento solo numeros y no mayor a 8
            string documento = txtDni.Text;
            if (!Regex.IsMatch(documento, @"^\d{1,8}$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "El documento debe contener solo números y tener hasta 8 dígitos.";
                divMensaje.Visible = true;
                return false;
            }

            //validar que se elija nacionalidad
            if (string.IsNullOrWhiteSpace(ddlNacionalidad.SelectedValue))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Debe seleccionar una nacionalidad.";
                divMensaje.Visible = true;
                return false;
            }

            //validar que se elija obra social
            if (string.IsNullOrWhiteSpace(ddlObraSocial.SelectedValue))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Debe seleccionar una obra social.";
                divMensaje.Visible = true;
                return false;
            }

            //validar que se elija provincia
            if (string.IsNullOrWhiteSpace(ddlProvincia.SelectedValue))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Debe seleccionar una provincia.";
                divMensaje.Visible = true;
                return false;
            }

            //validar que se elija localidad
            if (string.IsNullOrWhiteSpace(ddlLocalidad.SelectedValue))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Debe seleccionar una localidad.";
                divMensaje.Visible = true;
                return false;
            }

            //validar email que contenga formato de mail, validar longitud máxima del mail a 100
            string email = txtEmail.Text;
            if (email.Length > 100)
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "El email no puede superar los 100 caracteres.";
                divMensaje.Visible = true;
                return false;
            }
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Ingrese un email válido por favor.";
                divMensaje.Visible = true;
                return false;
            }

            //validar teléfono con expresión regular
            string telefono = txtTelefono.Text;
            if (!Regex.IsMatch(telefono, @"^\d{1,10}$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "El teléfono debe contener solo números y tener hasta 10 dígitos.";
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
            //permite espacios, puntos, comas y guiones
            string calle = txtDireccion.Text;
            if (!Regex.IsMatch(calle, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s.,-]{1,30}$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "La calle acepta solo números, letras, puntos, comas y guiones. No debe tener más de 30 caracteres.";
                divMensaje.Visible = true;
                return false;
            }

            //validar si el paciente existe mediante el documento
            PacienteNegocio negocio = new PacienteNegocio();
            List<Paciente> lista = negocio.ListarPacientes();

            if (Request.QueryString["id"] != null) //modificando
            {
                int idPacienteModificando = int.Parse(Request.QueryString["id"]);
                if (lista.Any(p => p.Documento == int.Parse(txtDni.Text) && p.Id != idPacienteModificando))
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "Ya existe un paciente registrado con ese documento.";
                    divMensaje.Visible = true;
                    return false;
                }
            }
            else //agregando
            {
                if (lista.Any(p => p.Documento == int.Parse(txtDni.Text)))
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "Ya existe un paciente registrado con ese documento.";
                    divMensaje.Visible = true;
                    return false;
                }
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
                //y que no sea menor a lo aceptado por SQL Server (1/1/1753)
                DateTime fechaNacimiento;
                if (!DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento) ||
                    fechaNacimiento > DateTime.Now ||
                    fechaNacimiento < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
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
                    pacientenuevo.Nacionalidad = ddlNacionalidad.SelectedValue;
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

                    // Lógica para Agregar o Modificar
                    if (Request.QueryString["id"] != null)
                    {
                        pacientenuevo.Id = int.Parse(Request.QueryString["id"].ToString()); //le paso el id para modificar
                        negocio.modificarPaciente(pacientenuevo);
                        divMensaje.Attributes["class"] = "alert alert-success";
                        divMensaje.InnerText = "Modificación realizada con éxito.";
                        divMensaje.Visible = true;
                        btnGuardar.Visible = false;
                    }
                    else
                    {
                        if (negocio.agregarPaciente(pacientenuevo))
                        {
                            divMensaje.Attributes["class"] = "alert alert-success";
                            divMensaje.InnerText = "Operación realizada con éxito.";
                            btnGuardar.Visible = false;
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