using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TipoUsuario"] != null)
            {

                lblTipoUsuario.Text = Session["TipoUsuario"].ToString();

            }
            else
            {
                lblTipoUsuario.Text = "Usuario o password incorrectos";
            }


        }
    }
}