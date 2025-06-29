<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Medicos.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Médicos</h1>
        <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert alert-info d-block w-50" Style="place-self: center;"></asp:Label>

        <%-- Búsqueda y Filtros --%>
        <div class="filter-section mb-4">
            <div class="row g-3 align-items-end">
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="txtFiltroNombre" CssClass="form-control" placeholder="Buscar médico..." />
                </div>
                <div class="col-md-2">
                    <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary w-50" ID="btnBuscar" />
                </div>
                <div class="col-md-3 ms-auto d-flex justify-content-end">
                    <asp:LinkButton runat="server" CssClass="btn btn-success" ID="btnNuevoMedico" OnClick="btnNuevoMedico_Click">
                        <img src="images/icon_add_patient.svg" alt="Agregar Médico" class="btn-icon-img me-2" />
                        Agregar Médico
                    </asp:LinkButton>
                </div>
            </div>
        </div>

        <%-- Tabla de Listado de Médicos --%>
        <div class="table-responsive">
            <table class="table table-striped table-hover table-bordered align-middle">
                <thead class="table-dark">
                    <tr>
                        <th scope="col">ID</th>
                        <th scope="col">NOMBRE</th>
                        <th scope="col">APELLIDO</th>
                        <th scope="col">MATRICULA</th>
                        <th scope="col">ESPECIALIDAD</th>
                        <th scope="col">ACCIONES</th>
                    </tr>
                </thead>
                <tbody>
                    <%-- Cuerpo del listado --%>
                    <% if (listaMedico != null && listaMedico.Any())
                        { %>
                    <% foreach (var medico in listaMedico)
                        { %>
                    <tr>
                        <td><%= medico.Id %></td>
                        <td><%= medico.Nombre %></td>
                        <td><%= medico.Apellido %></td>
                        <td><%= medico.Matricula %></td>
                        <td>
                            <%-- Muestra las especialidades separadas por coma --%>
                            <% if (medico.Especialidades != null && medico.Especialidades.Any())
                                { %>
                            <%= string.Join(", ", medico.Especialidades.Select(e => e.Descripcion)) %>
                            <% }
                                else
                                { %>
                                Sin Especialidades
                            <% } %>
                        </td>
                        <td class="d-flex justify-content-center align-items-center gap-3">

                            <%-- Ícono Ver --%>
                            <a href="FormularioMedico.aspx?id=<%= medico.Id %>&modo=ver" class="action-link" title="Ver Detalles">
                                <img src="images/icon_view.svg" alt="Ver" class="action-icon-img" />
                            </a>
                            <%-- Ícono Editar --%>
                            <a href="FormularioMedico.aspx?id=<%= medico.Id %>" class="action-link" title="Editar Médico">
                                <img src="images/icon_edit.svg" alt="Editar" class="action-icon-img" />
                            </a>
                            <%-- Ícono Especialidad --%>

                            <a href="FormularioMedico.aspx?agregarespecialidad=<%= medico.Id %>" class="btn">
                                <img src="images/icon_doctor.svg" alt="Especialidad" class="action-icon-img" style="cursor: pointer" /></a>


                            <%-- Ícono Eliminar --%>

                            <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" style="cursor: pointer"
                                data-bs-toggle="modal" data-bs-target="#eliminarModal_<%= medico.Id %>" />
                        </td>
                        <%--Modal Eliminar Medico--%>
                        <div class="modal fade" id="eliminarModal_<%= medico.Id %>" tabindex="-1" aria-labelledby="eliminarLabel_<%= medico.Id %>" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="eliminarLabel_<%= medico.Id %>">Confirmar Eliminación</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                    </div>
                                    <div class="modal-body">
                                         ¿Estás seguro que deseas eliminar el medico:  <br />
                                                                         
                                            Matricula: <strong><%= medico.Matricula%><br />
                                            </strong>Nombre: <strong><%= medico.Nombre%></strong> <strong><%= medico.Apellido%></strong>?         
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                        <a href="Medicos.aspx?eliminar=<%= medico.Id %>" class="btn btn-danger">Eliminar</a>
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
                        <td colspan="8" class="text-center py-4">No hay médicos cargados.</td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
