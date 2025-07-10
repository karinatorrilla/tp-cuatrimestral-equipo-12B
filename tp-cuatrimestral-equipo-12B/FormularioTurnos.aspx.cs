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

                //Asignacion de turno
                if (!IsPostBack && Request["darturno"] != null)
                {
                    try
                    {
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
                        CargarSugerencias(0); //No hay especialidad seleccionada

                        /* Sugerencias de horarios */


                    }
                    catch (Exception ex)
                    {

                        throw ex;
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
            divContenedorSugerencias.Controls.Clear(); // Limpiar cualquier contenido previo

            if (idEspecialidadSeleccionada == 0)
            {
                divContenedorSugerencias.Controls.Add(new LiteralControl(
                    "<div class='alert alert-info' role='alert'>Debe seleccionar una Especialidad para ver sugerencias de horarios.</div>"
                ));
                return;
            }

            MedicosNegocio medicoNegocio = new MedicosNegocio();
            TurnosNegocio turnosNegocio = new TurnosNegocio();
            DisponibilidadHorariaNegocio dhNegocio = new DisponibilidadHorariaNegocio();

            List<Medico> medicosPorEspecialidad = medicoNegocio.listarMedicosPorEspecialidad(idEspecialidadSeleccionada);
            List<Tuple<DateTime, int, Medico>> sugerenciasEncontradas = new List<Tuple<DateTime, int, Medico>>();

            int cantidadSugerenciasRequeridas = 3;
            int limiteDiasBusqueda = 60; // Buscar en los próximos 60 días

            // Comenzar la búsqueda desde mañana si ya es tarde hoy, o desde hoy si aún hay tiempo.
            DateTime fechaActual = DateTime.Today;
            // Si ya son más de las 23:00 (23 PM), la búsqueda debería empezar desde mañana.
            if (DateTime.Now.Hour >= 23) // Usamos DateTime.Now para la hora actual
            {
                fechaActual = fechaActual.AddDays(1);
            }


            // Busqueda de Sugerencias
            for (int i = 0; i < limiteDiasBusqueda && sugerenciasEncontradas.Count < cantidadSugerenciasRequeridas; i++)
            {
                DateTime fechaBusqueda = fechaActual.AddDays(i);
                DayOfWeek diaDeLaSemana = fechaBusqueda.DayOfWeek;

                // (1=Lunes ..... 7=Domingo)
                int diaDeLaSemanaInt = (int)diaDeLaSemana;
                if (diaDeLaSemanaInt == 0) // Si es 0, lo convertimos a 7
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

                        //Gnerar horarios dentro del rango dfel medico
                        List<int> horariosPotenciales = new List<int>();
                        for (int hora = horaInicioBloque; hora < horaFinBloque; hora++)
                        {
                            // Si la fecha de busqueda es hoy entopnces omitir horarios que ya pasaron
                            // Tambien, si la búsqueda empieza desde hoy (fechaActual es hoy) y el horario ya pasó, se ignora.
                            if (fechaBusqueda.Date == DateTime.Today.Date && hora <= DateTime.Now.Hour)
                            {
                                continue;
                            }
                            horariosPotenciales.Add(hora);
                        }

                        //Obtenemos los turnos ya ocupados para este médico y esta fecha
                        List<int> horasOcupadas = turnosNegocio.listarHorasOcupadas(medico.Id, fechaBusqueda);

                        //Filtrar los horarios potenciales para encontrar los realmente disponibles
                        foreach (int horaPotencial in horariosPotenciales)
                        {
                            if (!horasOcupadas.Contains(horaPotencial))
                            {
                                // Encuentra horarios disponible
                                sugerenciasEncontradas.Add(Tuple.Create(fechaBusqueda, horaPotencial, medico));

                                // Si ya tenemos la cantidad requerida de sugerencias salimos
                                if (sugerenciasEncontradas.Count >= cantidadSugerenciasRequeridas)
                                {
                                    break;
                                }
                            }
                        }
                        if (sugerenciasEncontradas.Count >= cantidadSugerenciasRequeridas)
                        {
                            break; // Sale de bloques de disponibilidad
                        }
                    }
                    if (sugerenciasEncontradas.Count >= cantidadSugerenciasRequeridas)
                    {
                        break; // Sale de médicos
                    }
                }
            }

            // Ordenar las sugerencias por fecha y hora de más próxima a más lejana
            sugerenciasEncontradas = sugerenciasEncontradas
                .OrderBy(s => s.Item1) // Ordenar por fecha
                .ThenBy(s => s.Item2) // Luego por hora
                .ToList();

            //Mostrar las sugerencias en tu divContenedorSugerencias
            if (sugerenciasEncontradas.Any())
            {
                // Tomamos solo las primeras cantidadSugerenciasRequeridas
                foreach (var sugerencia in sugerenciasEncontradas.Take(cantidadSugerenciasRequeridas))
                {
                    DateTime fechaTurno = sugerencia.Item1;
                    int horaTurno = sugerencia.Item2;
                    Medico medicoSugerido = sugerencia.Item3;

                    string horaDisplay = $"{horaTurno:00}:00 hs";
                    string fechaDisplay = fechaTurno.ToString("dd/MM/yyyy");

                    Button btnSugerencia = new Button
                    {
                        ID = "btnSugerencia_" + Guid.NewGuid().ToString("N"),
                        Text = $"{medicoSugerido.NombreCompleto} - {horaDisplay} - {fechaDisplay}",
                        CssClass = "btn btn-outline-secondary me-2 my-1 btn-custom-suggestion",
                        CommandName = "SeleccionarSugerencia",
                        CommandArgument = $"{medicoSugerido.Id}|{fechaTurno.ToString("yyyy-MM-dd")}|{horaTurno}"
                    };

                    divContenedorSugerencias.Controls.Add(btnSugerencia);
                }
            }
            else
            {
                divContenedorSugerencias.Controls.Add(new LiteralControl(
                    "<div class='alert alert-warning' role='alert'>No se encontraron sugerencias de horarios para esta especialidad en los próximos días.</div>"
                ));
            }
        }



        protected void ddlEspecialidades_SelectedIndexChanged(object sender, EventArgs e)
        {

            //valida si no eligió especialidad
            if (!int.TryParse(ddlEspecialidades.SelectedValue, out int idEspecialidad) || idEspecialidad <= 0)
            {
                ddlMedicos.Items.Clear();
                ddlMedicos.Items.Add(new ListItem("Seleccione Médico", ""));
                ddlHorarios.Items.Clear();
                ddlHorarios.Items.Add(new ListItem("Seleccione Horario", ""));
                ddlMedicos.Enabled = false;
                ddlHorarios.Enabled = false;
                calTurno.CssClass += " calendar-disabled";


                // Si la especialidad no es válida, muestra el mensaje de "Debe seleccionar..."
                CargarSugerencias(0);
                ScriptManager.RegisterStartupScript(this, GetType(), "ToggleAccordion", "$('#panelsStayOpen-collapseTwo').collapse('hide');", true);
                updSugerencias.Update(); // Forzar la actualización del UpdatePanel de sugerencias

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

            // Llamar al método para cargar las sugerencias con la especialidad seleccionada
            CargarSugerencias(idEspecialidad);

            // Asegúrate de expandir el panel de sugerencias (colapso 2)
            ScriptManager.RegisterStartupScript(this, GetType(), "ToggleAccordion", "$('#panelsStayOpen-collapseTwo').collapse('show');", true);

            updSugerencias.Update(); // Forzar la actualización del UpdatePanel de sugerencias

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

            if (e.Day.Date <= DateTime.Today) //si el día es anterior o igual a hoy, deshabilitar
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


    }
}
