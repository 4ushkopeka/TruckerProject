using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tirajii.Data.Models;
using Tirajii.Infrastructure.Extensions;
using Tirajii.Models.Company;
using Tirajii.Models.Truck;
using Tirajii.Models.Trucker;
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
            ViewBag.Class = truckService.GetClassById(truck.ClassId).Result.Name;
            return View(truck);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var truck = await truckService.GetTruckByUserId(User.Id());
            var model = new TruckViewModel
            {
                Name = truck.Name,
                RegistrationNumber = truck.RegistrationNumber,
                ClassId = truck.ClassId,
                Picture = truck.Picture,
                Colour = truck.Colour,
                Classes = await truckService.GetAllClasses()
            };
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(TruckViewModel truck, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(truck);
            }
            if (id==0)
            {
                notyf.Error("Invalid truck Id!");
                return View(truck);
            }
            await truckService.EditTruck(truck, id);
            notyf.Success("Successfully edited your truck!");
            return RedirectToAction("General", "Truck");
        }
        
        [HttpGet]
        public async Task<IActionResult> Upgrade(int truckid)
        {
            var truck = await truckService.GetTruckByUserId(User.Id());
            var model = new TruckUpgradeViewModel
            {
                Cost = 0,
                Upgrades = new Dictionary<string, bool>()
                {
                    { "Brakes", truck.HasInstaBrakes },
                    { "Speakers", truck.HasSpeakers },
                    { "Bluetooth", truck.HasBluetooth },
                    { "CDPlayer", truck.HasCDPlayer },
                    { "ParkTronic", truck.HasParkTronic }
                },
                Upgraded = new Dictionary<string, bool>()
                {
                    { "Brakes", false },
                    { "Speakers", false },
                    { "Bluetooth", false },
                    { "CDPlayer", false },
                    { "ParkTronic", false }
                }
            };

             return View(truckService.GenerateUpgrades(model));
        }
        
        [HttpPost]
        public async Task<IActionResult> Upgrade(TruckUpgradeViewModel truck, int truckid)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Sell(int truckid)
        {
            return View();
        }
    }
}
