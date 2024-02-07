using API.Extensions;
using AutoMapper;
using Business.Intefaces;
using Business.Services;
using Business.Services.Base;
using Business.Services.Interfaces;
using DAL.DTOs;
using DAL.Models;
using GestaoDePlantao.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDePlantao.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class AuthController : BaseController
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly RoleManager<IdentityRole> _roleManager;
        public INotificador _notificador { get; set; }
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private readonly AdmUserService _admUserService;
        private readonly MailService _mailService;
        private readonly IEmailService _emailService;

        public AuthController(INotificador notificador,
                                 SignInManager<Usuario> signInManager,
                                 IMapper mapper,
                                 ILogger<AuthController> logger,
                                 IOptions<AppSettings> appSettings,
                                 RoleManager<IdentityRole> roleManager,
                                 UserManager<Usuario> userManager,
                                 MailService mailService,
                                 IEmailService emailService) : base(notificador)
        {
            _notificador = notificador;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
            _appSettings = appSettings.Value;
            _roleManager = roleManager;
            _mailService = mailService;
            _emailService = emailService;
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
            else if (!_emailService.ValidaEmail(registerUser.Email))
            {
                NotificarErro("Formato de email inválido");
                return CustomResponse();

            }

            try
            {
                //Verificar se o usuario existe na base da uniodonto (Revover comentario após ajuste)

                //preencher dados restantes do usuário (Revover comentario após ajuste)
                var user = new Usuario
                {
                    //Admin = registerUser.Admin,
                    //Deve ser verificado se o padrão é iniciar ativo (Revover comentario após ajuste)
                    Ativo = true,
                    Nome = registerUser.Nome,
                    UserName = registerUser.Login,
                    Email = registerUser.Email,
                    Cpf = registerUser.cpf,
                    DataNascimento = DateTime.Now
                    //Perfil = registerUser.Perfil
                };


                var result = await _userManager.CreateAsync(user, registerUser.Senha);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return CustomResponse(await GerarJwt(user.UserName));
                }
                foreach (var error in result.Errors)
                {
                    NotificarErro(error.Description);
                }

                var userDto = _mapper.Map<UsuarioDto>(user);

                return CustomResponse(userDto);
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao gravar o usuário", _logger);
                return CustomResponse();
            }
        }

        private async Task<LoginResponseViewModel> GerarJwt(string login)
        {
            var user = await _userManager.FindByNameAsync(login);
            var claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id)); //É o assunto do token, mas é muito utilizado para guarda o ID do usuário
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras).ToShortDateString())); //Data para expiração do token
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString())); // Define uma data para qual o token não pode ser aceito antes dela
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)); // Data de criação do token
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); //O id do token
            
            if (!string.IsNullOrEmpty(user.Email))
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            //Busca as Claims das Roles
            foreach (var roleClaim in await GetRoleClaims(user))
            {
                claims.Add(roleClaim);
            }


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
                Id = user.Id
            };

            return response;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.login, loginUser.password, false, true);

            if (result.Succeeded)
            {
                _logger.LogInformation("Usuario " + loginUser.login + " logado com sucesso");
                return CustomResponse(await GerarJwt(loginUser.login));
            }

            if (result.IsLockedOut)
            {
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }

            NotificarErro("Usuário ou Senha incorretos");
            return CustomResponse(loginUser);
        
}
        private static long ToUnixEpochDate(DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<IList<Claim>> GetRoleClaims(Usuario user)
        {
            IList<Claim> claimsRole = new List<Claim>();
            if (_userManager.SupportsUserRole)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                //Claims definidas nas roles do Identity
                foreach (var userRole in userRoles)
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
            return claimsRole;
        }

    }
}
