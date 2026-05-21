using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Dtos;
using OrderApi.Services;

namespace OrderApi.Controllers
{
    [Route("api/coupons")]
    [Authorize]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> Apply(ApplyCouponDto dto)
        {
            try
            {
                var result = await _couponService.ApplyCouponAsync(dto.Code, dto.CartTotal);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                var result = await _couponService.GetByCodeAsync(code);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
