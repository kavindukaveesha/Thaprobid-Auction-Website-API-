using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Auction;
using api.Models;

namespace api.Interfaces
{
    public interface IItemBidderRepository
    {
        Task<ItemBidded> BidForItemAsync(ItemBidded itemBiddedDto);
        Task<IEnumerable<ItemBidded>> GetAllBiddersForItemAsync(int auctionId, int itemId);
        Task<ItemBidded> GetLastBidForItemAsync(int auctionId, int itemId);
        Task<ItemBidded> GetHighestBidForItemAsync(int auctionId, int itemId);
        Task<ItemBidded> GetUserBidForItemAsync(int auctionId, int itemId, int userId);
        Task UpdateUserBidAsync(ItemBidded bid);

    }
}