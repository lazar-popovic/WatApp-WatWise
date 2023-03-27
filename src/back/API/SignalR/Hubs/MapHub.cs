using API.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.SignalR.Hubs
{
    [Authorize(Roles = "Admin", "Employee")]
    public class MapHub : Hub
    {
        private readonly DataContext _dataContext;
        private static Dictionary<int, User> _userLocations = new Dictionary<int, User>();
        public MapHub(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task UpdateLocation(int userId, double latitude, double longitude, string adress, string city, int adressNumber)
        {
            // Find the user in the database and update their location
            try
            {
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

                    await Clients.All.SendAsync("UpdateUserLocation", userId, latitude, longitude, adress, city, adressNumber);
                }
                else
                {
                    throw new ArgumentException($"User with ID {userId} not found!");
                }
            }
            catch(Exception ex)
            {
                await Clients.Caller.SendAsync("UpdateLocationError", ex.Message);
            }
           
        }

        public async Task GetInitialLocations()
        {
            try
            {
                var users = await _dataContext.Users.Include(u => u.Location).ToListAsync();

                if (users != null)
                {
                    foreach (var user in users)
                    {
                        if (user.Location != null)
                            await Clients.Caller.SendAsync("AddUserLocation", user.Id, user.Location.Latitude, user.Location.Longitude, user.Location.Address, user.Location.AddressNumber, user.Location.City);
                    }
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("GetInitialLocationsError", ex.Message);
            }

        }

        public override async Task OnConnectedAsync()
        {
            // Send the client the locations of all users
            await GetInitialLocations();

            await base.OnConnectedAsync();
        }
    }
}
