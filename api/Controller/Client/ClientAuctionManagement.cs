using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.dto.response;
using api.Dto.Auction;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller.Client
{
    [ApiController]
    [Route("api/seller/auction")]
    public class ClientAuctionManagement : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepo;
        private readonly IAuctionLotRepository _auctionLot;
        public ClientAuctionManagement(IAuctionRepository auctionRepo, IAuctionLotRepository auctionLot)
        {
            _auctionRepo = auctionRepo;
            _auctionLot = auctionLot;
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
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"No stock found with ID: {auctionId}"));

            }
            var auction = await _auctionRepo.FindAuctionBYId(auctionId);
            var existSellerId = auction.SellerId;

            if (sellerId != existSellerId)
            {
                return BadRequest(new ApiErrorDto(400, "BAD_REQUEST", "Seller ID does not match the auction's seller."));
            }



            //check  seller id is exist or not


            var lotItemModel = auctionItemLotCreateDto.ToAuctionLotItemFromCreateDto(auctionId);
            await _auctionLot.AddnewItemAsync(lotItemModel);
            return Ok();



        }



    }
}