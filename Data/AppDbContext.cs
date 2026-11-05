using Microsoft.EntityFrameworkCore;
using ToolStore.Models;

namespace ToolStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<ApplicationArea> ApplicationAreas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }
}