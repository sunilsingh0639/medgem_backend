using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medgem_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddNavFooter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FooterColumns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Heading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterColumns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FooterConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CtaLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CtaUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CopyrightText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivacyPolicyUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CookiePolicyUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NavItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHighlight = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavItems_NavItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "NavItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FooterLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FooterColumnId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FooterLinks_FooterColumns_FooterColumnId",
                        column: x => x.FooterColumnId,
                        principalTable: "FooterColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FooterConfigs",
                columns: new[] { "Id", "CompanyDescription", "CookiePolicyUrl", "CopyrightText", "CtaLabel", "CtaUrl", "PrivacyPolicyUrl", "TermsUrl", "UpdatedAt" },
                values: new object[] { 1, "Medgem Global Healthcare assists patients in discovering and coordinating access to quality medical care across leading international destinations.", "#", "Medgem Global Healthcare. All rights reserved.", "Free Consultation", "/consultation", "#", "#", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_FooterLinks_FooterColumnId",
                table: "FooterLinks",
                column: "FooterColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_NavItems_ParentId",
                table: "NavItems",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FooterConfigs");

            migrationBuilder.DropTable(
                name: "FooterLinks");

            migrationBuilder.DropTable(
                name: "NavItems");

            migrationBuilder.DropTable(
                name: "FooterColumns");
        }
    }
}
