using AutoMapper;
using Business.Intefaces;
using Business.Services.Base;
using Business.Services.Interfaces;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(INotificador notificador, 
                              IConfiguration configuration,  
                              IMapper mapper, 
                              ILogger<UsuarioService> logger,
                              IUsuarioRepository usuarioRepository) 
                              : base(notificador, configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> GetUsuarioById(string userId)
        {
            return await _usuarioRepository.GetObjectTracking(x => x.Id == userId);
        }

    }
}

