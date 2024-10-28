using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Auction;
using api.Models;
using Interfaces;
using Microsoft.AspNetCore.Authorization;  // Added for authorization
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
        // Creates a new auction (authorization required)
        [HttpPost("create")]
        [Authorize]  // Authorization required
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
        // Fetch auctions by seller ID (authorization required)
        [HttpGet("seller/{sellerId}")]
        [Authorize]  // Authorization required
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
        // Fetch auction by auction ID (no authorization required)
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

        // GET: api/auction/live
        // Fetch top 10 live auctions (no authorization required)
        [HttpGet("live")]
        public async Task<IActionResult> GetLiveAuctions()
        {
            try
            {
                var liveAuctions = await _auctionRepository.GetLiveAuctionsAsync();
                return Ok(liveAuctions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/auction/upcoming
        // Fetch the next 6 upcoming auctions (no authorization required)
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingAuctions()
        {
            try
            {
                var upcomingAuctions = await _auctionRepository.GetUpcomingAuctionsAsync();
                return Ok(upcomingAuctions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/auction/all
        // Fetch all auctions (no authorization required)
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAuctions()
        {
            try
            {
                var auctions = await _auctionRepository.GetAllAuctionsAsync();
                return Ok(auctions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/auction/start-auction/{auctionId}
        // Start an auction automatically (authorization required)
        [HttpPost("start-auction/{auctionId}")]
        [Authorize]  // Authorization required
        public async Task<IActionResult> StartAuctionAutomatically(int auctionId)
        {
            try
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/auction/update/{auctionId}
        // Update an existing auction (authorization required)
        [HttpPut("update/{auctionId}")]
        [Authorize]  // Authorization required
        public async Task<IActionResult> UpdateAuction(int auctionId, [FromBody] Auction auctionToUpdate)
        {
            if (auctionId != auctionToUpdate.AuctionID)
            {
                return BadRequest("Auction ID mismatch.");
            }

            try
            {
                await _auctionRepository.UpdateAuctionAsync(auctionToUpdate);
                return NoContent();
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

        // DELETE: api/auction/delete/{auctionId}
        // Delete an auction (authorization required)
        // [HttpDelete("delete/{auctionId}")]
        // [Authorize]  // Authorization required
        // public async Task<IActionResult> DeleteAuction(int auctionId)
        // {
        //     try
        //     {
        //         var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);
        //         if (auction == null)
        //         {
        //             return NotFound("Auction not found.");
        //         }

        //         await _auctionRepository.DeleteAuctionAsync(auctionId);
        //         return NoContent();  // Return 204 No Content when the delete is successful
        //     }
        //     catch (KeyNotFoundException ex)
        //     {
        //         return NotFound(ex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, ex.Message);
        //     }
        // }

        // POST: api/auction/activate/{auctionId}
        // Manually activate or deactivate an auction (authorization required)
        [HttpPost("activate/{auctionId}")]
        [Authorize]  // Authorization required
        public async Task<IActionResult> ActivateAuction(int auctionId, [FromBody] bool isActive)
        {
            try
            {
                await _auctionRepository.UpdateIsActiveAsync(auctionId, isActive);
                return Ok($"Auction with ID {auctionId} has been updated to {(isActive ? "active" : "inactive")}.");
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
    }
}
