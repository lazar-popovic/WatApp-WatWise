using API.BL.Implementations;
using API.BL.Interfaces;
using API.Models.ViewModels;
using API.Services.Geocoding;
using API.Services.Geocoding.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.API;

[ApiController]
[Route("api/location")]
[EnableCors]
public class LocationController : ControllerBase
{
    private readonly IGeocodingService _geocodingService;
    private readonly ILocationBL locationBL;

    public LocationController(IGeocodingService geocodingService, ILocationBL location)
    {
        _geocodingService = geocodingService;
        locationBL = location;
    }

    [HttpPost, Authorize(Roles = "User")]
    public LongLat GetLongLat( LocationViewModel locationViewModel)
    {
       return _geocodingService.Geocode( locationViewModel);
    }
    [HttpGet("all-locations")]
    public async Task<IActionResult> GetAllLocation()
    {
        return Ok(await locationBL.GetAllLocation());
    }
    [HttpGet("address-autocomplete")]
    public async Task<IActionResult> GetPossibleAddresses( string streetAddress)
    {
        return Ok(await _geocodingService.Autocomplete( streetAddress));
    }
}