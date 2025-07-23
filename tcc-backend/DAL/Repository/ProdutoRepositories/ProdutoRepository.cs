using DAL.DTOs;
using DAL.Repository.Base;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DAL.Repository.ProdutoRepositories.Interfaces;
using DAL.Models.ProdutoModels;
using DAL.DTOs.ProdutosDto;
using System;
using DAL.Models;

namespace DAL.Repository.ProdutoRepositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        private readonly BancoAPIContext _db;

        public ProdutoRepository(BancoAPIContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProdutoReadDto>> GetProdutos()
        {
            return await _db.Produtos.Where(x => x.Id == x.Id).Select(x => new ProdutoReadDto()
            {
                Id = x.Id,
                Descricao = x.Descricao,
                Nome = x.Nome,
                Valor = x.Valor,
                Quantidade = x.Quantidade
            }).ToListAsync();
        }

        public async Task<Produto> GetProdutoById(int id)
        {
            return await _db.Produtos.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ComboIdIntDto>> GetCombo()
        {
            IEnumerable<ComboIdIntDto> combos = await Db.Produtos
                .Where(x => x.Ativo == true).Select(x => new ComboIdIntDto()
                {
                    Id = x.Id,
                    Descricao = x.Nome.ToString()
                }).ToListAsync();
            return combos;
        }

        public async Task<IEnumerable<ComboIdIntDto>> GetComboPorDescricao(string nome)
        {
            IEnumerable<ComboIdIntDto> combos = await Db.Produtos
            .Where(x => x.Nome.ToString().Contains(nome)).Select(x => new ComboIdIntDto()
            {
                Id = x.Id,
                Descricao = x.Nome.ToString()
            }).ToListAsync();
            return combos;
        }
        public async Task<IEnumerable<Produto>> ObterPorNome(string nome)
        {
            IEnumerable<Produto> produtos = await Buscar(x => x.Nome.Contains(nome));
            return produtos;
        }
        public async Task<IEnumerable<Produto>> ObterPorValor(double valor)
        {
            return await Buscar(x => x.Valor == valor);
        }
        public async Task<IEnumerable<Produto>> ObterPorDescricao(string descricao)
        {
            IEnumerable<Produto> produtos = await Buscar(x => x.Descricao.Contains(descricao));
            return produtos;
        }
        public async Task<IEnumerable<Produto>> ObterPorVendido(bool vendido)
        {
            IEnumerable<Produto> produtos = await Buscar(x => x.Vendido == vendido);
            return produtos;
        }
        public async Task<IEnumerable<Produto>> ObterPorDataVenda(string dataVenda)
        {
            IEnumerable<Produto> produtos = await Buscar(x => x.DataVenda.ToString() == dataVenda);
            return produtos;
        }

        public async Task<ResultadoPaginado<ProdutoReadDto>> GetByFiltro(ProdutoFiltroDto dto)
        {
            var query = _db.Produtos.AsQueryable();

            query = AplicarFiltros(dto, query);

            var totalItens = query.Count();
            var produtos = Paginar(query, dto.Pagina,dto.ItensPagina);

            var produtosLista = await produtos.Select(x => new ProdutoReadDto()
            {
                Id = x.Id,
                DataVenda = x.DataVenda.ToString(),
                Descricao = x.Descricao,
                Vendido = x.Vendido,
                ModificadoPor = x.ModificadoPor,
                Nome = x.Nome,
                Quantidade = x.Quantidade,
                UltimaModificacao = x.UltimaModificacao.ToString(),
                Valor = x.Valor,
            }).ToListAsync();

            return new ResultadoPaginado<ProdutoReadDto>(produtosLista, totalItens, dto.Pagina, dto.ItensPagina);
        }

        private IQueryable<Produto> AplicarFiltros(ProdutoFiltroDto filtro, IQueryable<Produto> query)
        {
            if (!string.IsNullOrEmpty(filtro.Nome))
                query = query.Where(x => x.Nome.Contains(filtro.Nome));

            if (!string.IsNullOrEmpty(filtro.Descricao))
                query = query.Where(x => x.Descricao.Contains(filtro.Descricao));

            if(!string.IsNullOrEmpty(filtro.ModificadoPor))
                query = query.Where(x => x.ModificadoPor.Contains(filtro.ModificadoPor));

            if (!string.IsNullOrEmpty(filtro.Id))
                query = query.Where(x => x.Id == int.Parse(filtro.Id));

            if (filtro.Valor.HasValue) 
                query = query.Where(x => x.Valor == filtro.Valor);

            if(filtro.Vendido.HasValue)
                query = query.Where(x => x.Vendido == filtro.Vendido);

            if(!string.IsNullOrEmpty(filtro.DataVenda))
                query = query.Where(x => x.DataVenda == DateTime.Parse(filtro.DataVenda));

            if(!string.IsNullOrEmpty(filtro.UltimaModificacao))
                query = query.Where(x => x.UltimaModificacao == DateTime.Parse(filtro.UltimaModificacao));

            return query;
        }

        public async Task<IEnumerable<ProdutoReadDto>> GetProdutosByIds(List<int> produtoIds)
        {
            var produtosEncontrados = await _db.Produtos
                .Where(x => produtoIds.Contains(x.Id))
                .ToListAsync();

            return produtosEncontrados.SelectMany(x => produtoIds.Where(id => id == x.Id)
                .Select(_ => new ProdutoReadDto
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Valor = x.Valor,
                    Descricao = x.Descricao,
                })).ToList();
        }

        public async Task<ProdutoReadDto> GetInformacoesCompraByProduto(int produtoId)
        {
            return await _db.Produtos.Where(x => x.Id == produtoId).Select(x => new ProdutoReadDto()
            {
                Nome = x.Nome,
                Descricao = x.Descricao,
                Valor = x.Valor,
                Quantidade = x.Quantidade
            }).FirstOrDefaultAsync();
        }
    }
}
