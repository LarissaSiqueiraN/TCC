using DAL.DTOs;
using DAL.DTOs.ProdutosDto;
using DAL.Models;
using DAL.Models.ProdutoModels;
using DAL.Repository.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repository.ProdutoRepositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<ProdutoReadDto>> GetProdutos();
        Task<Produto> GetProdutoById(int id);
        Task<IEnumerable<ComboIdIntDto>> GetCombo();
        Task<IEnumerable<ComboIdIntDto>> GetComboPorDescricao(string nome);
        Task<ResultadoPaginado<ProdutoReadDto>> GetByFiltro(ProdutoFiltroDto dto);
        Task<IEnumerable<ProdutoReadDto>> GetProdutosByIds(List<int> id);
        Task<ProdutoReadDto> GetInformacoesCompraByProduto(int produtoId);
    }
}
