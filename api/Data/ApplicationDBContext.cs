using System;
using System.Collections.Generic;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;


namespace api.data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Field> Fields { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionLotItem> AuctionLotItems { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ... other configurations ...

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.ClientProfile) // An AppUser has one ClientProfile
                .WithOne(cp => cp.AppUser)    // A ClientProfile belongs to one AppUser 
                .HasForeignKey<ClientProfile>(cp => cp.UserId); // Foreign key in ClientProfile

            // **Correct the relationship with SellerProfile**
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Seller) // An AppUser has one Seller
                .WithOne(sp => sp.AppUser)    // A Seller belongs to one AppUser
                .HasForeignKey<Seller>(sp => sp.UserId); // Foreign key in SellerProfile
        }



    }
}
