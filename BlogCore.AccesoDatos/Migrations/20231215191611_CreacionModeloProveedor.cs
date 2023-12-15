using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionModeloProveedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "Proveedor",
            columns: table => new
            {
                OrdenCompra = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FechaRegistro = table.Column<string>(nullable: false),
                Solicitante = table.Column<string>(nullable: false),
                Moneda = table.Column<string>(nullable: false),
                Monto = table.Column<int>(nullable: false),
                Folio = table.Column<string>(nullable: false),
                Estatus = table.Column<string>(nullable: false),
                fechaPago = table.Column<string>(nullable: false),
                nombreProveedor = table.Column<string>(nullable: false),
                Notas = table.Column<string>(nullable: true),
                comentariosSeguimiento = table.Column<string>(nullable: true),
                Complemento = table.Column<string>(nullable: false),
                PdfUrl = table.Column<string>(nullable: true),
                XmlUrl = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Proveedor", x => x.OrdenCompra);
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                        name: "Proveedor");
        }
    }
}
