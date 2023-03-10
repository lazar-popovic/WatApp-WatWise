using API.Models;
using API.Models.Entity;
using API.Services.Geocoding;
using API.Services.Geocoding.Implementations;
using API.Services.Geocoding.Interfaces;
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

    [HttpGet]
    public LongLat GetLongLat(string address)
    {
        var response = _geocodingService.Geocode(address);

        return  response;
    }
}