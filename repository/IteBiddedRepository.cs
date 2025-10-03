using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.repository
{
    public class ItemBiddedRepository : IItemBidderRepository
    {
        private readonly ApplicationDBContext _context;

        public ItemBiddedRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Places a new bid for an item.
        /// </summary>
        /// <param name="itemBidded">The bid to be placed.</param>
        /// <returns>The placed bid.</returns>
        public async Task<ItemBidded> BidForItemAsync(ItemBidded itemBidded)
        {
            try
            {
                await _context.ItemBiddeds.AddAsync(itemBidded);
                await _context.SaveChangesAsync();
                return itemBidded;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error occurred while saving the bid to the database. Please try again later.", dbEx);
            }
            catch (ArgumentNullException argEx)
            {
                throw new Exception("Null argument encountered while placing the bid. Please check your input.", argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while placing the bid. Please contact support.", ex);
            }
        }

        /// <summary>
        /// Gets the existing bid for a user on a specific auction item.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <param name="itemId">The ID of the item.</param>
        /// <param name="userId">The ID of the user who placed the bid.</param>
        /// <returns>The user's bid for the specific auction item, or null if no bid exists.</returns>
        public async Task<ItemBidded> GetUserBidForItemAsync(int auctionId, int itemId, int userId)
        {
            return await _context.ItemBiddeds
                .FirstOrDefaultAsync(b => b.AuctionId == auctionId && b.ItemId == itemId && b.UserId == userId);
        }

        /// <summary>
        /// Updates an existing user's bid for an auction item.
        /// </summary>
        /// <param name="bid">The updated bid.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task UpdateUserBidAsync(ItemBidded bid)
        {
            try
            {
                _context.ItemBiddeds.Update(bid);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error occurred while updating the bid in the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the bid. Please contact support.", ex);
            }
        }

        /// <summary>
        /// Retrieves all bidders for a specific auction item, ordered by bid time.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <param name="itemId">The ID of the item.</param>
        /// <returns>A list of all bids placed on the item.</returns>
        public async Task<IEnumerable<ItemBidded>> GetAllBiddersForItemAsync(int auctionId, int itemId)
        {
            try
            {
                var bidders = await _context.ItemBiddeds
                    .Where(b => b.AuctionId == auctionId && b.ItemId == itemId)
                    .OrderBy(b => b.DateTime)
                    .ToListAsync();

                if (!bidders.Any())
                {
                    throw new KeyNotFoundException($"No bidders found for auctionId {auctionId} and itemId {itemId}.");
                }

                return bidders;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error occurred while retrieving the bidders from the database.", dbEx);
            }
            catch (KeyNotFoundException notFoundEx)
            {
                throw new Exception(notFoundEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving the bidders. Please try again.", ex);
            }
        }




        /// <summary>
        /// Retrieves the last bid placed on a specific auction item.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <param name="itemId">The ID of the item.</param>
        /// <returns>The most recent bid placed on the item.</returns>
        public async Task<ItemBidded> GetLastBidForItemAsync(int auctionId, int itemId)
        {
            try
            {
                var lastBid = await _context.ItemBiddeds
                    .Where(b => b.AuctionId == auctionId && b.ItemId == itemId)
                    .OrderByDescending(b => b.DateTime)
                    .FirstOrDefaultAsync();


                return lastBid;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error occurred while retrieving the last bid from the database.", dbEx);
            }
            catch (KeyNotFoundException notFoundEx)
            {
                throw new Exception(notFoundEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving the last bid. Please try again.", ex);
            }
        }

        /// <summary>
        /// Retrieves the highest bid placed on a specific auction item.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <param name="itemId">The ID of the item.</param>
        /// <returns>The highest bid placed on the item.</returns>
        public async Task<ItemBidded> GetHighestBidForItemAsync(int auctionId, int itemId)
        {
            try
            {
                var highestBid = await _context.ItemBiddeds
                    .Where(b => b.AuctionId == auctionId && b.ItemId == itemId)
                    .OrderByDescending(b => b.Amount)
                    .FirstOrDefaultAsync();

                if (highestBid == null)
                {
                    throw new KeyNotFoundException($"No bids found for auctionId {auctionId} and itemId {itemId}.");
                }

                return highestBid;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error occurred while retrieving the highest bid from the database.", dbEx);
            }
            catch (KeyNotFoundException notFoundEx)
            {
                throw new Exception(notFoundEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving the highest bid. Please try again.", ex);
            }
        }
    }
}
