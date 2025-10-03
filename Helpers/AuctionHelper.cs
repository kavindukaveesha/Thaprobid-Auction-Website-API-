using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public static class AuctionHelper
    {
        public static bool IsAuctionLive(DateTime auctionLiveDate, TimeSpan liveAuctionTime, DateTime auctionClosingDate, TimeSpan auctionClosingTime)
        {
            var currentDateTime = DateTime.UtcNow;
            var startDateTime = auctionLiveDate.Add(liveAuctionTime);
            var endDateTime = auctionClosingDate.Add(auctionClosingTime);

            return currentDateTime >= startDateTime && currentDateTime <= endDateTime;
        }

        public static bool IsAuctionUpcoming(DateTime auctionLiveDate, TimeSpan liveAuctionTime)
        {
            var currentDateTime = DateTime.UtcNow;
            var startDateTime = auctionLiveDate.Add(liveAuctionTime);

            return currentDateTime < startDateTime;
        }

        public static bool IsAuctionClosed(DateTime auctionClosingDate, TimeSpan auctionClosingTime)
        {
            var currentDateTime = DateTime.UtcNow;
            var endDateTime = auctionClosingDate.Add(auctionClosingTime);

            return currentDateTime > endDateTime;
        }
    }

}