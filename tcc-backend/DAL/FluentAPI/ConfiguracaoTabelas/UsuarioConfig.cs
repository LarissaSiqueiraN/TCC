using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.FluentAPI.ConfiguracaoTabelas
{
    public class UsuarioConfig : ConfiguracaoContextoBase
    {
        public override void Configurar(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(p =>
            {
                p.Property(p => p.Nome).HasAnnotation("MaxLength", 55);
                p.Property(p => p.Nome).IsRequired();
                p.Property(p => p.Cpf).HasAnnotation("MaxLength", 55);
                p.Property(p => p.Cpf).IsRequired();
                p.Property(p => p.Rg).HasAnnotation("MaxLength", 55);
                p.Property(p => p.DataNascimento).HasAnnotation("MaxLength", 55);
                p.Property(p => p.DataNascimento).IsRequired();
                p.Property(p => p.Email).HasAnnotation("MaxLength", 55);
                p.Property(p => p.Email).IsRequired();
                p.Property(p => p.Ativo).IsRequired();
            });
        }
    }
}
