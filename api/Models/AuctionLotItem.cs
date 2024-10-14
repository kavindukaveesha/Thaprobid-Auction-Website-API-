using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AuctionLotItem
    {

        public int AuctionLotItemId { get; set; }


        public int AuctionId { get; set; }


        public string LotName { get; set; } = String.Empty;
        public string LotDescription { get; set; } = String.Empty;

        public string LotImageUrl { get; set; } = String.Empty;

        public string LotCondition { get; set; } // E.g., "New", "Used - Like New", "Used - Good", etc.


        public decimal EstimateBidStartPrice { get; set; }
        public decimal EstimateBidEndPrice { get; set; }


        public decimal AdditionalFees { get; set; }


        public decimal ShippingCost { get; set; }

        public int BidInterval { get; set; }


        public bool IsSold { get; set; } = false; // Flag to indicate if the lot has been sold

        public int? WinningBidderId { get; set; }

        // Navigation Property for Bidders (Many-to-Many Relationship)
        //public virtual ICollection<ItemBidder> ItemBidders { get; set; } = new List<ItemBidder>(); 

        // ---- Additional Attributes to Consider ----







        // public virtual ItemBidder WinningBidder { get; set; }
    }
}