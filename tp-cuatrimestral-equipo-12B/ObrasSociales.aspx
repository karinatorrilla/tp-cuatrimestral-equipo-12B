<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="ObrasSociales.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.ObrasSociales" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-check {
            padding-top: 0.5em;
            padding-left: 2.7em;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid py-4">
        <h1 class="mb-4">Listado de Obras Sociales</h1>
        <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert alert-info d-block w-50" Style="place-self: center;"></asp:Label>

        <%//para borrar el label a los 3 seg
            string script = "setTimeout(function() { var elem = document.getElementById('" + lblMensaje.ClientID + "'); elem.classList.remove('d-block'); elem.classList.add('fade'); elem.style.opacity = 0; }, 3000);";
            ClientScript.RegisterStartupScript(this.GetType(), "ocultarLabel", script, true);
        %>

        <%--Botón para mostrar el formulario de agregar--%>
        <div class="filter-section mb-4">
            <div class="row align-items-center">
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="txtBuscarOS" CssClass="form-control" placeholder="Buscar Obra Social..." />
                </div>
                <div class="col-md-2">
                    <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary w-75" ID="btnBuscarObra" OnClick="btnBuscarObra_Click" />
                </div>
                <div class="col-md-2">
                    <asp:Button Text="Limpiar" runat="server" CssClass="btn btn-primary w-75" ID="btnLimpiarBusqueda" OnClick="btnLimpiarBusqueda_Click" />
                </div>
                <div class="col-md-3 ms-auto">
                    <asp:Button ID="btnMostrarFormularioAgregar" runat="server" Text="Agregar Obra Social" CssClass="btn btn-success w-75" OnClick="btnMostrarFormularioAgregar_Click" />
                </div>
            </div>
            <div class="form-check">
                <asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="chkVerDeshabilitadas_CheckedChanged" ID="chkVerDeshabilitadas" />
                <label class="form-check-label" for="<%= chkVerDeshabilitadas.ClientID %>">Ver deshabilitadas</label>
            </div>
        </div>

    </div>
    <%--Formulario para agregar nueva obra social (oculto al principio)--%>
    <asp:Panel ID="formAgregar" runat="server" Visible="false" CssClass="card mt-4" Style="width: 50%; margin: auto;">
        <div class="card-header">
            Agregar Nueva Obra Social               
        </div>
        <div class="card-body">
            <div class="mb-3">
                <label for="txtNombreObraSocial" class="form-label">Nombre de la obra social:</label>
                <asp:TextBox ID="txtNombreObraSocial" runat="server" CssClass="form-control" placeholder="Ej. OSDE"></asp:TextBox>
            </div>

            <asp:Button ID="AgregarObraSocial" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="AgregarObraSocial_Click" />
            <asp:Button ID="cerrarForm" runat="server" CssClass="btn btn-secondary" Text="Cerrar" OnClick="cerrarForm_Click" />
        </div>

    </asp:Panel>





    <%--Tabla con listado de obras sociales--%>
    <div class="table-responsive d-flex justify-content-center">
        <div style="width: 50%;">
            <table class="table table-striped table-hover table-bordered align-middle">
                <thead class="table-dark">
                    <tr>
                        <th class="text-center">NOMBRE</th>
                        <th class="text-center">ACCIONES</th>
                    </tr>
                </thead>
                <tbody>
                    <% if (listaObrasSociales != null && listaObrasSociales.Any())
                        { %>
                    <% foreach (var obra in listaObrasSociales)
                        { %>
                    <tr>

                        <td class="text-center"><%= obra.Descripcion %></td>
                        <td class="d-flex justify-content-center align-items-center gap-3">
                            <% if (obra.Habilitado == 1)
                                { %>
                            <img src="images/icon_edit.svg" style="cursor: pointer" data-bs-toggle="modal"
                                data-bs-target="#modificarModal_<%= obra.Id %>" />
                            <img src="images/icon_delete.svg" alt="Eliminar" class="action-icon-img" style="cursor: pointer"
                                data-bs-toggle="modal" data-bs-target="#eliminarModal_<%= obra.Id %>" />
                            <% }
                                else
                                {
                            %>
                            <img src="images/icon_habilitar.svg" alt="Habilitar" class="action-icon-img" style="cursor: pointer"
                                data-bs-toggle="modal" data-bs-target="#habilitarModal<%= obra.Id %>" />
                            <% } %>
                        </td>
                        <!--Modal Habilitar Especialidad-->
                        <div class="modal fade" id="habilitarModal<%= obra.Id %>" tabindex="-1" aria-labelledby="habilitarModalLabel_<%= obra.Id %>" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <form method="post" action="ObrasSociales.aspx">
                                        <div class="modal-header">
                                            <h5 class="modal-title">Habilitar Obra Social</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                        </div>
                                        <div class="modal-body">
                                            ¿Estás seguro que deseas habilitar la Obra Social:
                                                <br />
                                            <strong><%= obra.Descripcion%></strong>?  
                
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                            <a href="ObrasSociales.aspx?habilitar=<%= obra.Id %>&descripcion=<%= Server.UrlEncode(obra.Descripcion) %>" class="btn btn-success">Habilitar</a>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                        <%--Modal dentro del foreach  modificar--%>
                        <div class="modal fade" id="modificarModal_<%= obra.Id %>" tabindex="-1">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <form method="post" action="ObrasSociales.aspx">
                                        <div class="modal-header">
                                            <h5 class="modal-title">Modificar Obra Social</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                        </div>
                                        <div class="modal-body">
                                            <input type="hidden" name="IdObraSocial" value="<%= obra.Id %>" />
                                            <label>Nombre</label>
                                            <input type="text" name="DescripcionModificada" class="form-control" value="<%= obra.Descripcion %>" />
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                            <button type="submit" class="btn btn-success">Guardar Modificacion</button>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>



                        <%--Modal Eliminar Obra Social--%>

                        <div class="modal fade" id="eliminarModal_<%= obra.Id %>" tabindex="-1" aria-labelledby="eliminarLabel_<%= obra.Id %>" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="eliminarLabel_<%= obra.Id %>">Confirmar Eliminación</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                    </div>
                                    <div class="modal-body">
                                        ¿Estás seguro que deseas eliminar la Obra Social: 
                                            <br />
                                        <strong><%= obra.Descripcion%></strong>?
                
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                        <a href="ObrasSociales.aspx?eliminar=<%= obra.Id %>" class="btn btn-danger">Eliminar</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </td>

                    </tr>
                    <% } %>
                    <% }
                        else
                        { %>
                    <tr>
                        <td colspan="2" class="text-center py-4">No hay obras sociales cargadas.</td>
                    </tr>
                    <% } %>
                </tbody>


            </table>
        </div>
    </div>




</asp:Content>
