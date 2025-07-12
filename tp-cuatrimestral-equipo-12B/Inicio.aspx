<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="bienvenida-panel">
            <div class="bienvenida-texto">
                <asp:Label Text="text" ID="lblTipoUsuario" runat="server" CssClass="h1 mb-3" />
                <p class="lead mt-3">
                    <%= 
                        Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 1 
                        ? "Aquí vas a poder ver un panorama general." 
                        : "Aquí vas a poder ver un panorama del día de hoy " + DateTime.Today.ToString("dd/MM/yyyy") + "." 
                    %>
                </p>
            </div>
            <div class="bienvenida-imagen">
                <img src="images/doctors.jpeg" alt="Médicos" class="img-fluid" />
            </div>
        </div>

        <%-- Sección de Tarjetas --%>
        <div class="dashboard-cards">
            <%-- Card 1: Pacientes --%>
            <div class="dashboard-card">
                <img src="images/icon_people.svg" alt="Icono Pacientes" />
                <div class="card-number">
                    <asp:Label ID="lblTotalPacientes" runat="server" Text="0"></asp:Label>
                </div>
                <div class="card-title">
                    <asp:Label ID="lblTextoPaciente" runat="server" Text="Pacientes"></asp:Label>
                </div>
            </div>

            <%-- Card 2: Médicos --%>
            <%if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] != 3)
                { %>
            <div class="dashboard-card">
                <img src="images/icon_doctor.svg" alt="Icono Médicos" />
                <div class="card-number">
                    <asp:Label ID="lblTotalMedicos" runat="server" Text="0"></asp:Label>
                </div>
                <div class="card-title">
                    <asp:Label ID="lblTextoMedico" runat="server" Text="Médicos"></asp:Label>
                </div>
            </div>
            <%   }%>


            <%-- Card 3: Turnos del Día --%>
            <div class="dashboard-card">
                <img src="images/icon_turno.svg" alt="Icono Turnos" />
                <div class="card-number">
                    <asp:Label ID="lblTotalTurnos" runat="server" Text="0"></asp:Label>
                </div>
                <div class="card-title">
                    <asp:Label ID="lblTextoTurno" runat="server" Text="Turnos"></asp:Label>
                </div>
            </div>
        </div>


        <%-- Listado de Pacientes y Médicos para Recepcionista / Listado de Turnos para Médico --%>
        <div class="content-listado-card" runat="server" id="divListadoGeneral">
            <div class="listado-card mt-4">
                <%if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 2) //si es recepcionista
                    { %>
                <div class="row mb-3 align-items-center">
                    <div class="col-md-2">
                        <label for="ddlFiltrarPor" class="form-label mb-0">Filtrar por:</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiltrarPor" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlFiltrarPor_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>
                <% }
                %>
                <%else if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 3) //si es médico
                    { %>
                <h3 class="mb-3 fw-bold">Mis Turnos</h3>
                <div class="row mb-3 align-items-center">
                    <div class="col-md-2">
                        <label for="txtFechaFiltro">Filtrar por fecha:</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFechaFiltro" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnFiltrarTurnos" runat="server" Text="Filtrar" CssClass="btn btn-primary w-100" />
                        <asp:Button ID="btnLimpiar" Text="Limpiar" runat="server" CssClass="btn btn-secondary w-100 ms-3" />
                    </div>
                </div>
                <%} %>

                <div class="table-responsive">
                    <table class="table table-striped table-hover table-bordered align-middle">
                        <thead class="table-dark">
                            <%if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 2) //si es recepcionista
                                { %>
                            <tr>
                                <th scope="col">ID</th>
                                <th scope="col">NOMBRE</th>
                                <th scope="col">APELLIDO</th>
                                <th scope="col">TELÉFONO</th>
                                <th scope="col">
                                    <%= ddlFiltrarPor.SelectedIndex == 0 ? "OBRA SOCIAL" : "ESPECIALIDAD" %>
                                </th>
                            </tr>
                            <%} %>
                            <%else if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 3) //si es médico
                                { %>
                            <tr>
                                <th scope="col">ID</th>
                                <th scope="col">NOMBRE</th>
                                <th scope="col">APELLIDO</th>
                                <th scope="col">DOCUMENTO</th>
                                <th scope="col">OBRA SOCIAL</th>
                                <th scope="col">ESPECIALIDAD DEL TURNO</th>
                                <th scope="col">FECHA</th>
                                <th scope="col">HORARIO</th>
                                <th scope="col">ESTADO</th>
                            </tr>
                            <%} %>
                        </thead>
                        <tbody>
                            <%if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 2) //si es recepcionista
                                { %>
                            <% if (ddlFiltrarPor.SelectedValue == "0" && listaPaciente != null)
                                {
                                    foreach (var p in listaPaciente)
                                    { %>
                            <tr>
                                <td><%= p.Id %></td>
                                <td><%= p.Nombre %></td>
                                <td><%= p.Apellido %></td>
                                <td><%= p.Telefono %></td>
                                <td><%= p.DescripcionObraSocial %></td>
                            </tr>
                            <%     }
                                }
                                else if (ddlFiltrarPor.SelectedValue == "1" && listaMedico != null)
                                {
                                    foreach (var m in listaMedico)
                                    { %>
                            <tr>
                                <td><%= m.Id %></td>
                                <td><%= m.Nombre %></td>
                                <td><%= m.Apellido %></td>
                                <td><%= m.Telefono %></td>
                                <td><%-- Muestra las especialidades separadas por coma --%>
                                    <% if (m.Especialidades != null && m.Especialidades.Any())
                                        { %>
                                    <%= string.Join(", ", m.Especialidades.Select(e => e.Descripcion)) %>
                                    <% }
                                        else
                                        { %>
                                         Sin Especialidades
                                    <% } %>
                                </td>
                            </tr>
                            <%     }
                                } %>
                            <%} %>
                            <% else if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 3 && listaTurnos != null)
                                {
                                    if (listaTurnos.Count > 0)
                                    {
                                        foreach (var t in listaTurnos)
                                        { %>
                            <tr>
                                <td><%= t.Id %></td>
                                <td><%= t.Paciente.Nombre %></td>
                                <td><%= t.Paciente.Apellido %></td>
                                <td><%= t.Paciente.Documento %></td>
                                <td><%= t.Paciente.DescripcionObraSocial %></td>
                                <td><%= t.Especialidad.Descripcion %></td>
                                <td><%= t.Fecha.ToString("dd/MM/yyyy") %></td>
                                <td><%= (t.Hora.ToString("00") + ":00") %></td>
                                <td><%= t.Estado.ToString() %></td>
                            </tr>
                            <%       }
                            }
                            else
                            { %>
                            <tr>
                                <td colspan="9" class="text-center">No se encontraron turnos.</td>
                            </tr>
                            <%   }
                            } %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <%-- Gráfico de estadisticas de turnos estados --%>
        <asp:Panel ID="pnlGraficoAdmin" runat="server" CssClass="listado-card mt-4" Visible="false">
            <h3 class="mb-3">Estadísticas de Turnos</h3>
            <div class="row">
                <div class="col-md-10 offset-md-1">
                    <canvas id="turnosChart" class="content-canvas-graphic"></canvas>
                </div>
            </div>
        </asp:Panel>

    </div>
    <script>
        function renderTurnosChart() {
            var ctx = document.getElementById('turnosChart').getContext('2d');
            var turnosChart = new Chart(ctx, {
                type: 'line',
                data: {
                    // Meses texto del eje X
                    labels: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    datasets: [
                        {
                            label: 'Nuevos',
                            data: [<%= ObtenerArregloJS((dominio.EstadoTurno)1) %>], // Datos de ejemplo por mes
                            borderColor: 'rgba(75, 192, 192, 1)',
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            tension: 0.3,
                            fill: true
                        },
                        {
                            label: 'Reprogramados',
                            data: [<%= ObtenerArregloJS((dominio.EstadoTurno)2) %>], // Datos de ejemplo por mes
                            borderColor: 'rgba(255, 206, 86, 1)',
                            backgroundColor: 'rgba(255, 206, 86, 0.2)',
                            tension: 0.3,
                            fill: true
                        },
                        {
                            label: 'Cancelados',
                            data: [<%= ObtenerArregloJS((dominio.EstadoTurno)3) %>], // Datos de ejemplo por mes
                            borderColor: 'rgba(255, 99, 132, 1)',
                            backgroundColor: 'rgba(255, 99, 132, 0.2)',
                            tension: 0.3,
                            fill: true
                        },
                        {
                            label: 'Inasistencias',
                            data: [<%= ObtenerArregloJS((dominio.EstadoTurno)4) %>], // Datos de ejemplo por mes
                            borderColor: 'rgba(153, 102, 255, 1)',
                            backgroundColor: 'rgba(153, 102, 255, 0.2)',
                            tension: 0.3,
                            fill: true
                        },
                        {
                            label: 'Cerrados',
                            data: [<%= ObtenerArregloJS((dominio.EstadoTurno)5) %>],// Datos de ejemplo por mes
                            borderColor: 'rgba(54, 162, 235, 1)',
                            backgroundColor: 'rgba(54, 162, 235, 0.2)',
                            tension: 0.3,
                            fill: true
                        }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                        title: {
                            display: true,
                            text: 'Turnos por Estado Mensual'
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            min: 0,
                            max: 50,
                            ticks: {
                                stepSize: 5,
                                callback: function (value) {
                                    return value; // Muestra 0, 5, 10, 15...
                                }
                            },
                            title: {
                                display: true,
                                text: 'Cantidad de Turnos'
                            }
                        },
                        x: {
                            title: {
                                display: true,
                                text: 'Mes'
                            }
                        }
                    }
                }
            });
        }

        // Llama a la función del gráfico solo si el elemento del gráfico es visible
        document.addEventListener('DOMContentLoaded', function () {
            var graficoAdminPanel = document.getElementById('<%= pnlGraficoAdmin.ClientID %>');
            if (graficoAdminPanel && graficoAdminPanel.visible != false) {
                renderTurnosChart();
            }
        });
    </script>

</asp:Content>
