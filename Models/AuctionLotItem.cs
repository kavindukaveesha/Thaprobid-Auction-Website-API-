using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class AuctionLotItem
    {
        public int AuctionLotItemId { get; set; }

        [Required]
        public int AuctionId { get; set; }


        public int FieldId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        [Required, MaxLength(100)]
        public string LotName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string LotDescription { get; set; } = string.Empty;

        public string LotImageUrl { get; set; } = string.Empty;

        public string LotCondition { get; set; } // E.g., "New", "Used - Like New", "Used - Good", etc.

        public decimal EstimateBidStartPrice { get; set; }

        public decimal EstimateBidEndPrice { get; set; }

        public decimal AdditionalFees { get; set; }

        public decimal ShippingCost { get; set; }

        public int BidInterval { get; set; }

        public bool IsSold { get; set; } = false;

        public int? WinningBidderId { get; set; }
        public bool IsBiddingActive { get; set; }

        // Add timestamps for better auditing
        // public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // You can add more relationships if necessary
    }
}
