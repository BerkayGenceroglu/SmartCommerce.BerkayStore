using Microsoft.EntityFrameworkCore;
using ProductApi.Entities;

namespace ProductApi.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
                entity.Property(x => x.Stock).HasDefaultValue(0);
                entity.HasOne(x => x.Category)
                      .WithMany(x => x.Products)
                      .HasForeignKey(x => x.CategoryId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired();
                entity.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Comment).IsRequired();
                entity.HasOne(x => x.Product)
                      .WithMany()
                      .HasForeignKey(x => x.ProductId);
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Product)
                      .WithMany()
                      .HasForeignKey(x => x.ProductId);
                entity.HasIndex(x => new { x.UserId, x.ProductId }).IsUnique();
            });
        }
    }
}
