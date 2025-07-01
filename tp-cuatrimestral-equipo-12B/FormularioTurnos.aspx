<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FormularioTurnos.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.FormularioTurnos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
     <h3 class="mb-4 text-dark">Datos Personales</h3>
     <div class="row align-items-end g-4 mb-4">

         <%-- Nombre --%>
         <div class="col-md-3">
             <label for="txtNombre" class="form-label font-weight-bold text-dark">Nombre</label>
             <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" MaxLength="100" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="El nombre debe contener solo letras, sin espacios ni al principio ni al final. El máximo de caracteres es 100." placeholder="Pedro" required="true" />
         </div>

         <%-- Apellido --%>
         <div class="col-md-3">
             <label for="txtApellido" class="form-label font-weight-bold text-dark">Apellido</label>
             <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" MaxLength="100" pattern="^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$" title="El apellido debe contener solo letras, sin espacios ni al principio ni al final. El máximo de caracteres es 100." placeholder="Lopez" required="true" />
         </div>

         <%-- DNI --%>
         <div class="col-md-3">
             <label for="txtDni" class="form-label font-weight-bold text-dark">Documento</label>

             <asp:TextBox runat="server" ID="txtDni" CssClass="form-control" MaxLength="8" placeholder="12345678" required="true" TextMode="SingleLine" pattern="\d{1,8}"
                 title="El documento debe contener solo números y tener hasta 8 dígitos." />

         </div>

         <%-- Fecha de Nacimiento --%>
         <div class="col-md-3">
             <label for="txtFechaNacimiento" class="form-label font-weight-bold text-dark">Fecha de Nacimiento</label>
             <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" TextMode="Date" required="true" />
         </div>

     </div>


     <asp:Panel ID="panelAsignarTurno" runat="server">
         <!-- Acordeon de turno paciente-->
         <div class="accordion" id="accordionExample">
             <div class="accordion-item">
                 <h2 class="accordion-header">
                     <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                         Especialidad
                     </button>
                 </h2>
                 <div id="collapseOne" class="accordion-collapse collapse show" data-bs-parent="#accordionExample">
                     <div class="accordion-body" style="place-self: center">


                         <asp:Repeater ID="repEspecialidades" runat="server">
                             <ItemTemplate>
                                 <div class="form-check form-check-inline align-items-center ">
                                     <input class="form-check-input" type="radio" name="especialidad"
                                         id="esp_<%# Eval("Id") %>" value='<%# Eval("Id") %>' />
                                     <label class="form-check-label" for="esp_<%# Eval("Id") %>">
                                         <%# Eval("Descripcion") %>
                                     </label>
                                 </div>
                             </ItemTemplate>
                         </asp:Repeater>

                     </div>
                     <button type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo">Siguiente paso</button>
                 </div>
             </div>

             <div class="accordion-item">
                 <h2 class="accordion-header">
                     <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                         Sugerencias de horarios
        
                     </button>
                 </h2>
                 <div id="collapseTwo" class="accordion-collapse collapse" data-bs-parent="#accordionExample">
                     <div class="accordion-body">
                         <strong>3 HORARIOS</strong>

                     </div>
                     <button type="button" data-bs-toggle="collapse" data-bs-target="#collapseThree">*ok* cerrar 2 y abrir 3</button>
                 </div>
             </div>
             <div class="accordion-item">
                 <h2 class="accordion-header">
                     <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                         Medico carga manual
                     </button>
                 </h2>
                 <div id="collapseThree" class="accordion-collapse collapse" data-bs-parent="#accordionExample">
                     <div class="accordion-body">
                         Seleccionar Medico DIA Y HORARIO

                     </div>
                     <button type="button" data-bs-toggle="collapse" data-bs-target="#collapseFour">*ok* cerrar 2 y abrir 3</button>
                 </div>
             </div>
             <div class="accordion-item">
                 <h2 class="accordion-header">
                     <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseFour" aria-expanded="false" aria-controls="collapseFour">
                         TURNO ASIGNADO
                     </button>
                 </h2>
                 <div id="collapseFour" class="accordion-collapse collapse" data-bs-parent="#accordionExample">
                     <div class="accordion-body">
                         Observaciones y nro de turno
                     </div>
                     <button>resumen</button>
                 </div>
             </div>
         </div>
         <!-- Acordeon de Turno Paciente -->
     </asp:Panel>
 </div>

</asp:Content>
