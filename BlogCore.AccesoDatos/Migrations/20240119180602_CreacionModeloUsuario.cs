using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionModeloUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
           name: "Usuario",
           columns: table => new
           {
               idUsuario = table.Column<int>(nullable: false)
                   .Annotation("SqlServer:Identity", "1, 1"),
               Correo = table.Column<string>(nullable: true),
               Rfc = table.Column<string>(nullable: true),
               idRolFK = table.Column<int>(nullable: false)
           },
           constraints: table =>
           {
               table.PrimaryKey("PK_Usuario", x => x.idUsuario);
               table.ForeignKey(
                   name: "FK_Usuarios_rolUsuarios_idRolFK",
                   column: x => x.idRolFK,
                   principalTable: "rolUsuario",
                   principalColumn: "idRol",
                   onDelete: ReferentialAction.Cascade);
           });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                                   name: "Usuario");

        }
    }
}
