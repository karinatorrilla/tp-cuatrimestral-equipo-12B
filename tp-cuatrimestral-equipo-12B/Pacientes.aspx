<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Pacientes.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Pacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--para el updatepanel--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Pacientes</h1>

        <%-- Busqueda y Filtros --%>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="filter-section mb-4">
                    <div class="row g-3 align-items-end">
                        <div class="col-md-1">
                            <label for="ddlFiltro" class="form-label font-weight-bold text-dark">Filtrar por</label>
                            <asp:DropDownList ID="ddlFiltro" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Nombre" />
                                <asp:ListItem Text="Apellido" />
                                <asp:ListItem Text="Documento" />
                                <asp:ListItem Text="Obra Social" />
                                <asp:ListItem Text="Email" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control" placeholder="Buscar paciente..." />
                        </div>
                        <div class="col-md-1">
                            <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary w-100" ID="btnBuscar" OnClick="btnBuscar_Click" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button Text="Limpiar" runat="server" CssClass="btn btn-secondary w-100" ID="btnLimpiar" OnClick="btnLimpiar_Click" />
                        </div>
                        <div class="col-md-2 ms-auto d-flex justify-content-end">
                            <asp:Button Text="Agregar Paciente" runat="server" CssClass="btn btn-success w-80" ID="btnNuevoPaciente" OnClick="btnNuevoPaciente_Click" />
                        </div>
                    </div>
                </div>

                <%-- Tabla de Listado de Pacientes --%>
                <div class="table-responsive">
                    <table class="table table-striped table-hover table-bordered align-middle">
                        <thead class="table-dark">
                            <tr>
                                <th scope="col">ID</th>
                                <th scope="col">NOMBRE</th>
                                <th scope="col">APELLIDO</th>
                                <th scope="col">DOCUMENTO</th>
                                <th scope="col">EMAIL</th>
                                <th scope="col">DIRECCIÓN</th>
                                <th scope="col">OBRA SOCIAL</th>
                                <th scope="col">ACCIONES</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%-- Cuerpo del listado --%>
                            <% if (listaPaciente != null && listaPaciente.Any())
                                { %>
                            <% foreach (var paciente in listaPaciente)
                                { %>
                            <tr>
                                <td><%= paciente.Id %></td>
                                <td><%= paciente.Nombre %></td>
                                <td><%= paciente.Apellido %></td>
                                <td><%= paciente.Documento %></td>
                                <td><%= paciente.Email %></td>
                                <%-- Construcción de la DIRECCIÓN --%>
                                <td>
                                    <%= paciente.Calle %> <%= paciente.Altura %>
                                    <%-- Mostrar la Descripción de la Obra Social --%>
                                <td><%= paciente.DescripcionObraSocial %></td>
                                <td class="d-flex justify-content-center align-items-center gap-4">
                                    <%-- Íconos de acciones (Ver, Editar, Eliminar, Dar Turno) --%>
                                    <%--ver--%>
                                    <a href="FormularioPaciente.aspx?id=<%= paciente.Id %>&modo=ver" class="action-link" title="Ver Detalles">
                                        <img src="images/icon_view.svg" alt="Ver" class="action-icon-img" />
                                    </a>
                                    <%--editar--%>
                                    <a href="FormularioPaciente.aspx?id=<%= paciente.Id %>" class="action-link" title="Editar Paciente">
                                        <img src="images/icon_edit.svg" alt="Editar" class="action-icon-img" />
                                    </a>

                                    <%--Eliminar--%>
                                    <div class="action-link">
                                        <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" style="cursor: pointer"
                                            data-bs-toggle="modal" data-bs-target="#eliminarModal_<%= paciente.Id %>" />
                                    </div>

                                    <div class="modal fade" id="eliminarModal_<%= paciente.Id %>" tabindex="-1" aria-labelledby="eliminarLabel_<%= paciente.Id %>" aria-hidden="true">
                                        <div class="modal-dialog modal-dialog-centered">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title" id="eliminarLabel_<%= paciente.Id %>">Confirmar Eliminación</h5>
                                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                                </div>
                                                <div class="modal-body">
                                                    ¿Estás seguro que deseas eliminar al paciente 
                                                Nombre:<strong><%= paciente.Nombre %> <%= paciente.Apellido %> DNI:<%= paciente.Documento %></strong>?
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                                    <a href="Pacientes.aspx?eliminar=<%= paciente.Id %>" class="btn btn-danger">Eliminar</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--Dar turno--%>
                                    <a href="FormularioTurnos.aspx?darturno=<%= paciente.Id %>" class="action-link" title="Dar Turno">
                                        <img src="images/icon_turno.svg" alt="Dar Turno!" class="action-icon-img" />
                                    </a>
                                </td>
                            </tr>
                            <% } %>
                            <% }
                                else
                                { %>
                            <tr>
                                <td colspan="8" class="text-center py-4">No se encontraron pacientes.</td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
