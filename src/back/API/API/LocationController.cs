using API.Services.Geocoding;
using API.Services.Geocoding.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.API;

[ApiController]
[Route("geocoding/[controller]")]
[EnableCors]
public class LocationController : ControllerBase
{
    private readonly IGeocodingService _geocodingService;

    public LocationController( IGeocodingService geocodingService)
    {
        _geocodingService = geocodingService;
    }

    [HttpGet, Authorize(Roles = "User")]
    public LongLat GetLongLat(string address)
    {
       return _geocodingService.Geocode(address);

    }
}