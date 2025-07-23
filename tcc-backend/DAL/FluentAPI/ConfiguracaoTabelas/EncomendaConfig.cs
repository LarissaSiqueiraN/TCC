using DAL.Models.EncomendaModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.FluentAPI.ConfiguracaoTabelas
{
    public class EncomendaConfig : ConfiguracaoContextoBase
    {
        public override void Configurar(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Encomenda>(p =>
            {
                p.Property(p => p.Ativo).IsRequired();
            });
        }
    }
}
