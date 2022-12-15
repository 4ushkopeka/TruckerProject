using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tirajii.Models.Company;
using Tirajii.Services.Contracts;
using Tirajii.Infrastructure.Extensions;
using Ganss.Xss;
using Microsoft.AspNetCore.Identity;
using Tirajii.Data.Models;

namespace Tirajii.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly HtmlSanitizer sanitizer = new();
        private readonly ICompanyService companyService;
        private readonly INotyfService notyf;
        private readonly IUserService userService;
        private readonly UserManager<User> manager;
        private readonly SignInManager<User> signInManager;
        public CompanyController(ICompanyService _cService, INotyfService _notyf, 
            IUserService _userService, UserManager<User> manager, SignInManager<User> signInManager)
        {
            companyService = _cService;
            notyf = _notyf;
            userService = _userService;
            this.manager = manager;
            this.signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new CompanyRegisterViewModel
            {
                Categories = await companyService.GetAllCompanyCategories()
            };
            ViewBag.Title = "Register your company";
            return View(model);
        }

        [Authorize]
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
                model.Picture = sanitizer.Sanitize(model.Picture);
                await companyService.RegisterCompany(model, userId);
                var user = await companyService.GetUserWithCompany(userId);
                if (user.IsTruckerCompanyOwner) await manager.AddToRoleAsync(user, "TruckCompany");
                else await manager.AddToRoleAsync(user, "OfferCompany");
                await signInManager.SignInAsync(user, false);
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

        [Authorize(Roles = "TruckCompany")]
        [HttpGet]
        public async Task<IActionResult> RegisterATruck()
        {
            var model = new TruckViewModel
            {
                Classes = await companyService.GetAllClasses()
            };
            return View(model);
        }

        [Authorize(Roles = "TruckCompany")]
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
                model.Picture = sanitizer.Sanitize(model.Picture);
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

        [Authorize(Roles = "TruckCompany")]
        [HttpGet]
        public IActionResult TruckOfferAdd(int id)
        {
            var offer = new TruckOfferAddNEditViewModel()
            {
                TruckId = id
            };
            ViewBag.Title = "Add a truck offer";
            return View(offer);
        }

        [Authorize(Roles = "TruckCompany")]
        [HttpGet]
        public async Task<IActionResult> GetMyTrucks()
        {
            var userId = this.User.Id();
            var trucks = await companyService.GetMyTrucksForOffer(userId);
            return View("CompanyTrucks", trucks);
        }

        [Authorize(Roles = "TruckCompany")]
        [HttpPost]
        public async Task<IActionResult> TruckOfferAdd(TruckOfferAddNEditViewModel model, int truckId, int companyId)
        {
            model.TruckId = truckId;
            model.CompanyId = companyId;
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

        [Authorize(Roles = "OfferCompany")]
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

        [Authorize(Roles = "TruckCompany")]
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

        [Authorize(Roles = "OfferCompany")]
        [HttpGet]
        public async Task<IActionResult> OfferAdd()
        {
            var model = new OfferAddNEditViewModel()
            {
                Categories = await companyService.GetAllOfferCategories()
            };
            ViewBag.Title = "Add an offer";
            return View(model);
        }

        [Authorize(Roles = "TruckCompany, OfferCompany")]
        [HttpGet]
        public async Task<IActionResult> Rating()
        {
            var model = await companyService.GetRating(User.Id());

            return View(model);
        }

        [Authorize(Roles = "TruckCompany")]
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

        [Authorize(Roles = "OfferCompany")]
        [HttpPost]
        public async Task<IActionResult> OfferAdd(OfferAddNEditViewModel model)
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

        [Authorize(Roles = "TruckCompany, OfferCompany")]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await companyService.GetUserWithCompany(User.Id());
            var model = new CompanyRegisterViewModel
            {
                Name = user.Company.Name,
                Picture = user.Company.Picture,
                CategoryId = user.Company.CategoryId,
                Categories = await companyService.GetAllCompanyCategories()
            };
            return View(model);
        }

        [Authorize(Roles = "TruckCompany, OfferCompany")]
        [HttpPost]
        public async Task<IActionResult> EditProfile(CompanyRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var userId = this.User.Id();
                model.Picture = sanitizer.Sanitize(model.Picture);
                await companyService.EditCompany(model, userId);
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

        [Authorize(Roles = "OfferCompany, Administrator")]
        [HttpGet]
        public IActionResult EditOffer(int id)
        {
            if (id == 0)
            {
                notyf.Error("Invalid offer!");
                return RedirectToAction("OfferMine", "Company");
            }
            var offer = companyService.GetOfferById(id).Result;
            var model = new OfferAddNEditViewModel
            {
                Description = offer.Description,
                Name = offer.Name,
                Payment = offer.Payment,
                DueDate = offer.DueDate.ToString("d"),
                CategoryId = offer.CategoryId,
                Categories = companyService.GetAllOfferCategories().Result,
                CompanyId = offer.CompanyId
            };
            return View(model);
        }
        
        [Authorize(Roles = "OfferCompany, Administrator")]
        [HttpPost]
        public async Task<IActionResult> EditOffer(OfferAddNEditViewModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await companyService.EditOffer(model, id);
                notyf.Information("Successfully edited your offer!");
                if (User.IsAdmin()) return RedirectToAction("OffersAll", "OfferAdministration", new { area = "Admin" });
                else return RedirectToAction("OfferMine", "Company");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "TruckCompany, Administrator")]
        [HttpGet]
        public IActionResult EditTruckOffer(int id)
        {
            if (id == 0)
            {
                notyf.Error("Invalid offer!");
                return RedirectToAction("OfferMine", "Company");
            }
            var truckOffer = companyService.GetTruckOfferById(id).Result;
            var model = new TruckOfferAddNEditViewModel
            {
                Name = truckOffer.Name,
                CompanyId = truckOffer.CompanyId,
                Cost = truckOffer.Cost,
                Description = truckOffer.Description,
                TruckId = truckOffer.TruckId,
            };
            return View(model);
        }
        
        [Authorize(Roles = "TruckCompany, Administrator")]
        [HttpPost]
        public async Task<IActionResult> EditTruckOffer(TruckOfferAddNEditViewModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await companyService.EditTruckOffer(model, id);
                notyf.Information("Successfully edited your truck offer!");
                if (User.IsAdmin()) return RedirectToAction("TruckOffersAll", "OfferAdministration", new { area = "Admin" });
                else return RedirectToAction("TruckOfferMine", "Company");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return View(model);
            }
        }
        
        [Authorize(Roles = "OfferCompany, Administrator")]
        [HttpPost]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            if (id == 0)
            {
                notyf.Error("Invalid offer!");
                return RedirectToAction("OfferMine", "Company");
            }
            try
            {
                await companyService.DeleteOffer(id);
                notyf.Information("Successfully deleted your offer!");
                if (User.IsAdmin()) return RedirectToAction("OffersAll", "OfferAdministration", new { area = "Admin" });
                else return RedirectToAction("OfferMine", "Company");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
        
        [Authorize(Roles = "TruckCompany, Administrator")]
        [HttpPost]
        public async Task<IActionResult> DeleteTruckOffer(int id)
        {
            if (id == 0)
            {
                notyf.Error("Invalid offer!");
                return RedirectToAction("OfferMine", "Company");
            }
            try
            {
                await companyService.DeleteTruckOffer(id);
                notyf.Information("Successfully deleted your truck offer!");
                if (User.IsAdmin()) return RedirectToAction("TruckOffersAll", "OfferAdministration", new { area = "Admin" });
                else return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("Index","Home");
            }
        }
    }
}
