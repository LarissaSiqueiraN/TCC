using DAL.Models.EncomendaModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.FluentAPI.ConfiguracaoTabelas
{
    public class EncomendaProdutoConfig : ConfiguracaoContextoBase
    {
        public override void Configurar(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EncomendaProduto>(p =>
            {
                p.HasOne(x => x.Encomenda).WithMany(x => x.Produtos).HasForeignKey(x => x.Fk_Encomenda).OnDelete(DeleteBehavior.Restrict);
                p.HasOne(x => x.Produto).WithMany(x => x.Encomendas).HasForeignKey(x => x.Fk_Produto).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}