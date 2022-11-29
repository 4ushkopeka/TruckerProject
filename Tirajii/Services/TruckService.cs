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

        public async Task GenerateUpgrades(TruckUpgradeViewModel truck)
        {
            var valuesForUpgrades = truck.Upgrades.Where(x => !x.Value).ToDictionary(x => x.Key, x => x.Value);
            var numberOfPotentialUpgrades = valuesForUpgrades.Count;
            var upgradeCount = 0;
            if (numberOfPotentialUpgrades == 1) upgradeCount = 1;
            else upgradeCount = new Random().Next(1, numberOfPotentialUpgrades+1);
            var upgraded = await UpgradeCalculations(truck, valuesForUpgrades, upgradeCount);
            var truckToUpgade = await context.Trucks.FirstAsync(x => x.Id == truck.TruckId);
            truckToUpgade.HasBluetooth = upgraded.Upgrades["Bluetooth"];
            truckToUpgade.HasCDPlayer = upgraded.Upgrades["CDPlayer"];
            truckToUpgade.HasInstaBrakes = upgraded.Upgrades["Brakes"];
            truckToUpgade.HasParkTronic = upgraded.Upgrades["ParkTronic"];
            truckToUpgade.HasSpeakers = upgraded.Upgrades["Speakers"];
            await context.SaveChangesAsync();   
        }

        private async Task<TruckUpgradeViewModel> UpgradeCalculations(TruckUpgradeViewModel truckStats, Dictionary<string, bool> valuesForUpgrades, int maxUpgrades)
        {
            while (maxUpgrades != 0)
            {
                foreach (var item in valuesForUpgrades)
                {
                    if (truckStats.Upgraded[item.Key]) continue;
                    var chance = new Random().Next(0, maxUpgrades + 1);
                    if (chance == 0)
                    {
                        truckStats.Upgrades[item.Key] = true;
                        maxUpgrades--;
                        truckStats.Upgraded[item.Key] = true;
                    }
                    if (maxUpgrades == 0) break;
                }
            }
            
            return truckStats;
        }

        public async Task PayUpgrades(string userId)
        {
            var wallet = await context.Wallets.FirstAsync(x => x.UserId == userId);
            if (wallet.Balance < 6000) throw new InvalidOperationException("Insufficient funds!");
            wallet.Balance -= 6000;
            await context.SaveChangesAsync();
        }

        public async Task<int> SellTruck(int truckid, string userId)
        {
            var truck = await context.Trucks.FirstAsync(x => x.Id == truckid);
            var offer = await context.TruckOffers.FirstAsync(x => x.TruckId == truckid);
            var user = await context.Users.Include(x => x.Wallet).FirstAsync(x => x.Id == userId);
            var money = 0;
            if (truck.HasCDPlayer) money += new Random().Next(300, 601);
            if (truck.HasInstaBrakes) money += new Random().Next(1800, 3501);
            if (truck.HasBluetooth) money += new Random().Next(700, 1251);
            if (truck.HasParkTronic) money += new Random().Next(1500, 2601);
            if (truck.HasSpeakers) money += new Random().Next(1000, 1781);
            user.Wallet.Balance += money+250;
            truck.OwnerId = null;
            truck.Owner = null;
            truck.CompanyId = 0;
            truck.TruckOffer = null;
            truck.ClassId = 0;
            offer.TruckId = 0;
            offer.Truck = null;
            context.TruckOffers.Remove(offer);
            context.Trucks.Remove(truck);
            await context.SaveChangesAsync();
            return money+250;
        }
    }
}
