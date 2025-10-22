using DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class AnaliseLinha : Entity
    {
        [Required]
        public DateTime DataCriacao { get; set; }

        [MaxLength(50)]
        [Required]
        public string Nome { get; set; }

        [MaxLength(50)]
        [Required]
        public string Cor { get; set; }

        [ForeignKey("Analise")]
        public int Fk_Analise { get; set; }
        public virtual Analise Analise { get; set; }

        public virtual List<AnaliseLinhaDados> Dados { get; set; }
    }
}
