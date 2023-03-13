using API.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.E_mail.Interfaces
{
    public interface IMailService
    {
        void sendTokenEmployee(User user);
        void sendTokenProsumer(User user);
        void sendResetTokenProsumer(User user, string token);
       
    }
}
