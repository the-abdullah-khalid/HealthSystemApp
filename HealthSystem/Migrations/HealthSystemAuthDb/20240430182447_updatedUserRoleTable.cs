using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthSystemApp.Migrations.HealthSystemAuthDb
{
    /// <inheritdoc />
    public partial class updatedUserRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "ClaimedId",
                schema: "authDB",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "ClaimedId",
                schema: "authDB",
                table: "AspNetUserRoles");

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
    }
}
