using API.Models.Entity;
using API.Models;

namespace API.BL.Interfaces
{
    public interface ILocationBL
    {
        Task<Response<List<Location>>> GetAllLocation();
    }
}
