using System.Collections.Generic;

namespace DAL.DTOs
{
     public class EncomendaGridDto
     {
          public string Produto { get; set; }

          public string Quantidade { get; set; }

          public string Codigo { get; set; }

     }

    public class EncomendaCreateDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public int Idade { get; set; }
        public bool Faz5DiasQueVacinou { get; set; }
        public bool TemAlergias { get; set; }
        public string Alergias { get; set; }
        public bool FurouAntes { get; set; }
        public int? JaFurouAntes { get; set; }
        public string LocalFuro { get; set; }
        public bool NoDomicilio { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Pagamento { get; set; }
        public double ValorTotal { get; set; }
        public string Email { get; set; }
        public bool AdicionarServicoFuro { get; set; }
        public bool Ativo { get; set; } = false;
        public List<EncomendaProdutoCreateDto> Produtos { get; set; }
    }

    public class EncomendaReadDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public bool TemAlergias { get; set; }
        public string Alergias { get; set; }
        public bool NoDomicilio { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Pagamento { get; set; }
        public double ValorTotal { get; set; }
        public string DataCriacao { get; set; }
        public bool Status { get; set; }
        public bool AdicionarServicoFuro { get; set; }
        public List<EncomendaProdutoReadDto> Produtos { get; set; }
    }

    public class EncomendaProdutoReadDto
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
    }

    public class EncomendaProdutoCreateDto 
    {

        public int Fk_Produto { get; set; }
    }

    public class EncomendaProdutosDto
    {
        public List<int> Fk_Produto { get; set; }
    }
}
