using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthSystemApp.Migrations.HealthSystemAuthDb
{
    /// <inheritdoc />
    public partial class AddCustomUserProperties3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                schema: "authDB",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1adf5d66-8e96-4f70-b30e-65bccb3105c3", "1adf5d66-8e96-4f70-b30e-65bccb3105c3", "HealthSystemAdmin", "HEALTHSYSTEMADMIN" },
                    { "aaa0c90e-c0bf-42be-819b-4d6777de6eb3", "aaa0c90e-c0bf-42be-819b-4d6777de6eb3", "RegionAdmin", "REGIONADMIN" },
                    { "c3e90feb-3173-4030-b748-7dd996fbd48c", "c3e90feb-3173-4030-b748-7dd996fbd48c", "Administrator", "ADMINISTRATOR" },
                    { "f2bbce0b-2066-4993-ba6b-27b73bb7eb25", "f2bbce0b-2066-4993-ba6b-27b73bb7eb25", "OrganizationAdmin", "ORGANIZATIONADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1adf5d66-8e96-4f70-b30e-65bccb3105c3");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaa0c90e-c0bf-42be-819b-4d6777de6eb3");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3e90feb-3173-4030-b748-7dd996fbd48c");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2bbce0b-2066-4993-ba6b-27b73bb7eb25");

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
    }
}
