using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IAuctionLotRepository
    {
        Task<AuctionLotItem> AddLotItemAsync(AuctionLotItem lotItem);
        Task<AuctionLotItem> GetLotItemByIdAsync(int lotItemId);
        Task<IEnumerable<AuctionLotItem>> GetLotItemsByAuctionIdAsync(int auctionId);
        Task<AuctionLotItem> UpdateLotItemAsync(AuctionLotItem lotItem);
        Task<bool> DeleteLotItemAsync(int lotItemId);
        Task<bool> DeleteAllLotItemsByAuctionIdAsync(int auctionId);
        Task<bool> AuctionItemExistsAsync(int auctionId, int itemId);

    }
}