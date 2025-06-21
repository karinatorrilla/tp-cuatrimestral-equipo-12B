<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Especialidades.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Especialidades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">






    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Especialidades</h1>

        <div class="table-responsive">
            <table class="table table-striped table-hover table-bordered align-middle">
                <thead class="table-dark">
                    <tr>
                       <%-- <th>ID</th> --%>
                        <th>DESCRIPCIÓN</th>
                    </tr>
                </thead>
                <tbody>
                    <% if (listaEspecialidades != null && listaEspecialidades.Any())
                        { %>
                    <% foreach (var esp in listaEspecialidades)
                        { %>
                    <tr>
                        <%-- <td><%= esp.Id %></td> --%>
                        <td><%= esp.Descripcion %></td>
                    </tr>
                    <% } %>
                    <% }
                    else
                    { %>
                    <tr>
                        <td colspan="2" class="text-center py-4">No hay especialidades cargadas.</td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>




    <!-- Button agregar especialidad para abrir el modal -->
                        <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger" ></asp:Label>

    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#agregarEspecialidadModal">
        Agregar Especialidad
    </button>

    <!-- Modal agregar especialidad -->
    <div class="modal fade" id="agregarEspecialidadModal" tabindex="-1" aria-labelledby="agregarEspecialidadModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5" id="agregarEspecialidadModalLabel">Agregar Especialidad</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="txtNombreEspecialidad" class="form-label">Nombre de la especialidad:</label>

                        <asp:TextBox ID="txtNombreEspecialidad" required="true" runat="server" CssClass="form-control" placeholder="Ej. Dentista"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button ID="AgregarEspecialidad" runat="server" CssClass="btn btn-primary" Text="Guardar"
                        OnClick="AgregarEspecialidad_Click" />

                </div>
            </div>
        </div>
    </div>




</asp:Content>
