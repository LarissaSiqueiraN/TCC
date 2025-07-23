using DAL.DTOs;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class AdmUserRepository : IDisposable
    {
        private readonly BancoAPIContext _db;

        public AdmUserRepository(BancoAPIContext db)
        {
            _db = db;
        }

        public void Dispose()
        {
            _db?.Dispose();
        }

        public async Task<IEnumerable<ComboIdTextoDto>> GetComboPerfil()
        {
            return await _db.Roles.Select(s => new ComboIdTextoDto()
            {
                Id = s.Id,
                Descricao = s.Name
            }).OrderBy(o => o.Descricao).ToListAsync();
        }
        //

        public AdmUserDto FindByKey(string id)
        {
            //return await _db.Grupos.Where(predicate).Include(s => s.Setor).ToListAsync();
            var user = from urs in _db.Users
                        join urls in _db.UserRoles on urs.Id equals urls.UserId into ur
                        from urls in ur.DefaultIfEmpty()
                        join rls in _db.Roles on urls.RoleId equals rls.Id into roles
                        from rls in roles.DefaultIfEmpty()
                        where (urs.Id == id)
                        select new AdmUserDto()
                        {
                            Id = urs.Id,
                            Nome = urs.Nome,
                            Ativo = urs.LockoutEnabled,
                            IdPerfil = urls.RoleId,
                            Email = urs.Email,
                            Telefone = urs.PhoneNumber,
                            Perfil = rls.Name
                        };

            return user.FirstOrDefault();
        }

        //public Usuario FindByTokenRedefinicao(string token)
        //{
        //   return _db.Users.Where(x => x.TokenRecuperacaoSenha == token).FirstOrDefault();
        //}

        public IEnumerable<AdmUserGridDto> BuscarUsuarios(string descBusca, bool? ativo)
        {
            //return await _db.Grupos.Where(predicate).Include(s => s.Setor).ToListAsync();
            var users = from urs in _db.Users
                        join urls in _db.UserRoles on urs.Id equals urls.UserId into ur
                        from urls in ur.DefaultIfEmpty()
                        join rls in _db.Roles on urls.RoleId equals rls.Id into roles
                        from rls in roles.DefaultIfEmpty()
                        where (
                                (string.IsNullOrEmpty(descBusca) || urs.Nome.Contains(descBusca) || rls.Name.Contains(descBusca)) &&
                                ((ativo == null || urs.LockoutEnabled == ativo))
                              )
                        select new AdmUserGridDto()
                        {
                            Id = urs.Id,
                            Nome = urs.Nome,
                            Ativo = urs.LockoutEnabled,
                            IdPerfil = urls.RoleId,
                            Perfil = rls.Name
                        };

            return users.ToList();
        }

    }
}
