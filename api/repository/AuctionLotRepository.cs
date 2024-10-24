using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Interfaces;
using api.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class AuctionLotItemRepository : IAuctionLotRepository
    {
        private readonly ApplicationDBContext _context;

        public AuctionLotItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // Add a new Auction Lot Item
        public async Task<AuctionLotItem> AddLotItemAsync(AuctionLotItem lotItem)
        {
            try
            {
                await _context.AuctionLotItems.AddAsync(lotItem);
                await _context.SaveChangesAsync();
                return lotItem;
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                throw new Exception("Error occurred while adding the auction lot item.", ex);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new Exception("An unexpected error occurred while adding the auction lot item.", ex);
            }
        }

        // Get Auction Lot Item by Id
        public async Task<AuctionLotItem> GetLotItemByIdAsync(int lotItemId)
        {
            try
            {
                var lotItem = await _context.AuctionLotItems
                    .FirstOrDefaultAsync(l => l.AuctionLotItemId == lotItemId);

                if (lotItem == null)
                {
                    throw new KeyNotFoundException($"Auction lot item with id {lotItemId} not found.");
                }

                return lotItem;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new Exception($"Error retrieving the auction lot item with id {lotItemId}.", ex);
            }
        }

        // Get all Auction Lot Items by Auction Id
        public async Task<IEnumerable<AuctionLotItem>> GetLotItemsByAuctionIdAsync(int auctionId)
        {
            try
            {
                var lotItems = await _context.AuctionLotItems
                    .Where(l => l.AuctionId == auctionId)
                    .ToListAsync();

                if (!lotItems.Any())
                {
                    throw new KeyNotFoundException($"No auction lot items found for auction id {auctionId}.");
                }

                return lotItems;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new Exception($"Error retrieving the auction lot items for auction id {auctionId}.", ex);
            }
        }

        // Update an Auction Lot Item
        public async Task<AuctionLotItem> UpdateLotItemAsync(AuctionLotItem lotItem)
        {
            try
            {
                var existingLotItem = await GetLotItemByIdAsync(lotItem.AuctionLotItemId);
                if (existingLotItem == null)
                {
                    throw new KeyNotFoundException($"Auction lot item with id {lotItem.AuctionLotItemId} not found.");
                }

                _context.Entry(existingLotItem).CurrentValues.SetValues(lotItem);
                await _context.SaveChangesAsync();

                return lotItem;
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                throw new Exception("Error occurred while updating the auction lot item.", ex);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new Exception("An unexpected error occurred while updating the auction lot item.", ex);
            }
        }

        // Delete a specific Auction Lot Item
        public async Task<bool> DeleteLotItemAsync(int lotItemId)
        {
            try
            {
                var lotItem = await GetLotItemByIdAsync(lotItemId);
                if (lotItem == null)
                {
                    throw new KeyNotFoundException($"Auction lot item with id {lotItemId} not found.");
                }

                _context.AuctionLotItems.Remove(lotItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                throw new Exception("Error occurred while deleting the auction lot item.", ex);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new Exception($"An unexpected error occurred while deleting the auction lot item with id {lotItemId}.", ex);
            }
        }

        // Delete all Auction Lot Items by Auction Id
        public async Task<bool> DeleteAllLotItemsByAuctionIdAsync(int auctionId)
        {
            try
            {
                var lotItems = await GetLotItemsByAuctionIdAsync(auctionId);
                if (!lotItems.Any())
                {
                    throw new KeyNotFoundException($"No auction lot items found for auction id {auctionId}.");
                }

                _context.AuctionLotItems.RemoveRange(lotItems);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                throw new Exception($"Error occurred while deleting all auction lot items for auction id {auctionId}.", ex);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new Exception($"An unexpected error occurred while deleting all auction lot items for auction id {auctionId}.", ex);
            }
        }

        public async Task<bool> AuctionItemExistsAsync(int auctionId, int itemId)
        {
            return await _context.AuctionLotItems
                .AnyAsync(a => a.AuctionId == auctionId && a.AuctionLotItemId == itemId);
        }

    }
}
