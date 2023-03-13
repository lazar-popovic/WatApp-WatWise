using API.Models.Entity;

namespace API.Services.JWTCreation.Interfaces
{
    public interface IJWTCreator
    {
        string CreateToken(User request);

        //string CreateVerificationToken(string email);
        string CreateVerificationToken(int userId);
    }
}
