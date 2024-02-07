using DAL.Models.ProdutoModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.FluentAPI.ConfiguracaoTabelas
{
    public class ProdutoConfig : ConfiguracaoContextoBase
    {
        public override void Configurar(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(p =>
            {
            });
        }
    }
}