<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="ObrasSociales.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.ObrasSociales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Obras Sociales</h1>

        <div class="table-responsive">
            <table class="table table-striped table-hover table-bordered align-middle">
                <thead class="table-dark">
                    <tr>
                        <th>NOMBRE</th>
                    </tr>
                </thead>
                <tbody>
                    <% if (listaObrasSociales != null && listaObrasSociales.Any())
                        { %>
                    <% foreach (var obra in listaObrasSociales)
                        { %>
                    <tr>
                        <td><%= obra.Descripcion %></td>
                    </tr>
                    <% } %>
                    <% }
                    else
                    { %>
                    <tr>
                        <td colspan="1" class="text-center py-4">No hay obras sociales cargadas.</td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>

    <!-- Label para mensajes -->
    <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger"></asp:Label>

    <!-- Botón para abrir modal -->
    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#agregarObraSocialModal">
        Agregar Obra Social
    </button>

    <!-- Modal para agregar obra social -->
    <div class="modal fade" id="agregarObraSocialModal" tabindex="-1" aria-labelledby="agregarObraSocialModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5" id="agregarObraSocialModalLabel">Agregar Obra Social</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="txtNombreObraSocial" class="form-label">Nombre de la obra social:</label>
                        <asp:TextBox ID="txtNombreObraSocial" required="true" runat="server" CssClass="form-control" placeholder="Ej. OSDE"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button ID="AgregarObraSocial" runat="server" CssClass="btn btn-primary" Text="Guardar"
                        OnClick="AgregarObraSocial_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
