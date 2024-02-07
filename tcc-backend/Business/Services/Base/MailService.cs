using Business.Intefaces;
using DAL.DTOs;
using DAL.Models;
using DAL.Models.EncomendaModels;
using DAL.Models.ProdutoModels;
using DAL.Repository.ProdutoRepositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Base
{
    public class MailService : BaseService
    {
        #region Enum
        public enum TipoDestinatario
        {
            Para,
            ComCopia,
            ComCopiaOculta
        }
        #endregion

        #region Atributos ou Variaveis
        /// <summary>
        /// The message to be sent using the SmtpClient class. This class exposes many properties specific to the message
        /// like To, CC, Bcc, Subject, and Body properties that corresponds to the message fields.
        /// </summary>
        private MailMessage EmailMensagem;

        #endregion

        #region Propriedades
        /// <summary>
        /// Network: (default)
        ///     The message is sent via the network to the SMTP server.
        /// PickupDirectoryFromIis:
        ///     The message is copied to the mail default directory of the Internet Information Services (IIS).
        /// SpecifiedPickupDirectory:
        ///     The message is copied to the directory specified by the property PickupDirectoryLocation.
        /// </summary>
        private string MailDeliveryMethod { get; set; }
        /// <summary>
        /// Value set if MailDeliveryMethod = SpecifiedPickupDirectory
        /// </summary>
        private string MailPickupDirectoryLocation { get; set; }
        /// <summary>
        /// Servidor SMTP para envio de email
        /// </summary>
        public string MailHost { get; set; }
        /// <summary>
        /// Port number: Usually 25, and sometimes 465. Depends on server’s configuration.
        /// </summary>
        public int MailPort { get; set; }
        /// <summary>
        /// User Mail accesss
        /// </summary>
        public string MailUser { get; set; }
        /// <summary>
        /// Password User mail access
        /// </summary>
        public string MailPassword { get; set; }
        /// <summary>
        /// Usuário par conexão no SendGrid
        /// </summary>
        public string UserSendGrid { get; set; }

        /// <summary>
        /// You need to know if the server requires a SSL (Secure Socket Layer) connection or not.
        /// To be honest, most servers require SSL connections.
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Get or Set emissor do email.
        /// </summary>
        private MailAddress De
        {
            get { return EmailMensagem.From; }
            set { EmailMensagem.From = value; }
        }
        /// <summary>
        /// Get or Set assunto do email.
        /// </summary>
        public string Assunto
        {
            get { return EmailMensagem.Subject; }
            set { EmailMensagem.Subject = value; }
        }
        /// <summary>
        /// Get or Set corpo no formato somente texto do email.
        /// </summary>
        public string CorpoTexto { get; set; }
        /// <summary>
        /// Get or Set corpo no formato HTML do email.
        /// </summary>
        public string CorpoHTML { get; set; }
        /// <summary>
        /// Array de strings com os parametros do email (Usuario,msg1,msg2,urlCallback)
        /// </summary>
        public Dictionary<string, string> ModeloHtmlParametro { get; set; }
        /// <summary>
        /// Diretorio do modelo HTML de email padrao - Caminho Fisico da Aplicacao+PastaModelos do webconfig+nomeModelo.html
        /// </summary>
        public string ModeloHtmlPath { get; set; }


        private INotificador _notificador { get; set; }
        private IConfiguration _configuration { get; set; }
        private ILogger<MailService> _logger { get; set; }
        private readonly UserManager<Usuario> _userManager;
        private readonly IProdutoRepository _produtoRepository;
        #endregion

        #region Construtor
        /// <summary>
        /// Construtor para envio de E-mail.
        /// </summary>
        /// <param name="parametroWebConfg">Se informar "false", os parametros de configuração deverão estar preenchidos:
        ///                                 MailHost / MailPort / MailUser / MailPassword
        ///                                 Caso seja "true" utilizara do webConfig </param>
        public MailService(IConfiguration configuration,
                            UserManager<Usuario> userManager,
                            ILogger<MailService> logger,
                            INotificador notificador,
                            IProdutoRepository produtoRepository) : base(notificador, configuration)
        {
            EmailMensagem = new MailMessage();
            MailDeliveryMethod = "Network";
            _notificador = notificador;
            _configuration = configuration;
            _userManager = userManager;
            _produtoRepository = produtoRepository;
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Enviar o email.
        /// </summary>
        /// <returns>Envio com sucesso ou falha do email.</returns>
        public bool Enviar()
        {
            SmtpClient smtp = new SmtpClient();

            smtp.EnableSsl = EnableSsl;

            //Seleciona método de enivo
            switch (this.MailDeliveryMethod)
            {
                case "Network":
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    break;
                case "PickupDirectoryFromIis":
                    smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                    break;
                case "SpecifiedPickupDirectory":
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    break;
                default:
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    break;
            }

            //Preenche o SmtpClient de acordo com o método de envio
            if (smtp.DeliveryMethod == SmtpDeliveryMethod.Network)
            {
                if (string.IsNullOrEmpty(this.MailHost))
                    throw new ArgumentException("Não foi informado e-mail Host");

                smtp.Host = this.MailHost;
                smtp.Port = this.MailPort;
            }
            else if (smtp.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                smtp.PickupDirectoryLocation = MailPickupDirectoryLocation;
            }

            //Preenche as credencias de acesso ao servidor para envio do email
            //if (!string.IsNullOrEmpty(this.MailUser))
            //    smtp.Credentials = new NetworkCredential(this.MailUser, this.MailPassword);
            //else
            //    smtp.UseDefaultCredentials = false;


            //Preenche as credencias de acesso ao servidor para envio do email
            if (!string.IsNullOrEmpty(this.MailUser))
            {
                string user = this.MailUser;

                if (MailHost.Contains("smtp.sendgrid.net"))
                {
                    user = this.UserSendGrid;
                }

                smtp.Credentials = new NetworkCredential(user, this.MailPassword);
            }
            else
                smtp.UseDefaultCredentials = false;


            //Verifica se existe destinatário no email
            if (this.EmailMensagem.To.Count == 0 && this.EmailMensagem.CC.Count == 0 && this.EmailMensagem.Bcc.Count == 0)
            {
                throw new ArgumentException("Não foi informado e-mail de destinatário");
            }

            //Verifica se exeiste From
            if (this.EmailMensagem.From == null)
            {
                AdicionarRemetente("", MailUser);
                //throw new ArgumentException("Não foi informado e-mail do remetente!");
            }

            //Verifica se existe Assunto
            if (string.IsNullOrEmpty(this.EmailMensagem.Subject))
            {
                throw new ArgumentException("Não foi informado assunto!");
            }

            if (!string.IsNullOrEmpty(this.ModeloHtmlPath))
            {
                if (!File.Exists(this.ModeloHtmlPath))
                    throw new ArgumentException("O arquivo modelo HTML informado não existe.");

                //this.CorpoHTML = GetBodyFromModelo(this.ModeloHtmlParametro, this.ModeloHtmlPath);
            }

            ////Verifica se Existe Corpo do email
            //if (string.IsNullOrEmpty(this.CorpoHTML) && string.IsNullOrEmpty(this.CorpoTexto))
            //{
            //    throw new ArgumentException("Não foi informado corpo da mensagem!");
            //}

            //Se cormpo HTML preenchido seta "IsBodyHtml = true"
            if (!string.IsNullOrEmpty(this.CorpoHTML))
            {
                AlternateView view = AlternateView.CreateAlternateViewFromString(this.CorpoHTML, new ContentType("text/html; charset=UTF-8"));
                EmailMensagem.AlternateViews.Add(view);
                EmailMensagem.Body = this.CorpoHTML;
                EmailMensagem.IsBodyHtml = true;
            }
            else
            {
                EmailMensagem.IsBodyHtml = false;
            }


            if (!string.IsNullOrEmpty(this.CorpoTexto))
            {
                AlternateView view = AlternateView.CreateAlternateViewFromString(this.CorpoTexto, new ContentType("text/plain; charset=UTF-8"));
                EmailMensagem.AlternateViews.Add(view);
                if (!EmailMensagem.IsBodyHtml)
                    EmailMensagem.Body = this.CorpoTexto;
            }

            //Tenta enviar o eamil
            try
            {
                smtp.Send(this.EmailMensagem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Falha ao envirar email: {0}", MailHost));
                return false;
            }
            return true;
        }      

        public async Task<bool> EnviarEmailCadastroEncomenda(Encomenda encomenda)
        {
            try
            {
                if (encomenda != null)
                {
                    MailHost = _configuration["AppSettings:_MailHost"];
                    MailPort = int.Parse(_configuration["AppSettings:_MailPort"]);
                    MailUser = _configuration["AppSettings:_MailUser"];
                    MailPassword = _configuration["AppSettings:_MailPassword"];
                    UserSendGrid = _configuration["AppSettings:_MailSendGrid"];
                    var logoCegonhaDosBrincos = _configuration["AppSettings:LogoCegonhaDosBrincos"];

                    EnableSsl = true;
                    Assunto = "Confirmação da compra.";

                    var sb = new StringBuilder();

                    byte[] imageBytes = File.ReadAllBytes(logoCegonhaDosBrincos);
                    string base64Image = Convert.ToBase64String(imageBytes);

                    sb.AppendLine($"<p>Olá, {encomenda.Nome}</p>");
                    sb.AppendLine($"<p>Segue a sua confirmação de compra na Cegonha dos Brincos.</p>");
                    sb.AppendLine($"<p>Código da compra: {encomenda.Id}</p>");

                    sb.AppendLine("<p>Produtos: </p>");

                    foreach(EncomendaProduto produto in encomenda.Produtos)
                    {
                        Produto pd = await _produtoRepository.GetObject(x => x.Id == produto.Fk_Produto);

                        sb.AppendLine($"<p>Nome: {pd.Nome} | Valor: {pd.Valor}</p>");
                    }

                    sb.AppendLine($"<p>Forma de pagamento: {encomenda.Pagamento}</p>");
                    sb.AppendLine($"<p>Agradecemos por escolher a nossa empresa.</p>");
                    sb.AppendLine($"<p>Entraremos em contato com o número: {encomenda.Telefone}");
                    sb.AppendLine($"<p>Atenciosamente,</p><br><br>");
                    sb.AppendLine($"<p style='font-size: 22px; color: '#FCBAAD'>Cegonha dos Brincos</p>");
                    sb.AppendLine($"<img alt='Logo-Cegonha-Dos-Brincos' style='height: 100px; width: 160px;' src=\"data:image/jpeg;base64,{base64Image}\">");
                    sb.AppendLine($"<p style=\"font-family: Helvetica, Arial, sans-serif; font-size: 8px; color: #808080;\">Rua Aldo Russo, n. 605 | Centro | Colégios Univap | São José dos Campos | CEP: xx.xxx-xxx\r\n\r\nTelefone: (xx) xxxx-xxxx / (xx) xxxxx-xxxx | E-Mail: cegonhaDosBrincos@gmail.com</p>");

                    CorpoHTML = sb.ToString();

                    AdicionarDestinatario(MailService.TipoDestinatario.Para, encomenda.Email);

                    bool envioSucesso = Enviar();

                    if (envioSucesso)
                        return true;

                    Notificar("Ocorreu um erro ao enviar email com sucesso.");
                    return false;
                }

                Notificar("Ocorreu um erro ao tentar encontrar email.");
                return false;
            }
            catch (Exception ex)
            {
                Notificar(ex, "Falha em MailService: EnviarEmailCadastroEncomenda", _logger);
                return false;
            }

        }

        public void AdicionarRemetente(string nome, string email)
        {
            this.De = new MailAddress(email, nome, Encoding.GetEncoding("utf-8"));
        }

        /// <summary>
        /// Adiciona destinatários extras para o email.
        /// </summary>
        /// <param name="tipoDestinatario">Tipo do destinatário.</param>
        /// <param name="email">Endereço do destinatário do email.</param>
        public void AdicionarDestinatario(TipoDestinatario tipoDestinatario, string email)
        {
            AdicionarDestinatario(tipoDestinatario, string.Empty, email);
        }

        /// <summary>
        /// Adiciona destinatários extras para o email.
        /// </summary>
        /// <param name="tipoDestinatario">Tipo do destinatário.</param>
        /// <param name="nome">Nome do destinatário.</param>
        /// <param name="email">Endereço do destinatário do email.</param>
        public void AdicionarDestinatario(TipoDestinatario tipoDestinatario, string nome, string email)
        {
            MailAddress endereco;

            //    endereco = new MailAddress(email, nome, Encoding.GetEncoding("utf-8")); //iso-8859-3 (Latin 3 (ISO)) //us-ascii
            //    endereco = new MailAddress(email, nome, Encoding.GetEncoding("ISO-8859-1")); //iso-8859-3 (Latin 3 (ISO)) //us-ascii
            //    endereco = new MailAddress(email, nome, Encoding.GetEncoding("ISO-8859-3")); //iso-8859-3 (Latin 3 (ISO)) //us-ascii
            //    endereco = new MailAddress(email, nome, Encoding.GetEncoding("us-ascii")); //iso-8859-3 (Latin 3 (ISO)) //us-ascii

            endereco = new MailAddress(email, nome, Encoding.GetEncoding("utf-8"));

            switch (tipoDestinatario)
            {
                case TipoDestinatario.Para:
                    EmailMensagem.To.Add(endereco);
                    break;
                case TipoDestinatario.ComCopia:
                    EmailMensagem.CC.Add(endereco);
                    break;
                case TipoDestinatario.ComCopiaOculta:
                    EmailMensagem.Bcc.Add(endereco);
                    break;
            }
            //}
        }


        /// <summary>
        /// Adiciona destinatários extras para o email.
        /// </summary>
        /// <param name="tipoDestinatario">Tipo do destinatário.</param>
        /// <param name="nome">Nome do destinatário.</param>
        /// <param name="email">Endereço do destinatário do email.</param>
        public void AdicionarDestinatarios(TipoDestinatario tipoDestinatario, List<string> listaEmail)
        {
            MailAddress endereco;

            foreach (var item in listaEmail)
            {
                endereco = new MailAddress(item, item, Encoding.GetEncoding("utf-8"));

                switch (tipoDestinatario)
                {
                    case TipoDestinatario.Para:
                        EmailMensagem.To.Add(endereco);
                        break;
                    case TipoDestinatario.ComCopia:
                        EmailMensagem.CC.Add(endereco);
                        break;
                    case TipoDestinatario.ComCopiaOculta:
                        EmailMensagem.Bcc.Add(endereco);
                        break;
                }
            }

            //}
        }


        /// <summary>
        /// Adiciona anexo.
        /// </summary>
        /// <param name="path">Caminho do anexo.</param>
        public void AdicionarAnexo(string path)
        {
            EmailMensagem.Attachments.Add(new Attachment(path));
        }

        public void AdicionarAnexo(List<MemoryStream> pListaAnexos, string pNomeArquivo)
        {
            ContentType content = new ContentType()
            {
                MediaType = MediaTypeNames.Text.Xml,
                Name = string.Format("{0}.xml", pNomeArquivo)
            };

            foreach (var item in pListaAnexos)
            {
                Stream anexo = item;
                EmailMensagem.Attachments.Add(new Attachment(anexo, content));
            }
        }

        #endregion

        


    }
}
