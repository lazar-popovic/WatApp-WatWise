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

        public MailService(IJWTCreator jWtCreator)
        {
            _jWTCreator = jWtCreator;
        }
        
        private void sendEmail(EmailModel model)
        {

            var fromAddress = new MailAddress("wattwise.noreply@gmail.com", "WattWise");
            var toAddress = new MailAddress(model.ToEmail, model.ToEmail);

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
        public void sendTokenProsumer(User user)
        {
            var token = _jWTCreator.CreateVerificationToken(user.Id);
            var mail = new EmailModel("Verification token", token, user.Email);

            mail.Body =
                "\r\nHi,\r\n\r\nYou are successfully registered to the WattWise as prosumer. To activate your account and create a password, click on the activation link below:\n\n";
            mail.Body += $" http://localhost:4200/prosumer/verification?token={token}";
            
            this.sendEmail( mail);
        }
        
        public void sendTokenEmployee(User user)
        {
            var token = _jWTCreator.CreateVerificationToken(user.Id);
            var mail = new EmailModel("Verification token", token, user.Email);

            mail.Body =
                "\r\nHi,\r\n\r\nYou are successfully registered to the WattWise as employee. To activate your account and create a password, click on the activation link below:\n\n";
            mail.Body += $" http://localhost:4200/dso/verification?token={token}";
            
            this.sendEmail( mail);
        }

        public void sendResetTokenProsumer(User user,string token)
        {
            var mail = new EmailModel("Reset token", token, user.Email);
      
            mail.Body =
                "\r\nHi,\r\n\r\nthere was a request to change your password!\r\n\r\nIf you did not make this request then please ignore this email.\r\n\r\nOtherwise, please click this link to change your password:";
            mail.Body += $" http://localhost:4200/prosumer/reset-password?token={token}";

            this.sendEmail(mail);
        }
    }
}
