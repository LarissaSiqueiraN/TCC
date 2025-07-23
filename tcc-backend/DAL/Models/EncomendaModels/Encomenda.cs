using DAL.Models.Base;
using DAL.Models.ProdutoModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models.EncomendaModels
{
    public class Encomenda : Entity<int>
    {
        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(25)]
        public string Telefone { get; set; }

        [Required]
        public int Idade { get; set; }
        public bool Faz5DiasQueVacinou { get; set; }
        public bool TemAlergias { get; set; }
        [MaxLength(250)]
        public string Alergias { get; set; }
        public bool FurouAntes { get; set; }
        public int? JaFurouAntes { get; set; }
        [Required]
        [MaxLength(80)]
        public string LocalFuro { get; set; }
        public bool NoDomicilio { get; set; }
        [Required]
        public string Cep { get; set; }
        [Required]
        [MaxLength(250)]
        public string Bairro { get; set; }
        [Required]
        [MaxLength(250)]
        public string Rua { get; set; }
        [Required]
        public int Numero { get; set; }
        [Required]
        [MaxLength(250)]
        public string Complemento { get; set; }
        [Required]
        [MaxLength(250)]
        public string Pagamento { get; set; }

        [Required]
        public double ValorTotal { get; set; }

        [Required]
        [MaxLength(250)]
        public string Email { get; set; }
        public List<EncomendaProduto> Produtos { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
        public bool AdicionarServicoFuro { get; set; }

        public string ModificadoPor { get; set; }
        public DateTime UltimaModificacao { get; set; }

    }
}
