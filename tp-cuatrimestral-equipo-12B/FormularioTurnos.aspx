<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FormularioTurnos.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.FormularioTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .calendar-disabled {
            pointer-events: none; /* bloquea clicks */
            opacity: 0.6; /* se ve más gris */
            filter: grayscale(100%);
        }

    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--para el updatepanel--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container-fluid py-4">
        <% 
            string idpacienteTurno = Request.QueryString["darturno"];


            if (!string.IsNullOrEmpty(idpacienteTurno))
            {
        %>
        <h1 class="mb-4">Asignacion de Turno</h1>

        <%  
            }
            else
            {
        %>

        <h1>Turnos</h1>
        <%  }  %>
    </div>

    <div class="p-4 rounded bg-white shadow-sm">
        <%-- Sección de Datos Personales --%>
        <h3 class="mb-4 text-dark">Datos del Paciente</h3>
        <div class="row align-items-end g-4 mb-4">
            
      
            <%-- Nombre --%>
            <div class="col-auto p-3 mb-2 flex-grow-1">
                <label for="txtNombre" class="form-label font-weight-bold text-dark">Nombre</label>
                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" MaxLength="100" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="El nombre debe contener solo letras, sin espacios ni al principio ni al final. El máximo de caracteres es 100." placeholder="Pedro" required="true" />
            </div>

            <%-- Apellido --%>
            <div class="col-auto p-3 mb-2 flex-grow-1">
                <label for="txtApellido" class="form-label font-weight-bold text-dark">Apellido</label>
                <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" MaxLength="100" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="El apellido debe contener solo letras, sin espacios ni al principio ni al final. El máximo de caracteres es 100." placeholder="Lopez" required="true" />
            </div>

            <%-- DNI --%>
            <div class="col-auto p-3 mb-2 flex-grow-1">
                <label for="txtDni" class="form-label font-weight-bold text-dark">Documento</label>

                <asp:TextBox runat="server" ID="txtDni" CssClass="form-control" MaxLength="8" placeholder="12345678" required="true" TextMode="SingleLine" pattern="\d{1,8}"
                    title="El documento debe contener solo números y tener hasta 8 dígitos." />

            </div>
            <%-- Email --%>
            <div class="col-auto p-3 mb-2 flex-grow-1">
                <label for="txtEmail" class="form-label font-weight-bold text-dark">Email</label>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TextMode="Email" MaxLength="100" placeholder="nombre@gmail.com" required="true" />
            </div>


            <%-- Fecha de Nacimiento --%>
            <div class="col-auto p-3 mb-2 flex-grow-1">
                <label for="txtFechaNacimiento" class="form-label font-weight-bold text-dark">Fecha de Nacimiento</label>
                <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" TextMode="Date" required="true" />
            </div>

        </div>



        <asp:Panel ID="panelAsignarTurno" runat="server">


            <!-- Acordeon de turno paciente-->
            <div class="accordion" id="accordionPanelsStayOpenExample">
                <div class="accordion-item">
                    <h2 class="accordion-header">
                         <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen-collapseOne" aria-expanded="true" aria-controls="panelsStayOpen-collapseOne">
                            Seleccione una especialidad:
                        </button>
                    </h2>
                    <div id="panelsStayOpen-collapseOne" class="accordion-collapse collapse show">
                        <div class="accordion-body">

                            <!-- Seleccionar especialidad!! -->
                            <div class="col-md-4 mb-3">
                                <%--<label for="ddlEspecialidades" class="form-label font-weight-bold text-dark">Especialidad</label>--%>
                                <asp:DropDownList ID="ddlEspecialidades" runat="server" CssClass="form-control"
                                    AutoPostBack="true" AppendDataBoundItems="true" required="true" OnSelectedIndexChanged="ddlEspecialidades_SelectedIndexChanged">
                                    <asp:ListItem Text="-" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                    </div>
                </div>
                <!-- luego de la seleccion de especialidad, el sistema debería sugerir tres horarios posibles con su respectivo médico -->
                <div class="accordion-item">
                    <h2 class="accordion-header">
                         <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen-collapseTwo" aria-expanded="false" aria-controls="panelsStayOpen-collapseTwo">
                            Sugerencias de horarios
        
                        </button>
                    </h2>
                    <div id="panelsStayOpen-collapseTwo" class="accordion-collapse collapse">
                        <div class="accordion-body">
                            <asp:UpdatePanel ID="updSugerencias" runat="server" UpdateMode="Conditional" EnableViewState="true">
                                <ContentTemplate>
                                    <asp:Panel ID="panelSugerencias" runat="server" CssClass="mt-3">
                                        <asp:Repeater ID="rptSugerencias" runat="server" EnableViewState="true" OnItemCommand="rptSugerencias_ItemCommand">
                                            <HeaderTemplate>
                                                <div class="d-flex flex-wrap">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button runat="server"
                                                    ID="btnSugerencia"
                                                    UseSubmitBehavior="false"
                                                    Text='<%# Eval("TextoDisplay") %>'
                                                    CssClass="btn btn-outline-secondary me-2 my-1 btn-custom-suggestion"
                                                    CommandName="SeleccionarSugerencia"
                                                    CommandArgument='<%# Eval("CommandArgumentValue") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </div>
                                            </FooterTemplate>
                                        </asp:Repeater>

                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlEspecialidades" EventName="SelectedIndexChanged" />
                                </Triggers>

                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header">
                         <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen-collapseThree" aria-expanded="false" aria-controls="panelsStayOpen-collapseThree">
                            CARGAR EL MÉDICO A PARTIR DE LA ESPECIALIDAD SELECCIONADA (si no le sirven los 3 horarios sugeridos)
                        </button>
                    </h2>
                    <div id="panelsStayOpen-collapseThree" class="accordion-collapse collapse">

                        <%--encerramos el calendario para que no recargue toda la pagina--%>
                        <asp:UpdatePanel ID="updCargaManual" runat="server" EnableViewState="true">
                            <ContentTemplate>

                                <div class="accordion-body">

                                    <!-- Médico (aparece después de elegir la especialidad!!) -->
                                    <div class="col-md-4 mb-3">
                                        <label for="ddlMedicos" class="form-label font-weight-bold text-dark">Médico</label>
                                        <asp:DropDownList ID="ddlMedicos" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlMedicos_SelectedIndexChanged">
                                            <asp:ListItem Text="Seleccione Médico" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <!-- Fecha del turno (ver disponibilidad horaria del médico seleccionado en la fecha seleccionada ) -->
                                    <div class="col-md-4 mb-3">
                                        <label for="calTurno" class="form-label font-weight-bold text-dark">Fecha del turno</label>
                                        <asp:Calendar ID="calTurno" runat="server" OnDayRender="calTurno_DayRender"
                                            OnSelectionChanged="calTurno_SelectionChanged"
                                            CssClass="form-control"
                                            Visible="true" />
                                    </div>

                                    <!-- Mostrar las horas disponibles de ese médico para ese día -->
                                    <div class="col-md-4 mb-3">
                                        <label for="ddlHorarios" class="form-label font-weight-bold text-dark">Horario del turno</label>
                                        <asp:DropDownList ID="ddlHorarios" AppendDataBoundItems="true" required="true" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Seleccione Horario" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header">
                       <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen-collapseFour" aria-expanded="false" aria-controls="panelsStayOpen-collapseFour">
                            Observaciones
                        </button>
                    </h2>
                    <div id="panelsStayOpen-collapseFour" class="accordion-collapse collapse">
                        <div class="accordion-body">
                            <%-- Sección Observaciones --%>
                            <div class="row g-3 mb-4">
                                <div class="col-md-12">
                                    <label for="txtObservaciones" class="form-label font-weight-bold text-dark">Notas del turno</label>
                                    <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control"  placeholder="Escriba aquí por que el paciente solicita turno..." required="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%-- Contenedor de Botones --%>
                <div class="d-flex justify-content-center mt-4">
                    <asp:Button Text="Guardar" ID="btnGuardar" CssClass="btn btn-primary px-5 me-3" runat="server" OnClick="btnGuardar_Click" />
                    <a href="Pacientes.aspx" class="btn btn-secondary px-5">Volver</a>
                </div>


            </div>
            <!-- Acordeon de Turno Paciente -->
        </asp:Panel>


    </div>


</asp:Content>
