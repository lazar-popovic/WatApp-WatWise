using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InsertAlteredDeviceTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 1, "Fridge", -1, 0.045 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 2, "Water heater", -1, 1 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 3, "Solar panel", 1, 0.32 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 4, "Desktop computer", -1, 0.1 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 5, "Freezer", -1, 0.05 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 6, "Kitchen stove", -1, 1.2 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 7, "TV", -1, 0.0586 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 8, "Night Lamp", -1, 0.016 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 9, "Microwave", -1, 1 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 10, "Vacuum cleaner", -1, 0.18 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Type", "Category", "WattageInkW" },
                values: new object[] { 11, "Battery", 0, 0.045 }
            );
            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "AddressNumber", "City", "Latitude", "Longitude" },
                values: new object[] { 1, "Skerliceva", 12, "Kragujevac", 44.0138, 20.90793 }
            );
            migrationBuilder.Sql("UPDATE Users SET LocationId = 1 WHERE Id = 3;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
