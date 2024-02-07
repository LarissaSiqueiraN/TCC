using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
    public interface IEmailService
    {
        bool ValidaEmail(string email);
    }
}
