using api.data;
using api.Dto.Auction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/admin/manage-auction")]
    public class ManageAuctionController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;

        public ManageAuctionController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        // Get all auction
        [HttpGet]
        public async Task<IActionResult> GetAllAuction()
        {
            try
            {
                // Fetch all auction from database
                var auctions = await _dbContext.Auctions.ToListAsync();

                // map the auction
                var auctionDtos = auctions.Select(auction => new AdminViewAuctionDto
                {
                    AuctionID = auction.AuctionID,
                    AuctionName = auction.AuctionName,
                    AuctionTitle = auction.AuctionTitle,
                    AuctionDescription = auction.AuctionDescription,
                    AuctionCoverImageUrl = auction.AuctionCoverImageUrl,
                    VenueAddress = auction.VenueAddress,
                    Location = auction.Location,
                    BiddingStartDate = auction.BiddingStartDate,
                    BiddingStartTime = auction.BiddingStartTime,
                    AuctionLiveDate = auction.AuctionLiveDate,
                    LiveAuctionTime = auction.LiveAuctionTime,
                    AuctionClosingDate = auction.AuctionClosingDate,
                    AuctionClosingTime = auction.AuctionClosingTime,
                    IsVerified = auction.IsVerified,
                    IsActive = auction.IsActive,
                    IsClosed = auction.IsClosed,
                    AuctionStatus = auction.AuctionStatus,
                    TermsAndConditions = auction.TermsAndConditions,
                    ImportantInformation = auction.ImportantInformation,
                    SellerId = auction.SellerId

                }).ToList();

                return Ok(auctionDtos);
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new { message = "An error occurred while retrieving auctions.", Details = ex.Message });
                }
            }
        }





        // Get auction by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            try
            {
                // Fetch auction by ID
                var auction = await _dbContext.Auctions.FirstOrDefaultAsync(a => a.AuctionID == id);

                // If auction not found, return 404
                if (auction == null)
                {
                    return NotFound(new { Message = $"Auction with ID {id} not found." });
                }

                // Map auction to AdminViewAuctionDto
                var auctionDto = new AdminViewAuctionDto
                {
                    AuctionID = auction.AuctionID,
                    AuctionName = auction.AuctionName,
                    AuctionTitle = auction.AuctionTitle,
                    AuctionDescription = auction.AuctionDescription,
                    AuctionCoverImageUrl = auction.AuctionCoverImageUrl,
                    VenueAddress = auction.VenueAddress,
                    Location = auction.Location,
                    BiddingStartDate = auction.BiddingStartDate,
                    BiddingStartTime = auction.BiddingStartTime,
                    AuctionLiveDate = auction.AuctionLiveDate,
                    LiveAuctionTime = auction.LiveAuctionTime,
                    AuctionClosingDate = auction.AuctionClosingDate,
                    AuctionClosingTime = auction.AuctionClosingTime,
                    IsVerified = auction.IsVerified,
                    IsActive = auction.IsActive,
                    IsClosed = auction.IsClosed,
                    AuctionStatus = auction.AuctionStatus,
                    TermsAndConditions = auction.TermsAndConditions,
                    ImportantInformation = auction.ImportantInformation,
                    SellerId = auction.SellerId
                };

                // Return the auction DTO
                return Ok(auctionDto);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with exception details
                return StatusCode(500, new { Message = "An error occurred while fetching the auction.", Details = ex.Message });
            }
        }



       
        // Delete auction by ID
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAuctionById(int id)
        {
            try
            {
                // Get auction by ID
                var auction = await _dbContext.Auctions.FirstOrDefaultAsync(a => a.AuctionID == id);

                if (auction == null)
                {
                    return NotFound(new { message = $"Auction with ID {id} not found." });
                }

                // Remove auction from the database
                _dbContext.Auctions.Remove(auction);

                // Save the changes
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = $"Auction with ID {id} has been deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the auction.", details = ex.Message });
            }
        }

    }

}