<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="bienvenida-panel">
            <div class="bienvenida-texto">
                <asp:Label  Text="text" ID="lblTipoUsuario" runat="server" CssClass="h1 mb-3" />
                <p class="lead mt-3">Aquí vas a poder ver un panorama del día de hoy.</p>
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
                <div class="card-number">100</div>
                <div class="card-title">Pacientes</div>
            </div>

            <%-- Card 2: Médicos --%>
            <div class="dashboard-card">
                <img src="images/icon_doctor.svg" alt="Icono Médicos" /> 
                <div class="card-number">15</div>
                <div class="card-title">Médicos</div>
            </div>

            <%-- Card 3: Turnos del Día --%>
            <div class="dashboard-card">
                <img src="images/icon_turno.svg" alt="Icono Turnos" />
                <div class="card-number">25</div>
                <div class="card-title">Turnos</div>
            </div>
        </div>


        <%-- Listado con filtros de turnos de Pacientes o Medicos con consulta a la DB con la fecha de HOY --%>
         <div class="content-listado-card">
              <div class="listado-card mt-4"> 
                <div class="row mb-3 align-items-center">
                    <div class="col-md-2">
                        <label for="ddlFiltrarPor" class="form-label mb-0">Filtrar por:</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiltrarPor" runat="server" CssClass="form-select"></asp:DropDownList>
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
                                <th scope="col">ESPECIALIDAD</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%-- Ejemplo Hardcodeado para Paciente --%>
                            <tr>
                                <td>1</td>
                                <td>Juan</td>
                                <td>Pérez</td>
                                <td>1123456789</td>
                                <td>Odontologia</td>
                            </tr>
                            <%-- Ejemplo Hardcodeado para Médico --%>
                            <tr>
                                <td>2</td>
                                <td>Dra. Ana</td>
                                <td>Gómez</td>
                                <td>1198765432</td>
                                <td>Cardiología</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

         </div>
       
    </div>
    

</asp:Content>
