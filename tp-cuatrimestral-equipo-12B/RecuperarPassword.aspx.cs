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
    public partial class RecuperarPassword : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["token"] != null)
                {
                    /*Verificamos si al llegar a la pagina hay un token, mostramos panel para cambiar contraseña */
                    panelCambiarContraseña.Visible = true;

                    //Obtenemos el token 
                    string token = Request.QueryString["token"];
                    RecuperacionPass recuperacion = new RecuperacionPass();

                    //Verificamos que el token este asociado a un medico y sea válido ( no mas de 30 min de creacion)
                    int idmedico = recuperacion.verificarToken(token);
                    if (idmedico > 0)
                    {
                        Session.Add("idmedico", idmedico);//Si se encuentra lo guardamos en session
                        recuperacion.inhabilitarToken(token);//Inhabilitamos el token
                    }
                    else
                    {
                        lblError.Visible = true;

                        lblError.Text = "Error el link es invalido, solicite uno nuevo ";
                    }
                }
                else
                {
                    panelRecuperarPass.Visible = true; // si no hay token, mostramos panel para recuperar contraseña.
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {//Recuperar contraseña
            try
            {
                lblError.Visible = false;
                EmailService email = new EmailService();
                Medico medico = new Medico();
                MedicosNegocio medicosNegocio = new MedicosNegocio();
                RecuperacionPass recuperacion = new RecuperacionPass();

                string correoUsuario = txtUsuario.Text;//Guardamos el usuario o mail ingresado
                medico = medicosNegocio.buscarCorreoUsuario(correoUsuario); // Verificamos si existe

                if (medico != null && !string.IsNullOrEmpty(medico.Email))
                {//Si se encuentra
                    string token = Guid.NewGuid().ToString(); //Creamos un token de seguridad para ese medico(30min de validez)
                    string linkRecuperacion = $"https://localhost:44318/RecuperarPassword.aspx?token={token}";//creamos el link con token

                    recuperacion.guardarTokenRecuperacion(medico.Id, token);//guardamos el token con el idmedico asociado

                    string cuerpomail = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>Recuperación de Contraseña</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;\">\r\n  <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px;\">\r\n <h2>Hola " + medico.Nombre + " " + medico.Apellido + "</h2>   <tr>\r\n      <td align=\"center\" style=\"padding-bottom: 20px;\">\r\n        <h2 style=\"color: #333333;\">Recuperación de contraseña</h2>\r\n      </td>\r\n    </tr>\r\n    <tr>\r\n      <td style=\"color: #555555; font-size: 16px;\"> <p>Recibimos una solicitud para restablecer la contraseña de tu cuenta. Si vos hiciste esta solicitud, hacé clic en el siguiente botón:</p>\r\n\r\n        <p style=\"text-align: center; margin: 30px 0;\">\r\n          <a href=\"" + linkRecuperacion + "\" style=\"background-color: #007BFF; color: #ffffff; padding: 12px 24px; text-decoration: none; border-radius: 5px;\">Restablecer contraseña</a>\r\n        </p>\r\n\r\n        <p>Si no solicitaste un cambio de contraseña, podés ignorar este correo.</p>\r\n        <p style=\"margin-top: 40px;\">Gracias,<br>El equipo de Clinica 12B</p>\r\n      </td>\r\n    </tr>\r\n    <tr>\r\n      <td align=\"center\" style=\"font-size: 12px; color: #aaaaaa; padding-top: 30px;\">\r\n        © 2025 Clinica 12B. Todos los derechos reservados.\r\n      </td>\r\n    </tr>\r\n  </table>\r\n</body>\r\n</html>\r\n";


                    email.armarCorreo(medico.Email, "Recuperación de contraseña", cuerpomail);
                    email.enviarEmail();
                    lblError.Visible = true;
                    lblError.Text = "Si los datos son válidos, recibirás un correo para restablecer tu contraseña.";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Si los datos son válidos, recibirás un correo para restablecer tu contraseña.";
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", "Error : " + ex.Message);
                Response.Redirect("Error.aspx", false); ;
            }
        }

        protected void btnGuardarNuevaContraseña_Click(object sender, EventArgs e)
        {//Cambio de contraseña
            try
            {

                if (txtContraseñaNueva.Text == txtContraseñaNuevaConfirmada.Text)
                {
                    lblError.Visible = false;
                    string contraseñaNueva = txtContraseñaNueva.Text;
                    RecuperacionPass recuperacion = new RecuperacionPass();
                    recuperacion.cambiarContraseña(contraseñaNueva, (int)Session["idmedico"]);
                    lblError.Visible = true;
                    lblError.Text = "Contraseña cambiada correctamente";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Verifique los campos contraseña, tienen que ser iguales";
                }


            }
            catch (Exception ex)
            {

                Session.Add("error", "Error al cargar la pagina: " + ex.Message);
                Response.Redirect("Error.aspx", false);
            }


        }
    }
}