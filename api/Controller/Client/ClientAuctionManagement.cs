using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.dto.response;
using api.Dto.Auction;
using api.Interfaces;
using api.Mappers;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Client
{
    [ApiController]
    [Route("api/seller/auction")]
    public class ClientAuctionManagement : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepo;
        private readonly IAuctionLotRepository _auctionLot;
        private readonly IFieldRepository _FieldRepo;
        private readonly ApplicationDBContext _dbContext;


        public ClientAuctionManagement(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public ClientAuctionManagement(IAuctionRepository auctionRepo, IAuctionLotRepository auctionLot, IFieldRepository FieldRepo)
        {
            _auctionRepo = auctionRepo;
            _auctionLot = auctionLot;
            _FieldRepo = FieldRepo;
        }

        //create new auction
        [HttpPost("{sellerId:int}")]
        public async Task<IActionResult> createNewAuction([FromRoute] int sellerId, [FromBody] CreateAuctionDto createAuctionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check  seller id is exist or not
            var auctionModel = createAuctionDto.ToAuctionDetailsFromCreate(sellerId);
            await _auctionRepo.CreateNewAuctionasync(auctionModel);
            return Ok();
        }





        [HttpPost("add-items/{sellerId:int}/{auctionId:int}")]
        public async Task<IActionResult> AddNewItemLot([FromRoute] int sellerId, [FromRoute] int auctionId, [FromBody] AuctionItemLotCreateDto auctionItemLotCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check auction available or not
            if (!await _auctionRepo.IsAuctionExist(auctionId))
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"No Auction found with ID: {auctionId}"));

            }
            if (!await _FieldRepo.IsFieldExist(auctionItemLotCreateDto.FieldId))
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"No Field found with ID: {auctionItemLotCreateDto.FieldId}"));
            }

            var auction = await _auctionRepo.FindAuctionBYId(auctionId);
            var existSellerId = auction.SellerId;

            if (sellerId != existSellerId)
            {
                return BadRequest(new ApiErrorDto(400, "BAD_REQUEST", "Seller ID does not match the auction's seller."));
            }



            //check  seller id is exist or not


            var lotItemModel = auctionItemLotCreateDto.ToAuctionLotItemFromCreateDto(auctionId);
            await _auctionLot.AddnewLotItemAsync(lotItemModel);
            return Ok();
        }








        // get all auction
        [HttpGet]
        public async Task<IActionResult> GetAllClentAuction()
        {
            try
            {
                // fetch all
                var auctions = await _dbContext.Auctions.ToListAsync();

                // map
                var auctionDtos = auctions.Select(auction => new ClientViewAuctionDto
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
                    ImportantInformation = auction.ImportantInformation
                }).ToList();

                return Ok(auctionDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving client auctions.", details = ex.Message });
            }
        }



        // get by ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllClentAuctionByID(int id)
        {
            try
            {
                // fetch auction by id
                var auction = await _dbContext.Auctions.FirstOrDefaultAsync(a => a.AuctionID == id);

                if (auction == null)
                {
                    return NotFound(new { message = $"Auction with ID {id} not found." });
                }


                // map auction dto
                var auctionDto = new ClientViewAuctionDto
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
                    ImportantInformation = auction.ImportantInformation
                };


                return Ok(auctionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the auction.", details = ex.Message });
            }
        }




        // auction delete by id
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteClentAuctionByID(int id)
        {
            try
            {
                // get auction by id
                var auction = await _dbContext.Auctions.FirstOrDefaultAsync(a => a.AuctionID == id);

                if (auction == null)
                {
                    return NotFound(new { message = $"Auction with ID {id} not found." });
                }

                // remove aucton from database
                _dbContext.Auctions.Remove(auction);

                // save the changes
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = $"Auction with ID {id} has been deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the auction.", details = ex.Message });
            }
        }






        // Auction update by ID
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateClentAuctionByID(int id, [FromBody] ClientViewAuctionDto updatedAuction)
        {
            try
            {
                // Fetch the auction by ID
                var auction = await _dbContext.Auctions.FirstOrDefaultAsync(a => a.AuctionID == id);

                // Check if the auction exists
                if (auction == null)
                {
                    return NotFound(new { message = $"Auction with ID {id} not found." });
                }

                // Update the auction fields
                auction.AuctionName = updatedAuction.AuctionName;
                auction.AuctionTitle = updatedAuction.AuctionTitle;
                auction.AuctionDescription = updatedAuction.AuctionDescription;
                auction.AuctionCoverImageUrl = updatedAuction.AuctionCoverImageUrl;
                auction.VenueAddress = updatedAuction.VenueAddress;
                auction.Location = updatedAuction.Location;
                auction.BiddingStartDate = updatedAuction.BiddingStartDate;
                auction.BiddingStartTime = updatedAuction.BiddingStartTime;
                auction.AuctionLiveDate = updatedAuction.AuctionLiveDate;
                auction.LiveAuctionTime = updatedAuction.LiveAuctionTime;
                auction.AuctionClosingDate = updatedAuction.AuctionClosingDate;
                auction.AuctionClosingTime = updatedAuction.AuctionClosingTime;
                auction.IsVerified = updatedAuction.IsVerified;
                auction.IsActive = updatedAuction.IsActive;
                auction.IsClosed = updatedAuction.IsClosed;
                auction.AuctionStatus = updatedAuction.AuctionStatus;
                auction.TermsAndConditions = updatedAuction.TermsAndConditions;
                auction.ImportantInformation = updatedAuction.ImportantInformation;

                // Save the changes to the database
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = $"Auction with ID {id} has been updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the auction.", details = ex.Message });
            }
        }






    }
}