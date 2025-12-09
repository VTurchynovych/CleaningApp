using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleaningApp.Migrations
{
    /// <inheritdoc />
    public partial class AddClientNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientNote",
                table: "Orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientNote",
                table: "Orders");
        }
    }
}
