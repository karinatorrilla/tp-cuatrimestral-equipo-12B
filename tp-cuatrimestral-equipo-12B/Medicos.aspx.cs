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

                if (Request["medicoId"] != null && Request["dia"] != null && Request["horaInicio"] != null && Request["horaFin"] != null)
                {
                    int medicoId;
                    int diaSemana;
                    TimeSpan horaInicio;
                    TimeSpan horaFin;

                    // Intentar para  parsear todos los parámetros
                    if (int.TryParse(Request["medicoId"], out medicoId) &&
                        int.TryParse(Request["dia"], out diaSemana) &&
                        TimeSpan.TryParse(Request["horaInicio"], out horaInicio) &&
                        TimeSpan.TryParse(Request["horaFin"], out horaFin))
                    {
                        try
                        {
                            DisponibilidadHoraria nuevaDisponibilidad = new DisponibilidadHoraria
                            {
                                MedicoId = medicoId,
                                DiaDeLaSemana = diaSemana,
                                HoraInicioBloque = horaInicio,
                                HoraFinBloque = horaFin
                            };

                            DisponibilidadHorariaNegocio dhNegocio = new DisponibilidadHorariaNegocio();
                            dhNegocio.AgregarDisponibilidadHoraria(nuevaDisponibilidad);

                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-success d-block";
                            lblMensaje.Text = "Disponibilidad horaria agregada correctamente.";

                        }
                        catch (Exception ex)
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-danger d-block";
                            lblMensaje.Text = "Error al agregar disponibilidad: " + ex.Message;
                        }
                    }
                    else
                    {
                        lblMensaje.Visible = true;
                        lblMensaje.CssClass = "alert alert-warning d-block";
                        lblMensaje.Text = "Los parámetros de disponibilidad no son válidos.";
                    }

                    // Recargar médicos y sus disponibilidades después de la operación
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

                if (Request["eliminarDisponibilidad"] != null)
                {
                    int idDisponibilidadEliminar;
                    int idMedicoAsociado; // Para saber qué modal reabrir

                    if (int.TryParse(Request["eliminarDisponibilidad"], out idDisponibilidadEliminar) &&
                        int.TryParse(Request["idMedico"], out idMedicoAsociado))
                    {
                        try
                        {
                            DisponibilidadHorariaNegocio dhNegocio = new DisponibilidadHorariaNegocio();
                            dhNegocio.EliminarDisponibilidadHoraria(idDisponibilidadEliminar);

                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-success d-block";
                            lblMensaje.Text = "Disponibilidad horaria eliminada correctamente.";
                        }
                        catch (Exception ex)
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-danger d-block";
                            lblMensaje.Text = "Error al eliminar disponibilidad horaria: " + ex.Message;
                            
                        }

                        // Recargar en caso de error
                        CargarMedicos();
                    }
                    else
                    {
                        lblMensaje.Visible = true;
                        lblMensaje.CssClass = "alert alert-warning d-block";
                        lblMensaje.Text = "No se pudo determinar la disponibilidad a eliminar.";
                        CargarMedicos();
                    }
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
            DisponibilidadHorariaNegocio dhNegocio = new DisponibilidadHorariaNegocio();

            foreach (var medico in listaMedico)
            {
                medico.Especialidades = medicoNegocio.ListarEspecialidadesPorMedico(medico.Id);
                // Cargar las disponibilidades horarias para cada médico
                medico.Disponibilidades = dhNegocio.ListarPorMedico(medico.Id);
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