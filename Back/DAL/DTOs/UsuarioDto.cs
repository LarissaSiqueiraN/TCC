using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs
{
    public class UsuarioDto
    {
    }

    public class UsuarioLoginDto
    {
        public string Nome { get; set; }

        public string Login { get; set; }
    }

    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(255, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Login { get; set; }

        public string Senha { get; set; }
    }

    public class LoginResponseViewModel
    {
        public string AccessToken { get; set; }

        public string Id { get; set; }

        public string Nome { get; set; }

        public string Login { get; set; }

        public bool Authenticated { get; set; }

        public UserTokenViewModel UserToken { get; set; }
    }

    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string login { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string senha { get; set; }
    }

    public class UserTokenViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }
}