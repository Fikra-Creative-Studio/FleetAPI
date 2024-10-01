using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class veiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Veiculos",
                newName: "EmUso");

            migrationBuilder.AlterColumn<int>(
                name: "Combustivel",
                table: "Veiculos",
                type: "int",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Chassi",
                table: "Veiculos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Cor",
                table: "Veiculos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "Veiculos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Renavam",
                table: "Veiculos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Seguradora",
                table: "Veiculos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Veiculos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "VencimentoSeguro",
                table: "Veiculos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chassi",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Cor",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Renavam",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Seguradora",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "VencimentoSeguro",
                table: "Veiculos");

            migrationBuilder.RenameColumn(
                name: "EmUso",
                table: "Veiculos",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "Combustivel",
                table: "Veiculos",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
