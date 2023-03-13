using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitDataInsert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
            
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", 
                    "PasswordHash", 
                    "Firstname", 
                    "Lastname", 
                    "Verified", 
                    "RoleId"
                },
                values: new object[]{ "admin@admin.com",
                    "$2a$12$sERFKctazogEEEK9T99TZu74BeGoNlBZPVndwrqYSGTrMipjwi8UO",
                    "Admin",
                    "Admin",
                    true,
                    1
                }
            );
            
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", 
                    "PasswordHash", 
                    "Firstname", 
                    "Lastname", 
                    "Verified", 
                    "RoleId"
                },
                values: new object[]{ "employee@employee.com",
                    "$2a$12$ZC.Jnso7luIOFnx3MfE/bum3Guhbrz/uYnyQqINibstdouG/i22IK",
                    "Employee",
                    "Employee",
                    true,
                    2
                }
            );
            
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", 
                    "PasswordHash", 
                    "Firstname", 
                    "Lastname", 
                    "Verified", 
                    "RoleId"
                },
                values: new object[]{ "prosumer@prosumer.com",
                    "$2a$12$mnsqrjWB5MQGWR8UpPaS3eFpDjXqbVc6nWBOfIOsUhPy4WwSAwrKa",
                    "Prosumer",
                    "Prosumer",
                    true,
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
