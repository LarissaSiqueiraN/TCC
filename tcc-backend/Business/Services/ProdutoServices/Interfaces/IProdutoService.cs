using DAL.DTOs;
using DAL.DTOs.ProdutoDto;
using DAL.DTOs.ProdutosDto;
using DAL.Models;
using DAL.Models.ProdutoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services.ProdutoServices.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoReadDto>> GetProdutos();

        Task<IEnumerable<ComboIdIntDto>> GetCombo();

        Task<int> Adicionar(ProdutoCadastroViewModel produto, string usuarioId);

        Task Atualizar(Produto produto, int id);

        Task<Produto> AtualizaStatus(int id);

        Task<IEnumerable<Produto>> Deletar(int id);

        Task<ResultadoPaginado<ProdutoReadDto>> GetByFiltro(ProdutoFiltroDto clienteFiltroDto);

        Task<IEnumerable<ProdutoReadDto>> GetProdutosByIds(List<int> produtoIds);

        Task<ProdutoReadDto> GetInformacoesCompraByProduto(int produtoId);

        Task DescontarQuantidadeProduto(EncomendaProdutosDto dto);
    }
}
