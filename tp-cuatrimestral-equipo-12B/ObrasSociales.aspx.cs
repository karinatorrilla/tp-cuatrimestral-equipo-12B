using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class ObrasSociales : Page
    {

        public List<ObraSocial> listaObrasSociales;

        protected void Page_Load(object sender, EventArgs e)
        {
            
                ObraSocialNegocio negocio = new ObraSocialNegocio();
                listaObrasSociales = negocio.Listar();
            
        }


        protected void AgregarObraSocial_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreObraSocial.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                lblMensaje.Text = "El nombre de la obra social no puede estar vacío.";
                return;
            }

            ObraSocialNegocio negocio = new ObraSocialNegocio();
            ObraSocial nuevo = new ObraSocial();
            nuevo.Descripcion = nombre;
            negocio.agregarObraSocial(nuevo);
            txtNombreObraSocial.Text = "";
            listaObrasSociales=negocio.Listar();

        }


    }


}
