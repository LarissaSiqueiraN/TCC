using Business.Intefaces;
using Business.Services.Base;
using Business.Services.Interfaces;
using DAL.DTOs;
using DAL.Models.EncomendaModels;
using GestaoDePlantao.Controllers.Base;
using GestaoDePlantao.Controllers.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class EncomendaController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IEncomendaService _encomendaService;
        private readonly MailService _mailService;

        public EncomendaController(INotificador notificador,
                                    ILogger<AuthController> logger,
                                    IEncomendaService encomendaService, 
                                    MailService mailService) : base(notificador)
        {
            _logger = logger;
            _encomendaService = encomendaService;
            _mailService = mailService;
        }

        [HttpPost("GetByFiltro")]
        public async Task<ActionResult> GetByFiltro(EncomendaFiltroDto dto)
        {
            try
            {
                return CustomResponse(await _encomendaService.GetByFiltro(dto));
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao buscar encomendas", _logger);
                return CustomResponse();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CadastrarEncomenda(EncomendaCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    NotificarErro("Dado de cadastro da encomenda estão incorretos ou incompletos.");
                    return CustomResponse();
                }

                string usuarioId = GetUsuarioId();

                Encomenda encomenda = await _encomendaService.Adicionar(dto, usuarioId);
                await _mailService.EnviarEmailCadastroEncomenda(encomenda);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao cadastrar encomenda", _logger);
                return CustomResponse();
            }
        }

        [HttpPut("Status/{id}")]
        public async Task<ActionResult> AtualizarStatus(int id)
        {
            try
            {
                await _encomendaService.AtualizarStatus(id);

                return CustomResponse();
            }

            catch (Exception ex)
            {
                GravaException(ex, "Falha ao cadastrar AtualizarStatus", _logger);
                return CustomResponse();
            }
        }

    }
}
