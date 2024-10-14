using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class upgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionLotItems_Auctions_AuctionId",
                table: "AuctionLotItems");

            migrationBuilder.DropIndex(
                name: "IX_AuctionLotItems_AuctionId",
                table: "AuctionLotItems");

            migrationBuilder.RenameColumn(
                name: "AuctionLotId",
                table: "AuctionLotItems",
                newName: "AuctionLotItemId");

            migrationBuilder.AlterColumn<string>(
                name: "LotName",
                table: "AuctionLotItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "LotDescription",
                table: "AuctionLotItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "LotCondition",
                table: "AuctionLotItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSold",
                table: "AuctionLotItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuctionLotItemId",
                table: "AuctionLotItems",
                newName: "AuctionLotId");

            migrationBuilder.AlterColumn<string>(
                name: "LotName",
                table: "AuctionLotItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LotDescription",
                table: "AuctionLotItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LotCondition",
                table: "AuctionLotItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "IsSold",
                table: "AuctionLotItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionLotItems_AuctionId",
                table: "AuctionLotItems",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionLotItems_Auctions_AuctionId",
                table: "AuctionLotItems",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "AuctionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
