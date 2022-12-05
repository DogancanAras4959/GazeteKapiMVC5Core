using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authorize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorizeName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    AuthorizeCode = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authorize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "currency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    crossorder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    currencyCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    currencyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ForexBuying = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ForexSelling = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BanknoteBuying = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BanknoteSelling = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CrossRateOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CrossRateUSD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isRateOrDown = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "footerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_footerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameSurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    emailAdress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    jobs = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    submitDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stylePosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StyleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stylePosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tagNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tagNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roleAuthorize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    AuthorizeId = table.Column<int>(type: "int", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleAuthorize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_roleAuthorize_authorize_AuthorizeId",
                        column: x => x.AuthorizeId,
                        principalTable: "authorize",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roleAuthorize_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    EmailAdress = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    ProfileImage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aboutus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aboutus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aboutus_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "brand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_brand_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sorted = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    StyleId = table.Column<int>(type: "int", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_categories_footerTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "footerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_categories_stylePosts_StyleId",
                        column: x => x.StyleId,
                        principalTable: "stylePosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_categories_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cookie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cookie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cookie_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "guest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    GuestImage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Biography = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Gmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Youtube = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_guest_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(375)", maxLength: 375, nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_medias_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "privacy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_privacy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_privacy_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "publishTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publishTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_publishTypes_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LogIsActive = table.Column<bool>(type: "bit", nullable: false),
                    LogSystemErrorActive = table.Column<bool>(type: "bit", nullable: false),
                    GetAgencyNewsService = table.Column<bool>(type: "bit", nullable: false),
                    SiteSlogan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActiveSettings = table.Column<bool>(type: "bit", nullable: false),
                    IsCurrencyService = table.Column<bool>(type: "bit", nullable: false),
                    FooterLogo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CopyrightText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyrightTextTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_setting_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stream",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stream", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stream_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "termsofUse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_termsofUse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_termsofUse_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Spot = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NewsContent = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Sorted = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VideoSlug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsTitle = table.Column<bool>(type: "bit", nullable: false),
                    IsLock = table.Column<bool>(type: "bit", nullable: false),
                    IsOpenNotifications = table.Column<bool>(type: "bit", nullable: false),
                    IsCommentActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSlide = table.Column<bool>(type: "bit", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Sound = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentNewsId = table.Column<int>(type: "int", nullable: false),
                    RowNo = table.Column<int>(type: "int", nullable: false),
                    ColNo = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: false),
                    PublishTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news", x => x.Id);
                    table.ForeignKey(
                        name: "FK_news_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_news_guest_GuestId",
                        column: x => x.GuestId,
                        principalTable: "guest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_news_publishTypes_PublishTypeId",
                        column: x => x.PublishTypeId,
                        principalTable: "publishTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_news_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "seoScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    IsCreated = table.Column<bool>(type: "bit", nullable: false),
                    NewsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seoScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seoScores_news_NewsId",
                        column: x => x.NewsId,
                        principalTable: "news",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tagNews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tagNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tagNews_news_NewsId",
                        column: x => x.NewsId,
                        principalTable: "news",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tagNews_tagNames_TagId",
                        column: x => x.TagId,
                        principalTable: "tagNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seoCheckMeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeLevel = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    Requirement = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: true),
                    Point = table.Column<int>(type: "int", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    metaCode = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    SeoScoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seoCheckMeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seoCheckMeta_seoScores_SeoScoreId",
                        column: x => x.SeoScoreId,
                        principalTable: "seoScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aboutus_UserId",
                table: "aboutus",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_brand_UserId",
                table: "brand",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_StyleId",
                table: "categories",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_TypeId",
                table: "categories",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_UserId",
                table: "categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_cookie_UserId",
                table: "cookie",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_guest_UserId",
                table: "guest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_medias_userId",
                table: "medias",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_news_CategoryId",
                table: "news",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_news_GuestId",
                table: "news",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_news_PublishTypeId",
                table: "news",
                column: "PublishTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_news_UserId",
                table: "news",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_privacy_UserId",
                table: "privacy",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_publishTypes_UserId",
                table: "publishTypes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_roleAuthorize_AuthorizeId",
                table: "roleAuthorize",
                column: "AuthorizeId");

            migrationBuilder.CreateIndex(
                name: "IX_roleAuthorize_RoleId",
                table: "roleAuthorize",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_seoCheckMeta_SeoScoreId",
                table: "seoCheckMeta",
                column: "SeoScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_seoScores_NewsId",
                table: "seoScores",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_setting_UserId",
                table: "setting",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_stream_UserId",
                table: "stream",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tagNews_NewsId",
                table: "tagNews",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_tagNews_TagId",
                table: "tagNews",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_termsofUse_UserId",
                table: "termsofUse",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aboutus");

            migrationBuilder.DropTable(
                name: "brand");

            migrationBuilder.DropTable(
                name: "cookie");

            migrationBuilder.DropTable(
                name: "currency");

            migrationBuilder.DropTable(
                name: "medias");

            migrationBuilder.DropTable(
                name: "members");

            migrationBuilder.DropTable(
                name: "privacy");

            migrationBuilder.DropTable(
                name: "roleAuthorize");

            migrationBuilder.DropTable(
                name: "seoCheckMeta");

            migrationBuilder.DropTable(
                name: "setting");

            migrationBuilder.DropTable(
                name: "stream");

            migrationBuilder.DropTable(
                name: "tagNews");

            migrationBuilder.DropTable(
                name: "termsofUse");

            migrationBuilder.DropTable(
                name: "authorize");

            migrationBuilder.DropTable(
                name: "seoScores");

            migrationBuilder.DropTable(
                name: "tagNames");

            migrationBuilder.DropTable(
                name: "news");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "guest");

            migrationBuilder.DropTable(
                name: "publishTypes");

            migrationBuilder.DropTable(
                name: "footerTypes");

            migrationBuilder.DropTable(
                name: "stylePosts");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
