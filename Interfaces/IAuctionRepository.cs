using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Auction;
using api.Models;

namespace Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction> CreateAuctionAsync(CreateAuctionDto createAuctionDto);
        Task<List<Auction>> GetAllAuctionsAsync();
        Task<IEnumerable<Auction>> GetUpcomingAuctionsAsync();
        Task<IEnumerable<Auction>> GetLiveAuctionsAsync();
        Task<Auction> GetAuctionByIdAsync(int auctionId);
        Task<List<Auction>> GetAuctionsBySellerIdAsync(int sellerId);
        Task UpdateAuctionAsync(Auction auction);
        Task UpdateIsActiveAsync(int auctionId, bool isActive);
        Task<bool> IsAuctionExistsAsync(int auctionId);
        Task<bool> SellerExistsAsync(int sellerId);
        Task<bool> IsAuctionActiveAsync(int auctionId);
    }
}
