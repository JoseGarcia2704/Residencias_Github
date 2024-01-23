using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionModeloComplemento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                                  name: "Complemento",
                                  columns: table => new
                                  {
                                      idComplemento = table.Column<int>(nullable: false)
                                          .Annotation("SqlServer:Identity", "1, 1"),
                                      UUIDC = table.Column<string>(nullable: true),
                                      Monto = table.Column<float>(nullable: false),
                                      saldoInsoluto = table.Column<float>(nullable: false)
                                  },
                                  constraints: table =>
                                  {
                                      table.PrimaryKey("PK_Complemento", x => x.idComplemento);
                                  });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
          name: "Complemento");

        }
    }
}
