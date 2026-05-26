using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using SmartCommerce.UI.Areas.Admin.Abstract;
using System;

namespace SmartCommerce.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminController : Controller
{
    private readonly IAdminProductService _productService;
    private readonly IAdminOrderService _orderService;
    private readonly IUserService _userService;
    private readonly ICargoService _cargoService;
    private readonly ICouponService _couponService;
    private readonly IInvoiceService _invoiceService;
    private readonly IReviewService _reviewService;
    public AdminController(
        IAdminProductService productService,
        IAdminOrderService orderService,
        IUserService userService,
        ICargoService cargoService,
        ICouponService couponService,
        IInvoiceService invoiceService,
        IReviewService reviewService)
    {
        _productService = productService;
        _orderService = orderService;
        _userService = userService;
        _cargoService = cargoService;
        _couponService = couponService;
        _invoiceService = invoiceService;
        _reviewService = reviewService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var action = context.RouteData.Values["action"]?.ToString();

        if (action == "Login" || action == "Logout" || action == "SaveSession")
        {
            base.OnActionExecuting(context);
            return;
        }

        var token = HttpContext.Session.GetString("AdminToken");
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToActionResult("Login", "Admin", new { area = "Admin" });
            return;
        }

        base.OnActionExecuting(context);
    }

    public IActionResult Login() { return View(); }

    [HttpPost]
    public IActionResult SaveSession(string token, string name)
    {
        HttpContext.Session.SetString("AdminToken", token);
        HttpContext.Session.SetString("AdminName", name);
        return Ok();
    }
    public IActionResult Logout() => RedirectToAction("Login");

    // ── DASHBOARD ──
    public async Task<IActionResult> Dashboard()
    {
        ViewData["Title"] = "Dashboard";
        var products = await _productService.GetAllAsync();
        var orders = await _orderService.GetAllAsync();
        var users = await _userService.GetAllAsync();
        var cargos = await _cargoService.GetAllAsync();
        var now = DateTime.UtcNow;

        // ── SİPARİŞ İSTATİSTİKLERİ ──
        ViewBag.TotalOrders = orders.Count;
        ViewBag.OrdersToday = orders.Count(o => o.CreatedAt.Date == now.Date);

        // ── GELİR İSTATİSTİKLERİ ──
        ViewBag.TotalRevenue = orders.Sum(o => o.TotalAmount);
        ViewBag.RevenueToday = orders.Where(o => o.CreatedAt.Date == now.Date).Sum(o => o.TotalAmount);

        // ── ÜRÜN İSTATİSTİKLERİ ──
        ViewBag.TotalProducts = products.Count;
        ViewBag.ActiveProducts = products.Count(p => p.IsActive);
        ViewBag.OutOfStock = products.Count(p => p.Stock == 0);
        ViewBag.LowStock = products.Count(p => p.Stock > 0 && p.Stock <= 5);
        ViewBag.TopOrderedProducts = orders
        .SelectMany(o => o.Items)
        .GroupBy(i => i.ProductName)
        .Select(g => new { ProductName = g.Key, TotalQuantity = g.Sum(i => i.Quantity), TotalRevenue = g.Sum(i => i.UnitPrice * i.Quantity) })
        .OrderByDescending(x => x.TotalQuantity)
        .Take(5)
        .ToList();

        // ── KULLANICI İSTATİSTİKLERİ ──
        ViewBag.TotalUsers = users.Count;
        ViewBag.AdminCount = users.Count(u => u.Role == 1);
        ViewBag.NewUsersThisMonth = users.Count(u => u.CreatedAt >= now.AddMonths(-1));

        // ── KARGO ──
         ViewBag.TotalCargos = cargos.Count;

        // ── SON 5 SİPARİŞ ──
        ViewBag.RecentOrders = orders.Take(5).ToList();

        // ── KATEGORİ DAĞILIMI ──
        var categoryStats = products
            .GroupBy(p => p.CategoryName)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToList();

        ViewBag.CategoryStats = categoryStats;

        // ── EN ÇOK TERCİH EDİLEN KATEGORİ ──
        ViewBag.TopCategory = categoryStats.FirstOrDefault()?.Category ?? "-";
        ViewBag.TopCategoryCount = categoryStats.FirstOrDefault()?.Count ?? 0;

        // ── EN ÇOK TERCİH EDİLEN ÜRÜN (En yüksek stok çıkışı = en düşük stok) ──
        var topProducts = products
            .Where(p => p.IsActive)
            .OrderBy(p => p.Stock)
            .Take(5)
            .Select(p => new { p.Name, p.Brand, p.CategoryName, p.Price, p.Stock })
            .ToList();

        ViewBag.TopProducts = topProducts;

        return View();
    }

    // ── ÜRÜNLER ──
    public async Task<IActionResult> Products()
    {
        ViewData["Title"] = "Ürün Yönetimi";

        var products = await _productService.GetAllAsync();
        var categories = await _productService.GetCategoriesAsync();

        ViewBag.Products = products
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        ViewBag.Categories = categories;
        ViewBag.TotalProducts = products.Count;
        ViewBag.ActiveProducts = products.Count(p => p.IsActive);
        ViewBag.OutOfStock = products.Count(p => p.Stock == 0);

        return View();
    }

    // ── SİPARİŞLER ──
    public async Task<IActionResult> Orders()
    {
        ViewData["Title"] = "Sipariş Yönetimi";

        var orders = await _orderService.GetAllAsync();

        ViewBag.Orders = orders
            .OrderByDescending(o => o.CreatedAt)
            .ToList();

        ViewBag.TotalOrders = orders.Count;
        ViewBag.TotalRevenue = orders.Sum(o => o.TotalAmount);
        ViewBag.TodayOrders = orders.Count(o => o.CreatedAt.Date == DateTime.UtcNow.Date);

        return View();
    }

    // ── KARGO ──
    public async Task<IActionResult> Cargo()
    {
        ViewData["Title"] = "Kargo Takibi";

        var cargos = await _cargoService.GetAllAsync();

        ViewBag.Cargos = cargos
            .OrderByDescending(c => c.CreatedAt)
            .ToList();

        ViewBag.TotalCargos = cargos.Count;
        ViewBag.DeliveredCargos = cargos.Count(c => c.Status == "Delivered");
        ViewBag.PreparingCargos = cargos.Count(c => c.Status == "Preparing");
        ViewBag.ShippedCargos = cargos.Count(c => c.Status == "Shipped");
        return View();
    }

    // ── KUPONLAR ──
    public async Task<IActionResult> Coupons()
    {
        ViewData["Title"] = "Kupon Yönetimi";

        var coupons = await _couponService.GetAllAsync();

        ViewBag.Coupons = coupons
            .ToList();

        ViewBag.TotalCoupons = coupons.Count;
        ViewBag.ActiveCoupons = coupons.Count(c => c.IsActive);

        return View();
    }

    // ── KULLANICILAR ──
    public async Task<IActionResult> Users()
    {
        ViewData["Title"] = "Kullanıcı Yönetimi";

        var users = await _userService.GetAllAsync();

        ViewBag.Users = users
            .OrderByDescending(u => u.CreatedAt)
            .ToList();

        ViewBag.TotalUsers = users.Count;
        ViewBag.AdminCount = users.Count(u => u.Role == 0);
        ViewBag.UserCount = users.Count(u => u.Role == 1);
        ViewBag.NewUsersThisMonth = users.Count(u =>
            u.CreatedAt.Month == DateTime.UtcNow.Month &&
            u.CreatedAt.Year == DateTime.UtcNow.Year);

        return View();
    }

    public async Task<IActionResult> Invoices()
    {
        ViewData["Title"] = "Fatura Yönetimi";
        var invoices = await _invoiceService.GetAllAsync();
        ViewBag.Invoices = invoices.OrderByDescending(i => i.CreatedAt).ToList();
        ViewBag.TotalInvoices = invoices.Count;
        ViewBag.TotalAmount = invoices.Sum(i => i.TotalAmount);
        ViewBag.TodayInvoices = invoices.Count(i => i.CreatedAt.Date == DateTime.UtcNow.Date);
        return View();
    }

    public async Task<IActionResult> Reviews()
    {
        ViewData["Title"] = "Yorum Yönetimi";

        var reviews = await _reviewService.GetReviewListsAsync();

        // ── TEMEL METRİKLER ──
        ViewBag.Reviews = reviews.OrderByDescending(r => r.CreatedAt).ToList();
        ViewBag.TotalReviews = reviews.Count;
        ViewBag.AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

        ViewBag.WithTheMostComments = reviews
            .GroupBy(r => r.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                ProductName = $"Ürün #{g.Key.ToString().Substring(0, 8).ToUpper()}",
                CommentCount = g.Count()
            })
            .OrderByDescending(x => x.CommentCount)
            .Take(5)
            .ToList();

        ViewBag.TopRatedProducts = reviews
            .GroupBy(r => r.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                ProductName = $"Ürün #{g.Key.ToString().Substring(0, 8).ToUpper()}",
                AverageRating = g.Average(r => r.Rating),
                CommentCount = g.Count()
            })
            .OrderByDescending(x => x.AverageRating)
            .ThenByDescending(x => x.CommentCount) // Güvenilir istatistik için önemli kırılım
            .Take(5)
            .ToList();

        return View();
    }


   
}