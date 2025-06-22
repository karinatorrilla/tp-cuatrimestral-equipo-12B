using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Especialidades : System.Web.UI.Page
    {
        public List<Especialidad> listaEspecialidades;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EspecialidadNegocio negocio = new EspecialidadNegocio();

                //ELIMINACIÓN
                if (!IsPostBack && Request["eliminar"] != null)
                {
                    int idEliminar;
                    if (int.TryParse(Request["eliminar"], out idEliminar))
                    {
                        negocio.eliminarEspecialidad(idEliminar);
                    }
                }

                //MODIFICACIÓN
                if (Request.Form["IdEspecialidad"] != null && Request.Form["DescripcionModificada"] != null)
                {
                    int idEspecialidad;
                    string nuevaDescripcion = Request.Form["DescripcionModificada"].Trim();

                    if (int.TryParse(Request.Form["IdEspecialidad"], out idEspecialidad) && !string.IsNullOrEmpty(nuevaDescripcion))
                    {
                        Especialidad especialidadModificada = new Especialidad();
                        List<Especialidad> lista = negocio.Listar();

                        bool yaExiste = lista.Any(o => o.Descripcion.Trim().ToLower() == nuevaDescripcion.ToLower());
                        if (yaExiste)
                        {
                            lblMensaje.CssClass = "alert alert-warning d-block";
                            lblMensaje.Text = "Ya existe una especialidad con ese nombre.";
                            lblMensaje.Visible = true;
                            listaEspecialidades = negocio.Listar();
                            return;
                            ///////////ver donde podemos mostrar fuera del modal 
                        }
                        else
                        {


                            especialidadModificada.Id = idEspecialidad;
                            especialidadModificada.Descripcion = nuevaDescripcion;

                            bool resultado = negocio.modificarEspecialidad(especialidadModificada);

                            if (!resultado)
                            {
                                lblMensaje.Text = "Ocurrió un error al modificar la especialidad.";
                                ///////////ver donde podemos mostrar fuera del modal 
                            }
                        }
                    }
                }

                listaEspecialidades = negocio.Listar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        protected void btnMostrarFormularioAgregar_Click(object sender, EventArgs e)
        {
            formAgregar.Visible = true;
            lblMensaje.Visible = false;
        }

        protected void cerrarForm_Click(object sender, EventArgs e)
        {
            formAgregar.Visible = false;
        }



        protected void AgregarEspecialidad_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreEspecialidad.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                lblMensaje.CssClass = "alert alert-danger d-block";
                lblMensaje.Text = "El nombre de la especialidad no puede estar vacío.";
                lblMensaje.Visible = true;
                return;
            }

            EspecialidadNegocio negocio = new EspecialidadNegocio();
            List<Especialidad> lista = negocio.Listar();

            bool yaExiste = lista.Any(o => o.Descripcion.Trim().ToLower() == nombre.ToLower());

            if (yaExiste)
            {
                lblMensaje.CssClass = "alert alert-warning d-block";
                lblMensaje.Text = "Ya existe una especialidad con ese nombre.";
                lblMensaje.Visible = true;

                return;
            }

            Especialidad nuevo = new Especialidad();
            nuevo.Descripcion = nombre;

            negocio.agregarEspecialidad(nuevo);

            txtNombreEspecialidad.Text = "";
            lblMensaje.Text = "Especialidad agregada correctamente.";
            lblMensaje.CssClass = "alert alert-success d-block";
            lblMensaje.Visible = true;

            //Ocultamos el formulario
            formAgregar.Visible = false;

            // Cargamos la lista actualizada
            listaEspecialidades = negocio.Listar();



        }
    }
}