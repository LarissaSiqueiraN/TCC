using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs
{
    public class AdmUserGridDto
    {
        [StringLength(50)]
        public string Id { get; set; }

        [StringLength(80)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string IdPerfil { get; set; }

        [StringLength(80)]
        public string Perfil { get; set; }

      

        public bool Ativo { get; set; }


    }
}
