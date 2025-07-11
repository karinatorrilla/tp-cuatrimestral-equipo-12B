﻿using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tp_cuatrimestral_equipo_12B
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["error"] != null)
            {
                lblMensaje.Text = Session["error"].ToString();
            }
            else
            {
                tituloError.InnerText = "Tienes que iniciar sesion";
            }
                   

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            if (Session["TipoUsuario"] == null)
            {
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                Response.Redirect("Inicio.aspx", false);
            }
        }
    }
}