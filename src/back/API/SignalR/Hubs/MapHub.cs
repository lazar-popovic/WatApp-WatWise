using API.Models.Entity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.SignalR.Hubs
{
    public class MapHub : Hub
    {
        private readonly DataContext _dataContext;

        public MapHub(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task UpdateLocation(int userId, double latitude, double longitude, string adress, string city, int adressNumber)
        {
            // Find the user in the database and update their location
            var user = await _dataContext.Users.Include(u => u.Location).FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                if (user.Location != null)
                {
                    user.Location.Latitude = latitude;
                    user.Location.Longitude = longitude;
                    user.Location.City = city;
                    user.Location.AddressNumber = adressNumber;
                    user.Location.Address = adress;

                    await _dataContext.SaveChangesAsync();
                }
                
                await Clients.All.SendAsync("UpdateUserLocation", userId, latitude, longitude);
            }
        }

        public async Task GetInitialLocations()
        {
            var users = await _dataContext.Users.Include(u => u.Location).ToListAsync();

            if (users != null)
            {
                foreach (var user in users)
                {
                    if(user.Location != null)
                        await Clients.Caller.SendAsync("AddUserLocation", user.Id, user.Location.Latitude, user.Location.Longitude, user.Location.Address, user.Location.AddressNumber, user.Location.City);
                }
            }
        }
    }
}
