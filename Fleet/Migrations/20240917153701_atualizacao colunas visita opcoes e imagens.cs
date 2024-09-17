using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class atualizacaocolunasvisitaopcoeseimagens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitaId",
                table: "VisitaOpcoes");

            migrationBuilder.DropColumn(
                name: "VisitaId",
                table: "VisitaImagens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitaId",
                table: "VisitaOpcoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VisitaId",
                table: "VisitaImagens",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
