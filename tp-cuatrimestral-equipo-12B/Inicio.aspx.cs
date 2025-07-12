using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Inicio : System.Web.UI.Page
    {
        public List<Paciente> listaPaciente;
        public List<Medico> listaMedico;
        public List<Turno> listaTurnos;

        public string ObtenerArregloJS(EstadoTurno estado)
        {
            var datos = ViewState["DatosPorEstadoTurnos"] as Dictionary<EstadoTurno, int[]>;
            if (datos != null && datos.ContainsKey(estado))
            {
                return string.Join(",", datos[estado]);
            }
            return "0,0,0,0,0,0,0,0,0,0,0,0";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TipoUsuario"] == null)
            {
                Session.Add("error", "Debes loguearte para ingresar.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            int tipoUsuario = (int)Session["TipoUsuario"];
            DateTime fechaHoy = DateTime.Today;

            // Instanciar negocios
            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            MedicosNegocio medicoNegocio = new MedicosNegocio();
            TurnosNegocio turnoNegocio = new TurnosNegocio();

            if (tipoUsuario == 3) // Médico
            {
                lblTipoUsuario.Text = Session["NombreMedico"] != null
                    ? $"¡Bienvenido/a {Session["NombreMedico"]}!"
                    : "¡Bienvenido/a Médico GENÉRICO!";

                if (!IsPostBack)
                {
                    if (Session["IDMedico"] != null)
                    {


                        int idMedico = (int)Session["IDMedico"];
                        listaTurnos = turnoNegocio.ListarTurnos(idMedico, fechaHoy);

                        int totalPacientes = listaTurnos
                            .Where(t => t.Paciente != null)
                            .Select(t => t.Paciente.Id)
                            .Distinct()
                            .Count();

                        int totalTurnos = listaTurnos.Count;

                        lblTotalPacientes.Text = totalPacientes.ToString();
                        lblTextoPaciente.Text = totalPacientes == 1 ? "Paciente" : "Pacientes";

                        lblTotalTurnos.Text = totalTurnos.ToString();
                        lblTextoTurno.Text = totalTurnos == 1 ? "Turno" : "Turnos";
                    }
                }
            }
            else if (tipoUsuario == 2) // Recepcionista
            {
                lblTipoUsuario.Text = "¡Bienvenido/a Recepcionista!";
                divListadoGeneral.Visible = true;
                pnlGraficoAdmin.Visible = false;

                if (!IsPostBack)
                {
                    listaPaciente = pacienteNegocio.ListarPacientes(0, fechaHoy);
                    listaMedico = medicoNegocio.ListarMedicos(0, fechaHoy);
                    listaTurnos = turnoNegocio.ListarTurnos(0, fechaHoy);
                    //                ClientScript.RegisterStartupScript(this.GetType(), "alertCantidadTurnos",
                    //"alert('Cantidad de turnos del día: " + listaTurnos.Count + "');", true);

                    ddlFiltrarPor.Items.Clear();
                    ddlFiltrarPor.Items.Add(new ListItem("Pacientes", "0"));
                    ddlFiltrarPor.Items.Add(new ListItem("Médicos", "1"));
                    ddlFiltrarPor.SelectedIndex = 0;

                    lblTotalPacientes.Text = listaPaciente.Count.ToString();
                    lblTextoPaciente.Text = listaPaciente.Count == 1 ? "Paciente" : "Pacientes";

                    lblTotalMedicos.Text = listaMedico.Count.ToString();
                    lblTextoMedico.Text = listaMedico.Count == 1 ? "Médico" : "Médicos";

                    lblTotalTurnos.Text = listaTurnos.Count.ToString();
                    lblTextoTurno.Text = listaTurnos.Count == 1 ? "Turno" : "Turnos";
                }
            }
            else if (tipoUsuario == 1) // Administrador
            {
                lblTipoUsuario.Text = "¡Bienvenido Administrador!";
                divListadoGeneral.Visible = false;
                pnlGraficoAdmin.Visible = true;

                if (!IsPostBack)
                {
                    listaMedico = medicoNegocio.ListarMedicos();
                    listaPaciente = pacienteNegocio.ListarPacientes();
                    listaTurnos = turnoNegocio.ListarTurnos();

                    Dictionary<EstadoTurno, int[]> datosPorEstado = new Dictionary<EstadoTurno, int[]>();

                    foreach (EstadoTurno estado in Enum.GetValues(typeof(EstadoTurno)))
                        datosPorEstado[estado] = new int[12];

                    foreach (Turno t in listaTurnos)
                    {
                        int mes = t.Fecha.Month - 1;
                        datosPorEstado[t.Estado][mes]++;
                    }

                    ViewState["DatosPorEstadoTurnos"] = datosPorEstado;

                    lblTotalPacientes.Text = listaPaciente.Count.ToString();
                    lblTextoPaciente.Text = listaPaciente.Count == 1 ? "Paciente" : "Pacientes";

                    lblTotalMedicos.Text = listaMedico.Count.ToString();
                    lblTextoMedico.Text = listaMedico.Count == 1 ? "Médico" : "Médicos";

                    lblTotalTurnos.Text = listaTurnos.Count.ToString();
                    lblTextoTurno.Text = listaTurnos.Count == 1 ? "Turno" : "Turnos";
                }
            }
            else
            {
                Session.Add("error", "Debes loguearte para ingresar.");
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnIrTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Turnos.aspx", false);
        }

        protected void BtnIrMedicos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Medicos.aspx", false);
        }

        protected void btnIrPacientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pacientes.aspx", false);
        }

        protected void ddlFiltrarPor_SelectedIndexChanged(object sender, EventArgs e) //filtrado de recepcionista con fecha de hoy
        {

            DateTime fechaHoy = DateTime.Today;

            if (ddlFiltrarPor.SelectedValue == "0")
            {
                // Filtro Pacientes del día
                PacienteNegocio negocioPaciente = new PacienteNegocio();
                listaPaciente = negocioPaciente.ListarPacientes(0, fechaHoy);
            }
            else if (ddlFiltrarPor.SelectedValue == "1")
            {
                // Filtro Médicos del día
                MedicosNegocio negocioMedico = new MedicosNegocio();
                listaMedico = negocioMedico.ListarMedicos(0, fechaHoy);
            }
        }

        protected void btnFiltrarTurnos_Click(object sender, EventArgs e)
        {
            if (Session["IdMedico"] != null)
            {
                int idMedico = (int)Session["IdMedico"];
                DateTime fecha;
                TurnosNegocio negocio = new TurnosNegocio();

                if (DateTime.TryParse(txtFechaFiltro.Text, out fecha))
                {
                    listaTurnos = negocio.ListarTurnos(idMedico, fecha);
                }
                else
                {
                    listaTurnos = negocio.ListarTurnos(idMedico, DateTime.Today);
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (Session["IdMedico"] != null)
            {
                TurnosNegocio negocio = new TurnosNegocio();
                int idMedico = (int)Session["IdMedico"];

                txtFechaFiltro.Text = "";
                listaTurnos = negocio.ListarTurnos(idMedico, DateTime.Today);
            }
        }
    }
}