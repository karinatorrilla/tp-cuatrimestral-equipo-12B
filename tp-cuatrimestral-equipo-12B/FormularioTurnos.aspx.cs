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
        public class SugerenciaTurno
        {
            public string TextoDisplay { get; set; }
            public string CommandArgumentValue { get; set; }
        }

        // Deshabilitar todos los textbox y ddl del formulario
        private void DeshabilitarCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtDni.Enabled = false;
            txtFechaNacimiento.Enabled = false;
            txtEmail.Enabled = false;

            //txtObservaciones.Enabled = false;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            rptSugerencias.ItemCommand += rptSugerencias_ItemCommand;
        }
        protected void Page_Load(object sender, EventArgs e)
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

                if (!IsPostBack)
                {
                    // Verificar si hay una especialidad seleccionada
                    if (ddlEspecialidades.SelectedValue != "")
                    {
                        int idEspecialidad = int.Parse(ddlEspecialidades.SelectedValue);
                        CargarSugerencias(idEspecialidad);
                    }
                    else
                    {
                        // No hay especialidad seleccionada, limpiar el Repeater
                        rptSugerencias.DataSource = null;
                        rptSugerencias.DataBind();
                    }

                    if (Request.QueryString["id"] != null)
                    {
                        int idTurno = int.Parse(Request.QueryString["id"]);
                        TurnosNegocio turnoNegocio = new TurnosNegocio();
                        Turno turno = turnoNegocio.ListarTurnos().FirstOrDefault(t => t.Id == idTurno);

                        if (turno != null)
                        {
                            // Datos del paciente
                            DeshabilitarCampos();
                            txtNombre.Text = turno.Paciente.Nombre;
                            txtApellido.Text = turno.Paciente.Apellido;
                            txtDni.Text = turno.Paciente.Documento.ToString();
                            txtEmail.Text = turno.Paciente.Email;
                            txtFechaNacimiento.Text = turno.Paciente.FechaNacimiento.ToString("yyyy-MM-dd");

                            // Datos del turno
                            txtObservaciones.Text = turno.Observaciones;

                            // Carga de especialidades
                            ddlEspecialidades.Items.Clear();
                            ddlEspecialidades.Items.Add(new ListItem("-", ""));
                            EspecialidadNegocio espNegocio = new EspecialidadNegocio();
                            ddlEspecialidades.DataSource = espNegocio.ListaEspecialidadesAsignadas();
                            ddlEspecialidades.DataTextField = "Descripcion";
                            ddlEspecialidades.DataValueField = "Id";
                            ddlEspecialidades.DataBind();
                            ddlEspecialidades.SelectedValue = turno.Especialidad.Id.ToString();

                            // Carga de médicos
                            MedicosNegocio medicoNegocio = new MedicosNegocio();
                            List<Medico> medicos = medicoNegocio.listarMedicosPorEspecialidad(turno.Especialidad.Id);
                            ddlMedicos.Items.Clear();
                            ddlMedicos.Items.Add(new ListItem("Seleccione Médico", ""));
                            ddlMedicos.DataSource = medicos;
                            ddlMedicos.DataTextField = "NombreCompleto";
                            ddlMedicos.DataValueField = "Id";
                            ddlMedicos.DataBind();
                            ddlMedicos.SelectedValue = turno.Medico.Id.ToString();
                            Session["IdMedicoSeleccionado"] = turno.Medico.Id;
                            DisponibilidadHorariaNegocio disponibilidadNegocio = new DisponibilidadHorariaNegocio();
                            List<DisponibilidadHoraria> disponibilidad = disponibilidadNegocio.ListarPorMedico(turno.Medico.Id);
                            Session["DiasTrabajaMedico"] = disponibilidad.Select(d => d.DiaDeLaSemana).ToList();

                            // Fecha y horario
                            calTurno.SelectedDate = turno.Fecha;
                            ddlHorarios.Items.Clear();
                            ddlHorarios.Items.Add(new ListItem(turno.Hora.ToString("00") + ":00", turno.Hora.ToString()));
                            ddlHorarios.SelectedValue = turno.Hora.ToString();


                            string modo = Request.QueryString["modo"];

                            if (modo == "ver")
                            {
                                // deshabilito los campos que faltan deshabilitar
                                ddlEspecialidades.Enabled = false;
                                ddlMedicos.Enabled = false;
                                calTurno.Enabled = false;
                                ddlHorarios.Enabled = false;
                                txtObservaciones.Enabled = false;
                                btnGuardar.Visible = false;
                            }
                            else if (modo == "editar")
                            {
                                // Reprogramar turno
                                ddlEspecialidades.Enabled = false;
                                ddlMedicos.Enabled = true;
                                calTurno.Enabled = true;
                                ddlHorarios.Enabled = true;
                                txtObservaciones.Enabled = true;
                                btnGuardar.Visible = true;

                                // Sugerencias
                                ViewState["EspecialidadSeleccionadaId"] = turno.Especialidad.Id;
                                CargarSugerencias(turno.Especialidad.Id);

                                ScriptManager.RegisterStartupScript(this, GetType(), "AbrirSugerenciasEditar",
                                    "$('#panelsStayOpen-collapseTwo').collapse('show');", true);
                            }
                        }
                    }
                }

                //Asignacion de turno
                if (!IsPostBack && Request["darturno"] != null)
                {

                    try
                    {
                        TurnosNegocio negociosTurno = new TurnosNegocio();
                        /* Datos del paciente */

                        DeshabilitarCampos();
                        PacienteNegocio negocio = new PacienteNegocio();
                        List<Paciente> lista = negocio.ListarPacientes(int.Parse(Request.QueryString["darturno"]));
                        Paciente seleccionado = lista[0];

                        txtNombre.Text = seleccionado.Nombre;
                        txtApellido.Text = seleccionado.Apellido;
                        txtDni.Text = seleccionado.Documento.ToString();
                        txtEmail.Text = seleccionado.Email.ToString();
                        txtFechaNacimiento.Text = seleccionado.FechaNacimiento.ToString(("yyyy-MM-dd"));
                        /* Datos del paciente */

                        /* Especialidad */
                        panelAsignarTurno.Visible = true;
                        EspecialidadNegocio negocioEsp = new EspecialidadNegocio();
                        List<Especialidad> listaEsp = negocioEsp.ListaEspecialidadesAsignadas();
                        ddlMedicos.Enabled = false;
                        calTurno.CssClass += " calendar-disabled";
                        ddlHorarios.Enabled = false;
                        ddlEspecialidades.Items.Clear();
                        ddlEspecialidades.Items.Add(new ListItem("-", "")); // Item por defecto
                        ddlEspecialidades.DataSource = listaEsp;
                        ddlEspecialidades.DataTextField = "Descripcion";
                        ddlEspecialidades.DataValueField = "Id";
                        ddlEspecialidades.DataBind();
                        /* Especialidad */

                        /* Sugerencias de horarios */

                        if (!string.IsNullOrEmpty(ddlEspecialidades.SelectedValue))
                        {
                            int idEspecialidadInicial;
                            if (int.TryParse(ddlEspecialidades.SelectedValue, out idEspecialidadInicial))
                            {
                                // Guarda el ID de la especialidad seleccionada en ViewState
                                ViewState["EspecialidadSeleccionadaId"] = idEspecialidadInicial;
                                CargarSugerencias(idEspecialidadInicial);
                            }
                            else
                            {
                                CargarSugerencias(0); //No hay especialidad seleccionada
                            }
                        }
                        else
                        {
                            CargarSugerencias(0); //No hay especialidad seleccionada
                        }


                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
                else if (ViewState["EspecialidadSeleccionadaId"] != null)
                {
                    CargarSugerencias((int)ViewState["EspecialidadSeleccionadaId"]);
                }
                else 
                {
                    
                    if (ViewState["EspecialidadSeleccionadaId"] != null)
                    {
                        int idEspecialidad = (int)ViewState["EspecialidadSeleccionadaId"];
                        CargarSugerencias(idEspecialidad); // Llama a cargar las sugerencias para reconstruir el Repeater
                    }
                    else
                    {
                        
                        CargarSugerencias(0);
                    }
                }

            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al cargar la pagina : " + ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        private void CargarSugerencias(int idEspecialidadSeleccionada)
        {
            List<SugerenciaTurno> sugerenciasParaRepeater = new List<SugerenciaTurno>();

            if (idEspecialidadSeleccionada == 0)
            {

                rptSugerencias.DataSource = null; // Limpiar el Repeater
                rptSugerencias.DataBind();
                return;
            }

            MedicosNegocio medicoNegocio = new MedicosNegocio();
            TurnosNegocio turnosNegocio = new TurnosNegocio();
            DisponibilidadHorariaNegocio dhNegocio = new DisponibilidadHorariaNegocio();

            List<Medico> medicosPorEspecialidad = medicoNegocio.listarMedicosPorEspecialidad(idEspecialidadSeleccionada);
            List<Tuple<DateTime, int, Medico>> sugerenciasEncontradas = new List<Tuple<DateTime, int, Medico>>();

            int cantidadSugerenciasRequeridas = 3;
            int limiteDiasBusqueda = 60; // Buscar en los próximos 60 días

            DateTime fechaActual = DateTime.Today;
            if (DateTime.Now.Hour >= 18)
            {
                fechaActual = fechaActual.AddDays(1);
            }

            for (int i = 0; i < limiteDiasBusqueda && sugerenciasEncontradas.Count < cantidadSugerenciasRequeridas; i++)
            {
                DateTime fechaBusqueda = fechaActual.AddDays(i);
                DayOfWeek diaDeLaSemana = fechaBusqueda.DayOfWeek;

                int diaDeLaSemanaInt = (int)diaDeLaSemana;
                if (diaDeLaSemanaInt == 0)
                    diaDeLaSemanaInt = 7;

                foreach (Medico medico in medicosPorEspecialidad)
                {
                    List<DisponibilidadHoraria> bloquesDisponiblesMedicoCompleto = dhNegocio.ListarPorMedico(medico.Id);
                    List<DisponibilidadHoraria> bloquesDisponiblesParaEsteDia = bloquesDisponiblesMedicoCompleto
                        .Where(b => b.DiaDeLaSemana == diaDeLaSemanaInt)
                        .ToList();

                    foreach (DisponibilidadHoraria bloque in bloquesDisponiblesParaEsteDia)
                    {
                        int horaInicioBloque = bloque.HoraInicioBloque.Hours;
                        int horaFinBloque = bloque.HoraFinBloque.Hours;

                        List<int> horariosPotenciales = new List<int>();
                        for (int hora = horaInicioBloque; hora < horaFinBloque; hora++)
                        {
                            if (fechaBusqueda.Date == DateTime.Today.Date && hora <= DateTime.Now.Hour)
                            {
                                continue;
                            }
                            horariosPotenciales.Add(hora);
                        }

                        List<int> horasOcupadas = turnosNegocio.listarHorasOcupadas(medico.Id, fechaBusqueda);

                        foreach (int horaPotencial in horariosPotenciales)
                        {
                            if (!horasOcupadas.Contains(horaPotencial))
                            {
                                sugerenciasEncontradas.Add(Tuple.Create(fechaBusqueda, horaPotencial, medico));

                                if (sugerenciasEncontradas.Count >= cantidadSugerenciasRequeridas)
                                {
                                    break;
                                }
                            }
                        }
                        if (sugerenciasEncontradas.Count >= cantidadSugerenciasRequeridas)
                        {
                            break;
                        }
                    }
                    if (sugerenciasEncontradas.Count >= cantidadSugerenciasRequeridas)
                    {
                        break;
                    }
                }
            }

            sugerenciasEncontradas = sugerenciasEncontradas
                .OrderBy(s => s.Item1)
                .ThenBy(s => s.Item2)
                .ToList();

            if (sugerenciasEncontradas.Any())
            {
                foreach (var sugerencia in sugerenciasEncontradas.Take(cantidadSugerenciasRequeridas))
                {
                    DateTime fechaTurno = sugerencia.Item1;
                    int horaTurno = sugerencia.Item2;
                    Medico medicoSugerido = sugerencia.Item3;

                    string horaDisplay = $"{horaTurno:00}:00 hs";
                    string fechaDisplay = fechaTurno.ToString("dd/MM/yyyy");

                    sugerenciasParaRepeater.Add(new SugerenciaTurno
                    {
                        TextoDisplay = $"{medicoSugerido.NombreCompleto} - {horaDisplay} - {fechaDisplay}",
                        CommandArgumentValue = $"{medicoSugerido.Id}|{fechaTurno.ToString("yyyy-MM-dd")}|{horaTurno}"
                    });
                }

            }
 

            rptSugerencias.DataSource = sugerenciasParaRepeater;
            rptSugerencias.DataBind();
        }

        protected void rptSugerencias_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Verifica que el comando sea el esperado
            if (e.CommandName == "SeleccionarSugerencia")
            {
                string commandArg = e.CommandArgument.ToString();


                // Crea un CommandEventArgs con el CommandName y CommandArgument recibidos.
                CommandEventArgs customEventArgs = new CommandEventArgs(e.CommandName, commandArg);

                // Llama a BtnSugerencia_Click
                BtnSugerencia_Click(source, customEventArgs);
            }
        }


        protected void BtnSugerencia_Click(object sender, CommandEventArgs e)
        {
            // Capturo los datos del CommandArgument
            string[] args = e.CommandArgument.ToString().Split('|');

            if (args.Length == 3)
            {
                string idMedicoStr = args[0];
                string fechaStr = args[1];
                string horaStr = args[2];

                if (!int.TryParse(ddlEspecialidades.SelectedValue, out int idEspecialidad) || idEspecialidad <= 0)
                {
                    return;
                }

                if (!int.TryParse(idMedicoStr, out int idMedico))
                {
                    return;
                }
                if (!DateTime.TryParse(fechaStr, out DateTime fechaSugerida))
                {
                    return;
                }

                MedicosNegocio medicoNegocio = new MedicosNegocio();
                List<Medico> listaMedicos = medicoNegocio.listarMedicosPorEspecialidad(idEspecialidad);

                ddlMedicos.Items.Clear();
                ddlMedicos.Items.Add(new ListItem("Seleccione Médico", ""));
                ddlMedicos.DataSource = listaMedicos;
                ddlMedicos.DataTextField = "NombreCompleto";
                ddlMedicos.DataValueField = "Id";
                ddlMedicos.DataBind();
                ddlMedicos.Enabled = true;

                try
                {
                    ddlMedicos.SelectedValue = idMedico.ToString();
                    ddlMedicos_SelectedIndexChanged(ddlMedicos, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    return;
                }

                calTurno.SelectedDates.Clear();
                calTurno.SelectedDate = fechaSugerida;
                calTurno_SelectionChanged(calTurno, EventArgs.Empty);

                int horaNumericaSugerida;
                if (horaStr.Contains(":"))
                {
                    horaNumericaSugerida = TimeSpan.Parse(horaStr).Hours;
                }
                else
                {
                    horaNumericaSugerida = int.Parse(horaStr);
                }

                try
                {
                    ddlHorarios.SelectedValue = horaNumericaSugerida.ToString();
                }
                catch (Exception ex)
                {
                    
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "ToggleAccordionPanels",
                    "$('#panelsStayOpen-collapseTwo').collapse('hide'); $('#panelsStayOpen-collapseThree').collapse('show');", true);

            }
        }


        protected void ddlEspecialidades_SelectedIndexChanged(object sender, EventArgs e)
        {

            //valida si no eligió especialidad
            if (!int.TryParse(ddlEspecialidades.SelectedValue, out int idEspecialidad) || idEspecialidad <= 0)
            {
                ViewState["EspecialidadSeleccionadaId"] = null;
                // Si la especialidad no es válida, muestra el mensaje de "Debe seleccionar..."
                CargarSugerencias(0);
                ddlMedicos.Items.Clear();
                ddlMedicos.Items.Add(new ListItem("Seleccione Médico", ""));
                ddlHorarios.Items.Clear();
                ddlHorarios.Items.Add(new ListItem("Seleccione Horario", ""));
                ddlMedicos.Enabled = false;
                ddlHorarios.Enabled = false;
                calTurno.CssClass += " calendar-disabled";

                updSugerencias.Update(); // Forzar la actualización del UpdatePanel de sugerencias

                // Cierra el acordeón de sugerencias si no hay especialidad seleccionada
                ScriptManager.RegisterStartupScript(this, GetType(), "CerrarSugerencias",
                    "$('#panelsStayOpen-collapseTwo').collapse('hide');", true);

                return;
            }


            // Si se seleccionó una especialidad válida
            MedicosNegocio negocio = new MedicosNegocio();
            List<Medico> lista = negocio.listarMedicosPorEspecialidad(idEspecialidad);
            ddlMedicos.Enabled = true;
            ddlMedicos.Items.Clear();
            ddlMedicos.Items.Add(new ListItem("Seleccione Médico", "")); // Item por defecto
            ddlMedicos.DataSource = lista;
            ddlMedicos.DataTextField = "NombreCompleto";
            ddlMedicos.DataValueField = "Id";
            ddlMedicos.DataBind();

            // Limpiar y deshabilitar controles de fecha/hora para una nueva selección de especialidad
            calTurno.SelectedDates.Clear();
            calTurno.Enabled = false; // Deshabilitar hasta seleccionar médico
            calTurno.CssClass += " calendar-disabled";
            ddlHorarios.Items.Clear();
            ddlHorarios.Items.Add(new ListItem("Seleccione Horario", ""));
            ddlHorarios.Enabled = false;

            if (int.TryParse(ddlEspecialidades.SelectedValue, out idEspecialidad))
            {
                
                ViewState["EspecialidadSeleccionadaId"] = idEspecialidad;
                CargarSugerencias(idEspecialidad);
            }
            else
            {
                // Si no hay una especialidad válida seleccionada (ej. el item "-"),
                // limpia el ViewState y las sugerencias.
                ViewState["EspecialidadSeleccionadaId"] = null; // O establece a 0, según tu lógica
                CargarSugerencias(0);
            }

            updSugerencias.Update(); // Forzar la actualización del UpdatePanel de sugerencias
            ScriptManager.RegisterStartupScript(this, GetType(), "AbrirSugerencias",
                "$('#panelsStayOpen-collapseTwo').collapse('show');", true);

        }

        protected void ddlMedicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //validar si no eligió médico
            if (!int.TryParse(ddlMedicos.SelectedValue, out int idMedico) || idMedico <= 0)
            {
                ddlHorarios.Items.Clear();
                ddlHorarios.Items.Add(new ListItem("Seleccione Horario", ""));
                ddlHorarios.Enabled = false;
                calTurno.CssClass += " calendar-disabled";
                calTurno.Enabled = false; // deshabilito calendario
                return;
            }

            //obtener disponibilidad
            DisponibilidadHorariaNegocio negocio = new DisponibilidadHorariaNegocio();
            List<DisponibilidadHoraria> disponibilidad = negocio.ListarPorMedico(idMedico);

            //guardar en Session los días disponibles (1 = lunes, ..., 7 = domingo) y el id del médico
            Session["DiasTrabajaMedico"] = disponibilidad.Select(d => d.DiaDeLaSemana).ToList();
            Session["IdMedicoSeleccionado"] = idMedico;

            //habilito el calendario
            calTurno.CssClass = calTurno.CssClass.Replace("calendar-disabled", "").Trim();
            calTurno.Enabled = true;

            //limpiar horarios
            ddlHorarios.Items.Clear();
            ddlHorarios.Enabled = false;

        }

        protected void calTurno_DayRender(object sender, DayRenderEventArgs e)
        {
            List<int> diasTrabaja = (List<int>)Session["DiasTrabajaMedico"];
            if (diasTrabaja == null)
            {
                return;
            }

            int diaSemana = (int)e.Day.Date.DayOfWeek;
            if (diaSemana == 0) //sirve para obtener el día de la semana en formato numérico,
                                //pero con una corrección personalizada para que el domingo sea 7 en lugar de 0
                                //(ya que en nuestra clase domingo = 7).
            {
                diaSemana = 7;
            }

            if (e.Day.Date < DateTime.Today) //si el día es anterior a hoy, deshabilitar
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.LightGray;
                return;
            }

            if (!diasTrabaja.Contains(diaSemana)) //disablea los días en los que el médico no trabaja
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.LightGray;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "AbrirObservaciones",
                "$('#panelsStayOpen-collapseFour').collapse('show');", true);
        }

        protected void calTurno_SelectionChanged(object sender, EventArgs e)
        {
            ddlHorarios.Items.Clear();

            int idMedico = (int)Session["IdMedicoSeleccionado"]; //obtengo id de médico de la session
            DateTime fechaSeleccionada = calTurno.SelectedDate; //obtengo fecha seleccionada

            int diaSemana = (int)fechaSeleccionada.DayOfWeek;
            if (diaSemana == 0) //sirve para obtener el día de la semana en formato numérico,
                                //pero con una corrección personalizada para que el domingo sea 7 en lugar de 0
                                //(ya que en nuestra clase domingo = 7).
            {
                diaSemana = 7;
            }

            //buscar bloque horario de ese día
            DisponibilidadHorariaNegocio negocio = new DisponibilidadHorariaNegocio();
            DisponibilidadHoraria bloque = negocio.ListarPorMedico(idMedico)
                .FirstOrDefault(d => d.DiaDeLaSemana == diaSemana); //devuelve todos los horarios del médico
                                                                    //en ese día

            if (bloque == null)
            {
                ddlHorarios.Items.Add(new ListItem("No se encontró disponibilidad.", ""));
                ddlHorarios.Enabled = false;
                return;
            }

            //busco turnos ocupados
            TurnosNegocio turnoNegocio = new TurnosNegocio();
            List<int> horasOcupadas = turnoNegocio.listarHorasOcupadas(idMedico, fechaSeleccionada);

            //bloque.HoraInicioBloque es el primer horario del turno del médico (ej: 08:00)
            //bloque.HoraFinBloque es la hora final (ej: 12:00)
            //cada vez que da una vuelta el for, se le suma 1 hora a la variable hora
            //si hora actual es 12:00, hora < 12:00 ?? NO, entonces sale del for
            for (TimeSpan hora = bloque.HoraInicioBloque; hora < bloque.HoraFinBloque; hora = hora.Add(TimeSpan.FromHours(1)))
            {
                if (!horasOcupadas.Contains(hora.Hours)) //si esta hora todavía no está ocupada, entonces la muestra
                {
                    ddlHorarios.Items.Add(new ListItem(hora.ToString(@"hh\:mm"), hora.Hours.ToString())); // "hh\\:mm" formatea 08:00, 09:00, etc
                }
            }

            ddlHorarios.Enabled = ddlHorarios.Items.Count > 0; //si hay al menos 1 item cargado, se habilita

            if (!ddlHorarios.Enabled) //si no se agregó ningún horario porque todos estan ocupados,
                                      //el ddl se desactiva y se muestra un mensaje indicando que
                                      //no hay horarios disponibles
            {
                ddlHorarios.Items.Add(new ListItem("Sin horarios disponibles.", ""));
            }

        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            TurnosNegocio negocioTurno = new TurnosNegocio();
            Turno turnoActual = new Turno();

            try
            {
                // Validar seleccionados
                if (calTurno.SelectedDate == DateTime.MinValue || string.IsNullOrEmpty(ddlHorarios.SelectedValue))
                {
                    divMensaje.Attributes["class"] = "alert alert-danger";
                    divMensaje.InnerText = "Debe seleccionar una fecha y horario válido.";
                    divMensaje.Visible = true;
                    return;
                }

                turnoActual.Fecha = calTurno.SelectedDate;
                turnoActual.Hora = int.Parse(ddlHorarios.SelectedValue);

                int idMedico = int.Parse(ddlMedicos.SelectedValue);
                int idEspecialidad = int.Parse(ddlEspecialidades.SelectedValue);
                turnoActual.Medico = new Medico { Id = idMedico };
                turnoActual.Especialidad = new Especialidad { Id = idEspecialidad };

                turnoActual.Observaciones = txtObservaciones.Text;

                if (Request.QueryString["modo"] == "editar" && Request.QueryString["id"] != null)
                {
                    // Reprogramar
                    int idTurno = int.Parse(Request.QueryString["id"]);
                    turnoActual.Id = idTurno;

                    Turno turnoOriginal = negocioTurno.ListarTurnos().FirstOrDefault(t => t.Id == idTurno);
                    if (turnoOriginal == null)
                    {
                        divMensaje.Attributes["class"] = "alert alert-danger";
                        divMensaje.InnerText = "No se encontró el turno a modificar.";
                        divMensaje.Visible = true;
                        return;
                    }

                    turnoActual.Paciente = turnoOriginal.Paciente;
                    turnoActual.Estado = EstadoTurno.Reprogramado;

                    negocioTurno.ReprogramarTurno(turnoActual);

                    divMensaje.Attributes["class"] = "alert alert-success";
                    divMensaje.InnerText = "Turno reprogramado con éxito.";
                    btnGuardar.Visible = false;
                }
                else if (Request.QueryString["darturno"] != null)
                {
                    // Alta nueva
                    int idPaciente = int.Parse(Request.QueryString["darturno"]);
                    turnoActual.Paciente = new Paciente { Id = idPaciente };
                    turnoActual.Estado = EstadoTurno.Nuevo;

                    if (negocioTurno.AgregarTurno(turnoActual))
                    {
                        divMensaje.Attributes["class"] = "alert alert-success";
                        divMensaje.InnerText = "Turno registrado con éxito.";
                        btnGuardar.Visible = false;
                    }
                    else
                    {
                        divMensaje.Attributes["class"] = "alert alert-danger";
                        divMensaje.InnerText = "No se pudo registrar el turno.";
                    }
                }

                divMensaje.Visible = true;
            }
            catch (Exception ex)
            {
                divMensaje.Attributes["class"] = "alert alert-danger";
                divMensaje.InnerText = "Error inesperado: " + ex.Message;
                divMensaje.Visible = true;
            }
        }

    }
}
