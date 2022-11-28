using Microsoft.EntityFrameworkCore;
using Tirajii.Data;
using Tirajii.Data.Models;
using Tirajii.Models;
using Tirajii.Models.Company;
using Tirajii.Models.Truck;
using Tirajii.Models.Trucker;
using Tirajii.Services.Contracts;

namespace Tirajii.Services
{
    public class TruckService : ITruckService
    {
        private readonly TruckersDbContext context;
        public TruckService(TruckersDbContext context)
        {
            this.context = context;
        }

        public async Task EditTruck(TruckViewModel truck, int truckid)
        {
            var _truck = await context.Trucks.FirstAsync(x => x.Id == truckid);

            _truck.Name = truck.Name;
            _truck.RegistrationNumber = truck.RegistrationNumber;
            _truck.Colour = truck.Colour;
            _truck.Picture = truck.Picture;
            _truck.ClassId = truck.ClassId;

            await context.SaveChangesAsync();
        }

        public async Task<Truck> GetTruckByUserId(string userId)
        {
            var user = await context.Users.Include(x => x.Trucker).ThenInclude(x => x.Truck).ThenInclude(x =>x.Company).FirstAsync(x => x.Id == userId);
            return user.Trucker.Truck;
        }
        public async Task<List<TruckClass>> GetAllClasses()
        {
            return await context.TruckClasses.ToListAsync();
        }
        
        public async Task<TruckClass> GetClassById(int id)
        {
            return await context.TruckClasses.FirstAsync(x => x.Id == id);
        }

        public Task UpgradeTruck(TruckUpgradeViewModel truck, int truckid, int cost)
        {
            throw new NotImplementedException();
        }

        public TruckUpgradeViewModel GenerateUpgrades(TruckUpgradeViewModel truck)
        {
            var valuesForUpgrades = truck.Upgrades.Where(x => !x.Value).ToDictionary(x => x.Key, x => x.Value);
            var numberOfPotentialUpgrades = valuesForUpgrades.Count;
            var upgradeCount = 0;
            if (numberOfPotentialUpgrades == 1) upgradeCount = 1;
            else upgradeCount = new Random().Next(1, numberOfPotentialUpgrades);
            return UpgradeCalculations(truck, valuesForUpgrades, upgradeCount);
        }

        private TruckUpgradeViewModel UpgradeCalculations(TruckUpgradeViewModel truckStats, Dictionary<string, bool> valuesForUpgrades, int maxUpgrades)
        {
            foreach (var item in valuesForUpgrades)
            {
                if (truckStats.Upgraded[item.Key]) continue;
                var chance = new Random().Next(0, maxUpgrades+1);
                if (chance == 0) 
                { 
                    truckStats.Upgrades[item.Key] = true; 
                    maxUpgrades--;
                    truckStats.Upgraded[item.Key] = true;
                    if (item.Key == "ParkTronic") truckStats.Cost += new Random().Next(1670, 3001);
                    else if (item.Key == "Speakers") truckStats.Cost += new Random().Next(800, 1801);
                    else if (item.Key == "Bluetooth") truckStats.Cost += new Random().Next(500, 1001);
                    else if (item.Key == "CDPlayer") truckStats.Cost += new Random().Next(300, 701);
                    else if (item.Key == "Brakes") truckStats.Cost += new Random().Next(3000, 5001);
                }
                if (maxUpgrades == 0) break;
            }
            return truckStats;
        }
    }
}
