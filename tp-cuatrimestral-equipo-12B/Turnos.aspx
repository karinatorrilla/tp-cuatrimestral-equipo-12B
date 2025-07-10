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
                            <%-- Íconos de acciones (Ver, Editar, Eliminar) --%>
                            <a href='FormularioTurnos.aspx?modo=ver&id=<%= t.Id %>' class="action-link" title="Detalles del Turno">
                                <img src="images/icon_view.svg" alt="Ver" class="action-icon-img" />
                            </a>
                            <a href='FormularioTurnos.aspx?modo=editar&id=<%= t.Id %>' class="action-link" title="Editar Turno">
                                <img src="images/icon_edit.svg" alt="Editar" class="action-icon-img" />
                            </a>
                            <div class="action-link">
                                <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" style="cursor: pointer"
                                    data-bs-toggle="modal" data-bs-target="#modalEliminar_<%= t.Id %>" />
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
