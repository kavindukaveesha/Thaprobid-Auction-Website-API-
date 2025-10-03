using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Auction
{
    public class ItemBiddedDto
    {

        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}