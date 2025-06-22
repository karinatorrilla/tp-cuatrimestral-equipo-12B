<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Especialidades.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Especialidades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Especialidades</h1>

        <%--Botón agregar especialidad para abrir el modal--%>

        <asp:Button ID="btnMostrarFormularioAgregar" runat="server" Text="Agregar Especialidad" CssClass="btn btn-success mb-4" OnClick="btnMostrarFormularioAgregar_Click" />

        <%--Tabla con listado de Especialidades--%>
        <div class="table-responsive d-flex justify-content-center">
            <div style="width: 50%;">
                <table class="table table-striped table-hover table-bordered align-middle">
                    <thead class="table-dark">
                        <tr>
                            <th class="text-center">NOMBRE</th>
                            <th class="text-center">ACCIONES</th>
                        </tr>
                    </thead>
                    <tbody>
                        <% if (listaEspecialidades != null && listaEspecialidades.Any())
                            { %>
                        <% foreach (var esp in listaEspecialidades)
                            { %>
                        <tr>
                            <td class="text-center"><%= esp.Descripcion %></td>

                            <td class="d-flex justify-content-center align-items-center gap-3">
                                <img src="images/icon_edit.svg" style="cursor: pointer" data-bs-toggle="modal"
                                    data-bs-target="#modificarModal_<%= esp.Id %>" />
                                <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" style="cursor: pointer"
                                    data-bs-toggle="modal" data-bs-target="#eliminarModal_<%= esp.Id %>" />
                            </td>
                            <!--Modal Modificar Especialidad-->
                            <div class="modal fade" id="modificarModal_<%= esp.Id %>" tabindex="-1" aria-labelledby="modificarLabel_<%= esp.Id %>" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <form method="post" action="Especialidades.aspx">
                                            <div class="modal-header">
                                                <h5 class="modal-title">Modificar Especialidad</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                            </div>
                                            <div class="modal-body">
                                                <input type="hidden" name="IdEspecialidad" value="<%= esp.Id %>" />
                                                <label>Nombre</label>
                                                <input type="text" name="DescripcionModificada" class="form-control" value="<%= esp.Descripcion %>" />
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                                <button type="submit" class="btn btn-success">Guardar Modificación</button>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                            <%--Modal Eliminar Especialidad--%>
                            <div class="modal fade" id="eliminarModal_<%= esp.Id %>" tabindex="-1" aria-labelledby="eliminarLabel_<%= esp.Id %>" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="eliminarLabel_<%= esp.Id %>">Confirmar Eliminación</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                        </div>
                                        <div class="modal-body">
                                            ¿Estás seguro que deseas eliminar la Especialidad:  
                                             <strong><%= esp.Descripcion%></strong>?         
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                            <a href="Especialidades.aspx?eliminar=<%= esp.Id %>" class="btn btn-danger">Eliminar</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
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






        <!-- Modal Agregar Especialidad -->
       <asp:Panel ID="formAgregar" runat="server" Visible="false" CssClass="card mt-4" Style="width: 50%; margin: auto;">
    <div class="card-header">
        Agregar Especialidad              
    </div>
    <div class="card-body">
        <div class="mb-3">
            <label for="txtNombreEspecialidad" class="form-label">Nombre de la especialidad:</label>
            <asp:TextBox ID="txtNombreEspecialidad" runat="server" CssClass="form-control" placeholder="Ej. Dentista"></asp:TextBox>
        </div>
        <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert alert-info d-block"></asp:Label>
        <asp:Button ID="AgregarEspecialidad" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="AgregarEspecialidad_Click" />
        <asp:Button ID="cerrarForm" runat="server" CssClass="btn btn-secondary" Text="Cerrar" OnClick="cerrarForm_Click" />
    </div>

</asp:Panel>
</asp:Content>
