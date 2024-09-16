using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class atualizacaocolunaschecklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_Usuarios_UsuarioId",
                table: "Checklist");

            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_Veiculos_VeiculosId",
                table: "Checklist");

            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_Workspaces_WorkspaceId",
                table: "Checklist");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistImagens_Checklist_ChecklistId",
                table: "ChecklistImagens");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistOpcao_Checklist_ChecklistId",
                table: "ChecklistOpcao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Checklist",
                table: "Checklist");

            migrationBuilder.RenameTable(
                name: "Checklist",
                newName: "Checklists");

            migrationBuilder.RenameIndex(
                name: "IX_Checklist_WorkspaceId",
                table: "Checklists",
                newName: "IX_Checklists_WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Checklist_VeiculosId",
                table: "Checklists",
                newName: "IX_Checklists_VeiculosId");

            migrationBuilder.RenameIndex(
                name: "IX_Checklist_UsuarioId",
                table: "Checklists",
                newName: "IX_Checklists_UsuarioId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataDevolucao",
                table: "Checklists",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "Avaria",
                table: "Checklists",
                type: "tinyint(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Checklists",
                table: "Checklists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistImagens_Checklists_ChecklistId",
                table: "ChecklistImagens",
                column: "ChecklistId",
                principalTable: "Checklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistOpcao_Checklists_ChecklistId",
                table: "ChecklistOpcao",
                column: "ChecklistId",
                principalTable: "Checklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklists_Usuarios_UsuarioId",
                table: "Checklists",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklists_Veiculos_VeiculosId",
                table: "Checklists",
                column: "VeiculosId",
                principalTable: "Veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklists_Workspaces_WorkspaceId",
                table: "Checklists",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistImagens_Checklists_ChecklistId",
                table: "ChecklistImagens");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistOpcao_Checklists_ChecklistId",
                table: "ChecklistOpcao");

            migrationBuilder.DropForeignKey(
                name: "FK_Checklists_Usuarios_UsuarioId",
                table: "Checklists");

            migrationBuilder.DropForeignKey(
                name: "FK_Checklists_Veiculos_VeiculosId",
                table: "Checklists");

            migrationBuilder.DropForeignKey(
                name: "FK_Checklists_Workspaces_WorkspaceId",
                table: "Checklists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Checklists",
                table: "Checklists");

            migrationBuilder.RenameTable(
                name: "Checklists",
                newName: "Checklist");

            migrationBuilder.RenameIndex(
                name: "IX_Checklists_WorkspaceId",
                table: "Checklist",
                newName: "IX_Checklist_WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Checklists_VeiculosId",
                table: "Checklist",
                newName: "IX_Checklist_VeiculosId");

            migrationBuilder.RenameIndex(
                name: "IX_Checklists_UsuarioId",
                table: "Checklist",
                newName: "IX_Checklist_UsuarioId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataDevolucao",
                table: "Checklist",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Avaria",
                table: "Checklist",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Checklist",
                table: "Checklist",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_Usuarios_UsuarioId",
                table: "Checklist",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_Veiculos_VeiculosId",
                table: "Checklist",
                column: "VeiculosId",
                principalTable: "Veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_Workspaces_WorkspaceId",
                table: "Checklist",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistImagens_Checklist_ChecklistId",
                table: "ChecklistImagens",
                column: "ChecklistId",
                principalTable: "Checklist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistOpcao_Checklist_ChecklistId",
                table: "ChecklistOpcao",
                column: "ChecklistId",
                principalTable: "Checklist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
