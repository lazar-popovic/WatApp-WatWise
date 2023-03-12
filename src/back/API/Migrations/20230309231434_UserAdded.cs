using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", 
                                 "PasswordHash", 
                                 "Firstname", 
                                 "Lastname", 
                                 "Authenticated", 
                                 "Verified", 
                                 "RoleId"
                },
                values: new object[]{ "john@doe.com",
                                      "$2y$10$vnDGNxapxkhaS77BuYdoiup6TscQlgEKrsLHC5jiaMeAtPWUsuv0q",
                                      "John",
                                      "Doe",
                                      false,
                                      false,
                                      3
                }

            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
