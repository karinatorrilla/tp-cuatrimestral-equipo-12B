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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Usuario usuario;
            UsuarioNegocio negocio = new UsuarioNegocio();

            try
            {
                usuario = new Usuario(txtUsuario.Text, txtPassword.Text);


                if (negocio.Loguear(usuario))
                {
                    Session.Add("Usuario", usuario);
                    Session.Add("TipoUsuario", usuario.TipoUsuario);

                    if (usuario.IDMedico > 0)
                    {
                        Session.Add("IDMedico", usuario.IDMedico);

                        MedicosNegocio medicoNegocio = new MedicosNegocio();
                        Medico medico = medicoNegocio.ListarMedicos(usuario.IDMedico).FirstOrDefault();

                        if (medico != null)
                            Session["NombreMedico"] = medico.Nombre + " " + medico.Apellido;
                    }

                    lblErrorLogin.Visible = false;
                    Response.Redirect("Inicio.aspx", false);
                }
                else
                {
                    lblErrorLogin.Visible = true;
                    Session.Add("error", "Usuario o contraseña incorrectos. Intentelo nuevamente.");

                }



            }
            catch (Exception ex)
            {
                lblErrorLogin.Text = "Ocurrió un error inesperado. Por favor, intente de nuevamente más tarde.";
                lblErrorLogin.Visible = true;
                Session.Add("error", ex.ToString());
            }




        }


       
    }
}