<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FormularioMedico.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.FormularioMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <% 
            string modo = Request.QueryString["modo"];
            string id = Request.QueryString["id"];

            if (id == null) // Si no hay ID, es AGREGAR
            {
        %>
        <h1 class="mb-4">Agregar Médico</h1>
        <%  }
            else if (modo == "ver") // Si hay ID y el modo es "ver", es para VER DETALLES
            {
        %>
        <h1 class="mb-4">Detalles del Médico</h1>
        <%  }
            else // Si hay ID y el modo NO es "ver" o no hay modo, es para EDITAR
            {
        %>
        <h1 class="mb-4">Editar Médico</h1>
        <%  }
        %>
    </div>
    <div id="divMensaje" runat="server" class="alert" visible="false"></div>

    <div class="p-4 rounded bg-white shadow-sm">
        <%-- Sección de Datos Profesionales y Personales --%>
        <h3 class="mb-4 text-dark">Datos Profesionales y Personales</h3>
        <div class="row align-items-end g-3 mb-4">

            <%-- Matricula (PRIMER CAMPO) --%>
            <div class="col-md-3">
                <label for="txtMatricula" class="form-label font-weight-bold text-dark">Matrícula</label>
                <asp:TextBox runat="server" ID="txtMatricula" CssClass="form-control" MaxLength="6" pattern="\d{1,6}" title="La matrícula debe contener solo números y tener hasta 6 dígitos." placeholder="113355" required="true"  />
            </div>

            <%-- Nombre --%>
            <div class="col-md-3">
                <label for="txtNombre" class="form-label font-weight-bold text-dark">Nombre</label>
                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" MaxLength="100" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="El nombre debe contener solo letras, sin espacios ni al principio ni al final. El máximo de caracteres es 100." placeholder="Dr. Juan" required="true" />
            </div>

            <%-- Apellido --%>
            <div class="col-md-3">
                <label for="txtApellido" class="form-label font-weight-bold text-dark">Apellido</label>
                <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" MaxLength="100" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="El nombre debe contener solo letras, sin espacios ni al principio ni al final. El máximo de caracteres es 100." placeholder="García" required="true" />
            </div>

            <%-- DNI --%>
            <div class="col-md-3">
                <label for="txtDni" class="form-label font-weight-bold text-dark">Documento</label>
                <asp:TextBox runat="server" ID="txtDni" CssClass="form-control" MaxLength="8" placeholder="22333444" required="true" TextMode="SingleLine" pattern="\d{1,8}" title="El documento debe contener solo números y tener hasta 8 dígitos." />
            </div>
        </div>

        <%-- Sección de Contacto --%>
        <h3 class="mb-4 text-dark">Datos de Contacto</h3>
        <div class="row align-items-end g-3 mb-4">
            <%-- Email --%>
            <div class="col-md-3">
                <label for="txtEmail" class="form-label font-weight-bold text-dark">Email</label>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" MaxLength="100" TextMode="Email" placeholder="doctor.juan@clinica.com" required="true" />
            </div>

            <%-- Teléfono --%>
            <div class="col-md-3">
                <label for="txtTelefono" class="form-label font-weight-bold text-dark">Teléfono</label>
                <asp:TextBox runat="server" ID="txtTelefono" CssClass="form-control" MaxLength="10" pattern="\d{1,10}" title="Solo números, máximo 10 cifras." placeholder="1122334455" required="true" />
            </div>

            <%-- Nacionalidad --%>
            <div class="col-md-3">
                <label for="ddlNacionalidad" class="form-label font-weight-bold text-dark">Nacionalidad</label>
                <asp:DropDownList ID="ddlNacionalidad" runat="server" CssClass="form-control" AppendDataBoundItems="true" required="true">
                    <asp:ListItem Text="Seleccione Nacionalidad" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Fecha de Nacimiento --%>
            <div class="col-md-3">
                <label for="txtFechaNacimiento" class="form-label font-weight-bold text-dark">Fecha de Nacimiento</label>
                <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" TextMode="Date" required="true" />
            </div>
        </div>

        <%-- Sección de Domicilio --%>
        <h3 class="mb-4 text-dark">Datos de Domicilio</h3>
        <div class="row align-items-end g-5 mb-4">
            <%-- Provincia (DropDownList) --%>
            <div class="col-md-3">
                <label for="ddlProvincia" class="form-label font-weight-bold text-dark">Provincia</label>
                <asp:DropDownList ID="ddlProvincia" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" AppendDataBoundItems="true" required="true">
                    <asp:ListItem Text="Seleccione Provincia" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Localidad (DropDownList) --%>
            <div class="col-md-3">
                <label for="ddlLocalidad" class="form-label font-weight-bold text-dark">Localidad</label>
                <asp:DropDownList ID="ddlLocalidad" runat="server" CssClass="form-control" AppendDataBoundItems="true" required="true" Enabled="false">
                    <asp:ListItem Text="Seleccione Localidad" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Dirección/Calle --%>
            <div class="col-md-2">
                <label for="txtCalle" class="form-label font-weight-bold text-dark">Calle</label>
                <asp:TextBox runat="server" ID="txtCalle" MaxLength="30" CssClass="form-control" placeholder="Mendoza" required="true" />
            </div>

            <%-- Altura --%>
            <div class="col-md-1">
                <label for="txtAltura" class="form-label font-weight-bold text-dark">Altura</label>
                <asp:TextBox runat="server" ID="txtAltura" MaxLength="7" CssClass="form-control" placeholder="123" required="true" />
            </div>

            <%-- Cod Postal --%>
            <div class="col-md-1">
                <label for="txtCodPostal" class="form-label font-weight-bold text-dark">Cod. Postal</label>
                <asp:TextBox runat="server" ID="txtCodPostal" MaxLength="6" CssClass="form-control" placeholder="1614" required="true" />
            </div>

            <%-- Depto --%>
            <div class="col-md-1">
                <label for="txtDepto" class="form-label font-weight-bold text-dark">Depto(Opcional)</label>
                <asp:TextBox runat="server" ID="txtDepto" MaxLength="50" CssClass="form-control" placeholder="12B" />
            </div>
        </div>

        <%-- Sección de Especialidades y Turno de Trabajo --%>
        <h3 class="mb-4 text-dark">Especialidades</h3>
        <div class="row g-3 mb-4">
            <%-- Especialidades selección múltiple --%>
            <div class="col-md-6">
                <label for="lstEspecialidades" class="form-label font-weight-bold text-dark">Especialidades</label>
                <asp:ListBox ID="lstEspecialidades" runat="server"
                    CssClass="form-control"
                    SelectionMode="Multiple"
                    Rows="3"
                    AutoPostBack="true" OnSelectedIndexChanged="lstEspecialidades_SelectedIndexChanged" required="true"></asp:ListBox>
                <%-- Mensaje de ayuda para la selección múltiple --%>
                <p style="font-size: 0.85em; font-weight: bold; color: #6c757d; margin-top: 5px;">
                    Para seleccionar más de una especialidad mantener apretada la tecla Ctrl + Click Izquierdo.
                </p>
                <%-- Este Label mostrará la cantidad de especialidades seleccionadas --%>
                <asp:Label ID="lblCantidadEspecialidadesSeleccionadas" runat="server"
                    CssClass="badge bg-primary mt-2"
                    Visible="false"> </asp:Label>
            </div>


        </div>

        <%-- Sección de Disponibilidad Horaria --%>
        <h3 class="mb-4 text-dark">Disponibilidad Horaria</h3>
        <div class="row g-3 mb-4">
            <%-- Día de la Semana selección múltiple --%>
            <div class="col-md-6">
                <label for="lstDiaSemana" class="form-label font-weight-bold text-dark">Día de la Semana</label>
                <asp:ListBox ID="lstDiaSemana" runat="server"
                    CssClass="form-control"
                    SelectionMode="Multiple"
                    Rows="3"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="lstDiaSemana_SelectedIndexChanged"
                    required="true">
                    <asp:ListItem Text="Lunes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Martes" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Miércoles" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Jueves" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Viernes" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Sábado" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Domingo" Value="7"></asp:ListItem>
                </asp:ListBox>
                <%-- Mensaje de ayuda para la selección multiple de das --%>
                <p style="font-size: 0.85em; font-weight: bold; color: #6c757d; margin-top: 5px;">
                    Para seleccionar más de un día mantener apretada la tecla Ctrl + Click Izquierdo.
                </p>
                <%-- muestra la cantidad de dias seleccionados --%>
                <asp:Label ID="lblCantidadDiasSeleccionados" runat="server"
                    CssClass="badge bg-primary mt-2"
                    Visible="false"> 
                </asp:Label>
            </div>

            <%-- Turno de Trabajo --%>
            <div class="col-md-2">
                <label for="ddlTurnoTrabajo" class="form-label font-weight-bold text-dark">Turno de Trabajo Asignado</label>
                <asp:DropDownList ID="ddlTurnoTrabajo" runat="server" CssClass="form-control"
                    AppendDataBoundItems="true" required="true"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlTurnoTrabajo_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione Turno" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Hora Inicio Bloque --%>
            <div class="col-md-2">
                <label for="ddlHoraInicioBloque" class="form-label font-weight-bold text-dark">Hora Inicio</label>
                <asp:DropDownList runat="server" ID="ddlHoraInicioBloque" CssClass="form-control" Enabled="false" AppendDataBoundItems="true">
                    <asp:ListItem Text="--:--" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Hora Fin Bloque --%>
            <div class="col-md-2">
                <label for="ddlHoraFinBloque" class="form-label font-weight-bold text-dark">Hora Fin</label>
                <asp:DropDownList runat="server" ID="ddlHoraFinBloque" CssClass="form-control" Enabled="false" AppendDataBoundItems="true">
                    <asp:ListItem Text="--:--" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <%-- Contenedor de Botones --%>
        <div class="d-flex justify-content-center mt-4">
            <asp:Button Text="Guardar" ID="btnGuardar" CssClass="btn btn-primary px-5 me-3" runat="server" OnClick="btnGuardar_Click" />
            <a href="Medicos.aspx" class="btn btn-secondary px-5">Volver</a>
        </div>
    </div>
</asp:Content>
