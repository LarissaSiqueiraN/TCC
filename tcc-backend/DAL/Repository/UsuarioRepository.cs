using DAL.Models;
using DAL.Repository.Base;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(BancoAPIContext db) : base(db)
        {
        }

        public async Task<Usuario> ObterUsuarioPorId(string id)
        {
            IEnumerable<Usuario> usuarios = await Buscar(x => x.Id == id);
            return usuarios.FirstOrDefault();
        }
    }
}
