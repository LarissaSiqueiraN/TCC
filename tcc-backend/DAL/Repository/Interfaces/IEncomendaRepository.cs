using DAL.DTOs;
using DAL.Models;
using DAL.Models.EncomendaModels;
using DAL.Repository.Base;
using System.Threading.Tasks;

namespace DAL.Repository.Interfaces
{
    public interface IEncomendaRepository : IRepository<Encomenda>
    {
        Task<ResultadoPaginado<EncomendaReadDto>> GetByFiltro(EncomendaFiltroDto dto);
        Task<Encomenda> GetById(int id);
    }
}
