namespace API.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }

        public string MailHost { get; set; }
        public string MailPort { get; set; }
        public string MailUser { get; set; }
        public string MailPassword { get; set; }
        public string MailUserSendGrid { get; set; }
    }
}