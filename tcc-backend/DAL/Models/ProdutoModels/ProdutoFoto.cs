using DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.ProdutoModels
{
    public class ProdutoFoto : Entity
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório"), MaxLength(250)]
        public string NomeArquivo { get; set; }
        public string DisplayeName { get; set; }
        public string Extensao { get; set; }

        [ForeignKey("Produto")]
        public int FK_Produto { get; set; }
        public virtual Produto Produto { get; set; }
    }
}
