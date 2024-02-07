using Business.Intefaces;
using Business.Interfaces;
using Business.Services.Base;
using DAL.DTOs;
using DAL.Models;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AdmUserService : BaseService, IAdmUserService
    {
        private readonly AdmUserRepository _repository;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<AdmUserService> _logger;

        public AdmUserService(AdmUserRepository repository,
                              UserManager<Usuario> userManager,
                              ILogger<AdmUserService> logger,
                              INotificador notificador,
                              IConfiguration configuration) : base(notificador, configuration)
        {
            _repository = repository;
            _logger = logger;
            _userManager = userManager;
      
        }

        public IEnumerable<AdmUserGridDto> BuscarUsuarios(string descBusca, bool? ativo)
        {
            return _repository.BuscarUsuarios(descBusca, ativo);
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }

        public AdmUserDto FindByKey(string id)
        {
            return _repository.FindByKey(id);
        }

        public Task<IEnumerable<ComboIdTextoDto>> GetComboPerfil()
        {
            return _repository.GetComboPerfil();
        }

        //public async Task<bool> RedefinirSenha(RedefinicaoSenhaViewModel model)
        //{
        //    try
        //    {
        //        var user = _repository.FindByTokenRedefinicao(model.Token);
        //        if (user != null)
        //        {
        //            if (user.DataExpiracaoToken < DateTime.Now )
        //            {
        //                Notificar("Token de redefinição de senha expirado");
        //                return false;
        //            }

        //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //            var resultado = await _userManager.ResetPasswordAsync(user, token, model.NovaSenha);

        //            return resultado.Succeeded; 
        //        }

        //        Notificar("Usuário incorreto ou inexistente");
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Notificar(ex, "Ocorreu um erro no processo de redefinição de senha do usuário", _logger);
        //        return false;
        //    }
        //}
    }
}
