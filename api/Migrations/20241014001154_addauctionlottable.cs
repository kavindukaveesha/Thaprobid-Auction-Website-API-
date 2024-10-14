using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class addauctionlottable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuctionLotItems",
                columns: table => new
                {
                    AuctionLotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    LotName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LotDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LotImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LotCondition = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EstimateBidStartPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimateBidEndPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdditionalFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BidInterval = table.Column<int>(type: "int", nullable: false),
                    IsSold = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinningBidderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionLotItems", x => x.AuctionLotId);
                    table.ForeignKey(
                        name: "FK_AuctionLotItems_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "AuctionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionLotItems_AuctionId",
                table: "AuctionLotItems",
                column: "AuctionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuctionLotItems");
        }
    }
}
