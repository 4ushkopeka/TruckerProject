using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tirajii.Data.Models;
using Tirajii.Infrastructure.Extensions;
using Tirajii.Models.Trucker;
using Tirajii.Services;
using Tirajii.Services.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tirajii.Controllers
{
    [Authorize]
    public class TruckerController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ITruckerService truckerService;
        public TruckerController(ITruckerService _trService, UserManager<User> _userManager)
        {
            truckerService = _trService;
            userManager = _userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new TruckerRegisterViewModel
            {
                TruckingCategories =  await truckerService.GetAllCategories()
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
                var userId = this.User.Id();
                await truckerService.RegisterTrucker(model, userId);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Offers(AllOffersViewModel model)
        {
            var result = truckerService.GetAllOffers(model.Category,
                model.SearchTerm,
                model.Sorting,
                model.CurrentPage);

            var categories = await truckerService.GetAllCategories();

            model.Offers = result.Offers;
            model.Categories = categories;
            model.TotalOffers = result.TotalOffers;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> TruckOffersAll(AllTruckOffersViewModel model)
        {
            var result = truckerService.GetAllTruckOffers(model.Category,
                model.SearchTerm,
                model.Sorting,
                model.CurrentPage);

            var categories = truckerService.GetAllClasses();

            model.Offers = result.Offers;
            model.Categories = categories;
            model.TotalOffers = result.TotalOffers;
            return View(model);
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
