using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Services;
using System.Security.Claims;

namespace ProductApi.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            try
            {
                var result = await _favoriteService.GetUserFavoritesAsync(GetUserId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddFavorite(Guid productId)
        {
            try
            {
                var result = await _favoriteService.AddFavoriteAsync(GetUserId(), productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFavorite(Guid productId)
        {
            try
            {
                await _favoriteService.RemoveFavoriteAsync(GetUserId(), productId);
                return Ok("Favorilerden kaldırıldı!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{productId}/check")]
        public async Task<IActionResult> IsFavorite(Guid productId)
        {
            try
            {
                var result = await _favoriteService.IsFavoriteAsync(GetUserId(), productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
