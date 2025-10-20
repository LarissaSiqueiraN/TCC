console.log("Entrou login.js");

document.addEventListener("DOMContentLoaded", () => {
  const loginButton = document.getElementById("login-button");
  if (loginButton) {
    loginButton.addEventListener("click", login);
  }
});

document.addEventListener("DOMContentLoaded", () => {
  const cadastroButton = document.getElementById("cadastro-button");
  if (cadastroButton) {
    cadastroButton.addEventListener("click", cadastrar);
  }
});

function fecharModal() {
  document.getElementById("authModal").style.display = "none";
}

async function cadastrar() {
  console.log("Iniciando cadastro...");

  const nome = document.getElementById("cadastro-name").value;
  const login = document.getElementById("cadastro-login").value;
  const email = document.getElementById("cadastro-email").value;
  const senha = document.getElementById("cadastro-password").value;
  const confirmarSenha = document.getElementById("cadastro-confirm").value;
  const alerta = document.getElementById("cadastro-alert");
  const cadastroButton = document.getElementById("cadastro-button");

  if (
    nome.trim() === "" ||
    email.trim() === "" ||
    senha.trim() === "" ||
    confirmarSenha.trim() === "" ||
    login.trim() === ""
  ) {
    alerta.textContent = "Por favor, preencha todos os campos.";
    alerta.style.display = "block";
    return;
  }

  if (!validarEmail(email)) {
    return;
  }

  if (!validarSenha(senha, confirmarSenha)) {
    return;
  }

  const originalButtonContent = cadastroButton.innerHTML;
  cadastroButton.innerHTML = '<span class="loading-dots"></span>'; // Usa a classe de animação
  cadastroButton.disabled = true;
  alerta.style.display = "none";

  try {
    console.log("Enviando dados de cadastro para o servidor...");

    const response = await fetch("http://localhost:5000/api/Auth/Registrar", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, nome, login, senha }),
    });

    console.log("Response: ", response);

    const data = await response.json();

    if (!response.ok) {
      alerta.textContent = data.message || "Erro ao registrar. Tente novamente.";
      alerta.style.display = "block";
      return;
    }

    localStorage.setItem("login", JSON.stringify(data.login));
    localStorage.setItem("token", data.accessToken);

    console.log("Usuário registrado com sucesso!", data);

    checkLoginStatus();

    fecharModal();
  } catch (error) {
    console.error("Erro ao registrar:", error);
    alerta.textContent =
      "Não foi possível conectar ao servidor. Tente novamente mais tarde.";
    alerta.style.display = "block";
  } finally {
    cadastroButton.innerHTML = originalButtonContent; 
    cadastroButton.disabled = false;
  }
}

async function login() {
  const login = document.getElementById("login").value;
  const senha = document.getElementById("login-password").value;
  const alerta = document.getElementById("login-alert");
  const loginButton = document.getElementById("login-button");

  if (login.trim() === "" || senha.trim() === "") {
    alerta.textContent = "Por favor, preencha todos os campos.";
    alerta.style.display = "block";
    return;
  }

  const originalButtonContent = loginButton.innerHTML;
  loginButton.innerHTML = '<span class="loading-dots"></span>'; // Usa a classe de animação
  loginButton.disabled = true;
  alerta.style.display = "none";

  try {
    const response = await fetch("http://localhost:5000/api/Auth/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ login, senha }),
    });

    const data = await response.json();

    if (!response.ok) {
      alerta.textContent = data.message || "Login ou senha incorretos.";
      alerta.style.display = "block";
      return;
    }

    localStorage.setItem("login", JSON.stringify(data.login));
    localStorage.setItem("token", data.accessToken);

    console.log("Login bem-sucedido:", data);

    checkLoginStatus();

    fecharModal();
  } catch (error) {
    console.error("Erro na requisição de login:", error);
    alerta.textContent =
      "Não foi possível conectar ao servidor. Tente novamente mais tarde.";
    alerta.style.display = "block";
  } finally {
    loginButton.innerHTML = originalButtonContent; // Restaura o conteúdo original
    loginButton.disabled = false;
  }
}

function validarSenha(senha, confirmarSenha) {
  const alerta = document.getElementById("cadastro-alert");

  if (senha !== confirmarSenha) {
    if (alerta){
      alerta.textContent = "Por favor, insira a mesma senha nos dois campos.";
      alerta.style.display = "block";
    }

    return false;
  }

  return true;
}

function validarEmail(email) {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  const alerta = document.getElementById("cadastro-alert");

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
