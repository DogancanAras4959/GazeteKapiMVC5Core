using Microsoft.EntityFrameworkCore.Migrations;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    public partial class currencyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    CrossRateUSD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currency", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "currency");
        }
    }
}
