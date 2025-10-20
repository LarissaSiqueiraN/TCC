using Business.Intefaces;
using Business.Services.Interfaces;
using Controllers.Base;
using DAL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AnaliseController : BaseController
    {
        private readonly INotificador _notificador;
        private readonly IAnaliseService _analiseService;

        public AnaliseController(INotificador notificador, IAnaliseService analiseService) : base(notificador)
        {
            _analiseService = analiseService;
        }

        [HttpPost]
        public async Task<ActionResult> Cadastrar(AnaliseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotificarErro("Dados da análise estão incorretos ou incompletos");
                return CustomResponse();
            }
            try
            {
                var usuarioId = GetUsuarioId();
                model.Fk_Usuario = usuarioId;

                return CustomResponse(await _analiseService.Cadastrar(model));
            }
            catch (Exception ex)
            {
                NotificarErro("Ocorreu um erro ao processar a análise.");
                return CustomResponse();
            }
        }

        [HttpGet("GetAnalisesByUsuario")]
        public async Task<ActionResult> GetAnalisesByUsuario()
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var analises = await _analiseService.GetAnalisesByUsuario(usuarioId);
                return CustomResponse(analises);
            }
            catch (Exception ex)
            {
                NotificarErro("Ocorreu um erro ao buscar as análises do usuário.");
                return CustomResponse();
            }
        }
    }
}