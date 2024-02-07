using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class teste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdutoFoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeArquivo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DisplayeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extensao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FK_Produto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoFoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoFoto_Produtos_FK_Produto",
                        column: x => x.FK_Produto,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoFoto_FK_Produto",
                table: "ProdutoFoto",
                column: "FK_Produto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoFoto");
        }
    }
}
