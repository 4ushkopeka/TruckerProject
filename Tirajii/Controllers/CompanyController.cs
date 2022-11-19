using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tirajii.Data.Models;
using Tirajii.Models.Company;
using Tirajii.Models.Trucker;
using Tirajii.Services.Contracts;

namespace Tirajii.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyService cService;
        private readonly INotyfService notyf;
        public CompanyController(ICompanyService _cService, INotyfService _notyf)
        {
            cService = _cService;
            notyf = _notyf;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new CompanyRegisterViewModel
            {
                Categories = await cService.GetAllCompanyCategories()
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
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) throw new Exception("Invalid User!");
                await cService.RegisterCompany(model, userId);
                notyf.Information("Welcome!");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error("Something went wrong, please try again.");
                return View(model);
            } 
        }

        [HttpGet]
        public async Task<IActionResult> RegisterATruck()
        {
            var model = new TruckViewModel
            {
                Classes = await cService.GetAllClasses()
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
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) throw new Exception("Invalid User!");
                await cService.RegisterTruck(model, userId);
                notyf.Success("Successfully registered a truck!");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error("Something went wrong, please try again.");
                return View(model);
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new Exception("Invalid User!");
            var trucks = await cService.GetMyTrucks(userId);
            return View("CompanyTrucks",trucks);
        }
        [HttpPost]
        public async Task<IActionResult> TruckOfferAdd(TruckOfferAddViewModel model, int truckId, int companyId)
        {
            model.truckId = truckId;
            model.CompanyId = cService.GetTruckById(truckId).Result.CompanyId;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) throw new Exception("Invalid User!");
                await cService.AddTruckOffer(model, userId);
                notyf.Success("Successfully added an offer!");
                return RedirectToAction("TruckOfferMine", "Company");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error("Something went wrong, please try again.");
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult OfferMine()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) throw new Exception("Invalid User!");
                var mine = cService.GetMyOffers(userId);
                return View(mine);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error("Something went wrong, please try again.");
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public IActionResult TruckOfferMine()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) throw new Exception("Invalid User!");
                var mine = cService.GetMyTruckOffers(userId).Result;
                return View(mine);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error("Something went wrong, please try again.");
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> OfferAdd()
        {
            var model = new OfferAddViewModel()
            {
                Categories = await cService.GetAllOfferCategories()
            };
            return View(model);
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
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    throw new ArgumentException("Invalid User");
                }
                await cService.AddOffer(model, userId);
                notyf.Success("Successfully added an offer!");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error("Something went wrong, please try again.");
                return View(model);
            }
        }
    }
}
