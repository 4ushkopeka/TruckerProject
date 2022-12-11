using Tirajii.Data.Models;
using Tirajii.Models.Company;
using Tirajii.Models.Trucker;

namespace Tirajii.Services.Contracts
{
    public interface ICompanyService
    {
        Task RegisterCompany(CompanyRegisterViewModel model, string userId);

        Task EditCompany(CompanyRegisterViewModel model, string userId);

        Task RegisterTruck(TruckViewModel model, string userId);

        Task<List<CompanyCategory>> GetAllCompanyCategories();

        Task<List<TruckClass>> GetAllClasses();

        Task<List<Truck>> GetMyTrucks(string userId);

        Task<List<Truck>> GetMyTrucksForOffer(string userId);

        Task<List<Offer>> GetMyOffers(string userId);

        Task<Truck> GetTruckById(int truckId);

        Task<User> GetUserWithCompany(string userId);

        Task<List<TruckOffer>> GetMyTruckOffers(string userId);

        Task AddTruckOffer(TruckOfferAddNEditViewModel model, string userId);

        Task AddOffer(OfferAddNEditViewModel model, string UserId);

        Task<RatingViewModel> GetRating(string userId);

        Task<List<TruckingCategory>> GetAllOfferCategories();

        Task EditOffer(OfferAddNEditViewModel model, int offerId);

        Task<Offer> GetOfferById(int offerId);

        Task<TruckOffer> GetTruckOfferById(int truckOfferId);

        Task EditTruckOffer(TruckOfferAddNEditViewModel model, int truckOfferId);

        Task DeleteTruckOffer(int truckOfferId);

        Task DeleteOffer(int offerId);

        Task<List<Offer>> AllOffers();

        Task<List<TruckOffer>> AllTruckOffers();

        Task ChangeOfferVisibility(int id);

        Task ChangeTruckOfferVisibility(int id);

        CompanyRegisterViewModel SanitizeCompany(CompanyRegisterViewModel model);

        OfferAddNEditViewModel SanitizeOffer(OfferAddNEditViewModel model);

        TruckOfferAddNEditViewModel SanitizeTruckOffer(TruckOfferAddNEditViewModel model);

        TruckViewModel SanitizeTruck(TruckViewModel model);
    }
}
