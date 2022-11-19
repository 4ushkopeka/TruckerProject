using Tirajii.Data.Models;
using Tirajii.Models.Company;
using Tirajii.Models.Trucker;

namespace Tirajii.Services.Contracts
{
    public interface ICompanyService
    {
        Task RegisterCompany(CompanyRegisterViewModel model, string userId);
        Task RegisterTruck(TruckViewModel model, string userId);
        Task<List<CompanyCategory>> GetAllCompanyCategories();
        Task<List<TruckClass>> GetAllClasses();
        Task<List<Truck>> GetMyTrucks(string userId);
        Task<List<Offer>> GetMyOffers(string userId);
        Task<Truck> GetTruckById(int truckId);
        Task<List<TruckOffer>> GetMyTruckOffers(string userId);
        Task AddTruckOffer(TruckOfferAddViewModel model, string userId);
        Task AddOffer(OfferAddViewModel model, string UserId);
        Task<List<TruckingCategory>> GetAllOfferCategories();
    }
}
