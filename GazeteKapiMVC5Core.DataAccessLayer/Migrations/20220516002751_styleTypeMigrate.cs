using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class styleTypeMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_footerTypes_TypeId",
                table: "categories");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StyleId",
                table: "categories",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_categories_StyleId",
                table: "categories",
                column: "StyleId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_footerTypes_TypeId",
                table: "categories",
                column: "TypeId",
                principalTable: "footerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_categories_stylePosts_StyleId",
                table: "categories",
                column: "StyleId",
                principalTable: "stylePosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_footerTypes_TypeId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_categories_stylePosts_StyleId",
                table: "categories");

            migrationBuilder.DropTable(
                name: "stylePosts");

            migrationBuilder.DropIndex(
                name: "IX_categories_StyleId",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "StyleId",
                table: "categories");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_categories_footerTypes_TypeId",
                table: "categories",
                column: "TypeId",
                principalTable: "footerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
