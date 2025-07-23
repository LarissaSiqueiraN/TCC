using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class correcaorelacionamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Encomendas_EncomendaId",
                table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "EncomendaId",
                table: "Produtos",
                newName: "Fk_Encomenda");

            migrationBuilder.RenameIndex(
                name: "IX_Produtos_EncomendaId",
                table: "Produtos",
                newName: "IX_Produtos_Fk_Encomenda");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Encomendas_Fk_Encomenda",
                table: "Produtos",
                column: "Fk_Encomenda",
                principalTable: "Encomendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Encomendas_Fk_Encomenda",
                table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "Fk_Encomenda",
                table: "Produtos",
                newName: "EncomendaId");

            migrationBuilder.RenameIndex(
                name: "IX_Produtos_Fk_Encomenda",
                table: "Produtos",
                newName: "IX_Produtos_EncomendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Encomendas_EncomendaId",
                table: "Produtos",
                column: "EncomendaId",
                principalTable: "Encomendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
