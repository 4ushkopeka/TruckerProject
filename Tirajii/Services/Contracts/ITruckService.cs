using Tirajii.Data.Models;

namespace Tirajii.Services.Contracts
{
    public interface ITruckService
    {
        Task<Truck> GetTruckByUserId(string userId);
    }
}
