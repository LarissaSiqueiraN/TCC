console.log("Entrou login.js");

function fecharModal() {
  document.getElementById("authModal").style.display = "none";
}

function login() {
  const email = document.getElementById("login-email").value.trim();
  const senha = document.getElementById("login-password").value;

  if (!validarEmail(email)) return;

  // Continue com o login normalmente
  console.log("Email:", email);
  console.log("Senha:", senha);
  // ...enviar para backend ou outras ações
}

function validarEmail(email) {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  const alerta = document.getElementById("login-alert");

  if (!emailRegex.test(email)) {
    if (alerta) {
      alerta.textContent = "Por favor, insira um e-mail válido.";
      alerta.style.display = "block";
    }
    return false;
  }

  if (alerta) {
    alerta.textContent = "";
    alerta.style.display = "none";
  }

  return true;
}
