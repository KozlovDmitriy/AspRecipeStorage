using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace AspRecipeStorage.Controllers
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // convert IdentityMessage to a MailMessage
            var email =
               new MailMessage(new MailAddress("mrdiko4@gmail.com", "Козлов Дмитрий"),
               new MailAddress(message.Destination))
               {
                   Subject = message.Subject,
                   Body = message.Body,
                   IsBodyHtml = true
               };
            using (var client = new SmtpClient())
            {
                return client.SendMailAsync(email);
            }
        }
    }
}