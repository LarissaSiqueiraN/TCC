console.log("Entrou inicio.js");

fetch('./components/navbar/navbar.html')
    .then(response => response.text())
    .then(html => {
        document.getElementById('navbar-container').innerHTML = html;
    });