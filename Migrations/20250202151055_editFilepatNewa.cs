using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class editFilepatNewa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePaths",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "ImagePaths",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
