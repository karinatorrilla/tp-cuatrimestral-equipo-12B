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







        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;
                EmailService email = new EmailService();
                Medico medico = new Medico();
                MedicosNegocio medicosNegocio = new MedicosNegocio();

                string correo = txtUsuario.Text;
                medico = medicosNegocio.buscarCorreo(correo);
                if (medico != null)
                {

                    string cuerpomail = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>Recuperación de Contraseña</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;\">\r\n  <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px;\">\r\n <h2>Hola " + medico.Nombre + " " + medico.Apellido + "</h2>   <tr>\r\n      <td align=\"center\" style=\"padding-bottom: 20px;\">\r\n        <h2 style=\"color: #333333;\">Recuperación de contraseña</h2>\r\n      </td>\r\n    </tr>\r\n    <tr>\r\n      <td style=\"color: #555555; font-size: 16px;\"> <p>Recibimos una solicitud para restablecer la contraseña de tu cuenta. Si vos hiciste esta solicitud, hacé clic en el siguiente botón:</p>\r\n\r\n        <p style=\"text-align: center; margin: 30px 0;\">\r\n          <a href=\"https://localhost:44318/RecuperarPassword.aspx\" style=\"background-color: #007BFF; color: #ffffff; padding: 12px 24px; text-decoration: none; border-radius: 5px;\">Restablecer contraseña</a>\r\n        </p>\r\n\r\n        <p>Si no solicitaste un cambio de contraseña, podés ignorar este correo.</p>\r\n        <p style=\"margin-top: 40px;\">Gracias,<br>El equipo de Clinica 12B</p>\r\n      </td>\r\n    </tr>\r\n    <tr>\r\n      <td align=\"center\" style=\"font-size: 12px; color: #aaaaaa; padding-top: 30px;\">\r\n        © 2025 Clinica 12B. Todos los derechos reservados.\r\n      </td>\r\n    </tr>\r\n  </table>\r\n</body>\r\n</html>\r\n";


                    email.armarCorreo(medico.Email, "Recuperar Password", cuerpomail);
                    email.enviarEmail();
                    lblError.Visible = true;
                    lblError.CssClass = "text-success";
                    lblError.Text = "Correo enviado correctamente";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.CssClass = "text-danger";
                    lblError.Text = "Si los datos ingresados son válidos, recibirás un correo para restablecer tu contraseña.";
                }


            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.CssClass = "text-danger";
                lblError.Text = "Si los datos ingresados son válidos, recibirás un correo para restablecer tu contraseña.";
            }


        }
    }
}