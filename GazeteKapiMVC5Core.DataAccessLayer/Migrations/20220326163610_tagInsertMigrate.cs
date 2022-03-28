using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class tagInsertMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_tagNews_NewsId",
                table: "tagNews",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_tagNews_TagId",
                table: "tagNews",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tagNews");

            migrationBuilder.DropTable(
                name: "tagNames");
        }
    }
}
