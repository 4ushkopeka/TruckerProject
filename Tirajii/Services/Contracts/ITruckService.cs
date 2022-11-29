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

        Task GenerateUpgrades(TruckUpgradeViewModel truck);

        Task PayUpgrades(string userId);

        Task<int> SellTruck(int truckid, string userId);

        Task<List<TruckClass>> GetAllClasses();

        Task<TruckClass> GetClassById(int id);
    }
}
