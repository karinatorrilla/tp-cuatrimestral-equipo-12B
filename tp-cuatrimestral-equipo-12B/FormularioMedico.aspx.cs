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
    public partial class FormularioMedico : System.Web.UI.Page
    {

        // Deshabilitar todos los textbox y ddl del formulario
        private void DeshabilitarCampos()
        {
            txtMatricula.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtDni.Enabled = false;
            txtFechaNacimiento.Enabled = false;
            txtEmail.Enabled = false;
            txtTelefono.Enabled = false;
            txtNacionalidad.Enabled = false;
            ddlProvincia.Enabled = false;
            ddlLocalidad.Enabled = false;
            txtCalle.Enabled = false; 
            txtAltura.Enabled = false;
            txtCodPostal.Enabled = false;
            txtDepto.Enabled = false;
            lstEspecialidades.Enabled = false; 
            ddlTurnoTrabajo.Enabled = false;
            lstDiaSemana.Enabled = false;
            ddlHoraInicioBloque.Enabled = false;
            ddlHoraFinBloque.Enabled = false;  

        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            ///Config. inicial
            if (!IsPostBack)
            {
                // Iniciar Clases de negocio para cargar datos en los selectores
                EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
                TurnoTrabajoNegocio turnoTrabajoNegocio = new TurnoTrabajoNegocio();

                // Deshabilitar los DropDownLists de hora al inicio
                ddlHoraInicioBloque.Enabled = false;
                ddlHoraFinBloque.Enabled = false;


                // Cargar especialidades en el DropDownList

                lstEspecialidades.DataSource = especialidadNegocio.Listar();
                lstEspecialidades.DataValueField = "Id";
                lstEspecialidades.DataTextField = "Descripcion";
                lstEspecialidades.DataBind();

                // Actualizamos el conteo para el estado actual de las selecciones
                ActualizarConteoEspecialidades();
                ActualizarConteoDias();


                // Cargar Turnos de Trabajo en el DropDownList
                ddlTurnoTrabajo.DataSource = turnoTrabajoNegocio.Listar();
                ddlTurnoTrabajo.DataValueField = "Id";
                ddlTurnoTrabajo.DataTextField = "Descripcion";
                ddlTurnoTrabajo.DataBind();

                // Cargar las Provincias
                await PopulateProvincias();

                // Deshabilitar localidad al inicio si no hay provincia seleccionada
                ddlLocalidad.Enabled = false;
            }

            ///Config. para modificar médico
            if (Request.QueryString["id"] != null && !IsPostBack)
            {
                MedicosNegocio negocio = new MedicosNegocio();
                List<Medico> lista = negocio.ListarMedicos(int.Parse(Request.QueryString["id"]));
                Medico seleccionado = lista[0];



                //precarga de datos

                //try
                //{
                //    // Buscar el médico por ID
                //    int idMedico = int.Parse(Request.QueryString["id"].ToString());
                //    // Asume que ListarMedicos(id) devuelve una lista con un solo médico o null
                //    List<Medico> listaMedicos = medicosNegocio.ListarMedicos(idMedico);
                //    if (listaMedicos != null && listaMedicos.Count > 0)
                //    {
                //        medicoActual = listaMedicos[0]; // Guardamos el médico en la propiedad de la página

                //if (medicoActual.Especialidades != null)
                //{
                //    foreach (Especialidad especialidadMedico in medicoActual.Especialidades)
                //    {
                //        ListItem item = lstEspecialidades.Items.FindByValue(especialidadMedico.Id.ToString());
                //        if (item != null)
                //        {
                //            item.Selected = true; // Selecciona la especialidad en el ListBox (ahora DropDownList)
                //        }
                //    }
                //}

                //        // Precarga de Datos Personales y Profesionales
                //        txtMatricula.Text = medicoActual.Matricula.ToString();
                //        txtNombre.Text = medicoActual.Nombre;
                //        txtApellido.Text = medicoActual.Apellido;
                //        txtDni.Text = medicoActual.Documento.ToString();
                //        txtFechaNacimiento.Text = medicoActual.FechaNacimiento.ToString("yyyy-MM-dd"); // Formato para input type="date"
                //        txtEmail.Text = medicoActual.Email;
                //        txtTelefono.Text = medicoActual.Telefono;
                //        txtNacionalidad.Text = medicoActual.Nacionalidad;

                //        // Precarga de Domicilio
                //        ddlProvincia.SelectedValue = medicoActual.Provincia;
                //        await PopulateLocalidades(medicoActual.Provincia); // Cargar localidades antes de seleccionarla
                //        ddlLocalidad.SelectedValue = medicoActual.Localidad;
                //        ddlLocalidad.Enabled = true; // Habilitar la localidad
                //        txtCalle.Text = medicoActual.Calle; // Corregido de txtDireccion
                //        txtAltura.Text = medicoActual.Altura.ToString();
                //        txtCodPostal.Text = medicoActual.CodPostal;
                //        txtDepto.Text = medicoActual.Depto;


                //        // Precarga de Especialidades (Manejo de ListBox de selección múltiple)
                //        foreach (Especialidad especialidadMedico in medicoActual.Especialidades)
                //        {
                //            ListItem item = lstEspecialidades.Items.FindByValue(especialidadMedico.Id.ToString());
                //            if (item != null)
                //            {
                //                item.Selected = true; // Selecciona la especialidad en el ListBox
                //            }
                //        }

                //        // Precarga de Turno de Trabajo
                //        if (medicoActual.TurnoDeTrabajoAsignado != null)
                //        {
                //            ddlTurnoTrabajo.SelectedValue = medicoActual.TurnoDeTrabajoAsignado.Id.ToString();
                //        }

                //        // Precarga de Disponibilidad Horaria (solo el primer bloque si hay más de uno)
                //        // Para manejar múltiples bloques se necesitaría un Repeater o lógica JS avanzada.
                //        if (medicoActual.HorariosDisponibles != null && medicoActual.HorariosDisponibles.Count > 0)
                //        {
                //            DisponibilidadHoraria primerHorario = medicoActual.HorariosDisponibles[0];
                //            lstDiaSemana.SelectedValue = primerHorario.DiaDeLaSemana.ToString();
                //            txtHoraInicioBloque.Text = primerHorario.HoraInicioBloque.ToString(@"hh\:mm"); // Formato HH:mm
                //            txtHoraFinBloque.Text = primerHorario.HoraFinBloque.ToString(@"hh\:mm");     // Formato HH:mm
                //        }

                //        // Si el modo es "ver", deshabilitar campos y ocultar botón de guardar
                //        if (Request.QueryString["modo"] == "ver")
                //        {
                //            DeshabilitarCampos();
                //            btnGuardar.Visible = false;
                //        }
                //    }
                //    else
                //    {
                //        // Médico no encontrado
                //        Response.Redirect("Medicos.aspx", false); // Redirigir a la lista de médicos
                //    }
                //}
                //catch (Exception ex)
                //{
                //    // Manejo de errores al cargar datos del médico
                //    divMensaje.Attributes["class"] = "alert alert-danger";
                //    divMensaje.InnerText = "Error al cargar los datos del médico: " + ex.Message;
                //    divMensaje.Visible = true;
                //}
            }

        }

        protected void lstEspecialidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarConteoEspecialidades(); // Volvemos a calcular el conteo
        }

        private void ActualizarConteoEspecialidades()
        {
            int seleccionadas = 0;
            foreach (ListItem item in lstEspecialidades.Items)
            {
                if (item.Selected)
                {
                    seleccionadas++;
                }
            }

            if (seleccionadas > 0)
            {
                lblCantidadEspecialidadesSeleccionadas.Text = seleccionadas + " seleccionadas";
                lblCantidadEspecialidadesSeleccionadas.Visible = true;
            }
            else
            {
                lblCantidadEspecialidadesSeleccionadas.Text = ""; 
                lblCantidadEspecialidadesSeleccionadas.Visible = false;
            }
        }

        protected void lstDiaSemana_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarConteoDias();
        }

        //conteo de Das
        private void ActualizarConteoDias()
        {
            int seleccionados = 0;

            if (lstDiaSemana.Items.Count > 0)
            {
                foreach (ListItem item in lstDiaSemana.Items)
                {

                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    {
                        seleccionados++;
                    }
                }
            }


            if (seleccionados > 0)
            {
                lblCantidadDiasSeleccionados.Text = seleccionados + " seleccionados";
                lblCantidadDiasSeleccionados.Visible = true;
            }
            else
            {
                lblCantidadDiasSeleccionados.Text = "";
                lblCantidadDiasSeleccionados.Visible = false;
            }
        }

        // --- Lógica para la carga de Provincias y Localidades (reutilizada del FormularioPaciente) ---
        private async Task PopulateProvincias()
        {
            GeoRefNegocio geoRefNegocio = new GeoRefNegocio();
            try
            {
                List<GeoRefEntity> provincias = await geoRefNegocio.ObtenerProvinciasAsync();

                ddlProvincia.Items.Clear();
                ddlProvincia.Items.Add(new ListItem("Seleccione Provincia", ""));

                foreach (var provincia in provincias)
                {
                    ddlProvincia.Items.Add(new ListItem(provincia.nombre, provincia.id));
                }
            }
            catch (Exception)
            {
                divMensaje.Visible = true;
            }
        }

        protected async void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProvincia = ddlProvincia.SelectedValue;
            if (!string.IsNullOrEmpty(selectedProvincia))
            {
                ddlLocalidad.Enabled = true;
                await PopulateLocalidades(selectedProvincia);
            }
            else
            {
                ddlLocalidad.Items.Clear();
                ddlLocalidad.Items.Add(new ListItem("Seleccione Localidad", ""));
                ddlLocalidad.Enabled = false;
            }
        }

        private async Task PopulateLocalidades(string idProvincia)
        {
            GeoRefNegocio geoRefNegocio = new GeoRefNegocio();
            try
            {
                List<GeoRefEntity> localidades = await geoRefNegocio.ObtenerLocalidadesPorProvinciaAsync(idProvincia);

                ddlLocalidad.Items.Clear();
                ddlLocalidad.Items.Add(new ListItem("Seleccione Localidad", ""));

                foreach (var localidad in localidades)
                {
                    ddlLocalidad.Items.Add(new ListItem(localidad.nombre, localidad.id));
                }
            }
            catch (Exception ex)
            {
                divMensaje.Visible = true;
                ddlLocalidad.Enabled = false;
            }
        }

        // Método para cargar los DropDownLists de Turno de Trabajo
        private void CargarTurnosTrabajo()
        {
            TurnoTrabajoNegocio turnoNegocio = new TurnoTrabajoNegocio();
            List<TurnoTrabajo> listaTurnos = turnoNegocio.Listar();

            ddlTurnoTrabajo.DataSource = listaTurnos;
            ddlTurnoTrabajo.DataValueField = "Id";
            ddlTurnoTrabajo.DataTextField = "Descripcion";
            ddlTurnoTrabajo.DataBind();
        }

        // Manejador del evento SelectedIndexChanged para ddlTurnoTrabajo
        protected void ddlTurnoTrabajo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTurnoTrabajo.SelectedValue))
            {
                int idTurnoSeleccionado = int.Parse(ddlTurnoTrabajo.SelectedValue);
                CargarHorasDisponibilidad(idTurnoSeleccionado);
                ddlHoraInicioBloque.Enabled = true;
                ddlHoraFinBloque.Enabled = true;
            }
            else
            {
                // Si no se selecciona un turno, deshabilitar y limpiar los DDL de horas
                ddlHoraInicioBloque.ClearSelection();
                ddlHoraInicioBloque.Items.Clear();
                ddlHoraInicioBloque.Items.Insert(0, new ListItem("--:--", ""));
                ddlHoraInicioBloque.Enabled = false;

                ddlHoraFinBloque.ClearSelection();
                ddlHoraFinBloque.Items.Clear();
                ddlHoraFinBloque.Items.Insert(0, new ListItem("--:--", ""));
                ddlHoraFinBloque.Enabled = false;
            }
        }

        // Nuevo método para cargar las opciones de hora en los DropDownLists
        private void CargarHorasDisponibilidad(int idTurno)
        {
            TurnoTrabajoNegocio turnoNegocio = new TurnoTrabajoNegocio();
            TurnoTrabajo turnoSeleccionado = turnoNegocio.Listar().FirstOrDefault(t => t.Id == idTurno);

            // Limpiar y agregar el item por defecto antes de poblar
            ddlHoraInicioBloque.Items.Clear();
            ddlHoraInicioBloque.Items.Insert(0, new ListItem("--:--", ""));
            ddlHoraFinBloque.Items.Clear();
            ddlHoraFinBloque.Items.Insert(0, new ListItem("--:--", ""));

            if (turnoSeleccionado != null)
            {
                TimeSpan horaActual = turnoSeleccionado.HoraInicioBase;
                TimeSpan horaFin = turnoSeleccionado.HoraFinBase;

                // Aca se puede ajustar el intervalo de tiempo (ej. cada 15, 30, 60 minutos)
                TimeSpan intervalo = TimeSpan.FromMinutes(60);

                while (horaActual <= horaFin)
                {
                    string horaTexto = horaActual.ToString(@"hh\:mm");
                    ddlHoraInicioBloque.Items.Add(new ListItem(horaTexto, horaTexto));
                    ddlHoraFinBloque.Items.Add(new ListItem(horaTexto, horaTexto));
                    horaActual = horaActual.Add(intervalo);
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Medico medicoNuevo = new Medico(); // Se crea una nueva instancia de Medico
            MedicosNegocio negocio = new MedicosNegocio();

            try
            {
                // Obtener datos personales y de domicilio del formulario y asignarlos al objeto Medico
                medicoNuevo.Matricula = int.Parse(txtMatricula.Text);
                medicoNuevo.Nombre = txtNombre.Text;
                medicoNuevo.Apellido = txtApellido.Text;
                medicoNuevo.Documento = int.Parse(txtDni.Text);
                medicoNuevo.Email = txtEmail.Text;
                medicoNuevo.Telefono = txtTelefono.Text;
                medicoNuevo.Nacionalidad = txtNacionalidad.Text;
                medicoNuevo.Provincia = ddlProvincia.SelectedValue;
                medicoNuevo.Localidad = ddlLocalidad.SelectedValue;
                medicoNuevo.Calle = txtCalle.Text; // Corregido de txtDireccion
                medicoNuevo.Altura = int.Parse(txtAltura.Text);
                medicoNuevo.CodPostal = txtCodPostal.Text;
                medicoNuevo.Depto = txtDepto.Text;
                medicoNuevo.Habilitado = 1; // Por defecto habilitado 

                // Validar Fecha de Nacimiento
                DateTime fechaNacimiento;
                if (!DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento) || fechaNacimiento > DateTime.Now)
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "La fecha de nacimiento no es válida o es una fecha futura.";
                    divMensaje.Visible = true;
                    return;
                }
                medicoNuevo.FechaNacimiento = fechaNacimiento;

                //Asignar Especialidades (desde el ListBox de selección múltiple)
                medicoNuevo.Especialidades = new List<Especialidad>();
                foreach (ListItem item in lstEspecialidades.Items)
                {
                    if (item.Selected)
                    {
                        // Agrega una nueva Especialidad a la lista del médico
                        medicoNuevo.Especialidades.Add(new Especialidad { Id = int.Parse(item.Value), Descripcion = item.Text });
                    }
                }


                // Asignar Turno de Trabajo
                if (!string.IsNullOrEmpty(ddlTurnoTrabajo.SelectedValue))
                {
                    medicoNuevo.TurnoDeTrabajoAsignado = new TurnoTrabajo();
                    medicoNuevo.TurnoDeTrabajoAsignado.Id = int.Parse(ddlTurnoTrabajo.SelectedValue);
                    medicoNuevo.TurnoDeTrabajoAsignado.Descripcion = ddlTurnoTrabajo.SelectedItem.Text;
                }

                // Asignar Disponibilidad Horaria
                if (!string.IsNullOrEmpty(lstDiaSemana.SelectedValue) &&
                    !string.IsNullOrEmpty(ddlHoraInicioBloque.SelectedValue) && 
                    !string.IsNullOrEmpty(ddlHoraFinBloque.SelectedValue))    
                {
                    DisponibilidadHoraria dh = new DisponibilidadHoraria();
                    dh.DiaDeLaSemana = int.Parse(lstDiaSemana.SelectedValue);
                    dh.HoraInicioBloque = TimeSpan.Parse(ddlHoraInicioBloque.SelectedValue); 
                    dh.HoraFinBloque = TimeSpan.Parse(ddlHoraFinBloque.SelectedValue); 

                    //  validamos el orden.
                    if (dh.HoraInicioBloque >= dh.HoraFinBloque)
                    {
                        divMensaje.Attributes["class"] = "alert alert-danger";
                        divMensaje.InnerText = "La hora de inicio no puede ser igual o posterior a la hora de fin.";
                        divMensaje.Visible = true;
                        return;
                    }

                    medicoNuevo.HorariosDisponibles.Add(dh);
                }


                // Lógica para Agregar o Modificar
                if (Request.QueryString["id"] != null) // Modo Modificar
                {
                    medicoNuevo.Id = int.Parse(Request.QueryString["id"].ToString()); // Obtener el ID del médico a modificar
                    negocio.modificarMedico(medicoNuevo); // Llama al método de negocio para modificar
                    divMensaje.Attributes["class"] = "alert alert-success";
                    divMensaje.InnerText = "Modificación realizada con éxito.";
                    divMensaje.Visible = true;
                }
                else
                {
                    if (negocio.agregarMedico(medicoNuevo)) // Llama al método de negocio para agregar
                    {
                        divMensaje.Attributes["class"] = "alert alert-success";
                        divMensaje.InnerText = "Operación realizada con éxito.";
                        divMensaje.Visible = true;
                        Response.Redirect("Medicos.aspx", false);
                    }
                    else
                    {
                        divMensaje.Attributes["class"] = "alert alert-danger";
                        divMensaje.InnerText = "Ocurrió un error al procesar la operación.";
                        divMensaje.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Ocurrió un error al procesar la operación: " + ex.Message;
                divMensaje.Visible = true;
            }
        }

    }
}