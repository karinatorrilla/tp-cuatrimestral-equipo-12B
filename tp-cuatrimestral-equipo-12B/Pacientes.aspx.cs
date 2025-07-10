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
    public partial class Pacientes : System.Web.UI.Page
    {
        public List<Paciente> listaPaciente;
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


                if (!IsPostBack && Request["eliminar"] != null)
                {
                    int idEliminar;
                    if (int.TryParse(Request["eliminar"], out idEliminar))
                    {
                        PacienteNegocio pacienteNegocio = new PacienteNegocio();
                        pacienteNegocio.eliminarPaciente(idEliminar);
                    }
                }

                ///se carga la lista actualizada por primera vez o despues de una eliminacion
                ///PacienteNegocio negocio = new PacienteNegocio();
                ///listaPaciente = negocio.ListarPacientes();
                if (!IsPostBack)
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
                }
            }
            catch (Exception ex)
            {


                Session.Add("error", "Error al cargar la pagina: " + ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnNuevoPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioPaciente.aspx", false);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtrarPor = ddlFiltro.SelectedValue;
                string textoBusqueda = txtFiltro.Text.Trim().ToLower();


                PacienteNegocio negocio = new PacienteNegocio();

                List<Paciente> todos = negocio.ListarPacientes(); //inicalizo lista con todos los pacientes

                List<ObraSocial> obrasSociales = new ObraSocialNegocio().Listar(); //agrego descripción de obra social antes de filtrar
                foreach (var paciente in todos)
                {
                    var os = obrasSociales.FirstOrDefault(x => x.Id == paciente.ObraSocial);
                    paciente.DescripcionObraSocial = os != null ? os.Descripcion : "-";
                }

                if (string.IsNullOrWhiteSpace(textoBusqueda)) //si el campo de busqueda esta vacío, recarga toda la grilla nuevamente
                {
                    listaPaciente = negocio.ListarPacientes();
                    foreach (var paciente in listaPaciente)
                    {
                        var os = obrasSociales.FirstOrDefault(x => x.Id == paciente.ObraSocial);
                        paciente.DescripcionObraSocial = os != null ? os.Descripcion : "-";
                    }
                    return;
                }

                List<Paciente> filtrados = new List<Paciente>(); //inicializo otra lista para guardar los filtrados

                switch (filtrarPor)
                {
                    case "Nombre":
                        filtrados = todos.Where(p => p.Nombre != null && p.Nombre.ToLower().Contains(textoBusqueda)).ToList();
                        break;
                    case "Apellido":
                        filtrados = todos.Where(p => p.Apellido != null && p.Apellido.ToLower().Contains(textoBusqueda)).ToList();
                        break;
                    case "Documento":
                        filtrados = todos.Where(p => p.Documento.ToString().Contains(textoBusqueda)).ToList();
                        break;
                    case "Obra Social":
                        filtrados = todos.Where(p => p.DescripcionObraSocial != null && p.DescripcionObraSocial.ToLower().Contains(textoBusqueda)).ToList();
                        break;
                    case "Email":
                        filtrados = todos.Where(p => p.Email != null && p.Email.ToLower().Contains(textoBusqueda)).ToList();
                        break;
                }

                listaPaciente = filtrados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            //limpio controles
            ddlFiltro.SelectedIndex = 0;
            txtFiltro.Text = "";

            //se vuelven a cargar pacientes con descripción de obra social
            PacienteNegocio negocio = new PacienteNegocio();
            listaPaciente = negocio.ListarPacientes();
          
            List<ObraSocial> obrasSociales = new ObraSocialNegocio().Listar();
            foreach (var paciente in listaPaciente)
            {
                var os = obrasSociales.FirstOrDefault(x => x.Id == paciente.ObraSocial);
                paciente.DescripcionObraSocial = os != null ? os.Descripcion : "-";
            }
        }
    }
}