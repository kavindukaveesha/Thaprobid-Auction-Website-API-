using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace api.data
{
    public class ApplicationDBContext : IdentityDbContext
    {

        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<Field> Fields { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionLotItem> AuctionLotItems { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // This is important!

            builder.Entity<AuctionLotItem>(entity =>
            {
                entity.Property(e => e.AdditionalFees).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.EstimateBidEndPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.EstimateBidStartPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ShippingCost).HasColumnType("decimal(18, 2)");
            });
        }



    }
}