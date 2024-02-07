using DAL.DTOs;
using DAL.Models;
using DAL.Models.EncomendaModels;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
    public interface IEncomendaService
    {
        Task<ResultadoPaginado<EncomendaReadDto>> GetByFiltro(EncomendaFiltroDto dto);
        Task<Encomenda> Adicionar(EncomendaCreateDto formulario, string usuarioId);
        Task AtualizarStatus(int id);
    }
}
