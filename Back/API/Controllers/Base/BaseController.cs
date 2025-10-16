using Business.Intefaces;
using Business.Notificacoes;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace GestaoDePlantao.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        private readonly INotificador _notificador;
        protected string UsuarioId { get; set; }
        protected bool UsuarioAutenticado { get; set; }

        protected BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected string GetUsuarioId()
        {
            try
            {
                ClaimsIdentity userContext = (ClaimsIdentity)HttpContext.User.Identity;
                if (userContext.Claims.Any() && userContext.Claims.Any(x => x.Type.Contains("nameidentifier")))
                    UsuarioId = userContext.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier")).Value;
                return UsuarioId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected string GetUsuarioNome()
        {
            try
            {
                ClaimsIdentity userContext = (ClaimsIdentity)HttpContext.User.Identity;
                if (userContext.Claims.Any() && userContext.Claims.Any(x => x.Type.Contains("givenname")))
                    return userContext.Claims.FirstOrDefault(x => x.Type.Contains("givenname")).Value;
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        //protected CategoriaUsuarioEnum? GetUsuarioPerfil()
        //{
        //    try
        //    {
        //        ClaimsIdentity userContext = (ClaimsIdentity)HttpContext.User.Identity;
        //        if (userContext.Claims.Any() && userContext.Claims.Any(x => x.Type.Contains("role")))
        //        {
        //            var perfil = userContext.Claims.FirstOrDefault(x => x.Type.Contains("role")).Value.ToLower().FirstCharToUpper();
        //            return (CategoriaUsuarioEnum)Enum.Parse(typeof(CategoriaUsuarioEnum), perfil);
        //        }

        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }
        protected ActionResult CustomResponse<T>(ResponseDto<T> result)
        {
            if (result.hasErrors)
            {
                foreach (var errorMsg in result.getErrors)
                    NotificarErro(errorMsg);

                return BadRequest(new
                {
                    success = false,
                    errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
                });
            }

            return Ok(result.getResponse);
        }
        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(result);
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }
        protected void GravaException(Exception ex, string mensagem, ILogger _logger)
        {
            _logger.LogError(ex, mensagem);
            NotificarErro(mensagem);
        }

    }
}
