using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addanalises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RotuloX = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RotuloY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fk_Usuario = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Analises_AspNetUsers_Fk_Usuario",
                        column: x => x.Fk_Usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalisesDados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValorY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorX = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fk_Analise = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalisesDados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalisesDados_Analises_Fk_Analise",
                        column: x => x.Fk_Analise,
                        principalTable: "Analises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analises_Fk_Usuario",
                table: "Analises",
                column: "Fk_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_AnalisesDados_Fk_Analise",
                table: "AnalisesDados",
                column: "Fk_Analise");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalisesDados");

            migrationBuilder.DropTable(
                name: "Analises");
        }
    }
}
