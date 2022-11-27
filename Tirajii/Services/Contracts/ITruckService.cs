using Tirajii.Data.Models;
using Tirajii.Models.Company;
using Tirajii.Models.Truck;
using Tirajii.Models.Trucker;

namespace Tirajii.Services.Contracts
{
    public interface ITruckService
    {
        Task<Truck> GetTruckByUserId(string userId);

        Task EditTruck(TruckViewModel truck, int truckid);

        Task UpgradeTruck(TruckUpgradeViewModel truck, int truckid, int cost);

        Task<List<TruckClass>> GetAllClasses();

        Task<TruckClass> GetClassById(int id);
    }
}
