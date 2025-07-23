using Business.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Business.Services
{
    public class EmailService : IEmailService
    {
        public bool ValidaEmail(string email)
        {
            if (new EmailAddressAttribute().IsValid(email))
            {
                return true;
            }
            return false;
        }
    }
}
