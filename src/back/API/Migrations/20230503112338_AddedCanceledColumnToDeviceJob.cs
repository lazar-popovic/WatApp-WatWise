using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddedCanceledColumnToDeviceJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Repeat",
                table: "DeviceJobs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Canceled",
                table: "DeviceJobs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "AddressNumber", "City", "Latitude", "Longitude", "Neighborhood" },
                values: new object[] { 1, "Skerliceva", 12, "Grad Kragujevac", 44.0138, 20.90793, "Kragujevac" }
            );
            migrationBuilder.Sql("UPDATE Users SET LocationId = 1 WHERE Id = 3;");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "AddressNumber", "City", "Latitude", "Longitude", "Neighborhood" },
                values: new object[] { 2, "Raška", 6, "Grad Kragujevac", 44.005594, 20.865328, "Stanovo" }
            );
            migrationBuilder.Sql("UPDATE Users SET LocationId = 2 WHERE Id = 4;");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "AddressNumber", "City", "Latitude", "Longitude", "Neighborhood" },
                values: new object[] { 3, "Bulevar kraljice Marije", 73, "Grad Kragujevac", 44.0048907, 20.8949847, "Palilule" }
            );
            migrationBuilder.Sql("UPDATE Users SET LocationId = 3 WHERE Id = 5;");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "AddressNumber", "City", "Latitude", "Longitude", "Neighborhood" },
                values: new object[] { 4, "Miloja Simovića", 10, "Grad Kragujevac", 44.007645, 20.877189, "Stanovo" }
            );
            migrationBuilder.Sql("UPDATE Users SET LocationId = 4 WHERE Id = 6;");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "AddressNumber", "City", "Latitude", "Longitude", "Neighborhood" },
                values: new object[] { 5, "Durmitorska", 5, "Grad Kragujevac", 44.014518, 20.898326, "Erdoglija" }
            );
            migrationBuilder.Sql("UPDATE Users SET LocationId = 5 WHERE Id = 7;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "DeviceJobs");

            migrationBuilder.AlterColumn<bool>(
                name: "Repeat",
                table: "DeviceJobs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");
        }
    }
}
