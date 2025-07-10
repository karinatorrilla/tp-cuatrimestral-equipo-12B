using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            ddlNacionalidad.Enabled = false;
            ddlProvincia.Enabled = false;
            ddlLocalidad.Enabled = false;
            txtCalle.Enabled = false;
            txtAltura.Enabled = false;
            txtCodPostal.Enabled = false;
            txtDepto.Enabled = false;

            // ddlTurnoTrabajo.Enabled = false;
            // lstDiaSemana.Enabled = false;
            // ddlHoraInicioBloque.Enabled = false;
            // ddlHoraFinBloque.Enabled = false;

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


                ///Config. inicial
                if (!IsPostBack)
                {
                    btnGuardar.Enabled = true;
                    panelEspecialidades.Visible = false;
                    botonesAgregarEspecialidad.Visible = false;

                    // Cargar las Provincias
                    await PopulateProvincias();

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

                    // Deshabilitar localidad al inicio si no hay provincia seleccionada
                    ddlLocalidad.Enabled = false;
                }

                ///Config. para modificar médico
                if (Request.QueryString["id"] != null && !IsPostBack)
                {


                    //precarga de datos

                    try
                    {
                        MedicosNegocio medicosNegocio = new MedicosNegocio();
                        Medico medicoActual = new Medico();
                        // Buscar el médico por ID
                        int idMedico = int.Parse(Request.QueryString["id"].ToString());
                        // Asume que ListarMedicos(id) devuelve una lista con un solo médico o null
                        List<Medico> listaMedicos = medicosNegocio.ListarMedicos(idMedico);
                        if (listaMedicos != null && listaMedicos.Count > 0)
                        {
                            medicoActual = listaMedicos[0]; // Guardamos el médico en la propiedad de la página

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
                            txtMatricula.Text = medicoActual.Matricula.ToString();
                            txtNombre.Text = medicoActual.Nombre;
                            txtApellido.Text = medicoActual.Apellido;
                            txtDni.Text = medicoActual.Documento.ToString();
                            txtFechaNacimiento.Text = medicoActual.FechaNacimiento.ToString("yyyy-MM-dd"); // Formato para input type="date"
                            txtEmail.Text = medicoActual.Email;
                            txtTelefono.Text = medicoActual.Telefono;
                            ddlNacionalidad.SelectedValue = medicoActual.Nacionalidad;

                            //        // Precarga de Domicilio
                            ddlProvincia.SelectedValue = medicoActual.Provincia;
                            await PopulateLocalidades(medicoActual.Provincia); // Cargar localidades antes de seleccionarla
                            ddlLocalidad.SelectedValue = medicoActual.Localidad;
                            ddlLocalidad.Enabled = true; // Habilitar la localidad
                            txtCalle.Text = medicoActual.Calle; // Corregido de txtDireccion
                            txtAltura.Text = medicoActual.Altura.ToString();
                            txtCodPostal.Text = medicoActual.CodPostal;
                            txtDepto.Text = medicoActual.Depto;


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

                            // Si el modo es "ver", deshabilitar campos y ocultar botón de guardar
                            if (Request.QueryString["modo"] == "ver")
                            {
                                DeshabilitarCampos();

                                btnGuardar.Visible = false;
                                // Mostrar panel de especialidades
                                panelEspecialidades.Visible = true;
                                alertaMensaje.Visible = false;
                                // Obtener las especialidades del médico
                                EspecialidadNegocio negocio = new EspecialidadNegocio();
                                List<Especialidad> todasEspecialidades = negocio.Listar();
                                List<int> idsEspecialidadesDelMedico = negocio.ListarIdsEspecialidadesPorMedico(idMedico);

                                // Filtrar las que tiene el médico
                                var especialidadesDelMedico = todasEspecialidades
                                    .Where(o => idsEspecialidadesDelMedico.Contains(o.Id))
                                    .ToList();

                                // Mostrar como texto (ej: separado por comas)
                                //   litEspecialidades.Text = string.Join(", ", especialidadesDelMedico.Select(o => o.Descripcion));
                                litEspecialidades.Text = string.Join(" ", especialidadesDelMedico.Select(o => $"<span class='badge bg-secondary me-1'>{o.Descripcion}</span>"));
                                litEspecialidades.Mode = LiteralMode.PassThrough;

                            }
                        }
                        else
                        {
                            // Médico no encontrado
                            Response.Redirect("Medicos.aspx", false); // Redirigir a la lista de médicos
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores al cargar datos del médico
                        divMensaje.Attributes["class"] = "alert alert-danger";
                        divMensaje.InnerText = "Error al cargar los datos del médico: " + ex.Message;
                        divMensaje.Visible = true;
                    }
                }
                if (!IsPostBack && Request["agregarespecialidad"] != null)
                {
                    try
                    {
                        //btnAgregarEspecialidad.Visible = true;
                        panelEspecialidades.Visible = true;
                        botonesAgregarEspecialidad.Visible = true;
                        alertaMensaje.Visible = false;
                        Medico medico = new Medico();
                        MedicosNegocio medicosNegocio = new MedicosNegocio();
                        int idmedico;
                        if (int.TryParse(Request["agregarespecialidad"], out idmedico))
                        {
                            List<Medico> listaMedicos = medicosNegocio.ListarMedicos(idmedico);
                            if (listaMedicos != null && listaMedicos.Count > 0)
                            {
                                medico = listaMedicos[0];
                                medico.Id = idmedico;
                            }
                            botonesAgregarEspecialidad.Visible = true;
                            panelbotonesGuardarMedico.Visible = false;
                            panelContacto.Visible = false;
                            panelDomicilio.Visible = false;
                            DeshabilitarCampos();
                            txtMatricula.Text = medico.Matricula.ToString();
                            txtNombre.Text = medico.Nombre;
                            txtApellido.Text = medico.Apellido;
                            txtDni.Text = medico.Documento.ToString();
                        }

                        EspecialidadNegocio negocio = new EspecialidadNegocio();
                        List<Especialidad> lista = negocio.Listar();

                        chkEspecialidades.DataSource = lista;
                        chkEspecialidades.DataTextField = "Descripcion";
                        chkEspecialidades.DataValueField = "ID";
                        chkEspecialidades.DataBind();

                        List<int> especialidadesAsociadas = negocio.ListarIdsEspecialidadesPorMedico(idmedico);
                        foreach (ListItem item in chkEspecialidades.Items)
                        {
                            if (especialidadesAsociadas.Contains(int.Parse(item.Value)))
                            {
                                item.Selected = true;
                            }
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

                Session.Add("error", "Error al cargar la pagina: " + ex.Message);
                Response.Redirect("Error.aspx", false);
            }

        }
        protected void btnAgregarEspecialidad_Click(object sender, EventArgs e)
        {
            int idMedico = int.Parse(Request["agregarespecialidad"]);
            List<int> especialidadesSeleccionadas = new List<int>();

            foreach (ListItem item in chkEspecialidades.Items)
            {
                if (item.Selected)
                {
                    especialidadesSeleccionadas.Add(int.Parse(item.Value));
                }
            }

            //valida que haya al menos una especialidad seleccionada
            if (especialidadesSeleccionadas.Count == 0)
            {
                alertaMensaje.Attributes["class"] = "alert alert-warning";
                alertaMensaje.InnerText = "Debe seleccionar al menos una especialidad.";
                alertaMensaje.Visible = true;
                return;
            }

            EspecialidadNegocio negocio = new EspecialidadNegocio();
            if (negocio.agregarEspecialidadesMedico(idMedico, especialidadesSeleccionadas))
            {
                alertaMensaje.Attributes["class"] = "alert alert-success";
                alertaMensaje.InnerText = "¡Guardado exitosamente!";
                btnAgregarEspecialidad.Visible = false;
            }
            else
            {
                alertaMensaje.Attributes["class"] = "alert alert-danger";
                alertaMensaje.InnerText = "Error al guardar.";
            }
            alertaMensaje.Visible = true;


        }



        protected void lstDiaSemana_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ActualizarConteoDias();
        }

        //conteo de Das
        //private void ActualizarConteoDias()
        //{
        //    int seleccionados = 0;

        //    if (lstDiaSemana.Items.Count > 0)
        //    {
        //        foreach (ListItem item in lstDiaSemana.Items)
        //        {

        //            if (item.Selected && !string.IsNullOrEmpty(item.Value))
        //            {
        //                seleccionados++;
        //            }
        //        }
        //    }


        //    if (seleccionados > 0)
        //    {
        //        lblCantidadDiasSeleccionados.Text = seleccionados + " seleccionados";
        //        lblCantidadDiasSeleccionados.Visible = true;
        //    }
        //    else
        //    {
        //        lblCantidadDiasSeleccionados.Text = "";
        //        lblCantidadDiasSeleccionados.Visible = false;
        //    }
        //}

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
            catch (Exception)
            {
                divMensaje.Visible = true;
                ddlLocalidad.Enabled = false;
            }
        }

        //// Método para cargar los DropDownLists de Turno de Trabajo
        //private void CargarTurnosTrabajo()
        //{
        //    TurnoTrabajoNegocio turnoNegocio = new TurnoTrabajoNegocio();
        //    List<TurnoTrabajo> listaTurnos = turnoNegocio.Listar();

        //    ddlTurnoTrabajo.DataSource = listaTurnos;
        //    ddlTurnoTrabajo.DataValueField = "Id";
        //    ddlTurnoTrabajo.DataTextField = "Descripcion";
        //    ddlTurnoTrabajo.DataBind();
        //}

        //// Manejador del evento SelectedIndexChanged para ddlTurnoTrabajo
        //protected void ddlTurnoTrabajo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(ddlTurnoTrabajo.SelectedValue))
        //    {
        //        int idTurnoSeleccionado = int.Parse(ddlTurnoTrabajo.SelectedValue);
        //        CargarHorasDisponibilidad(idTurnoSeleccionado);
        //        ddlHoraInicioBloque.Enabled = true;
        //        ddlHoraFinBloque.Enabled = true;
        //    }
        //    else
        //    {
        //        // Si no se selecciona un turno, deshabilitar y limpiar los DDL de horas
        //        ddlHoraInicioBloque.ClearSelection();
        //        ddlHoraInicioBloque.Items.Clear();
        //        ddlHoraInicioBloque.Items.Insert(0, new ListItem("--:--", ""));
        //        ddlHoraInicioBloque.Enabled = false;

        //        ddlHoraFinBloque.ClearSelection();
        //        ddlHoraFinBloque.Items.Clear();
        //        ddlHoraFinBloque.Items.Insert(0, new ListItem("--:--", ""));
        //        ddlHoraFinBloque.Enabled = false;
        //    }
        //}

        // Nuevo método para cargar las opciones de hora en los DropDownLists
        //private void CargarHorasDisponibilidad(int idTurno)
        //{
        //    TurnoTrabajoNegocio turnoNegocio = new TurnoTrabajoNegocio();
        //    TurnoTrabajo turnoSeleccionado = turnoNegocio.Listar().FirstOrDefault(t => t.Id == idTurno);

        //    // Limpiar y agregar el item por defecto antes de poblar
        //    ddlHoraInicioBloque.Items.Clear();
        //    ddlHoraInicioBloque.Items.Insert(0, new ListItem("--:--", ""));
        //    ddlHoraFinBloque.Items.Clear();
        //    ddlHoraFinBloque.Items.Insert(0, new ListItem("--:--", ""));

        //    if (turnoSeleccionado != null)
        //    {
        //        TimeSpan horaActual = turnoSeleccionado.HoraInicioBase;
        //        TimeSpan horaFin = turnoSeleccionado.HoraFinBase;

        //        // Aca se puede ajustar el intervalo de tiempo (ej. cada 15, 30, 60 minutos)
        //        TimeSpan intervalo = TimeSpan.FromMinutes(60);

        //        while (horaActual <= horaFin)
        //        {
        //            string horaTexto = horaActual.ToString(@"hh\:mm");
        //            ddlHoraInicioBloque.Items.Add(new ListItem(horaTexto, horaTexto));
        //            ddlHoraFinBloque.Items.Add(new ListItem(horaTexto, horaTexto));
        //            horaActual = horaActual.Add(intervalo);
        //        }
        //    }
        //}

        private bool validarMedico()
        {
            MedicosNegocio negocio = new MedicosNegocio();
            List<Medico> lista = negocio.ListarMedicos();

            //validar matrícula solo números y no mayor a 6
            //valida que no se repita la matricula 
            string matricula = txtMatricula.Text;
            if (Request.QueryString["id"] != null) //modificando
            {
                int idMedicoModificando = int.Parse(Request.QueryString["id"]);
                if (lista.Any(m => m.Matricula == int.Parse(matricula) && m.Id != idMedicoModificando))
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "Ya existe un médico registrado con esa matrícula.";
                    divMensaje.Visible = true;
                    return false;
                }
            }
            else //agregando
            {
                if (lista.Any(m => m.Matricula == int.Parse(matricula)))
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "Ya existe un médico registrado con esa matrícula.";
                    divMensaje.Visible = true;
                    return false;
                }
            }
            if (!Regex.IsMatch(matricula, @"^\d{1,6}$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "La matrícula debe contener solo números y tener hasta 6 dígitos. Sin espacios ni al principio ni al final.";
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
            string calle = txtCalle.Text;
            if (!Regex.IsMatch(calle, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s.,-]{1,30}$"))
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "La calle acepta solo números, letras, puntos, comas y guiones. No debe tener más de 30 caracteres.";
                divMensaje.Visible = true;
                return false;
            }

            //validar si el paciente existe mediante el documento           
            if (Request.QueryString["id"] != null) //modificando
            {
                int idMedicoModificando = int.Parse(Request.QueryString["id"]);
                if (lista.Any(m => m.Documento == int.Parse(txtDni.Text) && m.Id != idMedicoModificando))
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "Ya existe un médico registrado con ese documento.";
                    divMensaje.Visible = true;
                    return false;
                }
            }
            else //agregando
            {
                if (lista.Any(m => m.Documento == int.Parse(txtDni.Text)))
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "Ya existe un médico registrado con ese documento.";
                    divMensaje.Visible = true;
                    return false;
                }
            }

            return true;

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Medico medicoNuevo = new Medico(); // Se crea una nueva instancia de Medico
            MedicosNegocio negocio = new MedicosNegocio();

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

                if (validarMedico() == false)
                {
                    return;
                }

                // Obtener datos personales y de domicilio del formulario y asignarlos al objeto Medico
                medicoNuevo.Matricula = int.Parse(txtMatricula.Text);
                medicoNuevo.Nombre = txtNombre.Text;
                medicoNuevo.Apellido = txtApellido.Text;
                medicoNuevo.Documento = int.Parse(txtDni.Text);
                medicoNuevo.Email = txtEmail.Text;
                medicoNuevo.Telefono = txtTelefono.Text;
                medicoNuevo.Nacionalidad = ddlNacionalidad.Text;
                medicoNuevo.Provincia = ddlProvincia.SelectedValue;
                medicoNuevo.Localidad = ddlLocalidad.SelectedValue;
                medicoNuevo.Calle = txtCalle.Text;
                medicoNuevo.Altura = int.Parse(txtAltura.Text);
                medicoNuevo.CodPostal = txtCodPostal.Text;
                medicoNuevo.Depto = txtDepto.Text;
                // Habilitado: Por defecto en 1 (true) para nuevos medicoNuevo
                medicoNuevo.Habilitado = 1;
                medicoNuevo.FechaNacimiento = fechaNacimiento;

                // Asignar Especialidades
                // Recopila los IDs de las especialidades seleccionadas
                //List<string> especialidadesSeleccionadas = new List<string>();
                //foreach (ListItem item in lstEspecialidades.Items)
                //{
                //    if (item.Selected)
                //    {
                //        especialidadesSeleccionadas.Add(item.Value);
                //    }
                //}
                //// Convierte la lista de IDs a una cadena separada por comas
                //medicoNuevo.EspecialidadesIDs = string.Join(",", especialidadesSeleccionadas);


                //// Asignar Turno de Trabajo
                //if (!string.IsNullOrEmpty(ddlTurnoTrabajo.SelectedValue))
                //{
                //    medicoNuevo.IDTurnoTrabajo = int.Parse(ddlTurnoTrabajo.SelectedValue);
                //}
                //else
                //{
                //    medicoNuevo.IDTurnoTrabajo = null;
                //}


                ////Asignar Días Disponibles
                ////Recopila los IDs de los días seleccionados
                //List<string> diasSeleccionados = new List<string>();
                //foreach (ListItem item in lstDiaSemana.Items)
                //{
                //    if (item.Selected)
                //    {
                //        diasSeleccionados.Add(item.Value);
                //    }
                //}
                ////Convierte la lista de IDs a una cadena separada por comas
                //medicoNuevo.DiasDisponiblesIDs = string.Join(",", diasSeleccionados);


                //// Asignar Disponibilidad Horaria (HoraInicioBloque, HoraFinBloque)
                //TimeSpan horaInicio;
                //TimeSpan horaFin;

                //if (TimeSpan.TryParse(ddlHoraInicioBloque.SelectedValue, out horaInicio) &&
                //    TimeSpan.TryParse(ddlHoraFinBloque.SelectedValue, out horaFin))
                //{
                //    medicoNuevo.HoraInicioBloque = horaInicio;
                //    medicoNuevo.HoraFinBloque = horaFin;

                //    // Validamos el orden.
                //    if (medicoNuevo.HoraInicioBloque >= medicoNuevo.HoraFinBloque)
                //    {
                //        divMensaje.Attributes["class"] = "alert alert-danger";
                //        divMensaje.InnerText = "La hora de inicio no puede ser igual o posterior a la hora de fin.";
                //        divMensaje.Visible = true;
                //        return;
                //    }
                //}
                //else
                //{
                //    medicoNuevo.HoraInicioBloque = null;
                //    medicoNuevo.HoraFinBloque = null;
                //}


                // Lógica para Agregar o Modificar
                if (Request.QueryString["id"] != null) // Modo Modificar
                {
                    medicoNuevo.Id = int.Parse(Request.QueryString["id"].ToString()); // Obtener el ID del médico a modificar
                    negocio.modificarMedico(medicoNuevo); // Llama a modificar medico
                    divMensaje.Attributes["class"] = "alert alert-success";
                    divMensaje.InnerText = "Modificación realizada con éxito.";
                    divMensaje.Visible = true;
                    btnGuardar.Visible = false;
                }
                else
                {
                    if (negocio.agregarMedico(medicoNuevo)) // Llama a agregar medico
                    {
                        divMensaje.Attributes["class"] = "alert alert-success";
                        divMensaje.InnerText = "Operación realizada con éxito.";
                        divMensaje.Visible = true;
                        btnGuardar.Visible = false;
                        // Response.Redirect("Medicos.aspx", false); 
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