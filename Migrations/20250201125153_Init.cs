﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePaths = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoPaths = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoicePaths = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "logoSiteSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogoImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logoSiteSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuSiteSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuSiteSettings", x => x.Id);
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
                    PlaceOfBurial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BurialSiteLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeadPlaceLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    virtualLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    PoemVerseOne = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoemVerseTwo = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "Surahs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surahs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "ShahidTags",
                columns: table => new
                {
                    ShahidId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShahidTags", x => new { x.ShahidId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ShahidTags_shahids_ShahidId",
                        column: x => x.ShahidId,
                        principalTable: "shahids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShahidTags_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deceaseds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceOfMartyrdom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfMartyrdom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Khaterat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoiceUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deceaseds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deceaseds_users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    DeceasedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condolenceMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_condolenceMessages_Deceaseds_DeceasedId",
                        column: x => x.DeceasedId,
                        principalTable: "Deceaseds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_condolenceMessages_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_condolenceMessages_DeceasedId",
                table: "condolenceMessages",
                column: "DeceasedId");

            migrationBuilder.CreateIndex(
                name: "IX_condolenceMessages_UserId",
                table: "condolenceMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deceaseds_OwnerId",
                table: "Deceaseds",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShahidTags_TagId",
                table: "ShahidTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_users_PackageId",
                table: "users",
                column: "PackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blogs");

            migrationBuilder.DropTable(
                name: "condolenceMessages");

            migrationBuilder.DropTable(
                name: "logoSiteSettings");

            migrationBuilder.DropTable(
                name: "MenuSiteSettings");

            migrationBuilder.DropTable(
                name: "ShahidTags");

            migrationBuilder.DropTable(
                name: "smsTemplates");

            migrationBuilder.DropTable(
                name: "Surahs");

            migrationBuilder.DropTable(
                name: "userTokens");

            migrationBuilder.DropTable(
                name: "Deceaseds");

            migrationBuilder.DropTable(
                name: "shahids");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "packages");
        }
    }
}
