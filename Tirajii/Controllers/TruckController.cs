using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tirajii.Infrastructure.Extensions;
using Tirajii.Services.Contracts;

namespace Tirajii.Controllers
{
    [Authorize]
    public class TruckController : Controller
    {
        private readonly ITruckService truckService;
        private readonly INotyfService notyf;

        public TruckController(ITruckService truckService, INotyfService notyf)
        {
            this.truckService = truckService;
            this.notyf = notyf;
        }

        [HttpGet]
        public async Task<IActionResult> General()
        {
            var truck = await truckService.GetTruckByUserId(User.Id());
            return View(truck);
        }

        [HttpPost]
        public async Task<IActionResult> Upgrade()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Sell()
        {
            return View();
        }
    }
}
