using CargoWorker.Context;
using CargoWorker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Enums;


namespace CargoWorker.Context
{
    public class CargoDbContext : DbContext
    {
        public CargoDbContext(DbContextOptions<CargoDbContext> options) : base(options) { }
        public DbSet<CargoRecord> Cargos => Set<CargoRecord>();
    }
}
