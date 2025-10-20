using DAL.Models;
using DAL.Repository.Base;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Repository
{
    public class AnaliseRepository : Repository<Analise>, IAnaliseRepository
    {
        private BancoAPIContext _db { get; set; }
        private IConfiguration _configuration { get; set; }
        public AnaliseRepository(BancoAPIContext db, IConfiguration configuration) : base(db)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<Analise> Cadastrar(Analise analise)
        {
            try
            {
                var resp = await _db.Analises.AddAsync(analise);
                return resp.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}