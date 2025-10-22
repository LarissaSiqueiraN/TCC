using DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Analise : Entity
    {

        [MaxLength(50)]
        [Required]
        public string Nome { get; set; }

        [MaxLength(255)]
        [Required]
        public string Descricao { get; set; }

        [MaxLength(50)]
        [Required]
        public string RotuloX { get; set; }

        [MaxLength(50)]
        [Required]
        public string RotuloY { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }
        

        [ForeignKey("Usuario")]
        public string Fk_Usuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual List<AnaliseLinha> Linhas { get; set; }
    }
}
