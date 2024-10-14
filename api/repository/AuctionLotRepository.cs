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
    public class AuctionLotRepository : IAuctionLotRepository
    {
        private readonly ApplicationDBContext _context;

        public AuctionLotRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<AuctionLotItem?> AddnewItemAsync(AuctionLotItem auctionLotItemModel)
        {
            if (auctionLotItemModel == null)
            {
                throw new ArgumentNullException(nameof(auctionLotItemModel));
            }

            try
            {
                _context.AuctionLotItems.Add(auctionLotItemModel);
                await _context.SaveChangesAsync();
                return auctionLotItemModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while creating a new auction lot item. Please try again later.", ex);
            }
        }
    }
}