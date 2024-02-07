using DAL.DTOs;
using DAL.DTOs.ProdutosDto;
using System.Threading.Tasks;

namespace Business.Services.ProdutoServices.Interfaces
{
    public interface IProdutoFotoService
    {
        Task<ProdutoFotoRotaDto> GetImagensByProduto(int produtoId);
        Task<ProdutoFotoCaminhoDto> GetImagemByProduto(int produtoId);
        Task<string> GetImagensByFiltro(int produtoId);
        Task AdicionarFoto(AdicionarFotoViewModel model);
        Task DeletarImagemByProduto(int produtoId);
        Task DeletarDiretorioImagemByProduto(int produtoId);
    }
}
