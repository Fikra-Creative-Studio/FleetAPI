using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class campousoveiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmUso",
                table: "Veiculos");

            migrationBuilder.AddColumn<string>(
                name: "EmUsoPor",
                table: "Veiculos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmUsoPor",
                table: "Veiculos");

            migrationBuilder.AddColumn<bool>(
                name: "EmUso",
                table: "Veiculos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
