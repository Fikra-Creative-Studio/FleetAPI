using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class atualizacaocolunasveiculoschecklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VeiculoId",
                table: "Checklists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VeiculoId",
                table: "Checklists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
