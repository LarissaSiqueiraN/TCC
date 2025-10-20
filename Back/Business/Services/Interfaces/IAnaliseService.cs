using DAL.DTOs;

namespace Business.Services.Interfaces
{
    public interface IAnaliseService
    {
        Task<AnaliseDto> Cadastrar(AnaliseViewModel model);

        Task<IEnumerable<AnaliseDto>> GetAnalisesByUsuario(string usuarioId);

        Task<bool> Excluir(int analiseId);
    }
}
