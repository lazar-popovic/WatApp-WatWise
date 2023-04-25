using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class CreatedSybetypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceSubtypeId",
                table: "Devices",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeviceSubtypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubtypeName = table.Column<string>(type: "TEXT", nullable: true),
                    DeviceTypeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSubtypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceSubtypes_DeviceTypes_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceSubtypeId",
                table: "Devices",
                column: "DeviceSubtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceSubtypes_DeviceTypeId",
                table: "DeviceSubtypes",
                column: "DeviceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceSubtypes_DeviceSubtypeId",
                table: "Devices",
                column: "DeviceSubtypeId",
                principalTable: "DeviceSubtypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceSubtypes_DeviceSubtypeId",
                table: "Devices");

            migrationBuilder.DropTable(
                name: "DeviceSubtypes");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceSubtypeId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "DeviceSubtypeId",
                table: "Devices");
        }
    }
}
