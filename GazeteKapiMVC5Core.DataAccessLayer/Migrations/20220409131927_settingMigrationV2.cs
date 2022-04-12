using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class settingMigrationV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CopyrightText",
                table: "setting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CopyrightTextTitle",
                table: "setting",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FooterLogo",
                table: "setting",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CopyrightText",
                table: "setting");

            migrationBuilder.DropColumn(
                name: "CopyrightTextTitle",
                table: "setting");

            migrationBuilder.DropColumn(
                name: "FooterLogo",
                table: "setting");
        }
    }
}
