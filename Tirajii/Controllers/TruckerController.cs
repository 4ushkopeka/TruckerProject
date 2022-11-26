using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly ITruckerService truckerService;
        private readonly INotyfService notyf;
        public TruckerController(ITruckerService _trService, INotyfService _notyf)
        {
            truckerService = _trService;
            notyf = _notyf;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new TruckerRegisterViewModel
            {
                TruckingCategories =  await truckerService.GetAllTruckingCategories()
            };
            ViewBag.Title = "Register as a trucker";
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
                notyf.Information("Welcome");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await truckerService.GetUserWithTrucker(User.Id());
            var model = new TruckerRegisterViewModel
            {
                FullName = user.Trucker.Name,
                PhoneNumber = user.Trucker.PhoneNumber,
                Email = user.Trucker.Email,
                CategoryId = user.Trucker.CategoryId,
                Picture = user.Trucker.ProfilePicture,
                Description = user.Trucker.Description,
                TruckingCategories = await truckerService.GetAllTruckingCategories()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(TruckerRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var userId = this.User.Id();
                await truckerService.EditTrucker(model, userId);
                notyf.Information("Successfully edited your profile");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
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

            var categories = await truckerService.GetAllTruckingCategories();

            model.Offers = result.Offers;
            model.Categories = categories;
            model.TotalOffers = result.TotalOffers;
            ViewBag.User = await truckerService.GetUserWithTrucker(User.Id());
            return View(model);
        }

        [HttpGet]
        public IActionResult TruckOffersAll(AllTruckOffersViewModel model)
        {
            var result = truckerService.GetAllTruckOffers(model.Category,
                model.SearchTerm,
                model.Sorting,
                model.CurrentPage);

            var categories = truckerService.GetAllClasses();

            model.Offers = result.Offers;
            model.Categories = categories;
            model.TotalOffers = result.TotalOffers;

            ViewBag.User = truckerService.GetUserWithTrucker(User.Id()).Result;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AllCompanies(AllCompaniesViewModel model)
        {
            var result = truckerService.GetAllCompanies(model.Category,
                model.SearchTerm,
                model.Sorting,
                model.CurrentPage);

            var categories = await truckerService.GetAllCompanyCategories();

            model.Companies = result.Companies;
            model.Categories = categories;
            model.TotalCompanies = result.TotalCompanies;
            ViewBag.User = await truckerService.GetUserWithTrucker(User.Id());
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ClaimOffer(int offerId)
        {
            try
            {
                var userId = User.Id();
                await truckerService.ClaimAnOffer(userId, offerId);
                notyf.Success("Succesfully claimed an offer!");
                return RedirectToAction("OffersMine", "Trucker");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("Offers", "Trucker");
            }
           
        }
        
        [HttpPost]
        public async Task<IActionResult> RateACompany(int id, int rating)
        {
            
            try
            {
                if (rating == 0)
                {
                    throw new InvalidOperationException("Cannot submit a rating of 0!");
                }
                else
                {
                    var userId = User.Id();
                    await truckerService.RateACompany(userId, id, rating);
                    notyf.Success("Thank you for rating!");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("AllCompanies", "Trucker");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> OffersMine()
        {
            var userId = User.Id();
            var offers = await truckerService.GetMyOffers(userId);
            ViewBag.User = await truckerService.GetUserWithTrucker(User.Id());
            return View(offers);
        }

        [HttpGet]
        public async Task<IActionResult> OffersCompleted()
        {
            var userId = User.Id();
            var offers = await truckerService.GetMyCompletedOffers(userId);
            return View(offers);
        }
        
        [HttpPost]
        public async Task<IActionResult> CompleteOffer(int offerId)
        {
            var userId = User.Id();
            var result = await truckerService.OfferSucceed(userId, offerId);
            if (result.WasLeveledUp) notyf.Success($"Nice! You levelled up and now are on level {result.Level}!");
            else notyf.Success($"Nice! You were rewarded {result.Xp} XP, keep it up!");
            return RedirectToAction("OffersMine", "Trucker");
        }

        [HttpPost]
        public async Task<IActionResult> FailOffer(int offerId)
        {
            var userId = User.Id();
            var result = await truckerService.FailOffer(userId, offerId);
            if (result.WasLeveledUp) notyf.Error("Eek, you were sanctioned a level for that! Do better next time!");
            else notyf.Error($"You failed the task and lost {result.Xp} XP, a bit disappointing...");
            return RedirectToAction("OffersMine", "Trucker");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Purchase(int truckid)
        {
            var userId = this.User.Id();
            var successfulPurchase = await truckerService.Purchase(truckid, userId);
            if (!successfulPurchase) return BadRequest();
            notyf.Success("You successfully purchased a Truck!");
            return RedirectToAction("General", "Truck");
        }

    }
}
