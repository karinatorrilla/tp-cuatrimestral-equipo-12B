using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;
using dominio;
using System.Globalization;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Medicos : System.Web.UI.Page
    {
        private string quitarAcentos(string texto) //para poder obviar en el filtrado
        {
            return new string(texto.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .Normalize(NormalizationForm.FormC);
        }
        public string GenerarOpcionesHorario()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<option value=\"\" selected disabled>Seleccionar horario</option>");

            for (int hora = 0; hora < 24; hora++)
            {
                // Crea el string en formato HH:mm compatible con TimeSpan.Parse
                string horaStr = hora.ToString("00") + ":00";
                sb.AppendFormat("<option value=\"{0}\">{0}</option>", horaStr);
            }

            return sb.ToString();
        }


        public List<Medico> listaMedico;
        public List<Especialidad> todasLasEspecialidades;
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

                MedicosNegocio medicoNegocio = new MedicosNegocio();


                if (!IsPostBack)
                {
                    CargarEspecialidades();
                    CargarMedicos();
                }

                // --- Lógica para AGREGAR o ACTUALIZAR Disponibilidad ---
                if (Request["medicoId"] != null && Request["dia"] != null && Request["horaInicio"] != null && Request["horaFin"] != null)
                {
                    
                    int medicoId;
                    int diaSemana;
                    TimeSpan horaInicio;
                    TimeSpan horaFin;

                    // Intentar parsear todos los parámetros recibidos
                    if (int.TryParse(Request["medicoId"], out medicoId) &&
                        int.TryParse(Request["dia"], out diaSemana) &&
                        TimeSpan.TryParse(Request["horaInicio"], out horaInicio) &&
                        TimeSpan.TryParse(Request["horaFin"], out horaFin))
                    {
                        try
                        {
                            DisponibilidadHorariaNegocio dhNegocio = new DisponibilidadHorariaNegocio();
                            DisponibilidadHoraria disponibilidad = new DisponibilidadHoraria
                            {
                                MedicoId = medicoId,
                                DiaDeLaSemana = diaSemana,
                                HoraInicioBloque = horaInicio,
                                HoraFinBloque = horaFin
                            };

                            // Lógica CLAVE: Detectar si es ACTUALIZAR o AGREGAR 
                            // Verificamos si el parámetro 'actualizarDisponibilidad' existe en la URL
                            if (Request["actualizarDisponibilidad"] != null)
                            {
                                // Estamos en modo ACTUALIZAR
                                int idDisponibilidadAActualizar;
                                if (int.TryParse(Request["actualizarDisponibilidad"], out idDisponibilidadAActualizar))
                                {
                                    disponibilidad.Id = idDisponibilidadAActualizar; // Asigna el ID de la disponibilidad a modificar
                                    dhNegocio.ModificarDisponibilidadHoraria(disponibilidad); // Llama al método de modificación

                                    lblMensaje.Visible = true;
                                    lblMensaje.CssClass = "alert alert-success d-block";
                                    lblMensaje.Text = "Disponibilidad horaria actualizada correctamente.";
                                }
                                else
                                {
                                    // El ID de actualización no es válido
                                    lblMensaje.Visible = true;
                                    lblMensaje.CssClass = "alert alert-danger d-block";
                                    lblMensaje.Text = "Error: ID de disponibilidad para actualizar no válido.";
                                }
                            }
                            else
                            {
                                // No hay parámetro 'actualizarDisponibilidad', estamos en modo AGREGAR
                                dhNegocio.AgregarDisponibilidadHoraria(disponibilidad);

                                lblMensaje.Visible = true;
                                lblMensaje.CssClass = "alert alert-success d-block";
                                lblMensaje.Text = "Disponibilidad horaria agregada correctamente.";
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-danger d-block";
                            lblMensaje.Text = "Error en la operación de disponibilidad: " + ex.Message;
                        }
                    }
                    else
                    {
                        lblMensaje.Visible = true;
                        lblMensaje.CssClass = "alert alert-warning d-block";
                        lblMensaje.Text = "Los parámetros de disponibilidad no son válidos.";
                    }

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

                listaMedico = new List<Medico>();
                Session.Add("error", "Error al cargar el listado de médicos: " + ex.Message);
                Response.Redirect("Error.aspx", false);
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtrarPor = ddlFiltro.SelectedValue;
                string textoBusqueda = quitarAcentos(txtFiltro.Text.Trim().ToLower());

                CargarMedicos(); //inicializo lista con todos los médicos, especialidades y disponibilidad
                CargarEspecialidades();

                if (string.IsNullOrWhiteSpace(textoBusqueda)) //si el campo de busqueda esta vacío, recarga toda la grilla nuevamente
                {
                    CargarMedicos();
                    CargarEspecialidades();
                }

                List<Medico> filtrados = new List<Medico>(); //inicializo otra lista para guardar los filtrados

                switch (filtrarPor)
                {
                    case "Nombre":
                        filtrados = listaMedico.Where(m => m.Nombre != null && quitarAcentos(m.Nombre.ToLower()).Contains(textoBusqueda)).ToList();
                        break;
                    case "Apellido":
                        filtrados = listaMedico.Where(m => m.Apellido != null && quitarAcentos(m.Apellido.ToLower()).Contains(textoBusqueda)).ToList();
                        break;
                    case "Matricula":
                        filtrados = listaMedico.Where(m => m.Matricula.ToString().Contains(textoBusqueda)).ToList();
                        break;
                    case "Especialidad":
                        filtrados = listaMedico.Where(m => m.Especialidades != null && m.Especialidades.Any(esp => esp.Descripcion != null && quitarAcentos(esp.Descripcion.ToLower()).Contains(textoBusqueda))).ToList();
                        break;
                }

                listaMedico = filtrados;
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

            //se vuelven a cargar médicos con sus especialidades y disponibilidad
            CargarMedicos();
            CargarEspecialidades();
        }
    }
}