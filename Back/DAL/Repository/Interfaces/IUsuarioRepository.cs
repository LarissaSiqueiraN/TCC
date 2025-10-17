using DAL.Models;
using DAL.Repository.Base.Interfaces;

namespace DAL.Repository.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario> GetUsuarioById(string id);
    }
}
