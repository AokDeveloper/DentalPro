using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migfoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "TreatmentImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "RecordDate",
                table: "TreatmentImages",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "TreatmentImages");

            migrationBuilder.DropColumn(
                name: "RecordDate",
                table: "TreatmentImages");
        }
    }
}
