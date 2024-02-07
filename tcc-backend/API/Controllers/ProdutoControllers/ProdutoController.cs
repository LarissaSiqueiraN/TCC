using Business.Intefaces;
using Business.Services.ProdutoServices.Interfaces;
using DAL.DTOs;
using DAL.DTOs.ProdutoDto;
using DAL.DTOs.ProdutosDto;
using DAL.Models.ProdutoModels;
using GestaoDePlantao.Controllers.Base;
using GestaoDePlantao.Controllers.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.ProdutoControllers
{
    public class ProdutoController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IProdutoService _produtoService;

        public ProdutoController(INotificador notificador,
                                    ILogger<AuthController> logger,
                                    IProdutoService produtoService) : base(notificador)
        {
            _logger = logger;
            _produtoService = produtoService;
        }

        [HttpGet]
        [AllowAnonymous]

        public async Task<ActionResult> GetProdutos()
        {
            try
            {
                return CustomResponse(await _produtoService.GetProdutos());
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao buscar produtos", _logger);
                return CustomResponse();
            }
        }

        [HttpGet("combo")]
        public async Task<ActionResult> GetCombo()
        {
            try
            {
                return CustomResponse(await _produtoService.GetCombo());
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao buscar getCombos de divisão", _logger);
                return CustomResponse();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CadastrarProduto(ProdutoCadastroViewModel produto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    NotificarErro("Dado de cadastro do produto estão incorretos ou incompletos");
                    return CustomResponse();
                }

                string usuarioId = GetUsuarioId();

                return CustomResponse(await _produtoService.Adicionar(produto, usuarioId));
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao cadastrar produto", _logger);
                return CustomResponse();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> AtualizarProduto(Produto produto, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    NotificarErro("Dado de cadastro do funcionario estão incorretos ou incompletos");
                    return CustomResponse();
                }

                await _produtoService.Atualizar(produto, id);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                GravaException(ex, "Erro ao atualizar produto", _logger);
                return CustomResponse();
            }
        }

        [HttpPut("status/{id}")]
        public async Task<ActionResult> AtualizaStatusProduto(int id)
        {
            try
            {
                return CustomResponse(await _produtoService.AtualizaStatus(id));
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao atualizar status de produto", _logger);
                return CustomResponse();
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletarProduto(int id)
        {
            try
            {
                return CustomResponse(await _produtoService.Deletar(id));
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao deletar produto", _logger);
                return CustomResponse();
            }
        }

        [AllowAnonymous]
        [HttpPost("GetByFiltro")]
        public async Task<ActionResult> GetByFiltro(ProdutoFiltroDto produtoFiltroDto)
        {
            try
            {
                return CustomResponse(await _produtoService.GetByFiltro(produtoFiltroDto));
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao buscar produtos por filtro", _logger);
                return CustomResponse();
            }
        }

        [AllowAnonymous]
        [HttpPost("GetProdutosByIds")]
        public async Task<ActionResult> GetProdutosByIds(List<int> produtoIds)
        {
            try
            {
                return CustomResponse(await _produtoService.GetProdutosByIds(produtoIds));
            }
            catch (Exception ex) 
            {
                GravaException(ex, "Falha ao buscar produtos por ids", _logger);
                return CustomResponse();
            }
        }

        [AllowAnonymous]
        [HttpGet("GetInformacoesCompraByProduto/{produtoId}")]
        public async Task<ActionResult> GetInformacoesCompraByProduto(int produtoId)
        {
            try
            {
                return CustomResponse(await _produtoService.GetInformacoesCompraByProduto(produtoId));
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao buscar informações de compra do produto.", _logger);
                return CustomResponse();
            }
        }

        [HttpPost("DescontarQuantidadeProduto")]
        [AllowAnonymous]
        public async Task<ActionResult> DescontarQuantidadeProduto(EncomendaProdutosDto dto)
        {
            try
            {
                await _produtoService.DescontarQuantidadeProduto(dto);
                return CustomResponse();
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao descontar quantidade do produto.", _logger);
                return CustomResponse();
            }
        }
    }
}
