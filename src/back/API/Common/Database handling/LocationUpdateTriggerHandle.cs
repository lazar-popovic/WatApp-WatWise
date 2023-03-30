using API.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Common.Database_handling
{
    public class LocationUpdateTriggerHandle
    {
        private readonly DataContext _context;

        public LocationUpdateTriggerHandle(DataContext context)
        {
            _context = context;
        }

        [DbFunction("location_updated")]
        public static void HandleLocationUpdated(string payload)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlite("your_connection_string");

            using var dbContext = new DataContext(optionsBuilder.Options);

            // Parse the payload to get the updated location information
            var updatedLocation = JsonConvert.DeserializeObject<Location>(payload);

            // Update the corresponding user's location in the database
            var user = dbContext.Users.FirstOrDefault(u => u.Id == updatedLocation.Id);
            if (user != null)
            {
                user.Location.Latitude = updatedLocation.Latitude;
                user.Location.Longitude = updatedLocation.Longitude;
                dbContext.SaveChanges();
            }
        }
    }
}
