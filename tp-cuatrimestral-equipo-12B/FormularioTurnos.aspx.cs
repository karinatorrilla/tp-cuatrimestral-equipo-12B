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
            //txtEmail.Enabled = false;
            //txtTelefono.Enabled = false;
            //ddlNacionalidad.Enabled = false;
            //ddlObraSocial.Enabled = false;
            //ddlProvincia.Enabled = false;
            //ddlLocalidad.Enabled = false;
            //txtDireccion.Enabled = false;
            //txtAltura.Enabled = false;
            //txtCodPostal.Enabled = false;
            //txtDepto.Enabled = false;
            //txtObservaciones.Enabled = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TipoUsuario"] == null)
            {
                Session.Add("error", "Debes loguearte para ingresar.");
                Response.Redirect("Error.aspx", false);
            }
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
                    txtFechaNacimiento.Text = seleccionado.FechaNacimiento.ToString(("yyyy-MM-dd"));
                    /* Datos del paciente */

                    /* Especialidad */
                    panelAsignarTurno.Visible = true;
                    EspecialidadNegocio negocioEsp = new EspecialidadNegocio();
                    List<Especialidad> listaEsp = negocioEsp.Listar();
                    ddlMedicos.Enabled = false;
                    calTurno.CssClass += " calendar-disabled";
                    ddlHorarios.Enabled = false;
                    ddlEspecialidades.Items.Clear();
                    ddlEspecialidades.Items.Add(new ListItem("Seleccione Especialidad", "")); // Item por defecto
                    ddlEspecialidades.DataSource = listaEsp;
                    ddlEspecialidades.DataTextField = "Descripcion";
                    ddlEspecialidades.DataValueField = "Id";
                    ddlEspecialidades.DataBind();
                    /* Especialidad */

                    /* Sugerencias de horarios */

                    /* Sugerencias de horarios */


                }
                catch (Exception ex)
                {

                    throw ex;
                }
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
                return;
            }

            MedicosNegocio negocio = new MedicosNegocio();
            List<Medico> lista = negocio.listarMedicosPorEspecialidad(idEspecialidad);
            ddlMedicos.Enabled = true;
            ddlMedicos.Items.Clear();
            ddlMedicos.Items.Add(new ListItem("Seleccione Médico", "")); // Item por defecto
            ddlMedicos.DataSource = lista;
            ddlMedicos.DataTextField = "NombreCompleto";
            ddlMedicos.DataValueField = "Id";
            ddlMedicos.DataBind();

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
