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
                idProveedor = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                OrdenCompra = table.Column<int>(nullable: false),
                statusComplemento = table.Column<string>(nullable: true),
                Folio = table.Column<string>(nullable: true),
                Monto = table.Column<int>(nullable: false),
                Notas = table.Column<string>(nullable: true),
                Estatus = table.Column<string>(nullable: true),
                UUIDF = table.Column<string>(nullable: true),
                PdfUrl = table.Column<string>(nullable: true),
                XmlUrl = table.Column<string>(nullable: true),
                metodoPago = table.Column<string>(nullable: true),
                Solicitante = table.Column<string>(nullable: true),
                comentariosSeguimiento = table.Column<string>(nullable: true),
                nombreProveedor = table.Column<string>(nullable: true),
                Moneda = table.Column<string>(nullable: true),
                FechaRegistro = table.Column<DateTime>(nullable: true),
                fechaPago = table.Column<DateTime>(nullable: true),
                fechaProximaPago = table.Column<DateTime>(nullable: true),
                fechaFactura = table.Column<DateTime>(nullable: true),
                idComplementoFK = table.Column<int>(nullable: false),
                idUsuarioFK = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Proveedor", x => x.idProveedor);
                table.ForeignKey(
                    name: "FK_Proveedores_Complementos_idComplementoFK",
                    column: x => x.idComplementoFK,
                    principalTable: "Complemento",
                    principalColumn: "idComplemento",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Proveedores_Usuarios_idUsuarioFK",
                    column: x => x.idUsuarioFK,
                    principalTable: "Usuario",
                    principalColumn: "idUsuario",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_idComplementoFK",
                table: "Proveedor",
                column: "idComplementoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_idUsuarioFK",
                table: "Proveedor",
                column: "idUsuarioFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Proveedor");
        }
    }
}