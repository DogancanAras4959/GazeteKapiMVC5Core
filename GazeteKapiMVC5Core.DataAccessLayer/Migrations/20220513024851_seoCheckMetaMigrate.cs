using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class seoCheckMetaMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "seoCheckMeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeLevel = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    Requirement = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: true),
                    Point = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_seoCheckMeta_SeoScoreId",
                table: "seoCheckMeta",
                column: "SeoScoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "seoCheckMeta");
        }
    }
}
