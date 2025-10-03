using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Auction;
using api.Interfaces;
using api.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller.Client
{
    [Route("api/auction/{auctionId}/items")]
    [ApiController]
    public class AuctionLotItemController : ControllerBase
    {
        private readonly IAuctionLotRepository _auctionLotItemRepository;
        private readonly IItemBidderRepository _itemBidderRepository;

        public AuctionLotItemController(IAuctionLotRepository auctionLotItemRepository, IItemBidderRepository itemBidderRepository)
        {
            _auctionLotItemRepository = auctionLotItemRepository;
            _itemBidderRepository = itemBidderRepository;
        }

        // POST: api/auction/{auctionId}/items
        [HttpPost]
        public async Task<IActionResult> CreateAuctionItem(int auctionId, [FromBody] AuctionItemLotCreateDto lotItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lotItem = new AuctionLotItem
            {
                AuctionId = auctionId,
                LotName = lotItemDto.LotName,
                LotDescription = lotItemDto.LotDescription,
                LotImageUrl = lotItemDto.LotImageUrl,
                LotCondition = lotItemDto.LotCondition,
                EstimateBidStartPrice = lotItemDto.EstimateBidStartPrice,
                EstimateBidEndPrice = lotItemDto.EstimateBidEndPrice,
                AdditionalFees = lotItemDto.AdditionalFees,
                ShippingCost = lotItemDto.ShippingCost,
                BidInterval = lotItemDto.BidInterval,
                FieldId = lotItemDto.FieldId,
                CategoryId = lotItemDto.CategoryId,
                SubCategoryId = lotItemDto.SubCategoryId,
                IsBiddingActive = true,


            };

            try
            {
                var createdLotItem = await _auctionLotItemRepository.AddLotItemAsync(lotItem);
                return CreatedAtAction(nameof(GetAuctionItemById), new { auctionId, lotItemId = createdLotItem.AuctionLotItemId }, createdLotItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the auction lot item: {ex.Message}");
            }
        }

        // GET: api/auction/{auctionId}/items
        [HttpGet]
        public async Task<IActionResult> GetAllItemsByAuctionId(int auctionId)
        {
            try
            {
                var items = await _auctionLotItemRepository.GetLotItemsByAuctionIdAsync(auctionId);
                return Ok(items);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving items: {ex.Message}");
            }
        }

        // GET: api/auction/{auctionId}/items/{lotItemId}
        [HttpGet("{lotItemId}")]
        public async Task<IActionResult> GetAuctionItemById(int auctionId, int lotItemId)
        {
            try
            {
                var item = await _auctionLotItemRepository.GetLotItemByIdAsync(lotItemId);
                if (item == null || item.AuctionId != auctionId)
                {
                    return NotFound($"Auction lot item with id {lotItemId} not found for auction id {auctionId}.");
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the item: {ex.Message}");
            }
        }

        // PUT: api/auction/{auctionId}/items/{lotItemId}
        [HttpPut("{lotItemId}")]
        public async Task<IActionResult> UpdateAuctionItem(int auctionId, int lotItemId, [FromBody] AuctionItemLotCreateDto lotItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingItem = await _auctionLotItemRepository.GetLotItemByIdAsync(lotItemId);
                if (existingItem == null || existingItem.AuctionId != auctionId)
                {
                    return NotFound($"Auction lot item with id {lotItemId} not found for auction id {auctionId}.");
                }

                // Update properties
                existingItem.LotName = lotItemDto.LotName;
                existingItem.LotDescription = lotItemDto.LotDescription;
                existingItem.LotImageUrl = lotItemDto.LotImageUrl;
                existingItem.LotCondition = lotItemDto.LotCondition;
                existingItem.EstimateBidStartPrice = lotItemDto.EstimateBidStartPrice;
                existingItem.EstimateBidEndPrice = lotItemDto.EstimateBidEndPrice;
                existingItem.AdditionalFees = lotItemDto.AdditionalFees;
                existingItem.ShippingCost = lotItemDto.ShippingCost;
                existingItem.BidInterval = lotItemDto.BidInterval;
                existingItem.FieldId = lotItemDto.FieldId;
                existingItem.CategoryId = lotItemDto.CategoryId;
                existingItem.SubCategoryId = lotItemDto.SubCategoryId;

                var updatedItem = await _auctionLotItemRepository.UpdateLotItemAsync(existingItem);
                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the auction lot item: {ex.Message}");
            }
        }

        // DELETE: api/auction/{auctionId}/items/{lotItemId}
        [HttpDelete("{lotItemId}")]
        public async Task<IActionResult> DeleteAuctionItem(int auctionId, int lotItemId)
        {
            try
            {
                var existingItem = await _auctionLotItemRepository.GetLotItemByIdAsync(lotItemId);
                if (existingItem == null || existingItem.AuctionId != auctionId)
                {
                    return NotFound($"Auction lot item with id {lotItemId} not found for auction id {auctionId}.");
                }

                var result = await _auctionLotItemRepository.DeleteLotItemAsync(lotItemId);
                if (!result)
                {
                    return StatusCode(500, "An error occurred while deleting the auction lot item.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the auction lot item: {ex.Message}");
            }
        }

        // DELETE: api/auction/{auctionId}/items
        [HttpDelete]
        public async Task<IActionResult> DeleteAllAuctionItems(int auctionId)
        {
            try
            {
                var result = await _auctionLotItemRepository.DeleteAllLotItemsByAuctionIdAsync(auctionId);
                if (!result)
                {
                    return NotFound($"No auction lot items found for auction id {auctionId}.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting all auction lot items: {ex.Message}");
            }
        }


        // Finalize item when bidding time ends
        [HttpPost("{lotItemId}/finalize")]
        public async Task<IActionResult> FinalizeAuctionItem(int auctionId, int lotItemId)
        {
            var auctionItem = await _auctionLotItemRepository.GetLotItemByIdAsync(lotItemId);
            if (auctionItem == null || !auctionItem.IsBiddingActive)
            {
                return BadRequest("Bidding is not active for this item.");
            }

            var lastBid = await _itemBidderRepository.GetLastBidForItemAsync(auctionId, lotItemId);
            if (lastBid != null)
            {
                auctionItem.WinningBidderId = lastBid.UserId;
                auctionItem.IsSold = true;
                auctionItem.IsBiddingActive = false;

                await _auctionLotItemRepository.UpdateLotItemAsync(auctionItem);
                return Ok($"Item {lotItemId} sold to user {lastBid.UserId}.");
            }
            else
            {
                auctionItem.IsSold = false;
                auctionItem.IsBiddingActive = false;
                await _auctionLotItemRepository.UpdateLotItemAsync(auctionItem);
                return Ok($"Item {lotItemId} did not receive any bids.");
            }
        }

    }
}
