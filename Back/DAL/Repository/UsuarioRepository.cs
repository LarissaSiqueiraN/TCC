using DAL.Models;
using DAL.Repository.Base;
using DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private BancoAPIContext _db { get; set; }
        private IConfiguration _configuration { get; set; }
        private UserManager<Usuario> _userManager { get; set; }

        public UsuarioRepository(BancoAPIContext db, IConfiguration configuration, UserManager<Usuario> userManager) : base(db)
        {
            _db = db;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<Usuario> GetUsuarioById(string id)
        {
            try
            {
                return await _db.Usuarios.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
