using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.FluentAPI.ConfiguracaoTabelas
{
    public class AnaliseLinhaDadosConfig : ConfiguracaoContextoBase
    {
        public override void Configurar(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnaliseLinhaDados>(a =>
            {
                a.HasOne(a => a.AnaliseLinha).WithMany(a => a.Dados).HasForeignKey(a => a.Fk_AnaliseLinha).OnDelete(DeleteBehavior.Cascade);
                a.Property(a => a.ValorX).HasColumnType("decimal(18, 10)");
                a.Property(a => a.ValorY).HasColumnType("decimal(18, 10)");
            });
        }
    }
}
