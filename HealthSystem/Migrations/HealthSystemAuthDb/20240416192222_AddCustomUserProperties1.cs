using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthSystemApp.Migrations.HealthSystemAuthDb
{
    /// <inheritdoc />
    public partial class AddCustomUserProperties1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "1708e4e0-058c-4655-80fd-416245e6182b", "1708e4e0-058c-4655-80fd-416245e6182b", "HealthSystemAdmin", "HEALTHSYSTEMADMIN" },
                    { "6b211be8-f551-4d40-b302-03418a9fb8c6", "6b211be8-f551-4d40-b302-03418a9fb8c6", "Administrator", "ADMINISTRATOR" },
                    { "ab00d3b5-6ed7-449c-b703-d03a4758e10e", "ab00d3b5-6ed7-449c-b703-d03a4758e10e", "RegionAdmin", "REGIONADMIN" },
                    { "b924d6f5-aa16-4f90-ae66-a250235b5e60", "b924d6f5-aa16-4f90-ae66-a250235b5e60", "OrganizationAdmin", "ORGANIZATIONADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1708e4e0-058c-4655-80fd-416245e6182b");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b211be8-f551-4d40-b302-03418a9fb8c6");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab00d3b5-6ed7-449c-b703-d03a4758e10e");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b924d6f5-aa16-4f90-ae66-a250235b5e60");

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
    }
}
