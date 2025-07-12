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
        public string ObtenerArregloJS(EstadoTurno estado)
        {
            var datos = ViewState["DatosPorEstadoTurnos"] as Dictionary<EstadoTurno, int[]>;
            if (datos != null && datos.ContainsKey(estado))
            {
                return string.Join(",", datos[estado]);
            }
            return "0,0,0,0,0,0,0,0,0,0,0,0";
        }


        public List<Paciente> listaPaciente;
        public List<Medico> listaMedico;
        public List<Turno> listaTurnos;

        private void CargarMedicos(DateTime? fecha = null) //parametros por omision
        {
            MedicosNegocio medicoNegocio = new MedicosNegocio();
            listaMedico = medicoNegocio.ListarMedicos(0, fecha); //lista los médicos, se puede filtrar por fecha
            DisponibilidadHorariaNegocio dhNegocio = new DisponibilidadHorariaNegocio();

            foreach (var medico in listaMedico)
            {
                medico.Especialidades = medicoNegocio.ListarEspecialidadesPorMedico(medico.Id);
                // Cargar las disponibilidades horarias para cada médico
                medico.Disponibilidades = dhNegocio.ListarPorMedico(medico.Id);            
            }        
        }

        private void CargarPacientes(DateTime? fecha = null) //parametros por omision
        {
            PacienteNegocio negocio = new PacienteNegocio();
            listaPaciente = negocio.ListarPacientes(0, fecha); //lista los pacientes, se puede filtrar por fecha
                                                              

            List<ObraSocial> obrasSociales = new ObraSocialNegocio().Listar();
            // Recorrer la lista de pacientes para enriquecer los datos a mostrar
            foreach (var paciente in listaPaciente)
            {
                // Obtener la descripción de la Obra Social
                var os = obrasSociales.FirstOrDefault(x => x.Id == paciente.ObraSocial);
                paciente.DescripcionObraSocial = os != null ? os.Descripcion : "-";               
            }                    
        }

        private void CargarTurnos(int idMedico = 0, DateTime? fecha = null) //parametros por omision
        {
            TurnosNegocio turnosNegocio = new TurnosNegocio();
            listaTurnos = turnosNegocio.ListarTurnos(idMedico, fecha);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TipoUsuario"] == null)
            {
                Session.Add("error", "Debes loguearte para ingresar.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            if ((int)Session["TipoUsuario"] == 3)//medico
            {
                if (Session["NombreMedico"] != null)
                {
                    lblTipoUsuario.Text = "¡Bienvenido/a " + Session["NombreMedico"].ToString() + "!";
                }
                else
                {
                    lblTipoUsuario.Text = "¡Bienvenido/a Médico GENÉRICO!";
                }

                if (!IsPostBack && Session["NombreMedico"] != null)
                {
                    int idMedico = (int)Session["IDMedico"]; //obtengo el id del medico desde la session
                    CargarTurnos(idMedico, DateTime.Today);
                    
                    ////muestra total de pacientes y turnos del día del médico asociado en las cards
                    //obtener pacientes únicos del médico
                    int totalPacientesDelMedico = listaTurnos
                        .Where(t => t.Paciente != null)
                        .Select(t => t.Paciente.Id)
                        .Distinct()
                        .Count();
                                   
                    int totalTurnosDelMedico = listaTurnos.Count;

                    lblTotalPacientes.Text = totalPacientesDelMedico.ToString();
                    lblTextoPaciente.Text = totalPacientesDelMedico == 1 ? "Paciente" : "Pacientes";                  

                    lblTotalTurnos.Text = totalTurnosDelMedico.ToString();
                    lblTextoTurno.Text = totalTurnosDelMedico == 1 ? "Turno" : "Turnos";

                }
            }
            else if ((int)Session["TipoUsuario"] == 2)//recepcionista
            {
                lblTipoUsuario.Text = "¡Bienvenido/a Recepcionista!";
                divListadoGeneral.Visible = true; //visión del recepcionista
                pnlGraficoAdmin.Visible = false;

                if (!IsPostBack)
                {
                    CargarPacientes(DateTime.Today); //LLAMO A LOS MÉTODOS CON LA FECHA DE HOY
                    CargarMedicos(DateTime.Today);
                    CargarTurnos(0, DateTime.Today);

                    ddlFiltrarPor.Items.Clear();

                    // Añade los items al DropDownList
                    ddlFiltrarPor.Items.Add(new ListItem("Pacientes", "0"));
                    ddlFiltrarPor.Items.Add(new ListItem("Médicos", "1"));

                    //Selecciona por defecto
                    ddlFiltrarPor.SelectedIndex = 0;

                    //Muestra total de pacientes, medicos  y turnos del día en las cards
                    int totalPacientes = listaPaciente.Count;
                    int totalMedicos = listaMedico.Count;
                    int totalTurnos = listaTurnos.Count;

                    lblTotalPacientes.Text = totalPacientes.ToString();
                    lblTextoPaciente.Text = totalPacientes == 1 ? "Paciente" : "Pacientes";

                    lblTotalMedicos.Text = totalMedicos.ToString();
                    lblTextoMedico.Text = totalMedicos == 1 ? "Médico" : "Médicos";

                    lblTotalTurnos.Text = totalTurnos.ToString();
                    lblTextoTurno.Text = totalTurnos == 1 ? "Turno" : "Turnos";

                }
            }
            else if ((int)Session["TipoUsuario"] == 1)//admin
            {
                lblTipoUsuario.Text = "¡Bienvenido Administrador!";
                divListadoGeneral.Visible = false; //ocultar la tabla de filtro
                pnlGraficoAdmin.Visible = true;
                if (!IsPostBack)
                {
                    CargarMedicos(); //llamo a los métodos para listar todos los turnos, todos los pacientes y todos los médicos sin importar la fecha
                    CargarPacientes();
                    CargarTurnos();

                    // Agrupar cantidad de turnos por estado y mes
                    Dictionary<EstadoTurno, int[]> datosPorEstado = new Dictionary<EstadoTurno, int[]>();

                    foreach (EstadoTurno estado in Enum.GetValues(typeof(EstadoTurno)))
                        datosPorEstado[estado] = new int[12]; // inicializo array de 12 meses

                    foreach (Turno t in listaTurnos)
                    {
                        int mes = t.Fecha.Month - 1; // índice 0-based
                        datosPorEstado[t.Estado][mes]++;
                    }

                    // Lo guardás en ViewState para poder accederlo desde la propiedad pública
                    ViewState["DatosPorEstadoTurnos"] = datosPorEstado;

                    //Muestra total de pacientes, medicos  y turnos del día en las cards
                    int totalPacientes = listaPaciente.Count;
                    int totalMedicos = listaMedico.Count;
                    int totalTurnos = listaTurnos.Count;

                    lblTotalPacientes.Text = totalPacientes.ToString();
                    lblTextoPaciente.Text = totalPacientes == 1 ? "Paciente" : "Pacientes";

                    lblTotalMedicos.Text = totalMedicos.ToString();
                    lblTextoMedico.Text = totalMedicos == 1 ? "Médico" : "Médicos";

                    lblTotalTurnos.Text = totalTurnos.ToString();
                    lblTextoTurno.Text = totalTurnos == 1 ? "Turno" : "Turnos";
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

            try
            {
                if (ddlFiltrarPor.SelectedValue == "0") //pacientes
                {
                    CargarPacientes(DateTime.Today);

                }
                else if (ddlFiltrarPor.SelectedValue == "1") //médicos
                {
                    CargarMedicos(DateTime.Today);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}