using Microsoft.EntityFrameworkCore;
using SmartCommerce.UI.Areas.Admin.Abstract;
using SmartCommerce.UI.Areas.Admin.Context;
using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AdminDbContext _context;

        public InvoiceService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceDto>> GetAllAsync()
        {
            try
            {
                return (await _context.Invoices
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync())
                    .Select(x => new InvoiceDto
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        UserId = x.UserId,
                        TotalAmount = x.TotalAmount,
                        InvoiceNumber = x.InvoiceNumber,
                        CreatedAt = x.CreatedAt
                    })
                    .ToList();
            }
            catch { return new(); }
        }
    }
}
