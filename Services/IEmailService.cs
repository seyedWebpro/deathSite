using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string apiKey, string to, string subject, string body);
    }
}