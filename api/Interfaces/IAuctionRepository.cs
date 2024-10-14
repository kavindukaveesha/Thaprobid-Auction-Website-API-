using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction> CreateNewAuctionasync(Auction auctionModel);
        Task<bool> IsAuctionExist(int id);
        Task<Auction> FindAuctionBYId(int id);
    }
}