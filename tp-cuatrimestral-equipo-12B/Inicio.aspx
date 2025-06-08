<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>¡Bienvenido!</h1>
    <asp:Label Text="text" ID="lblTipoUsuario" runat="server" />

    <%-- Menú de opciones --%>
    <hr />
    <div class="row row-cols-1 row-cols-md-3 g-4">
        <%if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString() != "Médico")
            { %>
        <%-- Administrar pacientes --%>
        <div class="col">
            <div class="card text-bg-success mb-3" style="width: 18rem;">
                <img src="images/adm_pacientes.jpg" class="card-img-top" alt="...">
                <div class="card-body">
                    <h5 class="card-title">Administrar pacientes</h5>
                    <p class="card-text">Navegar al menú para administrar todos los pacientes de la clínica.</p>
                    <asp:Button Text="Ir" ID="btnIrPacientes" OnClick="btnIrPacientes_Click" CssClass="btn btn-dark" runat="server" />
                </div>
            </div>
        </div>
        <%-- Administrar médicos --%>
        <div class="col">
            <div class="card text-bg-info mb-3" style="width: 18rem;">
                <img src="images/adm_medicos.jpg" class="card-img-top" alt="...">
                <div class="card-body">
                    <h5 class="card-title">Administrar médicos</h5>
                    <p class="card-text">Navegar al menú para administrar todos los médicos de la clínica.</p>
                    <asp:Button Text="Ir" ID="BtnIrMedicos" OnClick="BtnIrMedicos_Click" CssClass="btn btn-dark" runat="server" />
                </div>
            </div>
        </div>
        <% } %>
        <%if ( Session["TipoUsuario"] != null )
                if ( Session["TipoUsuario"].ToString() == "Recepcionista" || 
                    Session["TipoUsuario"].ToString() == "Médico" || 
                    Session["TipoUsuario"].ToString() == "Administrador" )
            { %>
        <%-- Adminstrar turnos --%>
        <div class="col">
            <div class="card text-bg-warning mb-3" style="width: 18rem;">
                <img src="images/adm_turnos.jpg" class="card-img-top" alt="...">
                <div class="card-body">                   
                    <h5 class="card-title">Administrar turnos</h5>
                    <p class="card-text">Navegar al menú para administrar turnos.</p>                    
                    <asp:Button Text="Ir" ID="btnIrTurnos" OnClick="btnIrTurnos_Click" CssClass="btn btn-dark" runat="server" />
                </div>
            </div>
        </div>
         <% } %>
    </div>
</asp:Content>
