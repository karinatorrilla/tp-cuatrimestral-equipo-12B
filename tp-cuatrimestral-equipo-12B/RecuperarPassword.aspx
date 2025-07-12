<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="RecuperarPassword.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.RecuperarPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <%--Panel para recuperar contraseña, no hay token--%>
    <asp:Panel ID="panelRecuperarPass" Visible="false" runat="server">
        <h2>Recuperar Contraseña</h2>
        <div class="container vh-100 d-flex justify-content-center align-items-center">
            <div class="col-12 col-md-6 col-lg-4 p-4 shadow-lg rounded bg-white">

                <h3 class="text-center mb-4">Login</h3>
                <div class="mb-3">
                    <label for="txtUsuario" class="form-label">Usuario o email</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtUsuario" />
                </div>


                <%-- Muestra label de error --%>
                <div class="text-center mt-3 mb-4">
                    <asp:Label ID="lblError" runat="server" Text="Error al ingresar los datos, usuario o contraseña incorrecta" CssClass="text-success" Visible="false" />
                </div>

                <div class="text-center d-grid gap-2">
                    <asp:Button Text="Recuperar" CssClass="btn btn-primary btn-lg" ID="btnRecuperar" OnClick="btnRecuperar_Click" runat="server" />

                </div>
            </div>
        </div>
    </asp:Panel>


    <%--Panel para cambiar contraseña, hay un token--%>
    <asp:Panel ID="panelCambiarContraseña" Visible="false" runat="server">
        <h2>Cambiar contraseña</h2>
        <div class="container vh-100 d-flex justify-content-center align-items-center">
            <div class="col-12 col-md-6 col-lg-4 p-4 shadow-lg rounded bg-white">

                <h3 class="text-center mb-4">Login</h3>
                <div class="mb-3">
                    <label for="txtUsuario" class="form-label">Usuario o email</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtUsuarioRecupero" />
                </div>
                <div class="mb-3">
                    <label for="txtContraseña" class="form-label">Contraseña nueva</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtContraseñaNueva" TextMode="Password" />
                </div>
                <div class="mb-3">
                    <label for="txtContraseñaConfirmacion" class="form-label">Confirmar Contraseña</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtContraseñaNuevaConfirmada" TextMode="Password" />
                </div>


                <%-- Muestra label de error --%>
                <div class="text-center mt-3 mb-4">
                    <asp:Label ID="Label1" runat="server" Text="Error al ingresar los datos, usuario o contraseña incorrecta" CssClass="text-success" Visible="false" />
                </div>

                <div class="text-center d-grid gap-2">
                    <asp:Button Text="Guardar contraseña" CssClass="btn btn-primary btn-lg" ID="btnGuardarNuevaContraseña" OnClick="btnGuardarNuevaContraseña_Click" runat="server" />

                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
