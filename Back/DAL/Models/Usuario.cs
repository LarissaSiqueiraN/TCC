using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Usuario : IdentityUser
    {

        [MaxLength(255)]
        [Required]
        public override string Email { get; set; }

        [MaxLength(50)]
        [Required]
        public string Nome {  get; set; }

        public string? TokenRecuperacaoSenha { get; set; }

        public DateTime? DataExpiracaoToken { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

        public DateTime UltimoLogin { get; set; }

    }
}
