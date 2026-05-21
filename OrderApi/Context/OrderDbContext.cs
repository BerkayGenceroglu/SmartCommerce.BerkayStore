using Microsoft.EntityFrameworkCore;
using OrderApi.Entities;

namespace OrderApi.Context
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<Coupon> Coupons => Set<Coupon>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(x => x.Address).IsRequired();
                entity.HasMany(x => x.Items)
                      .WithOne(x => x.Order)
                      .HasForeignKey(x => x.OrderId);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductName).IsRequired();
                entity.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<AppUser>().ToTable("Users");
        }

    }
}
        