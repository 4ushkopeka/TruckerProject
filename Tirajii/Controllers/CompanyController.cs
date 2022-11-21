using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tirajii.Data.Models;
using Tirajii.Models.Company;
using Tirajii.Models.Trucker;
using Tirajii.Services.Contracts;
using Tirajii.Infrastructure.Extensions;

namespace Tirajii.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;
        private readonly INotyfService notyf;
        private readonly IUserService userService;
        public CompanyController(ICompanyService _cService, INotyfService _notyf, IUserService _userService)
        {
            companyService = _cService;
            notyf = _notyf;
            userService = _userService;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new CompanyRegisterViewModel
            {
                Categories = await companyService.GetAllCompanyCategories()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register(CompanyRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var userId = this.User.Id();
                await companyService.RegisterCompany(model, userId);
                notyf.Information("Welcome!");
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
        public async Task<IActionResult> RegisterATruck()
        {
            var model = new TruckViewModel
            {
                Classes = await companyService.GetAllClasses()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterATruck(TruckViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var userId = this.User.Id();
                await companyService.RegisterTruck(model, userId);
                notyf.Success("Successfully registered a truck!");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public  IActionResult TruckOfferAdd(int id)
        {
            var offer = new TruckOfferAddViewModel()
            {
                truckId = id
            };
            return View(offer);
        }
        [HttpGet]
        public async Task<IActionResult> GetMyTrucks()
        {
            var userId = this.User.Id();
            var trucks = await companyService.GetMyTrucksForOffer(userId);
            return View("CompanyTrucks",trucks);
        }
        [HttpPost]
        public async Task<IActionResult> TruckOfferAdd(TruckOfferAddViewModel model, int truckId, int companyId)
        {
            model.truckId = truckId;
            model.CompanyId = companyService.GetTruckById(truckId).Result.CompanyId;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var userId = this.User.Id();
                await companyService.AddTruckOffer(model, userId);
                notyf.Success("Successfully added an offer!");
                return RedirectToAction("TruckOfferMine", "Company");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> OfferMine()
        {
            try
            {
                var userId = this.User.Id();
                var mine = await companyService.GetMyOffers(userId);
                return View(mine);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public IActionResult TruckOfferMine()
        {
            try
            {
                var userId = this.User.Id();
                var mine = companyService.GetMyTruckOffers(userId).Result;
                return View(mine);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> OfferAdd()
        {
            var model = new OfferAddViewModel()
            {
                Categories = await companyService.GetAllOfferCategories()
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Rating()
        {
            var model = await companyService.GetRating(User.Id());

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> TrucksMine()
        {
            try
            {
                var userId = this.User.Id();
                var trucks = await companyService.GetMyTrucks(userId);
                return View(trucks);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> OfferAdd(OfferAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var userId = this.User.Id();
                await companyService.AddOffer(model, userId);
                notyf.Success("Successfully added an offer!");
                return RedirectToAction(nameof(OfferMine));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
