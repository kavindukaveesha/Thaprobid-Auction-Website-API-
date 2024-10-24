using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ItemBidded
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}