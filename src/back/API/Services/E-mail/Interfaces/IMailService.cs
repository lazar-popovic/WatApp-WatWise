using API.Models.Entity;
using API.Models.ViewModels;

namespace API.Services.E_mail.Interfaces
{
    public interface IMailService
    {
        void sendToken(User user);
        void sendResetToken(User user, string token);
        void resendToken(User user);
    }
}
