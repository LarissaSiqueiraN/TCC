using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs
{
    public class AnaliseViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(255, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string RotuloX { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string RotuloY { get; set; }

        public string? Fk_Usuario { get; set; }

        [Required]
        public List<AnaliseDadosDto> Dados { get; set; }
    }

    public class AnaliseDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string RotuloX { get; set; }
        public string RotuloY { get; set; }
        public List<AnaliseDadosDto> Dados { get; set; }
    }

    public class AnaliseDadosDto
    {
        public decimal ValorY { get; set; }
        public decimal ValorX { get; set; }
    }
}