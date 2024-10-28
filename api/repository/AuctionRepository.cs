using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dto.Auction;
using api.Helpers;
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


        // Create a new auction (requires authorization)
        public async Task<Auction> CreateAuctionAsync(CreateAuctionDto createAuctionDto)
        {
            if (createAuctionDto == null)
            {
                throw new ArgumentNullException(nameof(createAuctionDto), "Auction DTO cannot be null");
            }

            // Check if the seller exists
            if (!await SellerExistsAsync(createAuctionDto.SellerId))
            {
                throw new KeyNotFoundException($"Seller with ID {createAuctionDto.SellerId} does not exist.");
            }

            try
            {
                // Convert to UTC if not already in UTC
                var auction = new Auction
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



        // Get all auctions (no authorization required)
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

        // Helper method to get current time in Sri Lankan time zone
        private DateTime GetSriLankaCurrentTime()
        {
            var sriLankaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sriLankaTimeZone);
        }

        // Helper method to convert UTC to Sri Lankan time
        private DateTime ConvertToSriLankaTime(DateTime utcDateTime)
        {
            var sriLankaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, sriLankaTimeZone);
        }

        // Get 6 upcoming auctions (no authorization required)
        public async Task<IEnumerable<Auction>> GetUpcomingAuctionsAsync()
        {
            var sriLankaCurrentTime = GetSriLankaCurrentTime();

            // Retrieve upcoming auctions and handle time conversion on the client side
            var upcomingAuctions = await _context.Auctions
                .Where(a => a.AuctionLiveDate > sriLankaCurrentTime.Date ||
                           (a.AuctionLiveDate == sriLankaCurrentTime.Date && a.LiveAuctionTime.TimeOfDay > sriLankaCurrentTime.TimeOfDay)) // Only future live auctions
                .OrderBy(a => a.AuctionLiveDate)
                .ThenBy(a => a.LiveAuctionTime)
                .Take(6) // Get the nearest 6 upcoming auctions
                .ToListAsync(); // Perform asynchronous list retrieval

            // Convert UTC to Sri Lanka Time for display purposes
            return upcomingAuctions.Select(a =>
            {
                a.AuctionLiveDate = ConvertToSriLankaTime(a.AuctionLiveDate);
                a.LiveAuctionTime = ConvertToSriLankaTime(a.LiveAuctionTime);
                return a;
            });
        }

        // Get 10 live auctions (no authorization required)
        public async Task<IEnumerable<Auction>> GetLiveAuctionsAsync()
        {
            var sriLankaCurrentTime = GetSriLankaCurrentTime();

            // Retrieve live auctions and handle time conversion on the client side
            var liveAuctions = await _context.Auctions
                .Where(a => (a.AuctionLiveDate < sriLankaCurrentTime.Date ||
                            (a.AuctionLiveDate == sriLankaCurrentTime.Date && a.LiveAuctionTime.TimeOfDay <= sriLankaCurrentTime.TimeOfDay)) && // Auction has started
                            (a.AuctionClosingDate > sriLankaCurrentTime.Date ||
                            (a.AuctionClosingDate == sriLankaCurrentTime.Date && a.AuctionClosingTime.TimeOfDay >= sriLankaCurrentTime.TimeOfDay))) // Auction has not ended
                .OrderBy(a => a.AuctionLiveDate)
                .ThenBy(a => a.LiveAuctionTime)
                .Take(10) // Get the first 10 live auctions
                .ToListAsync(); // Perform asynchronous list retrieval

            // Convert UTC to Sri Lanka Time for display purposes
            return liveAuctions.Select(a =>
            {
                a.AuctionLiveDate = ConvertToSriLankaTime(a.AuctionLiveDate);
                a.LiveAuctionTime = ConvertToSriLankaTime(a.LiveAuctionTime);
                a.AuctionClosingDate = ConvertToSriLankaTime(a.AuctionClosingDate);
                a.AuctionClosingTime = ConvertToSriLankaTime(a.AuctionClosingTime);
                return a;
            });
        }


        // Get auction by ID (no authorization required)
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

        // Get auctions by seller ID (requires authorization)
        public async Task<List<Auction>> GetAuctionsBySellerIdAsync(int sellerId)
        {
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

        // Update auction details (requires authorization)
        public async Task UpdateAuctionAsync(Auction auction)
        {
            if (auction == null)
            {
                throw new ArgumentNullException(nameof(auction), "Auction cannot be null");
            }

            if (!await IsAuctionExistsAsync(auction.AuctionID))
            {
                throw new KeyNotFoundException($"Auction with ID {auction.AuctionID} not found.");
            }

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

        // Check if auction exists (utility method)
        public async Task<bool> IsAuctionExistsAsync(int auctionId)
        {
            return await _context.Auctions.AnyAsync(a => a.AuctionID == auctionId);
        }

        // Check if seller exists (utility method)
        public async Task<bool> SellerExistsAsync(int sellerId)
        {
            return await _context.Sellers.AnyAsync(s => s.SellerId == sellerId);
        }

        // Update the auction's IsActive status (requires authorization)
        public async Task UpdateIsActiveAsync(int auctionId, bool isActive)
        {
            var auction = await _context.Auctions.FindAsync(auctionId);
            if (auction == null)
            {
                throw new KeyNotFoundException($"Auction with ID {auctionId} not found.");
            }

            auction.IsActive = isActive;

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

        // Check if auction is active (utility method, implement as needed)
        public Task<bool> IsAuctionActiveAsync(int auctionId)
        {
            throw new NotImplementedException();
        }
    }
}
