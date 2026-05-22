using Microsoft.EntityFrameworkCore;

namespace SmartCommerce.UI.Areas.Admin.Context
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

        public DbSet<CargoEntity> Cargos => Set<CargoEntity>();
    }
}
