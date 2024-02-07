using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTOs.ProdutosDto
{
    public class ProdutoFotoRotaDto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
    }

    public class ProdutoFotoCaminhoDto 
    {
        public int Id { get; set; }
        public string CaminhoFoto { get; set; }
        public string Nome { get; set; }
    }
}
