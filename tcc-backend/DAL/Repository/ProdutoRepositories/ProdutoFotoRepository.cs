using DAL.DTOs.ProdutosDto;
using DAL.Models.ProdutoModels;
using DAL.Repository.Base;
using DAL.Repository.ProdutoRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository.ProdutoRepositories
{
    public class ProdutoFotoRepository : Repository<ProdutoFoto>, IProdutoFotoRepository
    {
        public BancoAPIContext _db { get; set; }

        public ProdutoFotoRepository(BancoAPIContext db) : base (db) 
        {
            _db = db;
        }

        public async Task<ProdutoFotoCaminhoDto> GetImagemByProduto(int produtoId)
        {
            return await _db.ProdutoFotos.Where(x => x.FK_Produto == produtoId).Select(x => new ProdutoFotoCaminhoDto()
            {
                Id = x.Id,
                Nome = x.NomeArquivo
            }).FirstOrDefaultAsync();
        }

        public async Task<int> GetImagemIdByProduto(int produtoId)
        {
            return await _db.ProdutoFotos.Where(x => x.FK_Produto == produtoId).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}