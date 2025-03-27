using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class INIT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aboutUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Poshtiban = table.Column<int>(type: "int", nullable: true),
                    Sabeghe = table.Column<int>(type: "int", nullable: true),
                    ShahidSabt = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aboutUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesktopImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactMeForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMeForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GolestanShohadaSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolestanShohadaSections", x => x.Id);
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
                    Order = table.Column<int>(type: "int", nullable: true),
                    IconImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuSiteSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    UpdateCapabilityEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsFreePackage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GatewayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "posters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sarbarg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sarbarg", x => x.Id);
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
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
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
                name: "Deceaseds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceOfMartyrdom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfMartyrdom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Khaterat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoiceUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlaceBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Job = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tahsilat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildrenNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HowDeath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OstanMazar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityMazar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aramestan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhesteMazar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RadifMazar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberMazar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ghaleb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<int>(type: "int", nullable: false),
                    SarbargId = table.Column<int>(type: "int", nullable: true),
                    QRCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deceaseds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deceaseds_Sarbarg_SarbargId",
                        column: x => x.SarbargId,
                        principalTable: "Sarbarg",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deceaseds_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeceasedId = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentGateway = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factors_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factors_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    PoemVerseTwo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Approved = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shahids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shahids_users_UserId",
                        column: x => x.UserId,
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
                    Status = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "DeathViewCounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeceasedId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeathViewCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeathViewCounts_Deceaseds_DeceasedId",
                        column: x => x.DeceasedId,
                        principalTable: "Deceaseds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeathViewCounts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeceasedLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeceasedId = table.Column<int>(type: "int", nullable: false),
                    Balad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Neshan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleMap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mokhtasat = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeceasedLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeceasedLocations_Deceaseds_DeceasedId",
                        column: x => x.DeceasedId,
                        principalTable: "Deceaseds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Elamiehs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeceasedId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elamiehs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Elamiehs_Deceaseds_DeceasedId",
                        column: x => x.DeceasedId,
                        principalTable: "Deceaseds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Elamiehs_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikeDeceaseds",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DeceasedId = table.Column<int>(type: "int", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeDeceaseds", x => new { x.UserId, x.DeceasedId });
                    table.ForeignKey(
                        name: "FK_LikeDeceaseds_Deceaseds_DeceasedId",
                        column: x => x.DeceasedId,
                        principalTable: "Deceaseds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikeDeceaseds_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedDeceaseds",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DeceasedId = table.Column<int>(type: "int", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedDeceaseds", x => new { x.UserId, x.DeceasedId });
                    table.ForeignKey(
                        name: "FK_SavedDeceaseds_Deceaseds_DeceasedId",
                        column: x => x.DeceasedId,
                        principalTable: "Deceaseds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedDeceaseds_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeceasedPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeceasedId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFreePackage = table.Column<bool>(type: "bit", nullable: false),
                    FactorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeceasedPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeceasedPackages_Deceaseds_DeceasedId",
                        column: x => x.DeceasedId,
                        principalTable: "Deceaseds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeceasedPackages_Factors_FactorId",
                        column: x => x.FactorId,
                        principalTable: "Factors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeceasedPackages_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "ShahidUpdateRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShahidId = table.Column<int>(type: "int", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Will = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoiceUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShahidUpdateRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShahidUpdateRequests_shahids_ShahidId",
                        column: x => x.ShahidId,
                        principalTable: "shahids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShahidUpdateRequests_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShahidViewCounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShahidId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShahidViewCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShahidViewCounts_shahids_ShahidId",
                        column: x => x.ShahidId,
                        principalTable: "shahids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShahidViewCounts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CondolenceReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReplyText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CondolenceMessageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondolenceReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CondolenceReplies_condolenceMessages_CondolenceMessageId",
                        column: x => x.CondolenceMessageId,
                        principalTable: "condolenceMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CondolenceReplies_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
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
                name: "IX_CondolenceReplies_CondolenceMessageId",
                table: "CondolenceReplies",
                column: "CondolenceMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CondolenceReplies_UserId",
                table: "CondolenceReplies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeathViewCounts_DeceasedId",
                table: "DeathViewCounts",
                column: "DeceasedId");

            migrationBuilder.CreateIndex(
                name: "IX_DeathViewCounts_UserId",
                table: "DeathViewCounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeceasedLocations_DeceasedId",
                table: "DeceasedLocations",
                column: "DeceasedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeceasedPackages_DeceasedId",
                table: "DeceasedPackages",
                column: "DeceasedId");

            migrationBuilder.CreateIndex(
                name: "IX_DeceasedPackages_FactorId",
                table: "DeceasedPackages",
                column: "FactorId");

            migrationBuilder.CreateIndex(
                name: "IX_DeceasedPackages_PackageId",
                table: "DeceasedPackages",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Deceaseds_SarbargId",
                table: "Deceaseds",
                column: "SarbargId",
                unique: true,
                filter: "[SarbargId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Deceaseds_UserId",
                table: "Deceaseds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Elamiehs_DeceasedId",
                table: "Elamiehs",
                column: "DeceasedId");

            migrationBuilder.CreateIndex(
                name: "IX_Elamiehs_UserId",
                table: "Elamiehs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_PackageId",
                table: "Factors",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_UserId",
                table: "Factors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeDeceaseds_DeceasedId",
                table: "LikeDeceaseds",
                column: "DeceasedId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedDeceaseds_DeceasedId",
                table: "SavedDeceaseds",
                column: "DeceasedId");

            migrationBuilder.CreateIndex(
                name: "IX_shahids_UserId",
                table: "shahids",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShahidTags_TagId",
                table: "ShahidTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ShahidUpdateRequests_ShahidId",
                table: "ShahidUpdateRequests",
                column: "ShahidId");

            migrationBuilder.CreateIndex(
                name: "IX_ShahidUpdateRequests_UserId",
                table: "ShahidUpdateRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShahidViewCounts_ShahidId",
                table: "ShahidViewCounts",
                column: "ShahidId");

            migrationBuilder.CreateIndex(
                name: "IX_ShahidViewCounts_UserId",
                table: "ShahidViewCounts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aboutUs");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "blogs");

            migrationBuilder.DropTable(
                name: "CondolenceReplies");

            migrationBuilder.DropTable(
                name: "ContactMeForms");

            migrationBuilder.DropTable(
                name: "DeathViewCounts");

            migrationBuilder.DropTable(
                name: "DeceasedLocations");

            migrationBuilder.DropTable(
                name: "DeceasedPackages");

            migrationBuilder.DropTable(
                name: "Elamiehs");

            migrationBuilder.DropTable(
                name: "GolestanShohadaSections");

            migrationBuilder.DropTable(
                name: "LikeDeceaseds");

            migrationBuilder.DropTable(
                name: "logoSiteSettings");

            migrationBuilder.DropTable(
                name: "MenuSiteSettings");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "PaymentSettings");

            migrationBuilder.DropTable(
                name: "posters");

            migrationBuilder.DropTable(
                name: "SavedDeceaseds");

            migrationBuilder.DropTable(
                name: "ShahidTags");

            migrationBuilder.DropTable(
                name: "ShahidUpdateRequests");

            migrationBuilder.DropTable(
                name: "ShahidViewCounts");

            migrationBuilder.DropTable(
                name: "smsTemplates");

            migrationBuilder.DropTable(
                name: "Surahs");

            migrationBuilder.DropTable(
                name: "userTokens");

            migrationBuilder.DropTable(
                name: "condolenceMessages");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "shahids");

            migrationBuilder.DropTable(
                name: "Deceaseds");

            migrationBuilder.DropTable(
                name: "packages");

            migrationBuilder.DropTable(
                name: "Sarbarg");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
