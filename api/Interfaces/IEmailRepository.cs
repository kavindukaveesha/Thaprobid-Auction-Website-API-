using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.EmailDto;

namespace api.Interfaces
{
    public interface IEmailRepository
    {

        Task SendEmailAsync(string toEmail, string subject, string body);

    }
}