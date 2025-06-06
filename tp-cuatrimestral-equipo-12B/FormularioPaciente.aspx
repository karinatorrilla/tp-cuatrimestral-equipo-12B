<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FormularioPaciente.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.FormularioPaciente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <h1 class="mb-4">Agregar paciente</h1>
    </div>
    <div id="divMensaje" runat="server" class="alert" visible="false"></div>

    <div class="p-4 rounded bg-white shadow-sm">
        <div class="row align-items-end g-3">

            <!-- Nombre -->
            <div class="col-md-2">
                <label for="txtNombre" class="form-label font-weight-bold text-dark">Nombre</label>
                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" placeholder="Pedro" />
            </div>

            <!-- Apellido -->
            <div class="col-md-2">
                <label for="txtApellido" class="form-label font-weight-bold text-dark">Apellido</label>
                <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" placeholder="Lopez" />
            </div>

            <!-- DNI -->
            <div class="col-md-2">
                <label for="txtDni" class="form-label font-weight-bold text-dark">DNI</label>
                <asp:TextBox runat="server" ID="txtDni" CssClass="form-control" MaxLength="8" TextMode="Number" placeholder="11222333" />
            </div>

            <!-- VALIDAR SOLO HASTA 8 DIGITOS DE MAXIMO   FALTA -->



            <!-- Email -->
            <div class="col-md-2">
                <label for="txtEmail" class="form-label font-weight-bold text-dark">Email</label>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TextMode="Email" placeholder="nombre@gmail.com" />
            </div>


            <!-- Fecha de Nacimiento -->
            <div class="col-md-2">
                <label for="Calendar1" class="form-label font-weight-bold text-dark">Fecha de Nacimiento</label>

                <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" placeholder="Mendoza 123" TextMode="Date" />
            </div>

            <!-- Dirección -->
            <div class="col-md-2">
                <label for="txtDireccion" class="form-label font-weight-bold text-dark">Dirección</label>
                <asp:TextBox runat="server" ID="txtDireccion" CssClass="form-control" placeholder="Mendoza 123" />
            </div>

            <!-- Obra social -->
            <div class="col-md-2">
                <label for="txtObraSocial" class="form-label font-weight-bold text-dark">Obra Social</label>
                <asp:TextBox runat="server" ID="txtObraSocial" CssClass="form-control" placeholder="Galeno" />
            </div>
        </div>
    </div>

    <!-- botones -->
    <div class="row mt-4">
        <div class="col-md-6 mb-2 text-center">
            <asp:Button Text="Guardar" ID="btnGuardar" CssClass="btn btn-primary w-50" runat="server" OnClick="btnGuardar_Click" />
        </div>
        <div class="col-md-6 mb-2 text-center">
            <a href="Pacientes.aspx" class="btn btn-secondary w-50">Volver</a>
        </div>
    </div>


</asp:Content>
