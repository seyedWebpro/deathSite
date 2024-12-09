using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "condolenceMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeceasedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condolenceMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RenewalFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValidityPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageCount = table.Column<int>(type: "int", nullable: false),
                    VideoCount = table.Column<int>(type: "int", nullable: false),
                    NotificationCount = table.Column<int>(type: "int", nullable: false),
                    AudioFileLimit = table.Column<int>(type: "int", nullable: false),
                    BarcodeEnabled = table.Column<bool>(type: "bit", nullable: false),
                    DisplayEnabled = table.Column<bool>(type: "bit", nullable: false),
                    TemplateSelectionEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CondolenceMessageEnabled = table.Column<bool>(type: "bit", nullable: false),
                    VisitorContentSubmissionEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LocationAndNavigationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    SharingEnabled = table.Column<bool>(type: "bit", nullable: false),
                    File360DegreeEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UpdateCapabilityEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shahids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthBorn = table.Column<DateOnly>(type: "date", nullable: false),
                    BirthDead = table.Column<DateOnly>(type: "date", nullable: false),
                    PlaceDead = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Operations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Will = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Memories = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoiceUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CauseOfMartyrdom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastResponsibility = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gorooh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Yegan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Niru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ghesmat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Poem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shahids", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "smsTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_smsTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "packages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_PackageId",
                table: "users",
                column: "PackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "condolenceMessages");

            migrationBuilder.DropTable(
                name: "shahids");

            migrationBuilder.DropTable(
                name: "smsTemplates");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "userTokens");

            migrationBuilder.DropTable(
                name: "packages");
        }
    }
}
