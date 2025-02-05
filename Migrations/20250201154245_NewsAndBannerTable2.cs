using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class NewsAndBannerTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Banners");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Banners",
                newName: "MobileImagePath");

            migrationBuilder.RenameColumn(
                name: "ImagePaths",
                table: "Banners",
                newName: "DesktopImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileImagePath",
                table: "Banners",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "DesktopImagePath",
                table: "Banners",
                newName: "ImagePaths");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
