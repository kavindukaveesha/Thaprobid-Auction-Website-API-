using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Auction
{
    public class AuctionItemLotCreateDto
    {


        [Required]
        [StringLength(100)]
        public string LotName { get; set; } = String.Empty;

        [StringLength(500)]
        public string LotDescription { get; set; } = String.Empty;

        public string LotImageUrl { get; set; } = String.Empty;

        [StringLength(255)]
        public string LotCondition { get; set; } // E.g., "New", "Used - Like New", "Used - Good", etc.

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimateBidStartPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimateBidEndPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalFees { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        public int BidInterval { get; set; }

        public int FieldId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}