using System;
using System.Threading.Tasks;
using api.Dto.Auction;
using api.Interfaces;
using api.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller.Client
{
    [Route("api/auction/{auctionId}/items/{itemId}/bids")]
    [ApiController]
    public class ItemBiddedController : ControllerBase
    {
        private readonly IItemBidderRepository _itemBiddedRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IAuctionLotRepository _auctionLotItemRepository;
        private readonly IUserProfileService _userProfileService;

        public ItemBiddedController(
            IItemBidderRepository itemBiddedRepository,
            IAuctionRepository auctionRepository,
            IAuctionLotRepository auctionLotItemRepository,
            IUserProfileService userProfileService)
        {
            _itemBiddedRepository = itemBiddedRepository;
            _auctionRepository = auctionRepository;
            _auctionLotItemRepository = auctionLotItemRepository;
            _userProfileService = userProfileService;
        }

        /// <summary>
        /// Places a bid for a specific auction item.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <param name="itemId">The ID of the auction item.</param>
        /// <param name="bid">The bid details submitted by the user.</param>
        /// <returns>The placed bid or an appropriate error response.</returns>
        [HttpPost]
        public async Task<IActionResult> BidForItem([FromRoute] int auctionId, [FromRoute] int itemId, [FromBody] ItemBiddedDto bid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if auction exists
            var auctionExists = await _auctionRepository.IsAuctionExistsAsync(auctionId);
            if (!auctionExists)
            {
                return NotFound($"Auction with id {auctionId} not found.");
            }

            // Check if the item exists in the auction
            var auctionItem = await _auctionLotItemRepository.GetLotItemByIdAsync(itemId);
            if (auctionItem == null)
            {
                return NotFound($"Auction item with id {itemId} not found in auction {auctionId}.");
            }

            // Check if the user exists and if they are allowed to bid (IsBidder == true)
            var user = await _userProfileService.GetProfileAsync(bid.UserId);
            if (user == null)
            {
                return NotFound($"User with id {bid.UserId} not found.");
            }
            if (!user.IsClientBidder)
            {
                return BadRequest($"User with id {bid.UserId} is not allowed to bid.");
            }

            // Check for the last bid
            var lastBid = await _itemBiddedRepository.GetLastBidForItemAsync(auctionId, itemId);
            decimal minimumBidAmount;

            if (lastBid == null)
            {
                // No bids yet, use the starting price and bid interval
                minimumBidAmount = auctionItem.EstimateBidStartPrice + auctionItem.BidInterval;
            }
            else
            {
                // Bids exist, use the last bid amount and bid interval
                minimumBidAmount = lastBid.Amount + auctionItem.BidInterval;
            }

            // Validate the bid amount
            if (bid.Amount < minimumBidAmount)
            {
                return BadRequest($"The bid amount must be at least {minimumBidAmount}.");
            }

            // Create a new ItemBidded object from the DTO
            var newBid = new ItemBidded
            {
                AuctionId = auctionId,
                ItemId = itemId,
                UserId = bid.UserId,
                Amount = bid.Amount,
                DateTime = DateTime.UtcNow // Set bid time to the current UTC time
            };

            try
            {
                // Place the bid by calling the repository
                var placedBid = await _itemBiddedRepository.BidForItemAsync(newBid);
                return Ok(placedBid);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while placing the bid: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves all bids placed on a specific auction item.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <param name="itemId">The ID of the auction item.</param>
        /// <returns>A list of all bidders for the auction item.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBiddersForItem(int auctionId, int itemId)
        {
            try
            {
                var bidders = await _itemBiddedRepository.GetAllBiddersForItemAsync(auctionId, itemId);
                return Ok(bidders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving bidders: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the most recent bid placed on a specific auction item.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <param name="itemId">The ID of the auction item.</param>
        /// <returns>The last bid placed on the auction item.</returns>
        [HttpGet("last")]
        public async Task<IActionResult> GetLastBidForItem(int auctionId, int itemId)
        {
            try
            {
                var lastBid = await _itemBiddedRepository.GetLastBidForItemAsync(auctionId, itemId);
                if (lastBid == null)
                {
                    return NotFound("No bids found for this item.");
                }

                return Ok(lastBid);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the last bid: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the highest bid placed on a specific auction item.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <param name="itemId">The ID of the auction item.</param>
        /// <returns>The highest bid placed on the auction item.</returns>
        [HttpGet("highest")]
        public async Task<IActionResult> GetHighestBidForItem(int auctionId, int itemId)
        {
            try
            {
                var highestBid = await _itemBiddedRepository.GetHighestBidForItemAsync(auctionId, itemId);
                if (highestBid == null)
                {
                    return NotFound("No bids found for this item.");
                }

                return Ok(highestBid);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the highest bid: {ex.Message}");
            }
        }
    }
}
