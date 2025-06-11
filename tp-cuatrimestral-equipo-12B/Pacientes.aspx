<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Pacientes.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Pacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de pacientes</h1>

        <%-- Busqueda y Filtros --%>
        <div class="filter-section mb-4">
            <div class="row g-3 align-items-end">
                <div class="col-md-4">
                    <%--<label for="txtFiltroNombre" class="form-label">Filtrar por Nombre/Apellido/DNI</label>--%>
                    <asp:TextBox runat="server" ID="txtFiltroNombre" CssClass="form-control" placeholder="Buscar paciente..." />
                </div>
                <div class="col-md-2">
                    <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary w-50" ID="btnBuscar" />
                </div>
                <div class="col-md-3 ms-auto d-flex justify-content-end">
                    <asp:Button Text="Agregar Paciente" runat="server" CssClass="btn btn-success w-60" ID="btnNuevoPaciente" OnClick="btnNuevoPaciente_Click" />
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
                        <th scope="col">DNI</th>
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
                        <td><%= paciente.Dni %></td>
                        <td><%= paciente.Email %></td>
                        <td>DIRECCIÓN</td>
                        <%-- Hardcodeado  --%>
                        <td><%= paciente.ObraSocial %></td>
                        <td class="d-flex justify-content-center align-items-center gap-3">

                            <%-- Ícono Ver --%>
                            <a href="#" class="action-link" title="Ver Detalles" onclick="alert('Ver paciente ID: <%= paciente.Id %>'); return false;">
                                <img src="images/icon_view.svg" alt="Ver" class="action-icon-img" />
                            </a>

                            <%-- Ícono Editar --%>
                            <a href="FormularioPaciente.aspx?id=<%= paciente.Id %>" class="action-link" title="Editar Paciente">
                                <img src="images/icon_edit.svg" alt="Editar" class="action-icon-img" />
                            </a>

                            <!-- Ícono Eliminar -->
                            <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" style="cursor: pointer"
                                data-bs-toggle="modal" data-bs-target="#eliminarModal_<%= paciente.Id %>" />

                            <!-- Modal de Confirmación de Eliminación -->
                            <div class="modal fade" id="eliminarModal_<%= paciente.Id %>" tabindex="-1" aria-labelledby="eliminarLabel_<%= paciente.Id %>" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="eliminarLabel_<%= paciente.Id %>">Confirmar Eliminación</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                        </div>
                                        <div class="modal-body">
                                            ¿Estás seguro que deseas eliminar al paciente 
                      Nombre:<strong><%= paciente.Nombre %> <%= paciente.Apellido %> DNI:<%= paciente.Dni %></strong>?
                                        </div>
                                        <div class="modal-footer">
                                            <a href="Pacientes.aspx?eliminar=<%= paciente.Id %>" class="btn btn-danger">Eliminar</a>
                                            <!-- hacemos un request a la misma pagina para usar el code behind -->
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <%-- Ícono Dar Turno --%>
                            <a href="#" class="action-link" title="Dar Turno" onclick="alert('Dar turno a ID: <%= paciente.Nombre %>'); return false;">
                                <img src="images/icon_turno.svg" alt="Dar Turno!" class="action-icon-img" />
                            </a>
                            <%--  Hacer un modal para dar turno / eliminar / editar o enviar a otra pagina?????????? --%>




                        </td>
                    </tr>
                    <% } %>
                    <% }
                        else
                        { %>
                    <tr>
                        <td colspan="8" class="text-center py-4">No hay pacientes cargados.</td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
