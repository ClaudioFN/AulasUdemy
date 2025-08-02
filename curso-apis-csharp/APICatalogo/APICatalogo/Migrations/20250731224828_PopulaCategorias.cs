using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Categorias(Nome, ImagemUrl) Values('Bebidas', 'bebidas.jgp')");
            migrationBuilder.Sql("insert into Categorias(Nome, ImagemUrl) Values('Lanches', 'lanches.jgp')");
            migrationBuilder.Sql("insert into Categorias(Nome, ImagemUrl) Values('Sobremesas', 'sobremesas.jgp')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categorias");
        }
    }
}
