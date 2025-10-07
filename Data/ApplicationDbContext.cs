using CleaningApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CleaningApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
        }

        public DbSet<Order> Orders { get; set; }
    }
}
