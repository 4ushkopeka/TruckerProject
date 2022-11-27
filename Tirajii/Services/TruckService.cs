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
    }
}
