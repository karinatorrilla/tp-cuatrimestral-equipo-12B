<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="RecuperarPassword.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.RecuperarPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Recuperando password</h1>


    <div class="container h-100 d-flex justify-content-center align-items-center">
        <div class="col-12 col-md-6 col-lg-4 p-4 shadow-lg rounded bg-white">

            <h3 class="text-center mb-4">Login</h3>
            <div class="mb-3">
                <label for="txtUsuario" class="form-label">Usuario o email</label>
                <asp:TextBox runat="server" CssClass="form-control" ID="txtUsuario" />
            </div>
          

            <%-- Muestra label de error --%>
            <div class="text-center mt-3 mb-4">
                <asp:Label ID="lblError" runat="server" Text="Error al ingresar los datos, usuario o contraseña incorrecta" CssClass="text-danger" Visible="false" />
            </div>

            <div class="text-center d-grid gap-2">
                <asp:Button Text="Recuperar" CssClass="btn btn-primary btn-lg" ID="btnRecuperar" Onclick="btnRecuperar_Click" runat="server" />

            </div>
        </div>
    </div>

</asp:Content>
