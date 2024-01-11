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
                FechaRegistro = table.Column<string>(nullable: true),
                Solicitante = table.Column<string>(nullable: true),
                Moneda = table.Column<string>(nullable: true),
                Monto = table.Column<int>(nullable: true),
                Folio = table.Column<string>(nullable: true),
                Estatus = table.Column<string>(nullable: true),
                fechaPago = table.Column<string>(nullable: true),
                nombreProveedor = table.Column<string>(nullable: true),
                Notas = table.Column<string>(nullable: true),
                comentariosSeguimiento = table.Column<string>(nullable: true),
                Complemento = table.Column<string>(nullable: true),
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