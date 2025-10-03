using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.seller;

namespace api.repository
{
    public interface ISellerRepository
    {
        // Create a new seller
        Task<SellerDto> CreateSellerAsync(CreateSellerDto newSellerDto);

        // Get all sellers
        Task<List<SellerDto>> GetAllSellersAsync();

        // Get seller by ID
        Task<SellerDto> GetSellerByIdAsync(int id);

        // Update seller details
        Task<SellerDto> UpdateSellerAsync(int id, UpdateSellerDto updatedSellerDto);

        // Delete seller by ID
        Task DeleteSellerAsync(int id);

        // Activate or deactivate seller
        Task<SellerDto> SetSellerActiveStatusAsync(int id, bool isActive);
        //get sellerdetails by userid
        Task<SellerDto> GetSellerByUserIdAsync(int userId);
    }
}
