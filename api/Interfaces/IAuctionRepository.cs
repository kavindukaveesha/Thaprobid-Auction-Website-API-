using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Auction;
using api.Models;
using Models;

namespace Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction> CreateAuctionAsync(CreateAuctionDto auctionDto);
        Task<List<Auction>> GetAllAuctionsAsync();
        Task<Auction> GetAuctionByIdAsync(int auctionId);
        Task<List<Auction>> GetAuctionsBySellerIdAsync(int sellerId);
        Task<bool> IsAuctionExistsAsync(int auctionId);
        Task<bool> SellerExistsAsync(int sellerId);
        Task<bool> IsAuctionActiveAsync(int auctionId);
        Task UpdateIsActiveAsync(int auctionId, bool isActive);
    }
}