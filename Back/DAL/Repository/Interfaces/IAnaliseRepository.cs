using DAL.Models;
using DAL.Repository.Base.Interfaces;

namespace DAL.Repository.Interfaces
{
    public interface IAnaliseRepository : IRepository<Analise>
    {
        Task<Analise> Cadastrar(Analise analise);
    }
}
