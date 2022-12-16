using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class bannerRotateCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rotate",
                table: "banners");

            migrationBuilder.AddColumn<int>(
                name: "RotateId",
                table: "banners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "bannerRotate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RotateName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bannerRotate", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_banners_RotateId",
                table: "banners",
                column: "RotateId");

            migrationBuilder.AddForeignKey(
                name: "FK_banners_bannerRotate_RotateId",
                table: "banners",
                column: "RotateId",
                principalTable: "bannerRotate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_banners_bannerRotate_RotateId",
                table: "banners");

            migrationBuilder.DropTable(
                name: "bannerRotate");

            migrationBuilder.DropIndex(
                name: "IX_banners_RotateId",
                table: "banners");

            migrationBuilder.DropColumn(
                name: "RotateId",
                table: "banners");

            migrationBuilder.AddColumn<string>(
                name: "Rotate",
                table: "banners",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
