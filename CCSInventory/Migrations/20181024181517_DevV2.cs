using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCSInventory.Migrations
{
    public partial class DevV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StreetAddress1 = table.Column<string>(nullable: false),
                    StreetAddress2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    Zip = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                    table.UniqueConstraint("AK_Categories_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "PantryPackType",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PantryPackType", x => x.ID);
                    table.UniqueConstraint("AK_PantryPackType_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    AddressID = table.Column<int>(nullable: false),
                    MailingAddressID = table.Column<int>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IsArchived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.ID);
                    table.UniqueConstraint("AK_Agencies_Name", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Agencies_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agencies_Addresses_MailingAddressID",
                        column: x => x.MailingAddressID,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.ID);
                    table.UniqueConstraint("AK_Subcategories_Name", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Subcategories_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PantryPackTransactions",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Qty = table.Column<int>(nullable: false),
                    PackTypeID = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PantryPackTransactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PantryPackTransactions_PantryPackType_PackTypeID",
                        column: x => x.PackTypeID,
                        principalTable: "PantryPackType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AgencyID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsOutgoing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transactions_Agencies_AgencyID",
                        column: x => x.AgencyID,
                        principalTable: "Agencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BinNumber = table.Column<int>(nullable: false),
                    SubcategoryID = table.Column<int>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Containers_Subcategories_SubcategoryID",
                        column: x => x.SubcategoryID,
                        principalTable: "Subcategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Created", "CreatedBy", "Modified", "ModifiedBy", "Name", "Note" },
                values: new object[] { 1, new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", "Dry Goods", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Created", "CreatedBy", "Modified", "ModifiedBy", "Name", "Note" },
                values: new object[] { 2, new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", "Perishable", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Created", "CreatedBy", "Modified", "ModifiedBy", "Name", "Note" },
                values: new object[] { 3, new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", "Non-Food", null });

            migrationBuilder.InsertData(
                table: "PantryPackType",
                columns: new[] { "ID", "Created", "CreatedBy", "Modified", "ModifiedBy", "Name", "Note" },
                values: new object[] { 1, new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", "Generic", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Created", "CreatedBy", "Email", "FirstName", "LastName", "Modified", "ModifiedBy", "Note", "PasswordHash", "Role", "UserName" },
                values: new object[] { 1, new DateTime(2018, 10, 18, 12, 30, 18, 51, DateTimeKind.Utc), "Seeded Data", null, "Weber", "CS", new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", "Default user for an empty database", "$2a$10$/n.xV7jA5piJOZmfbT270eAKstycJ9WHqfpSttqz25ARWwnyLCyhu", 3, "skram" });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "ID", "CategoryID", "Created", "CreatedBy", "Modified", "ModifiedBy", "Name", "Note" },
                values: new object[] { 1, 1, new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", "Unsorted (Dry Goods)", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "ID", "CategoryID", "Created", "CreatedBy", "Modified", "ModifiedBy", "Name", "Note" },
                values: new object[] { 2, 2, new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", "Unsorted (Perishable)", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "ID", "CategoryID", "Created", "CreatedBy", "Modified", "ModifiedBy", "Name", "Note" },
                values: new object[] { 3, 3, new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 10, 24, 18, 3, 0, 0, DateTimeKind.Utc), "Seeded Data", "Unsorted (Non-Food)", null });

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_AddressID",
                table: "Agencies",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_MailingAddressID",
                table: "Agencies",
                column: "MailingAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_SubcategoryID",
                table: "Containers",
                column: "SubcategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PantryPackTransactions_PackTypeID",
                table: "PantryPackTransactions",
                column: "PackTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryID",
                table: "Subcategories",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AgencyID",
                table: "Transactions",
                column: "AgencyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "PantryPackTransactions");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropTable(
                name: "PantryPackType");

            migrationBuilder.DropTable(
                name: "Agencies");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
