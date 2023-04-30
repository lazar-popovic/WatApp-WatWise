using API.Models.Entity;
using API.Models;

namespace API.BL.Interfaces
{
    public interface ILocationBL
    {
        Task<Response<List<Location>>> GetAllLocation();
        Task<Response<List<String>>> GetAllLocationDistinctCity();
        Task<Response<List<String>>> GetAllNeighborhood(string city);
        Task<Response<List<Location>>> GetAllLocationWithNeighborhood(string neighborhood);
    }
}
