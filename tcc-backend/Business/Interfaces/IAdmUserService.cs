using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAdmUserService : IDisposable
    {

        Task<IEnumerable<ComboIdTextoDto>> GetComboPerfil();

        AdmUserDto FindByKey(string id);

        IEnumerable<AdmUserGridDto> BuscarUsuarios(string descBusca, bool? Ativo);
    }
}
