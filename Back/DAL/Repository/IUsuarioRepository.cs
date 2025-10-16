using DAL.Models;
using DAL.Repository.Base;

namespace DAL.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario> GetUsuarioById(string id);
    }
}
