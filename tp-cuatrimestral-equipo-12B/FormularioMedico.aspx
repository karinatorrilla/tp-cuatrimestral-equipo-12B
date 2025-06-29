<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FormularioMedico.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.FormularioMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <%
        string modo = Request.QueryString["modo"];
        string id = Request.QueryString["id"];
        string idAgregandoEsp = Request.QueryString["agregarespecialidad"];

        if (!string.IsNullOrEmpty(idAgregandoEsp))
        {
    %>
            <h1 class="mb-4">Agregar Especialidades al Médico</h1>
    <%
        }
        else if (string.IsNullOrEmpty(id))
        {
    %>
            <h1 class="mb-4">Agregar Médico</h1>
    <%
        }
        else if (modo == "ver")
        {
    %>
            <h1 class="mb-4">Detalles del Médico</h1>
    <%
        }
        else
        {
    %>
            <h1 class="mb-4">Editar Médico</h1>
    <%
        }
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
                <asp:TextBox runat="server" ID="txtMatricula" CssClass="form-control" MaxLength="6" pattern="\d{1,6}" title="La matrícula debe contener solo números y tener hasta 6 dígitos. Sin espacios ni al principio ni al final." placeholder="113355" required="true" />
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
        <%-- Sección de Datos Profesionales y Personales --%>

        <%-- Sección de Contacto --%>
        <asp:Panel ID="panelContacto" runat="server">
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

        </asp:Panel>
          <%-- Sección de Contacto --%>




        <%-- Sección de Domicilio --%>
        <asp:Panel ID="panelDomicilio" runat="server">
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
                    <asp:TextBox runat="server" ID="txtCalle" MaxLength="30" pattern="[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s.,-]{1,30}" title="La calle acepta solo números, letras, puntos, comas y guiones. No debe tener más de 30 caracteres." CssClass="form-control" placeholder="Mendoza" required="true" />
                </div>

                <%-- Altura --%>
                <div class="col-md-1">
                    <label for="txtAltura" class="form-label font-weight-bold text-dark">Altura</label>
                    <asp:TextBox runat="server" ID="txtAltura" MaxLength="7" pattern="\d{1,7}" title="La altura debe contener solo números y tener hasta 7 dígitos." CssClass="form-control" placeholder="123" required="true" />
                </div>

                <%-- Cod Postal --%>
                <div class="col-md-1">
                    <label for="txtCodPostal" class="form-label font-weight-bold text-dark">Cod. Postal</label>
                    <asp:TextBox runat="server" ID="txtCodPostal" pattern="\d{1,6}" title="El código postal debe contener solo números y tener hasta 6 dígitos." MaxLength="6" CssClass="form-control" placeholder="1614" required="true" />
                </div>

                <%-- Depto --%>
                <div class="col-md-1">
                    <label for="txtDepto" class="form-label font-weight-bold text-dark">Depto(Opcional)</label>
                    <asp:TextBox runat="server" ID="txtDepto" MaxLength="50" CssClass="form-control" placeholder="12B" />
                </div>
            </div>
                    </asp:Panel>
         <%-- Sección de Domicilio --%>


         <%-- Sección de Especialidades --%>
        <asp:Panel ID="panelEspecialidades" runat="server" CssClass="mb-4">
            <h3 class="mb-3 text-dark">Especialidades del Médico</h3>
            <div id="alertaMensaje" runat="server" class="alert alert-success" role="alert">
              
            </div>
            <asp:Literal ID="litEspecialidades" runat="server" Mode="PassThrough" />
            <asp:CheckBoxList ID="chkEspecialidades" runat="server" CssClass="form-check" RepeatDirection="Horizontal" RepeatColumns="3">
            </asp:CheckBoxList>
        </asp:Panel>
          <%-- Sección de Especialidades --%>
       

        <%-- Contenedor de Botones Guardar Medico Nuevo--%>
        <asp:Panel ID="panelbotonesGuardarMedico" runat="server">

            <div class="d-flex justify-content-center mt-4">
                <asp:Button Text="Guardar" ID="btnGuardar" CssClass="btn btn-primary px-5 me-3" runat="server" OnClick="btnGuardar_Click" />
                <a href="Medicos.aspx" class="btn btn-secondary px-5">Volver</a>
            </div>
        </asp:Panel>
        <%-- Contenedor de Botones Guardar Medico Nuevo--%>


        <%--Contenedor de botones agregar especialidad--%>
        <asp:Panel ID="botonesAgregarEspecialidad" runat="server">

            <div class="d-flex justify-content-center mt-4">
                <asp:Button Text="Agregar Especialidad/es" ID="btnAgregarEspecialidad" CssClass="btn btn-primary px-5 me-3" runat="server" OnClick="btnAgregarEspecialidad_Click" />
                <a href="Medicos.aspx" class="btn btn-secondary px-5">Volver</a>
            </div>
        </asp:Panel>
        <%--Contenedor de botones agregar especialidad--%>
    </div>
</asp:Content>
