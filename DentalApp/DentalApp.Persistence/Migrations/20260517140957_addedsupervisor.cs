using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedsupervisor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_SupervisorId",
                table: "Patients",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Supervisors_SupervisorId",
                table: "Patients",
                column: "SupervisorId",
                principalTable: "Supervisors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Supervisors_SupervisorId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "Supervisors");

            migrationBuilder.DropIndex(
                name: "IX_Patients_SupervisorId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Patients");
        }
    }
}
