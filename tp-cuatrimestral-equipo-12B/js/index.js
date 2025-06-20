console.log("HOLAAAAaaaaa");
var selectEspecialidades = document.getElementById('<%= lstEspecialidades.ClientID %>');
var lblConteo = document.getElementById('lblCantidadEspecialidadesSeleccionadas');

function actualizarConteoEspecialidades() {
    // Ya no debería ser null si el script está bien posicionado,
    // pero mantenemos la verificación como buena práctica.
    if (!selectEspecialidades || !lblConteo) {
        console.error("Error: selectEspecialidades o lblConteo son null. Verifica la ubicación del script o los IDs.");
        return;
    }

    var seleccionadas = 0;
    for (var i = 0; i < selectEspecialidades.options.length; i++) {
        if (selectEspecialidades.options[i].selected) {
            seleccionadas++;
        }
    }

    if (seleccionadas > 0) {
        lblConteo.innerText = seleccionadas + ' seleccionadas';
        lblConteo.style.display = 'inline-block'; // Muestra el badge
    } else {
        lblConteo.innerText = ''; // Limpia el texto
        lblConteo.style.display = 'none'; // Oculta el badge
    }
}

// Ejecutar la función para el estado inicial
actualizarConteoEspecialidades();

// Asignar el evento 'change' para actualizar el conteo cuando la selección cambie
selectEspecialidades.addEventListener('change', function () {
    actualizarConteoEspecialidades();
});