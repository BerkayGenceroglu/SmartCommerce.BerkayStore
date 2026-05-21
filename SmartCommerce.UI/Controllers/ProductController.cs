using Microsoft.AspNetCore.Mvc;

namespace SmartCommerce.UI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(Guid id)
        {
            return View(id);
        }
    }
}
