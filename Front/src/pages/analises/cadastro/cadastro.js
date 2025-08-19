console.log("Entrou Analises/Cadastro.js");

window.addEventListener('DOMContentLoaded', function() {
    const params = new URLSearchParams(window.location.search);
    const tipo = params.get('tipo');
    const select = document.getElementById('graph-type');
    if (tipo && select) {
        // Mapeia os valores para os options
        if (tipo === 'linha') select.value = 'linha';
        else if (tipo === 'barra') select.value = 'barra';
    }
});