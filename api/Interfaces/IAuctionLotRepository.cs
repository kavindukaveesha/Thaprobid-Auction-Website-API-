using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IAuctionLotRepository
    {
        Task<List<Category>> GetAllLotItemsAsync();
        Task<AuctionLotItem?> AddnewLotItemAsync(AuctionLotItem auctionLotItemModel);
        Task<Category?> GetLotItemByIdAsync(int id);
        //  Task<Category?> UpdateLotItemAsync(int id, UpdateCategoryDto categoryDto);
        Task<Category?> DeleteLotItemAsync(int id);

    }
}