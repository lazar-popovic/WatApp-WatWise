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
                values: new object[]{ "admin@mail.com",
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
                values: new object[]{ "employee@mail.com",
                    "$2y$12$XrdwSWq4tm3GtXWJHCT5YuEBDdqzbuwLaqCsh1hJDuW9hGWBAX0t2",
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
                values: new object[]{ "prosumer1@mail.com",
                    "$2y$12$q8u3DoahdCx/OKSRqTgv1.PuzEePaFII85/J2GfA.4PNgT1lROava",
                    "Prosumer",
                    "Prosumer",
                    true,
                    3
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
                values: new object[]{ "prosumer2@mail.com",
                    "$2y$12$vM4aJuUSE4e1JUEv6lIyF.Eaw324j6Bhq1rrDMY8aD76wlBKrwsKK",
                    "Prosumer",
                    "Prosumer",
                    true,
                    3
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
                values: new object[]{ "prosumer3@mail.com",
                    "$2y$12$gwCAI.w3j3elO2.TE6Aw2ubnh1HbWOCLO7p.MTpf3ujCLm09i4vKK",
                    "Prosumer",
                    "Prosumer",
                    true,
                    3
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
                values: new object[]{ "prosumer4@mail.com",
                    "$2y$12$5hthqn.ni/n54kN90iqkg.mpeBF4M.FlDPrZUHiO9ncFG.fMTC8zW",
                    "Prosumer",
                    "Prosumer",
                    true,
                    3
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
                values: new object[]{ "prosumer5@mail.com",
                    "$2y$12$fCH6OVUiMhnwyx2o3Ucedee2QB3.uuONYZjONKyjPtfzukhv8UqOK",
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
