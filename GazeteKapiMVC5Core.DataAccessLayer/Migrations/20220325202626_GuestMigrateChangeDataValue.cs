using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class GuestMigrateChangeDataValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "guest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "guest",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "guest",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "guest");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "guest");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "guest");
        }
    }
}
