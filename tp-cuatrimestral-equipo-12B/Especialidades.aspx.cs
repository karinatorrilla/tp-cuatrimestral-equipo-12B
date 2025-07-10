using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Especialidades : System.Web.UI.Page
    {
        public List<Especialidad> listaEspecialidades;

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

                EspecialidadNegocio negocio = new EspecialidadNegocio();

                if (!IsPostBack)
                {
                    if (Session["ListaEspecialidades"] == null)
                    {
                        listaEspecialidades = negocio.Listar();
                        Session["ListaEspecialidades"] = listaEspecialidades;
                    }
                    else
                    {
                        listaEspecialidades = (List<Especialidad>)Session["ListaEspecialidades"];
                    }
                }

                //ELIMINACIÓN
                if (!IsPostBack && Request["eliminar"] != null)
                {
                    int idEliminar;
                    if (int.TryParse(Request["eliminar"], out idEliminar))
                    {
                        try
                        {
                            negocio.eliminarEspecialidad(idEliminar);
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-warning d-block";
                            lblMensaje.Text = "Especialidad Eliminada";

                            listaEspecialidades = negocio.Listar();
                            Session["ListaEspecialidades"] = listaEspecialidades;
                        }
                        catch (Exception ex)
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-danger d-block";
                            lblMensaje.Text = ex.Message;
                        }
                    }
                }

                //HABILITACIÓN
                if (!IsPostBack && Request["habilitar"] != null)
                {
                    int idHabilitar;
                    if (int.TryParse(Request["habilitar"], out idHabilitar))
                    {
                        try
                        {
                            string descripcion = Request["descripcion"];
                            negocio.habilitarEspecialidad(idHabilitar, descripcion);
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-success d-block";
                            lblMensaje.Text = "Especialidad Habilitada";

                            listaEspecialidades = negocio.Listar();
                            Session["ListaEspecialidades"] = listaEspecialidades;
                        }
                        catch (Exception ex)
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-danger d-block";
                            lblMensaje.Text = ex.Message;
                        }
                    }
                }

                //MODIFICACIÓN
                string target = Request["__EVENTTARGET"];
                if (target == null && Request.Form["IdEspecialidad"] != null && Request.Form["DescripcionModificada"] != null)
                {
                    int idEspecialidad;
                    string nuevaDescripcion = Request.Form["DescripcionModificada"].Trim();

                    if (int.TryParse(Request.Form["IdEspecialidad"], out idEspecialidad) && !string.IsNullOrEmpty(nuevaDescripcion))
                    {
                        Especialidad especialidadModificada = new Especialidad();
                        List<Especialidad> lista = (List<Especialidad>)Session["ListaEspecialidades"];

                        bool yaExiste = lista.Any(o => o.Descripcion.Trim().ToLower() == nuevaDescripcion.ToLower());
                        if (yaExiste)
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-warning d-block";
                            lblMensaje.Text = "Ya existe una especialidad con ese nombre.";
                            return;
                        }

                        especialidadModificada.Id = idEspecialidad;
                        especialidadModificada.Descripcion = nuevaDescripcion;

                        bool resultado = negocio.modificarEspecialidad(especialidadModificada);

                        if (!resultado)
                        {
                            lblMensaje.Text = "Ocurrió un error al modificar la especialidad.";
                        }
                        else
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-success d-block";
                            lblMensaje.Text = "Especialidad modificada";

                            listaEspecialidades = negocio.Listar();
                            Session["ListaEspecialidades"] = listaEspecialidades;
                        }
                    }
                }

                if (listaEspecialidades == null)
                    listaEspecialidades = (List<Especialidad>)Session["ListaEspecialidades"];
            }
            catch (Exception ex)
            {
                lblMensaje.Visible = true;
                lblMensaje.CssClass = "alert alert-danger d-block";
                lblMensaje.Text = "Error general: " + ex.Message;
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
            txtNombreEspecialidad.Text = "";
            lblMensaje.Visible = false;
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

            if (!Regex.IsMatch(txtNombreEspecialidad.Text, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                lblMensaje.CssClass = "alert alert-warning d-block";
                lblMensaje.Text = "El nombre de la especialidad solo puede contener letras.";
                lblMensaje.Visible = true;

                return;
            }

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

        protected void btnBuscarEspecialidad_Click(object sender, EventArgs e)
        {
            try
            {
                string txtBusqueda = txtBuscarEsp.Text.Trim().ToLower();

                List<Especialidad> todas = (List<Especialidad>)Session["ListaEspecialidades"];

                listaEspecialidades = todas
                    .Where(es => es.Descripcion.ToLower().Contains(txtBusqueda))
                    .ToList();
                lblMensaje.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscarEsp.Text = "";
            listaEspecialidades = (List<Especialidad>)Session["ListaEspecialidades"];
        }

        protected void chkVerDeshabilitadas_CheckedChanged(object sender, EventArgs e)
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            lblMensaje.Visible = false;

            if (chkVerDeshabilitadas.Checked)
            {
                // Mostrar todas
                listaEspecialidades = negocio.Listar(true);
            }
            else
            {
                // Mostrar solo habilitadas
                listaEspecialidades = negocio.Listar();
            }

            Session["ListaEspecialidades"] = listaEspecialidades;
        }
    }
}
