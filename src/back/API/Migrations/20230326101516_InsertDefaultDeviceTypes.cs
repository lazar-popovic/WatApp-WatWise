using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InsertDefaultDeviceTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id","Type", "Category" },
                values: new object[] { 1, "Fridge", -1 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id","Type", "Category" },
                values: new object[] { 2, "Water heater", -1 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id","Type", "Category" },
                values: new object[] { 3, "Solar panel", 1 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id","Type", "Category" },
                values: new object[] { 4, "Desktop computer", -1 }
            );
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id","Type", "Category" },
                values: new object[] { 5, "Freezer", -1 }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
