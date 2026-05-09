using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class appoinmentesnotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletionNotes",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionNotes",
                table: "Appointments");
        }
    }
}
