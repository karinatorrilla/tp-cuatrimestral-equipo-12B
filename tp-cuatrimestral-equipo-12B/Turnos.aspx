<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Turnos.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Turnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--para el updatepanel--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Turnos</h1>

        <%-- Busqueda y Filtros --%>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="filter-section mb-4">
                    <div class="row g-3 align-items-end">
                        <div class="col-md-1">
                            <label for="ddlFiltro" class="form-label font-weight-bold text-dark">Filtrar por</label>
                            <asp:DropDownList ID="ddlFiltro" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostback="true" runat="server" CssClass="form-control">
                                <%--<asp:ListItem Text="Nombre" />
                                <asp:ListItem Text="Apellido" />
                                <asp:ListItem Text="Documento" />
                                <asp:ListItem Text="Especialidad" />                              
                                <asp:ListItem Text="Médico" />
                                <asp:ListItem Text="Fecha" />
                                <asp:ListItem Text="Estado" /> --%>                            
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control" placeholder="Buscar turnos..." />
                        </div>
                        <div class="col-md-1">
                            <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary w-100" ID="btnBuscar" OnClick="btnBuscar_Click" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button Text="Limpiar" runat="server" CssClass="btn btn-secondary w-100" ID="btnLimpiar" OnClick="btnLimpiar_Click" />
                        </div>                       
                    </div>
                </div>

                <%-- Tabla de Listado de Turnos --%>
                <div class="table-responsive">
                    <table class="table table-striped table-hover table-bordered align-middle">
                        <thead class="table-dark">
                            <tr>
                                <th scope="col">NRO° DE TURNO</th>
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
                                <td>
                                    <span class="estado-cell <%= GetEstadoCssClass(t.Estado) %>">
                                        <%= t.Estado.ToString() %>
                                    </span>
                                </td>
                                <td class="d-flex justify-content-center align-items-center gap-4">
                                    <%-- Ver detalles del turno --%>
                                    <a href='FormularioTurnos.aspx?modo=ver&id=<%= t.Id %>' class="action-link" title="Detalles del Turno">
                                        <img src="images/icon_view.svg" alt="Ver" class="action-icon-img" />
                                    </a>

                                    <!-- Solo mostramos acciones si Estado es Nuevo 1 o Reprogramado 2 -->
                                    <% if ((int)t.Estado == 1 || (int)t.Estado == 2)
                                        { %>

                                    <%-- Cerrar turno --%>
                                    <div class="action-link">
                                        <img src="images/icon_turno_cerrado.svg" title="Cerrar Turno" alt="Cerrar Turno" class="action-icon-img" style="cursor: pointer"
                                            data-bs-toggle="modal" data-bs-target="#cerrarModal_<%= t.Id %>" />
                                    </div>

                                    <%-- Marcar inasistencia --%>
                                    <div class="action-link">
                                        <img src="images/icon_turno_ausente.svg" title="Inasistencia Turno" alt="Inasistencia" class="action-icon-img" style="cursor: pointer"
                                            data-bs-toggle="modal" data-bs-target="#modalInasistencia_<%= t.Id %>" />
                                    </div>

                                    <%-- Reprogramar turno --%>
                                    <a href='FormularioTurnos.aspx?modo=editar&id=<%= t.Id %>' class="action-link" title="Reprogramar Turno">
                                        <img src="images/icon_turno_reprogramado.svg" alt="Reprogramar" class="action-icon-img" />
                                    </a>

                                    <%-- Cancelar turno --%>
                                    <div class="action-link">
                                        <img src="images/icon_turno_cancelado.svg" title="Cancelar Turno" alt="Cancelar" class="action-icon-img" style="cursor: pointer"
                                            data-bs-toggle="modal" data-bs-target="#cancelarModal_<%= t.Id %>" />
                                    </div>

                                    <% } %>


                                    <!-- Modal de Cierre -->
                                    <div class="modal fade" id="cerrarModal_<%= t.Id %>" tabindex="-1" aria-labelledby="cerrarLabel_<%= t.Id %>" aria-hidden="true">
                                        <div class="modal-dialog modal-dialog-centered">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title">Cerrar Turno</h5>
                                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                                </div>
                                                <div class="modal-body">
                                                    ¿Estás seguro que deseás cerrar el turno del paciente 
                                            <strong><%= t.Paciente.Nombre %> <%= t.Paciente.Apellido %></strong>
                                                    del día <strong><%= t.Fecha.ToString("dd/MM/yyyy") %> a las <%= t.Hora.ToString("00") %>:00 hs</strong>?
                                            <br>
                                                    Esta acción marca el turno como finalizado y lo bloquea para reprogramar.
                                                </div>
                                                <div class="modal-footer">
                                                    <a href="Turnos.aspx?cerrar=<%= t.Id %>" class="btn btn-success">Sí, cerrar turno</a>
                                                </div>
                                            </div>
                                        </div>
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
                                                    <a href="Turnos.aspx?cancelar=<%= t.Id %>" class="btn btn-danger">Sí, cancelar</a>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
