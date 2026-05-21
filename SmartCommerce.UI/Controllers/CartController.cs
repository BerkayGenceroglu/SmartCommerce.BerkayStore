using Microsoft.AspNetCore.Mvc;

namespace SmartCommerce.UI.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
