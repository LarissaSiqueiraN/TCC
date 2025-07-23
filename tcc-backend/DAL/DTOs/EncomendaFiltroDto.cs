using System;
namespace DAL.DTOs
{
     public class EncomendaFiltroDto
     {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public bool? Status { get; set; }
        public int Pagina { get; set; } = 1;
        public int ItensPagina { get; set; } = 10;


    }
}
