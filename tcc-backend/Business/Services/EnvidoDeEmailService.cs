using Business.Intefaces;
using Business.Services.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class EnvidoDeEmailService : BaseService
    {
        public SmtpClient Client { get; set; }
        public ILogger<EnvidoDeEmailService> _logger { get; set; }

        public EnvidoDeEmailService(ILogger<EnvidoDeEmailService> logger,
                                    IConfiguration configuration,
                                    INotificador notificador) : base(notificador, configuration)
        {
            Client = new SmtpClient();
        }

        public async Task<bool> Enviar(MailMessage email)
        {
            Client.Host = _configuration["AppSettings:MailHost"];
            Client.Credentials = new NetworkCredential(_configuration["AppSettings:MailUser"], _configuration["AppSettings:MailPassword"]);
            Client.EnableSsl = true;
            Client.Port = int.Parse(_configuration["AppSettings:MailPort"]);

            email.From = new MailAddress(_configuration["AppSettings:MailSender"], "Sender");
            email.IsBodyHtml = true;
            email.Priority = MailPriority.High;

            try
            {
                await Client.SendMailAsync(email);
                return true;
            }
            catch (Exception ex)
            {
                Notificar(ex, "Erro no envio de email", _logger);
                return false;
            }
        }
    }
}
