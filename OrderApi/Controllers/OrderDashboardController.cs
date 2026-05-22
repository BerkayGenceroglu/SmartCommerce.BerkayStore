using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApi.Context;

namespace OrderApi.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderDashboardController : ControllerBase
    {
        private readonly OrderDbContext _context;

        public OrderDashboardController(OrderDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders
        .Include(x => x.Items)
        .OrderByDescending(x => x.CreatedAt)
        .Select(x => new
        {
            x.Id,
            x.UserId,
            x.Address,
            x.TotalAmount,
            x.Status,
            x.CreatedAt,
            Items = x.Items.Select(i => new
            {
                i.Id,
                i.ProductName,
                i.Quantity,
                i.UnitPrice,
            }).ToList()
        })
        .ToListAsync();

            return Ok(orders);
        }
    }
}
