using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace api.Models
{

    public class Auction
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuctionID { get; set; }

        public String AuctionRegisterId { get; set; }

        public string AuctionName { get; set; } = String.Empty;

        public string AuctionTitle { get; set; } = String.Empty;


        public string AuctionDescription { get; set; } = String.Empty;

        public string AuctionCoverImageUrl { get; set; } = String.Empty;

        public string VenueAddress { get; set; } = String.Empty;

        public string Location { get; set; } = String.Empty;

        public DateTime BiddingStartDate { get; set; }

        public DateTime BiddingStartTime { get; set; }

        public DateTime AuctionLiveDate { get; set; }

        public DateTime LiveAuctionTime { get; set; }

        public DateTime AuctionClosingDate { get; set; }
        public DateTime AuctionClosingTime { get; set; }

        public string TermsAndConditions { get; set; } = String.Empty;

        public string ImportantInformation { get; set; } = String.Empty;
        public bool IsVerified { get; set; } = false;//check auction verified from admin terms and condtions 
        public bool IsActive { get; set; } = false; // Track if the auction is currently open for bidding
        public bool IsClosed { get; set; } = false; // Mark if the auction has ended 

        // E.g., "Upcoming", "Live", "Closed", "Cancelled"
        public string AuctionStatus { get; set; } = String.Empty;

        // Foreign Key for Seller (1:M relationship)
        public int SellerId { get; set; }






        //   public virtual Seller Seller { get; set; } // Navigation property

        // Navigation Property for Auction Items (1:M relationship)
        //public virtual ICollection<AuctionItem> AuctionItems { get; set; } = new List<AuctionItem>();

        // ------ Additional Attributes to Consider ------

        // public decimal? StartingBid { get; set; } // Optional: Specify a starting bid amount
        // public decimal? MinimumBidIncrement { get; set; } // Control bid increments
        // public decimal? ReservePrice { get; set; }  // Optional: Hidden minimum price the seller will accept



        // public int? WinningBidId { get; set; } // Optional: Store the ID of the winning bid 
        // public virtual Bid WinningBid { get; set; } // Navigation property for the winning bid



    }
}
