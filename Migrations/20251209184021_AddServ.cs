using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleaningApp.Migrations
{
    /// <inheritdoc />
    public partial class AddServ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaRange",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstimatedTime",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaRange",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "EstimatedTime",
                table: "Services");
        }
    }
}
