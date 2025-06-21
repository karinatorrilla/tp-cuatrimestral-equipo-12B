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
                listaEspecialidades = negocio.Listar();
                    
        }



        protected void AgregarEspecialidad_Click(object sender, EventArgs e)
        {
            string descripcion = txtNombreEspecialidad.Text.Trim();
           

            if (string.IsNullOrEmpty(descripcion))
            {
                ///mostrar mensaje de error
                lblMensaje.Text = "No puede ir vacio";
                return;
            }




            EspecialidadNegocio negocio = new EspecialidadNegocio();
            Especialidad nueva = new Especialidad();
            nueva.Descripcion = txtNombreEspecialidad.Text;

            negocio.agregarEspecialidad(nueva);
            txtNombreEspecialidad.Text = ""; //borramos la caja de texto despues de agregar

            listaEspecialidades=negocio.Listar(); // recargamos la lista
          
        }

        protected void EliminarEspecialidad_Click(object sender, EventArgs e)
        {

        }

        protected void ModificarEspecialidad_Click(object sender, EventArgs e)
        {

        }





    }
}