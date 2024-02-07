using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class correcaoprodutofoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoFoto_Produtos_FK_Produto",
                table: "ProdutoFoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProdutoFoto",
                table: "ProdutoFoto");

            migrationBuilder.RenameTable(
                name: "ProdutoFoto",
                newName: "ProdutoFotos");

            migrationBuilder.RenameIndex(
                name: "IX_ProdutoFoto_FK_Produto",
                table: "ProdutoFotos",
                newName: "IX_ProdutoFotos_FK_Produto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProdutoFotos",
                table: "ProdutoFotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoFotos_Produtos_FK_Produto",
                table: "ProdutoFotos",
                column: "FK_Produto",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoFotos_Produtos_FK_Produto",
                table: "ProdutoFotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProdutoFotos",
                table: "ProdutoFotos");

            migrationBuilder.RenameTable(
                name: "ProdutoFotos",
                newName: "ProdutoFoto");

            migrationBuilder.RenameIndex(
                name: "IX_ProdutoFotos_FK_Produto",
                table: "ProdutoFoto",
                newName: "IX_ProdutoFoto_FK_Produto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProdutoFoto",
                table: "ProdutoFoto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoFoto_Produtos_FK_Produto",
                table: "ProdutoFoto",
                column: "FK_Produto",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
