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
                if (Session["TipoUsuario"] == null)
                {
                    Session.Add("error", "Debes loguearte para ingresar.");
                    Response.Redirect("Error.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    MedicosNegocio negocio = new MedicosNegocio();
                    listaMedico = negocio.ListarMedicos();

                    // cargar datos en los selectores
                    EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
                    todasLasEspecialidades = especialidadNegocio.Listar();

                    // Cargar especialidades en el DropDownList
                    lstEspecialidades.DataSource = especialidadNegocio.Listar();
                    lstEspecialidades.DataValueField = "Id";
                    lstEspecialidades.DataTextField = "Descripcion";
                    lstEspecialidades.DataBind();

                    foreach (var medico in listaMedico)
                    {
                        // Llama ListarEspecialidadesPorMedico para traer las especialidades de un médico dado su ID
                        medico.Especialidades = negocio.ListarEspecialidadesPorMedico(medico.Id);
                    }
                }
                else //recargar los datos
                {
                    MedicosNegocio medicoNegocio = new MedicosNegocio();
                    listaMedico = medicoNegocio.ListarMedicos();
                    foreach (var medico in listaMedico)
                    {
                        medico.Especialidades = medicoNegocio.ListarEspecialidadesPorMedico(medico.Id);
                    }

                    EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
                    todasLasEspecialidades = especialidadNegocio.Listar();
                }

            }
            catch (Exception ex)
            {

                Session.Add("error", "Error al cargar el listado de médicos: " + ex.Message);
                listaMedico = new List<Medico>();
            }

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