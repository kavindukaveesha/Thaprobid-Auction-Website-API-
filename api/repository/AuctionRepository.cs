using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dto.Auction;
using api.Models;
using Interfaces;
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

        public async Task<Auction> CreateAuctionAsync(CreateAuctionDto createAuctionDto)
        {
            if (createAuctionDto == null)
            {
                throw new ArgumentNullException(nameof(createAuctionDto), "Auction DTO cannot be null");
            }

            // Check if the seller exists
            if (!await SellerExistsAsync(createAuctionDto.SellerId)) // Ensure you have SellerId in your DTO
            {
                throw new KeyNotFoundException($"Seller with ID {createAuctionDto.SellerId} does not exist.");
            }

            try
            {
                var auction = new Auction
                {
                    AuctionRegisterId = createAuctionDto.AuctionRegisterId,
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
                    SellerId = createAuctionDto.SellerId
                };

                _context.Auctions.Add(auction);
                await _context.SaveChangesAsync();
                return auction;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while creating the auction.", ex);
            }
        }

        public async Task<List<Auction>> GetAllAuctionsAsync()
        {
            try
            {
                return await _context.Auctions.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving auctions.", ex);
            }
        }

        public async Task<Auction> GetAuctionByIdAsync(int auctionId)
        {
            try
            {
                var auction = await _context.Auctions.FindAsync(auctionId);
                if (auction == null)
                {
                    throw new KeyNotFoundException($"Auction with ID {auctionId} not found.");
                }
                return auction;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving auction with ID {auctionId}.", ex);
            }
        }

        public async Task<List<Auction>> GetAuctionsBySellerIdAsync(int sellerId)
        {
            // Check if the seller exists
            if (!await SellerExistsAsync(sellerId))
            {
                throw new KeyNotFoundException($"Seller with ID {sellerId} does not exist.");
            }

            try
            {
                return await _context.Auctions.Where(a => a.SellerId == sellerId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving auctions for seller with ID {sellerId}.", ex);
            }
        }

        public async Task UpdateAuctionAsync(Auction auction)
        {
            if (auction == null)
            {
                throw new ArgumentNullException(nameof(auction), "Auction cannot be null");
            }

            // Check if the auction exists
            if (!await IsAuctionExistsAsync(auction.AuctionID))
            {
                throw new KeyNotFoundException($"Auction with ID {auction.AuctionID} not found.");
            }

            // Check if the seller exists
            if (!await SellerExistsAsync(auction.SellerId))
            {
                throw new KeyNotFoundException($"Seller with ID {auction.SellerId} does not exist.");
            }

            try
            {
                _context.Entry(auction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception($"An error occurred while updating auction with ID {auction.AuctionID}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the auction.", ex);
            }
        }

        public async Task<bool> IsAuctionExistsAsync(int auctionId)
        {
            return await _context.Auctions.AnyAsync(a => a.AuctionID == auctionId);
        }

        public async Task<bool> SellerExistsAsync(int sellerId)
        {
            return await _context.Sellers.AnyAsync(s => s.SellerId == sellerId);
        }

        public async Task UpdateIsActiveAsync(int auctionId, bool isActive)
        {
            var auction = await _context.Auctions.FindAsync(auctionId);
            if (auction == null)
            {
                throw new KeyNotFoundException($"Auction with ID {auctionId} not found.");
            }

            auction.IsActive = isActive;

            // Update only the IsActive property
            _context.Entry(auction).Property(a => a.IsActive).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception($"An error occurred while updating IsActive for auction with ID {auctionId}: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the auction: {ex.Message}");
            }
        }

        public Task<bool> IsAuctionActiveAsync(int auctionId)
        {
            throw new NotImplementedException();
        }
    }
}
