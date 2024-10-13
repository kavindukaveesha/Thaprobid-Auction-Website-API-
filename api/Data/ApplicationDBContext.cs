using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Models;

namespace api.data
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<Field> Fields { get; set; }
        public DbSet<Category> Categories { get; set; }


    }
}