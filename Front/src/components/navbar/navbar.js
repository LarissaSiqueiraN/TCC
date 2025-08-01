console.log("Navbar script loaded");

function openAuthModal(form = "login") {
  document.getElementById("authModal").style.display = "flex";
  switchAuthForm(form);
}

function switchAuthForm(form) {
  if (form === "login") {
    document.getElementById("loginForm").classList.add("active");
    document.getElementById("registerForm").classList.remove("active");
    document.getElementById("authModalTitle").textContent = "Entrar";
  } else {
    document.getElementById("loginForm").classList.remove("active");
    document.getElementById("registerForm").classList.add("active");
    document.getElementById("authModalTitle").textContent = "Cadastrar";
  }
}
