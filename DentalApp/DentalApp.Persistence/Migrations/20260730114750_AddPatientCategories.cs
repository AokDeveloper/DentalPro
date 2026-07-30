using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientCategory_PatientCategory_ParentId",
                table: "PatientCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCategoryAssignments_PatientCategory_PatientCategoryId",
                table: "PatientCategoryAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientCategory",
                table: "PatientCategory");

            migrationBuilder.RenameTable(
                name: "PatientCategory",
                newName: "PatientCategories");

            migrationBuilder.RenameIndex(
                name: "IX_PatientCategory_ParentId",
                table: "PatientCategories",
                newName: "IX_PatientCategories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientCategories",
                table: "PatientCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCategories_PatientCategories_ParentId",
                table: "PatientCategories",
                column: "ParentId",
                principalTable: "PatientCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCategoryAssignments_PatientCategories_PatientCategoryId",
                table: "PatientCategoryAssignments",
                column: "PatientCategoryId",
                principalTable: "PatientCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientCategories_PatientCategories_ParentId",
                table: "PatientCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCategoryAssignments_PatientCategories_PatientCategoryId",
                table: "PatientCategoryAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientCategories",
                table: "PatientCategories");

            migrationBuilder.RenameTable(
                name: "PatientCategories",
                newName: "PatientCategory");

            migrationBuilder.RenameIndex(
                name: "IX_PatientCategories_ParentId",
                table: "PatientCategory",
                newName: "IX_PatientCategory_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientCategory",
                table: "PatientCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCategory_PatientCategory_ParentId",
                table: "PatientCategory",
                column: "ParentId",
                principalTable: "PatientCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCategoryAssignments_PatientCategory_PatientCategoryId",
                table: "PatientCategoryAssignments",
                column: "PatientCategoryId",
                principalTable: "PatientCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
