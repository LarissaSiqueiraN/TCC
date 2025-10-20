using AutoMapper;
using Business.Intefaces;
using Business.Services.Base;
using Business.Services.Interfaces;
using DAL.DTOs;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class AnaliseService : BaseService, IAnaliseService
    {
        private readonly ILogger<AnaliseService> _logger;
        private readonly IMapper _mapper;
        private readonly IAnaliseRepository _analiseRepository;

        public AnaliseService(INotificador notificador,
                              IConfiguration configuration,
                              ILogger<AnaliseService> logger,
                              IMapper mapper,
                              IAnaliseRepository analiseRepository)
                              : base(notificador, configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _analiseRepository = analiseRepository;
        }

        public async Task<AnaliseDto> Cadastrar(AnaliseViewModel model)
        {
            try
            {
                Analise analise = _mapper.Map<Analise>(model);
                analise.DataCadastro = DateTime.Now;

                var resp = await _analiseRepository.Adicionar(analise);
                return _mapper.Map<AnaliseDto>(resp);
            }
            catch(Exception ex)
            {
                Notificar(ex, "Ocorreu um erro ao cadastrar o registro: Analise.", _logger);
                throw ex;
            }
        }

        public async Task<IEnumerable<AnaliseDto>> GetAnalisesByUsuario(string usuarioId)
        {
            try
            {
                var analises = await _analiseRepository.Buscar(a => a.Fk_Usuario == usuarioId);
                return _mapper.Map<IEnumerable<AnaliseDto>>(analises);
            }
            catch (Exception ex)
            {
                Notificar(ex, "Ocorreu um erro ao buscar as análises do usuário.", _logger);
                throw ex;
            }
        }
        public async Task<bool> Excluir(int analiseId)
        {
            try
            {
                var analise = await _analiseRepository.Buscar(a => a.Id == analiseId);

                if (analise == null)
                {
                    Notificar("Análise não encontrada.");
                    return false;
                }

                await _analiseRepository.Remover(analiseId);
                return true;
            }
            catch (Exception ex)
            {
                Notificar(ex, "Ocorreu um erro ao excluir a análise.", _logger);
                throw ex;
            }
        }
    }
}