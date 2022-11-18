using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tirajii.Models.Trucker;
using Tirajii.Services;
using Tirajii.Services.Contracts;

namespace Tirajii.Controllers
{
    [Authorize]
    public class TruckerController : Controller
    {
        private readonly ITruckerService trService;
        public TruckerController(ITruckerService _trService)
        {
            trService = _trService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            var model = new TruckerRegisterViewModel();
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
                await trService.RegisterTrucker(model, userId);
                return RedirectToAction(nameof(Offers));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong");
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
