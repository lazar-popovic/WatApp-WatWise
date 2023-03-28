using API.DAL.Interfaces;
using API.SignalR.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ZstdSharp.Unsafe;

namespace API.API
{
    [Route("api/mapHub")]
    [ApiController]
    public class SignalRMapUpdateController : ControllerBase
    {
        private readonly IHubContext<MapHub> _hub;
        private readonly ILocationDAL _locationDAL;

        public SignalRMapUpdateController(IHubContext<MapHub> hub, ILocationDAL locationDAL)
        {
            _hub = hub;
            _locationDAL = locationDAL;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _hub.Clients.All.SendAsync("getLocations", _locationDAL.GetAllLocationsAsync());
            return Ok(new { Message = "Request Completed" });
        }
    }
}
