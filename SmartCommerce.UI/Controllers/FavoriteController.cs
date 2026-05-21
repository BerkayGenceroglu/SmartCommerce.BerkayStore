using Microsoft.AspNetCore.Mvc;

namespace SmartCommerce.UI.Controllers
{
    public class FavoriteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
