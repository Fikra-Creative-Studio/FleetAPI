using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Migrations
{
    /// <inheritdoc />
    public partial class Abastecimento3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abastecimento_Estabelecimentos_EstabelecimentosId",
                table: "Abastecimento");

            migrationBuilder.DropForeignKey(
                name: "FK_Abastecimento_Usuarios_UsuarioId",
                table: "Abastecimento");

            migrationBuilder.DropForeignKey(
                name: "FK_Abastecimento_Veiculos_VeiculosId",
                table: "Abastecimento");

            migrationBuilder.DropForeignKey(
                name: "FK_Abastecimento_Workspaces_WorkspaceId",
                table: "Abastecimento");

            migrationBuilder.DropForeignKey(
                name: "FK_AbastecimentoImagens_Abastecimento_AbastecimentoId",
                table: "AbastecimentoImagens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Abastecimento",
                table: "Abastecimento");

            migrationBuilder.DropColumn(
                name: "EstabelecimentoId",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "VeiculoId",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "EstabelecimentoId",
                table: "Abastecimento");

            migrationBuilder.DropColumn(
                name: "VeiculoId",
                table: "Abastecimento");

            migrationBuilder.RenameTable(
                name: "Abastecimento",
                newName: "Abastecimentos");

            migrationBuilder.RenameIndex(
                name: "IX_Abastecimento_WorkspaceId",
                table: "Abastecimentos",
                newName: "IX_Abastecimentos_WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Abastecimento_VeiculosId",
                table: "Abastecimentos",
                newName: "IX_Abastecimentos_VeiculosId");

            migrationBuilder.RenameIndex(
                name: "IX_Abastecimento_UsuarioId",
                table: "Abastecimentos",
                newName: "IX_Abastecimentos_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Abastecimento_EstabelecimentosId",
                table: "Abastecimentos",
                newName: "IX_Abastecimentos_EstabelecimentosId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Abastecimentos",
                table: "Abastecimentos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbastecimentoImagens_Abastecimentos_AbastecimentoId",
                table: "AbastecimentoImagens",
                column: "AbastecimentoId",
                principalTable: "Abastecimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Abastecimentos_Estabelecimentos_EstabelecimentosId",
                table: "Abastecimentos",
                column: "EstabelecimentosId",
                principalTable: "Estabelecimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Abastecimentos_Usuarios_UsuarioId",
                table: "Abastecimentos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Abastecimentos_Veiculos_VeiculosId",
                table: "Abastecimentos",
                column: "VeiculosId",
                principalTable: "Veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Abastecimentos_Workspaces_WorkspaceId",
                table: "Abastecimentos",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbastecimentoImagens_Abastecimentos_AbastecimentoId",
                table: "AbastecimentoImagens");

            migrationBuilder.DropForeignKey(
                name: "FK_Abastecimentos_Estabelecimentos_EstabelecimentosId",
                table: "Abastecimentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Abastecimentos_Usuarios_UsuarioId",
                table: "Abastecimentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Abastecimentos_Veiculos_VeiculosId",
                table: "Abastecimentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Abastecimentos_Workspaces_WorkspaceId",
                table: "Abastecimentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Abastecimentos",
                table: "Abastecimentos");

            migrationBuilder.RenameTable(
                name: "Abastecimentos",
                newName: "Abastecimento");

            migrationBuilder.RenameIndex(
                name: "IX_Abastecimentos_WorkspaceId",
                table: "Abastecimento",
                newName: "IX_Abastecimento_WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Abastecimentos_VeiculosId",
                table: "Abastecimento",
                newName: "IX_Abastecimento_VeiculosId");

            migrationBuilder.RenameIndex(
                name: "IX_Abastecimentos_UsuarioId",
                table: "Abastecimento",
                newName: "IX_Abastecimento_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Abastecimentos_EstabelecimentosId",
                table: "Abastecimento",
                newName: "IX_Abastecimento_EstabelecimentosId");

            migrationBuilder.AddColumn<int>(
                name: "EstabelecimentoId",
                table: "Manutencao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VeiculoId",
                table: "Manutencao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstabelecimentoId",
                table: "Abastecimento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VeiculoId",
                table: "Abastecimento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Abastecimento",
                table: "Abastecimento",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Abastecimento_Estabelecimentos_EstabelecimentosId",
                table: "Abastecimento",
                column: "EstabelecimentosId",
                principalTable: "Estabelecimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Abastecimento_Usuarios_UsuarioId",
                table: "Abastecimento",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Abastecimento_Veiculos_VeiculosId",
                table: "Abastecimento",
                column: "VeiculosId",
                principalTable: "Veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Abastecimento_Workspaces_WorkspaceId",
                table: "Abastecimento",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbastecimentoImagens_Abastecimento_AbastecimentoId",
                table: "AbastecimentoImagens",
                column: "AbastecimentoId",
                principalTable: "Abastecimento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
