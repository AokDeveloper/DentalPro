using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class hastakategori : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientCategory_PatientCategory_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PatientCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientCategoryAssignments",
                columns: table => new
                {
                    PatientCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCategoryAssignments", x => new { x.PatientCategoryId, x.PatientId });
                    table.ForeignKey(
                        name: "FK_PatientCategoryAssignments_PatientCategory_PatientCategoryId",
                        column: x => x.PatientCategoryId,
                        principalTable: "PatientCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientCategoryAssignments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientCategory_ParentId",
                table: "PatientCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientCategoryAssignments_PatientId",
                table: "PatientCategoryAssignments",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientCategoryAssignments");

            migrationBuilder.DropTable(
                name: "PatientCategory");
        }
    }
}
