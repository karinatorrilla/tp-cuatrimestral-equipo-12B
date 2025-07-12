<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Medicos.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--para el updatepanel--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Médicos</h1>
        <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert alert-info d-block w-50" Style="place-self: center;"></asp:Label>

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
                                <asp:ListItem Text="Matricula" />
                                <asp:ListItem Text="Especialidad" />                                
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control" placeholder="Buscar médico..." />
                        </div>
                        <div class="col-md-1">
                            <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary w-100" ID="btnBuscar" OnClick="btnBuscar_Click" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button Text="Limpiar" runat="server" CssClass="btn btn-secondary w-100" ID="btnLimpiar" OnClick="btnLimpiar_Click" />
                        </div>
                        <div class="col-md-2 ms-auto d-flex justify-content-end">
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
                                        <img src="images/icon_doctor.svg" alt="Especialidad" class="action-icon-img" style="cursor: pointer" />
                                    </a>

                                    <%-- Ícono Disponibilidad horaria --%>
                                    <div class="action-link">
                                        <img src="images/icon_clock.svg" alt="Disponibilidad horaria" class="action-icon-img" style="cursor: pointer"
                                            data-bs-toggle="modal" data-bs-target="#modalDisponibilidadMedico_<%= medico.Id %>" />
                                    </div>

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
                                                ¿Estás seguro que deseas eliminar el medico: 
                                        <br />

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

                                <%-- MODAL DE DISPONIBILIDAD HORARIA --%>
                                <div class="modal fade" id="modalDisponibilidadMedico_<%= medico.Id %>" tabindex="-1" role="dialog" aria-labelledby="modalDisponibilidadMedicoLabel_<%= medico.Id %>" aria-hidden="true">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="modalDisponibilidadMedicoLabel_<%= medico.Id %>">Administrar Disponibilidad Horaria de <%= medico.Apellido %> <%= medico.Nombre %>
                                                </h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"
                                                    onclick="limpiarCamposModalDisponibilidad('<%= medico.Id %>');">
                                                </button>
                                            </div>

                                            <%-- Cuerpo del Modal --%>
                                            <div class="modal-body">
                                                <div class="row g-3 align-items-end mb-4">
                                                    <div class="col-md-4">
                                                        <label for="ddlDiaSemana_<%= medico.Id %>" class="form-label">Día de la Semana:</label>
                                                        <select id="ddlDiaSemana_<%= medico.Id %>" name="ddlDiaSemana_<%= medico.Id %>" class="form-control">
                                                            <option value="" selected disabled>Seleccione un día</option>
                                                            <option value="1">Lunes</option>
                                                            <option value="2">Martes</option>
                                                            <option value="3">Miércoles</option>
                                                            <option value="4">Jueves</option>
                                                            <option value="5">Viernes</option>
                                                            <option value="6">Sábado</option>
                                                            <option value="7">Domingo</option>
                                                        </select>
                                                    </div>
                                                    <%--<div class="col-md-3">
                                                        <label for="txtHoraInicio_<%= medico.Id %>" class="form-label">Hora de Inicio:</label>
                                                        <input type="time" id="txtHoraInicio_<%= medico.Id %>" name="txtHoraInicio_<%= medico.Id %>" class="form-control" step="3600" />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="txtHoraFin_<%= medico.Id %>" class="form-label">Hora de Fin:</label>
                                                        <input type="time" id="txtHoraFin_<%= medico.Id %>" name="txtHoraFin_<%= medico.Id %>" class="form-control" step="3600" />
                                                    </div>--%>
                                                    <div class="col-md-3">
                                                         <label for="txtHoraInicio_<%= medico.Id %>" class="form-label">Hora de Inicio:</label>
                                                        <select id="ddlHoraInicio_<%= medico.Id %>" name="horaInicio" class="form-select">
                                                            <%= GenerarOpcionesHorario() %>
                                                        </select>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="txtHoraFin_<%= medico.Id %>" class="form-label">Hora de Fin:</label>
                                                        <select id="ddlHoraFin_<%= medico.Id %>" name="horaFin" class="form-select">
                                                            <%= GenerarOpcionesHorario() %>
                                                        </select>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <%-- Botón para agregar disponibilidad horaria que envía datos por URL --%>
                                                        <a id="btnAgregarDisponibilidad_<%= medico.Id %>"
                                                            href="#" class="btn btn-success w-100"
                                                            onclick="return generarEnlaceDisponibilidad('<%= medico.Id %>');">Agregar
                                                        </a>
                                                        <%-- Campo oculto para guardar el ID de la disponibilidad que se está editando --%>
                                                        <input type="hidden" id="hdnDisponibilidadId_<%= medico.Id %>" value="" />
                                                    </div>

                                                </div>

                                                <h6 class="mt-4 mb-3">Horarios de Trabajo Asignados:</h6>
                                                <%-- Tabla Listado de hrarios disponibles --%>
                                                <div class="list-group">

                                                    <div class="list-group-item list-group-item-dark d-flex justify-content-between align-items-center fw-bold py-2" style="background-color: #000; color: #fff; border-radius: 0;">
                                                        <span style="flex: 0 0 5%;">ID</span>
                                                        <span style="flex: 0 0 25%;">DÍA DE LA SEMANA</span>
                                                        <span style="flex: 0 0 20%;">INICIO HORARIO</span>
                                                        <span style="flex: 0 0 20%;">FIN HORARIO</span>
                                                        <span style="flex: 0 0 10%; text-align: center;">ACCIONES</span>
                                                    </div>

                                                    <%-- Lista las disponibilidades horarias del medico --%>
                                                    <% if (medico.Disponibilidades != null && medico.Disponibilidades.Any())
                                                        { %>
                                                    <% foreach (var disp in medico.Disponibilidades)
                                                        { %>
                                                    <div class="list-group-item d-flex justify-content-between align-items-center py-2">
                                                        <span style="flex: 0 0 5%;" data-disponibilidad-id="<%= disp.Id %>"><%= disp.Id %></span>
                                                        <span style="flex: 0 0 25%;" data-dia-numero="<%= disp.DiaDeLaSemana %>"><%= disp.DiaSemanaDescripcion %></span>
                                                        <span style="flex: 0 0 20%;"><%= disp.HoraInicioBloque.ToString("hh':'mm") %></span>
                                                        <span style="flex: 0 0 20%;"><%= disp.HoraFinBloque.ToString("hh':'mm") %></span>
                                                        <div style="flex: 0 0 10%; text-align: center; display: flex; justify-content: center; align-items: center;">
                                                            <%-- Icono de editar para la disponibilidad horaria --%>
                                                            <button type="button" class="action-link" style="margin-right: 5px; background-color: transparent; border: none;" title="Editar Disponibilidad"
                                                                onclick="
                                                                var row = this.closest('.list-group-item');
                                                                var dispId = row.querySelector('[data-disponibilidad-id]').innerText;
                                                                var diaNum = row.querySelector('[data-dia-numero]').dataset.diaNumero;
                                                                var horaInicio = row.children[2].innerText;
                                                                var horaFin = row.children[3].innerText;

                                                                editarDisponibilidad(
                                                                    '<%= medico.Id %>',
                                                                    dispId,
                                                                    diaNum,
                                                                    horaInicio,
                                                                    horaFin
                                                                );
                                                                return false;
                                                            ">
                                                                <img src="images/icon_edit.svg" alt="Editar" class="action-icon-img" style="cursor: pointer; width: 24px; height: 24px;" />
                                                            </button>
                                                            <%-- Icono de eliminar para la disponibilidad horaria --%>
                                                            <a href="Medicos.aspx?eliminarDisponibilidad=<%= disp.Id %>&idMedico=<%= medico.Id %>" class="action-link" title="Eliminar Disponibilidad">
                                                                <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" style="cursor: pointer" />
                                                            </a>
                                                        </div>
                                                    </div>
                                                    <% } %>
                                                    <% }
                                                        else
                                                        { %>
                                                    <div class="list-group-item text-center text-muted py-3">
                                                        No hay horarios de trabajo asignados.
                                                    </div>
                                                    <% } %>
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
                                <td colspan="8" class="text-center py-4">No se encontraron médicos.</td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
