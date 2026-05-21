using Microsoft.AspNetCore.Mvc;
using SmartCommerce.UI.Services;

namespace SmartCommerce.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _authService.LoginAsync(email, password);

            if (!result.Success)
            {
                ViewBag.Error = result.Error;
                return View();
            }

            HttpContext.Session.SetString("Token", result.Token!);
            HttpContext.Session.SetString("UserName", result.FullName!);
            HttpContext.Session.SetString("UserId", result.UserId!); // ← ekle
            return RedirectToAction("Index", "Product");
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            var success = await _authService.RegisterAsync(fullName, email, password);

            if (!success)
            {
                ViewBag.Error = "Kayıt oluşturulamadı. Email zaten kullanımda olabilir.";
                return View();
            }

            ViewBag.Success = "Kayıt başarılı! Giriş yapabilirsiniz.";
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Product");
        }
    }
}
