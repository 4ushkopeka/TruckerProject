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
    [Authorize(Roles ="Trucker")]
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
            if (truck is null)
            {
                notyf.Error("You do not yet own a truck!");
                return RedirectToAction("Index","Home");
            }
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
        
        [HttpPost]
        public async Task<IActionResult> PayRetry()
        {
            try
            {
                await truckService.PayUpgrades(User.Id());
                notyf.Success("Successful transaction!");
                return RedirectToAction("Upgrade", "Truck");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("General", "Truck");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Upgrade()
        {
            try
            {
                await truckService.PayUpgrades(User.Id());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notyf.Error(ex.Message);
                return RedirectToAction("General", "Truck");
            }
            var truck = await truckService.GetTruckByUserId(User.Id());
            var model = new TruckUpgradeViewModel()
            {
                Upgrades = new Dictionary<string, bool>()
                {
                    { "Brakes", truck.HasInstaBrakes },
                    { "Bluetooth", truck.HasBluetooth },
                    { "Speakers", truck.HasSpeakers },
                    { "CDPlayer", truck.HasCDPlayer },
                    { "ParkTronic", truck.HasParkTronic }
                },
                Upgraded = new Dictionary<string, bool>()
                {
                    { "Brakes", false },
                    { "Bluetooth", false },
                    { "Speakers", false },
                    { "CDPlayer", false },
                    { "ParkTronic", false }
                },
                TruckId = truck.Id
            };
            await truckService.GenerateUpgrades(model);
            return RedirectToAction("General", "Truck");
        }

        [HttpPost]
        public async Task<IActionResult> Sell()
        {
            var truck = await truckService.GetTruckByUserId(User.Id());
            var gains = await truckService.SellTruck(truck.Id, User.Id());
            notyf.Success($"You successfully sold your truck for {gains:f2}$!");
            return RedirectToAction("Index","Home");
        }
    }
}
