using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class patientnotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientNotes",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientNotes",
                table: "Patients");
        }
    }
}
