using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class manutencao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Manutencao",
                table: "Veiculos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Veiculos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manutencao",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Veiculos");
        }
    }
}
