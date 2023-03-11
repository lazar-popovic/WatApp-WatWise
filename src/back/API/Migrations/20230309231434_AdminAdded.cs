using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AdminAdded : Migration
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
                values: new object[]{ "admin@admin.com",
                                      "$2a$12$sERFKctazogEEEK9T99TZu74BeGoNlBZPVndwrqYSGTrMipjwi8UO",
                                      "Admin",
                                      "Admin",
                                      true,
                                      true,
                                      1
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
