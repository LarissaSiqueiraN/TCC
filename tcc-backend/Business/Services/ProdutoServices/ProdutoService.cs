using AutoMapper;
using Business.Intefaces;
using Business.Services.Base;
using Business.Services.ProdutoServices.Interfaces;
using DAL.DTOs;
using DAL.DTOs.ProdutoDto;
using DAL.DTOs.ProdutosDto;
using DAL.Models;
using DAL.Models.ProdutoModels;
using DAL.Repository.Interfaces;
using DAL.Repository.ProdutoRepositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private IProdutoRepository _produtoRepository;
        private IUsuarioRepository _usuarioRepository;
        private IMapper _mapper;
        private ILogger<ProdutoService> _logger;

        public ProdutoService(INotificador notificador, 
                                IConfiguration configuration, 
                                IProdutoRepository produtoRepository, 
                                IUsuarioRepository usuarioRepository,
                                IMapper mapper, 
                                ILogger<ProdutoService> logger) : base(notificador, configuration)
        {
           _produtoRepository = produtoRepository;
           _usuarioRepository = usuarioRepository;
           _mapper = mapper;
           _logger = logger;
        }
        public async Task<IEnumerable<ProdutoReadDto>> GetProdutos()
        {
            return await _produtoRepository.GetProdutos();
        }

        public async Task<IEnumerable<ComboIdIntDto>> GetCombo()
        {
            return await _produtoRepository.GetCombo();
        }

        public async Task<int> Adicionar(ProdutoCadastroViewModel novoproduto, string usuarioId)
        {

            Usuario usuario = await _usuarioRepository.ObterUsuarioPorId(usuarioId);

            Produto produto = _mapper.Map<Produto>(novoproduto);

            return _produtoRepository.Adicionar(produto).Result.Id;
        }

        public async Task Atualizar(Produto produto, int id)
        {
            produto.Id = id;

            await _produtoRepository.update(produto);
        }
        public async Task<Produto> AtualizaStatus(int id)
        {
            Produto produto = await _produtoRepository.GetProdutoById(id);
            if(produto != null)
            {
                produto.Ativo = !produto.Ativo;
            }

            await _produtoRepository.update(produto);

            return produto;
        }

        public async Task<IEnumerable<Produto>> Deletar(int id)
        {
            await _produtoRepository.Remover(id);

            IEnumerable<Produto> produtos = await _produtoRepository.ObterTodos();
            return produtos;
        }

        public async Task<ResultadoPaginado<ProdutoReadDto>> GetByFiltro(ProdutoFiltroDto produtoFiltroDto)
        {
            return await _produtoRepository.GetByFiltro(produtoFiltroDto);
        }

        public async Task<IEnumerable<ProdutoReadDto>> GetProdutosByIds(List<int> produtoIds)
        {
            try
            {
                return await _produtoRepository.GetProdutosByIds(produtoIds);
            }
            catch(Exception ex)
            {
                Notificar(ex, "Ocorreu um errp em ProdutoService: GetProdutosByIds", _logger);
                throw ex;
            }
        }

        public async Task<ProdutoReadDto> GetInformacoesCompraByProduto(int produtoId)
        {
            return await _produtoRepository.GetInformacoesCompraByProduto(produtoId);
        }

        public async Task DescontarQuantidadeProduto(EncomendaProdutosDto dto)
        {
            foreach (int produtoId in dto.Fk_Produto)
            {
                Produto produto = await _produtoRepository.GetProdutoById(produtoId);
                produto.Quantidade--;

                await _produtoRepository.update(produto);
            }
        }

    }
}
