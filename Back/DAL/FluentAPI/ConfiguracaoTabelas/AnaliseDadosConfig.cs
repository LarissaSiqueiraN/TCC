using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.FluentAPI.ConfiguracaoTabelas
{
    public class AnaliseDadosConfig : ConfiguracaoContextoBase
    {
        public override void Configurar(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnaliseDados>(a =>
            {
                a.HasOne(a => a.Analise).WithMany(a => a.Dados).HasForeignKey(a => a.Fk_Analise).OnDelete(DeleteBehavior.Cascade);
                a.Property(a => a.ValorX).HasColumnType("decimal(18, 10)");
                a.Property(a => a.ValorY).HasColumnType("decimal(18, 10)");
            });
        }
    }
}
