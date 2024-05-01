using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthSystemApp.Migrations.HealthSystemAuthDb
{
    /// <inheritdoc />
    public partial class updatedUserRoleTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "authDB",
                table: "AspNetUserRoles");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a7211e6-6e46-4c5e-bb98-83fd7f346514");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f7f4c61-fe87-4de0-a240-2adba1562f4f");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56961fdf-1ab9-489d-8605-bba9bc09cb7e");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b129d08c-12f4-4da8-b859-2438f34fb852");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "authDB",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "ClaimedId" });

            migrationBuilder.InsertData(
                schema: "authDB",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4e423c78-d157-4e3e-af1b-a12ad46355ba", "4e423c78-d157-4e3e-af1b-a12ad46355ba", "Administrator", "ADMINISTRATOR" },
                    { "b3d427c9-e0a3-41b1-b16b-b48ee9ca4839", "b3d427c9-e0a3-41b1-b16b-b48ee9ca4839", "OrganizationAdmin", "ORGANIZATIONADMIN" },
                    { "e793c8e2-8c07-443e-b8dd-e2ef2ee3a9a6", "e793c8e2-8c07-443e-b8dd-e2ef2ee3a9a6", "HealthSystemAdmin", "HEALTHSYSTEMADMIN" },
                    { "f893dc57-9fcc-46b1-a013-4c01988ce57a", "f893dc57-9fcc-46b1-a013-4c01988ce57a", "RegionAdmin", "REGIONADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "authDB",
                table: "AspNetUserRoles");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e423c78-d157-4e3e-af1b-a12ad46355ba");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3d427c9-e0a3-41b1-b16b-b48ee9ca4839");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e793c8e2-8c07-443e-b8dd-e2ef2ee3a9a6");

            migrationBuilder.DeleteData(
                schema: "authDB",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f893dc57-9fcc-46b1-a013-4c01988ce57a");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "authDB",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.InsertData(
                schema: "authDB",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a7211e6-6e46-4c5e-bb98-83fd7f346514", "0a7211e6-6e46-4c5e-bb98-83fd7f346514", "RegionAdmin", "REGIONADMIN" },
                    { "1f7f4c61-fe87-4de0-a240-2adba1562f4f", "1f7f4c61-fe87-4de0-a240-2adba1562f4f", "HealthSystemAdmin", "HEALTHSYSTEMADMIN" },
                    { "56961fdf-1ab9-489d-8605-bba9bc09cb7e", "56961fdf-1ab9-489d-8605-bba9bc09cb7e", "Administrator", "ADMINISTRATOR" },
                    { "b129d08c-12f4-4da8-b859-2438f34fb852", "b129d08c-12f4-4da8-b859-2438f34fb852", "OrganizationAdmin", "ORGANIZATIONADMIN" }
                });
        }
    }
}
