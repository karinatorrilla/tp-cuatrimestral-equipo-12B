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
    public partial class Medicos : System.Web.UI.Page
    {
        public List<Medico> listaMedico;
        public List<Especialidad> todasLasEspecialidades;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                MedicosNegocio medicoNegocio = new MedicosNegocio();
                if (Session["TipoUsuario"] == null)
                {
                    Session.Add("error", "Debes loguearte para ingresar.");
                    Response.Redirect("Error.aspx", false);
                    return;
                }
               
                if (!IsPostBack)
                {
                    CargarEspecialidades();
                    CargarMedicos();
                }

                
                if (Request["eliminar"] != null)
                {
                    int idEliminar;
                    if (int.TryParse(Request["eliminar"], out idEliminar))
                    {
                        try
                        {
                            MedicosNegocio medicosNegocio = new MedicosNegocio();
                            medicosNegocio.eliminarMedico(idEliminar);

                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-success d-block";
                            lblMensaje.Text = "Médico eliminado correctamente.";
                        }
                        catch (Exception ex)
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-danger d-block";
                            lblMensaje.Text = "Error al eliminar médico: " + ex.Message;
                        }
                    }

                    // Recargar médicos después de eliminar
                    CargarMedicos();
                }
            }
            catch (Exception ex)
            {

                Session.Add("error", "Error al cargar el listado de médicos: " + ex.Message);
                listaMedico = new List<Medico>();
            }

        }
        private void CargarMedicos()
        {
            MedicosNegocio medicoNegocio = new MedicosNegocio();
            listaMedico = medicoNegocio.ListarMedicos();

            foreach (var medico in listaMedico)
            {
                medico.Especialidades = medicoNegocio.ListarEspecialidadesPorMedico(medico.Id);
            }
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
            todasLasEspecialidades = especialidadNegocio.Listar();
        }

        protected void btnGuardarEspecialidades_Click(object sender, EventArgs e)
        {
            int medicoIdActual = -1;
          //  bool operacionExitosa = false;
            try
            {
                 //Revisar porque no lo encuntra
                HiddenField hfMedicoIdControl = (HiddenField)this.FindControl("hfMedicoId");
                if (hfMedicoIdControl != null && !string.IsNullOrEmpty(hfMedicoIdControl.Value))
                {
                    medicoIdActual = Convert.ToInt32(hfMedicoIdControl.Value);
                }
                else
                {
                    throw new Exception("No se pudo obtener el ID del médico.");
                }

                ListBox lstEspecialidadesModal = (ListBox)this.FindControl("lstEspecialidades");

                if (lstEspecialidadesModal == null)
                {
                    throw new Exception("No se pudo encontrar el ListBox.");
                }


                //Recopilar los IDs de las especialidades seleccionadas
                List<int> especialidadesSeleccionadas = new List<int>();
                foreach (ListItem item in lstEspecialidadesModal.Items)
                {
                    if (item.Selected)
                    {
                        especialidadesSeleccionadas.Add(Convert.ToInt32(item.Value));
                    }
                }

                MedicosNegocio medicoNegocio = new MedicosNegocio();
                medicoNegocio.AgregarEspecialidadPorMedico(medicoIdActual, especialidadesSeleccionadas);

               // operacionExitosa = true;
            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al agregar especialidades: " + ex.Message);
               // operacionExitosa = false;
            }

        }


        protected void btnNuevoMedico_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioMedico.aspx", false);
        }
    }
}