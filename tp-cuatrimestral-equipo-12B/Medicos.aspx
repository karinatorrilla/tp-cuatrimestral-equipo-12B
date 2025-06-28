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
                            <% if (medico.Especialidades != null && medico.Especialidades.Any()) { %>
                                <%= string.Join(", ", medico.Especialidades.Select(e => e.Descripcion)) %>
                            <% } else { %>
                                Sin Especialidades
                            <% } %>
                        </td>
                        <td class="d-flex justify-content-center align-items-center gap-3">

                            <%-- Ícono Ver --%>
                            <a href="#" class="action-link" title="Ver Detalles" onclick="alert('Ver médico ID: 1'); return false;">
                                <img src="images/icon_view.svg" alt="Ver" class="action-icon-img" />
                            </a>
                            <%-- Ícono Editar --%>
                            <a href="FormularioMedico.aspx?id=<%= medico.Id %>" class="action-link" title="Editar Médico">
                                <img src="images/icon_edit.svg" alt="Editar" class="action-icon-img" />
                            </a>
                            <%-- Ícono Especialidad --%>
                            <a href="#" class="action-link" title="Especialidad Médico">
                                <img src="images/icon_doctor.svg" alt="Especialidad" class="action-icon-img" style="cursor: pointer" data-bs-toggle="modal"
                                    data-bs-target="#especialidadModal_<%= medico.Id %>" />
                            </a>
                            <!--Modal Especialidad-->
                            <div class="modal fade" id="especialidadModal_<%= medico.Id %>" tabindex="-1" aria-labelledby="especialidadLabel_<%= medico.Id %>" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div>
                                            <div class="modal-header">
                                                <h5 class="modal-title">Especialidades Dr/Dra <%= medico.Nombre %> <%= medico.Apellido %> </h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                            </div>
                                            <div class="modal-body">
                                                    <div class="row g-3 mb-4">
                                                        <%-- Especialidades selección múltiple --%>
                                                        <div class="col-md-12">
                                                            <label for="lstEspecialidades" class="form-label font-weight-bold text-dark">Listado de Especialidades</label>
                                                            <asp:ListBox ID="lstEspecialidades" runat="server"
                                                                CssClass="form-control"
                                                                SelectionMode="Multiple"
                                                                Rows="4"
                                                                AutoPostBack="false" 
                                                                required="true">
                                                            </asp:ListBox>
                                                            <asp:HiddenField ID="hfMedicoId" runat="server" Value='<%= medico.Id %>' />

                                                            <%-- Mensaje de ayuda para la selección múltiple --%>
                                                            <p style="font-size: 0.85em; font-weight: bold; color: #6c757d; margin-top: 10px;">
                                                                Para seleccionar más de una especialidad mantener apretada la tecla Ctrl + Click Izquierdo por cada una.
                                                            </p>
                                                        </div>
                                                    </div>
                                                <div class="table-responsive">
                                                    <table class="table table-striped table-hover table-bordered align-middle">
                                                        <thead class="table-dark">
                                                            <tr>
                                                                <th scope="col">ID</th>
                                                                <th scope="col">ESPECIALIDAD</th>
                                                                <th scope="col">ACCIONES</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                             <%-- especialidades del médico --%>
                                                                    <% if (medico.Especialidades != null && medico.Especialidades.Any()) { %>
                                                                        <% foreach (var especialidad in medico.Especialidades) { %>
                                                                            <tr>
                                                                                <td><%= especialidad.Id %></td>
                                                                                <td><%= especialidad.Descripcion %></td>
                                                                                <td class="d-flex justify-content-center align-items-center gap-3">
                                                                                    <a href="#" class="action-link" title="Eliminar Especialidad del médico"
                                                                                        onclick="if(confirm('¿Estás seguro de eliminar la especialidad <%= especialidad.Descripcion %> del Dr/Dra <%= medico.Apellido %>?')) { window.location.href = 'EliminarEspecialidadMedico.aspx?medicoId=<%= medico.Id %>&especialidadId=<%= especialidad.Id %>'; return false; } else { return false; }">
                                                                                        <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" />
                                                                                    </a>
                                                                                </td>
                                                                            </tr>
                                                                        <% } %>
                                                                    <% } else { %>
                                                                        <tr>
                                                                            <td colspan="3" class="text-center py-3">Este médico no tiene especialidades asignadas.</td>
                                                                        </tr>
                                                                    <% } %>
                                                         </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnGuardarEspecialidades" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="btnGuardarEspecialidades_Click" CommandArgument='<%= medico.Id %>' />
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
