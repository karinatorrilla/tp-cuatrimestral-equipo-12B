console.log("HOLAAAAaaaaa");

function generarEnlaceDisponibilidad(medicoId) {
    var ddlDia = document.getElementById('ddlDiaSemana_' + medicoId);
    var txtHoraInicio = document.getElementById('txtHoraInicio_' + medicoId);
    var txtHoraFin = document.getElementById('txtHoraFin_' + medicoId);

    var diaSeleccionado = ddlDia.value;
    var horaInicio = txtHoraInicio.value;
    var horaFin = txtHoraFin.value;

    // Validar que se seleccionó un día y las horas están completas
    if (diaSeleccionado === "" || horaInicio === "" || horaFin === "") {
        alert("Por favor, complete todos los campos de día, hora de inicio y hora de fin.");
        return false; 
    }

    // Construye la URL con los parámetros
    var url = 'Medicos.aspx?medicoId=' + medicoId +
        '&dia=' + diaSeleccionado +
        '&horaInicio=' + encodeURIComponent(horaInicio) +
        '&horaFin=' + encodeURIComponent(horaFin);

    url += '&medicoSeleccionado=' + medicoId;

    // Redirigir a la URL
    window.location.href = url;
    return false;
}