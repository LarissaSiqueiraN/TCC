using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class upgradeencomenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encomendas_Produtos_FK_Produto",
                table: "Encomendas");

            migrationBuilder.DropIndex(
                name: "IX_Encomendas_FK_Produto",
                table: "Encomendas");

            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "Encomendas",
                newName: "Numero");

            migrationBuilder.RenameColumn(
                name: "FK_Produto",
                table: "Encomendas",
                newName: "JaFurouAntes");

            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "Encomendas",
                newName: "Cep");

            migrationBuilder.AddColumn<int>(
                name: "Fk_Encomenda",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Alergias",
                table: "Encomendas",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Encomendas",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Encomendas",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Encomendas",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Faz5DiasQueVacinou",
                table: "Encomendas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FurouAntes",
                table: "Encomendas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Idade",
                table: "Encomendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LocalFuro",
                table: "Encomendas",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "NoDomicilio",
                table: "Encomendas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Pagamento",
                table: "Encomendas",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimeiroNome",
                table: "Encomendas",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rua",
                table: "Encomendas",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SegundoNome",
                table: "Encomendas",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "TemAlergias",
                table: "Encomendas",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Alergias",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "Faz5DiasQueVacinou",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "FurouAntes",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "Idade",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "LocalFuro",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "NoDomicilio",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "Pagamento",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "PrimeiroNome",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "Rua",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "SegundoNome",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "TemAlergias",
                table: "Encomendas");

            migrationBuilder.RenameColumn(
                name: "Numero",
                table: "Encomendas",
                newName: "Quantidade");

            migrationBuilder.RenameColumn(
                name: "JaFurouAntes",
                table: "Encomendas",
                newName: "FK_Produto");

            migrationBuilder.RenameColumn(
                name: "Cep",
                table: "Encomendas",
                newName: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_FK_Produto",
                table: "Encomendas",
                column: "FK_Produto");

            migrationBuilder.AddForeignKey(
                name: "FK_Encomendas_Produtos_FK_Produto",
                table: "Encomendas",
                column: "FK_Produto",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
