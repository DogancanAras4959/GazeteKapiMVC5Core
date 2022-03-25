using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class newsCreateMigrateChangeContentName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "news",
                newName: "NewsContent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewsContent",
                table: "news",
                newName: "Content");
        }
    }
}
