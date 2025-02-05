using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class editFilepatBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePaths",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "VideoPaths",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "VoicePaths",
                table: "blogs");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "blogs");

            migrationBuilder.AddColumn<string>(
                name: "ImagePaths",
                table: "blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoPaths",
                table: "blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VoicePaths",
                table: "blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
