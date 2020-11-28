using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface IEmail
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
