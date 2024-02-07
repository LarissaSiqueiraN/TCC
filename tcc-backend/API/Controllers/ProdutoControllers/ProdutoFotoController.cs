using Business.Intefaces;
using Business.Services.ProdutoServices.Interfaces;
using DAL.DTOs;
using GestaoDePlantao.Controllers.Base;
using GestaoDePlantao.Controllers.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.ProdutoControllers
{
    public class ProdutoFotoController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IProdutoFotoService _produtoFotoService;

        public ProdutoFotoController(INotificador notificador,
                            ILogger<AuthController> logger,
                            IProdutoFotoService produtoFotoService) : base(notificador)
        {
            _logger = logger;
            _produtoFotoService = produtoFotoService;
        }

        [HttpGet("{produtoId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetImagensByProduto(int produtoId)
        {
            try
            {
                return CustomResponse(await _produtoFotoService.GetImagensByProduto(produtoId));
            }
            catch (Exception ex)
            {
                GravaException(ex, "Erro ao buscar imagem do produto", _logger);
                return CustomResponse();
            }
        }

        [HttpGet("GetImagemByProduto/{produtoId}")]
        public async Task<ActionResult> GetImagemByProduto(int produtoId)
        {
            try
            {
                return CustomResponse(await _produtoFotoService.GetImagemByProduto(produtoId));
            }
            catch (Exception ex)
            {
                GravaException(ex, "Erro ao buscaro imagem do produto", _logger);
                return CustomResponse();
            }
        }

        [HttpPut]
        public async Task<ActionResult> AtualizarImagem(int produtoId, List<IFormFile> imagens)
        {
            try
            {
                await _produtoFotoService.DeletarImagemByProduto(produtoId);

                AdicionarFotoViewModel model = new AdicionarFotoViewModel()
                {
                    ProdutoId = produtoId,
                    Files = imagens
                };

                await _produtoFotoService.AdicionarFoto(model);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                GravaException(ex, "Erro ao atualizar imagem do produto.", _logger);
                return CustomResponse();
            }
        }

        [HttpDelete("{produtoId}")]
        public async Task<ActionResult> DeletarDirotorioImagemByProduto(int produtoId)
        {
            try
            {
                await _produtoFotoService.DeletarDiretorioImagemByProduto(produtoId);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                GravaException(ex, "Erro ao deletar imagem do produto.", _logger);
                return CustomResponse();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CadastrarImagens(int produtoId, List<IFormFile> imagens)
        {
            try
            {
                AdicionarFotoViewModel model = new AdicionarFotoViewModel()
                {
                    ProdutoId = produtoId,
                    Files = imagens
                };

                await _produtoFotoService.AdicionarFoto(model);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                GravaException(ex, "Erro ao cadastrar imagens ao produto", _logger);
                return CustomResponse();
            }
        }
    }
}
