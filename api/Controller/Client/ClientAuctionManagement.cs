using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Auction;
using api.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/auction")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionController(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        // POST: api/auction/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionDto createAuctionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var auction = await _auctionRepository.CreateAuctionAsync(createAuctionDto);
                return CreatedAtAction(nameof(GetAuctionById), new { auctionId = auction.AuctionID }, auction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/auction/seller/{sellerId}
        [HttpGet("seller/{sellerId}")]
        public async Task<IActionResult> GetAuctionsBySellerId(int sellerId)
        {
            try
            {
                var auctions = await _auctionRepository.GetAuctionsBySellerIdAsync(sellerId);
                return Ok(auctions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/auction/{auctionId}
        [HttpGet("{auctionId}")]
        public async Task<IActionResult> GetAuctionById(int auctionId)
        {
            try
            {
                var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);
                return Ok(auction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("start-auction/{auctionId}")]
        public async Task<IActionResult> StartAuctionAutomatically(int auctionId)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);
            if (auction == null)
            {
                return NotFound("Auction not found.");
            }

            if (auction.BiddingStartDate <= DateTime.UtcNow && !auction.IsActive)
            {
                // Update the auction to be active
                await _auctionRepository.UpdateIsActiveAsync(auctionId, true);

                return Ok($"Auction with ID {auctionId} is now live.");
            }

            return BadRequest("Auction cannot be started at this time.");
        }

    }
}
