using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCSInventory.Migrations
{
    public partial class TitanC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agencies_Addresses_MailingAddressID",
                table: "Agencies");

            migrationBuilder.DropIndex(
                name: "IX_Agencies_MailingAddressID",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "MailingAddressID",
                table: "Agencies");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "AgencyID",
                keyValue: 1,
                column: "ModifiedDate",
                value: new DateTime(2018, 12, 12, 3, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "AgencyID",
                keyValue: 2,
                columns: new[] { "AddressID", "AgencyNote", "HasAddress", "ModifiedDate" },
                values: new object[] { null, "This is a catch-all Agency for anonymous donations.", false, new DateTime(2018, 12, 12, 3, 0, 0, 0, DateTimeKind.Utc) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MailingAddressID",
                table: "Agencies",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "AgencyID",
                keyValue: 1,
                columns: new[] { "MailingAddressID", "ModifiedDate" },
                values: new object[] { 1, new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "AgencyID",
                keyValue: 2,
                columns: new[] { "AddressID", "AgencyNote", "HasAddress", "ModifiedDate" },
                values: new object[] { 1, "This is a catch-all Agency for anonymous donations.  The address is CCS Ogden", true, new DateTime(2018, 11, 28, 16, 38, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_MailingAddressID",
                table: "Agencies",
                column: "MailingAddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Agencies_Addresses_MailingAddressID",
                table: "Agencies",
                column: "MailingAddressID",
                principalTable: "Addresses",
                principalColumn: "AddressID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
