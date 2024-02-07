using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class correcaorelacionmentoencomenda : Migration
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

            migrationBuilder.CreateTable(
                name: "EncomendaProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Encomenda = table.Column<int>(type: "int", nullable: false),
                    Fk_Produto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncomendaProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncomendaProdutos_Encomendas_Fk_Encomenda",
                        column: x => x.Fk_Encomenda,
                        principalTable: "Encomendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EncomendaProdutos_Produtos_Fk_Produto",
                        column: x => x.Fk_Produto,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EncomendaProdutos_Fk_Encomenda",
                table: "EncomendaProdutos",
                column: "Fk_Encomenda");

            migrationBuilder.CreateIndex(
                name: "IX_EncomendaProdutos_Fk_Produto",
                table: "EncomendaProdutos",
                column: "Fk_Produto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncomendaProdutos");

            migrationBuilder.AddColumn<int>(
                name: "Fk_Encomenda",
                table: "Produtos",
                type: "int",
                nullable: true);

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
