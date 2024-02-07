using DAL.FluentAPI.ConfiguracaoTabelas;
using DAL.Models;
using DAL.Models.EncomendaModels;
using DAL.Models.ProdutoModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DAL
{
    public class BancoAPIContext : IdentityDbContext<Usuario>
    {
        public BancoAPIContext(DbContextOptions<BancoAPIContext> options) : base(options) { }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoFoto> ProdutoFotos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Encomenda> Encomendas { get; set; }
        public DbSet<EncomendaProduto> EncomendaProdutos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BancoAPIContext).Assembly);


            ConfigurarEntidadesAtravesDasClassesConfig(modelBuilder);

        }

        private void ConfigurarEntidadesAtravesDasClassesConfig(ModelBuilder modelBuilder)
        {
            var configTypes = RecuperarTodosOsTiposQueHerdamDaClasse(typeof(ConfiguracaoContextoBase));

            foreach (var configType in configTypes)
            {
                var obj = (ConfiguracaoContextoBase)Activator.CreateInstance(configType);
                obj.Configurar(modelBuilder);
            }
        }

        IEnumerable<Type> RecuperarTodosOsTiposQueHerdamDaClasse(Type MyType)
        {
            return Assembly.GetAssembly(MyType)
            .GetTypes()
            .Where(TheType =>
            TheType.IsClass
            && !TheType.IsAbstract
            && TheType.IsSubclassOf(MyType)
            );
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
