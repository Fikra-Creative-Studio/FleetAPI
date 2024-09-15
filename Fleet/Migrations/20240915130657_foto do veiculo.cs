using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class fotodoveiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Veiculos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Veiculos");
        }
    }
}
