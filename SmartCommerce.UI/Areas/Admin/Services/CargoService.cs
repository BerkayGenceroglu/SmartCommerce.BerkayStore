using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using SmartCommerce.UI.Areas.Admin.Abstract;
using SmartCommerce.UI.Areas.Admin.Context;
using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Services
{
    public class CargoService : ICargoService
    {
        private readonly AdminDbContext _context;

        public CargoService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<CargoDto>> GetAllAsync()
        {
            try
            {
                return await _context.Cargos
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new CargoDto
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        TrackingNumber = x.TrackingNumber,
                        Status = x.Status.ToString(),
                        CreatedAt = x.CreatedAt
                    })
                    .ToListAsync();
            }
            catch { return new(); }
        }
    }
}
