using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class BancoAPIContext : IdentityDbContext<Usuario>
    {
        public BancoAPIContext(DbContextOptions<BancoAPIContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Analise> Analises { get; set; }
        public DbSet<AnaliseDados> AnalisesDados { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AnaliseDados>()
                .Property(p => p.DataCriacao)
                .HasDefaultValueSql("GETDATE()");

            builder.ApplyConfigurationsFromAssembly(typeof(BancoAPIContext).Assembly);
        }
    }
}
