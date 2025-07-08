using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;
using dominio;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Inicio : System.Web.UI.Page
    {
        public List<Paciente> listaPaciente;
        public List<Medico> listaMedico;
        private void CargarMedicos()
        {
            MedicosNegocio medicoNegocio = new MedicosNegocio();
            listaMedico = medicoNegocio.ListarMedicos();
            DisponibilidadHorariaNegocio dhNegocio = new DisponibilidadHorariaNegocio();

            foreach (var medico in listaMedico)
            {
                medico.Especialidades = medicoNegocio.ListarEspecialidadesPorMedico(medico.Id);
                // Cargar las disponibilidades horarias para cada médico
                medico.Disponibilidades = dhNegocio.ListarPorMedico(medico.Id);
            }

            //guardo en session la lista
            Session["Medicos"] = listaMedico;
        }

        private void CargarPacientes()
        {
            PacienteNegocio negocio = new PacienteNegocio();
            listaPaciente = negocio.ListarPacientes();

            List<ObraSocial> obrasSociales = new ObraSocialNegocio().Listar();
            // Recorrer la lista de pacientes para enriquecer los datos a mostrar
            foreach (var paciente in listaPaciente)
            {
                // Obtener la descripción de la Obra Social
                var os = obrasSociales.FirstOrDefault(x => x.Id == paciente.ObraSocial);
                paciente.DescripcionObraSocial = os != null ? os.Descripcion : "-";

            }

            //guardo en session la lista
            Session["Pacientes"] = listaPaciente;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TipoUsuario"] != null)
            {
                lblTipoUsuario.Text = "Bienvenido " + Session["TipoUsuario"].ToString() + "!";
            }
            else
            {
                Session.Add("error", "Debes loguearte para ingresar.");
                Response.Redirect("Error.aspx", false);
            }

            if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 1) //visión del admin
            {
                divListadoGeneral.Visible = false; //ocultar la tabla de filtro
                pnlGraficoAdmin.Visible = true;
                panelTurnosMedico.Visible = false;
            }
            else
            {
                divListadoGeneral.Visible = true; //visión del recepcionista
                pnlGraficoAdmin.Visible = false;
                panelTurnosMedico.Visible = false;

                if (!IsPostBack)
                {
                    CargarPacientes();
                    CargarMedicos();

                    ddlFiltrarPor.Items.Clear();

                    // Añade los items al DropDownList
                    ddlFiltrarPor.Items.Add(new ListItem("Pacientes", "0"));
                    ddlFiltrarPor.Items.Add(new ListItem("Médicos", "1"));

                    //Selecciona por defecto
                    ddlFiltrarPor.SelectedIndex = 0;

                }
            }

            if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 3) //visión del médico
            {
                divListadoGeneral.Visible = false;
                pnlGraficoAdmin.Visible = false;
                panelTurnosMedico.Visible = true;

                if (!IsPostBack)
                {

                }
            }

            //recupero las listas desde session
            listaPaciente = (List<Paciente>)Session["Pacientes"];
            listaMedico = (List<Medico>)Session["Medicos"];

            //Muestra total de pacientes y medicos en las cards
            if (!IsPostBack)
            {
                PacienteNegocio pacienteNegocio = new PacienteNegocio();
                listaPaciente = pacienteNegocio.ListarPacientes();
                lblTotalPacientes.Text = listaPaciente.Count.ToString();

                MedicosNegocio medicoNegocio = new MedicosNegocio();
                listaMedico = medicoNegocio.ListarMedicos();
                lblTotalMedicos.Text = listaMedico.Count.ToString();
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

        protected void ddlFiltrarPor_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (ddlFiltrarPor.SelectedValue == "0") //pacientes
                {
                    CargarPacientes();

                }
                else if (ddlFiltrarPor.SelectedValue == "1") //médicos
                {
                    CargarMedicos();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}