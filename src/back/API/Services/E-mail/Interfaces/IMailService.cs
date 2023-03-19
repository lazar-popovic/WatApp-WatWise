using API.Models.Dto;
using API.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.E_mail.Interfaces
{
    public interface IMailService
    {
        void sendToken(User user);
        void sendResetToken(User user, string token);
       
    }
}
