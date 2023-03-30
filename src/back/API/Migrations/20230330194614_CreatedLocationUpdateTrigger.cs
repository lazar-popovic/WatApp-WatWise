using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class CreatedLocationUpdateTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
            CREATE TRIGGER location_updated
             AFTER UPDATE ON Locations
                FOR EACH ROW
                WHEN OLD.Latitude <> NEW.Latitude OR OLD.Longitude <> NEW.Longitude OR OLD.Address <> NEW.Address
                BEGIN
                 SELECT location_updated(NEW.Id, NEW.Latitude, NEW.Longitude,NEW.Address,NEW.AddressNumber, NEW.City);
                 END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
