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
                    <%-- Cuerpo del listado --%>

                    <tr>
                        <td>1</td>
                        <td>Karina</td>
                        <td>Torrilla</td>
                        <td>11222333</td>
                        <td>Particular</td>
                        <td>Dentista</td>
                        <td>Lopez Pedro</td>
                        <td>3/07/2025</td>
                        <td>09:00</td>
                        <td>Asignado</td>


                        <td class="d-flex justify-content-center align-items-center gap-4">
                            <%-- Íconos de acciones (Ver, Editar, Eliminar, Dar Turno) --%>
                            <%--ver--%>
                            <a href="FormularioTurnos.aspx" class="action-link" title="Detalles del Turno">
                                <img src="images/icon_view.svg" alt="Ver" class="action-icon-img" />
                            </a>
                            <%--editar--%>
                            <a href="FormularioTurnos.aspx" class="action-link" title="Editar Turno">
                                <img src="images/icon_edit.svg" alt="Editar" class="action-icon-img" />
                            </a>

                            <%--Eliminar--%>
                            <div class="action-link">
                                <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" style="cursor: pointer"
                                    data-bs-toggle="modal"  />
                            </div>

                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
