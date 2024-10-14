using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class uaddauctiondetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    AuctionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionRegisterId = table.Column<int>(type: "int", nullable: false),
                    AuctionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuctionTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuctionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuctionCoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VenueAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BiddingStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BiddingStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuctionLiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LiveAuctionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuctionClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuctionClosingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportantInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    AuctionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.AuctionID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auctions");
        }
    }
}
