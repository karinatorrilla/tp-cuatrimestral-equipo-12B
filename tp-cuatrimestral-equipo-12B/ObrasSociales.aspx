<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="ObrasSociales.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.ObrasSociales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Obras Sociales</h1>

        <%--Botón para mostrar el formulario de agregar--%>
        <asp:Button ID="btnMostrarFormularioAgregar" runat="server" Text="Agregar Obra Social" CssClass="btn btn-success mb-4" OnClick="btnMostrarFormularioAgregar_Click" />



        <%--Tabla con listado de obras sociales--%>
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
                        <% if (listaObrasSociales != null && listaObrasSociales.Any())
                            { %>
                        <% foreach (var obra in listaObrasSociales)
                            { %>
                        <tr>

                            <td class="text-center"><%= obra.Descripcion %></td>
                            <td>
                                <img src="images/icon_edit.svg" style="cursor: pointer" data-bs-toggle="modal"
                                    data-bs-target="#modificarModal_<%= obra.Id %>" />
                                <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" style="cursor: pointer"
                                    data-bs-toggle="modal" data-bs-target="#eliminarModal_<%= obra.Id %>" />
                            </td>

                            <!-- Modal dentro del foreach -->
                            <div class="modal fade" id="modificarModal_<%= obra.Id %>" tabindex="-1">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <form method="post" action="ObrasSociales.aspx">
                                            <div class="modal-header">
                                                <h5 class="modal-title">Modificar Obra Social</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                            </div>
                                            <div class="modal-body">
                                                <input type="hidden" name="IdObraSocial" value="<%= obra.Id %>" />
                                                <label>Nombre</label>
                                                <input type="text" name="DescripcionModificada" class="form-control" value="<%= obra.Descripcion %>" />
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                                <button type="submit" class="btn btn-success">Guardar Modificacion</button>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>


                            <%--Modal Eliminar Obra Social--%>

                            <div class="modal fade" id="eliminarModal_<%= obra.Id %>" tabindex="-1" aria-labelledby="eliminarLabel_<%= obra.Id %>" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="eliminarLabel_<%= obra.Id %>">Confirmar Eliminación</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                        </div>
                                        <div class="modal-body">
                                            ¿Estás seguro que deseas eliminar la Obra Social:  
                                                <strong><%= obra.Descripcion%></strong>?
                
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                            <a href="ObrasSociales.aspx?eliminar=<%= obra.Id %>" class="btn btn-danger">Eliminar</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </td>

                        </tr>
                        <% } %>
                        <% }
                            else
                            { %>
                        <tr>
                            <td colspan="2" class="text-center py-4">No hay obras sociales cargadas.</td>
                        </tr>
                        <% } %>
                    </tbody>


                </table>
            </div>
        </div>

        <%--Formulario para agregar nueva obra social (oculto al principio)--%>
        <asp:Panel ID="formAgregar" runat="server" Visible="false" CssClass="card mt-4" Style="width: 50%; margin: auto;">
            <div class="card-header">
                Agregar Nueva Obra Social               
            </div>
            <div class="card-body">
                <div class="mb-3">
                    <label for="txtNombreObraSocial" class="form-label">Nombre de la obra social:</label>
                    <asp:TextBox ID="txtNombreObraSocial" runat="server" CssClass="form-control" placeholder="Ej. OSDE"></asp:TextBox>
                </div>
                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert alert-info d-block"></asp:Label>
                <asp:Button ID="AgregarObraSocial" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="AgregarObraSocial_Click" />
                <asp:Button ID="cerrarForm" runat="server" CssClass="btn btn-secondary" Text="Cerrar" OnClick="cerrarForm_Click" />
            </div>

        </asp:Panel>




    </div>

</asp:Content>
