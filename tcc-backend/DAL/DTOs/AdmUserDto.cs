using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTOs
{
    public class AdmUserDto
    {
        public string Id { get; set; }

        public string Nome { get; set; }

        public string IdPerfil { get; set; }

        public string Perfil { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }


        public bool Ativo { get; set; }

    }
}
