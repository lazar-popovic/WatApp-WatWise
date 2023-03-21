using API.Models.ViewModels;
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

    [HttpPost, Authorize(Roles = "User")]
    public LongLat GetLongLat( LocationViewModel locationViewModel)
    {
       return _geocodingService.Geocode( locationViewModel);

    }
}