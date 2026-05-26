using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace SmartCommerce.UI.Controllers;

public class ProfileController : Controller
{
    private readonly HttpClient _http;

    public ProfileController(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient();
    }

    public IActionResult Index()
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Index", "Default");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(string fullName, string? phoneNumber, string? country, string? city, string? gender)
    {
        var token = HttpContext.Session.GetString("Token");
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var body = JsonSerializer.Serialize(new
        {
            FullName = fullName,
            PhoneNumber = phoneNumber,
            Country = country,
            City = city,
            Gender = gender
        });
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var res = await _http.PutAsync("https://localhost:7038/api/auth/profile", content);
        if (res.IsSuccessStatusCode)
            TempData["Success"] = "Bilgileriniz güncellendi.";
        else
            TempData["Error"] = "Güncelleme başarısız.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
        {
            TempData["Error"] = "Yeni şifreler eşleşmiyor.";
            return RedirectToAction("Index");
        }
        var token = HttpContext.Session.GetString("Token");
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var body = JsonSerializer.Serialize(new { CurrentPassword = currentPassword, NewPassword = newPassword });  // ← düzelt
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var res = await _http.PutAsync("https://localhost:7038/api/auth/password", content);
        if (res.IsSuccessStatusCode)
            TempData["Success"] = "Şifreniz değiştirildi.";
        else
            TempData["Error"] = "Mevcut şifre yanlış.";

        TempData["ActiveTab"] = "password"; // ← ekle
        return RedirectToAction("Index");
    }
}