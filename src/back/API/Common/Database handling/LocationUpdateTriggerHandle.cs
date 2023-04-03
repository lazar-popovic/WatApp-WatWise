using API.Models.Entity;
using API.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Common.Database_handling
{
    public class LocationUpdateTriggerHandle
    {
        //private readonly DataContext _context;
        private readonly IHubContext<MapHub> _hubContext;

        public LocationUpdateTriggerHandle(/*DataContext context,*/ IHubContext<MapHub> hubContext)
        {
            _hubContext = hubContext;
            //_context = context;
        }

        [DbFunction("location_updated")]
        public async void HandleLocationUpdated(int id, double latitude, double longitude, string address, int adressNumber, string city)
        {
            /*
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlite("your_connection_string");

            using var dbContext = new DataContext(optionsBuilder.Options);

            
            // Update the corresponding user's location in the database
            var user = dbContext.Users.FirstOrDefault(u => u.LocationId == id);
            if (user != null)
            {
                if(user.Location != null)
                {
                    user.Location.Latitude = latitude;
                    user.Location.Longitude = longitude;
                    user.Location.Address = address;
                    user.Location.AddressNumber = adressNumber;
                    user.Location.City = city;
                    dbContext.SaveChanges();
                }
                
            }*/

             await _hubContext.Clients.All.SendAsync("locationUpdated",id,latitude, longitude, address, adressNumber,city);
        }
    }
    }
}
