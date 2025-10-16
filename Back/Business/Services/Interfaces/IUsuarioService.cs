using DAL.Models;

namespace Business.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuarioById(string userId);
    }
}
