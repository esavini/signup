using signup.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using signup.Data;

namespace CustomIdentity.Controllers;

public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : Controller {
    
    public IActionResult Register() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register model) {
        if (ModelState.IsValid) {
            AppUser user = new() {
                Name = model.Name,
                UserName = model.Email,
                Email = model.Email,
                Surname = model.Surname,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Role = model.Role
            };

            var result = await userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded) {
                await signInManager.PasswordSignInAsync(user, model.Password!, false, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description); // Whatever error it may be e.g. Username already taken, Password error etc.
            }
        }
        return View(model);
    }

    public IActionResult Login() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login model) {
        if (ModelState.IsValid) {
            var result = await signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);

            if (result.Succeeded) {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }
        return View(model);
    }

    public async Task<IActionResult> Logout() {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index","Home");
    }
}