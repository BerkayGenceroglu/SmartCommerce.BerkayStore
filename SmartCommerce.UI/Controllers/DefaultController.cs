using Microsoft.AspNetCore.Mvc;

namespace SmartCommerce.UI.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
