using System;
namespace DAL.DTOs
{
     public class UsuarioFiltroDto
     {
          public string Nome { get; set; }

          public string Cpf { get; set; }

          public string Rg { get; set; }

          public string DataNascimento { get; set; }

          public string Email { get; set; }

        public string? ModificadoPor { get; set; }

        public string UltimaModificacao { get; set; }

     }
}
