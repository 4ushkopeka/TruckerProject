using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tirajii.Infrastructure.Extensions;
using Tirajii.Services.Contracts;

namespace Tirajii.Areas.Administration.Controllers
{
    public class OfferAdministrationController : AdminController
    {
        private readonly ICompanyService companyService;
        public OfferAdministrationController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> OffersAll()
        {
            var offers = await companyService.AllOffers();
            return View(offers);
        }
        
        [HttpGet]
        public async Task<IActionResult> TruckOffersAll()
        {
            var offers = await companyService.AllTruckOffers();
            return View(offers);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeOfferVisibility(int id)
        {
            await companyService.ChangeOfferVisibility(id);
            return RedirectToAction(nameof(OffersAll));
        }
        
        [HttpGet]
        public async Task<IActionResult> ChangeTruckOfferVisibility(int id)
        {
            await companyService.ChangeTruckOfferVisibility(id);
            return RedirectToAction(nameof(TruckOffersAll));
        }
    }
}
