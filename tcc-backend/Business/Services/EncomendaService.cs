using AutoMapper;
using Business.Intefaces;
using Business.Services.Base;
using Business.Services.Interfaces;
using DAL.DTOs;
using DAL.Models;
using DAL.Models.EncomendaModels;
using DAL.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class EncomendaService : BaseService, IEncomendaService
    {
        private IEncomendaRepository _encomendaRepository;
        private IMapper _mapper;
        private ILogger<EncomendaService> _logger;

        public EncomendaService(INotificador notificador, 
                                    IConfiguration configuration, 
                                    IEncomendaRepository encomendaRepository, 
                                    IMapper mapper, 
                                    ILogger<EncomendaService> logger) : base(notificador, configuration)
        {
            _encomendaRepository = encomendaRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResultadoPaginado<EncomendaReadDto>> GetByFiltro(EncomendaFiltroDto dto)
        {
            return await _encomendaRepository.GetByFiltro(dto);
        }

        public async Task<Encomenda> Adicionar(EncomendaCreateDto dto, string usuarioId)
        {
            try
            {
                List<EncomendaProduto> produtos = _mapper.Map<List<EncomendaProduto>>(dto.Produtos); 
                Encomenda encomenda = _mapper.Map<Encomenda>(dto);
                
                encomenda.Produtos = produtos;
                encomenda.DataCriacao = DateTime.Now;

                return await _encomendaRepository.Adicionar(encomenda);
            }
            catch (Exception ex)
            {
                Notificar(ex, "Ocorreu um erro em EncomendaService: Adicionar", _logger);
                throw ex;
            }
        }

        public async Task AtualizarStatus(int id)
        {
            try
            {
                Encomenda encomenda = await _encomendaRepository.GetById(id);

                if(encomenda != null)
                {
                    encomenda.Ativo = !encomenda.Ativo;  
                }

                await _encomendaRepository.update(encomenda);

            }
            catch (Exception ex)
            {
                Notificar(ex, "Ocorreu um erro em EncomendaService: AtualizarStatus", _logger);
                throw ex;
            }
        }

    }
}
