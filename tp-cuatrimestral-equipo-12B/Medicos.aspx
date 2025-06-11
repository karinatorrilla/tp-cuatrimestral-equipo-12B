<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Medicos.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Médicos</h1>
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
                        <td><%= medico.EspecialidadSeleccionada.Descripcion %></td>
                        <td class="d-flex justify-content-center align-items-center gap-3">

                            <%-- Ícono Ver --%>
                            <a href="#" class="action-link" title="Ver Detalles" onclick="alert('Ver médico ID: 1'); return false;">
                                <img src="images/icon_view.svg" alt="Ver" class="action-icon-img" />
                            </a>
                            <%-- Ícono Editar --%>
                            <a href="#" class="action-link" title="Editar Médico" onclick="alert('Editar médico ID: 1'); return false;">
                                <img src="images/icon_edit.svg" alt="Editar" class="action-icon-img" />
                            </a>
                            <%-- Ícono Eliminar --%>
                            <a href="#" class="action-link" title="Eliminar Médico" onclick="if(confirm('¿Estás seguro de eliminar al médico Perez?')) { alert('Eliminar médico ID: 1'); return true; } else { return false; }">
                                <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" />
                            </a>
                        </td>
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
