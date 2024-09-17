using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class atualizacaocolunasvisitas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstabelecimentoId",
                table: "Visitas");

            migrationBuilder.DropColumn(
                name: "VeiculoId",
                table: "Visitas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstabelecimentoId",
                table: "Visitas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VeiculoId",
                table: "Visitas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
