using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                if (!IsPostBack && Request["eliminar"] != null)
                {
                    int idEliminar;
                    if (int.TryParse(Request["eliminar"], out idEliminar))
                    {
                       
                        negocio.eliminarObra(idEliminar);
                    }
                }
           

            if (!IsPostBack)
            {
                if (Request.Form["IdObraSocial"] != null && Request.Form["DescripcionModificada"] != null)
                {
                    int idObraSocial;
                    string nuevaDescripcion = Request.Form["DescripcionModificada"].Trim();

                    if (int.TryParse(Request.Form["IdObraSocial"], out idObraSocial) && !string.IsNullOrEmpty(nuevaDescripcion))
                    {
                        ObraSocial obraModificada = new ObraSocial();
                        obraModificada.Id = idObraSocial;
                        obraModificada.Descripcion= nuevaDescripcion;
                        // Llamar a una función para modificar en la base de datos
                        bool resultado = negocio.modificarObraSocial(obraModificada);

                        if (resultado)
                        {
                           //recargamos la lista con la modificacion
                            listaObrasSociales = negocio.Listar();
                        }
                        else
                        {
                            lblMensaje.Text = "Ocurrió un error al modificar la obra social.";
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
            //listaObrasSociales = negocio.Listar();
        }


    }


}
