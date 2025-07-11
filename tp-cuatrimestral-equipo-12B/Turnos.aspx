<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Turnos.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Turnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Turnos</h1>

        <%-- Busqueda y Filtros --%>
        <div class="filter-section mb-4">
            <div class="row g-3 align-items-end">
                <div class="col-md-4">
                    <%--<label for="txtFiltroNombre" class="form-label">Filtrar por Nombre/Apellido/DNI</label>--%>
                    <asp:TextBox runat="server" ID="txtFiltroNombre" CssClass="form-control" placeholder="Buscar turnos..." />
                </div>
                <div class="col-md-2">
                    <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary w-50" ID="btnBuscar" />
                </div>
            </div>
        </div>

        <%-- Tabla de Listado de Turnos --%>
        <div class="table-responsive">
            <table class="table table-striped table-hover table-bordered align-middle">
                <thead class="table-dark">
                    <tr>
                        <th scope="col">ID</th>
                        <th scope="col">NOMBRE</th>
                        <th scope="col">APELLIDO</th>
                        <th scope="col">DOCUMENTO</th>
                        <th scope="col">OBRA SOCIAL</th>
                        <th scope="col">ESPECIALIDAD TURNO</th>
                        <th scope="col">MÉDICO</th>
                        <th scope="col">FECHA</th>
                        <th scope="col">HORARIO</th>
                        <th scope="col">ESTADO</th>
                        <th scope="col">ACCIONES</th>
                    </tr>
                </thead>
                <tbody>
                    <% if (listaTurno != null && listaTurno.Any())
                        {
                            foreach (var t in listaTurno)
                            { %>
                    <tr>
                        <td><%= t.Id %></td>
                        <td><%= t.Paciente.Nombre %></td>
                        <td><%= t.Paciente.Apellido %></td>
                        <td><%= t.Paciente.Documento %></td>
                        <td><%= t.Paciente.DescripcionObraSocial %></td>
                        <td><%= t.Especialidad.Descripcion %></td>
                        <td><%= t.Medico.Nombre + " " + t.Medico.Apellido %></td>
                        <td><%= t.Fecha.ToString("dd/MM/yyyy") %></td>
                        <td><%= t.Hora.ToString("00") + ":00" %></td>
                        <td><%= t.Estado.ToString() %></td>
                        <td class="d-flex justify-content-center align-items-center gap-4">
                            <%-- Ver detalles del turno --%>
                            <a href='FormularioTurnos.aspx?modo=ver&id=<%= t.Id %>' class="action-link" title="Detalles del Turno">
                                <img src="images/icon_view.svg" alt="Ver" class="action-icon-img" />
                            </a>

                            <%-- Cerrar turno --%>
                            <a href='Turnos.aspx?cerrar=<%= t.Id %>' class="action-link" title="Cerrar Turno">
                                <img src="images/icon_turno_cerrado.svg" alt="Cerrar Turno" class="action-icon-img" />
                            </a>

                            <%-- Marcar inasistencia (modal) --%>
                            <div class="action-link">
                                <img src="images/icon_turno_ausente.svg" title="Inasistencia Turno" alt="Inasistencia" class="action-icon-img" style="cursor: pointer"
                                    data-bs-toggle="modal" data-bs-target="#modalInasistencia_<%= t.Id %>" />
                            </div>

                            <%-- Reprogramar turno --%>
                            <a href='FormularioTurnos.aspx?modo=editar&id=<%= t.Id %>' class="action-link" title="Reprogramar Turno">
                                <img src="images/icon_turno_reprogramado.svg" alt="Reprogramar" class="action-icon-img" />
                            </a>

                            <%-- Cancelar turno (modal) --%>
                            <div class="action-link">
                                <img src="images/icon_cancel.svg" alt="Cancelar" class="action-icon-img" style="cursor:pointer"
                                    data-bs-toggle="modal" data-bs-target="#cancelarModal_<%= t.Id %>" />
                            </div>

                            <!-- Modal de Inasistencia -->
                            <div class="modal fade" id="modalInasistencia_<%= t.Id %>" tabindex="-1" aria-labelledby="modalLabelInasistencia_<%= t.Id %>" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">Marcar Inasistencia</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                        </div>
                                        <div class="modal-body">
                                            ¿Deseás marcar la inasistencia del turno del paciente <strong><%= t.Paciente.Nombre %> <%= t.Paciente.Apellido %></strong>?
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                            <a href="Turnos.aspx?inasistencia=<%= t.Id %>" class="btn btn-danger">Marcar Inasistencia</a>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Modal de Cancelación -->
                            <div class="modal fade" id="cancelarModal_<%= t.Id %>" tabindex="-1" aria-labelledby="cancelarLabel_<%= t.Id %>" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">Cancelar Turno</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                        </div>
                                        <div class="modal-body">
                                            ¿Estás seguro que deseás cancelar el turno del paciente 
                                            <strong><%= t.Paciente.Nombre %> <%= t.Paciente.Apellido %></strong>
                                            del día <strong><%= t.Fecha.ToString("dd/MM/yyyy") %> a las <%= t.Hora.ToString("00") %>:00 hs</strong>?
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                            <a href="Turnos.aspx?cancelar=<%= t.Id %>" class="btn btn-warning">Sí, cancelar</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <%     }
                    }
                    else
                    { %>
                    <tr>
                        <td colspan="11" class="text-center">No se encontraron turnos.</td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
