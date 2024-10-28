using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api.Dto.Auction
{
    public class CreateAuctionDto
    {


        [Required(ErrorMessage = "Auction Name is required.")]
        [StringLength(100, ErrorMessage = "Auction Name cannot exceed 100 characters.")]
        public string AuctionName { get; set; } = String.Empty;

        [StringLength(200, ErrorMessage = "Auction Title cannot exceed 200 characters.")]
        public string AuctionTitle { get; set; } = String.Empty;

        [StringLength(500, ErrorMessage = "Auction Description cannot exceed 500 characters.")]
        public string AuctionDescription { get; set; } = String.Empty;

        public string AuctionCoverImageUrl { get; set; } = String.Empty;

        [StringLength(255, ErrorMessage = "Venue Address cannot exceed 255 characters.")]
        public string VenueAddress { get; set; } = String.Empty;

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]

        public string Location { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bidding Start Date is required.")]
        [DataType(DataType.Date)]
        public DateTime BiddingStartDate { get; set; }

        [Required(ErrorMessage = "Bidding Start Time is required.")]
        [DataType(DataType.Time)]
        public DateTime BiddingStartTime { get; set; }

        [Required(ErrorMessage = "Auction Live Date is required.")]
        [DataType(DataType.Date)]
        public DateTime AuctionLiveDate { get; set; }


        [Required(ErrorMessage = "Live Auction Time is required.")]
        [DataType(DataType.Time)]
        public DateTime LiveAuctionTime { get; set; }

        [Required(ErrorMessage = "Auction Closing Date is required.")]
        [DataType(DataType.Date)]
        public DateTime AuctionClosingDate { get; set; }


        [Required(ErrorMessage = " Auction Closing  Time is required.")]
        [DataType(DataType.Time)]
        public DateTime AuctionClosingTime { get; set; }




        public string TermsAndConditions { get; set; } = String.Empty;

        public string ImportantInformation { get; set; } = String.Empty;
        public int SellerId { get; set; }

    }
}
