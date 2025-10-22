using DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class AnaliseLinhaDados : Entity
    {

        [Required]
        public decimal ValorY { get; set; }

        [Required] 
        public decimal ValorX { get; set; }

        [Required]
        public DateTime DataCriacao {  get; set; }

        [ForeignKey("AnaliseLinha")]
        public int Fk_AnaliseLinha { get; set; }
        public virtual AnaliseLinha AnaliseLinha { get; set; }
    }
}
