using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCSInventory.Migrations
{
    public partial class TitanA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    StreetAddress1 = table.Column<string>(nullable: false),
                    StreetAddress2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    Zip = table.Column<string>(nullable: false),
                    AddressNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CategoryName = table.Column<string>(nullable: false),
                    CategoryNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                    table.UniqueConstraint("AK_Categories_CategoryName", x => x.CategoryName);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    LocationName = table.Column<string>(nullable: false),
                    LocationNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationID);
                    table.UniqueConstraint("AK_Locations_LocationName", x => x.LocationName);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    TemplateID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TemplateName = table.Column<string>(nullable: false),
                    TemplateData = table.Column<string>(nullable: false),
                    TemplateType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.TemplateID);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    TransactionTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TransactionTypeName = table.Column<string>(nullable: false),
                    IsOutgoing = table.Column<bool>(nullable: false),
                    TransactionTypeNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.TransactionTypeID);
                    table.UniqueConstraint("AK_TransactionTypes_TransactionTypeName", x => x.TransactionTypeName);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    UserNote = table.Column<string>(nullable: true),
                    UserRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.UniqueConstraint("AK_Users_Username", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    AgencyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    AgencyName = table.Column<string>(nullable: false),
                    AddressID = table.Column<int>(nullable: true),
                    HasAddress = table.Column<bool>(nullable: false),
                    MailingAddressID = table.Column<int>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    AgencyNote = table.Column<string>(nullable: true),
                    IsArchived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.AgencyID);
                    table.UniqueConstraint("AK_Agencies_AgencyName", x => x.AgencyName);
                    table.ForeignKey(
                        name: "FK_Agencies_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agencies_Addresses_MailingAddressID",
                        column: x => x.MailingAddressID,
                        principalTable: "Addresses",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    SubcategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    SubcategoryName = table.Column<string>(nullable: false),
                    IsArchived = table.Column<bool>(nullable: false),
                    SubcategoryNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.SubcategoryID);
                    table.UniqueConstraint("AK_Subcategories_SubcategoryName", x => x.SubcategoryName);
                    table.ForeignKey(
                        name: "FK_Subcategories_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_Log_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    AgencyID = table.Column<int>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    IsOutgoing = table.Column<bool>(nullable: false),
                    TransactionNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_Agencies_AgencyID",
                        column: x => x.AgencyID,
                        principalTable: "Agencies",
                        principalColumn: "AgencyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    ContainerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    BinNumber = table.Column<int>(nullable: false),
                    SubcategoryID = table.Column<int>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false),
                    Units = table.Column<int>(nullable: false),
                    LocationID = table.Column<int>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    IsArchived = table.Column<bool>(nullable: false),
                    ContainerNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.ContainerID);
                    table.ForeignKey(
                        name: "FK_Containers_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Containers_Subcategories_SubcategoryID",
                        column: x => x.SubcategoryID,
                        principalTable: "Subcategories",
                        principalColumn: "SubcategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionLineItems",
                columns: table => new
                {
                    TransactionLineItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TransactionID = table.Column<int>(nullable: false),
                    TransactionTypeID = table.Column<int>(nullable: false),
                    SubcategoryID = table.Column<int>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false),
                    Units = table.Column<int>(nullable: false),
                    IsPantryPack = table.Column<bool>(nullable: false),
                    TransactionLineItemNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLineItems", x => x.TransactionLineItemID);
                    table.ForeignKey(
                        name: "FK_TransactionLineItems_Subcategories_SubcategoryID",
                        column: x => x.SubcategoryID,
                        principalTable: "Subcategories",
                        principalColumn: "SubcategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionLineItems_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionLineItems_TransactionTypes_TransactionTypeID",
                        column: x => x.TransactionTypeID,
                        principalTable: "TransactionTypes",
                        principalColumn: "TransactionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressID", "AddressNote", "City", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "State", "StreetAddress1", "StreetAddress2", "Zip" },
                values: new object[] { 1, "Catholic Community Services Ogden", "Ogden", "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "UT", "2504 F Ave", null, "84401" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName", "CategoryNote", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate" },
                values: new object[,]
                {
                    { 1, "Dry Goods", null, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc) },
                    { 2, "Perishable", null, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc) },
                    { 3, "Non-Food", null, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc) },
                    { 4, "USDA", null, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc) },
                    { 5, "Grocery Rescue", null, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc) },
                    { 6, "Pantry Pack", null, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationID", "CreatedBy", "CreatedDate", "LocationName", "LocationNote", "ModifiedBy", "ModifiedDate" },
                values: new object[,]
                {
                    { 10, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "Refrigerator 2", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) },
                    { 9, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "Refrigerator 1", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) },
                    { 8, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "New Warehouse", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) },
                    { 7, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "South East", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) },
                    { 6, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "Offsite Storage 1", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) },
                    { 4, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "(NONE)", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) },
                    { 3, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "South Room", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) },
                    { 2, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "Main Room", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) },
                    { 1, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "East Room", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) },
                    { 5, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc), "USDA Room", null, "Seeded Data", new DateTime(2018, 12, 5, 4, 27, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "TransactionTypeID", "CreatedBy", "CreatedDate", "IsOutgoing", "ModifiedBy", "ModifiedDate", "TransactionTypeName", "TransactionTypeNote" },
                values: new object[,]
                {
                    { 8, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Non-Taxable", "Donations with where the items are not taxed" },
                    { 1, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "In-Kind", "Donations with where the items have not been taxed." },
                    { 2, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Taxed", "Donations with where the items have been taxed." },
                    { 3, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "USDA", "Donations by the Utah Food Bank." },
                    { 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Grocery Rescue", "Donations from Grocery Rescue." },
                    { 5, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), true, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Spoiled", "Outgoing food due to spoilage." },
                    { 6, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), true, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "On-The-Line", "Outgoing items to the pantry." },
                    { 7, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), true, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Organization Transfer", "Outgoing items to other organizations." }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "CreatedBy", "CreatedDate", "Email", "FirstName", "LastName", "ModifiedBy", "ModifiedDate", "PasswordHash", "UserNote", "UserRole", "Username" },
                values: new object[] { 1, "Seeded Data", new DateTime(2018, 10, 18, 12, 30, 18, 51, DateTimeKind.Utc), null, "Weber", "CS", "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "$2a$10$/n.xV7jA5piJOZmfbT270eAKstycJ9WHqfpSttqz25ARWwnyLCyhu", "Default user for an empty database", 3, "skram" });

            migrationBuilder.InsertData(
                table: "Agencies",
                columns: new[] { "AgencyID", "AddressID", "AgencyName", "AgencyNote", "CreatedBy", "CreatedDate", "HasAddress", "IsArchived", "MailingAddressID", "ModifiedBy", "ModifiedDate", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, 1, "Catholic Community Services Ogden", null, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), true, false, 1, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "+18013945944" },
                    { 2, 1, "Anonymous", "This is a catch-all Agency for anonymous donations.  The address is CCS Ogden", "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), true, false, null, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "+10000000000" }
                });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "IsArchived", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[,]
                {
                    { 110, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110450", "Pasta Spaghetti" },
                    { 111, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100918", "Bakery flour mix" },
                    { 112, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100514", "Red Apples" },
                    { 113, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100236", "Cherries, rtp" },
                    { 114, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100512", "Apples Granny Smith" },
                    { 115, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110652", "Salmom canned" },
                    { 116, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100521", "Gala Apples Fresh" },
                    { 117, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110560", "Pears, Fresh" },
                    { 118, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100523", "Apples Braeburn" },
                    { 119, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110672", "Lamb Shoulder Roast" },
                    { 120, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110651", "Orange Juice" },
                    { 121, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110556", "Raisins unsweetened" },
                    { 122, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100908", "Walnut Pieces" },
                    { 123, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110390", "Catfish fillets, Unbreaded" },
                    { 124, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100227", "Canned Cherries" },
                    { 109, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110481", "Frozen Carrots" },
                    { 108, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100441", "Vegetable Oil" },
                    { 107, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110260", "Beef Fine Ground" },
                    { 106, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100929", "Oat Circles" },
                    { 91, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100380", "Beans Great North'" },
                    { 92, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100880", "Chicken, Whole, Bagged" },
                    { 93, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100123", "Turkey, Consumer Whole" },
                    { 94, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100320", "Mix Veg" },
                    { 95, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100894", "Cherry Apple Juice" },
                    { 96, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100893", "Apple Juice" },
                    { 97, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100198", "Salmon, canned" },
                    { 98, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100300", "Dried Cranberries" },
                    { 99, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100298", "Cherries, dried" },
                    { 100, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100207", "Applesauce #300" },
                    { 101, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100315", "Sweet Peas" },
                    { 102, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100466", "Oats" },
                    { 103, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100313", "Corn, whole kernel" },
                    { 104, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110470", "Frozen Sliced Apples" },
                    { 105, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100469", "Grits" },
                    { 125, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110511", "Elbow Macaroni" },
                    { 126, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100046", "Whole Eggs Frozen" },
                    { 127, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100194", "Tuna Canned" },
                    { 128, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100936", "Eggs" },
                    { 148, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110904", "Turkey Breast Smoked Sliced" },
                    { 149, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110990", "Potatoes Red Fresh" },
                    { 150, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110980", "Pork Pulled CKD" },
                    { 151, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110880", "Farina Wheat Cereal" },
                    { 152, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110380", "Pork Chops Boneless" },
                    { 153, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100376", "Split Pea Dry" },
                    { 154, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110843", "Cheese Ched Yei Shred" },
                    { 155, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "111008", "1% Milk Fresh" },
                    { 9, 5, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Bakery", null },
                    { 10, 5, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Dairy", null },
                    { 11, 5, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Produce", null },
                    { 12, 5, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Deli", null },
                    { 13, 5, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Meat", null },
                    { 14, 5, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Frozen", null },
                    { 15, 5, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Dry Grocery", null },
                    { 147, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100289", "Fig Pieces 24 1lb" },
                    { 90, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100295", "Raisins" },
                    { 146, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110154", "Chicken Consumer Split Breast" },
                    { 144, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110345", "Fish AK Plck Fillets" },
                    { 129, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110623", "Blueberries, High Bush" },
                    { 130, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110580", "Kosher Salmon, Canned" },
                    { 131, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110845", "Whole Eggs Frozen" },
                    { 132, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "101035", "Whole Grain Spaghetti" },
                    { 133, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110478", "Boned chicken canned" },
                    { 134, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100003", "Shredded Yellow Cheddar" },
                    { 135, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110741", "Whole Wheat Tortillas" },
                    { 136, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100487", "Rice, Medium" },
                    { 137, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110610", "KH Tomato Sauce" },
                    { 138, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "101024", "Macaroni and cheese" },
                    { 139, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100233", "Plums" },
                    { 140, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110561", "Apples Fresh" },
                    { 141, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100492", "Rice, Long (heavy bag)" },
                    { 142, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110777", "Whole Grain Pasta Rotini " },
                    { 143, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100384", "Dark Red Kidney Beans" },
                    { 145, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110903", "Turkey Breast Deli Sliced" },
                    { 89, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110164", "Cream of Mushroom Soup" },
                    { 88, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100331", "Potatoes, sliced" },
                    { 87, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110431", "Turkey Breast Frozen" },
                    { 33, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100293", "Raisins" },
                    { 34, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100319", "Pumpkin" },
                    { 35, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100139", "Pork" },
                    { 36, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100290", "Plums Pitted Dried" },
                    { 37, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110178", "Pistachios" },
                    { 38, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110021", "Pinto Beans" },
                    { 39, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100223", "Pears" },
                    { 40, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100395", "Peanut Butter" },
                    { 41, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100218", "Peaches # 300" },
                    { 42, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100219", "Peaches" },
                    { 43, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100314", "Peas" },
                    { 44, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100897", "Orange Juice" },
                    { 45, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100211", "Mixed Fruit" },
                    { 46, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100306", "Green Beans" },
                    { 47, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100896", "Grapefruit Juice" },
                    { 32, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100491", "Rice, Long Grain" },
                    { 48, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100589", "Fig Pieces" },
                    { 31, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100526", "Beef stew" },
                    { 29, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100426", "Spaghetti, Noodles" },
                    { 1, 1, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Unsorted (Dry Goods)", null },
                    { 2, 2, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Unsorted (Perishable)", null },
                    { 3, 3, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Unsorted (Non-Food)", null },
                    { 4, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Unsorted (USDA)", null },
                    { 18, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110361", "Applesauce" },
                    { 19, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100210", "Apricot Halves" },
                    { 20, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100363", "Beans(vegetarian)" },
                    { 21, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100435", "Whole Grain Rotini" },
                    { 22, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100321", "Vegetable Soup" },
                    { 23, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100328", "Tomatoes, diced" },
                    { 24, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100333", "Tomato Sauce" },
                    { 25, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100322", "Tomato Soup" },
                    { 26, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100316", "Sweet Potatoes" },
                    { 27, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100323", "Spinach" },
                    { 28, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100335", "Spaghetti Sauce" },
                    { 30, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100050", "Shelf Milk 1%" },
                    { 49, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100433", "Egg Noodles" },
                    { 50, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100213", "Cranberry Sauce" },
                    { 51, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100899", "Cranberry Apple Juice" },
                    { 72, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100375", "Red Beans" },
                    { 73, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100342", "Tomatoes-perishable" },
                    { 74, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100324", "Diced tomatoes" },
                    { 75, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "101017", "Fresh Potatoes-perishable" },
                    { 76, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100125", "Turkey Roasts-perishable" },
                    { 77, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110290", "Lamb Leg Roast" },
                    { 78, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100382", "Beans, pinto 2lb. bags" },
                    { 79, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100356", "Potatoes, Wedges" },
                    { 80, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "101020", "Garbanzo Beans" },
                    { 81, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100361", "Refried Beans" },
                    { 82, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100275", "Cranberry Juice Concentrate" },
                    { 83, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100256", "Strawberry Frozen Cup" },
                    { 84, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100895", "Grape Juice" },
                    { 85, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110163", "Cream of Chicken Soup" },
                    { 86, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110440", "Turkey Sliced Frozen" },
                    { 71, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100242", "Blueberries, Frozen" },
                    { 70, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100367", "Beans, Blackeye" },
                    { 69, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100297", "Fruit and Nut Mix" },
                    { 68, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100105", "Chicken, Leg Qrts 40 lb" },
                    { 52, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100311", "Corn Kernel" },
                    { 53, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100308", "Carrots" },
                    { 54, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100372", "Red Kidney Beans" },
                    { 55, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100287", "Dates" },
                    { 56, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100182", "Ham, water added" },
                    { 57, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100145", "Pork Patties" },
                    { 58, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110301", "Catfish Fillets" },
                    { 16, 5, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Non-Food", null },
                    { 59, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110092", "Chix Leg Quarters" },
                    { 61, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110094", "chicken legs" },
                    { 62, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "110332", "USDA Turkey Whole Bagged" },
                    { 63, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100898", "Tomato Juice" },
                    { 64, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100337", "Dehydrated Potatoes" },
                    { 65, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100277", "Orange Juice singles" },
                    { 66, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100263", "Blueberry F Cult" },
                    { 67, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100310", "Corn Cream 24" },
                    { 60, 4, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "100241", "peach cups" },
                    { 17, 6, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc), "Pantry Pack", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_AddressID",
                table: "Agencies",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_MailingAddressID",
                table: "Agencies",
                column: "MailingAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_BinNumber",
                table: "Containers",
                column: "BinNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_LocationID",
                table: "Containers",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_SubcategoryID",
                table: "Containers",
                column: "SubcategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationName",
                table: "Locations",
                column: "LocationName");

            migrationBuilder.CreateIndex(
                name: "IX_Log_UserID",
                table: "Log",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryID",
                table: "Subcategories",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLineItems_SubcategoryID",
                table: "TransactionLineItems",
                column: "SubcategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLineItems_TransactionID",
                table: "TransactionLineItems",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLineItems_TransactionTypeID",
                table: "TransactionLineItems",
                column: "TransactionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AgencyID",
                table: "Transactions",
                column: "AgencyID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_TransactionTypeName",
                table: "TransactionTypes",
                column: "TransactionTypeName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "TransactionLineItems");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Agencies");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
