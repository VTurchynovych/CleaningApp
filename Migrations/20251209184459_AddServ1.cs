using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleaningApp.Migrations
{
    /// <inheritdoc />
    public partial class AddServ1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Includes",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Includes",
                table: "Services");
        }
    }
}
