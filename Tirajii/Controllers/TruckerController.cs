using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tirajii.Data.Models;
using Tirajii.Models.Trucker;
using Tirajii.Services;
using Tirajii.Services.Contracts;

namespace Tirajii.Controllers
{
    [Authorize]
    public class TruckerController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ITruckerService trService;
        public TruckerController(ITruckerService _trService, UserManager<User> _userManager)
        {
            trService = _trService;
            userManager = _userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new TruckerRegisterViewModel
            {
                TruckingCategories =  await trService.GetAllCategories()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register(TruckerRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) throw new Exception("Invalid User!");
                await trService.RegisterTrucker(model, userId);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
        public IActionResult Offers()
        {
            return View();
        }
        public IActionResult OffersMine()
        {
            return View();
        }
        public IActionResult OffersCompleted()
        {
            return View();
        }
    }
}
