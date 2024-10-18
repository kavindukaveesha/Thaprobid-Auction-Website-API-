using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.Dto.Auction
{
    public class AdminViewAuctionDto
    {

        public int AuctionID { get; set; }

        public string AuctionName { get; set; } = string.Empty;

        public string AuctionTitle { get; set; } = string.Empty;

        public string AuctionDescription { get; set; } = string.Empty;

        public string AuctionCoverImageUrl { get; set; } = string.Empty;

        public string VenueAddress { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime BiddingStartDate { get; set; }

        public DateTime BiddingStartTime { get; set; }

        public DateTime AuctionLiveDate { get; set; }

        public DateTime LiveAuctionTime { get; set; }

        public DateTime AuctionClosingDate { get; set; }

        public DateTime AuctionClosingTime { get; set; }

        public bool IsVerified { get; set; } // Admin check for auction verification

        public bool IsActive { get; set; } // Check if auction is active

        public bool IsClosed { get; set; } // Check if auction is closed

        public string AuctionStatus { get; set; } = string.Empty; // "Upcoming", "Live", "Closed", etc.

        public string TermsAndConditions { get; set; } = string.Empty;

        public string ImportantInformation { get; set; } = string.Empty;

        public int SellerId { get; set; } // To know who listed the auction


    }
}
