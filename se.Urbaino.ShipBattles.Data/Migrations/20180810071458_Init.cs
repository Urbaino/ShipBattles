using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace se.Urbaino.ShipBattles.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    PlayerA = table.Column<string>(nullable: true),
                    PlayerB = table.Column<string>(nullable: true),
                    PlayerAState = table.Column<int>(nullable: false),
                    PlayerBState = table.Column<int>(nullable: false),
                    JSONShipsA = table.Column<string>(nullable: true),
                    JSONShotsA = table.Column<string>(nullable: true),
                    HeightA = table.Column<int>(nullable: false),
                    WidthA = table.Column<int>(nullable: false),
                    JSONShipsB = table.Column<string>(nullable: true),
                    JSONShotsB = table.Column<string>(nullable: true),
                    HeightB = table.Column<int>(nullable: false),
                    WidthB = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
