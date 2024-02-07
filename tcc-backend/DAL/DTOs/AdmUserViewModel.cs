using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.DTOs
{
    public class AdmUserViewModel
    {
        public string Descricao { get; set; }
        public bool? Ativo { get; set; }
    }

    public class RedefinicaoSenhaViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Token { get; set; }
        public string Email { get; set; }
    }

    public class EsqueciSenhaViewModel
    {
        public string Email { get; set; }
        public string UrlRedefinicao { get; set; }
    }

}
