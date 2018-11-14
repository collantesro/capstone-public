using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCSInventory.Migrations
{
    public partial class DevV9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    AddressID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(nullable: false),
                    CategoryNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                    table.UniqueConstraint("AK_Categories_CategoryName", x => x.CategoryName);
                });

            migrationBuilder.CreateTable(
                name: "PantryPackType",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    PantryPackTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PantryPackTypeName = table.Column<string>(nullable: false),
                    PantryPackTypeNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PantryPackType", x => x.PantryPackTypeID);
                    table.UniqueConstraint("AK_PantryPackType_PantryPackTypeName", x => x.PantryPackTypeName);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TemplateID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TransactionTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    AgencyID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AgencyName = table.Column<string>(nullable: false),
                    AddressID = table.Column<int>(nullable: true),
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
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    SubcategoryID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryID = table.Column<int>(nullable: false),
                    SubcategoryName = table.Column<string>(nullable: false),
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
                name: "PantryPackTransactions",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    PantryPackTransactionID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Qty = table.Column<int>(nullable: false),
                    PantryPackTypeID = table.Column<int>(nullable: false),
                    PantryPackTransactionNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PantryPackTransactions", x => x.PantryPackTransactionID);
                    table.ForeignKey(
                        name: "FK_PantryPackTransactions_PantryPackType_PantryPackTypeID",
                        column: x => x.PantryPackTypeID,
                        principalTable: "PantryPackType",
                        principalColumn: "PantryPackTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TransactionID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AgencyID = table.Column<int>(nullable: false),
                    TransactionTypeID = table.Column<int>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    IsOutgoing = table.Column<bool>(nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionTypes_TransactionTypeID",
                        column: x => x.TransactionTypeID,
                        principalTable: "TransactionTypes",
                        principalColumn: "TransactionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ContainerID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BinNumber = table.Column<int>(nullable: false),
                    SubcategoryID = table.Column<int>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false),
                    Cases = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    IsUSDA = table.Column<bool>(nullable: false),
                    IsArchived = table.Column<bool>(nullable: false),
                    ContainerNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.ContainerID);
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
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TransactionLineItemID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionID = table.Column<int>(nullable: false),
                    SubcategoryID = table.Column<int>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false),
                    Cases = table.Column<int>(nullable: true),
                    USDANumber = table.Column<string>(nullable: true),
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
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressID", "AddressNote", "City", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "State", "StreetAddress1", "StreetAddress2", "Zip" },
                values: new object[] { 1, "Catholic Community Services Ogden", "Ogden", "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "UT", "2504 F Ave", null, "84401" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressID", "AddressNote", "City", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "State", "StreetAddress1", "StreetAddress2", "Zip" },
                values: new object[] { 2, "Walmart", "Ogden", "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "UT", "123 Memory Lane", null, "84401" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressID", "AddressNote", "City", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "State", "StreetAddress1", "StreetAddress2", "Zip" },
                values: new object[] { 3, "Smiths", "Ogden", "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "UT", "543 n 250 e ", null, "84401" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressID", "AddressNote", "City", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "State", "StreetAddress1", "StreetAddress2", "Zip" },
                values: new object[] { 4, "Mark", "Ogden", "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "UT", "635 s 400 w", null, "84401" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressID", "AddressNote", "City", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "State", "StreetAddress1", "StreetAddress2", "Zip" },
                values: new object[] { 5, "Kelly Vasquez", "Ogden", "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "UT", "4567 washington blvd", null, "84401" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName", "CategoryNote", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate" },
                values: new object[] { 4, "USDA", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName", "CategoryNote", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate" },
                values: new object[] { 3, "Non-Food", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName", "CategoryNote", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate" },
                values: new object[] { 5, "Grocery Rescue", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName", "CategoryNote", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate" },
                values: new object[] { 1, "Dry Goods", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName", "CategoryNote", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate" },
                values: new object[] { 2, "Perishable", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "PantryPackType",
                columns: new[] { "PantryPackTypeID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "PantryPackTypeName", "PantryPackTypeNote" },
                values: new object[] { 1, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Generic", null });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "TransactionTypeID", "CreatedBy", "CreatedDate", "IsOutgoing", "ModifiedBy", "ModifiedDate", "TransactionTypeName", "TransactionTypeNote" },
                values: new object[] { 8, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), true, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Organization Transfer", "Outgoing items to other organizations." });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "TransactionTypeID", "CreatedBy", "CreatedDate", "IsOutgoing", "ModifiedBy", "ModifiedDate", "TransactionTypeName", "TransactionTypeNote" },
                values: new object[] { 1, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "In-Kind", "Donations with where the items have not been taxed." });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "TransactionTypeID", "CreatedBy", "CreatedDate", "IsOutgoing", "ModifiedBy", "ModifiedDate", "TransactionTypeName", "TransactionTypeNote" },
                values: new object[] { 2, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Taxed", "Donations with where the items have been taxed." });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "TransactionTypeID", "CreatedBy", "CreatedDate", "IsOutgoing", "ModifiedBy", "ModifiedDate", "TransactionTypeName", "TransactionTypeNote" },
                values: new object[] { 3, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "USDA", "Donations by the Utah Food Bank." });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "TransactionTypeID", "CreatedBy", "CreatedDate", "IsOutgoing", "ModifiedBy", "ModifiedDate", "TransactionTypeName", "TransactionTypeNote" },
                values: new object[] { 4, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), false, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Grocery Rescue", "Donations from Grocery Rescue." });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "TransactionTypeID", "CreatedBy", "CreatedDate", "IsOutgoing", "ModifiedBy", "ModifiedDate", "TransactionTypeName", "TransactionTypeNote" },
                values: new object[] { 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), true, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Spoiled", "Outgoing food due to spoilage." });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "TransactionTypeID", "CreatedBy", "CreatedDate", "IsOutgoing", "ModifiedBy", "ModifiedDate", "TransactionTypeName", "TransactionTypeNote" },
                values: new object[] { 6, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), true, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "On-The-Line", "Outgoing items to the pantry." });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "TransactionTypeID", "CreatedBy", "CreatedDate", "IsOutgoing", "ModifiedBy", "ModifiedDate", "TransactionTypeName", "TransactionTypeNote" },
                values: new object[] { 7, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), true, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Pantry Pack", "Outgoing items for pantry packs." });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "CreatedBy", "CreatedDate", "Email", "FirstName", "LastName", "ModifiedBy", "ModifiedDate", "PasswordHash", "UserNote", "UserRole", "Username" },
                values: new object[] { 1, "Seeded Data", new DateTime(2018, 10, 18, 12, 30, 18, 51, DateTimeKind.Utc), null, "Weber", "CS", "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "$2a$10$/n.xV7jA5piJOZmfbT270eAKstycJ9WHqfpSttqz25ARWwnyLCyhu", "Default user for an empty database", 3, "skram" });

            migrationBuilder.InsertData(
                table: "Agencies",
                columns: new[] { "AgencyID", "AddressID", "AgencyName", "AgencyNote", "CreatedBy", "CreatedDate", "IsArchived", "MailingAddressID", "ModifiedBy", "ModifiedDate", "PhoneNumber" },
                values: new object[] { 1, 1, "Catholic Community Services Ogden", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), false, 1, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "+18013945944" });

            migrationBuilder.InsertData(
                table: "Agencies",
                columns: new[] { "AgencyID", "AddressID", "AgencyName", "AgencyNote", "CreatedBy", "CreatedDate", "IsArchived", "MailingAddressID", "ModifiedBy", "ModifiedDate", "PhoneNumber" },
                values: new object[] { 2, 2, "Walmart", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), false, 2, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "+18013551243" });

            migrationBuilder.InsertData(
                table: "Agencies",
                columns: new[] { "AgencyID", "AddressID", "AgencyName", "AgencyNote", "CreatedBy", "CreatedDate", "IsArchived", "MailingAddressID", "ModifiedBy", "ModifiedDate", "PhoneNumber" },
                values: new object[] { 3, 3, "Smiths", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), false, 3, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "+18017231598" });

            migrationBuilder.InsertData(
                table: "Agencies",
                columns: new[] { "AgencyID", "AddressID", "AgencyName", "AgencyNote", "CreatedBy", "CreatedDate", "IsArchived", "MailingAddressID", "ModifiedBy", "ModifiedDate", "PhoneNumber" },
                values: new object[] { 4, 4, "Mark", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), false, 4, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "+18013665201" });

            migrationBuilder.InsertData(
                table: "Agencies",
                columns: new[] { "AgencyID", "AddressID", "AgencyName", "AgencyNote", "CreatedBy", "CreatedDate", "IsArchived", "MailingAddressID", "ModifiedBy", "ModifiedDate", "PhoneNumber" },
                values: new object[] { 5, 5, "Kelly Vasquez", null, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), false, 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "+180139918034" });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 14, 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Frozen", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 13, 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Meat", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 12, 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Deli", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 11, 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Produce", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 10, 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Dairy", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 9, 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Bakery", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 8, 4, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Fruit", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 6, 4, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Milk", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 15, 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Dry Grocery", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 5, 4, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Beans", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 4, 4, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Unsorted (USDA)", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 3, 3, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Unsorted (Non-Food)", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 2, 2, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Unsorted (Perishable)", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 1, 1, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Unsorted (Dry Goods)", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 7, 4, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Rice", null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "SubcategoryID", "CategoryID", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "SubcategoryName", "SubcategoryNote" },
                values: new object[] { 16, 5, "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Seeded Data", new DateTime(2018, 11, 14, 3, 40, 0, 0, DateTimeKind.Utc), "Non-Food", null });

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
                name: "IX_Containers_SubcategoryID",
                table: "Containers",
                column: "SubcategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PantryPackTransactions_PantryPackTypeID",
                table: "PantryPackTransactions",
                column: "PantryPackTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_PantryPackType_PantryPackTypeName",
                table: "PantryPackType",
                column: "PantryPackTypeName");

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
                name: "IX_Transactions_AgencyID",
                table: "Transactions",
                column: "AgencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionTypeID",
                table: "Transactions",
                column: "TransactionTypeID");

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
                name: "PantryPackTransactions");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "TransactionLineItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PantryPackType");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Agencies");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
