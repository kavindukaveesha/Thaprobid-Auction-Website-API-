using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.seller;
using api.Interfaces;
using api.Models;
using api.repository;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller.Client
{
    [Route("api/seller")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerRepository _sellerRepo;
        private readonly IUserService _userRepository;

        public SellerController(ISellerRepository sellerRepo, IUserService userRepository)
        {
            _sellerRepo = sellerRepo;
            _userRepository = userRepository;
        }

        // Create a new seller
        [HttpPost]
        public async Task<IActionResult> CreateSeller([FromBody] CreateSellerDto newSeller)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = 400, message = "Invalid request data" });
            }

            try
            {
                // Check if the user exists
                var user = await _userRepository.GetAppUserByIdAsync(newSeller.UserId);
                if (user == null)
                {
                    return NotFound(new { status = 404, message = $"User with ID {newSeller.UserId} not found" });
                }

                var seller = await _sellerRepo.CreateSellerAsync(newSeller);
                if (seller == null)
                {
                    return StatusCode(500, new { status = 500, message = "Error creating seller" });
                }
                return CreatedAtAction(nameof(GetSellerById), new { id = seller.SellerId }, new { status = 201, message = "Seller created successfully", data = seller });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = $"Error creating seller: {ex.Message}" });
            }
        }

        // Get all sellers
        [HttpGet]
        public async Task<IActionResult> GetAllSellers()
        {
            try
            {
                var sellers = await _sellerRepo.GetAllSellersAsync();
                if (sellers == null || !sellers.Any())
                {
                    return NotFound(new { status = 404, message = "No sellers found" });
                }
                return Ok(new { status = 200, data = sellers });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = $"Error fetching sellers: {ex.Message}" });
            }
        }

        // Get seller by ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSellerById([FromRoute] int id)
        {
            try
            {
                var seller = await _sellerRepo.GetSellerByIdAsync(id);
                if (seller == null)
                {
                    return NotFound(new { status = 404, message = $"Seller with ID {id} not found" });
                }
                return Ok(new { status = 200, data = seller });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = $"Error fetching seller by ID: {ex.Message}" });
            }
        }

        // Get seller by User ID
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetSellerByUserId([FromRoute] int userId)
        {
            try
            {
                // Check if the user exists
                var user = await _userRepository.GetAppUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { status = 404, message = $"User with ID {userId} not found" });
                }

                var seller = await _sellerRepo.GetSellerByUserIdAsync(userId);
                if (seller == null)
                {
                    return NotFound(new { status = 404, message = $"Seller with User ID {userId} not found" });
                }
                return Ok(new { status = 200, data = seller });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = $"Error fetching seller by User ID: {ex.Message}" });
            }
        }

        // Update seller details by User ID and Seller ID
        [HttpPut("{userId:int}")]
        public async Task<IActionResult> UpdateSellerDetails([FromRoute] int userId, [FromBody] UpdateSellerDto updatedSeller)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = 400, message = "Invalid request data" });
            }

            try
            {
                // Check if the user exists
                var user = await _userRepository.GetAppUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { status = 404, message = $"User with ID {userId} not found" });
                }

                var seller = await _sellerRepo.UpdateSellerAsync(userId, updatedSeller);
                if (seller == null)
                {
                    return NotFound(new { status = 404, message = $"Seller with  User ID {userId} not found" });
                }
                return Ok(new { status = 200, message = "Seller updated successfully", data = seller });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = $"Error updating seller: {ex.Message}" });
            }
        }
    }
}