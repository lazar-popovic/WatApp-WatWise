using API.Models.Entity;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class MapHub : Hub
    {
        public async Task UpdateLocation(User location)
        {
            // Send the location update to all clients
            await Clients.All.SendAsync("LocationUpdated", location);
        }
    }
}
