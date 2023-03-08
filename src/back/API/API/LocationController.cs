using API.Models;
using API.Models.Entity;
using API.Services.Geocoding;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.API;

[ApiController]
[Route("geocoding/[controller]")]
[EnableCors]
public class LocationController : ControllerBase
{
    private readonly GeocodingService _geocodingService;

    public LocationController(GeocodingService geocodingService)
    {
        _geocodingService = geocodingService;
    }

    [HttpGet]
    public LongLat GetLongLat(string location)
    {
        LongLat response = _geocodingService.Geocode(location);

        return  response;
    }
}