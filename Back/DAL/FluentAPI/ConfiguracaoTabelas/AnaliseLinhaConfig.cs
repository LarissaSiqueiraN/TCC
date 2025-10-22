using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.FluentAPI.ConfiguracaoTabelas
{
    public class AnaliseLinhaConfig : ConfiguracaoContextoBase
    {
        public override void Configurar(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnaliseLinha>(a =>
            {
                a.HasOne(a => a.Analise).WithMany(a => a.Linhas).HasForeignKey(a => a.Fk_Analise).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}