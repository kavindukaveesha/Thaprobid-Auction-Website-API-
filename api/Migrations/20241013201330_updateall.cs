using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updateall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId1",
                table: "SubCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_SubCategoryId1",
                table: "SubCategories",
                column: "SubCategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Categories_CategoryId",
                table: "SubCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_SubCategories_SubCategoryId1",
                table: "SubCategories",
                column: "SubCategoryId1",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Categories_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_SubCategories_SubCategoryId1",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_SubCategoryId1",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "SubCategoryId1",
                table: "SubCategories");
        }
    }
}
