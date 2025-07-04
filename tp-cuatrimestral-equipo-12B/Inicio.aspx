<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="bienvenida-panel">
            <div class="bienvenida-texto">
                <asp:Label  Text="text" ID="lblTipoUsuario" runat="server" CssClass="h1 mb-3" />
                <p class="lead mt-3">
                    <%= 
                        Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == 1 
                        ? "Aquí vas a poder ver un panorama general" 
                        : "Aquí vas a poder ver un panorama del día de hoy" 
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
                <div class="card-number"><asp:Label ID="lblTotalPacientes" runat="server" Text="0"></asp:Label></div>
                <div class="card-title">Pacientes</div>
            </div>

            <%-- Card 2: Médicos --%>
            <%if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] != 3)
               { %>
                <div class="dashboard-card">
                    <img src="images/icon_doctor.svg" alt="Icono Médicos" /> 
                    <div class="card-number"><asp:Label ID="lblTotalMedicos" runat="server" Text="0"></asp:Label></div>
                    <div class="card-title">Médicos</div>
                </div>
                <%   }%>
            

            <%-- Card 3: Turnos del Día --%>
            <div class="dashboard-card">
                <img src="images/icon_turno.svg" alt="Icono Turnos" />
                <div class="card-number">25</div>
                <div class="card-title">Turnos</div>
            </div>
        </div>


        <%-- Listado con filtros de turnos de Pacientes o Medicos con consulta a la DB con la fecha de HOY --%>
         <div class="content-listado-card" runat="server" id="divListadoGeneral">
              <div class="listado-card mt-4"> 
                <div class="row mb-3 align-items-center">
                    <div class="col-md-2">
                        <label for="ddlFiltrarPor" class="form-label mb-0">Filtrar por:</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiltrarPor" runat="server" CssClass="form-select" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>

                <div class="table-responsive">
                    <table class="table table-striped table-hover table-bordered align-middle">
                        <thead class="table-dark">
                            <tr>
                                <th scope="col">ID</th>
                                <th scope="col">NOMBRE</th>
                                <th scope="col">APELLIDO</th>
                                <th scope="col">TELÉFONO</th>
                                <th scope="col">
                                    <%= ddlFiltrarPor.SelectedIndex == 0 ? "OBRA SOCIAL" : "ESPECIALIDAD" %>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <%-- Ejemplo Hardcodeado para Paciente --%>
                            <tr>
                                <td>1</td>
                                <td>
                                    <%= ddlFiltrarPor.SelectedIndex == 0 ? "" : "Dr." %>
                                    Juan

                                </td>
                                <td>Pérez</td>
                                <td>1123456789</td>
                                <td>
                                    <%= ddlFiltrarPor.SelectedIndex == 0 ? "OSDE" : "Odontología" %>
                                </td>
                            </tr>
                            <%-- Ejemplo Hardcodeado para Médico --%>
                            <tr>
                                <td>2</td>
                                <td>
                                    <%= ddlFiltrarPor.SelectedIndex == 0 ? "" : "Dra." %>
                                     Ana
                                </td>
                                <td>Gómez</td>
                                <td>1198765432</td>
                                <td>
                                     <%= ddlFiltrarPor.SelectedIndex == 0 ? "GALENO" : "Cardiología" %>
                                </td>
                            </tr>
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
                            data: [65, 59, 80, 81, 56, 55, 40, 60, 70, 75, 85, 90], // Datos de ejemplo por mes
                            borderColor: 'rgba(75, 192, 192, 1)',
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            tension: 0.3,
                            fill: true
                        },
                        {
                            label: 'Reprogramados',
                            data: [28, 48, 40, 19, 86, 27, 90, 45, 30, 25, 35, 40], // Datos de ejemplo por mes
                            borderColor: 'rgba(255, 206, 86, 1)',
                            backgroundColor: 'rgba(255, 206, 86, 0.2)',
                            tension: 0.3,
                            fill: true
                        },
                        {
                            label: 'Cancelados',
                            data: [18, 20, 15, 25, 10, 12, 18, 22, 14, 16, 10, 11], // Datos de ejemplo por mes
                            borderColor: 'rgba(255, 99, 132, 1)',
                            backgroundColor: 'rgba(255, 99, 132, 0.2)',
                            tension: 0.3,
                            fill: true
                        },
                        {
                            label: 'Inasistencias',
                            data: [5, 7, 3, 8, 4, 6, 9, 5, 7, 3, 6, 8], // Datos de ejemplo por mes
                            borderColor: 'rgba(153, 102, 255, 1)',
                            backgroundColor: 'rgba(153, 102, 255, 0.2)',
                            tension: 0.3,
                            fill: true
                        },
                        {
                            label: 'Cerrados',
                            data: [40, 45, 50, 60, 70, 65, 75, 80, 90, 85, 95, 100], // Datos de ejemplo por mes
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
