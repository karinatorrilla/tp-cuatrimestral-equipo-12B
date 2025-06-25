<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" Async="true" AutoEventWireup="true" CodeBehind="FormularioPaciente.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.FormularioPaciente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <% 
            string modo = Request.QueryString["modo"];
            string id = Request.QueryString["id"];

            if (id == null) // Si no hay ID, es AGREGAR
            {
        %>
        <h1 class="mb-4">Agregar Paciente</h1>
        <%  }
            else if (modo == "ver") // Si hay ID y el modo es "ver", es para VER DETALLES
            {
        %>
        <h1 class="mb-4">Detalles del Paciente</h1>
        <%  }
            else // Si hay ID y el modo NO es "ver" o no hay modo, es para EDITAR
            {
        %>
        <h1 class="mb-4">Editar Paciente</h1>
        <%  }
        %>
    </div>
    <div id="divMensaje" runat="server" class="alert" visible="false"></div>

    <div class="p-4 rounded bg-white shadow-sm">
        <%-- Sección de Datos Personales --%>
        <h3 class="mb-4 text-dark">Datos Personales</h3>
        <div class="row align-items-end g-4 mb-4">

            <%-- Nombre --%>
            <div class="col-md-3">
                <label for="txtNombre" class="form-label font-weight-bold text-dark">Nombre</label>
                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="El nombre debe contener solo letras, sin espacios ni al principio ni al final." placeholder="Pedro" required="true" />
            </div>

            <%-- Apellido --%>
            <div class="col-md-3">
                <label for="txtApellido" class="form-label font-weight-bold text-dark">Apellido</label>
                <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="El apellido debe contener solo letras, sin espacios ni al principio ni al final." placeholder="Lopez" required="true" />
            </div>

            <%-- DNI --%>
            <div class="col-md-3">
                <label for="txtDni" class="form-label font-weight-bold text-dark">Documento</label>

                <asp:TextBox runat="server" ID="txtDni" CssClass="form-control" MaxLength="8" placeholder="12345678" required="true" TextMode="SingleLine" pattern="\d{1,8}"
                    title="El documento debe contener solo números y tener hasta 8 dígitos." />

            </div>

            <%-- Fecha de Nacimiento --%>
            <div class="col-md-3">
                <label for="txtFechaNacimiento" class="form-label font-weight-bold text-dark">Fecha de Nacimiento</label>
                <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" TextMode="Date" required="true" />
            </div>

        </div>

        <%-- Sección de Contacto  --%>
        <div class="row align-items-end g-4 mb-4">
            <%-- Email --%>
            <div class="col-md-3">
                <label for="txtEmail" class="form-label font-weight-bold text-dark">Email</label>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TextMode="Email" placeholder="nombre@gmail.com" required="true" />
            </div>

            <%-- Teléfono --%>
            <div class="col-md-3">
                <label for="txtTelefono" class="form-label font-weight-bold text-dark">Teléfono</label>
                <asp:TextBox runat="server" ID="txtTelefono" CssClass="form-control" pattern="\d{1,10}" MaxLength="10" placeholder="1123456789" required="true" title="Solo números, máximo 10 cifras" />
            </div>

            <%-- Nacionalidad --%>
            <div class="col-md-3">
                <label for="ddlNacionalidad" class="form-label font-weight-bold text-dark">Nacionalidad</label>
                <asp:DropDownList ID="ddlNacionalidad" runat="server" CssClass="form-control" AppendDataBoundItems="true" required="true">
                    <asp:ListItem Text="Seleccione Nacionalidad" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Obra Social (DropDownList) --%>
            <div class="col-md-3">
                <label for="ddlObraSocial" class="form-label font-weight-bold text-dark">Obra Social</label>
                <asp:DropDownList ID="ddlObraSocial" runat="server" CssClass="form-control" AppendDataBoundItems="true" required="true">
                    <asp:ListItem Text="Seleccione Obra Social" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

        </div>



        <%-- Sección domicilio --%>
        <h3 class="mb-4 text-dark">Datos de Domicilio</h3>
        <div class="row align-items-end g-5 mb-4">

            <%-- Provincia (DropDownList) --%>
            <div class="col-md-3">
                <label for="ddlProvincia" class="form-label font-weight-bold text-dark">Provincia</label>
                <asp:DropDownList ID="ddlProvincia" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" AppendDataBoundItems="true" required="true">
                    <asp:ListItem Text="Seleccione Provincia" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Localidad (DropDownList) --%>
            <div class="col-md-3">
                <label for="ddlLocalidad" class="form-label font-weight-bold text-dark">Localidad</label>
                <asp:DropDownList ID="ddlLocalidad" runat="server" CssClass="form-control" AppendDataBoundItems="true" required="true" Enabled="false">
                    <asp:ListItem Text="Seleccione Localidad" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Dirección/Calle --%>
            <div class="col-md-2">
                <label for="txtDireccion" class="form-label font-weight-bold text-dark">Calle</label>
                <asp:TextBox runat="server" ID="txtDireccion" CssClass="form-control" placeholder="Mendoza" required="true" />
            </div>

            <%-- Altura --%>
            <div class="col-md-1">
                <label for="txtAltura" class="form-label font-weight-bold text-dark">Altura</label>
                <asp:TextBox runat="server" ID="txtAltura" MaxLength="7" CssClass="form-control" placeholder="123" required="true" />
            </div>

            <%-- Cod Postal --%>
            <div class="col-md-1">
                <label for="txtCodPostal" class="form-label font-weight-bold text-dark">Cod. Postal</label>
                <asp:TextBox runat="server" ID="txtCodPostal" MaxLength="6" CssClass="form-control" placeholder="1614" required="true" />
            </div>

            <%-- Depto --%>
            <div class="col-md-1">
                <label for="txtDepto" class="form-label font-weight-bold text-dark">Depto(Opcional)</label>
                <asp:TextBox runat="server" ID="txtDepto" CssClass="form-control" placeholder="12B" />
            </div>
        </div>

        <%-- Sección Observaciones --%>
        <h3 class="mb-4 text-dark">Observaciones</h3>
        <div class="row g-3 mb-4">
            <div class="col-md-12">
                <label for="txtObservaciones" class="form-label font-weight-bold text-dark">Notas adicionales</label>
                <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" />
            </div>
        </div>

        <%-- Contenedor de Botones --%>
        <div class="d-flex justify-content-center mt-4">
            <asp:Button Text="Guardar" ID="btnGuardar" CssClass="btn btn-primary px-5 me-3" runat="server" OnClick="btnGuardar_Click" />
            <a href="Pacientes.aspx" class="btn btn-secondary px-5">Volver</a>
        </div>
    </div>


</asp:Content>
