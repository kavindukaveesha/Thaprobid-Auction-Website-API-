using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Auction;
using api.Models;

namespace api.Mappers
{
    public static class AuctionMapper
    {
        public static Auction ToAuctionDetailsFromCreate(this CreateAuctionDto createAuctionDto, int sellerId)
        {
            return new Auction
            {
                AuctionName = createAuctionDto.AuctionName,
                AuctionTitle = createAuctionDto.AuctionTitle,
                AuctionDescription = createAuctionDto.AuctionDescription,
                AuctionCoverImageUrl = createAuctionDto.AuctionCoverImageUrl,
                VenueAddress = createAuctionDto.VenueAddress,
                Location = createAuctionDto.Location,
                BiddingStartDate = createAuctionDto.BiddingStartDate,
                BiddingStartTime = createAuctionDto.BiddingStartTime,
                AuctionLiveDate = createAuctionDto.AuctionLiveDate,
                LiveAuctionTime = createAuctionDto.LiveAuctionTime,
                AuctionClosingDate = createAuctionDto.AuctionClosingDate,
                AuctionClosingTime = createAuctionDto.AuctionClosingTime,
                TermsAndConditions = createAuctionDto.TermsAndConditions,
                ImportantInformation = createAuctionDto.ImportantInformation,
                SellerId = sellerId
            };
        }

        public static AuctionLotItem ToAuctionLotItemFromCreateDto(this AuctionItemLotCreateDto auctionItemLotCreateDto, int auctionId)
        {
            return new AuctionLotItem
            {
                LotName = auctionItemLotCreateDto.LotName,
                LotDescription = auctionItemLotCreateDto.LotDescription,
                LotImageUrl = auctionItemLotCreateDto.LotImageUrl,
                LotCondition = auctionItemLotCreateDto.LotCondition,
                EstimateBidStartPrice = auctionItemLotCreateDto.EstimateBidStartPrice,
                EstimateBidEndPrice = auctionItemLotCreateDto.EstimateBidEndPrice,
                AdditionalFees = auctionItemLotCreateDto.AdditionalFees,
                ShippingCost = auctionItemLotCreateDto.ShippingCost,
                BidInterval = auctionItemLotCreateDto.BidInterval,
                AuctionId = auctionId,
                FieldId = auctionItemLotCreateDto.FieldId,
                CategoryId = auctionItemLotCreateDto.CategoryId,
                SubCategoryId = auctionItemLotCreateDto.SubCategoryId


            };
        }
    }
}