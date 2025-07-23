using DAL.Models.Base;
using DAL.Models.ProdutoModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.EncomendaModels
{
    public class EncomendaProduto : Entity
    {
        [ForeignKey("Encomenda")]
        public int Fk_Encomenda {  get; set; }
        public virtual Encomenda Encomenda { get; set; }
        [ForeignKey("Produto")]
        public int Fk_Produto {  get; set; }
        public Produto Produto { get; set; }
    }
}
