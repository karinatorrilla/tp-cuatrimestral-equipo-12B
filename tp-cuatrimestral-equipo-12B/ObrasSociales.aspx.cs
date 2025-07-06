using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class ObrasSociales : Page
    {
        public List<ObraSocial> listaObrasSociales;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ObraSocialNegocio negocio = new ObraSocialNegocio();
                if (!IsPostBack)
                {
                    listaObrasSociales = negocio.Listar();
                    Session["ListaObrasSociales"] = listaObrasSociales;
                }


                //ELIMINACION
                if (!IsPostBack && Request["eliminar"] != null)
                {
                    int idEliminar;
                    if (int.TryParse(Request["eliminar"], out idEliminar))
                    {
                        try
                        {
                            negocio.eliminarObra(idEliminar);
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-warning d-block";
                            lblMensaje.Text = "Obra Social Eliminada";
                        }
                        catch (Exception ex)
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-danger d-block";
                            lblMensaje.Text = ex.Message;
                        }
                       
                    }
                }
                //Habilitacion
                if (!IsPostBack && Request["habilitar"] != null)
                {
                    int idHabilitar;
                    if (int.TryParse(Request["habilitar"], out idHabilitar))
                    {
                        try
                        {
                            negocio.habilitarObraSocial(idHabilitar);
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-success d-block";
                            lblMensaje.Text = "Obra Social Habilitada";
                        }
                        catch (Exception ex)
                        {
                            lblMensaje.Visible = true;
                            lblMensaje.CssClass = "alert alert-danger d-block";
                            lblMensaje.Text = ex.Message;
                        }
                    }
                    Response.Redirect("ObrasSociales.aspx");
                }
                //MODIFICACIÓN
                string target = Request["__EVENTTARGET"];

                if (target == null && Request.Form["IdObraSocial"] != null && Request.Form["DescripcionModificada"] != null)
                {
                    int idObraSocial;
                    string nuevaDescripcion = Request.Form["DescripcionModificada"].Trim();

                    if (int.TryParse(Request.Form["IdObraSocial"], out idObraSocial) && !string.IsNullOrEmpty(nuevaDescripcion))
                    {
                        ObraSocial obraModificada = new ObraSocial();
                        List<ObraSocial> lista = negocio.Listar();

                        bool yaExiste = lista.Any(o => o.Descripcion.Trim().ToLower() == nuevaDescripcion.ToLower());

                        if (yaExiste)
                        {
                            lblMensaje.CssClass = "alert alert-warning d-block";
                            lblMensaje.Text = "Ya existe una obra social con ese nombre.";
                            lblMensaje.Visible = true;
                            listaObrasSociales = negocio.Listar();
                            ///////////ver donde podemos mostrar fuera del modal 

                            return;
                        }
                        else
                        {

                            obraModificada.Id = idObraSocial;
                            obraModificada.Descripcion = nuevaDescripcion;

                            bool resultado = negocio.modificarObraSocial(obraModificada);

                            if (!resultado)
                            {

                                lblMensaje.Text = "Ocurrió un error al modificar la obra social.";
                                ///////////ver donde podemos mostrar fuera del modal 
                            }
                            else
                            {
                                lblMensaje.Visible = true;
                                lblMensaje.CssClass = "alert alert-success d-block";
                                lblMensaje.Text = "Obra Social modificada";

                            }

                            listaObrasSociales = negocio.Listar();
                            Session["ListaObrasSociales"] = listaObrasSociales;
                        }
                    }
                }
                if (listaObrasSociales == null)
                    listaObrasSociales = (List<ObraSocial>)Session["ListaObrasSociales"];
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
            txtNombreObraSocial.Text = "";
            lblMensaje.Visible = false;
        }



        protected void AgregarObraSocial_Click(object sender, EventArgs e) // se pueden agregar mas validaciones !!!!!!!!!
        {
            string nombre = txtNombreObraSocial.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                lblMensaje.CssClass = "alert alert-danger d-block";
                lblMensaje.Text = "El nombre de la obra social no puede estar vacío.";
                lblMensaje.Visible = true;
                return;
            }

            ObraSocialNegocio negocio = new ObraSocialNegocio();
            List<ObraSocial> lista = negocio.Listar();

            bool yaExiste = lista.Any(o => o.Descripcion.Trim().ToLower() == nombre.ToLower());

            if (!Regex.IsMatch(txtNombreObraSocial.Text, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                lblMensaje.CssClass = "alert alert-warning d-block";
                lblMensaje.Text = "El nombre de la obra social solo puede contener letras.";
                lblMensaje.Visible = true;

                return;
            }

            if (yaExiste)
            {
                lblMensaje.CssClass = "alert alert-warning d-block";
                lblMensaje.Text = "Ya existe una obra social con ese nombre.";
                lblMensaje.Visible = true;

                return;
            }

            ObraSocial nuevo = new ObraSocial();
            nuevo.Descripcion = nombre;

            negocio.agregarObraSocial(nuevo);

            txtNombreObraSocial.Text = "";
            lblMensaje.Text = "Obra social agregada correctamente.";
            lblMensaje.CssClass = "alert alert-success d-block";
            lblMensaje.Visible = true;

            //Ocultamos el formulario
            formAgregar.Visible = false;

            // Cargamos la lista actualizada
            listaObrasSociales = negocio.Listar();
        }

        protected void btnBuscarObra_Click(object sender, EventArgs e)
        {
            try
            {
                string txtBusqueda = txtBuscarOS.Text.Trim().ToLower();
                List<ObraSocial> todas = (List<ObraSocial>)Session["ListaObrasSociales"];

                listaObrasSociales = todas
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
            txtBuscarOS.Text = "";
            listaObrasSociales = (List<ObraSocial>)Session["ListaObrasSociales"];
        }
        protected void chkVerDeshabilitadas_CheckedChanged(object sender, EventArgs e)
        {
            ObraSocialNegocio negocio = new ObraSocialNegocio();

            if (chkVerDeshabilitadas.Checked)
            {
                // Mostrar todas
                listaObrasSociales = negocio.Listar(true);
            }
            else
            {
                // Mostrar solo habilitadas
                listaObrasSociales = negocio.Listar();
            }

            Session["ListaObrasSociales"] = listaObrasSociales;
        }
    }


}
