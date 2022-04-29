using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class memberMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameSurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    emailAdress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    description = table.Column<string>(type: "nvarchar(MAX)", maxLength: int.MaxValue, nullable: true),
                    jobs = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    submitDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "members");
        }
    }
}
