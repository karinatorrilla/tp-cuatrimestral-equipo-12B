<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FormularioPaciente.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.FormularioPaciente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <h1 class="mb-4">Agregar paciente</h1>
    </div>
    <div id="divMensaje" runat="server" class="alert" visible="false"></div>

    <div class="p-4 rounded bg-white shadow-sm">
        <%-- Sección de Datos Personales --%>
        <div class="row align-items-end g-3 mb-4">

            <%-- Nombre --%>
            <div class="col-md-4">
                <label for="txtNombre" class="form-label font-weight-bold text-dark">Nombre</label>
                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="No puede contener numeros" placeholder="Pedro" required="true"/>
                      </div>

            <%-- Apellido --%>
            <div class="col-md-4">
                <label for="txtApellido" class="form-label font-weight-bold text-dark">Apellido</label>
                <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="No puede contener numeros" placeholder="Lopez" required="true" />
            </div>

            <%-- DNI --%>
            <div class="col-md-4">
                <label for="txtDni" class="form-label font-weight-bold text-dark">DNI</label>
                
              <asp:TextBox runat="server" ID="txtDni" CssClass="form-control" MaxLength="8" placeholder="12345678" required="true" TextMode="SingleLine" pattern="\d{1,8}" 
    title="El DNI debe tener entre 1 y 8 dígitos"  />

            </div>
          
        </div>

        <%-- Sección de Contacto  --%>
        <div class="row align-items-end g-3 mb-4">
            <%-- Email --%>
            <div class="col-md-4">
                <label for="txtEmail" class="form-label font-weight-bold text-dark">Email</label>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TextMode="Email" placeholder="nombre@gmail.com" required="true"/>
            </div>

            <%-- Fecha de Nacimiento --%>
            <div class="col-md-4">
                <label for="txtFechaNacimiento" class="form-label font-weight-bold text-dark">Fecha de Nacimiento</label>
                <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" TextMode="Date" required="true"/>
            </div>

            <%-- Dirección --%>
            <div class="col-md-4">
                <label for="txtDireccion" class="form-label font-weight-bold text-dark">Dirección</label>
                <asp:TextBox runat="server" ID="txtDireccion" CssClass="form-control" placeholder="Mendoza 123" required="true"/>
            </div>
        </div>

        <%-- Sección de Obra Social --%>
        <div class="row align-items-end g-3 mb-4">
          <%--  Obra social >
            <div class="col-md-4">
                <label for="txtObraSocial" class="form-label font-weight-bold text-dark">Obra Social</label>
                <asp:TextBox runat="server" ID="txtObraSocial" CssClass="form-control" placeholder="Galeno" required="true"/>
            </div>--%>

             <%-- Obra Social (DropDownList) --%>
 <div class="col-md-4">
     <label for="ddlObraSocial" class="form-label font-weight-bold text-dark">Obra Social</label>
     <asp:DropDownList ID="ddlObraSocial" runat="server" CssClass="form-control"></asp:DropDownList>
 </div>

        </div>

        <%-- Contenedor de Botones --%>
        <div class="d-flex justify-content-center mt-4">
            <asp:Button Text="Guardar" ID="btnGuardar" CssClass="btn btn-primary px-5 me-3" runat="server" OnClick="btnGuardar_Click" />
            <a href="Pacientes.aspx" class="btn btn-secondary px-5">Volver</a>
        </div>
    </div>


</asp:Content>
