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
