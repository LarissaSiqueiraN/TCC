using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class upgradenomeencomenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimeiroNome",
                table: "Encomendas");

            migrationBuilder.RenameColumn(
                name: "SegundoNome",
                table: "Encomendas",
                newName: "Nome");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Encomendas",
                newName: "SegundoNome");

            migrationBuilder.AddColumn<string>(
                name: "PrimeiroNome",
                table: "Encomendas",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }
    }
}
