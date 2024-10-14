using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Handlers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.repository
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly ApplicationDBContext _context;

        public AuctionRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Auction> CreateNewAuctionasync(Auction auctionModel)
        {
            if (auctionModel == null)
            {
                throw new ArgumentNullException(nameof(auctionModel));
            }
            // Check if an auction with the same AuctionRegisterId already exists
            if (await _context.Auctions.AnyAsync(a => a.AuctionRegisterId == auctionModel.AuctionRegisterId))
            {
                throw new BadRequestException($"An auction with Auction Register ID {auctionModel.AuctionRegisterId} already exists.");
            }
            try
            {
                await _context.Auctions.AddAsync(auctionModel);
                await _context.SaveChangesAsync();
                return auctionModel;

            }
            catch (Exception ex)
            {
                throw new BadRequestException("An error occurred while saving a comment. Please try again later.", ex);
            }
        }

        public async Task<Auction> FindAuctionBYId(int id)
        {
            var auction = await _context.Auctions.FirstOrDefaultAsync(a => a.AuctionID == id);
            return auction;
        }

        public async Task<bool> IsAuctionExist(int id)
        {
            return await _context.Auctions.AnyAsync(i => i.AuctionID == id);
        }
    }
}