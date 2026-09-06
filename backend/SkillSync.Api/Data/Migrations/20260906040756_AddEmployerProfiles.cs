using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillSync.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployerProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CompanySize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AboutCompany = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerProfiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployerProfiles_UserId",
                table: "EmployerProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployerProfiles");
        }
    }
}
