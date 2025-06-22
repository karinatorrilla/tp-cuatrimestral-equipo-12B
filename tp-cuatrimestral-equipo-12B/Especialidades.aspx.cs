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
            EspecialidadNegocio negocio = new EspecialidadNegocio();

            //MODIFICACIÓN
            if (!IsPostBack)
            {
                if (Request.Form["IdEspecialidad"] != null && Request.Form["DescripcionModificada"] != null)
                {
                    int idEspecialidad;
                    string nuevaDescripcion = Request.Form["DescripcionModificada"].Trim();

                    if (int.TryParse(Request.Form["IdEspecialidad"], out idEspecialidad) && !string.IsNullOrEmpty(nuevaDescripcion))
                    {
                        Especialidad especialidadModificada = new Especialidad();
                        especialidadModificada.Id = idEspecialidad;
                        especialidadModificada.Descripcion = nuevaDescripcion;
                        // Llamar a una función para modificar en la base de datos
                        bool resultado = negocio.modificarEspecialidad(especialidadModificada);

                        if (resultado)
                        {

                            Response.Redirect("Especialidades.aspx");
                        }
                        else
                        {
                            lblMensaje.Text = "Ocurrió un error al modificar la especialidad.";
                        }
                    }
                }
            }

            //ELIMINACIÓN
            if (!IsPostBack && Request["eliminar"] != null)
            {
                int idEliminar;
                if (int.TryParse(Request["eliminar"], out idEliminar))
                {

                    negocio.eliminarEspecialidad(idEliminar);
                }
            }


            listaEspecialidades = negocio.Listar();


        }



        protected void AgregarEspecialidad_Click(object sender, EventArgs e)
        {

            string descripcion = txtNombreEspecialidad.Text.Trim();

            if (string.IsNullOrEmpty(descripcion))
            {
                ///mostrar mensaje de error
                lblMensaje.Text = "¡Complete el campo!";
                return;
            }

            EspecialidadNegocio negocio = new EspecialidadNegocio();
            Especialidad nueva = new Especialidad();

            nueva.Descripcion = txtNombreEspecialidad.Text;
            negocio.agregarEspecialidad(nueva);

            txtNombreEspecialidad.Text = ""; //borramos la caja de texto despues de agregar

            listaEspecialidades = negocio.Listar(); // recargamos la lista

        }


    }
}