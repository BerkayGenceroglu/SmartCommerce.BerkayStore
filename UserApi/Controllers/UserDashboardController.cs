using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UserApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserDashboardController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserDashboardController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new {
                    x.Id,
                    x.FullName,
                    x.Email,
                    x.Role,
                    x.CreatedAt
                })
                .ToListAsync();
            return Ok(users);
        }
    }
}
