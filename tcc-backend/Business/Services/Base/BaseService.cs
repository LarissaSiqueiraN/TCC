using Business.Intefaces;
using Business.Notificacoes;
using DAL.Models.Base;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data.SqlClient;

namespace Business.Services.Base
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;
        protected readonly IConfiguration _configuration;

        protected BaseService(INotificador notificador, IConfiguration configuration)
        {
            _notificador = notificador;
            _configuration = configuration;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected void Notificar(Exception ex, string mensagem, ILogger _logger)
        {
            _logger.LogError(ex, mensagem);
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected Tuple<List<DateTime>, bool> ValidaDisponibilidadeData(DateTime dataVerificarDisponibilidade, List<DateTime> datasJautilizadas)
        {
            var diaDisponivel = dataVerificarDisponibilidade.Day;
            var mesDisponivel = dataVerificarDisponibilidade.Month;

            foreach (var dataUtilizada in datasJautilizadas)
            {
                if (dataUtilizada.Day.CompareTo(diaDisponivel) == 0 && dataUtilizada.Month.CompareTo(mesDisponivel) == 0)
                {
                    return new Tuple<List<DateTime>, bool>(datasJautilizadas, false);
                }

                datasJautilizadas.Add(dataVerificarDisponibilidade);
                return new Tuple<List<DateTime>, bool>(datasJautilizadas, true);
            }

            datasJautilizadas.Add(dataVerificarDisponibilidade);
            return new Tuple<List<DateTime>, bool>(datasJautilizadas, true);
        }

        protected DateTime NormalizaData(DateTime data)
        {
            var dataNova = data.ToString("yyyy-MM-dd");
            return DateTime.Parse(dataNova);
        }

    }
}
