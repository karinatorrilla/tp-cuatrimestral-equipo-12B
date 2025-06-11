<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FormularioMedico.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.FormularioMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <% if (Request.QueryString["id"] != null)
            { %>
        <h1 class="mb-4">Editar médico</h1>
        <%} %>
        <%else
            { %>
        <h1 class="mb-4">Agregar médico</h1>
        <%} %>
    </div>
    <div id="divMensaje" runat="server" class="alert" visible="false"></div>

    <div class="p-4 rounded bg-white shadow-sm">
        <%-- Sección de Datos Personales --%>
        <div class="row align-items-end g-3 mb-4">

            <%-- Nombre --%>
            <div class="col-md-4">
                <label for="txtNombre" class="form-label font-weight-bold text-dark">Nombre</label>
                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" placeholder="Dr. Juan" />
            </div>

            <%-- Apellido --%>
            <div class="col-md-4">
                <label for="txtApellido" class="form-label font-weight-bold text-dark">Apellido</label>
                <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" placeholder="García" />
            </div>

            <%-- DNI --%>
            <div class="col-md-4">
                <label for="txtDni" class="form-label font-weight-bold text-dark">DNI</label>
                <asp:TextBox runat="server" ID="txtDni" CssClass="form-control" MaxLength="8" TextMode="Number" placeholder="22333444" />
            </div>
        </div>

        <%-- Sección de Contacto--%>
        <div class="row align-items-end g-3 mb-4">
            <%-- Email --%>
            <div class="col-md-4">
                <label for="txtEmail" class="form-label font-weight-bold text-dark">Email</label>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TextMode="Email" placeholder="doctor.juan@clinica.com" />
            </div>

            <%-- Teléfono --%>
            <div class="col-md-4">
                <label for="txtTelefono" class="form-label font-weight-bold text-dark">Teléfono</label>
                <asp:TextBox runat="server" ID="txtTelefono" CssClass="form-control" placeholder="1122334455" TextMode="Number" />
            </div>

            <%-- Fecha de Nacimiento --%>
            <div class="col-md-4">
                <label for="txtFechaNacimiento" class="form-label font-weight-bold text-dark">Fecha de Nacimiento</label>
                <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" TextMode="Date" />
            </div>
        </div>

        <%-- Sección de Datos Profesionales --%>
        <div class="row align-items-end g-3 mb-4">
            <%-- Dirección --%>
            <div class="col-md-4">
                <label for="txtDireccion" class="form-label font-weight-bold text-dark">Dirección</label>
                <asp:TextBox runat="server" ID="txtDireccion" CssClass="form-control" placeholder="Av. Siempre Viva 742" />
            </div>

            <%-- Matrícula --%>
            <div class="col-md-4">
                <label for="txtMatricula" class="form-label font-weight-bold text-dark">Matrícula</label>
                <asp:TextBox runat="server" ID="txtMatricula" CssClass="form-control" placeholder="1000" />
            </div>

            <%-- Especialidad (DropDownList) --%>
            <div class="col-md-4">
                <label for="ddlEspecialidad" class="form-label font-weight-bold text-dark">Especialidad</label>
                <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <%-- Contenedor de Botones --%>
        <div class="d-flex justify-content-center mt-4">
            <asp:Button Text="Guardar" ID="btnGuardar" CssClass="btn btn-primary px-5 me-3" runat="server" OnClick="btnGuardar_Click" />
            <a href="Medicos.aspx" class="btn btn-secondary px-5">Volver</a>
        </div>
    </div>
</asp:Content>
