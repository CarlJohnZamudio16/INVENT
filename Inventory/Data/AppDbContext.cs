using Microsoft.EntityFrameworkCore;
using Inventory.Models.Entities;
using System.Collections.Generic;

namespace Inventory.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Items> Items { get; set; }



    }
}
