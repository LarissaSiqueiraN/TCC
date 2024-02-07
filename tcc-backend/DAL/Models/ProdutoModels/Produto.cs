using DAL.Models.Base;
using DAL.Models.EncomendaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models.ProdutoModels
{
    public class Produto : Entity
    {
        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        [MaxLength(255)]
        public string Descricao { get; set; }

        public bool? Vendido { get; set; }

        public DateTime? DataVenda { get; set; }

        [Required]
        public int Quantidade { get; set; }

        public List<EncomendaProduto> Encomendas { get; set; }

        public bool Ativo { get; set; }

        [MaxLength(80)]
        public string ModificadoPor { get; set; }

        public DateTime? UltimaModificacao { get; set; }
    }
}
