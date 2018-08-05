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
                    BoardA_Height = table.Column<int>(nullable: false),
                    BoardA_Width = table.Column<int>(nullable: false),
                    BoardB_Height = table.Column<int>(nullable: false),
                    BoardB_Width = table.Column<int>(nullable: false),
                    PlayerA = table.Column<string>(nullable: true),
                    PlayerB = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
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
