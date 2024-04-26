using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthSystemApp.Migrations
{
    /// <inheritdoc />
    public partial class _1st : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "healthDB");

            migrationBuilder.CreateTable(
                name: "healthRegions",
                schema: "healthDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_healthRegions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "healthSystems",
                schema: "healthDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_healthSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "organizations",
                schema: "healthDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "healthSystemHealthRegions",
                schema: "healthDB",
                columns: table => new
                {
                    HealthSystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HealthRegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_healthSystemHealthRegions", x => new { x.HealthRegionId, x.HealthSystemId });
                    table.ForeignKey(
                        name: "FK_healthSystemHealthRegions_healthRegions_HealthRegionId",
                        column: x => x.HealthRegionId,
                        principalSchema: "healthDB",
                        principalTable: "healthRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_healthSystemHealthRegions_healthSystems_HealthSystemId",
                        column: x => x.HealthSystemId,
                        principalSchema: "healthDB",
                        principalTable: "healthSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "healthRegionOrganizations",
                schema: "healthDB",
                columns: table => new
                {
                    HealthRegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_healthRegionOrganizations", x => new { x.HealthRegionId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_healthRegionOrganizations_healthRegions_HealthRegionId",
                        column: x => x.HealthRegionId,
                        principalSchema: "healthDB",
                        principalTable: "healthRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_healthRegionOrganizations_organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "healthDB",
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_healthRegionOrganizations_OrganizationId",
                schema: "healthDB",
                table: "healthRegionOrganizations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_healthSystemHealthRegions_HealthSystemId",
                schema: "healthDB",
                table: "healthSystemHealthRegions",
                column: "HealthSystemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "healthRegionOrganizations",
                schema: "healthDB");

            migrationBuilder.DropTable(
                name: "healthSystemHealthRegions",
                schema: "healthDB");

            migrationBuilder.DropTable(
                name: "organizations",
                schema: "healthDB");

            migrationBuilder.DropTable(
                name: "healthRegions",
                schema: "healthDB");

            migrationBuilder.DropTable(
                name: "healthSystems",
                schema: "healthDB");
        }
    }
}
