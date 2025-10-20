using API.Extensions;
using AutoMapper;
using Business.Intefaces;
using Business.Services.Interfaces;
using Controllers.Base;
using DAL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private readonly IUsuarioService _usuarioService;

        public AuthController(INotificador notificador,
                                 SignInManager<Usuario> signInManager,
                                 IMapper mapper,
                                 ILogger<AuthController> logger,
                                 IOptions<AppSettings> appSettings,
                                 RoleManager<IdentityRole> roleManager,
                                 UserManager<Usuario> userManager,
                                 IUsuarioService usuarioService) : base(notificador)
        {
            _notificador = notificador;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;   
            _appSettings = appSettings.Value;
            _roleManager = roleManager;
            _usuarioService = usuarioService;
        }

        [HttpPost("Registrar")]
        [AllowAnonymous]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                NotificarErro("Dado de cadastro do usuário estão incorretos ou incompletos");
                return CustomResponse();
            }

            try
            {
                //Verificar se o usuario existe na base da uniodonto (Revover comentario após ajuste)

                //preencher dados restantes do usuário (Revover comentario após ajuste)
                var user = new Usuario();

                user.Nome = registerUser.Nome;
                user.UserName = registerUser.Login;
                user.Email = registerUser.Email;
                user.DataCadastro = DateTime.Now;

                var result = await _userManager.CreateAsync(user, registerUser.Senha);

                if (result.Succeeded)
                {
                     await _signInManager.SignInAsync(user, true);
                    return CustomResponse(await GerarJwt(user.Nome));
                }

                foreach (var error in result.Errors)
                {
                    NotificarErro(error.Description);
                }

                UsuarioLoginDto userLoginDto = new UsuarioLoginDto();
                userLoginDto.Nome = user.Nome;

                return CustomResponse(userLoginDto);
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao gravar o usuário", _logger);
                return CustomResponse();
            }
        }

        [HttpPost("AlterarSenha")]
        public async Task<ActionResult> AlterarSenha(AlterarSenhaViewModel model, string idioma)
        {
            try
            {
                string userId = GetUsuarioId();
                Usuario user = await _usuarioService.GetUsuarioById(userId);

                if(model.Senha == model.NovaSenha)
                {
                    NotificarErro("A senha inserida não pode ser igual a anterior.");
                    return CustomResponse();
                }
                else if(! await _userManager.CheckPasswordAsync(user, model.Senha))
                {
                    NotificarErro("A senha inserida não é válida.");
                    return CustomResponse();
                }

                return CustomResponse(await _userManager.ChangePasswordAsync(user, model.Senha, model.NovaSenha));
            }
            catch(Exception ex)
            {
                GravaException(ex, "Erro ao enviar alterar senha!", _logger);
                return CustomResponse();
            }
        }

        //[HttpPost("EnviarEmailRedefinicaoSenha")]
        //[AllowAnonymous]
        //public async Task<ActionResult> EnviarEmailRedefinicaoSenha(EsqueciSenhaViewModel model, string idioma)
        //{
        //    try
        //    {
        //        if(! await _verificaUser.VerificarRedefinicaoSenha(model.Usuario, model.CpfCnpj))
        //        {
        //            NotificarErro(_traducaoService.GetTraducao("login.emailRedefinicao", idioma));
        //            return CustomResponse();
        //        }

        //        return CustomResponse(await _mailService.EnviarEmailEsqueciSenha(model.Usuario, model.UrlRedefinicao, idioma));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Erro ao enviar e-mail! Confira os dados preenchidos.", _logger);
        //        return CustomResponse();
        //    }

        //}

        //[HttpPost("RedefinirSenhaUser")]
        //[AllowAnonymous]
        //public async Task<ActionResult> RedefinirSenhaUser(RedefinicaoSenhaViewModel model)
        //{
        //    try
        //    {
        //        return CustomResponse(await _admUserService.RedefinirSenha(model));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Não foi possivel acessar a funcionalidade 'Alteração de senha'", _logger);
        //        return CustomResponse();
        //    }
        //}

        private async Task<LoginResponseViewModel> GerarJwt(string login)
        {
            var user = await _userManager.FindByNameAsync(login);
            var claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id)); //É o assunto do token, mas é muito utilizado para guarda o ID do usuário
            claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.Nome));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras).ToShortDateString())); //Data para expiração do token
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString())); // Define uma data para qual o token não pode ser aceito antes dela
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)); // Data de criação do token
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); //O id do token
            if (!string.IsNullOrEmpty(user.Email))
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                Authenticated = true,
                Id = user.Id,
                Nome = user.Nome,
                Login = user.UserName
            };

            return response;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var result = await _signInManager.PasswordSignInAsync(loginUser.login, loginUser.senha, false, true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuario " + loginUser.login + " logado com sucesso");
                    Usuario user = await _userManager.FindByNameAsync(loginUser.login);
                    user.UltimoLogin = DateTime.Now;
                    await _userManager.UpdateAsync(user);   

                    return CustomResponse(await GerarJwt(loginUser.login));
                }

                if (result.IsLockedOut)
                {
                    NotificarErro("Usuário foi bloqueado.");
                    return CustomResponse(loginUser);
                }

                NotificarErro("Os dados inserido são inválidos.");
                return CustomResponse(loginUser);
            }
            catch
            {
                NotificarErro("Ocorreu um erro no login");
                return null;
            }
        }

        private static long ToUnixEpochDate(DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<IList<Claim>> GetRoleClaims(Usuario user, int perfil)
        {
            IList<Claim> claimsRole = new List<Claim>();
            if (_userManager.SupportsUserRole)
            {
                var rolesList = new List<string>(){ "TECNICO", "ANALISTA", "FINANCEIRO", "COMERCIAL", "AUDITOR", "ADMINISTRATIVO", "CLIENTE" };
                var userRoles = await _userManager.GetRolesAsync(user);

                //Claims definidas nas roles do Identity
                foreach (var userRole in userRoles)
                {
                    if(userRole == rolesList[perfil])
                    {
                        claimsRole.Add(new Claim("role", userRole));
                        if (_roleManager.SupportsRoleClaims)
                        {
                            var role = await _roleManager.FindByNameAsync(userRole);
                            if (role != null)
                            {
                                var roleClaim = await _roleManager.GetClaimsAsync(role);
                                foreach (var rClaim in roleClaim)
                                {
                                    claimsRole.Add(rClaim);
                                }
                            }
                        }
                    }

                }
            }
            return claimsRole;
        }
    }
}