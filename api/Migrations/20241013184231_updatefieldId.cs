using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updatefieldId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Fields_fieldId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "fieldId",
                table: "Categories",
                newName: "FieldId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_fieldId",
                table: "Categories",
                newName: "IX_Categories_FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Fields_FieldId",
                table: "Categories",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "FieldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Fields_FieldId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "FieldId",
                table: "Categories",
                newName: "fieldId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_FieldId",
                table: "Categories",
                newName: "IX_Categories_fieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Fields_fieldId",
                table: "Categories",
                column: "fieldId",
                principalTable: "Fields",
                principalColumn: "FieldId");
        }
    }
}
