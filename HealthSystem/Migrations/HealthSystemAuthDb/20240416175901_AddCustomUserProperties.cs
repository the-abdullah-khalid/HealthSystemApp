using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthSystemApp.Migrations.HealthSystemAuthDb
{
    /// <inheritdoc />
    public partial class AddCustomUserProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19ec1414-c440-40f4-8bc9-2973d765aa73");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d192029-4567-418b-9dea-7e6cf332790c");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7866ea46-c88c-4e9c-b473-ca3991f36ea1");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb3266fd-b495-49cf-8281-ca28ed9d4ac1");

            migrationBuilder.AddColumn<Guid>(
                name: "HealthRegionId",
                schema: "authDB",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HealthSystemId",
                schema: "authDB",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "authDB",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "authDB",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "50c0642a-e599-4ba8-909b-1d351e25b3c3", "50c0642a-e599-4ba8-909b-1d351e25b3c3", "HealthSystemAdmin", "HEALTHSYSTEMADMIN" },
                    { "896dfe27-0cb4-4b1a-894a-047bfcc7744c", "896dfe27-0cb4-4b1a-894a-047bfcc7744c", "Administrator", "ADMINISTRATOR" },
                    { "8a2d1157-e3d9-4c37-8375-2bc84f301a2e", "8a2d1157-e3d9-4c37-8375-2bc84f301a2e", "OrganizationAdmin", "ORGANIZATIONADMIN" },
                    { "b9b2137b-66fc-41c0-8413-eef65bf8d34d", "b9b2137b-66fc-41c0-8413-eef65bf8d34d", "RegionAdmin", "REGIONADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50c0642a-e599-4ba8-909b-1d351e25b3c3");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "896dfe27-0cb4-4b1a-894a-047bfcc7744c");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a2d1157-e3d9-4c37-8375-2bc84f301a2e");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9b2137b-66fc-41c0-8413-eef65bf8d34d");

            migrationBuilder.DropColumn(
                name: "HealthRegionId",
                schema: "authDB",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HealthSystemId",
                schema: "authDB",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "authDB",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                schema: "authDB",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19ec1414-c440-40f4-8bc9-2973d765aa73", "19ec1414-c440-40f4-8bc9-2973d765aa73", "RegionAdmin", "REGIONADMIN" },
                    { "1d192029-4567-418b-9dea-7e6cf332790c", "1d192029-4567-418b-9dea-7e6cf332790c", "OrganizationAdmin", "ORGANIZATIONADMIN" },
                    { "7866ea46-c88c-4e9c-b473-ca3991f36ea1", "7866ea46-c88c-4e9c-b473-ca3991f36ea1", "Admin", "ADMIN" },
                    { "eb3266fd-b495-49cf-8281-ca28ed9d4ac1", "eb3266fd-b495-49cf-8281-ca28ed9d4ac1", "HealthSystemUser", "HEALTHSYSTEMUSER" }
                });
        }
    }
}
