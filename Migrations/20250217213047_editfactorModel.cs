using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class editfactorModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Factors");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "Factors",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "Factors");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Factors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
