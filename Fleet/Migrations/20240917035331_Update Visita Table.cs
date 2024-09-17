using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVisitaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitaOpcao_Visitas_VisitasId",
                table: "VisitaOpcao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VisitaOpcao",
                table: "VisitaOpcao");

            migrationBuilder.RenameTable(
                name: "VisitaOpcao",
                newName: "VisitaOpcoes");

            migrationBuilder.RenameIndex(
                name: "IX_VisitaOpcao_VisitasId",
                table: "VisitaOpcoes",
                newName: "IX_VisitaOpcoes_VisitasId");

            migrationBuilder.AddColumn<string>(
                name: "GPS",
                table: "Visitas",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VisitaOpcoes",
                table: "VisitaOpcoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitaOpcoes_Visitas_VisitasId",
                table: "VisitaOpcoes",
                column: "VisitasId",
                principalTable: "Visitas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitaOpcoes_Visitas_VisitasId",
                table: "VisitaOpcoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VisitaOpcoes",
                table: "VisitaOpcoes");

            migrationBuilder.DropColumn(
                name: "GPS",
                table: "Visitas");

            migrationBuilder.RenameTable(
                name: "VisitaOpcoes",
                newName: "VisitaOpcao");

            migrationBuilder.RenameIndex(
                name: "IX_VisitaOpcoes_VisitasId",
                table: "VisitaOpcao",
                newName: "IX_VisitaOpcao_VisitasId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VisitaOpcao",
                table: "VisitaOpcao",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitaOpcao_Visitas_VisitasId",
                table: "VisitaOpcao",
                column: "VisitasId",
                principalTable: "Visitas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
