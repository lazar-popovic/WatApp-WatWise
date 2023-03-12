using API.Services.E_mail.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using API.Models.Entity;
using API.Services.JWTCreation.Interfaces;

namespace API.Services.E_mail.Implementations
{
    public class MailService : IMailService
    {
        private readonly IJWTCreator _jWTCreator;
        private void sendEmail(EmailModel model)
        {

            var fromAddress = new MailAddress("wattwise.noreply@gmail.com", "From Name");
            var toAddress = new MailAddress(model.ToEmail, "To Name");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("wattwise.noreply@gmail.com", "zrasxeeuibgsbkdw"),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = model.Subject,
                Body = model.Body
            })
            {
                smtp.Send(message);
            }
           }
        public void sendToken(User user)
        {
            var token = _jWTCreator.CreateToken(user);

            this.sendEmail(new EmailModel("no-reply-message", token, user.Email));

        }


    }
}
