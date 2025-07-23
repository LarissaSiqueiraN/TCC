using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class confrelacaoencomendaproduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Encomendas_Fk_Encomenda",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_Fk_Encomenda",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Fk_Encomenda",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "EncomendaId",
                table: "Produtos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_EncomendaId",
                table: "Produtos",
                column: "EncomendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Encomendas_EncomendaId",
                table: "Produtos",
                column: "EncomendaId",
                principalTable: "Encomendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Encomendas_EncomendaId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_EncomendaId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "EncomendaId",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "Fk_Encomenda",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Fk_Encomenda",
                table: "Produtos",
                column: "Fk_Encomenda");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Encomendas_Fk_Encomenda",
                table: "Produtos",
                column: "Fk_Encomenda",
                principalTable: "Encomendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
