using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthSystemApp.Migrations.HealthSystemAuthDb
{
    /// <inheritdoc />
    public partial class AddCustomUserProperties2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "5020f717-9ad3-48fd-9517-8406be172266", "5020f717-9ad3-48fd-9517-8406be172266", "Administrator", "ADMINISTRATOR" },
                    { "db39d269-e12f-418a-adca-3007124c740f", "db39d269-e12f-418a-adca-3007124c740f", "RegionAdmin", "REGIONADMIN" },
                    { "e23ba460-1a6a-43e7-aca4-aab995ab4f49", "e23ba460-1a6a-43e7-aca4-aab995ab4f49", "HealthSystemAdmin", "HEALTHSYSTEMADMIN" },
                    { "f38e4292-f79f-46eb-adfd-5a11b3d0107a", "f38e4292-f79f-46eb-adfd-5a11b3d0107a", "OrganizationAdmin", "ORGANIZATIONADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5020f717-9ad3-48fd-9517-8406be172266");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db39d269-e12f-418a-adca-3007124c740f");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e23ba460-1a6a-43e7-aca4-aab995ab4f49");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f38e4292-f79f-46eb-adfd-5a11b3d0107a");

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
    }
}
