using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs
{
    public class AdicionarFotoViewModel
    {
        [Required]
        public int ProdutoId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
