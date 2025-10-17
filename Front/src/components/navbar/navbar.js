console.log("Navbar script loaded");

document.addEventListener('DOMContentLoaded', () => {
  checkLoginStatus();
});

function checkLoginStatus() {
  const userActionsContainer = document.getElementById('user-actions-container');
  const token = localStorage.getItem('token');
  const login = localStorage.getItem('login').replace(/"/g, ''); 

  if (token) {
    // Se o token existir, cria o botão de perfil com dropdown
    userActionsContainer.innerHTML = `
      <div class="user-profile-dropdown">
        <button class="btn btn-profile" id="user-profile-btn">
          <i class="fas fa-user-circle"></i> ${login}
        </button>
        <div class="dropdown-content" id="user-dropdown-content">
          <a href="#" onclick="logout()">
            <i class="fas fa-sign-out-alt"></i> Sair
          </a>
        </div>
      </div>
    `;

    // Adiciona o evento para mostrar/esconder o dropdown
    const userProfileBtn = document.getElementById('user-profile-btn');
    const dropdownContent = document.getElementById('user-dropdown-content');
    
    userProfileBtn.addEventListener('click', (event) => {
      event.stopPropagation(); // Impede que o clique se propague e feche o menu
      dropdownContent.classList.toggle('show');
    });

  } else {
    // Se não existir, mostra o botão de Login
    userActionsContainer.innerHTML = `
      <button class="btn btn-login" onclick="openAuthModal('login')">
        <i class="fas fa-user"></i> Login
      </button>
    `;
  }
}

// Fecha o dropdown se o usuário clicar fora dele
window.addEventListener('click', (event) => {
  const dropdownContent = document.getElementById('user-dropdown-content');
  // Verifica se o dropdown existe e se o clique foi fora do container do dropdown
  if (dropdownContent && !event.target.closest('.user-profile-dropdown')) {
    if (dropdownContent.classList.contains('show')) {
      dropdownContent.classList.remove('show');
    }
  }
});

function logout() {
  localStorage.removeItem('token');
  checkLoginStatus();
}

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
