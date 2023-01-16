using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.EmailService
{
    internal interface IEmailService
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
