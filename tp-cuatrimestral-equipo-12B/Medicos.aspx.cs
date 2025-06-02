using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Medicos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["TipoUsuario"] != null)
            {
                lblPermisos.Text = "Bienvenido tu rol es : "+Session["TipoUsuario"];
                //lblPermisos.Text = Session["TipoUsuario"].ToString();

            }
            else
            {

                lblPermisos.Text = "Usuario o password incorrectos";

            }




        }
    }
}