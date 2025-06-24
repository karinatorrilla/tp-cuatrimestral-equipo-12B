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

                //ELIMINACION
                if (!IsPostBack && Request["eliminar"] != null)
                {
                    int idEliminar;
                    if (int.TryParse(Request["eliminar"], out idEliminar))
                    {
                        negocio.eliminarObra(idEliminar);
                    }
                }

                //MODIFICACIÓN
                if (Request.Form["IdObraSocial"] != null && Request.Form["DescripcionModificada"] != null)
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
                        }
                    }
                }
                listaObrasSociales = negocio.Listar();
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


    }


}
