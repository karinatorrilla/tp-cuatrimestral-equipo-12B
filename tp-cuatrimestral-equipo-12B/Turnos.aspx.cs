using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tp_cuatrimestral_equipo_12B
{

    public partial class Turnos : System.Web.UI.Page
    {
        public List<Turno> listaTurno;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                /*Solo puede ver esta pagina un usuario que esté logueado*/
                if (Session["TipoUsuario"] == null)
                {
                    Session.Add("error", "Debes loguearte para ingresar.");
                    Response.Redirect("Error.aspx", false);
                    return;
                }

                /*Solo puede ver esta pagina un usuario que esté logueado*/

                if (!IsPostBack)
                {
                    TurnosNegocio negocio = new TurnosNegocio();

                    //si hay médico logueado, cargar solo sus turnos
                    if (Session["IDMedico"] != null)
                    {
                        int idMedico = (int)Session["IDMedico"];
                        listaTurno = negocio.ListarTurnos(idMedico);
                    }
                    else //sino cargo todos los turnos (admin o recepcionista)
                    {                    
                        listaTurno = negocio.ListarTurnos();
                    }
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