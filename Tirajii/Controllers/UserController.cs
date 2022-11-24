using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tirajii.Data.Models;
using Tirajii.Infrastructure.Extensions;
using Tirajii.Models.User;
using Tirajii.Services.Contracts;

namespace Tirajii.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;    
        private readonly INotyfService notyf;    
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signManager;
        public UserController(UserManager<User> userManager, SignInManager<User> signManager, 
            IUserService _userService, INotyfService _notyf)
        {
            this.userManager = userManager;
            this.signManager = signManager;
            this.userService = _userService;
            notyf = _notyf;
        }

        [Authorize]
        public async Task<IActionResult> Purchase(int truckId)
        {
            var userId = this.User.Id();
            var successfulPurchase = await userService.Purchase(truckId, userId);
            if (!successfulPurchase) return BadRequest();
            notyf.Success("You successfully purchased a Truck!");
            return RedirectToAction("General", "Truck");
        }

        [Authorize]
        public async Task<IActionResult> ConnectWallet()
        {
            var userId = this.User.Id();
            var canConnect = await userService.ConnectWallet(userId);
            if (!canConnect) return BadRequest();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            var mod = new LoginViewModel();
            return View(mod);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await signManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Login failed.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "User");
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
