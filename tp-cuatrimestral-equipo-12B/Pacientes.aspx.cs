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


                if (Session["TipoUsuario"] == null)
                {
                    Session.Add("error", "Debes loguearte para ingresar.");
                    Response.Redirect("Error.aspx", false);
                }

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

                throw ex;
            }
        }

        protected void btnNuevoPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioPaciente.aspx", false);
        }
    }
}