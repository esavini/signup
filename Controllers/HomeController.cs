using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using signup.Data;
using signup.Models;

namespace signup.Controllers;

public class HomeController(ILogger<HomeController> logger, AppDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : Controller {
    private readonly ILogger<HomeController> _logger = logger;
    private readonly AppDbContext _dbContext = context;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly UserManager<AppUser> _userManager = userManager;

    public IActionResult Index() {
        return View();
    }

    public IActionResult Products() {
        return View();
    }

    [Authorize]
    public IActionResult Cart() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userCart = _dbContext.Cart.Where(c => c.UserId == userId).ToList(); // .Select(c => c.Product).ToList()

        return View(userCart);
    }

    [HttpPost]
    public IActionResult AddToCart(Cart cart) { 
        /* cart has ProductId and Quantity */
        var product = _dbContext.Product.Where(p => p.ProductId == cart.ProductId).ElementAt(0);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _dbContext.Users.Where(u => u.Id == userId).ElementAt(0);

        _dbContext.Cart.Add(
            new Cart { 
                Product = product, 
                User = user,
                Quantity = cart.Quantity 
            }
        );
        _dbContext.SaveChanges();

        return RedirectToAction("Products");
    }

    [HttpPost]
    public IActionResult Order(Cart cart) {
        var toDelete = _dbContext.Cart.Where(c => c.CartId == cart.CartId);
        
        foreach (var item in toDelete) {
            _dbContext.Cart.Remove(item);
        }

        _dbContext.SaveChanges();
        
        return RedirectToAction("Cart");
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
