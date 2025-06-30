console.log("HOLAAAAaaaaa");

function generarEnlaceDisponibilidad(medicoId) {
    var diaSeleccionado = $('#ddlDiaSemana_' + medicoId).val();
    var horaInicio = $('#txtHoraInicio_' + medicoId).val();
    var horaFin = $('#txtHoraFin_' + medicoId).val();
    // Obtener el ID de la disponibilidad del campo oculto
    var idDisponibilidad = $('#hdnDisponibilidadId_' + medicoId).val();

    // Validar que se seleccionó un día y las horas están completas
    if (diaSeleccionado === "" || horaInicio === "" || horaFin === "") {
        alert("Por favor, complete todos los campos de día, hora de inicio y hora de fin.");
        return false;
    }

    // Construye la URL 
    var url = 'Medicos.aspx?medicoId=' + medicoId +
        '&dia=' + diaSeleccionado +
        '&horaInicio=' + encodeURIComponent(horaInicio) +
        '&horaFin=' + encodeURIComponent(horaFin);

    // Si el campo oculto tiene un valor, significa que es una actualización
    if (idDisponibilidad !== "") {
        url += '&actualizarDisponibilidad=' + idDisponibilidad; // Añade el ID de la disponibilidad a actualizar
    }

    url += '&medicoSeleccionado=' + medicoId;

    // Redirigir a la URL
    window.location.href = url;
    return false;
}

// --- Función para la precarga de datos al editar ---
function editarDisponibilidad(medicoId, idDisponibilidad, diaSemana, horaInicio, horaFin) {
    console.log("medicoId", medicoId);
    console.log("idDisponibilidad", idDisponibilidad);
    console.log("diaSemana", diaSemana);


    //Precargar los campos 
    $('#ddlDiaSemana_' + medicoId).val(diaSemana);
    $('#txtHoraInicio_' + medicoId).val(horaInicio);
    $('#txtHoraFin_' + medicoId).val(horaFin);

    // Este ID sera usado luego para saber si se debe hacer un INSERT o un UPDATE
    $('#hdnDisponibilidadId_' + medicoId).val(idDisponibilidad);

    //Cambiar el texto del botón de "Agregar" a "Actualizar"
    var btnAgregar = $('#btnAgregarDisponibilidad_' + medicoId);
    btnAgregar.text('Actualizar');

    return false;
}

window.limpiarCamposModalDisponibilidad = function (medicoId) {
    // Seleccionamos los campos dentro del modal y los vaciamos
    $('#ddlDiaSemana_' + medicoId).val('');
    $('#txtHoraInicio_' + medicoId).val(''); 
    $('#txtHoraFin_' + medicoId).val(''); 

    // limpia el campo oculto del ID de disponibilidad y restaura el botón a "Agregar"
    $('#hdnDisponibilidadId_' + medicoId).val('');
    $('#btnAgregarDisponibilidad_' + medicoId).text('Agregar');
};

const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))