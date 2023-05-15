using API.Models.Entity;
using API.Models;
using API.Models.DTOs;

namespace API.BL.Interfaces
{
    public interface ILocationBL
    {
        Task<Response<List<LocationWithPowerUsageDTO>>> GetAllLocation();
        Task<Response<List<String>>> GetAllLocationDistinctCity();
        Task<Response<List<String>>> GetAllNeighborhood(string city);
        Task<Response<List<LocationWithPowerUsageDTO>>> GetAllLocationWithNeighborhood(string city, string neighborhood);
        Task<Response<List<NeighborhoodPowerUsageDTO>>> Top5NeighborhoodsForCityAndCategory(string city, int category);
    }
}
