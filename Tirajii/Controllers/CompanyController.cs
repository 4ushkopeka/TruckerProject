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
        public CompanyController(ICompanyService _cService)
        {
            cService = _cService;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new CompanyRegisterViewModel
            {
                Categories = await cService.GetAllCategories()
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
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            } 
        }
        public IActionResult OfferMine()
        {
            return View();
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
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
        public IActionResult OfferEdit()
        {
            return View();
        }public IActionResult OfferRemove()
        {
            return View();
        }
    }
}
