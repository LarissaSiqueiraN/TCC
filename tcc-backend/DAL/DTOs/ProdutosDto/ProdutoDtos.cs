using System.ComponentModel.DataAnnotations;
using System;

namespace DAL.DTOs.ProdutosDto
{
    public class ProdutoFiltroDto
    {
        public string Id { get; set; }
        public string Nome { get; set; }

        public double? Valor { get; set; }

        public string Descricao { get; set; }

        public bool? Vendido { get; set; }

        public string DataVenda { get; set; }

        public string ModificadoPor { get; set; }

        public string UltimaModificacao { get; set; }
        public int Pagina { get; set; } = 1;
        public int ItensPagina { get; set; } = 10;

    }

    public class ProdutoGridDto
    {
        public string Nome { get; set; }

        public string Valor { get; set; }

        public string Descricao { get; set; }

        public string Vendido { get; set; }

        public string DataVenda { get; set; }

    }

    public class ProdutoReadDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }
        public bool? Vendido { get; set; }
        public string DataVenda { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; }
        public string ModificadoPor { get; set; }
        public string UltimaModificacao { get; set; }
    }
}
