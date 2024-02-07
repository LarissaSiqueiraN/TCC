using DAL.DTOs.ProdutosDto;
using DAL.Models.ProdutoModels;
using DAL.Repository.Base;
using System.Threading.Tasks;

namespace DAL.Repository.ProdutoRepositories.Interfaces
{
    public interface IProdutoFotoRepository : IRepository<ProdutoFoto>
    {
        Task<ProdutoFotoCaminhoDto> GetImagemByProduto(int produtoId);
        Task<int> GetImagemIdByProduto(int produtoId);

    }
}
