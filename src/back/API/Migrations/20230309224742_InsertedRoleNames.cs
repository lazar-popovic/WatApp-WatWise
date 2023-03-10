using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InsertedRoleNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ADDED ROLES
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id","RoleName" },
                values: new object[] { 1, "Admin" }
            );
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id","RoleName" },
                values: new object[] { 2, "Employee" }
            );
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id","RoleName" },
                values: new object[] { 3, "User" }
            );
            
            // DODAVANJE ADMINA
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
