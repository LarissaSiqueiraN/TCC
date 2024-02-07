using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class correcaorelacaodeleteprodutos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncomendaProdutos_Encomendas_Fk_Encomenda",
                table: "EncomendaProdutos");

            migrationBuilder.DropForeignKey(
                name: "FK_EncomendaProdutos_Produtos_Fk_Produto",
                table: "EncomendaProdutos");

            migrationBuilder.AddForeignKey(
                name: "FK_EncomendaProdutos_Encomendas_Fk_Encomenda",
                table: "EncomendaProdutos",
                column: "Fk_Encomenda",
                principalTable: "Encomendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EncomendaProdutos_Produtos_Fk_Produto",
                table: "EncomendaProdutos",
                column: "Fk_Produto",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncomendaProdutos_Encomendas_Fk_Encomenda",
                table: "EncomendaProdutos");

            migrationBuilder.DropForeignKey(
                name: "FK_EncomendaProdutos_Produtos_Fk_Produto",
                table: "EncomendaProdutos");

            migrationBuilder.AddForeignKey(
                name: "FK_EncomendaProdutos_Encomendas_Fk_Encomenda",
                table: "EncomendaProdutos",
                column: "Fk_Encomenda",
                principalTable: "Encomendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EncomendaProdutos_Produtos_Fk_Produto",
                table: "EncomendaProdutos",
                column: "Fk_Produto",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
