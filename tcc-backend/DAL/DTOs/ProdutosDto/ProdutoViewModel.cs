using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.ProdutoDto
{
    public class ProdutoCadastroViewModel
    {
        [Required]
        [MaxLength(55)]
        public string Nome { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        [MaxLength(255)]
        public string Descricao { get; set; }

        [Required]
        public int Quantidade { get; set; }

        public bool Vendido { get; set; }

        public DateTime? DataVenda { get; set; }
    }
}
