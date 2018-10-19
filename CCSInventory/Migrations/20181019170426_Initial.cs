using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCSInventory.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.UniqueConstraint("AK_Users_UserName", x => x.UserName);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Created", "CreatedBy", "Email", "FirstName", "LastName", "Modified", "ModifiedBy", "Note", "PasswordHash", "Role", "UserName" },
                values: new object[] { 1L, new DateTime(2018, 10, 18, 12, 30, 18, 51, DateTimeKind.Utc), "Seeded Data", null, "Weber", "CS", new DateTime(2018, 10, 18, 12, 30, 18, 51, DateTimeKind.Utc), "Seeded Data", "Default user for an empty database", "$2a$10$/n.xV7jA5piJOZmfbT270eAKstycJ9WHqfpSttqz25ARWwnyLCyhu", 3, "skram" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
