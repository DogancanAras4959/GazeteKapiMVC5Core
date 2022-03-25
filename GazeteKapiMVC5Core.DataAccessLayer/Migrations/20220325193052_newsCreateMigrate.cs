using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class newsCreateMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "news",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Spot = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Sorted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsLock = table.Column<bool>(type: "bit", nullable: false),
                    IsOpenNotifications = table.Column<bool>(type: "bit", nullable: false),
                    IsCommentActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSlide = table.Column<bool>(type: "bit", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_guest_UserId",
                table: "guest",
                column: "UserId");

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
                name: "IX_publishTypes_UserId",
                table: "publishTypes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "news");

            migrationBuilder.DropTable(
                name: "guest");

            migrationBuilder.DropTable(
                name: "publishTypes");
        }
    }
}
