<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>¡Hubo un problema!</h1>
    <asp:Label Text="text" ID="lblMensaje" runat="server" />
    <div class="col-md-4">
        <hr />
        <asp:Button Text="Volver" ID="btnVolver" CssClass="btn btn-primary px-5 me-3" OnClick="btnVolver_Click" runat="server" />
    </div>
</asp:Content>
