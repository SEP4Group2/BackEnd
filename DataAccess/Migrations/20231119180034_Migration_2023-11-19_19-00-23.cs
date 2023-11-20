using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20231119_190023 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Plants");

            migrationBuilder.CreateTable(
                name: "PlantData",
                columns: table => new
                {
                    TimeStamp = table.Column<string>(type: "text", nullable: false),
                    Humidity = table.Column<float>(type: "real", nullable: true),
                    Temperature = table.Column<float>(type: "real", nullable: true),
                    UVLight = table.Column<float>(type: "real", nullable: false),
                    Moisture = table.Column<float>(type: "real", nullable: false),
                    TankLevel = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantData", x => x.TimeStamp);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlantData");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Plants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
