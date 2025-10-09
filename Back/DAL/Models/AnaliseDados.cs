using DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class AnaliseDados : Entity
    {

        [Required]
        public decimal ValorY { get; set; }

        [Required] 
        public decimal ValorX { get; set; }

        [Required]
        public DateTime DataCriacao {  get; set; }

        [ForeignKey("Analise")]
        public int Fk_Analise { get; set; }
        public virtual Analise Analise { get; set; }
    }
}
