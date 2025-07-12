using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tp_cuatrimestral_equipo_12B
{

    public partial class Turnos : System.Web.UI.Page
    {
        private string quitarAcentos(string texto) //para poder obviar en el filtrado
        {
            return new string(texto.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .Normalize(NormalizationForm.FormC);
        }
        public string GetEstadoCssClass(EstadoTurno estado)
        {
            switch (estado)
            {
                case EstadoTurno.Nuevo: return "estado-nuevo";
                case EstadoTurno.Reprogramado: return "estado-reprogramado";
                case EstadoTurno.Cancelado: return "estado-cancelado";
                case EstadoTurno.Inasistencia: return "estado-inasistencia";
                case EstadoTurno.Cerrado: return "estado-cerrado";
                default: return "estado-default";
            }
        }

        public List<Turno> listaTurno;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                /*Solo puede ver esta pagina un usuario que esté logueado*/
                if (Session["TipoUsuario"] == null)
                {
                    Session.Add("error", "Debes loguearte para ingresar.");
                    Response.Redirect("Error.aspx", false);
                    return;
                }

                /*Solo puede ver esta pagina un usuario que esté logueado*/

                if (!IsPostBack)
                {
                    TurnosNegocio negocio = new TurnosNegocio();

                    //CARGA DEL DLL DE FILTROS
                    ddlFiltro.Items.Clear();
                    ddlFiltro.Items.Add("Nombre");
                    ddlFiltro.Items.Add("Apellido");
                    ddlFiltro.Items.Add("Documento");
                    ddlFiltro.Items.Add("Especialidad");
                    //solo para admin y recepcionista,
                    //ya que el filtro de fecha está en la pagina de inicio del médico y
                    //no necesitamos filtrar por médico
                    if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] != 3)
                    {
                        ddlFiltro.Items.Add("Médico");
                        ddlFiltro.Items.Add("Fecha");
                    }
                    //
                    ddlFiltro.Items.Add("Estado");
                    ddlFiltro.SelectedIndex = 0;
                    txtFiltro.TextMode = TextBoxMode.SingleLine;
                    
                    //si hay médico logueado, cargar solo sus turnos
                    if (Session["IDMedico"] != null)
                    {
                        int idMedico = (int)Session["IDMedico"];
                        listaTurno = negocio.ListarTurnos(idMedico);
                    }
                    else //sino cargo todos los turnos (admin o recepcionista)
                    {
                        listaTurno = negocio.ListarTurnos();
                    }

                    if (Request["cancelar"] != null)
                    {
                        int id = int.Parse(Request["cancelar"]);
                        new TurnosNegocio().CambiarEstadoTurno(id, EstadoTurno.Cancelado);
                        listaTurno = negocio.ListarTurnos();
                    }

                    if (Request["inasistencia"] != null)
                    {
                        int id = int.Parse(Request["inasistencia"]);
                        new TurnosNegocio().CambiarEstadoTurno(id, EstadoTurno.Inasistencia);
                        listaTurno = negocio.ListarTurnos();
                    }

                    if (Request["cerrar"] != null)
                    {
                        int id = int.Parse(Request["cerrar"]);
                        new TurnosNegocio().CambiarEstadoTurno(id, EstadoTurno.Cerrado);
                        listaTurno = negocio.ListarTurnos();
                    }
                }

            }
            catch (Exception ex)
            {


                Session.Add("error", "Error al cargar la pagina: " + ex.Message);
                Response.Redirect("Error.aspx", false);

            }


        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            //limpio controles
            ddlFiltro.SelectedIndex = 0;
            txtFiltro.TextMode = TextBoxMode.SingleLine;
            txtFiltro.Text = "";

            //obtener id del medico si el usuario logueado es tipo 3
            int idMedico = 0;
            if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 3 && Session["IdMedico"] != null)
            {
                idMedico = (int)Session["IdMedico"];
            }

            //se vuelven a cargar los turnos filtrados por medico (si corresponde)...
            TurnosNegocio negocio = new TurnosNegocio();
            listaTurno = negocio.ListarTurnos(idMedico);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int idMedico = 0;
                if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 3 && Session["IdMedico"] != null)
                {
                    idMedico = (int)Session["IdMedico"];
                }

                string filtrarPor = ddlFiltro.SelectedValue;
                string textoBusqueda = quitarAcentos(txtFiltro.Text.Trim().ToLower());


                TurnosNegocio negocio = new TurnosNegocio();

                List<Turno> todos = negocio.ListarTurnos(idMedico); //lista completa de todos los turnos

                List<Turno> filtrados = new List<Turno>();

                if (string.IsNullOrWhiteSpace(textoBusqueda)) //si el campo de busqueda esta vacío, recarga toda la grilla nuevamente
                {
                    listaTurno = negocio.ListarTurnos();
                    return;
                }

                switch (filtrarPor)
                {
                    case "Nombre":
                        filtrados = todos.Where(t => t.Paciente.Nombre != null && quitarAcentos(t.Paciente.Nombre.ToLower()).Contains(textoBusqueda)).ToList();
                        break;
                    case "Apellido":
                        filtrados = todos.Where(t => t.Paciente.Apellido != null && quitarAcentos(t.Paciente.Apellido.ToLower()).Contains(textoBusqueda)).ToList();
                        break;
                    case "Documento":
                        filtrados = todos.Where(t => t.Paciente.Documento.ToString().Contains(textoBusqueda)).ToList();
                        break;
                    case "Especialidad":
                        filtrados = todos.Where(t => t.Especialidad.Descripcion != null && quitarAcentos(t.Especialidad.Descripcion.ToLower()).Contains(textoBusqueda)).ToList();
                        break;
                    case "Médico":
                        filtrados = todos.Where(t => t.Medico != null &&
                        (quitarAcentos((t.Medico.Nombre + " " + t.Medico.Apellido).ToLower()).Contains(textoBusqueda) ||
                        quitarAcentos(t.Medico.Nombre.ToLower()).Contains(textoBusqueda) ||
                        quitarAcentos(t.Medico.Apellido.ToLower()).Contains(textoBusqueda))
                        ).ToList();
                        break;
                    case "Fecha":
                        if (DateTime.TryParse(textoBusqueda, out DateTime fecha))
                        {
                            filtrados = todos.Where(t => t.Fecha.Date == fecha.Date).ToList();
                        }
                        break;
                    case "Estado":
                        filtrados = todos.Where(t => quitarAcentos(t.Estado.ToString().ToLower()).Contains(textoBusqueda)).ToList();
                        break;
                }

                listaTurno = filtrados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFiltro.SelectedValue == "Fecha")
            {
                txtFiltro.TextMode = TextBoxMode.Date;
            }
            else
            {
                txtFiltro.TextMode = TextBoxMode.SingleLine;
            }

            txtFiltro.Text = "";

            //obtener id del medico si el usuario logueado es tipo 3
            int idMedico = 0;
            if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 3 && Session["IdMedico"] != null)
            {
                idMedico = (int)Session["IdMedico"];
            }

            //recargar la lista segun el tipo de usuario
            TurnosNegocio negocio = new TurnosNegocio();
            listaTurno = negocio.ListarTurnos(idMedico);
        }
    }
}