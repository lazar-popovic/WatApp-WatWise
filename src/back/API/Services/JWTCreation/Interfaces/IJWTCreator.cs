using API.Models.Entity;

namespace API.Services.JWTCreation.Interfaces
{
    public interface IJWTCreator
    {
        string CreateToken(User request);
    }
}
