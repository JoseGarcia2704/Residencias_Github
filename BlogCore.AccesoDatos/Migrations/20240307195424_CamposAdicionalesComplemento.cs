using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class CamposAdicionalesComplemento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rfc",
                table: "Complemento",
                type: "nvarchar(max)",
                nullable: true);
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rfc",
                table: "Complemento");

            
        }
    }
}
