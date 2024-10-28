using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;

namespace api.Dto.Auction
{
    public class AuctionDto
    {
        public int AuctionID { get; set; }
        public string AuctionName { get; set; }
        public string AuctionTitle { get; set; }
        public string AuctionDescription { get; set; }
        public string AuctionCoverImageUrl { get; set; }
        public DateTime AuctionLiveDate { get; set; }
        public TimeSpan LiveAuctionTime { get; set; }
        public DateTime AuctionClosingDate { get; set; }
        public TimeSpan AuctionClosingTime { get; set; }

        // Calculated property for status
        public string Status
        {
            get
            {
                if (AuctionHelper.IsAuctionLive(AuctionLiveDate, LiveAuctionTime, AuctionClosingDate, AuctionClosingTime))
                    return "Live";
                else if (AuctionHelper.IsAuctionUpcoming(AuctionLiveDate, LiveAuctionTime))
                    return "Upcoming";
                else
                    return "Closed";
            }
        }
    }

}