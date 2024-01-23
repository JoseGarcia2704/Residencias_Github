using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionModeloRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                      name: "rolUsuario",
                      columns: table => new
                      {
                          idRol = table.Column<int>(nullable: false)
                              .Annotation("SqlServer:Identity", "1, 1"),
                          Nombre = table.Column<string>(nullable: true)
                      },
                      constraints: table =>
                      {
                          table.PrimaryKey("PK_rolUsuario", x => x.idRol);
                      });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                      name: "rolUsuarios");

        }
    }
}
