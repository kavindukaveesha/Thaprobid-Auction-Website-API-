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


    }
}
