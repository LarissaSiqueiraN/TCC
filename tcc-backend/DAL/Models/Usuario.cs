using DAL.Models.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Usuario: IdentityUser
    {
        public string Nome { get; set; }

        public string Cpf { get; set; }

        public string? Rg { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string Email { get; set; }

        public bool Ativo { get; set; }

        public string? ModificadoPor { get; set; }

        public DateTime? UltimaModificacao { get; set; }

        [ForeignKey("Carrinho")]
        public int Fk_Carrinho { get; set; }

    }
}
